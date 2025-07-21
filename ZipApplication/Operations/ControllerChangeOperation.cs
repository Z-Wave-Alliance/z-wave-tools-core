/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Linq;
using ZWave.CommandClasses;
using ZWave.Devices;
using ZWave.Enums;

namespace ZWave.ZipApplication.Operations
{
    public class ControllerChangeOperation : RequestDataOperation
    {
        private Modes Mode { get; set; }
        private TransmitOptions TxOptions { get; set; }
        public ControllerChangeOperation(Modes mode, TransmitOptions txOptions, int timeoutMs)
            : base(null, null,
            COMMAND_CLASS_NETWORK_MANAGEMENT_PRIMARY.ID,
            COMMAND_CLASS_NETWORK_MANAGEMENT_PRIMARY.CONTROLLER_CHANGE_STATUS.ID,
            timeoutMs)
        {
            Mode = mode;
            TxOptions = txOptions;
            Data = new COMMAND_CLASS_NETWORK_MANAGEMENT_PRIMARY.CONTROLLER_CHANGE()
            {
                mode = (byte)Mode,
                txOptions = (byte)TxOptions,
                seqNo = SequenceNumber
            };
        }

        protected ZipApiMessage messageStop;
        protected override void CreateInstance()
        {
            base.CreateInstance();
            messageStop = new ZipApiMessage(_headerExtension, new COMMAND_CLASS_NETWORK_MANAGEMENT_PRIMARY.CONTROLLER_CHANGE()
            {
                mode = (byte)Modes.NodeStop,
                seqNo = SequenceNumber,
            });
            messageStop.SetSequenceNumber(SequenceNumber);
            StopActionUnit = new StopActionUnit(messageStop);
        }

        protected override void OnReceived(DataReceivedUnit ou)
        {
            COMMAND_CLASS_NETWORK_MANAGEMENT_PRIMARY.CONTROLLER_CHANGE_STATUS packet = (COMMAND_CLASS_NETWORK_MANAGEMENT_PRIMARY.CONTROLLER_CHANGE_STATUS)ou.DataFrame.Payload;
            SpecificResult.Node = new NodeTag(packet.newNodeId);
            SpecificResult.NodeInfo = new NodeInfo
            {
                Capability = packet.properties1,
                Security = packet.properties2,
                Properties1 = 0,
                Basic = packet.basicDeviceClass,
                Generic = packet.genericDeviceClass,
                Specific = packet.specificDeviceClass
            };
            SpecificResult.CommandClasses = packet.commandClass.ToArray();
            SpecificResult.Status = packet.status;

            SetStateCompleted(ou);
        }

        public override string AboutMe()
        {
            return string.Format("Id={0}, Status={1}", SpecificResult.Node, SpecificResult.Status);
        }

        public new ControllerChangeResult SpecificResult
        {
            get { return (ControllerChangeResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ControllerChangeResult();
        }
    }

    public class ControllerChangeResult : RequestDataResult
    {
        public NodeTag Node { get; set; }
        public NodeInfo NodeInfo { get; set; }
        public byte[] CommandClasses { get; set; }
        public byte Status { get; set; }
    }
}
