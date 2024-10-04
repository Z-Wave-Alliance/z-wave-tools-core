/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using Utils;
using ZWave.BasicApplication.Enums;
using ZWave.Devices;
using ZWave.Enums;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0xA9 | srcNodeID | destNodeID | dataLength | pData[ ] | txOptions | pRoute[4] | funcID
    /// ZW->HOST: RES | 0xA9 | RetVal
    /// ZW->HOST: REQ | 0xA9 | funcID | txStatus
    /// 
    /// WARNING: Use pRoute[4] equal [0,0,0,0].
    /// </summary>
    public class SendDataBridgeOperation : CallbackApiOperation, ISendDataAction
    {
        internal NodeTag SrcNode { get; set; }
        public NodeTag DstNode { get; set; }
        public byte[] Data { get; set; }
        internal bool IsFollowup { get; set; }
        internal TransmitOptions TxOptions { get; private set; }
        public Action SubstituteCallback { get; set; }
        public object Extensions { get; set; }
        public SendDataBridgeOperation(NetworkViewPoint network, NodeTag srcNode, NodeTag dstNode, byte[] data, TransmitOptions txOptions)
            : base(CommandTypes.CmdZWaveSendData_Bridge, CommandTypes.CmdZWaveSendDataAbort)
        {
            _network = network;
            SrcNode = srcNode;
            DstNode = dstNode;
            Data = data;
            TxOptions = txOptions;
        }

        private ApiMessage messageSendDataAbort;

        protected override void CreateWorkflow()
        {
            base.CreateWorkflow();
            StopActionUnit = new StopActionUnit(messageSendDataAbort);
        }

        protected override void CreateInstance()
        {
            base.CreateInstance();
            TimeoutMs = SubstituteSettings.CallbackWaitTimeoutMs;
            SpecificResult.TransmitStatus = TransmitStatuses.ResMissing;
            messageSendDataAbort = new ApiMessage(CommandTypes.CmdZWaveSendDataAbort);
        }

        protected override byte[] CreateInputParameters()
        {
            byte[] ret = new byte[8 + Data.Length];
            ret[0] = (byte)SrcNode.Id;
            ret[1] = (byte)DstNode.Id;
            ret[2] = (byte)Data.Length;
            for (int i = 0; i < Data.Length; i++)
            {
                ret[i + 3] = Data[i];
            }
            ret[3 + Data.Length] = (byte)TxOptions;
            ret[4 + Data.Length] = 0x00;
            ret[5 + Data.Length] = 0x00;
            ret[6 + Data.Length] = 0x00;
            ret[7 + Data.Length] = 0x00;
            if (_network.IsNodeIdBaseTypeLR)
            {
                ret = new byte[10 + Data.Length];
                ret[0] = (byte)(SrcNode.Id >> 8);
                ret[1] = (byte)SrcNode.Id;
                ret[2] = (byte)(DstNode.Id >> 8);
                ret[3] = (byte)DstNode.Id;
                ret[4] = (byte)Data.Length;
                for (int i = 0; i < Data.Length; i++)
                {
                    ret[i + 5] = Data[i];
                }
                ret[5 + Data.Length] = (byte)TxOptions;
                ret[6 + Data.Length] = 0x00;
                ret[7 + Data.Length] = 0x00;
                ret[8 + Data.Length] = 0x00;
                ret[9 + Data.Length] = 0x00;
            }
            return ret;
        }

        protected override void OnHandled(DataReceivedUnit ou)
        {
            if (SubstituteSettings.IsSkipWaitingSendCallbackEnabled)
            {
                SetStateCompleted(ou);
            }
        }

        protected override void OnCallbackInternal(DataReceivedUnit ou)
        {
            var payload = ou.DataFrame.Payload;
            if (payload != null && payload.Length > 1)
            {
                SpecificResult.FuncId = payload[0];
                SpecificResult.TransmitStatus = (TransmitStatuses)payload[1];
                int index = 3;
                if (payload.Length > index)
                {
                    SpecificResult.HasTxTransmitReport = true;
                    SpecificResult.TransmitTicks = (ushort)((payload[index - 1] << 8) + payload[index]);
                    index++;
                    if (payload.Length > index)
                    {
                        SpecificResult.RepeatersCount = payload[index];
                        index += 5;
                        if (payload.Length > index)
                        {
                            SpecificResult.RssiValuesIncoming = new sbyte[] { (sbyte)payload[index - 4], (sbyte)payload[index - 3], (sbyte)payload[index - 2], (sbyte)payload[index - 1], (sbyte)payload[index] };
                            index++;
                            if (payload.Length > index)
                            {
                                SpecificResult.AckChannelNo = payload[index];
                                index++;
                                if (payload.Length > index)
                                {
                                    SpecificResult.LastTxChannelNo = payload[index];
                                    index++;
                                    if (payload.Length > index)
                                    {
                                        SpecificResult.RouteScheme = (RoutingSchemes)payload[index];
                                        index += 4;
                                        if (payload.Length > index)
                                        {
                                            SpecificResult.Repeaters = new byte[]
                                            {
                                                payload[index - 3],
                                                payload[index - 2],
                                                payload[index - 1],
                                                payload[index]
                                            };
                                            index++;
                                            if (payload.Length > index)
                                            {
                                                SpecificResult.RouteSpeed = payload[index];
                                                index++;
                                                if (payload.Length > index)
                                                {
                                                    SpecificResult.RouteTries = payload[index];
                                                    index++;
                                                    if (payload.Length > index)
                                                    {
                                                        SpecificResult.LastFailedLinkFrom = payload[index];
                                                        index++;
                                                        if (payload.Length > index)
                                                        {
                                                            SpecificResult.LastFailedLinkTo = payload[index];
                                                            index++;
                                                            if (payload.Length > index)
                                                            {
                                                                SpecificResult.UsedTxpower = (sbyte)payload[index];
                                                                index++;
                                                                if (payload.Length > index)
                                                                {
                                                                    SpecificResult.MeasuredNoiseFloor = (sbyte)payload[index];
                                                                    index++;
                                                                    if (payload.Length > index)
                                                                    {
                                                                        SpecificResult.AckDestinationUsedTxPower = (sbyte)payload[index];
                                                                        index++;
                                                                        if (payload.Length > index)
                                                                        {
                                                                            SpecificResult.DestinationAckMeasuredRSSI = (sbyte)payload[index];
                                                                            index++;
                                                                            if (payload.Length > index)
                                                                            {
                                                                                SpecificResult.DestinationckMeasuredNoiseFloor = (sbyte)payload[index];
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public override string AboutMe()
        {
            return $"Data={Data.GetHex()}; {SpecificResult.GetReport()}";
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
