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
    /// HOST->ZW: REQ | 0x3A 
    /// ZW->HOST: RES | 0x3A | wRFTxFrames[2] | wRFTxLBTBackOffs[2] | wRFRxFrames[2] | wRFRxLRCErrors[2] | wRFRxCRC16Errors[2] | wRFRxForeignHomeID[2]
    /// </summary>
    public class GetNetworkStatsOperation : RequestApiOperation
    {
        public GetNetworkStatsOperation()
     : base(CommandTypes.CmdGetNetworkStats, false)
        {
        }

        protected override byte[] CreateInputParameters()
        {
            return null;
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            var payload = ((DataReceivedUnit)ou).DataFrame.Payload;
            int index = 0;
            if (payload.Length >= index + 2)
            {
                SpecificResult.RFTxFrames = (ushort)((payload[index++] << 8) + payload[index++]);
            }
            if (payload.Length >= index + 2)
            {
                SpecificResult.RFTxLBTBackOffs = (ushort)((payload[index++] << 8) + payload[index++]);
            }
            if (payload.Length >= index + 2)
            {
                SpecificResult.RFRxFrames = (ushort)((payload[index++] << 8) + payload[index++]);
            }
            if (payload.Length >= index + 2)
            {
                SpecificResult.RFRxLRCErrors = (ushort)((payload[index++] << 8) + payload[index++]);
            }
            if (payload.Length >= index + 2)
            {
                SpecificResult.RFRxCRC16Errors = (ushort)((payload[index++] << 8) + payload[index++]);
            }
            if (payload.Length >= index + 2)
            {
                SpecificResult.RFRxForeignHomeID = (ushort)((payload[index++] << 8) + payload[index++]);
            }
            base.SetStateCompleted(ou);
        }

        public GetNetworkStatsResult SpecificResult
        {
            get { return (GetNetworkStatsResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new GetNetworkStatsResult();
        }

    }

    public class GetNetworkStatsResult : ActionResult
    {
        public ushort RFTxFrames { get; set; }
        public ushort RFTxLBTBackOffs { get; set; }
        public ushort RFRxFrames { get; set; }
        public ushort RFRxLRCErrors { get; set; }
        public ushort RFRxCRC16Errors { get; set; }
        public ushort RFRxForeignHomeID { get; set; }
    }
}
