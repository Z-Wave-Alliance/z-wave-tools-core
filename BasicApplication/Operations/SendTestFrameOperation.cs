/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;
using ZWave.Devices;
using ZWave.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class SendTestFrameOperation : CallbackApiOperation
    {
        internal NodeTag Node { get; set; }
        internal byte PowerLevel { get; set; }
        public SendTestFrameOperation(NetworkViewPoint network, NodeTag node, byte powerLevel)
            : base(CommandTypes.CmdZWaveSendTestFrame)
        {
            _network = network;
            Node = node;
            PowerLevel = powerLevel;
        }

        protected override byte[] CreateInputParameters()
        {
            if (_network.IsNodeIdBaseTypeLR)
            {
                return new byte[] { (byte)(Node.Id >> 8), (byte)Node.Id, PowerLevel };
            }
            else
            {
                return new byte[] { (byte)Node.Id, PowerLevel };
            }
        }

        protected override void OnCallbackInternal(DataReceivedUnit ou)
        {
            if (ou.DataFrame.Payload != null && ou.DataFrame.Payload.Length > 1)
            {
                SpecificResult.FuncId = ou.DataFrame.Payload[0];
                SpecificResult.TransmitStatus = (TransmitStatuses)ou.DataFrame.Payload[1];
            }
        }

        public TransmitResult SpecificResult
        {
            get { return (TransmitResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new TransmitResult();
        }
    }
}
