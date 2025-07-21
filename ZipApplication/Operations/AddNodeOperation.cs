/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;
using ZWave.CommandClasses;
using ZWave.Enums;
using ZWave.Xml.Application;
using ZWave.Devices;

namespace ZWave.ZipApplication.Operations
{
    public sealed class AddNodeOperation : ZipApiOperation
    {
        private readonly Modes _mode;
        private readonly TransmitOptions _txOptions;
        private readonly int _timeoutMs;

        public AddNodeOperation(Modes mode, TransmitOptions txOptions, int timeoutMs)
            : base(false)
        {
            _mode = mode;
            _txOptions = txOptions;
            _timeoutMs = timeoutMs;
        }

        private ZipApiMessage _messageStop;
        private ZipApiMessage _nodeAddMsg;
        private ZipApiHandler _ackHandler;
        private ZipApiHandler _nackHandler;
        private ZipApiHandler _nodeAddStatusHandler;
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, AckTimeout, _nodeAddMsg));
            ActionUnits.Add(new DataReceivedUnit(_ackHandler, OnAck, _timeoutMs));
            ActionUnits.Add(new DataReceivedUnit(_nackHandler, OnAck, _timeoutMs));
            ActionUnits.Add(new DataReceivedUnit(_nodeAddStatusHandler, OnNodeAddStatus));
            StopActionUnit = new StopActionUnit(_messageStop);
        }

        protected override void CreateInstance()
        {
            var data = new COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V3.NODE_ADD()
            {
                mode = (byte)_mode,
                txOptions = (byte)_txOptions,
                seqNo = SequenceNumber
            };
            _nodeAddMsg = new ZipApiMessage(_headerExtension, data);
            _nodeAddMsg.SetSequenceNumber(SequenceNumber);

            _nodeAddStatusHandler = new ZipApiHandler(COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V3.ID,
                COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V3.NODE_ADD_STATUS.ID);

            _ackHandler = ZipApiHandler.CreateAckHandler(SequenceNumber, true);

            _nackHandler = ZipApiHandler.CreateNAckHandler(SequenceNumber, true);

            _messageStop = new ZipApiMessage(_headerExtension, new COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V3.NODE_ADD()
            {
                mode = (byte)Modes.NodeStop,
                seqNo = SequenceNumber
            });
            _messageStop.SetSequenceNumber(SequenceNumber);
        }

        private void OnAck(DataReceivedUnit ou)
        {
            SpecificResult.IsAckReceived = ((COMMAND_CLASS_ZIP_V2.COMMAND_ZIP_PACKET.Tproperties1)ou.DataFrame.Data[2]).ackResponse > 0;
            SpecificResult.IsNAckReceived = ((COMMAND_CLASS_ZIP_V2.COMMAND_ZIP_PACKET.Tproperties1)ou.DataFrame.Data[2]).nackResponse > 0;
        }

        private void OnNodeAddStatus(DataReceivedUnit ou)
        {
            var packet = (COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V3.NODE_ADD_STATUS)ou.DataFrame.Payload;
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

            var grantedKeys = (NetworkKeyS2Flags)packet.grantedKeys.Value;
            List<SecuritySchemes> securitySchemes = new List<SecuritySchemes>();
            if (grantedKeys.HasFlag(NetworkKeyS2Flags.S2Class2))
            {
                securitySchemes.Add(SecuritySchemes.S2_ACCESS);
            }
            if (grantedKeys.HasFlag(NetworkKeyS2Flags.S2Class1))
            {
                securitySchemes.Add(SecuritySchemes.S2_AUTHENTICATED);
            }
            if (grantedKeys.HasFlag(NetworkKeyS2Flags.S2Class0))
            {
                securitySchemes.Add(SecuritySchemes.S2_UNAUTHENTICATED);
            }
            if (grantedKeys.HasFlag(NetworkKeyS2Flags.S0))
            {
                securitySchemes.Add(SecuritySchemes.S0);
            }
            SpecificResult.SecuritySchemes = securitySchemes.ToArray();

            if (packet.commandClass != null)
            {
                ZWaveDefinition.TryParseCommandClassRef(packet.commandClass, out byte[] commandClasses, out byte[] secureCommandClasses);
                SpecificResult.CommandClasses = commandClasses;
                SpecificResult.SecureCommandClasses = secureCommandClasses;
            }
            SpecificResult.Status = packet.status;

            SetStateCompleted(ou);
        }

        public override string AboutMe()
        {
            return string.Format("Id={0}, Status={1}", SpecificResult.Node, SpecificResult.Status);
        }

        public AddNodeResult SpecificResult
        {
            get { return (AddNodeResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new AddNodeResult();
        }
    }

    public sealed class AddNodeResult : RequestDataResult
    {
        public NodeTag Node { get; set; }
        public NodeInfo NodeInfo { get; set; }
        public byte[] CommandClasses { get; set; }
        public byte[] SecureCommandClasses { get; set; }
        public SecuritySchemes[] SecuritySchemes { get; set; }
        public byte Status { get; set; }
    }
}
