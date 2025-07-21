/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0x81|  (FUNC_ID_GET_TX_COUNTER)
    /// ZW->HOST: RES | 0x81 | ZW_TX_COUNTER (1 byte)
    /// </summary>
    public class GetTransmitCountOperation : RequestApiOperation
    {
        public GetTransmitCountOperation()
            : base(CommandTypes.CmdGetTXCounter, true)
        {
        }

        protected override byte[] CreateInputParameters()
        {
            return null;
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            byte[] res = ((DataReceivedUnit)ou).DataFrame.Payload;
            SpecificResult.TxCounter = res[0];
            base.SetStateCompleted(ou);
        }

        public GetTransmitCountResult SpecificResult
        {
            get { return (GetTransmitCountResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new GetTransmitCountResult();
        }
    }

    public class GetTransmitCountResult : ActionResult
    {
        public byte TxCounter { get; set; }
    }
}
