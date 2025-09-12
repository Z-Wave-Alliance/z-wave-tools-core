/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
/// SPDX-FileCopyrightText: Z-Wave Alliance https://z-wavealliance.org
using System;

namespace ZWave.Enums
{
    [Flags]
    public enum NetworkKeyS2Flags
    {
        None = 0x00,
        /// <summary>
        /// S2 Unauthenticated Class
        /// </summary>
        S2Class0 = 0x01,
        /// <summary>
        /// S2 Authenticated Class
        /// </summary>
        S2Class1 = 0x02,
        /// <summary>
        /// S2 Access Control Class
        /// </summary>
        S2Class2 = 0x04,
        /// <summary>
        /// S0 Secure Legacy Class
        /// </summary>
        S0 = 0x80
    }
}
