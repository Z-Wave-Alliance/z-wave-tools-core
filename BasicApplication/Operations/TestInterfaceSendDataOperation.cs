/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using Utils;
using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class TestInterfaceSendDataOperation : ApiOperation
    {
        readonly int _timeoutMs = 0;
        private readonly byte[] _testInterfaceCmd;
        public bool IsRetransmitWithNextFuncId { get; set; }
        public byte[] TestInterfaceCmd
        {
            get { return _testInterfaceCmd; }
        }

        public TestInterfaceSendDataOperation(byte[] testInterfaceCmd, int timeoutMs)
            : this(testInterfaceCmd, false, timeoutMs)
        {
        }

        public TestInterfaceSendDataOperation(byte[] testInterfaceCmd, bool isRetransmitWithNextFuncId, int timeoutMs)
            : base(true, null, true)
        {
            _testInterfaceCmd = testInterfaceCmd ?? new byte[0];
            _timeoutMs = timeoutMs;
            IsExtraSequenceNumberRequired = isRetransmitWithNextFuncId;
        }


        ApiProgMessage message;
        ApiProgMessage extraMessage;
        ApiProgHandler handler;
        ITimeoutItem timeoutItem;

        protected override void CreateWorkflow()
        {
            if (IsExtraSequenceNumberRequired)
            {
                ActionUnits.Add(new StartActionUnit(null, _timeoutMs, message, timeoutItem));
                ActionUnits.Add(new TimeElapsedUnit(timeoutItem, OnRetry, _timeoutMs, extraMessage));
                ActionUnits.Add(new DataReceivedUnit(handler, OnRetDataReceived));
            }
            else
            {
                ActionUnits.Add(new StartActionUnit(null, _timeoutMs, message));
                ActionUnits.Add(new DataReceivedUnit(handler, OnRetDataReceived));
            }
        }

        private void OnRetry(TimeElapsedUnit ou)
        {
            "retry TestInterfaceSendData command"._DLOG();
        }

        protected override void CreateInstance()
        {
            message = new ApiProgMessage(_testInterfaceCmd);
            message.SetSequenceNumber(SequenceNumber);
            message.IsNoAck = true;
            extraMessage = new ApiProgMessage(_testInterfaceCmd);
            extraMessage.SetSequenceNumber(ExtraSequenceNumber);
            extraMessage.IsNoAck = true;
            handler = new ApiProgHandler(_testInterfaceCmd[0]);
            timeoutItem = new TimeInterval(GetNextCounter(), Id, _timeoutMs - 200);
        }

        private void OnRetDataReceived(DataReceivedUnit ou)
        {
            SpecificResult.ByteArray = ou.DataFrame.Payload;
            if (ou.DataFrame.Payload[0] == _testInterfaceCmd[0])
            {
                if (ou.DataFrame.Payload.Length >= 2)
                {
                    SpecificResult.ByteArray = new byte[ou.DataFrame.Payload.Length - 2];
                    Array.Copy(ou.DataFrame.Payload, 1, SpecificResult.ByteArray, 0, SpecificResult.ByteArray.Length);
                }
            }
            SetStateCompleted(ou);
        }

        public override string AboutMe()
        {
            if (_testInterfaceCmd != null)
            {
                if (SpecificResult.ByteArray != null)
                {
                    return string.Format("Data: {0} Ret: {1}", _testInterfaceCmd.GetHex(), SpecificResult.ByteArray.GetHex());
                }
                else
                {
                    return string.Format("Data: {0}", _testInterfaceCmd.GetHex());
                }
            }
            else
            {
                return string.Empty;
            }

        }

        public ReturnValueResult SpecificResult
        {
            get { return (ReturnValueResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ReturnValueResult();
        }
    }
}
