/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;
using ZWave.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class SetDefaultOperation : ApiOperation
    {
        public SetDefaultOperation()
            : base(true, CommandTypes.CmdZWaveSetDefault, true)
        {
        }

        ApiMessage message;
        ApiHandler handler;

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 4000, message));
            ActionUnits.Add(new DataReceivedUnit(handler, SetStateCompleted));
        }

        protected override void CreateInstance()
        {
            message = new ApiMessage(SerialApiCommands[0]);
            message.SetSequenceNumber(SequenceNumber);
            handler = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ActionResult();
        }
    }
}
