/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.Enums;
using ZWave.BasicApplication.Enums;
using Utils;
using ZWave.Devices;
using System;

namespace ZWave.BasicApplication
{
    public abstract class CallbackApiOperation : ApiOperation
    {
        public static int RET_TIMEOUT = 7000;
        public static int CALLBACK_TIMEOUT = 60000;
        internal int TimeoutMs { get; set; }
        private CommandTypes _commandAbort = CommandTypes.None;
        //internal Action<ActionUnit> OnHandledCallback { get; set; }
        public CallbackApiOperation(CommandTypes command)
            : base(true, command, true)
        {
        }

        public CallbackApiOperation(CommandTypes command, CommandTypes commandAbort)
          : base(true, command, true)
        {
            _commandAbort = commandAbort;
        }

        private ApiMessage Message;
        private ApiMessage MessageAbort;
        private ApiHandler HandlerOk;
        private ApiHandler HandlerFailed;
        private ApiHandler CallbackHandler;
        private ITimeoutItem timeoutItem;
        protected override void CreateWorkflow()
        {
            TimeoutMs = TimeoutMs > 0 ? TimeoutMs : CALLBACK_TIMEOUT;
            isHandled = false;
            timeoutItem = new TimeInterval(GetNextCounter(), Id, TimeoutMs);

            ActionUnits.Add(new StartActionUnit(OnStart, RET_TIMEOUT, Message));
            ActionUnits.Add(new DataReceivedUnit(HandlerFailed, OnFailed));
            if (_commandAbort != CommandTypes.None)
            {
                ActionUnits.Add(new DataReceivedUnit(HandlerOk, OnHandled, TimeoutMs * 2, timeoutItem));
                ActionUnits.Add(new TimeElapsedUnit(timeoutItem, null, TimeoutMs, MessageAbort));
            }
            else
            {
                ActionUnits.Add(new DataReceivedUnit(HandlerOk, OnHandled, TimeoutMs));
            }
            ActionUnits.Add(new DataReceivedUnit(CallbackHandler, OnCallback));
        }

        protected virtual void OnStart(StartActionUnit sau)
        {
            
        }

        protected override void CreateInstance()
        {
            Message = new ApiMessage(SerialApiCommands[0], CreateInputParameters());
            Message.SetSequenceNumber(SequenceNumber);

            if (_commandAbort != CommandTypes.None)
            {
                MessageAbort = new ApiMessage(_commandAbort);
                MessageAbort.IsSequenceNumberRequired = false;
            }

            HandlerOk = new ApiHandler(SerialApiCommands[0]);
            HandlerOk.AddConditions(new ByteIndex(0x01));
            HandlerFailed = new ApiHandler(SerialApiCommands[0]);
            HandlerFailed.AddConditions(new ByteIndex(0x00));
            CallbackHandler = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            CallbackHandler.AddConditions(new ByteIndex[]
            {
                new ByteIndex(SequenceNumber),
            });
            
        }

        private void OnFailed(DataReceivedUnit ou)
        {
            SetStateFailed(ou);
        }

        protected bool isHandled = false;
        protected virtual void OnHandled(DataReceivedUnit ou)
        {
            if (!isHandled)
            {
                isHandled = true;
            }
        }

        protected virtual void OnCallback(DataReceivedUnit ou)
        {
            OnCallbackInternal(ou);
            if (ou.DataFrame.Payload != null && ou.DataFrame.Payload.Length > 1)
            {
                SetStateCompleted(ou);
            }
            else
                SetStateFailed(ou);
        }

        protected virtual void OnCallbackInternal(DataReceivedUnit ou)
        {

        }

        protected override ActionResult CreateOperationResult()
        {
            return new ActionResult();
        }

        protected abstract byte[] CreateInputParameters();
    }
}
