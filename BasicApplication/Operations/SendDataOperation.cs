/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using ZWave.Enums;
using ZWave.BasicApplication.Enums;
using Utils;
using ZWave.Devices;
using System.Linq;

namespace ZWave.BasicApplication.Operations
{
    public class SendDataOperation : CallbackApiOperation, ISendDataAction
    {
        public NodeTag DstNode { get; set; }
        internal NodeTag SrcNode { get; set; }
        public byte[] Data { get; set; }
        internal bool IsFollowup { get; set; }
        internal TransmitOptions TxOptions { get; private set; }
        public Action SubstituteCallback { get; set; }
        public object Extensions { get; set; }

        public SendDataOperation(NetworkViewPoint network, NodeTag node, byte[] data, TransmitOptions txOptions)
            : this(network, NodeTag.Empty, node, data, txOptions)
        { }

        public SendDataOperation(NetworkViewPoint network, NodeTag srcNode, NodeTag dstNode, byte[] data, TransmitOptions txOptions)
            : base(CommandTypes.CmdZWaveSendData, CommandTypes.CmdZWaveSendDataAbort)
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
            if (Data == null)
                Data = new byte[0];
            byte[] ret = new byte[3 + Data.Length];
            ret[0] = (byte)DstNode.Id;
            ret[1] = (byte)Data.Length;
            for (int i = 0; i < Data.Length; i++)
            {
                ret[i + 2] = Data[i];
            }
            ret[2 + Data.Length] = (byte)TxOptions;
            if (_network.IsNodeIdBaseTypeLR)
            {
                ret = new byte[4 + Data.Length];
                ret[0] = (byte)(DstNode.Id >> 8);
                ret[1] = (byte)DstNode.Id;
                ret[2] = (byte)Data.Length;
                for (int i = 0; i < Data.Length; i++)
                {
                    ret[i + 3] = Data[i];
                }
                ret[3 + Data.Length] = (byte)TxOptions;
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

    public class TransmitResult : ActionResult
    {
        public byte FuncId { get; set; }
        public TransmitStatuses TransmitStatus { get; set; }
    }

    public class SendDataResult : TransmitResult
    {
        public bool HasTxTransmitReport { get; set; }
        public ushort TransmitTicks { get; set; }
        public byte RepeatersCount { get; set; }
        public sbyte[] RssiValuesIncoming { get; set; }
        public byte AckChannelNo { get; set; }
        public byte LastTxChannelNo { get; set; }
        public RoutingSchemes RouteScheme { get; set; }
        public byte[] Repeaters { get; set; }
        public byte RouteSpeed { get; set; }
        public byte RouteTries { get; set; }
        public byte LastFailedLinkFrom { get; set; }
        public byte LastFailedLinkTo { get; set; }
        public sbyte UsedTxpower { get; set; }
        public sbyte MeasuredNoiseFloor { get; set; }
        public sbyte AckDestinationUsedTxPower { get; set; }
        public sbyte DestinationAckMeasuredRSSI { get; set; }
        public sbyte DestinationckMeasuredNoiseFloor { get; set; }
        public SubstituteStatuses TxSubstituteStatus { get; set; }

        public void CopyFrom(SendDataResult result)
        {
            if (result != null)
            {
                TransmitStatus = result.TransmitStatus;
                HasTxTransmitReport = result.HasTxTransmitReport;
                TransmitTicks = result.TransmitTicks;
                RepeatersCount = result.RepeatersCount;
                RssiValuesIncoming = result.RssiValuesIncoming;
                AckChannelNo = result.AckChannelNo;
                LastTxChannelNo = result.LastTxChannelNo;
                RouteScheme = result.RouteScheme;
                Repeaters = result.Repeaters;
                RouteSpeed = result.RouteSpeed;
                RouteTries = result.RouteTries;
                LastFailedLinkFrom = result.LastFailedLinkFrom;
                LastFailedLinkTo = result.LastFailedLinkTo;
                UsedTxpower = result.UsedTxpower;
                MeasuredNoiseFloor = result.MeasuredNoiseFloor;
                AckDestinationUsedTxPower = result.AckDestinationUsedTxPower;
                DestinationAckMeasuredRSSI = result.DestinationAckMeasuredRSSI;
                DestinationckMeasuredNoiseFloor = result.DestinationckMeasuredNoiseFloor;
                TxSubstituteStatus = result.TxSubstituteStatus;
            }
        }

        public void AggregateWith(SendDataResult result)
        {
            if (result != null)
            {
                HasTxTransmitReport = result.HasTxTransmitReport;
                TransmitTicks += result.TransmitTicks;
                RepeatersCount = result.RepeatersCount;
                RssiValuesIncoming = result.RssiValuesIncoming;
                AckChannelNo = result.AckChannelNo;
                LastTxChannelNo = result.LastTxChannelNo;
                RouteScheme = result.RouteScheme;
                Repeaters = result.Repeaters;
                RouteSpeed = result.RouteSpeed;
                RouteTries += result.RouteTries;
                LastFailedLinkFrom = result.LastFailedLinkFrom;
                LastFailedLinkTo = result.LastFailedLinkTo;
                UsedTxpower = result.UsedTxpower;
                MeasuredNoiseFloor = result.MeasuredNoiseFloor;
                AckDestinationUsedTxPower = result.AckDestinationUsedTxPower;
                DestinationAckMeasuredRSSI = result.DestinationAckMeasuredRSSI;
                DestinationckMeasuredNoiseFloor = result.DestinationckMeasuredNoiseFloor;
                TxSubstituteStatus = result.TxSubstituteStatus;
            }
        }

        public string GetReport()
        {
            return $"{TransmitStatus} (Tr={RouteTries} RSc=0x{((byte)RouteScheme):X2} RSp=0x{RouteSpeed:X2}" 
                + (Repeaters != null && Repeaters.Length > 0 && Repeaters.Any(x => x > 0) ? $" Rpt={Repeaters.TakeWhile(x => x > 0).GetHex()}" : $"")
                + (RssiValuesIncoming!= null && RssiValuesIncoming.Length > 0 ? $" TPw={UsedTxpower} Rs0={RssiValuesIncoming[0]}" : $"")
                + ")";
        }
    }
}
