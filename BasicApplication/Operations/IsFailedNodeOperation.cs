/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    public class IsFailedNodeOperation : RequestApiOperation
    {
        internal NodeTag Node { get; set; }
        public IsFailedNodeOperation(NetworkViewPoint network, NodeTag node)
            : base(CommandTypes.CmdZWaveIsFailedNode, false)
        {
            _network = network;
            Node = node;
        }

        protected override byte[] CreateInputParameters()
        {
            var ret = new byte[] { (byte)Node.Id };
            if (_network.IsNodeIdBaseTypeLR)
            {
                ret = new byte[] { (byte)(Node.Id >> 8), (byte)Node.Id };
            }
            return ret;
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            SpecificResult.RetValue = ((DataReceivedUnit)ou).DataFrame.Payload[0] == 1;
            base.SetStateCompleted(ou);
        }

        public IsFailedNodeResult SpecificResult
        {
            get { return (IsFailedNodeResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new IsFailedNodeResult { Node = Node };
        }
    }

    public class IsFailedNodeResult : ActionResult
    {
        public NodeTag Node { get; set; }
        public bool RetValue { get; set; }
    }
}
