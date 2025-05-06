/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿namespace ZWave.Enums
{
    public enum DataFrameTypes
    {
        Data = 0x00,
        Ack = 0x06,
        NAck = 0x15,
        CAN = 0x18,
        Publish = 0xFE,
        Subscribe = 0xFF
    }
}
