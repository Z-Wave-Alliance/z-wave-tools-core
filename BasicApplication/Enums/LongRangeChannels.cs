/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿namespace ZWave.BasicApplication.Enums
{
    public enum LongRangeChannels : byte
    {
        Undefined = 0,
        /// <summary>
        /// ZW_LR_CHANNEL_A
        /// </summary>
        ChannelA = 0x01,
        /// <summary>
        /// ZW_LR_CHANNEL_B
        /// </summary>
        ChannelB = 0x02, 
        /// <summary>
        /// Automatic channel selection
        /// </summary>
        ChannelAuto = 0xFF,
    }
}
