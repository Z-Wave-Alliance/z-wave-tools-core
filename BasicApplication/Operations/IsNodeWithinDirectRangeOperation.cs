/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0x5D | bNodeID
    /// ZW->HOST: RES | 0x5D | retVal
    /// </summary>
    public class IsNodeWithinDirectRangeOperation : RequestApiOperation
    {
        private byte NodeId { get; set; }
        public IsNodeWithinDirectRangeOperation(byte nodeId)
            : base(CommandTypes.CmdZWaveIsNodeWithinDirectRange, false)
        {
            NodeId = nodeId;
        }

        protected override byte[] CreateInputParameters()
        {
            return new byte[] { NodeId };
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            byte[] res = ((DataReceivedUnit)ou).DataFrame.Payload;
            SpecificResult.RetValue = res[0] == 1;
            base.SetStateCompleted(ou);
        }

        public IsNodeWithinDirectRangeResult SpecificResult
        {
            get { return (IsNodeWithinDirectRangeResult)Result; }
        }
        
        protected override ActionResult CreateOperationResult()
        {
            return new IsNodeWithinDirectRangeResult();
        }
    }

    public class IsNodeWithinDirectRangeResult : ActionResult
    {
        public bool RetValue { get; set; }
    }
}
