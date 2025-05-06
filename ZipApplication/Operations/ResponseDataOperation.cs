/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Linq;
using System.Collections.Generic;
using ZWave.CommandClasses;

namespace ZWave.ZipApplication.Operations
{
    public class ResponseDataOperation : ZipApiOperation
    {
        internal List<byte[]> Data { get; set; }
        private byte CmdClass { get; set; }
        private byte Cmd { get; set; }
        private byte[] BytesToCompare { get; set; }
        private int NumBytesToCompare { get; set; }

        private bool IsNoAck = false;
        public Func<byte[], byte[], byte[]> ReceiveCallback { get; protected set; }
        public Func<ActionToken, byte[], byte[], List<byte[]>> ReceiveExCallback { get; private set; }
        protected bool _isHeaderExtensionSpecified;

        public ResponseDataOperation(byte[] headerExtension, Func<ActionToken, byte[], byte[], List<byte[]>> receiveCallback, byte cmdClass, byte cmd)
            : base(false)
        {
            _headerExtension = headerExtension;
            _isHeaderExtensionSpecified = headerExtension != null;
            ReceiveExCallback = receiveCallback;
            CmdClass = cmdClass;
            Cmd = cmd;
            Data = new List<byte[]>();
        }

        public ResponseDataOperation(byte[] headerExtension, Func<byte[], byte[], byte[]> receiveCallback, byte cmdClass, byte cmd)
            : base(false)
        {
            _headerExtension = headerExtension;
            _isHeaderExtensionSpecified = headerExtension != null;
            ReceiveCallback = receiveCallback;
            CmdClass = cmdClass;
            Cmd = cmd;
            Data = new List<byte[]>();
        }

        public ResponseDataOperation(byte[] headerExtension, Func<byte[], byte[], byte[]> receiveCallback, byte cmdClass, byte cmd, bool isNoAck)
            : base(false)
        {
            _headerExtension = headerExtension;
            _isHeaderExtensionSpecified = headerExtension != null;
            ReceiveCallback = receiveCallback;
            CmdClass = cmdClass;
            Cmd = cmd;
            Data = new List<byte[]>();
            IsNoAck = isNoAck;
        }

        public ResponseDataOperation(byte[] headerExtension, byte[] data, byte cmdClass, byte cmd)
            : base(false)
        {
            _headerExtension = headerExtension;
            _isHeaderExtensionSpecified = headerExtension != null;
            CmdClass = cmdClass;
            Cmd = cmd;
            Data = new List<byte[]>();
            Data.Add(data);
        }

        public ResponseDataOperation(byte[] headerExtension, byte[] data, byte[] expectedData, byte cmdClass, byte cmd, int numBytesToCompare)
            : base(false)
        {
            _headerExtension = headerExtension;
            _isHeaderExtensionSpecified = headerExtension != null;
            CmdClass = cmdClass;
            Cmd = cmd;
            BytesToCompare = expectedData;
            NumBytesToCompare = numBytesToCompare;
            Data = new List<byte[]>();
            Data.Add(data);
        }


        public ResponseDataOperation(byte cmdClass)
            : base(false)
        {
            Data = new List<byte[]>();
            CmdClass = cmdClass;
        }

        private ZipApiHandler expectReceived;
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 0));
            ActionUnits.Add(new DataReceivedUnit(expectReceived, OnReceived));
        }

        protected override void CreateInstance()
        {
            if (NumBytesToCompare > 0)
            {
                Utils.ByteIndex[] commandFilter = new Utils.ByteIndex[NumBytesToCompare];
                for (int i = 0; i < NumBytesToCompare; i++)
                {
                    commandFilter[i] = new Utils.ByteIndex(BytesToCompare[i + 2], 0xFF);
                }
                expectReceived = new ZipApiHandler(CmdClass, Cmd, commandFilter);
            }
            else
                expectReceived = new ZipApiHandler(CmdClass, Cmd);
        }

        private void OnReceived(DataReceivedUnit ou)
        {
            byte[] headerReceived = null;
            var dataReceived = ou.DataFrame.Data;
            if (dataReceived != null && dataReceived.Length > 2 && dataReceived[0] == COMMAND_CLASS_ZIP_V2.ID && dataReceived[1] == COMMAND_CLASS_ZIP_V2.COMMAND_ZIP_PACKET.ID)
            {
                COMMAND_CLASS_ZIP_V4.COMMAND_ZIP_PACKET packet = dataReceived;
                if (packet.headerExtension != null)
                {
                    headerReceived = packet.headerExtension.ToArray();
                }
            }

            if (ReceiveCallback != null)
            {
                Data.Clear();
                var data = ReceiveCallback(_headerExtension, ou.DataFrame.Payload);
                if (data != null)
                    Data.Add(data);
            }
            else if (ReceiveExCallback != null)
            {
                Data = ReceiveExCallback(Token, _headerExtension, ou.DataFrame.Payload);
            }

            if (Data != null)
            {
                //var headerExtension = _isHeaderExtensionSpecified ? _headerExtension : headerReceived;
                var headerExtension = _headerExtension;
                ZipApiOperation[] operations = new ZipApiOperation[Data.Count];
                for (int i = 0; i < Data.Count; i++)
                {
                    operations[i] = new SendDataOperation(headerExtension, Data[i], false, 10, IsNoAck);//We need to be more agressive on the data response, do not wait for callbacks
                    operations[i].CompletedCallback = (x) =>
                    {
                        var action = x as ActionBase;
                        if (action != null)
                        {
                            SpecificResult.TotalCount++;
                            if (action.Result.State != ActionStates.Completed)
                                SpecificResult.FailCount++;
                        }
                    };
                }
                ou.SetNextActionItems(operations);
            }
            else
            {
                ou.SetNextActionItems();
            }
            SpecificResult.TotalCount++;
        }

        public ResponseDataResult SpecificResult
        {
            get { return (ResponseDataResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ResponseDataResult();
        }
    }

    public class ResponseDataResult : ActionResult
    {
        public int TotalCount { get; set; }
        public int FailCount { get; set; }
    }
}
