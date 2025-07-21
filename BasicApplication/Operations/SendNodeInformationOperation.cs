/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.Enums;
using ZWave.BasicApplication.Enums;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    public class SendNodeInformationOperation : CallbackApiOperation
    {
        private NodeTag Destination { get; set; }
        private TransmitOptions TxOptions { get; set; }
        public SendNodeInformationOperation(NetworkViewPoint network, NodeTag destination, TransmitOptions txOptions)
            : base(CommandTypes.CmdZWaveSendNodeInformation)
        {
            _network = network;
            Destination = destination;
            TxOptions = txOptions;
        }

        protected override byte[] CreateInputParameters()
        {
            var ret = new byte[] { (byte)Destination.Id, (byte)TxOptions };
            if (_network.IsNodeIdBaseTypeLR)
            {
                ret = new byte[] { (byte)(Destination.Id >> 8), (byte)Destination.Id, (byte)TxOptions };
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

        public TransmitResult SpecificResult
        {
            get { return (TransmitResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new TransmitResult();
        }
    }
}
