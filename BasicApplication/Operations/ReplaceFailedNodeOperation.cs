/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using ZWave.BasicApplication.Enums;
using ZWave.Enums;
using Utils;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0x63 | nodeID | funcID
    /// ZW->HOST: RES | 0x63 | retVal
    /// ZW->HOST: REQ | 0x63 | funcID | txStatus
    /// </summary>
    public class ReplaceFailedNodeOperation : ApiOperation, IAddRemoveNode
    {
        public byte[] DskValue { get; set; }
        public NetworkKeyS2Flags GrantSchemesValue { get; set; }
        public byte[] Args { get; set; }
        internal int TimeoutMs { get; set; }
        public NodeTag ReplacedNode { get; private set; }
        public Action<FailedNodeStatuses> FailedNodeStatusCallback { get; set; }

        public ReplaceFailedNodeOperation(NetworkViewPoint network, NodeTag node)
            : base(true, CommandTypes.CmdZWaveReplaceFailedNode, true)
        {
            _network = network;
            ReplacedNode = node;
        }

        public ReplaceFailedNodeOperation(NetworkViewPoint network, NodeTag node, Action<FailedNodeStatuses> failedNodeStatusCallback, int timeoutMs, params byte[] args)
            : base(true, CommandTypes.CmdZWaveReplaceFailedNode, true)
        {
            _network = network;
            Args = args;
            TimeoutMs = timeoutMs;
            ReplacedNode = node;
            FailedNodeStatusCallback = failedNodeStatusCallback;
        }


        public FailedNodeStatuses FailedNodeStatus { get; set; }
        public FailedNodeRetValues FailedNodeRetValues { get; set; }

        private ApiMessage messageStart;
        private ApiMessage messageStopDone;

        private ApiHandler handlerRemoveStarted;
        private ApiHandler handlerNotPrimary;
        private ApiHandler handlerNoCallback;
        private ApiHandler handlerNotFound;
        private ApiHandler handlerRemoveBusy;
        private ApiHandler handlerRemoveFail;

        private ApiHandler handlerNodeOk;
        private ApiHandler handlerReplace;
        private ApiHandler handlerReplaceDone;
        private ApiHandler handlerReplaceFail;

        private ApiHandler handlerDone;                // If operation cancelled with AddNode.Stop 


        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, TimeoutMs, messageStart));
            ActionUnits.Add(new DataReceivedUnit(handlerRemoveStarted, OnRetValue));
            ActionUnits.Add(new DataReceivedUnit(handlerNotPrimary, OnRetValue));
            ActionUnits.Add(new DataReceivedUnit(handlerNoCallback, OnRetValue));
            ActionUnits.Add(new DataReceivedUnit(handlerNotFound, OnRetValue));
            ActionUnits.Add(new DataReceivedUnit(handlerRemoveBusy, OnRetValue));
            ActionUnits.Add(new DataReceivedUnit(handlerRemoveFail, OnRetValue));

            ActionUnits.Add(new DataReceivedUnit(handlerNodeOk, OnCallback));
            ActionUnits.Add(new DataReceivedUnit(handlerReplace, OnCallback));
            ActionUnits.Add(new DataReceivedUnit(handlerReplaceDone, OnCallback));
            ActionUnits.Add(new DataReceivedUnit(handlerReplaceFail, OnCallback));

            ActionUnits.Add(new DataReceivedUnit(handlerDone, OnDone));

            StopActionUnit = new StopActionUnit(messageStopDone);
        }

        protected override void CreateInstance()
        {
            byte[] args = null;
            if (Args != null && Args.Length > 0)
            {
                args = new byte[Args.Length + 2];
                args[0] = (byte)ReplacedNode.Id;
                Array.Copy(Args, 0, args, 2, Args.Length);
                if (_network.IsNodeIdBaseTypeLR)
                {
                    args = new byte[Args.Length + 3];
                    args[0] = (byte)(ReplacedNode.Id >> 8);
                    args[1] = (byte)ReplacedNode.Id;
                    Array.Copy(Args, 0, args, 3, Args.Length);
                }
                messageStart = new ApiMessage(SerialApiCommands[0], args);
                messageStart.SequenceNumberCustomIndex = 3;
            }
            else
            {
                args = new byte[1];
                args[0] = (byte)ReplacedNode.Id;
                if (_network.IsNodeIdBaseTypeLR)
                {
                    args = new byte[2];
                    args[0] = (byte)(ReplacedNode.Id >> 8);
                    args[1] = (byte)ReplacedNode.Id;
                }
                messageStart = new ApiMessage(SerialApiCommands[0], args);
            }

            messageStart.SetSequenceNumber(SequenceNumber);

            messageStopDone = new ApiMessage(CommandTypes.CmdZWaveAddNodeToNetwork, new byte[] { (byte)Modes.NodeStop });
            messageStopDone.SetSequenceNumber(0); //NULL funcID = 0

            handlerRemoveStarted = new ApiHandler(SerialApiCommands[0]);
            handlerRemoveStarted.AddConditions(new ByteIndex((byte)FailedNodeRetValues.ZW_FAILED_NODE_REMOVE_STARTED));

            handlerNotPrimary = new ApiHandler(SerialApiCommands[0]);
            handlerNotPrimary.AddConditions(new ByteIndex((byte)FailedNodeRetValues.ZW_NOT_PRIMARY_CONTROLLER));

            handlerNoCallback = new ApiHandler(SerialApiCommands[0]);
            handlerNoCallback.AddConditions(new ByteIndex((byte)FailedNodeRetValues.ZW_NO_CALLBACK_FUNCTION));

            handlerNotFound = new ApiHandler(SerialApiCommands[0]);
            handlerNotFound.AddConditions(new ByteIndex((byte)FailedNodeRetValues.ZW_FAILED_NODE_NOT_FOUND));

            handlerRemoveBusy = new ApiHandler(SerialApiCommands[0]);
            handlerRemoveBusy.AddConditions(new ByteIndex((byte)FailedNodeRetValues.ZW_FAILED_NODE_REMOVE_PROCESS_BUSY));

            handlerRemoveFail = new ApiHandler(SerialApiCommands[0]);
            handlerRemoveFail.AddConditions(new ByteIndex((byte)FailedNodeRetValues.ZW_FAILED_NODE_REMOVE_FAIL));

            handlerNodeOk = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            handlerNodeOk.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)FailedNodeStatuses.NodeOk));

            handlerReplace = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            handlerReplace.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)FailedNodeStatuses.NodeReplace));

            handlerReplaceDone = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            handlerReplaceDone.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)FailedNodeStatuses.NodeReplaceDone));

            handlerReplaceFail = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            handlerReplaceFail.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)FailedNodeStatuses.NodeReplaceFailed));

            handlerDone = new ApiHandler(FrameTypes.Request, CommandTypes.CmdZWaveAddNodeToNetwork);
            handlerDone.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)NodeStatuses.Done));
        }

        public AddRemoveNodeResult SpecificResult
        {
            get { return (AddRemoveNodeResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new AddRemoveNodeResult();
        }

        private void OnRetValue(DataReceivedUnit ou)
        {
            FailedNodeRetValues = (FailedNodeRetValues)ou.DataFrame.Payload[0];
            if (FailedNodeRetValues != FailedNodeRetValues.ZW_FAILED_NODE_REMOVE_STARTED)
            {
                SetStateFailed(ou);
            }
        }

        private void OnCallback(DataReceivedUnit ou)
        {
            FailedNodeStatus = (FailedNodeStatuses)ou.DataFrame.Payload[1];
            if (FailedNodeStatus != FailedNodeStatuses.NodeReplace && FailedNodeStatus != FailedNodeStatuses.NodeReplaceDone)
            {
                base.SetStateFailed(ou);
            }
            FailedNodeStatusCallback?.Invoke(FailedNodeStatus);
            if (FailedNodeStatus == FailedNodeStatuses.NodeReplaceDone)
            {
                SpecificResult.Node = ReplacedNode;
                base.SetStateCompleted(ou);
            }
        }

        //Operation cancelled with AddNode.Stop 
        private void OnDone(DataReceivedUnit ou)
        {
            base.SetStateFailed(ou);
        }
    }
}
