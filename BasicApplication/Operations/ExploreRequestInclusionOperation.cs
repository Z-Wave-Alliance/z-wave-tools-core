/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;
using Utils;
using ZWave.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class ExploreRequestInclusionOperation : ApiOperation
    {
        readonly byte FuncId;
        public ExploreRequestInclusionOperation(byte funcId)
            : base(true, CommandTypes.CmdZWaveExploreRequestInclusion, true)
        {
            FuncId = funcId;
        }
        ApiMessage message;
        ApiHandler handlerOk;
        ApiHandler handlerFail;

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 1000, message));
            ActionUnits.Add(new DataReceivedUnit(handlerOk, SetStateCompleted));
            ActionUnits.Add(new DataReceivedUnit(handlerFail, SetStateFailed));
        }

        protected override void CreateInstance()
        {
            message = new ApiMessage(SerialApiCommands[0]);
            if (FuncId > 0)
                message.SetSequenceNumber(FuncId);
            handlerOk = new ApiHandler(FrameTypes.Response, SerialApiCommands[0]);
            handlerOk.AddConditions(new ByteIndex(0x01));
            handlerFail = new ApiHandler(FrameTypes.Response, SerialApiCommands[0]);
            handlerFail.AddConditions(new ByteIndex(0x00));
        }

    }
}
