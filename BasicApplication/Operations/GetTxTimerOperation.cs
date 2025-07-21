/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0x38 
    /// ZW->HOST: RES | 0x38 | TxTimeChannel0[4] | TxTimeChannel1[4] | TxTimeChannel2[4] | TxTimeChannel3[4] | TxTimeChannel4[4]
    /// </summary>
    public class GetTxTimerOperation : ApiOperation
    {
        public GetTxTimerOperation() : base(true, CommandTypes.CmdGetTxTimer, false)
        { }

        private ApiMessage _message;
        private ApiHandler _handler;
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 2000, _message));
            ActionUnits.Add(new DataReceivedUnit(_handler, OnReceived));
        }

        protected override void CreateInstance()
        {
            _message = new ApiMessage(CommandTypes.CmdGetTxTimer);
            _handler = new ApiHandler(CommandTypes.CmdGetTxTimer);
        }

        private void OnReceived(DataReceivedUnit ou)
        {
            var payload = ou.DataFrame.Payload;
            int index = 0;
            if (payload.Length >= index + 4)
            {
                SpecificResult.TxTimeChannel0 = (payload[index++] << 24) + (payload[index++] << 16) + (payload[index++] << 8) + payload[index++];
            }
            if (payload.Length >= index + 4)
            {
                SpecificResult.TxTimeChannel1 = (payload[index++] << 24) + (payload[index++] << 16) + (payload[index++] << 8) + payload[index++];
            }
            if (payload.Length >= index + 4)
            {
                SpecificResult.TxTimeChannel2 = (payload[index++] << 24) + (payload[index++] << 16) + (payload[index++] << 8) + payload[index++];
            }
            if (payload.Length >= index + 4)
            {
                SpecificResult.TxTimeChannel3 = (payload[index++] << 24) + (payload[index++] << 16) + (payload[index++] << 8) + payload[index++];
            }
            if (payload.Length >= index + 2)
            {
                SpecificResult.TxTimeChannel4 = (payload[index++] << 24) + (payload[index++] << 16) + (payload[index++] << 8) + payload[index++];
            }
            SetStateCompleted(ou);
        }

        public GetTxTimerResult SpecificResult
        {
            get { return (GetTxTimerResult)Result; }
        }
        protected override ActionResult CreateOperationResult()
        {
            return new GetTxTimerResult();
        }
    }

    public class GetTxTimerResult : ActionResult
    {
        public int TxTimeChannel0 { get; set; }
        public int TxTimeChannel1 { get; set; }
        public int TxTimeChannel2 { get; set; }
        public int TxTimeChannel3 { get; set; }
        public int TxTimeChannel4 { get; set; }
    }
}
