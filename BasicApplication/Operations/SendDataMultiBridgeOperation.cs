/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using Utils;
using ZWave.BasicApplication.Enums;
using ZWave.Devices;
using ZWave.Enums;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0xAB | srcNodeID | numberNodes | pNodeIDList[ ] | dataLength | pData[ ] | txOptions | funcID
    /// ZW->HOST: RES | 0xAB | RetVal
    /// ZW->HOST: REQ | 0xAB | funcID | txStatus
    /// </summary>
    public class SendDataMultiBridgeOperation : CallbackApiOperation
    {
        public NodeTag SrcNode { get; private set; }
        public NodeTag[] Nodes { get; private set; }
        public byte[] Data { get; set; }
        public TransmitOptions TxOptions { get; private set; }
        public SendDataMultiBridgeOperation(NetworkViewPoint network, NodeTag srcNode, NodeTag[] nodes, byte[] data, TransmitOptions txOptions)
            : base(CommandTypes.CmdZWaveSendDataMulti_Bridge)
        {
            _network = network;
            SrcNode = srcNode;
            Nodes = nodes;
            Data = data;
            TxOptions = txOptions;
        }

        protected override byte[] CreateInputParameters()
        {
            byte[] ret = new byte[4 + Nodes.Length + Data.Length];
            ret[0] = (byte)SrcNode.Id;
            ret[1] = (byte)Nodes.Length;
            for (int i = 0; i < Nodes.Length; i++)
            {
                ret[i + 2] = (byte)Nodes[i].Id;
            }
            ret[2 + Nodes.Length] = (byte)Data.Length;
            for (int i = 0; i < Data.Length; i++)
            {
                ret[i + 3 + Nodes.Length] = Data[i];
            }
            ret[3 + Nodes.Length + Data.Length] = (byte)TxOptions;
            if (_network.IsNodeIdBaseTypeLR)
            {
                ret = new byte[5 + (2 * Nodes.Length) + Data.Length];
                ret[0] = (byte)(SrcNode.Id >> 8);
                ret[1] = (byte)SrcNode.Id;
                ret[2] = (byte)Nodes.Length;
                for (int i = 0; i < Nodes.Length; i++)
                {
                    ret[(2 * i) + 3] = (byte)(Nodes[i].Id >> 8);
                    ret[(2 * i) + 1 + 3] = (byte)Nodes[i].Id;
                }
                ret[3 + (2 * Nodes.Length)] = (byte)Data.Length;
                for (int i = 0; i < Data.Length; i++)
                {
                    ret[i + 4 + (2 * Nodes.Length)] = Data[i];
                }
                ret[4 + (2 * Nodes.Length) + Data.Length] = (byte)TxOptions;
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
