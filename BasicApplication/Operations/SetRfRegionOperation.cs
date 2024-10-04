/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0x0B | 0x40 | RfRegion
    /// ZW->HOST: RES | 0x0B | 0x40 | cmdRes
    /// </summary>
    public class SetRfRegionOperation : SerialApiSetupOperation
    {
        private const byte SERIAL_API_SETUP_CMD_RF_REGION_SET = 1 << 6;
        private RfRegions _rfRegion;
        public SetRfRegionOperation(RfRegions rfRegion)
            : base(SERIAL_API_SETUP_CMD_RF_REGION_SET, (byte)rfRegion)
        {
        }
    }
}
