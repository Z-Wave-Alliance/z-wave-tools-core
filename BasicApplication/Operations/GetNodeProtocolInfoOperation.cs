/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    public class GetNodeProtocolInfoOperation : RequestApiOperation
    {
        private NodeTag Node { get; set; }
        public GetNodeProtocolInfoOperation(NetworkViewPoint network, NodeTag node)
            : base(CommandTypes.CmdZWaveGetNodeProtocolInfo, false)
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
            SpecificResult.Node = Node;
            if (((DataReceivedUnit)ou).DataFrame.Payload.Length > 6)
            {
                SpecificResult.NodeInfo = new NodeInfo
                {
                    Capability = ((DataReceivedUnit)ou).DataFrame.Payload[0],
                    Security = ((DataReceivedUnit)ou).DataFrame.Payload[1],
                    Properties1 = ((DataReceivedUnit)ou).DataFrame.Payload[2],
                    Basic = ((DataReceivedUnit)ou).DataFrame.Payload[3],
                    Generic = ((DataReceivedUnit)ou).DataFrame.Payload[4],
                    Specific = ((DataReceivedUnit)ou).DataFrame.Payload[5],
                    LongRange = ((DataReceivedUnit)ou).DataFrame.Payload[6]
                };
            }
            else if (((DataReceivedUnit)ou).DataFrame.Payload.Length > 5)
            {
                SpecificResult.NodeInfo = new NodeInfo
                {
                    Capability = ((DataReceivedUnit)ou).DataFrame.Payload[0],
                    Security = ((DataReceivedUnit)ou).DataFrame.Payload[1],
                    Properties1 = ((DataReceivedUnit)ou).DataFrame.Payload[2],
                    Basic = ((DataReceivedUnit)ou).DataFrame.Payload[3],
                    Generic = ((DataReceivedUnit)ou).DataFrame.Payload[4],
                    Specific = ((DataReceivedUnit)ou).DataFrame.Payload[5],
                };
            }
            base.SetStateCompleted(ou);
        }

        public GetNodeProtocolInfoResult SpecificResult
        {
            get { return (GetNodeProtocolInfoResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new GetNodeProtocolInfoResult();
        }
    }

    public class GetNodeProtocolInfoResult : ActionResult
    {
        public NodeTag Node { get; set; }
        public NodeInfo NodeInfo { get; set; }
    }
}
