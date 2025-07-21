/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.CommandClasses;
using ZWave.Enums;
using Utils;

namespace ZWave.ZipApplication.Operations
{
    public class SetLearnModeOperation : RequestDataOperation
    {
        private LearnModes Mode { get; set; }
        public SetLearnModeOperation(LearnModes mode, int timeoutMs)
            : base(null, null,
            COMMAND_CLASS_NETWORK_MANAGEMENT_BASIC.ID,
            COMMAND_CLASS_NETWORK_MANAGEMENT_BASIC.LEARN_MODE_SET_STATUS.ID,
            new ByteIndex[]
            {
                //ByteIndex.AnyValue,
                //new ByteIndex(0x0A)
            },
            timeoutMs)
        {
            Mode = mode;
            Data = new COMMAND_CLASS_NETWORK_MANAGEMENT_BASIC_V2.LEARN_MODE_SET()
            {
                mode = (byte)Mode,
                seqNo = SequenceNumber,
                properties1 = 0x00
            };
        }

        protected override void OnReceived(DataReceivedUnit ou)
        {
            var packet = (COMMAND_CLASS_NETWORK_MANAGEMENT_BASIC.LEARN_MODE_SET_STATUS)ou.DataFrame.Payload;
            SpecificResult.NodeId = packet.newNodeId;
            SpecificResult.Status = packet.status;

            SetStateCompleted(ou);
        }

        public override string AboutMe()
        {
            return string.Format("Id={0}, Status={1}", SpecificResult.NodeId, SpecificResult.Status);
        }

        public new SetLearnModeResult SpecificResult
        {
            get { return (SetLearnModeResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new SetLearnModeResult();
        }
    }

    public class SetLearnModeResult : RequestDataResult
    {
        public byte NodeId { get; set; }
        public byte Status { get; set; }
    }
}