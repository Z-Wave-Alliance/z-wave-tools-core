/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;

namespace ZWave.Enums
{
    [Flags]
    public enum DeviceOptions
    {
        /// <summary>
        /// Is node a NoneListening node - Is RF in Powerdown after startup.
        /// </summary>
        NoneListening = 0x00,
        /// <summary>
        /// Is node a Listening node - Is RF in Receive after startup.
        /// </summary>
        Listening = 0x01,
        /// <summary>
        /// Do node contain optional fundtionality.
        /// </summary>
        OptionalFunctionality = 0x02,
        /// <summary>
        /// Is Node a FLiRS 1000ms node.
        /// </summary>
        FreqListeningMode1000ms = 0x10,
        /// <summary>
        /// Is Node a FLiRS 250ms node.
        /// </summary>
        FreqListeningMode250ms = 0x20
    }
}
