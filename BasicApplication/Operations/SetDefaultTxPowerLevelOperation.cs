/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0x0B | 0x04 | NormalTxPower | Measured0dBmPower
    /// ZW->HOST: RES | 0x0B | 0x04 | cmdRes
    /// </summary>
    public class SetDefaultTxPowerLevelOperation : SerialApiSetupOperation
    {
        private const byte SERIAL_API_SETUP_CMD_TX_POWERLEVEL_SET = 1 << 2;
        private readonly byte _normalTxPower;
        private readonly byte _measured0dBmPower;

        public SetDefaultTxPowerLevelOperation(sbyte normalTxPower, sbyte measured0dBmPower)
            : base(SERIAL_API_SETUP_CMD_TX_POWERLEVEL_SET, (byte)normalTxPower, (byte)measured0dBmPower)
        {
        }
    }
}
