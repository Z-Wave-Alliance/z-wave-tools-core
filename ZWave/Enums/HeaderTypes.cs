/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿namespace ZWave.Enums
{
    public enum HeaderTypes
    {
        /// <summary>
        /// 
        /// </summary>
        Unknown = 0x00,
        /// <summary>
        /// HEADER_SOF
        /// </summary>
        Sof = 0x01,
        /// <summary>
        /// HEADER_ACK
        /// </summary>
        Ack = 0x06,
        /// <summary>
        /// HEADER_NAK
        /// </summary>
        Nak = 0x15,
        /// <summary>
        /// HEADER_CAN
        /// </summary>
        Can = 0x18
    }
}
