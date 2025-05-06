/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.CommandClasses;

namespace ZWave.ZipApplication.Operations
{
    public class SendDataOperation : ZipApiOperation
    {
        internal byte[] Data { get; set; }

        private bool IsNoAck = false;
        private bool IsValidateAckSeqNo { get; set; }
        public SendDataOperation(byte[] headerExtension, byte[] data)
            : this(headerExtension, data, true)
        { 
        }
        public SendDataOperation(byte[] headerExtension, byte[] data, bool isValidateAckSeqNo)
            : this(headerExtension, data, isValidateAckSeqNo, 0)
        {
        }
        public SendDataOperation(byte[] headerExtension, byte[] data, bool isValidateAckSeqNo, int timeout)
            : base(true)
        {
            Data = data;
            IsValidateAckSeqNo = isValidateAckSeqNo;
            _headerExtension = headerExtension;
            AckTimeout = timeout == 0 ? AckTimeout : timeout;
        }
        public SendDataOperation(byte[] headerExtension, byte[] data, bool isValidateAckSeqNo, int timeout, bool isNoAck)
            : base(true)
        {
            Data = data;
            IsValidateAckSeqNo = isValidateAckSeqNo;
            _headerExtension = headerExtension;
            AckTimeout = timeout == 0 ? AckTimeout : timeout;
            IsNoAck = isNoAck;
        }

        private ZipApiMessage message;
        private ZipApiHandler ack;
        private ZipApiHandler nack;
        private ZipApiHandler waitingNack;
        private ZipApiHandler busyReceived;

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, AckTimeout, message));
            ActionUnits.Add(new DataReceivedUnit(ack, OnAck));
            ActionUnits.Add(new DataReceivedUnit(waitingNack, OnAck));
            ActionUnits.Add(new DataReceivedUnit(nack, OnAck));
            ActionUnits.Add(new DataReceivedUnit(busyReceived, OnBusyReceived));
        }

        protected override void CreateInstance()
        {
            busyReceived = null;
            message = new ZipApiMessage(_headerExtension, Data, IsNoAck);
            message.SetSequenceNumber(SequenceNumber);
            ack = ZipApiHandler.CreateAckHandler(SequenceNumber, IsValidateAckSeqNo);
            nack = ZipApiHandler.CreateNAckHandler(SequenceNumber, IsValidateAckSeqNo);
            waitingNack = ZipApiHandler.CreateWaitingNAckHandler(SequenceNumber, IsValidateAckSeqNo);
        }

        private void OnAck(DataReceivedUnit ou)
        {            
            SpecificResult.AckDataBuffer = ou.DataFrame.Data;            

            COMMAND_CLASS_ZIP_V4.COMMAND_ZIP_PACKET myZipPacket = ou.DataFrame.Data;
            if (myZipPacket.properties1.nackResponse == 0x01)
            {
                SpecificResult.NackDataBuffer = ou.DataFrame.Data;
            }
            else
            {
                SpecificResult.NackDataBuffer = null;
            }

            SpecificResult.IsAckReceived = 
                ((COMMAND_CLASS_ZIP_V4.COMMAND_ZIP_PACKET.Tproperties1)ou.DataFrame.Data[2]).ackResponse > 0;
            SpecificResult.IsNAckReceived = 
                (((COMMAND_CLASS_ZIP_V4.COMMAND_ZIP_PACKET.Tproperties1)ou.DataFrame.Data[2]).nackResponse > 0) &&
                (((COMMAND_CLASS_ZIP_V4.COMMAND_ZIP_PACKET.Tproperties1)ou.DataFrame.Data[2]).nackWaiting < 1);
            SpecificResult.IsWnackReceived =
                (((COMMAND_CLASS_ZIP_V4.COMMAND_ZIP_PACKET.Tproperties1)ou.DataFrame.Data[2]).nackResponse > 0) &&
                (((COMMAND_CLASS_ZIP_V4.COMMAND_ZIP_PACKET.Tproperties1)ou.DataFrame.Data[2]).nackWaiting > 1);
            SetStateCompleted(ou);
        }

        private void OnBusyReceived(DataReceivedUnit ou)
        {
            SpecificResult.IsBusyReceived = true;
            SetStateFailed(ou);
        }

        public SendDataResult SpecificResult
        {
            get { return (SendDataResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new SendDataResult();
        }
    }

    public class SendDataResult : ActionResult
    {
        public byte[] NackDataBuffer { get; set; }
        public byte[] AckDataBuffer { get; set; }
        public bool IsAckReceived { get; set; }
        public bool IsNAckReceived { get; set; }
        public bool IsWnackReceived { get; set; }        
        public bool IsBusyReceived { get; set; }
    }
}
