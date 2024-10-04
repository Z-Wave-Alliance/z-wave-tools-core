/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using ZWave.BasicApplication.Enums;
using ZWave.Enums;
using Utils;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    public class CreateNewPrimaryCtrlOperation : ApiOperation
    {
        public static int TIMEOUT = 60000;

        public CreatePrimaryModes InitMode { get; set; }
        internal int TimeoutMs { get; set; }
        internal Action<NodeStatuses> NodeStatusCallback { get; set; } 
        public CreateNewPrimaryCtrlOperation(NetworkViewPoint network, CreatePrimaryModes mode, Action<NodeStatuses> nodeStatusCallback, int timeoutMs)
            : base(true, CommandTypes.CmdZWaveCreateNewPrimary, true)
        {
            _network = network;
            InitMode = mode;
            TimeoutMs = timeoutMs;
            NodeStatusCallback = nodeStatusCallback;
            if (TimeoutMs <= 0)
                TimeoutMs = TIMEOUT; 
        }

        public NodeStatuses NodeStatus { get; set; }

        private ApiMessage messageStart;
        private ApiMessage messageStop;
        private ApiMessage messageStopFailed;

        private ApiHandler handlerLearnReady;
        private ApiHandler handlerFailed;              // code=7
        private ApiHandler handlerNodeFound;
        private ApiHandler handlerAddingController;
        private ApiHandler handlerAddingEndDevice;
        private ApiHandler handlerProtocolDone;        // code=5
        private ApiHandler handlerDone;                // code=6 
        private ApiHandler handlerNotPrimary;                // code=23


        protected override void CreateWorkflow()
        {
            if (InitMode == CreatePrimaryModes.Start)
            {
                ActionUnits.Add(new StartActionUnit(null, TimeoutMs, messageStart));
                ActionUnits.Add(new DataReceivedUnit(handlerLearnReady, OnLearnReady));
                ActionUnits.Add(new DataReceivedUnit(handlerFailed, OnFailed, messageStopFailed));
                ActionUnits.Add(new DataReceivedUnit(handlerNodeFound, OnNodeFound));
                ActionUnits.Add(new DataReceivedUnit(handlerAddingController, OnAddingController));
                ActionUnits.Add(new DataReceivedUnit(handlerAddingEndDevice, OnAddingEndDevice));
                ActionUnits.Add(new DataReceivedUnit(handlerProtocolDone, OnProtocolDone, messageStop));
                ActionUnits.Add(new DataReceivedUnit(handlerDone, OnDone));
                ActionUnits.Add(new DataReceivedUnit(handlerNotPrimary, OnNotPrimary));
            }
            else if (InitMode == CreatePrimaryModes.Stop)
            {
                ActionUnits.Add(new StartActionUnit(null, 0, messageStop));
                ActionUnits.Add(new DataReceivedUnit(handlerDone, OnDone));
            }
            else
                base.SetStateCompleted(null);
        }

        protected override void CreateInstance()
        {
            messageStart = new ApiMessage(SerialApiCommands[0], new byte[] { (byte)InitMode });
            messageStart.SetSequenceNumber(SequenceNumber);

            messageStop = new ApiMessage(SerialApiCommands[0], new byte[] { (byte)CreatePrimaryModes.Stop });
            messageStop.SetSequenceNumber(SequenceNumber);

            messageStopFailed = new ApiMessage(SerialApiCommands[0], new byte[] { (byte)CreatePrimaryModes.StopFailed });
            messageStopFailed.SetSequenceNumber(0);//remove SequenceNumber (use zero) from payload

            handlerProtocolDone = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            handlerProtocolDone.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)NodeStatuses.ProtocolDone));

            handlerDone = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            handlerDone.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)NodeStatuses.Done));

            handlerNodeFound = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            handlerNodeFound.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)NodeStatuses.NodeFound));

            handlerAddingController = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            handlerAddingController.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)NodeStatuses.AddingRemovingController));

            handlerAddingEndDevice = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            handlerAddingEndDevice.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)NodeStatuses.AddingRemovingEndDevice));

            handlerLearnReady = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            handlerLearnReady.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)NodeStatuses.LearnReady));

            handlerFailed = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            handlerFailed.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)NodeStatuses.Failed));

            handlerNotPrimary = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            handlerNotPrimary.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)NodeStatuses.NotPrimary));
        }

        private void OnNotPrimary(DataReceivedUnit ou)
        {
            ParseHandler(ou);
            base.SetStateFailed(ou);
        }

        private void OnNodeFound(DataReceivedUnit ou)
        {
            ParseHandler(ou);
        }

        private void OnFailed(DataReceivedUnit ou)
        {
            ParseHandler(ou);
            base.SetStateFailed(ou);
        }

        private void OnAddingController(DataReceivedUnit ou)
        {
            ParseHandler(ou);
        }

        private void OnAddingEndDevice(DataReceivedUnit ou)
        {
            ParseHandler(ou);
        }

        private void OnLearnReady(DataReceivedUnit ou)
        {
            ParseHandler(ou);
        }

        private void OnProtocolDone(DataReceivedUnit ou)
        {
            ParseHandler(ou);
        }

        private void OnDone(DataReceivedUnit ou)
        {
            //((ApiHandler)ou.CommandHandler).ReceivedFrame.IsHandled = true;
            ParseHandler(ou);
            base.SetStateCompleted(ou);
        }

        protected void ParseHandler(DataReceivedUnit ou)
        {
            byte[] res = ou.DataFrame.Payload;
            SequenceNumber = res[0];
            NodeStatus = (NodeStatuses)res[1];

            if (NodeStatus == NodeStatuses.AddingRemovingController || NodeStatus == NodeStatuses.AddingRemovingEndDevice)
            {
                SpecificResult.FillFromRaw(res, _network.IsNodeIdBaseTypeLR);
            }
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
