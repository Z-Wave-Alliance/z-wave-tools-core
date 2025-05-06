/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using ZWave.Enums;
using Utils;
using ZWave.BasicApplication.Enums;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    public class RemoveNodeIdOperation : ApiOperation
    {
        public static int TIMEOUT = 60000;

        Modes mInitMode;
        readonly int TimeoutMs;
        NodeTag _node;
        internal Action<NodeStatuses> NodeStatusCallback { get; set; }
        public RemoveNodeIdOperation(NetworkViewPoint network, Modes mode, NodeTag node, Action<NodeStatuses> nodeStatusCallback, int timeoutMS)
            : base(true, CommandTypes.CmdZWaveRemoveNodeIdFromNetwork, true)
        {
            _network = network;
            mInitMode = mode;
            TimeoutMs = timeoutMS;
            _node = node;
            NodeStatusCallback = nodeStatusCallback;
            if (TimeoutMs <= 0)
                TimeoutMs = TIMEOUT;
        }

        public NodeStatuses NodeStatus { get; set; }

        private ApiMessage messageStart;
        private ApiMessage messageStop;
        private ApiMessage messageStopFailed;
        private ApiMessage messageStopDone;

        private ApiHandler handlerLearnReady;
        private ApiHandler handlerFailed;
        private ApiHandler handlerNodeFound;
        private ApiHandler handlerRemovingController;
        private ApiHandler handlerRemovingEndDevice;
        private ApiHandler handlerDone;
        private ApiHandler handlerNotPrimary;                // code=23


        protected override void CreateWorkflow()
        {

            if (mInitMode != Modes.NodeStop)
            {
                //UniqueCategory = "AddingRemovingNode";
                ActionUnits.Add(new StartActionUnit(OnStart, TimeoutMs, messageStart));
                ActionUnits.Add(new DataReceivedUnit(handlerLearnReady, OnLearnReady));
                ActionUnits.Add(new DataReceivedUnit(handlerFailed, OnFailed, messageStopDone));
                ActionUnits.Add(new DataReceivedUnit(handlerNodeFound, OnNodeFound));
                ActionUnits.Add(new DataReceivedUnit(handlerRemovingEndDevice, OnRemovingEndDevice));
                ActionUnits.Add(new DataReceivedUnit(handlerRemovingController, OnRemovingController));
                ActionUnits.Add(new DataReceivedUnit(handlerDone, OnDone, messageStopDone));
                ActionUnits.Add(new DataReceivedUnit(handlerNotPrimary, OnNotPrimary));

                StopActionUnit = new StopActionUnit(messageStop);
            }
            else
            {
                ActionUnits.Add(new StartActionUnit(null, 0, messageStop));
                ActionUnits.Add(new DataReceivedUnit(handlerDone, OnDone, messageStopDone));
            }
        }

        protected override void CreateInstance()
        {
            if (_network.IsNodeIdBaseTypeLR)
            {
                messageStart = new ApiMessage(SerialApiCommands[0], new byte[] { (byte)mInitMode, (byte)(_node.Id >> 8), (byte)_node.Id });
            }
            else
            {
                messageStart = new ApiMessage(SerialApiCommands[0], new byte[] { (byte)mInitMode, (byte)_node.Id });
            }
            messageStart.SetSequenceNumber(SequenceNumber);

            messageStop = new ApiMessage(SerialApiCommands[0], new byte[] { (byte)Modes.NodeStop });
            messageStop.SetSequenceNumber(SequenceNumber);

            messageStopFailed = new ApiMessage(SerialApiCommands[0], new byte[] { (byte)Modes.NodeStopFailed });
            messageStopFailed.SetSequenceNumber(0); //NULL FuncId

            messageStopDone = new ApiMessage(SerialApiCommands[0], new byte[] { (byte)Modes.NodeStop });
            messageStopDone.SetSequenceNumber(0); //NULL FuncId

            handlerDone = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            handlerDone.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)NodeStatuses.Done));

            handlerNodeFound = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            handlerNodeFound.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)NodeStatuses.NodeFound));

            handlerRemovingController = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            handlerRemovingController.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)NodeStatuses.AddingRemovingController));

            handlerRemovingEndDevice = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            handlerRemovingEndDevice.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)NodeStatuses.AddingRemovingEndDevice));

            handlerLearnReady = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            handlerLearnReady.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)NodeStatuses.LearnReady));

            handlerFailed = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            handlerFailed.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)NodeStatuses.Failed));

            handlerNotPrimary = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            handlerNotPrimary.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)NodeStatuses.NotPrimary));
        }

        private void OnStart(StartActionUnit ou)
        {
            //mController.ResetNodeStatusSignals();
        }

        private void OnNotPrimary(DataReceivedUnit ou)
        {
            ParseHandler(ou);
            base.SetStateFailed(ou);
        }

        private void OnFailed(DataReceivedUnit ou)
        {
            ParseHandler(ou);
            base.SetStateFailed(ou);
        }

        private void OnRemovingController(DataReceivedUnit ou)
        {
            ParseHandler(ou);
        }

        private void OnNodeFound(DataReceivedUnit ou)
        {
            ParseHandler(ou);
        }

        private void OnRemovingEndDevice(DataReceivedUnit ou)
        {
            ParseHandler(ou);
        }

        private void OnLearnReady(DataReceivedUnit ou)
        {
            ParseHandler(ou);
        }

        private void OnDone(DataReceivedUnit ou)
        {
            ParseHandler(ou);
            base.SetStateCompleting(ou);
        }

        private void ParseHandler(DataReceivedUnit ou)
        {
            byte[] res = ou.DataFrame.Payload;
            SequenceNumber = res[0];

            NodeStatus = (NodeStatuses)res[1];
            NodeStatusCallback(NodeStatus);
            if (NodeStatus == NodeStatuses.AddingRemovingController || NodeStatus == NodeStatuses.AddingRemovingEndDevice)
            {
                SpecificResult.FillFromRaw(res, _network.IsNodeIdBaseTypeLR);
            }
        }

        public override string AboutMe()
        {
            return string.Format("Id={0}", SpecificResult.Id);
        }

        public AddRemoveNodeResult SpecificResult
        {
            get { return (AddRemoveNodeResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new AddRemoveNodeResult();
        }
    }
}
