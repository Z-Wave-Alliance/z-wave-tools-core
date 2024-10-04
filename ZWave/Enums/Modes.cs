/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;

namespace ZWave.Enums
{
    /// <summary>
    /// Bits 0-3 contains different modes. 
    /// Bits 4-7 specify different additional options
    /// </summary>
    public enum Modes
    {
        None = 0,
        NodeAny = 1,
        NodeController = 2,
        NodeEndDevice = 3,
        NodeExisting = 4,
        NodeStop = 5,
        NodeStopFailed = 6,
        NodeReserved = 7,
        NodeHomeId = 8,
        NodeSmartStart = 9,
        
        // mask to cut options 
        NodeModeMask = 0x0F,

        // option's flags
        NodeOptionLongRange = 0x20, // Flag
        NodeOptionNetworkWide = 0x40, // Flag
        NodeOptionNormalPower = 0x80, // Flag
    }
}

