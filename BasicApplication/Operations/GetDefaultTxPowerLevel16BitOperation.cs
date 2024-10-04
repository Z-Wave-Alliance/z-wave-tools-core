/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0x0B | 0x13
    /// ZW->HOST: RES | 0x0B | 0x13 | NormalTxPowerLevel (MSB) | NormalTxPowerLevel (LSB) | Measured0dBmPower (MSB)| Measured0dBmPower (LSB)
    /// </summary>
    /// 
    public class GetDefaultTxPowerLevel16BitOperation : SerialApiSetupOperation
    {
        private const byte SERIAL_API_SETUP_CMD_TX_POWERLEVEL_GET = 0x13;

        public GetDefaultTxPowerLevel16BitOperation()
            : base(SERIAL_API_SETUP_CMD_TX_POWERLEVEL_GET)
        {
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            var payload = ((DataReceivedUnit)ou).DataFrame.Payload;
            var normalTxPower = (payload[1] << 8) + payload[2];
            var measured0dBmPower = (payload[3] << 8) + payload[4];
            SpecificResult.NormalTxPower = (short)normalTxPower;
            SpecificResult.Measured0dBmPower = (short)measured0dBmPower;
            base.SetStateCompleted(ou);
        }

        public DefaultTxPowerLevelGetResult SpecificResult
        {
            get { return (DefaultTxPowerLevelGetResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new DefaultTxPowerLevelGetResult();
        }
    }

}
