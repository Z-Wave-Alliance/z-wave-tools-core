/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
namespace ZWave.BasicApplication.Enums
{
    public enum DcdcModes : byte
    {
        Auto            = 0x00, // EDCDCMODE_AUTO = 0
        ByPass          = 0x01, // EDCDCMODE_BYPASS = 1
        DCDCLowNoise    = 0x02  // EDCDCMODE_DCDC_LOW_NOISE = 2
    }
}