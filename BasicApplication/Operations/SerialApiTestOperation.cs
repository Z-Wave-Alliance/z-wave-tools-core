/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using ZWave.BasicApplication.Enums;
using ZWave.Enums;
using Utils;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    public class SerialApiTestOperation : ApiOperation
    {
        private const byte MAX_NODES_COUNT = 232;
        Action<SerialApiTestResult> ResponseCallback { get; set; }
        private byte TestCmd { get; set; }
        private byte[] TestNodeMask { get; set; }
        private ushort TestDelay { get; set; }
        private byte TestPayloadLength { get; set; }
        private ushort TestCount { get; set; }
        private TransmitOptions TxOptions { get; set; }
        private bool IsStopOnErrors { get; set; }
        public SerialApiTestOperation(Action<SerialApiTestResult> responseCallback, byte testCmd, ushort testDelay, byte testPayloadLength, ushort testCount, TransmitOptions txOptions, NodeTag[] nodes, bool isStopOnErrors)
            : base(true, CommandTypes.CmdSerialApiTest, true)
        {
            TestCmd = testCmd;
            TestDelay = testDelay;
            TestPayloadLength = testPayloadLength;
            TestCount = testCount;
            TxOptions = txOptions;
            TestNodeMask = new byte[MAX_NODES_COUNT / 8];
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    TestNodeMask[(((byte)node.Id) - 1) >> 3] |= (byte)(1 << ((((byte)node.Id) - 1) & 0x07));
                }
            }
            ResponseCallback = responseCallback;
            IsStopOnErrors = isStopOnErrors;
        }

        ApiMessage message;
        ApiMessage messageStop;
        ApiHandler handlerRetFailed;
        ApiHandler handlerCallback;

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 0, message));
            ActionUnits.Add(new DataReceivedUnit(handlerRetFailed, SetStateFailed));
            ActionUnits.Add(new DataReceivedUnit(handlerCallback, OnHandle));
            StopActionUnit = new StopActionUnit(messageStop);
        }

        protected override void CreateInstance()
        {
            message = new ApiMessage(CommandTypes.CmdSerialApiTest,
                TestCmd,
                (byte)(TestDelay >> 8),
                (byte)TestDelay,
                TestPayloadLength,
                (byte)(TestCount >> 8),
                (byte)TestCount,
                (byte)TxOptions,
                (byte)TestNodeMask.Length);
            message.AddData(TestNodeMask);
            message.SetSequenceNumber(SequenceNumber);

            messageStop = new ApiMessage(CommandTypes.CmdSerialApiTest,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0);

            handlerRetFailed = new ApiHandler(CommandTypes.CmdSerialApiTest);
            handlerRetFailed.AddConditions(new ByteIndex(0));

            handlerCallback = new ApiHandler(FrameTypes.Request, CommandTypes.CmdSerialApiTest);
        }

        public void OnHandle(IActionUnit ou)
        {
            byte[] res = ((DataReceivedUnit)ou).DataFrame.Payload;
            SerialApiTestResult result = new SerialApiTestResult();
            result.TestCommand = res[1];
            result.TestState = res[2];
            if (result.TestState != 2)
            {
                SetStateCompleted(ou);
            }
            else if (result.TestCommand < 5)
            {
                result.ResOne = new ResOne();
                result.ResOne.TestNodeId = new NodeTag(res[3]);
                result.ResOne.TransmitStatus = (TransmitStatuses)res[4];
                result.ResOne.TestCount = res[5];
                if (IsStopOnErrors && result.ResOne.TransmitStatus != TransmitStatuses.CompleteOk)
                    ((DataReceivedUnit)ou).SetNextActionItems(messageStop);
            }
            else
            {
                result.ResTwo = new ResTwo();
                result.ResTwo.TestCount = (res[3] << 8) + res[4];
                if (res.Length > 6 && res.Length == (6 + res[5]))
                {
                    List<NodeTag> tmp = new List<NodeTag>();
                    for (int i = 6; i < res.Length; i++)
                    {
                        int byteIndex = i - 6;
                        byte maskByte = res[i];
                        if (maskByte == 0)
                        {
                            continue;
                        }
                        byte bitMask = 0x01;
                        byte bitOffset = 0x01;//nodes starting from 1 in mask bytes array
                        for (int j = 0; j < 8; j++)
                        {
                            if ((bitMask & maskByte) != 0)
                            {
                                byte nodeID = (byte)(((byteIndex * 8) + j) + bitOffset);
                                tmp.Add(new NodeTag(nodeID));
                            }
                            bitMask <<= 1;
                        }
                    }
                    result.ResTwo.FailedNodes = tmp.ToArray();
                    if (IsStopOnErrors && result.ResTwo.FailedNodes.Length > 0)
                        ((DataReceivedUnit)ou).SetNextActionItems(messageStop);
                }
            }
            ResponseCallback?.Invoke(result);
        }

        public SerialApiTestResult SpecificResult
        {
            get { return (SerialApiTestResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new SerialApiTestResult();
        }
    }

    public class SerialApiTestResult : ActionResult
    {
        public byte TestCommand { get; set; }
        public byte TestState { get; set; }
        public ResOne ResOne { get; set; }
        public ResTwo ResTwo { get; set; }
    }

    public class ResOne
    {
        public NodeTag TestNodeId { get; set; }
        public TransmitStatuses TransmitStatus { get; set; }
        public byte TestCount { get; set; }
    }
    public class ResTwo
    {
        public int TestCount { get; set; }
        public NodeTag[] FailedNodes { get; set; }
    }
}
