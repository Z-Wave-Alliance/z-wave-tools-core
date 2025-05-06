/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;
using ZWave.Devices;
using ZWave.Enums;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0x57 | node | txOption | funcID
    /// ZW->HOST: RES | 0x57 | RetVal
    /// ZW->HOST: REQ | 0x57 | funcID | txStatus
    /// </summary>
    public class SendSucIdOperation : CallbackApiOperation
    {
        private NodeTag Node { get; set; }
        private TransmitOptions TxOptions { get; set; }
        public SendSucIdOperation(NodeTag node, TransmitOptions txOptions)
            : base(CommandTypes.CmdZWaveSendSucId)
        {
            Node = node;
            TxOptions = txOptions;
        }

        protected override byte[] CreateInputParameters()
        {
            return new byte[] { (byte)Node.Id, (byte)TxOptions };
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
