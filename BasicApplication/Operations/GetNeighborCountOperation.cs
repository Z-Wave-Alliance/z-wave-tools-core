/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    public class GetNeighborCountOperation : RequestApiOperation
    {
        private NodeTag Node { get; set; }
        public GetNeighborCountOperation(NodeTag node)
            : base(CommandTypes.CmdZWaveGetNeighborCount, false)
        {
            Node = node;
        }

        protected override byte[] CreateInputParameters()
        {
            return new byte[] { (byte)Node.Id };
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            SpecificResult.RetValue = ((DataReceivedUnit)ou).DataFrame.Payload[0];
            base.SetStateCompleted(ou);
        }

        public GetNeighborCountResult SpecificResult
        {
            get { return (GetNeighborCountResult)Result; }
        }
        
        protected override ActionResult CreateOperationResult()
        {
            return new GetNeighborCountResult() ;
        }
    }

    public class GetNeighborCountResult : ActionResult
    {
        public byte RetValue { get; set; }
    }
}
