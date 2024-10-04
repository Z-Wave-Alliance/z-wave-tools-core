/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using ZWave.Enums;
using ZWave.BasicApplication.Enums;
using ZWave.Devices;
using Utils;

namespace ZWave.BasicApplication.Operations
{
    public class SendDataExOperation : CallbackApiOperation, ISendDataAction
    {
        public NodeTag DstNode { get; set; }
        internal NodeTag SrcNode { get; set; }
        public byte[] Data { get; set; }
        internal TransmitOptions TxOptions { get; private set; }
        internal TransmitOptions2 TxOptions2 { get; private set; }
        internal TransmitSecurityOptions TxSecOptions { get; private set; }
        internal SecuritySchemes SecurityScheme { get; set; }
        public Action SubstituteCallback { get; set; }

        public SendDataExOperation(NetworkViewPoint network, NodeTag node, byte[] data, TransmitOptions txOptions, SecuritySchemes scheme)
           : this(network, NodeTag.Empty, node, data, txOptions, TransmitSecurityOptions.S2_TXOPTION_VERIFY_DELIVERY, scheme, TransmitOptions2.TRANSMIT_OPTION_2_TRANSPORT_SERVICE)
        {
        }

        public SendDataExOperation(NetworkViewPoint network, NodeTag srcNode, NodeTag dstNode, byte[] data, TransmitOptions txOptions, SecuritySchemes scheme)
          : this(network, srcNode, dstNode, data, txOptions, TransmitSecurityOptions.S2_TXOPTION_VERIFY_DELIVERY, scheme, TransmitOptions2.TRANSMIT_OPTION_2_TRANSPORT_SERVICE)
        {
        }

        public SendDataExOperation(NetworkViewPoint network, NodeTag node, byte[] data, TransmitOptions txOptions, TransmitSecurityOptions txSecOptions, SecuritySchemes scheme, TransmitOptions2 txOptions2)
           : this(network, NodeTag.Empty, node, data, txOptions, txSecOptions, scheme, txOptions2)
        {
        }

        public SendDataExOperation(NetworkViewPoint network, NodeTag node, byte[] data, TransmitOptions txOptions, TransmitSecurityOptions txSecOptions, SecuritySchemes scheme, TransmitOptions2 txOptions2, int Timeout)
           : this(network, NodeTag.Empty, node, data, txOptions, txSecOptions, scheme, txOptions2, Timeout)
        {
        }

        public SendDataExOperation(NetworkViewPoint network, NodeTag srcNode, NodeTag dstNode, byte[] data, TransmitOptions txOptions, TransmitSecurityOptions txSecOptions, SecuritySchemes scheme, TransmitOptions2 txOptions2)
            : base(CommandTypes.CmdZWaveSendDataEx)
        {
            _network = network;
            SrcNode = srcNode;
            DstNode = dstNode;
            Data = data;
            TxOptions = txOptions;
            TxOptions2 = txOptions2;
            TxSecOptions = txSecOptions;
            SecurityScheme = scheme;
        }

        public SendDataExOperation(NetworkViewPoint network, NodeTag srcNode, NodeTag dstNode, byte[] data, TransmitOptions txOptions, TransmitSecurityOptions txSecOptions, SecuritySchemes scheme, TransmitOptions2 txOptions2, int Timeout)
            : base(CommandTypes.CmdZWaveSendDataEx)
        {
            _network = network;
            SrcNode = srcNode;
            DstNode = dstNode;
            Data = data;
            TxOptions = txOptions;
            TxOptions2 = txOptions2;
            TxSecOptions = txSecOptions;
            SecurityScheme = scheme;
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
            SpecificResult.TransmitStatus = TransmitStatuses.ResMissing;
            messageSendDataAbort = new ApiMessage(CommandTypes.CmdZWaveSendDataAbort);
        }

        protected override byte[] CreateInputParameters()
        {
            if (Data == null)
                Data = new byte[0];
            byte[] ret = new byte[6 + Data.Length];
            ret[0] = (byte)DstNode.Id;
            ret[1] = (byte)Data.Length;
            for (int i = 0; i < Data.Length; i++)
            {
                ret[i + 2] = Data[i];
            }
            ret[2 + Data.Length] = (byte)TxOptions;
            ret[3 + Data.Length] = (byte)TxSecOptions;
            ret[4 + Data.Length] = (byte)SecurityScheme;
            ret[5 + Data.Length] = (byte)TxOptions2;
            if (_network.IsNodeIdBaseTypeLR)
            {
                ret = new byte[7 + Data.Length];
                ret[0] = (byte)(DstNode.Id >> 8);
                ret[1] = (byte)DstNode.Id;
                ret[2] = (byte)Data.Length;
                for (int i = 0; i < Data.Length; i++)
                {
                    ret[i + 3] = Data[i];
                }
                ret[3 + Data.Length] = (byte)TxOptions;
                ret[4 + Data.Length] = (byte)TxSecOptions;
                ret[5 + Data.Length] = (byte)SecurityScheme;
                ret[6 + Data.Length] = (byte)TxOptions2;
            }
            return ret;
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
