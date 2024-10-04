/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Collections.Generic;
using System.Linq;
using ZWave.Enums;
using Utils;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    public delegate byte[] ResponseDataDelegate(ReceiveStatuses options, NodeTag destNodeId, NodeTag srcNodeId, byte[] data);
    public delegate List<byte[]> ResponseAchDataDelegate(AchData achData);
    public delegate List<byte[]> ResponseExDataDelegate(ReceiveStatuses options, NodeTag destNodeId, NodeTag srcNodeId, byte[] data);
    public class ResponseDataOperation : DelayedResponseOperation
    {
        internal List<byte[]> Data { get; set; }
        internal TransmitOptions TxOptions { get; private set; }
        public ResponseDataDelegate ReceiveCallback { get; private set; }
        public ResponseAchDataDelegate ReceiveAchDataCallback { get; private set; }
        public ResponseExDataDelegate ReceiveExCallback { get; private set; }

        public ResponseDataOperation(NetworkViewPoint network, ResponseAchDataDelegate receiveAchDataCallback, TransmitOptions txOptions, NodeTag srcNode, byte cmdClass, byte cmd)
            : base(network, NodeTag.Empty, srcNode, new ByteIndex(cmdClass), new ByteIndex(cmd))
        {
            ReceiveAchDataCallback = receiveAchDataCallback;
            TxOptions = txOptions;
        }

        public ResponseDataOperation(NetworkViewPoint network, ResponseDataDelegate receiveCallback, TransmitOptions txOptions, NodeTag srcNode, byte cmdClass, byte cmd)
            : base(network, NodeTag.Empty, srcNode, new ByteIndex(cmdClass), new ByteIndex(cmd))
        {
            ReceiveCallback = receiveCallback;
            TxOptions = txOptions;
        }

        public ResponseDataOperation(NetworkViewPoint network, NodeTag dstNode, byte[] data, TransmitOptions txOptions, NodeTag srcNode, byte cmdClass, byte cmd)
            : base(network, dstNode, srcNode, new ByteIndex(cmdClass), new ByteIndex(cmd))
        {
            Data = new List<byte[]>();
            Data.Add(data);
            TxOptions = txOptions;
        }

        public ResponseDataOperation(NetworkViewPoint network, ResponseExDataDelegate receiveCallback, TransmitOptions txOptions, NodeTag srcNode, byte cmdClass, byte cmd)
            : base(network, NodeTag.Empty, srcNode, new ByteIndex(cmdClass), new ByteIndex(cmd))
        {
            ReceiveExCallback = receiveCallback;
            TxOptions = txOptions;
        }

        public ResponseDataOperation(NetworkViewPoint network, List<byte[]> data, TransmitOptions txOptions, NodeTag srcNode, byte cmdClass, byte cmd)
            : base(network, NodeTag.Empty, srcNode, new ByteIndex(cmdClass), new ByteIndex(cmd))
        {
            Data = data;
            TxOptions = txOptions;
        }

        NodeTag handlingRequestFromNode = NodeTag.Empty;
        static readonly byte[] emptyArray = new byte[0];
        byte[] handlingRequest = emptyArray;
        protected override void OnHandledDelayed(DataReceivedUnit ou)
        {
            var node = ReceivedAchData.SrcNode;
            byte[] cmd = ReceivedAchData.Command;
            if (handlingRequestFromNode != node || !handlingRequest.SequenceEqual(cmd))
            {
                handlingRequestFromNode = node;
                handlingRequest = cmd;
                if (ReceiveCallback != null)
                {
                    byte[] data = ReceiveCallback(ReceivedAchData.Options, ReceivedAchData.DstNode, ReceivedAchData.SrcNode, ReceivedAchData.Command);
                    if (data != null && data.Length > 0)
                    {
                        Data = new List<byte[]>();
                        Data.Add(data);
                    }
                }
                else if (ReceiveExCallback != null)
                {
                    Data = ReceiveExCallback(ReceivedAchData.Options, ReceivedAchData.DstNode, ReceivedAchData.SrcNode, ReceivedAchData.Command);
                }
                else if (ReceiveAchDataCallback != null)
                {
                    Data = ReceiveAchDataCallback(ReceivedAchData);
                }
                if (Data != null && Data.Count > 0)
                {
                    if (DstNode.EndPointId == ReceivedAchData.DstNode.EndPointId)
                    {
                        ApiOperation[] operations = new ApiOperation[Data.Count];
                        if (ReceivedAchData.DstNode.Id == 0)
                        {
                            for (int i = 0; i < Data.Count; i++)
                            {
                                operations[i] = new SendDataOperation(_network, ReceivedAchData.SrcNode, Data[i], TxOptions);
                                operations[i].SubstituteSettings = SubstituteSettings;
                                operations[i].CompletedCallback = (x) =>
                                {
                                    var action = x as ActionBase;
                                    if (action != null)
                                    {
                                        handlingRequestFromNode = NodeTag.Empty;
                                        handlingRequest = emptyArray;
                                        SpecificResult.TotalCount++;
                                        if (action.Result.State != ActionStates.Completed)
                                            SpecificResult.FailCount++;
                                    }
                                };
                            }
                        }
                        else
                        {
                            for (int i = 0; i < Data.Count; i++)
                            {
                                operations[i] = new SendDataBridgeOperation(_network, ReceivedAchData.DstNode, ReceivedAchData.SrcNode, Data[i], TxOptions);
                                operations[i].SubstituteSettings = SubstituteSettings;
                                operations[i].CompletedCallback = (x) =>
                                {
                                    var action = x as ActionBase;
                                    if (action != null)
                                    {
                                        handlingRequestFromNode = NodeTag.Empty;
                                        handlingRequest = emptyArray;
                                        SpecificResult.TotalCount++;
                                        if (action.Result.State != ActionStates.Completed)
                                            SpecificResult.FailCount++;
                                    }
                                };
                            }
                        }
                        if (operations.Length > 1)
                        {
                            ou.SetNextActionItems(new ActionSerialGroup(operations));
                        }
                        else
                        {
                            ou.SetNextActionItems(operations);
                        }
                    }
                }
                else
                {
                    handlingRequestFromNode = NodeTag.Empty;
                    handlingRequest = emptyArray;
                    ou.SetNextActionItems();
                }
            }
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
