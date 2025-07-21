/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.Enums;
using ZWave.BasicApplication.Enums;
using Utils;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0x14 | numberNodes | pNodeIDList[ ] | dataLength | pData[ ] | txOptions | funcID
    /// ZW->HOST: RES | 0x14 | RetVal
    /// ZW->HOST: REQ | 0x14 | funcID | txStatus
    /// </summary>
    public class SendDataMultiOperation : CallbackApiOperation
    {
        public NodeTag[] Nodes { get; private set; }
        public byte[] Data { get; set; }
        public TransmitOptions TxOptions { get; private set; }
        public SendDataMultiOperation(NetworkViewPoint network, NodeTag[] nodes, byte[] data, TransmitOptions txOptions)
            : base(CommandTypes.CmdZWaveSendDataMulti)
        {
            _network = network;
            Nodes = nodes;
            Data = data;
            TxOptions = txOptions;
        }

        protected override byte[] CreateInputParameters()
        {
            byte[] ret = new byte[3 + Nodes.Length + Data.Length];
            ret[0] = (byte)Nodes.Length;
            for (int i = 0; i < Nodes.Length; i++)
            {
                ret[i + 1] = (byte)Nodes[i].Id;
            }
            ret[1 + Nodes.Length] = (byte)Data.Length;
            for (int i = 0; i < Data.Length; i++)
            {
                ret[i + 2 + Nodes.Length] = Data[i];
            }
            ret[2 + Nodes.Length + Data.Length] = (byte)TxOptions;
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
