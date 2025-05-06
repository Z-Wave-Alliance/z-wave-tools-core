/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using ZWave.BasicApplication.Operations;
using ZWave.CommandClasses;
using ZWave.Devices;
using ZWave.Enums;

namespace ZWave.BasicApplication.CommandClasses
{
    public class InclusionControllerSupport : DelayedResponseOperation
    {
        public InclusionControllerSupport(NetworkViewPoint network)
            : base(network, NodeTag.Empty, NodeTag.Empty, null)
        {
        }
    }

    public class InclusionController
    {
        public class Initiate : RequestDataOperation
        {
            private static byte[] _dataToSend = new COMMAND_CLASS_INCLUSION_CONTROLLER.INITIATE();
            public NodeTag Node { get; private set; }
            public byte StepId { get; private set; }

            public Initiate(NetworkViewPoint network, NodeTag srcNode, NodeTag destNode, TransmitOptions txOptions, int timeoutMs)
                : base(network, srcNode, destNode, _dataToSend, txOptions, new COMMAND_CLASS_INCLUSION_CONTROLLER.COMPLETE(), 2, timeoutMs)
            {
            }

            public void SetCommandParameters(NodeTag node, byte stepId)
            {
                Node = node;
                StepId = stepId;
                var internalData = (COMMAND_CLASS_INCLUSION_CONTROLLER.INITIATE)Data;
                internalData.nodeId = (byte)node.Id;
                internalData.stepId = stepId;
                Data = internalData;
            }

            protected override void SetStateCompleted(IActionUnit ou)
            {
                base.SetStateCompleted(ou);
            }
        }

        public class Complete : ApiOperation
        {
            internal int TimeoutMs { get; set; }
            internal NodeTag SrcNode { get; set; }
            internal NodeTag DstNode { get; set; }
            internal byte[] Data { get; set; }
            internal TransmitOptions TxOptions { get; set; }
            public byte StatusComplete { get; private set; }
            public byte StepId { get; private set; }
            public Action SendDataSubstituteCallback { get; set; }

            public Complete(NetworkViewPoint network, NodeTag srcNode, NodeTag dstNode, TransmitOptions txOptions, int timeoutMs)
                : base(false, null, false)
            {
                _network = network;
                TimeoutMs = timeoutMs;
                SrcNode = srcNode;
                DstNode = dstNode;
                Data = new COMMAND_CLASS_INCLUSION_CONTROLLER.COMPLETE();
                TxOptions = txOptions;
            }

            public void SetCommandParameters(byte statusComplete, byte stepId)
            {
                StatusComplete = statusComplete;
                StepId = stepId;
                var internalData = (COMMAND_CLASS_INCLUSION_CONTROLLER.COMPLETE)Data;
                internalData.status = statusComplete;
                internalData.stepId = stepId;
                Data = internalData;
            }

            private SendDataOperation _sendData;
            private SendDataBridgeOperation _sendDataBridge;

            protected override void CreateWorkflow()
            {
                if (SrcNode.Id == 0)
                {
                    ActionUnits.Add(new StartActionUnit(OnStart, 0, _sendData));
                    ActionUnits.Add(new ActionCompletedUnit(_sendData, OnSendDataComleted));
                }
                else
                {
                    ActionUnits.Add(new StartActionUnit(OnStart, 0, _sendDataBridge));
                    ActionUnits.Add(new ActionCompletedUnit(_sendDataBridge, OnSendDataComleted));
                }
            }

            protected virtual void OnStart(StartActionUnit ou)
            {
                _sendData.SrcNode = SrcNode;
                _sendData.DstNode = DstNode;
                _sendData.Data = Data;
                _sendData.DataDelay = DataDelay;
                _sendData.TimeoutMs = TimeoutMs;

                _sendDataBridge.SrcNode = SrcNode;
                _sendDataBridge.DstNode = DstNode;
                _sendDataBridge.Data = Data;
                _sendDataBridge.DataDelay = DataDelay;
                _sendDataBridge.TimeoutMs = TimeoutMs;
            }

            private void OnSendDataComleted(ActionCompletedUnit ou)
            {
                AddTraceLogItems(ou.Action.Result.TraceLog);
                SpecificResult.CopyFrom(ou.Action.Result as SendDataResult);
                if (ou.Action.Result.State == ActionStates.Completed)
                {
                    SetStateCompleted(ou);
                }
                else
                {
                    SetStateFailed(ou);
                }
            }

            protected override void CreateInstance()
            {
                _sendData = new SendDataOperation(_network, SrcNode, DstNode, Data, TxOptions);
                _sendData.DataDelay = DataDelay;
                _sendData.SubstituteCallback = SendDataSubstituteCallback;
                _sendData.SubstituteSettings = SubstituteSettings;

                _sendDataBridge = new SendDataBridgeOperation(_network, SrcNode, DstNode, Data, TxOptions);
                _sendDataBridge.DataDelay = DataDelay;
                _sendDataBridge.SubstituteCallback = SendDataSubstituteCallback;
                _sendDataBridge.SubstituteSettings = SubstituteSettings;
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
    }
}
