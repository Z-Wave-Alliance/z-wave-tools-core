/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;
using ZWave.BasicApplication.Enums;
using ZWave.Devices;
using ZWave.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class SerialApiGetInitDataOperation : RequestApiOperation
    {
        private readonly bool _isNoAck;
        public SerialApiGetInitDataOperation()
            : base(CommandTypes.CmdSerialApiGetInitData, false)
        {
        }

        public SerialApiGetInitDataOperation(bool isNoAck)
           : base(CommandTypes.CmdSerialApiGetInitData, false)
        {
            _isNoAck = isNoAck;
            TimeoutMs = 200;
        }

        protected override byte[] CreateInputParameters()
        {
            return null;
        }

        protected override void CreateInstance()
        {
            base.CreateInstance();
            message.IsNoAck = _isNoAck;
            message.IsSequenceNumberRequired = false;
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            SpecificResult.SerialApiVersion = ((DataReceivedUnit)ou).DataFrame.Payload[0];
            SpecificResult.SerialApiCapability = ((DataReceivedUnit)ou).DataFrame.Payload[1];

            byte nodeIdx = 0;
            List<NodeTag> includedNodes = new List<NodeTag>();
            for (int i = 0; i < ((DataReceivedUnit)ou).DataFrame.Payload[2]; i++)
            {
                byte availabilityMask = ((DataReceivedUnit)ou).DataFrame.Payload[3 + i];
                for (byte bit = 0; bit < 8; bit++)
                {
                    nodeIdx++;
                    if ((availabilityMask & (1 << bit)) > 0)
                    {
                        includedNodes.Add(new NodeTag(nodeIdx));
                    }
                }
            }
            SpecificResult.IncludedNodes = includedNodes.ToArray();
            SpecificResult.ChipType = (ChipTypes)((DataReceivedUnit)ou).DataFrame.Payload[3 + ((DataReceivedUnit)ou).DataFrame.Payload[2]];
            SpecificResult.ChipRevision = ((DataReceivedUnit)ou).DataFrame.Payload[4 + ((DataReceivedUnit)ou).DataFrame.Payload[2]];
            base.SetStateCompleted(ou);
        }

        public SerialApiGetInitDataResult SpecificResult
        {
            get { return (SerialApiGetInitDataResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new SerialApiGetInitDataResult();
        }

        public override string AboutMe()
        {
            return string.Format($"ver={SpecificResult.SerialApiVersion}" +
                $", cap={SpecificResult.SerialApiCapability}" +
                $", chip={SpecificResult.ChipType}" +
                $", rev={SpecificResult.ChipRevision}" +
                $", nodes={SpecificResult.IncludedNodes?.Length}");
        }
    }

    public class SerialApiGetInitDataResult : ActionResult
    {
        public byte SerialApiVersion { get; set; }
        public byte SerialApiCapability { get; set; }
        public ChipTypes ChipType { get; set; }
        public byte ChipRevision { get; set; }
        public NodeTag[] IncludedNodes { get; set; }
    }
}
