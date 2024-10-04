/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class SetRFReceiveModeOperation : ApiOperation
    {
        private static int RETRIES = 3;
        private static int TRY_TIMEOUT = 300;
        private int _timeout = (RETRIES + 1) * TRY_TIMEOUT;
        private byte Mode { get; set; }
        public SetRFReceiveModeOperation(byte mode)
            : base(false, CommandTypes.CmdZWaveSetRFReceiveMode, false)
        {
            Mode = mode;
        }

        ApiMessage message;
        ApiHandler handler;
        ITimeoutItem timeoutItem;   

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, _timeout, message));
            ActionUnits.Add(new DataReceivedUnit(handler, OnReceived));
            ActionUnits.Add(new TimeElapsedUnit(timeoutItem, null, 0, message));
        }

        protected override void CreateInstance()
        {
            message = new ApiMessage(CommandTypes.CmdZWaveSetRFReceiveMode, Mode);
            handler = new ApiHandler(CommandTypes.CmdZWaveSetRFReceiveMode);
            timeoutItem = new TimeInterval(GetNextCounter(), Id, TRY_TIMEOUT);
        }

        private int retries = RETRIES;
        private void OnReceived(DataReceivedUnit ou)
        {
            SpecificResult.ByteArray = ou.DataFrame.Payload;
            if (ou.DataFrame.Payload != null && ou.DataFrame.Payload.Length > 0 && ou.DataFrame.Payload[0] == 1)
            {
                ou.SetNextActionItems(null);
                SetStateCompleted(ou);
            }
            else
            {
                if (--retries <= 0)
                {
                    ou.SetNextActionItems(null);
                    SetStateFailed(ou);
                }
                else
                {
                    ou.SetNextActionItems(timeoutItem);
                }
            }
        }

        public ReturnValueResult SpecificResult
        {
            get { return (ReturnValueResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ReturnValueResult();
        }
    }
}