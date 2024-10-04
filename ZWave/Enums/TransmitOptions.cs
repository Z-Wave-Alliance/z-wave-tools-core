/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;

namespace ZWave.Enums
{
    [Flags]
    public enum TransmitOptions
    {
        /// <summary>
        /// No Request.
        /// </summary>
        TransmitOptionNone = 0x00,
        /// <summary>
        /// Request acknowledge from destination node.
        /// </summary>  
        TransmitOptionAcknowledge = 0x01,
        /// <summary>
        /// Request with low power option.
        /// </summary>
        TransmitOptionLowPower = 0x02,
        /// <summary>
        /// Request retransmission via repeater nodes (at normal output power level).
        /// </summary>
        TransmitOptionAutoRoute = 0x04,
        /// <summary>
        /// Request with no route option.
        /// </summary>
        TransmitOptionNoRoute = 0x10,
        /// <summary>
        /// Request Explore Frame route resolution if all else fails
        /// </summary>
        TransmitOptionExplore = 0x20,
        /// <summary>
        /// Request without retransmission. Is used for ERTT 
        /// </summary>
        TransmitOptionNoRetransmit = 0x40
    }
}
