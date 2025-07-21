/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com

namespace ZWave.Security
{
    public enum ExtensionTypes : byte
    {
        Span = 0x01,
        Mpan = 0x02,
        MpanGrp = 0x03,
        Mos = 0x04,
        Test = 0xFF
    }
}
