/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;

namespace ZWave.Enums
{
    [Flags]
    public enum NetworkKeyS2Flags
    {
        None = 0x00,
        S2Class0 = 0x01,
        S2Class1 = 0x02, 
        S2Class2 = 0x04,
        S0 = 0x80
    }
}
