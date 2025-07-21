/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Utils;
using Utils.Threading;
using ZWave.Enums;
using ZWave.Layers.Frame;

namespace ZWave.Layers.Session
{
    public class SessionClient : ISessionClient
    {
        // interface
        public ushort SessionId { get; set; }
        public bool IsHandleFrameEnabled { get; set; }
        public Func<ActionHandlerResult, bool> SendFramesCallback { get; set; }
        public Func<ActionBase, ActionBase> PostSubstituteAction { get; set; }
        public Action<ActionBase> ActionStartedCallback { get; set; }
        public Action<ActionBase> ActionCompletedCallback { get; set; }

        // public
        public bool SuppressDebugOutput { get; set; }
        public string LogEntryPointClass { get; set; }
        public ConcurrentDictionary<int, ActionBase> RunningActions { get; set; }
        public ConcurrentQueue<ActionBase> PendingExclusiveActions { get; set; }
        private ITimeoutManager TimeoutManager { get; set; }

        // private
        private byte _funcIdCounter = 0;
        private readonly Action<ActionBase> _actionChangeCallback;
        private readonly IEnumerable<SubstituteIncomingFlags> _substituteManagersOrder = new[]
        {
            SubstituteIncomingFlags.Settings,
            SubstituteIncomingFlags.Supervision,
            SubstituteIncomingFlags.MultiChannel,
            SubstituteIncomingFlags.Crc16Encap,
            SubstituteIncomingFlags.Security,
            SubstituteIncomingFlags.TransportService,
        };

        private ConcurrentDictionary<SubstituteIncomingFlags, ISubstituteManager> _substituteManagersDictionary = new ConcurrentDictionary<SubstituteIncomingFlags, ISubstituteManager>();
        private int exclusiveState;
        private IConsumerThread<IActionCase> _handleCases;

        public SessionClient(Action<ActionBase> actionChangeCallback)
        {
            _actionChangeCallback = action =>
            {
                if (action.Token.IsStateActive)
                {
                    ActionStartedCallback?.Invoke(action);
                }
                else if (action.IsStateCompleted)
                {
                    ActionCompletedCallback?.Invoke(action);
                }
                actionChangeCallback?.Invoke(action);
            };
            IsHandleFrameEnabled = true;
            RunningActions = new ConcurrentDictionary<int, ActionBase>();
            PendingExclusiveActions = new ConcurrentQueue<ActionBase>();
        }

        bool _componentsInitialized = false;
        public void RunComponents(ITimeoutManager timeoutManager, IConsumerThread<IActionCase> handleCases)
        {
            if (!_componentsInitialized)
            {
                TimeoutManager = timeoutManager;
                TimeoutManager.Start(this);
                _handleCases = handleCases;
                _handleCases.Start(SessionId, "HandleActionCaseInner thread", HandleActionCaseInner);
                _componentsInitialized = true;
            }
        }

        public void RunComponentsDefault()
        {
            RunComponents(new TimeoutManager(), new ConsumerThread<IActionCase>());
        }

        public void AddSubstituteManager(ISubstituteManager sm, params ActionBase[] actions)
        {
            _substituteManagersDictionary.AddOrUpdate(sm.Id, sm, (x, y) => sm);
            if (actions != null)
            {
                foreach (var item in actions)
                {
                    ExecuteAsync(item);
                    sm.AddRunningActionToken(item.Token);
                }
            }
        }

        public void Cancel(Type actionType)
        {
            $"{SessionId:X2} Cancel item {actionType.ToString()} handling thread {Thread.CurrentThread.ManagedThreadId:000}"._DLOG();
            var cancelledPendingActions = PendingExclusiveActions.Where(a => a.GetType() == actionType)?.ToArray();
            if (cancelledPendingActions != null)
            {
                foreach (var action in cancelledPendingActions)
                {
                    action.SetCancelling();
                    action.IsExclusive = false;
                    _handleCases.Add(action);
                }
            }

            var cancelledActions = RunningActions.Where(a => a.Value.GetType() == actionType)?.ToArray();
            if (cancelledActions != null)
            {
                foreach (var action in cancelledActions)
                {
                    action.Value.SetCancelling();
                    action.Value.IsExclusive = false;
                    _handleCases.Add(action.Value);
                }
            }
        }

        public void Cancel(ActionToken actionToken)
        {
            if (actionToken != null)
            {
                $"{SessionId:X2} Cancel item {actionToken.ToString()}"._DLOG();
                if (actionToken != null && !actionToken.IsStateFinished)
                {
                    if (RunningActions.TryGetValue(actionToken.ActionId, out ActionBase action))
                    {
                        action.SetCancelling();
                        _handleCases.Add(action);
                    }
                    else
                    {
                        actionToken.SetCancelled();
                        actionToken.SetCompletedSignal();
                    }
                }
            }
        }

        public void ClearSubstituteManagers()
        {
            foreach (var item in _substituteManagersDictionary)
            {
                var runningTokens = item.Value.GetRunningActionTokens();
                if (runningTokens != null)
                {
                    foreach (var token in runningTokens)
                    {
                        Cancel(token);
                    }
                }
            };
            _substituteManagersDictionary.Clear();
        }

        public ActionToken ExecuteAsync(IActionItem actionItem)
        {
            ActionToken ret = null;
            var action = actionItem as ActionBase;
            if (action != null)
            {
                ret = action.Token;
                $"{SessionId:X2} ExecuteAsync item {actionItem.ToString()}"._DLOG();
                _handleCases.Add(action);
            }
            return ret;
        }

        public ISubstituteManager GetSubstituteManager(Type type)
        {
            ISubstituteManager ret = null;
            foreach (var item in _substituteManagersDictionary)
            {
                if (item.Value.GetType() == type)
                {
                    ret = item.Value;
                    break;
                }
            }
            return ret;
        }

        public void ResetSubstituteManagers()
        {
            foreach (var item in _substituteManagersDictionary)
            {
                item.Value.SetDefault();
            }
        }

        public void HandleActionCase(IActionCase actionCase)
        {
            _handleCases.Add(actionCase);
        }

        public void HandleActionCaseInner(IActionCase actionCase)
        {
            if (actionCase as ActionBase != null)
            {
                var action = actionCase as ActionBase;
                if (action.Token.State == ActionStates.None && !(action.ParentAction != null && action.ParentAction.Token.IsStateFinished))
                {
                    if (action.ParentAction != null && action.DataDelay > 0)
                    {
                        var id = action.ParentAction.GetNextCounter();
                        var timeInterval = new TimeInterval(id, action.ParentAction.Id, action.DataDelay);
                        action.DataDelay = 0;
                        action.ParentAction.ActionUnits.Add(new TimeElapsedUnit(timeInterval, null, 0, action));
                        $"{SessionId:X2} Handle add time interval (data delay) actionId:{timeInterval.ActionId} id:{timeInterval.Id} duration:{timeInterval.TimeoutMs} ms"._DLOG();
                        TimeoutManager.AddTimer(timeInterval);
                    }
                    else
                    {
                        IActionUnit ou = null;
                        lock (this)
                        {
                            if (action.Token.State == ActionStates.None && !(action.ParentAction != null && action.ParentAction.Token.IsStateFinished))
                            {
                                action = SubstituteAction(action);
                                action.Token.LogEntryPointClassLineNumber = GetLineNumberInTheEntryPointClass(LogEntryPointClass);
                                action.SessionId = SessionId;
                                if (action.IsSequenceNumberRequired)
                                {
                                    action.SequenceNumber = NextFuncId();
                                    if (action.IsExtraSequenceNumberRequired)
                                    {
                                        action.ExtraSequenceNumber = NextFuncId();
                                    }
                                }
                                if (action.IsExclusive && Interlocked.CompareExchange(ref exclusiveState, 1, 0) == 1)
                                {
                                    if (!SuppressDebugOutput)
                                    {
                                        $"{SessionId:X2} (Wait Exlusive) HandleActionCaseInner action {action.GetName() + AboutActionSafe(action)}"._DLOG();
                                    }
                                    PendingExclusiveActions.Enqueue(action);
                                }
                                else
                                {
                                    ActionHandlerResult ahResult = null;
                                    action.Token.Name = action.Name;
                                    action.Initialize();
                                    action.SetRunning();
                                    _actionChangeCallback?.Invoke(action);

                                    RunningActions.AddOrUpdate(action.Id, action, (x, y) =>
                                    {
                                        return action;
                                    });
                                    if (!SuppressDebugOutput)
                                    {
                                        $"{SessionId:X2} {(action.IsExclusive ? "(Exclusive) " : "")}HandleActionCaseInner action {action.GetName() + AboutActionSafe(action)}"._DLOG();
                                    }
                                    ou = action.ActionUnits?.FirstOrDefault(x => x is StartActionUnit);
                                }
                            }
                        }
                        if (ou != null)
                        {
                            Handle(action, ou);
                        }
                    }
                }
                else if (action.Token.State == ActionStates.Expiring || action.Token.State == ActionStates.Cancelling)
                {
                    Handle(action, action.StopActionUnit);
                }
                else if (action.Token.State == ActionStates.Running)
                {
                    // case when one action completed and cancel other action of the parent action
                    var childrenCancelledActions = RunningActions.Where(x => x.Value.ParentAction != null && x.Value.ParentAction.Id == action.Id && x.Value.Token.State == ActionStates.Cancelled)?.ToArray();
                    if (childrenCancelledActions != null)
                    {
                        foreach (var childAction in childrenCancelledActions)
                        {
                            childAction.Value.Token.SetCompletedSignal();
                            HandleActionCaseInner(childAction.Value);
                        }
                    }
                }

                if (action.Token.IsStateFinished)
                {
                    if (RunningActions.TryRemove(action.Id, out ActionBase removingAction))
                    {
                        if (!SuppressDebugOutput)
                        {
                            $"{SessionId:X2} HandleActionCaseInner action {action.GetName() + AboutActionSafe(action)}"._DLOG();
                        }
                        TimeoutManager.ClearTimers(action.Id);
                        removingAction.Token.Name = removingAction.Name + ": " + AboutActionSafe(removingAction);
                        _actionChangeCallback?.Invoke(removingAction);
                        removingAction.Token.SetCompletedSignal();
                        if (action.IsExclusive)
                        {
                            Interlocked.Exchange(ref exclusiveState, 0);
                            if (PendingExclusiveActions.TryDequeue(out ActionBase pendingAction))
                            {
                                if (!SuppressDebugOutput)
                                {
                                    $"{SessionId:X2} (Dequeue Exclusive After) HandleActionCaseInner action {action.GetName() + AboutActionSafe(action)}"._DLOG();
                                }
                                _handleCases.Add(pendingAction);
                            }
                        }

                        ThreadPool.QueueUserWorkItem(new WaitCallback(x => removingAction.CompletedCallback?.Invoke(removingAction)));

                        var childrenActions = RunningActions.Where(x => x.Value.ParentAction != null && x.Value.ParentAction.Id == action.Id)?.ToArray();
                        if (childrenActions != null)
                        {
                            foreach (var childAction in childrenActions)
                            {
                                childAction.Value.SetCancelling();
                                HandleActionCaseInner(childAction.Value);
                            }
                        }

                        if (action.ParentAction != null)
                        {
                            if (RunningActions.TryGetValue(action.ParentAction.Id, out ActionBase parentAction))
                            {
                                if (action.ParentAction.Token.IsStateActive)
                                {
                                    Handle(action.ParentAction, action.ParentAction.ActionUnits?.FirstOrDefault(x => x.TryHandle(action)));
                                }
                            }
                        }
                    }
                    else if (action.Token.State == ActionStates.Cancelled && action.ParentAction != null)
                    {
                        // case when action of the action group cancelled. need to proceed to next 
                        Handle(action.ParentAction, action.ParentAction.ActionUnits?.FirstOrDefault(x => x.TryHandle(action)));
                    }
                }
            }

            else if (actionCase as TimeInterval != null)
            {
                var timeInterval = actionCase as TimeInterval;
                if (!SuppressDebugOutput)
                {
                    $"{SessionId:X2} HandleActionCaseInner timeout {timeInterval.Id} of action {timeInterval.ParentAction?.GetName()} actionId={timeInterval.ActionId} untill {timeInterval.ExpireDateTime:mm:ss.fff}"._DLOG();
                }
                if (RunningActions.TryGetValue(timeInterval.ActionId, out ActionBase expiringAction))
                {
                    if (!expiringAction.Token.IsStateFinished)
                    {
                        if (timeInterval.Id == 0)
                        {
                            expiringAction.SetExpiring();
                            HandleActionCaseInner(expiringAction);
                        }
                        else
                        {
                            var ou = expiringAction.ActionUnits?.FirstOrDefault(x => x.TryHandle(timeInterval));
                            Handle(expiringAction, ou);
                        }
                    }
                }
            }

            else if (actionCase as CustomDataFrame != null)
            {
                var dataFrame = actionCase as CustomDataFrame;
                if (!dataFrame.IsOutcome && !dataFrame.IsHandled)
                {
                    if (IsHandleFrameEnabled)
                    {
                        Dictionary<SubstituteIncomingFlags, CustomDataFrame> substitutedDataFrames = new Dictionary<SubstituteIncomingFlags, CustomDataFrame>();
                        dataFrame = SubstituteIncoming(dataFrame, substitutedDataFrames);
                        if (dataFrame != null)
                        {
                            var activeActions = RunningActions.Where(x => !x.Value.Token.IsSuspended)?.OrderByDescending(x => x.Value.IsFirstPriority).ToArray();

                            if (activeActions != null)
                            {
                                foreach (var item in activeActions)
                                {
                                    if (!dataFrame.IsHandled)
                                    {
                                        if (item.Value.Token.IsStateActive)
                                        {
                                            var actionUnit = item.Value.ActionUnits?.FirstOrDefault(x => x.TryHandle(dataFrame));
                                            if (actionUnit != null)
                                            {
                                                if (!SuppressDebugOutput)
                                                {
                                                    $"{SessionId:X2} HandleActionCaseInner action {item.Value.GetName()} dataFrame {dataFrame.Data?.GetHex()}"._DLOG();
                                                }
                                                Handle(item.Value, actionUnit);
                                            }
                                            if (item.Value.Token.IsStateActive)
                                            {
                                                if (dataFrame.Parent != null)
                                                {
                                                    actionUnit = item.Value.ActionUnits.FirstOrDefault(x => x.TryHandle(dataFrame.Parent));
                                                    if (actionUnit != null)
                                                    {
                                                        if (!SuppressDebugOutput)
                                                        {
                                                            $"{SessionId:X2} HandleActionCaseInner action {item.Value.GetName()} dataFrame.Parent {dataFrame.Parent.Data?.GetHex()}"._DLOG();
                                                        }
                                                        Handle(item.Value, actionUnit);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                // 32 bits in int
                                for (int i = 31; i >= 0; i--)
                                {
                                    int mask = 1 << i;
                                    if ((dataFrame.SubstituteIncomingFlags & (SubstituteIncomingFlags)mask) > 0)
                                    {
                                        if (_substituteManagersDictionary.TryGetValue((SubstituteIncomingFlags)mask, out ISubstituteManager smgr))
                                        {
                                            var dataFrameOri = substitutedDataFrames[(SubstituteIncomingFlags)mask];
                                            if (!smgr.OnIncomingSubstituted(dataFrameOri, dataFrame, new List<ActionHandlerResult>(), out ActionBase additionalAction))
                                            {
                                                foreach (var item in activeActions)
                                                {
                                                    if (!dataFrameOri.IsHandled)
                                                    {
                                                        if (item.Value.Token.IsStateActive)
                                                        {
                                                            Handle(item.Value, item.Value.ActionUnits?.FirstOrDefault(x => x.TryHandle(dataFrameOri)));
                                                            if (dataFrameOri.Parent != null)
                                                            {
                                                                Handle(item.Value, item.Value.ActionUnits?.FirstOrDefault(x => x.TryHandle(dataFrameOri.Parent)));
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            if (additionalAction != null)
                                            {
                                                HandleActionCaseInner(additionalAction);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void Handle(ActionBase action, IActionUnit actionUnit)
        {
            if (actionUnit != null)
            {
                IActionUnit ou = actionUnit;
                if (actionUnit is DataReceivedUnit)
                {
                    DataReceivedUnit dou = (DataReceivedUnit)actionUnit;
                    if (dou.DataFrame != null && dou.DataFrame.Data != null)
                        action.AddTraceLogItem(DateTime.Now, dou.DataFrame.Data, false);
                }
                try
                {
                    ou.Func?.Invoke(ou);
                }
                catch (Exception ex)
                {
                    if (ex is IndexOutOfRangeException || ex is NullReferenceException)
                    {
                        $"{action.ToString()} thrown {ex.Message}"._DLOG();
                        action.Token.LogEntryPointException = $"{ex.GetType().Name}: {ex.Message}";
                        action.SetFailed();
                    }
                    else
                    {
                        throw;
                    }
                }

                var ahResult = new ActionHandlerResult(action);
                ou.SetParentAction(action);
                ou.CopyActionsItemsTo(ahResult.NextActions);

                if (ahResult != null && ahResult.NextActions != null)
                {
                    var sendFrames = ahResult.NextActions.Where(x => x is CommandMessage);
                    var timeIntervals = ahResult.NextActions.Where(x => x is TimeInterval);
                    var actions = ahResult.NextActions.Where(x => x is ActionBase);
                    if (sendFrames.Any())
                    {
                        if (SendFramesCallback != null)
                        {
                            var isTransmitted = SendFramesCallback(ahResult);

                            if (!isTransmitted)
                            {
                                ahResult.Parent.SetFailed();
                            }
                        }
                    }

                    if (timeIntervals.Any())
                    {
                        foreach (TimeInterval item in timeIntervals)
                        {
                            TimeoutManager.AddTimer(item);
                            $"{SessionId:X2} Handle add time interval actionId:{item.ActionId} id:{item.Id} duration:{item.TimeoutMs} ms"._DLOG();
                        }
                    }

                    if (actions.Any())
                    {
                        foreach (ActionBase item in actions)
                        {
                            HandleActionCaseInner(item);
                        }
                    }
                }
                if (ou.TimeoutMs > 0)
                {
                    var ti = new TimeInterval(0, action.Id, ou.TimeoutMs);
                    TimeoutManager.AddTimer(ti);
                    if (!SuppressDebugOutput)
                    {
                        $"{SessionId:X2} Handle add action time interval actionId:{action.Id} duration:{ou.TimeoutMs} ms"._DLOG();
                    }
                }
            }

            if (action.Token.State == ActionStates.Expiring)
                action.SetExpired();
            else if (action.Token.State == ActionStates.Cancelling)
                action.SetCancelled();
            else if (action.Token.State == ActionStates.Completing)
                action.SetCompleted();
            else if (action.Token.State == ActionStates.Failing)
                action.SetFailed();
            HandleActionCaseInner(action);
        }

        public void SetFuncId(byte value)
        {
            _funcIdCounter = value;
        }

        private byte NextFuncId()
        {
            _funcIdCounter++;
            while (_funcIdCounter == 0 || _funcIdCounter > 126)
                _funcIdCounter++;
            return _funcIdCounter;
        }

        internal Action<ushort> ReleaseSessionIdCallback;
        private void ReleaseSessionId(ushort sessionId)
        {
            ReleaseSessionIdCallback?.Invoke(sessionId);
        }

        public static int GetLineNumberInTheEntryPointClass(string logEntryPointClass)
        {
            int ret = -1;
            try
            {
                if (!string.IsNullOrEmpty(logEntryPointClass))
                {
                    StackTrace st = new StackTrace(true);
                    StackFrame sfTC = null;
                    StackFrame sfMTC = null;
                    for (int i = 4; i < st.FrameCount; i++)
                    {
                        StackFrame sf = st.GetFrame(i);
                        string fileName = sf.GetFileName();
                        if (fileName != null && fileName.Contains(logEntryPointClass))
                        {
                            sfTC = st.GetFrame(i - 1);
                            sfMTC = st.GetFrame(i - 2);
                            break;
                        }
                    }
                    if (sfTC != null)
                    {
                        ret = sfTC.GetFileLineNumber();
                    }
                }
            }
            catch { }
            return ret;
        }

        public string AboutActionSafe(ActionBase action)
        {
            string ret;
            try
            {
                ret = action.AboutMe();
            }
            catch
            {
                ret = "### not valid";
            }
            return ret;
        }

        private ActionBase SubstituteAction(ActionBase action)
        {
            var comparer = new OrderComparer<SubstituteIncomingFlags>(_substituteManagersOrder);
            foreach (var item in _substituteManagersDictionary.ToArray().OrderBy(x => x.Key, comparer))
            {
                var tmp = item.Value.SubstituteAction(action);
                if (tmp != null)
                {
                    action = tmp;
                    if (action.IsSequenceNumberRequired)
                    {
                        action.SequenceNumber = NextFuncId();
                        if (action.IsExtraSequenceNumberRequired)
                        {
                            action.ExtraSequenceNumber = NextFuncId();
                        }
                    }
                }
            }

            if (PostSubstituteAction != null)
            {
                var tmp = PostSubstituteAction(action);
                if (tmp != null)
                {
                    action = tmp;
                    if (action.IsSequenceNumberRequired)
                    {
                        action.SequenceNumber = NextFuncId();
                        if (action.IsExtraSequenceNumberRequired)
                        {
                            action.ExtraSequenceNumber = NextFuncId();
                        }
                    }
                }
            }
            return action;
        }


        private CustomDataFrame SubstituteIncoming(CustomDataFrame dataFrame, Dictionary<SubstituteIncomingFlags, CustomDataFrame> substitutedDataFrames)
        {
            var comparer = new OrderComparer<SubstituteIncomingFlags>(_substituteManagersOrder, true);
            foreach (var item in _substituteManagersDictionary.ToArray().OrderBy(x => x.Key, comparer))
            {
                substitutedDataFrames.Add(item.Value.Id, dataFrame);
                dataFrame = item.Value.SubstituteIncoming(dataFrame, out ActionBase additionalAction, out ActionBase completeAction);
                if (additionalAction != null)
                {
                    HandleActionCaseInner(additionalAction);
                }
                if (completeAction != null)
                {
                    HandleActionCaseInner(completeAction);
                }
            }
            return dataFrame;
        }


        #region IDisposable Members
        private bool disposedValue = false;
        private volatile bool isDisposing;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    isDisposing = true;
                    TimeoutManager.Dispose();
                    _handleCases.Dispose();
                    foreach (var item in RunningActions)
                    {
                        item.Value.SetCancelled();
                        item.Value.Token.SetCompletedSignal();
                    }
                    ReleaseSessionId(SessionId);
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
