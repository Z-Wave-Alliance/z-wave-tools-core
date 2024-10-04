/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class InitiateShutdownOperation : ApiOperation
    {
        public InitiateShutdownOperation()
            : base(false, CommandTypes.CmdZWaveInitiateShutdown, false)
        {
        }

        ApiMessage _message;
        ApiHandler _handler;

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 5000, _message));
            ActionUnits.Add(new DataReceivedUnit(_handler, OnReceived));
        }

        private void OnReceived(DataReceivedUnit ou)
        {
            if (ou.DataFrame?.Payload != null
                && ou.DataFrame.Payload.Length > 0)
            {
                SpecificResult.ByteArray = ou.DataFrame.Payload;
                SetStateCompleted(ou);
            }
            else
            {
                SetStateFailed(ou);
            }
        }

        protected override void CreateInstance()
        {
            _message = new ApiMessage(SerialApiCommands[0]);
            _handler = new ApiHandler(SerialApiCommands[0]);
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
