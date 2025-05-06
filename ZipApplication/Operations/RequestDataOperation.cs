/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Linq;
using ZWave.CommandClasses;
using Utils;

namespace ZWave.ZipApplication.Operations
{
    public class RequestDataOperation : ZipApiOperation
    {
        protected bool IsNoAck { get; set; }
        internal byte[] Data { get; set; }
        protected byte CmdClass { get; set; }
        protected byte Cmd { get; set; }
        protected ByteIndex[] PayloadFilter { get; set; }
        protected int TimeoutMs { get; set; }
        public RequestDataOperation(byte[] headerExtension, byte[] data, byte cmdClass, byte cmd, ByteIndex[] payloadFilter, int timeoutMs)
            : base(true)
        {
            _headerExtension = headerExtension;
            Data = data;
            CmdClass = cmdClass;
            Cmd = cmd;
            PayloadFilter = payloadFilter;
            TimeoutMs = timeoutMs;
        }

        public RequestDataOperation(byte[] headerExtension, byte[] data, byte cmdClass, byte cmd, int timeoutMs)
            : this(headerExtension, data, cmdClass, cmd, null, timeoutMs)
        {
        }

        protected ZipApiMessage message;
        private ZipApiHandler ack;
        private ZipApiHandler nack;
        private ZipApiHandler expectReceived;
        private ZipApiHandler busyReceived;
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, IsNoAck ? TimeoutMs : AckTimeout, message));
            if (!IsNoAck)
            {
                ActionUnits.Add(new DataReceivedUnit(ack, OnAck, TimeoutMs));
                ActionUnits.Add(new DataReceivedUnit(nack, OnAck, TimeoutMs));
            }
            ActionUnits.Add(new DataReceivedUnit(expectReceived, OnReceived));
            ActionUnits.Add(new DataReceivedUnit(busyReceived, OnBusyReceived));
        }

        protected override void CreateInstance()
        {
            message = new ZipApiMessage(_headerExtension, Data);
            message.SetSequenceNumber(SequenceNumber);
            message.IsNoAck = IsNoAck;
            expectReceived = new ZipApiHandler(CmdClass, Cmd, PayloadFilter);
            busyReceived = new ZipApiHandler(COMMAND_CLASS_APPLICATION_STATUS.ID, COMMAND_CLASS_APPLICATION_STATUS.APPLICATION_BUSY.ID);
            ack = ZipApiHandler.CreateAckHandler(SequenceNumber, true);
            nack = ZipApiHandler.CreateNAckHandler(SequenceNumber, true);
        }

        private void OnAck(DataReceivedUnit ou)
        {
            ((RequestDataResult)Result).IsAckReceived = ((COMMAND_CLASS_ZIP_V2.COMMAND_ZIP_PACKET.Tproperties1)ou.DataFrame.Data[2]).ackResponse > 0;
            ((RequestDataResult)Result).IsNAckReceived = ((COMMAND_CLASS_ZIP_V2.COMMAND_ZIP_PACKET.Tproperties1)ou.DataFrame.Data[2]).nackResponse > 0;
        }

        private void OnBusyReceived(DataReceivedUnit ou)
        {
            COMMAND_CLASS_APPLICATION_STATUS.APPLICATION_BUSY cmd = busyReceived.DataFrame.Payload;
            ((RequestDataResult)Result).IsBusyReceived = true;
            ((RequestDataResult)Result).BusyWaitTime = cmd.waitTime;
            ((RequestDataResult)Result).BusyStatus = cmd.status;
            SetStateFailed(ou);
        }

        protected virtual void OnReceived(DataReceivedUnit ou)
        {
            SpecificResult.ReceivedData = ou.DataFrame.Payload;
            var dataReceived = ou.DataFrame.Data;
            if (dataReceived != null && dataReceived.Length > 2 && dataReceived[0] == COMMAND_CLASS_ZIP_V2.ID && dataReceived[1] == COMMAND_CLASS_ZIP_V2.COMMAND_ZIP_PACKET.ID)
            {
                COMMAND_CLASS_ZIP_V2.COMMAND_ZIP_PACKET packet = dataReceived;
                if (packet.headerExtension != null)
                {
                    SpecificResult.ReceivedHeader = packet.headerExtension.ToArray();
                }
            }
            base.SetStateCompleted(ou);
        }

        public virtual RequestDataResult SpecificResult
        {
            get { return (RequestDataResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new RequestDataResult();
        }
    }

    public class RequestDataResult : ActionResult
    {
        public bool IsAckReceived { get; set; }
        public bool IsNAckReceived { get; set; }
        public bool IsBusyReceived { get; set; }
        public byte BusyWaitTime { get; set; }
        public byte BusyStatus { get; set; }
        public byte[] ReceivedHeader { get; set; }
        public byte[] ReceivedData { get; set; }
    }
}
