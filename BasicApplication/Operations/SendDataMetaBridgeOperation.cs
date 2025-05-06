/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using Utils;
using ZWave.BasicApplication.Enums;
using ZWave.Devices;
using ZWave.Enums;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0xAA | srcNodeID | destNodeID | dataLength | pData[ ] | txOptions | pRoute[4] | funcID
    /// ZW->HOST: RES | 0xAA | RetVal
    /// ZW->HOST: REQ | 0xAA | funcID | txStatus
    /// 
    /// WARNING: Use pRoute[4] equal [0,0,0,0].
    /// </summary>
    public class SendDataMetaBridgeOperation : CallbackApiOperation
    {
        private NodeTag SrcNode { get; set; }
        private NodeTag DestNode { get; set; }
        private byte[] Data { get; set; }
        private TransmitOptions TxOptions { get; set; }
        public SendDataMetaBridgeOperation(NetworkViewPoint network, NodeTag srcNode, NodeTag destNode, byte[] data, TransmitOptions txOptions)
            : base(CommandTypes.CmdZWaveSendDataMeta_Bridge)
        {
            _network = network;
            SrcNode = srcNode;
            DestNode = destNode;
            Data = data;
            TxOptions = txOptions;
        }

        protected override byte[] CreateInputParameters()
        {
            byte[] ret = new byte[8 + Data.Length];
            ret[0] = (byte)SrcNode.Id;
            ret[1] = (byte)DestNode.Id;
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
                ret[2] = (byte)(DestNode.Id >> 8);
                ret[3] = (byte)DestNode.Id;
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

        protected override void OnCallbackInternal(DataReceivedUnit ou)
        {
            if (ou.DataFrame.Payload != null && ou.DataFrame.Payload.Length > 1)
            {
                SpecificResult.FuncId = ou.DataFrame.Payload[0];
                SpecificResult.TransmitStatus = (TransmitStatuses)ou.DataFrame.Payload[1];
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
