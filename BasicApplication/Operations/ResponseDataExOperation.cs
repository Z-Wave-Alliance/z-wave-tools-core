/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Collections.Generic;
using System.Linq;
using ZWave.Enums;
using Utils;
using ZWave.Devices;
using ZWave.BasicApplication.Tasks;

namespace ZWave.BasicApplication.Operations
{
    public class ResponseDataExOperation : DelayedResponseOperation
    {
        internal List<byte[]> Data { get; set; }
        internal TransmitOptions TxOptions { get; private set; }
        internal TransmitOptions2 TxOptions2 { get; private set; }
        internal TransmitSecurityOptions TxSecOptions { get; private set; }
        internal SecuritySchemes SecurityScheme { get; private set; }
        internal bool IsSecuritySchemeSpecified { get; private set; }
        public ResponseDataDelegate ReceiveCallback { get; private set; }
        public ResponseExDataDelegate ReceiveExCallback { get; private set; }
        public bool IsStopOnNak { get; private set; }

        public ResponseDataExOperation(NetworkViewPoint network, ResponseDataDelegate receiveCallback, TransmitOptions txOptions,
            TransmitSecurityOptions txSecOptions, TransmitOptions2 txOptions2, NodeTag destNode, byte cmdClass, byte cmd)
            : base(network, destNode, NodeTag.Empty, new ByteIndex(cmdClass), new ByteIndex(cmd))
        {
            ReceiveCallback = receiveCallback;
            TxOptions = txOptions;
            TxSecOptions = txSecOptions;
            TxOptions2 = txOptions2;
        }

        public ResponseDataExOperation(NetworkViewPoint network, ResponseDataDelegate receiveCallback, TransmitOptions txOptions,
            TransmitSecurityOptions txSecOptions, SecuritySchemes scheme, TransmitOptions2 txOptions2, NodeTag destNode, byte cmdClass, byte cmd)
            : base(network, destNode, NodeTag.Empty, new ByteIndex(cmdClass), new ByteIndex(cmd))
        {
            ReceiveCallback = receiveCallback;
            TxOptions = txOptions;
            TxSecOptions = txSecOptions;
            SecurityScheme = scheme;
            IsSecuritySchemeSpecified = true;
            TxOptions2 = txOptions2;
        }

        public ResponseDataExOperation(NetworkViewPoint network, byte[] data, TransmitOptions txOptions,
            TransmitSecurityOptions txSecOptions, TransmitOptions2 txOptions2, NodeTag destNode, byte cmdClass, byte cmd)
            : base(network, destNode, NodeTag.Empty, new ByteIndex(cmdClass), new ByteIndex(cmd))
        {
            Data = new List<byte[]>();
            Data.Add(data);
            TxOptions = txOptions;
            TxSecOptions = txSecOptions;
            TxOptions2 = txOptions2;
        }

        public ResponseDataExOperation(NetworkViewPoint network, byte[] data, TransmitOptions txOptions, TransmitSecurityOptions txSecOptions,
            TransmitOptions2 txOptions2, NodeTag destNode, int NumBytesToCompare
            , byte cmdClass, byte[] cmd)
            : base(network, destNode, NodeTag.Empty, cmd, NumBytesToCompare)
        {
            Data = new List<byte[]>();
            Data.Add(data);
            TxOptions = txOptions;
            TxSecOptions = txSecOptions;
            TxOptions2 = txOptions2;
        }

        public ResponseDataExOperation(NetworkViewPoint network, byte[] data, TransmitOptions txOptions,
            TransmitSecurityOptions txSecOptions, SecuritySchemes scheme, TransmitOptions2 txOptions2, NodeTag destNode, byte cmdClass, byte cmd)
            : base(network, destNode, NodeTag.Empty, new ByteIndex(cmdClass), new ByteIndex(cmd))
        {
            Data = new List<byte[]>();
            Data.Add(data);
            TxOptions = txOptions;
            TxSecOptions = txSecOptions;
            TxOptions2 = txOptions2;
            SecurityScheme = scheme;
            IsSecuritySchemeSpecified = true;
        }

        public ResponseDataExOperation(NetworkViewPoint network, ResponseExDataDelegate receiveCallback, TransmitOptions txOptions,
            TransmitSecurityOptions txSecOptions, TransmitOptions2 txOptions2, NodeTag destNode, byte cmdClass, byte cmd)
            : base(network, destNode, NodeTag.Empty, new ByteIndex(cmdClass), new ByteIndex(cmd))
        {
            ReceiveExCallback = receiveCallback;
            TxOptions = txOptions;
            TxSecOptions = txSecOptions;
            TxOptions2 = txOptions2;
        }

        public ResponseDataExOperation(NetworkViewPoint network, ResponseExDataDelegate receiveCallback, TransmitOptions txOptions,
            TransmitSecurityOptions txSecOptions, TransmitOptions2 txOptions2, NodeTag destNode, bool isStopOnNak, byte cmdClass, byte cmd)
            : base(network, destNode, NodeTag.Empty, new ByteIndex(cmdClass), new ByteIndex(cmd))
        {
            ReceiveExCallback = receiveCallback;
            TxOptions = txOptions;
            TxSecOptions = txSecOptions;
            TxOptions2 = txOptions2;
            IsStopOnNak = isStopOnNak;
        }

        public ResponseDataExOperation(NetworkViewPoint network, List<byte[]> data, TransmitOptions txOptions,
            TransmitSecurityOptions txSecOptions, SecuritySchemes scheme, TransmitOptions2 txOptions2, NodeTag destNode, byte cmdClass, byte cmd)
            : base(network, destNode, NodeTag.Empty, new ByteIndex(cmdClass), new ByteIndex(cmd))
        {
            Data = data;
            TxOptions = txOptions;
            TxSecOptions = txSecOptions;
            SecurityScheme = scheme;
            TxOptions2 = txOptions2;
            IsSecuritySchemeSpecified = true;
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
                    else
                    {
                        Data = null;
                    }
                }
                else if (ReceiveExCallback != null)
                {
                    Data = ReceiveExCallback(ReceivedAchData.Options, ReceivedAchData.DstNode, ReceivedAchData.SrcNode, ReceivedAchData.Command);
                }
                ou.SetNextActionItems();
                List<ActionBase> nextOperations = new List<ActionBase>();
                if (Data != null)
                {
                    if (DstNode.EndPointId == ReceivedAchData.DstNode.EndPointId)
                    {
                        var scheme = IsSecuritySchemeSpecified ? SecurityScheme : (SecuritySchemes)ReceivedAchData.SecurityScheme;
                        foreach (var command in Data)
                        {
                            bool isSuportedScheme = IsSupportedScheme(_network, node, command, scheme);
                            if (command != null && command.Length > 1 && isSuportedScheme)
                            {
                                CallbackApiOperation operation = null;
                                operation = new SendDataExOperation(_network, ReceivedAchData.DstNode, ReceivedAchData.SrcNode, command, TxOptions, TxSecOptions, scheme, TxOptions2);
                                operation.SubstituteSettings = new SubstituteSettings(SubstituteSettings.SubstituteFlags, SubstituteSettings.S0MaxBytesPerFrameSize);
                                if (ReceivedAchData.SubstituteIncomingFlags.HasFlag(SubstituteIncomingFlags.Crc16Encap))
                                {
                                    operation.SubstituteSettings.SetFlag(SubstituteFlags.UseCrc16Encap);
                                }
                                nextOperations.Add(operation);
                            }
                        }
                    }
                }
                if (nextOperations.Count > 0)
                {
                    var next = new SendDataGroupTask(IsStopOnNak, nextOperations.ToArray());
                    next.CompletedCallback = (x) =>
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
                    ou.SetNextActionItems(next);
                }
                else
                {
                    handlingRequestFromNode = NodeTag.Empty;
                    handlingRequest = emptyArray;
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
}
