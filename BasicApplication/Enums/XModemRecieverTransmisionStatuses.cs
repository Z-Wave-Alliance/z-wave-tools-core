/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿namespace ZWave.BasicApplication.Enums
{
    public enum XModemRecieverTransmisionStatuses : byte
    {
        ///<summary>Start of Header<summary/>
        SOH = 0x01,
        ///<summary>End of Transmission<summary/>
        EOT = 0x04,
        ///<summary>End of Transmission Block(Return to Amulet OS mode)<summary/>
        ETB = 0x17,
        ///<summary>Cancel(Force receiver to start sending C's)<summary/>
        CAN = 0x18,
    }
}
