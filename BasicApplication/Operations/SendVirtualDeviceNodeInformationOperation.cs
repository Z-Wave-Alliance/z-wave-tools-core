/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.BasicApplication.Enums;
using ZWave.Devices;
using ZWave.Enums;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0xA2 | srcNode | destNode | txOptions | funcID
    /// ZW->HOST: RES | 0xA2 | retVal
    /// ZW->HOST; REQ | 0xA2 | funcID | txStatus
    /// 
    /// only use 8 bit for nodeID (regardless of basetype)
    /// </summary>
    public class SendVirtualDeviceNodeInformationOperation : CallbackApiOperation
    {
        private NodeTag SrcNode { get; set; }
        private NodeTag DestNode { get; set; }
        private TransmitOptions TxOptions { get; set; }
        public SendVirtualDeviceNodeInformationOperation(NodeTag srcNode, NodeTag destNode, TransmitOptions txOptions)
            : base(CommandTypes.CmdZWaveSendVirtualDeviceNodeInfo)
        {
            SrcNode = srcNode;
            DestNode = destNode;
            TxOptions = txOptions;
        }

        protected override byte[] CreateInputParameters()
        {
            return new byte[] { (byte)SrcNode.Id, (byte)DestNode.Id, (byte)TxOptions };
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
