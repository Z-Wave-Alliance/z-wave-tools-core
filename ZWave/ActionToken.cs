/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Text;
using Utils.Threading;
using System.Diagnostics;
using System.Threading;
using Utils;

namespace ZWave
{
    public class CustomToken : IActionCase
    {
        public bool IsHandled { get; set; }
        protected int _waitCompletedSignalReceived;
        protected SignalSlim _completedSignal;
        public int ActionId { get; set; }
        public ActionResult Result { get; set; }
        public CustomToken()
        {
            _completedSignal = new SignalSlim(false);
        }

        public void Close()
        {
            _completedSignal?.Close();
        }

        [DebuggerHidden]
        public ActionResult WaitCompletedSignal()
        {
            if (Interlocked.Exchange(ref _waitCompletedSignalReceived, 1) == 0)
            {
                _completedSignal.WaitOne();
                //bool res = _completedSignal.WaitOne(20000);
                //if (!res)
                //{
                //    $"token not signaled {ActionId}"._DLOG();
                //}
            }
            return Result;
        }

        [DebuggerHidden]
        public void SetCompletedSignal()
        {
            _completedSignal.Set();
        }
    }

    public class SimpleToken : CustomToken
    {
        int _timeoutMs;
        public SimpleToken(int timeoutMs)
        {
            _timeoutMs = timeoutMs;
        }

        [DebuggerHidden]
        public ActionResult WaitCompletedSignal()
        {
            if (Interlocked.Exchange(ref _waitCompletedSignalReceived, 1) == 0)
            {
                _completedSignal.WaitOne(_timeoutMs);
            }
            return Result;
        }
    }

    public class ActionToken : CustomToken
    {
        private bool _isSuspended;
        public bool IsSuspended => _isSuspended;

        public static int DefaultTimeout = 5 * 60000;
        public static bool ThrowExceptionOnDefaultTimeoutExpired = false;
        public int LogEntryPointClassLineNumber { get; set; }
        public string LogEntryPointSource { get; set; }
        public string LogEntryPointCategory { get; set; }
        public string LogEntryPointException { get; set; }
        public string Name { get; set; }
        public Type ParentType { get; set; }
        public bool IsChildAction { get; set; }
        public object CustomObject { get; set; }
        public Action<ActionToken> HandleMe { get; set; }


        internal bool IsStateFinished
        {
            get
            {
                return State == ActionStates.Cancelled ||
                    State == ActionStates.Completed ||
                    State == ActionStates.Expired ||
                    State == ActionStates.Failed;
            }
        }

        public ActionToken(Type actionType, int actionId, ActionResult result)
        {
            ParentType = actionType;
            ActionId = actionId;
            Result = result;
            Result.StartTimestamp = DateTime.Now;
        }

        public ActionStates State
        {
            get { return Result.State; }
        }

        public bool IsStateActive
        {
            get
            {
                return Result.State != ActionStates.Completed &&
                    Result.State != ActionStates.Expired &&
                    Result.State != ActionStates.Failed &&
                    Result.State != ActionStates.Cancelled;
            }
        }

        public void Complete()
        {
            SetCompleted();
            HandleMe?.Invoke(this);
        }

        [DebuggerHidden]
        public void SetCompleted()
        {
            Result.State = ActionStates.Completed;
            Result.StopTimestamp = DateTime.Now;
        }

        public string Description
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                if (LogEntryPointClassLineNumber > 0)
                    sb.AppendFormat(" Ln {0}", LogEntryPointClassLineNumber);
                if (!string.IsNullOrEmpty(LogEntryPointCategory))
                    sb.AppendFormat(" {0}", LogEntryPointCategory);
                if (!string.IsNullOrEmpty(LogEntryPointSource))
                    sb.AppendFormat(" {0}", LogEntryPointSource);
                return sb.ToString();
            }
        }

        public void Suspend() => _isSuspended = true;

        public void Resume() => _isSuspended = false;

        internal void SetCancelling()
        {
            Result.State = ActionStates.Cancelling;
        }

        internal void SetCancelled()
        {
            Result.State = ActionStates.Cancelled;
        }
    }
}
