/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
namespace ZWave.BasicApplication.Enums
{
    public enum XModemHeaderTypes
    {
        ///<summary>Acknowledge<summary/>
        ACK = 0x06,
        ///<summary>Not Acknowledge<summary/>
        NACK = 0x15,
        /// <summary>Cancel</summary>
        CAN = 0x18,
        ///<summary>ASCII 'C'</summary>
        C = 0x43
    }
}
