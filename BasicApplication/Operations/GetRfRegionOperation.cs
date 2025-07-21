/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0x0B | 0x20
    /// ZW->HOST: RES | 0x0B | 0x20 | RfRegion
    /// </summary>
    /// 
    public class GetRfRegionOperation : SerialApiSetupOperation
    {
        private const byte SERIAL_API_SETUP_CMD_RF_REGION_GET = 1 << 5;

        public GetRfRegionOperation()
            : base(SERIAL_API_SETUP_CMD_RF_REGION_GET)
        {
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            var payload = ((DataReceivedUnit)ou).DataFrame.Payload;
            if (payload != null && payload.Length > 1)
            {
                var region = ((DataReceivedUnit)ou).DataFrame.Payload[1];
                if (Enum.TryParse(region.ToString(), out RfRegions rfRegion))
                {
                    SpecificResult.RfRegion = rfRegion;
                }
            }
            base.SetStateCompleted(ou);
        }

        public RfRegionGetResult SpecificResult
        {
            get { return (RfRegionGetResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new RfRegionGetResult();
        }
    }

    public class RfRegionGetResult : ReturnValueResult
    {
        public RfRegions RfRegion { get; set; } = RfRegions.Undefined;
    }
}
