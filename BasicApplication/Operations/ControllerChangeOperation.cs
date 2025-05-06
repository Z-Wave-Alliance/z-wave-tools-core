/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using ZWave.BasicApplication.Enums;
using ZWave.Enums;
using Utils;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    public class ControllerChangeOperation : ApiOperation
    {
        public static int TIMEOUT = 60000;
        private bool IsModeStopEnabled { get; set; }
        public ControllerChangeModes InitMode { get; set; }
        internal int TimeoutMs { get; set; }
        internal Action<NodeStatuses> NodeStatusCallback { get; set; }
        public ControllerChangeOperation(NetworkViewPoint network, ControllerChangeModes mode, Action<NodeStatuses> nodeStatusCallback, int timeoutMs)
            : this(network, mode, nodeStatusCallback, true, timeoutMs)
        {
        }
        public ControllerChangeOperation(NetworkViewPoint network, ControllerChangeModes mode, Action<NodeStatuses> nodeStatusCallback, bool isModeStopEnabled, int timeoutMs)
        : base(true, CommandTypes.CmdZWaveControllerChange, true)
        {
            _network = network;
            IsModeStopEnabled = isModeStopEnabled;
            IsExclusive = IsModeStopEnabled;
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
        private ApiMessage messageStopDone;

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
            if ((InitMode & ControllerChangeModes.Start) == ControllerChangeModes.Start)
            {
                ActionUnits.Add(new StartActionUnit(null, TimeoutMs, messageStart));
                ActionUnits.Add(new DataReceivedUnit(handlerLearnReady, OnLearnReady));
                ActionUnits.Add(new DataReceivedUnit(handlerNodeFound, OnNodeFound));
                ActionUnits.Add(new DataReceivedUnit(handlerAddingController, OnAddingController));
                ActionUnits.Add(new DataReceivedUnit(handlerAddingEndDevice, OnAddingEndDevice));
                if (IsModeStopEnabled)
                {
                    ActionUnits.Add(new DataReceivedUnit(handlerFailed, OnFailed, messageStopFailed));
                    ActionUnits.Add(new DataReceivedUnit(handlerProtocolDone, OnProtocolDone, messageStop));
                    ActionUnits.Add(new DataReceivedUnit(handlerDone, OnDone, messageStopDone));
                    ActionUnits.Add(new DataReceivedUnit(handlerNotPrimary, OnNotPrimary, messageStopDone));

                    StopActionUnit = new StopActionUnit(messageStopDone);
                }
                else
                {
                    ActionUnits.Add(new DataReceivedUnit(handlerFailed, OnFailed));
                    ActionUnits.Add(new DataReceivedUnit(handlerProtocolDone, OnDone));
                    ActionUnits.Add(new DataReceivedUnit(handlerNotPrimary, OnNotPrimary));
                }
            }
            else
            {
                ActionUnits.Add(new StartActionUnit(SetStateCompleting, 0, messageStopDone));
            }
        }

        protected override void CreateInstance()
        {
            messageStart = new ApiMessage(SerialApiCommands[0], new byte[] { (byte)InitMode });
            messageStart.SetSequenceNumber(SequenceNumber);

            messageStop = new ApiMessage(SerialApiCommands[0], new byte[] { (byte)ControllerChangeModes.Stop });
            messageStop.SetSequenceNumber(SequenceNumber);

            messageStopFailed = new ApiMessage(SerialApiCommands[0], new byte[] { (byte)ControllerChangeModes.StopFailed });
            messageStopFailed.SetSequenceNumber(0); //remove SequenceNumber from payload

            messageStopDone = new ApiMessage(SerialApiCommands[0], new byte[] { (byte)ControllerChangeModes.Stop });
            messageStopDone.SetSequenceNumber(0); //NULL funcID = 0

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
            base.SetStateFailing(ou);
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
                if (_network.SucNodeId > 0 &&
                    _network.SucNodeId != _network.NodeTag.Id)
                {
                    SpecificResult.AddRemoveNodeStatus = AddRemoveNodeStatuses.Shifted;
                }
                else
                {
                    SpecificResult.AddRemoveNodeStatus = AddRemoveNodeStatuses.Added;
                }
            }
            NodeStatusCallback(NodeStatus);
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
