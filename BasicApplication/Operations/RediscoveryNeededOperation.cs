/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;
using ZWave.Enums;
using Utils;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0x59 | bNodeID | funcID
    /// ZW->HOST: RES | 0x59 | retVal
    /// ZW->HOST: REQ | 0x59 | funcID | bStatus
    /// </summary>
    public class RediscoveryNeededOperation : ApiOperation
    {
        private readonly byte mNodeId;
        public RediscoveryNeededOperation(byte nodeId)
            : base(true, CommandTypes.CmdZWaveRediscoveryNeeded, true)
        {
            mNodeId = nodeId;
        }

        private ApiMessage messageStart;
        private ApiHandler handlerRetValTrue;
        private ApiHandler handlerRetValFalse;
        private ApiHandler handlerUpdateDone;              // code=0
        private ApiHandler handlerUpdateAbort;             // code=1
        private ApiHandler handlerLostFailed;              // code=4
        private ApiHandler handlerLostAccept;              // code=5

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 30000, messageStart));
            //ActionUnits.Add(new DataReceivedUnit(handlerRetValTrue, null));
            ActionUnits.Add(new DataReceivedUnit(handlerRetValFalse, SetStateFailed));
            //ActionUnits.Add(new DataReceivedUnit(handlerLostAccept, null));
            ActionUnits.Add(new DataReceivedUnit(handlerLostFailed, SetStateFailed));
            ActionUnits.Add(new DataReceivedUnit(handlerUpdateDone, SetStateCompleted));
            ActionUnits.Add(new DataReceivedUnit(handlerUpdateAbort, SetStateCompleted));
        }

        protected override void CreateInstance()
        {
            messageStart = new ApiMessage(SerialApiCommands[0], new byte[] { mNodeId });
            messageStart.SetSequenceNumber(SequenceNumber);

            handlerRetValTrue = new ApiHandler(SerialApiCommands[0]);
            handlerRetValTrue.AddConditions(new ByteIndex(0xFF, 0xFF));

            handlerRetValFalse = new ApiHandler(SerialApiCommands[0]);
            handlerRetValFalse.AddConditions(new ByteIndex(0x00));

            handlerUpdateDone = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            handlerUpdateDone.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)RouteStatuses.UpdateDone));

            handlerUpdateAbort = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            handlerUpdateAbort.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)RouteStatuses.UpdateAbort));

            handlerLostFailed = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            handlerLostFailed.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)RouteStatuses.LostFailed));

            handlerLostAccept = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            handlerLostAccept.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)RouteStatuses.LostAccept));
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ActionResult();
        }
    }
}
