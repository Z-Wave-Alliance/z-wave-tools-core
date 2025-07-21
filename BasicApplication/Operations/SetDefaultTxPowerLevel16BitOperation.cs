/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0x0B | 0x12 | NormalTxPowerLevel (MSB) | NormalTxPowerLevel (LSB) | Measured0dBmPower (MSB)| Measured0dBmPower (LSB)
    /// ZW->HOST: RES | 0x0B | 0x12 | cmdRes
    /// </summary>
    public class SetDefaultTxPowerLevel16BitOperation : SerialApiSetupOperation
    {
        private const byte SERIAL_API_SETUP_CMD_TX_POWERLEVEL_SET = 0x012;
        private readonly short _normalTxPower;
        private readonly short _measured0dBmPower;

        public SetDefaultTxPowerLevel16BitOperation(short normalTxPower, short measured0dBmPower)
            : base(SERIAL_API_SETUP_CMD_TX_POWERLEVEL_SET, (byte)(normalTxPower >> 8), (byte)normalTxPower, (byte)(measured0dBmPower >> 8), (byte)measured0dBmPower)
        {
        }
    }
}
