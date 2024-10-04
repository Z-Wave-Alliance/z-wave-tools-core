/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using Utils;
using Utils.Threading;
using ZWave.Enums;

namespace ZWave
{
    public abstract class ActionBase : IActionCase, IActionItem
    {
        public bool IsTraceLogDisabled { get; set; }
        public bool IsHandled { get; set; }
        public ushort SessionId { get; set; }
        public int Id { get; set; }
        public bool IsFirstPriority { get; set; }
        public bool IsExclusive { get; set; }
        public int DataDelay { get; set; }

        public ActionToken Token { get; set; }
        public Action<IActionItem> CompletedCallback { get; set; }
        private ActionBase _parentAction;

        public ActionBase ParentAction
        {
            get { return _parentAction; }
            set
            {
                _parentAction = value;
                Token.IsChildAction = _parentAction != null;
            }
        }

        private int _counter = 1;
        public byte SequenceNumber { get; set; }
        public byte ExtraSequenceNumber { get; set; }
        public bool IsSequenceNumberRequired { get; set; }
        public bool IsExtraSequenceNumberRequired { get; set; }
        public StopActionUnit StopActionUnit { get; set; }
        public List<IActionUnit> ActionUnits { get; set; }

        public bool IsStateCompleted
        {
            get { return Token.Result.State == ActionStates.Completed; }
        }

        public ActionResult Result
        {
            get { return Token.Result; }
        }

        public ActionBase(bool isExclusive)
        {
            IsExclusive = isExclusive;
            Id = NextId();
            ActionUnits = new List<IActionUnit>();
            Token = new ActionToken(GetType(), Id, CreateOperationResult());
        }

        public int GetNextCounter()
        {
            return ++_counter;
        }

        public virtual void NewToken(bool isNextId)
        {
            if (isNextId)
            {
                var newId = NextId();
                "{0}->{1}"._DLOG(Id, newId);
                Id = newId;
            }
            Token = new ActionToken(GetType(), Id, CreateOperationResult());
            Token.Name = Name;
            ActionUnits.Clear();
            CompletedCallback = null;
        }

        public virtual void NewToken()
        {
            NewToken(false);
        }

        public virtual string AboutMe()
        {
            return string.Empty;
        }

        internal void Initialize()
        {
            if (!IsTraceLogDisabled && Result.TraceLog == null)
            {
                Result.TraceLog = new ConcurrentList<TraceLogItem>(100);
            }
            CreateInstance();
            CreateWorkflow();
        }

        protected abstract void CreateWorkflow();
        protected abstract void CreateInstance();
        protected virtual ActionResult CreateOperationResult()
        {
            return new ActionResult();
        }

        private static int _IdCounter;

        internal static void ResetSharedIdCounter()
        {
            _IdCounter = 0;
        }

        private int NextId()
        {
            return Interlocked.Increment(ref _IdCounter);
        }

        public void SetCompleted()
        {
            Result.State = ActionStates.Completed;
            Result.StopTimestamp = DateTime.Now;
        }

        public void SetFailed()
        {
            Result.State = ActionStates.Failed;
            Result.StopTimestamp = DateTime.Now;
        }

        public void SetCancelled()
        {
            Result.State = ActionStates.Cancelled;
            Result.StopTimestamp = DateTime.Now;
        }

        public void SetExpired()
        {
            Result.State = ActionStates.Expired;
            Result.StopTimestamp = DateTime.Now;
        }

        internal void SetExpiring()
        {
            Result.State = ActionStates.Expiring;
        }

        internal void SetCompleting()
        {
            Result.State = ActionStates.Completing;
        }

        internal void SetFailing()
        {
            Result.State = ActionStates.Failing;
        }

        internal void SetRunning()
        {
            Result.State = ActionStates.Running;
            Result.StartTimestamp = DateTime.Now;
        }

        internal void SetCancelling()
        {
            Result.State = ActionStates.Cancelling;
        }

        protected virtual void SetStateCompleted(IActionUnit ou)
        {
            SetCompleted();
        }

        protected virtual void SetStateFailed(IActionUnit ou)
        {
            SetFailed();
        }

        protected virtual void SetStateCancelled(IActionUnit ou)
        {
            SetCancelled();
        }

        protected virtual void SetStateExpired(IActionUnit ou)
        {
            SetExpired();
        }

        protected virtual void SetStateCompleting(IActionUnit ou)
        {
            SetCompleting();
        }

        protected virtual void SetStateFailing(IActionUnit ou)
        {
            SetFailing();
        }

        private string _typeName = null;
        public string Name
        {
            get
            {
                if (_typeName == null)
                {
                    StringBuilder sbType = new StringBuilder();
                    string type = GetType().Name;
                    if (type.EndsWith("Operation"))
                    {
                        type = type.Replace("Operation", "");
                    }
                    sbType.Append(type);
                    ActionBase tmp = ParentAction;
                    while (tmp != null)
                    {
                        type = tmp.GetType().Name;
                        if (type.EndsWith("Operation"))
                        {
                            type = type.Replace("Operation", "");
                        }
                        sbType.AppendFormat(".{0}", type);
                        tmp = tmp.ParentAction;
                    }
                    _typeName = sbType.ToString();
                }
                return _typeName;
            }
            set
            {
                _typeName = value;
            }
        }

        private string GetId(ActionBase action)
        {
            string ret = action.Id.ToString();
            if (action.ParentAction != null)
            {
                ret += "." + GetId(action.ParentAction);
            }
            return ret;
        }

        public string GetName()
        {
            return $"{Token.Result.State} {Name} (Id={GetId(this)}) ";
        }

        public void AddTraceLogItem(DateTime dateTime, byte[] data, bool isOutcome)
        {
            Result.AddTraceLogItem(dateTime, data, isOutcome);
        }

        public void AddTraceLogItems(ConcurrentList<TraceLogItem> traceLog)
        {
            if (Result.TraceLog != null && traceLog != null)
            {
                // Added To Array to prevent modification of traceLog in foreach loop itself.
                foreach (var item in traceLog.ToArray())
                {
                    Result.TraceLog.Add(item);
                }
            }
        }

        public override string ToString()
        {
            return Tools.FormatStr("{0}={1}({2})", GetType().Name, Token.Result.State, Token.Result.RetryCount);
        }
    }

    public class SubstituteSettings
    {
        public SubstituteFlags SubstituteFlags { get; set; }
        public int S0MaxBytesPerFrameSize { get; set; }
        public bool IsSkipWaitingSendCallbackEnabled { get; set; }
        public int CallbackWaitTimeoutMs { get; set; }
        public bool IsBitAdress { get; set; }
        public byte SrcEndPoint { get; set; }
        public byte DstEndPoint { get; set; }
        public SubstituteSettings() { }

        public SubstituteSettings(SubstituteFlags substituteFlags, int s0MaxBytesPerFrameSize)
        {
            SubstituteFlags = substituteFlags;
            S0MaxBytesPerFrameSize = s0MaxBytesPerFrameSize;
        }

        public bool HasFlag(SubstituteFlags flag)
        {
            return (SubstituteFlags & flag) == flag;
        }

        public void SetFlag(SubstituteFlags flag)
        {
            SubstituteFlags |= flag;
        }

        public void ClearFlag(SubstituteFlags flag)
        {
            SubstituteFlags &= ~flag;
        }
    }
}
