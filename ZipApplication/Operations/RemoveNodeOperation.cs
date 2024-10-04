/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.CommandClasses;
using ZWave.Devices;
using ZWave.Enums;

namespace ZWave.ZipApplication.Operations
{
    public class RemoveNodeOperation : RequestDataOperation
    {
        private Modes Mode { get; set; }
        public RemoveNodeOperation(Modes mode, int timeoutMs)
            : base(null, null,
            COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V4.ID,
            COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V4.NODE_REMOVE_STATUS.ID,
            timeoutMs)
        {
            Mode = mode;
            Data = new COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V4.NODE_REMOVE()
            {
                mode = (byte)Mode,
                seqNo = SequenceNumber
            };
        }

        protected ZipApiMessage messageStop;
        protected override void CreateInstance()
        {
            base.CreateInstance();
            messageStop = new ZipApiMessage(_headerExtension, new COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V4.NODE_REMOVE()
            {
                mode = (byte)Modes.NodeStop,
                seqNo = SequenceNumber
            });
            messageStop.SetSequenceNumber(SequenceNumber);
            StopActionUnit = new StopActionUnit(messageStop);
        }

        protected override void OnReceived(DataReceivedUnit ou)
        {
            COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V4.NODE_REMOVE_STATUS packet = ou.DataFrame.Payload;
            SpecificResult.Status = packet.status;
            if (packet.nodeid == 0xFF && // Removed NodeID is greater than 255 i.e. working in long range.
                packet.extendedNodeid != null && 
                packet.extendedNodeid.Length == 2) 
            {
                SpecificResult.Node = new NodeTag((ushort)((packet.extendedNodeid[0] << 8) + packet.extendedNodeid[1]));
            }
            else
            {
                SpecificResult.Node = new NodeTag(packet.nodeid);
            }

            SetStateCompleted(ou);
        }

        public override string AboutMe()
        {
            return string.Format("Id={0}, Status={1}", SpecificResult.Node, SpecificResult.Status);
        }

        public new RemoveNodeResult SpecificResult
        {
            get { return (RemoveNodeResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new RemoveNodeResult();
        }
    }

    public class RemoveNodeResult : RequestDataResult
    {
        public NodeTag Node { get; set; }
        public byte Status { get; set; }
    }
}
