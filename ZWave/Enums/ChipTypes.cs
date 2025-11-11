/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.ComponentModel;

namespace ZWave.Enums
{
    public enum ChipTypes
    {
        [Description("Unknown")]
        UNKNOWN = 0x00,
        [Description("ZW010x chip type (100 series)")]
        ZW010x = 0x01,
        [Description("ZW020x chip type (200 series)")]
        ZW020x = 0x02,
        [Description("ZW030x chip type (300 series)")]
        ZW030x = 0x03,
        [Description("ZW040x chip type (400 series)")]
        ZW040x = 0x04,
        [Description("ZW050x chip type (500 series)")]
        ZW050x = 0x05,
        [Description("ZW070x chip type (700 series)")]
        ZW070x = 0x07,
        [Description("ZW080x chip type (800 series)")]
        ZW080x = 0x08,
        [Description("ZGM130S chip type (700 series). Example: ZGM130S_BRD4202A, ZGM130S_BRD4207A")]
        ZW0713 = 0x13,
        [Description("EFR32ZG14 chip type (700 series). Example: ZG14_BRD4201A, ZG14_BRD4206A")]
        ZW0714 = 0x14,
        [Description("Any chip type")]
        Any = 0x20,
        [Description("EFR32ZG14 Radio Board")]
        ZG14_BRD4201A,
        [Description("EFR32RZ13 QFN32 Radio Board")]
        RZ13_BRD4201B,
        [Description("EFR32ZG13L Radio Board")]
        ZG13L_BRD4201C,
        [Description("EFR32ZG13S Radio Board")]
        ZG13S_BRD4201D,

        [Description("ZGM130S Radio Board")]
        ZGM130S_BRD4200A,
        [Description("ZGM130S Radio Board")]
        ZGM130S_BRD4202A,
        [Description("EFR32RZ13 868-915 MHz 13 dBm Radio Board")]
        RZ13_BRD4203A,
        [Description("EFR32ZG14 Z-Wave Long Range Radio Board")]
        ZG14_BRD4206A,
        [Description("ZGM130S Z-Wave Long Range Radio Board")]
        ZGM130S_BRD4207A,
        [Description("EFR32ZG14 915 MHz 20 dBm Radio Board")]
        ZG14_BRD4208A,
        [Description("EFR32RZ13 915 MHz 20 dBm Radio Board")]
        RZ13_BRD4209A,

        [Description("EFR32ZG23 868-915 MHz 14 dBm Radio Board")]
        ZG23_BRD4204A,
        [Description("EFR32ZG23 868-915 MHz 14 dBm Radio Board")]
        ZG23_BRD4204C,
        [Description("EFR32ZG23 868-915 MHz 14 dBm Radio Board")]
        ZG23_BRD4204D,
        [Description("ZGM230S Radio Board")]
        ZGM230S_BRD4205A,
        [Description("ZGM230S Radio Board")]
        ZGM230S_BRD4205B,
        [Description("ZGM230 +14 dBm Dev Kit Board")]
        ZGM230_BRD2603A,
        [Description("EFR32ZG23 868-915 MHz 20 dBm Radio Board")]
        ZG23_BRD4210A,
        [Description("EFR32xG28 868/915 MHz +14 dBm + 2.4 GHz +10 dBm Radio Board")]
        xG28_BRD4400A,
        [Description("EFR32xG28 868/915 MHz +20 dBm + 2.4 GHz +10 dBm Radio Board")]
        xG28_BRD4401A,
        [Description("EFR32xG28 868/915 MHz +14 dBm + 2.4 GHz +10 dBm Radio Board")]
        xG28_BRD4400B,
        [Description("EFR32xG28 868/915 MHz +20 dBm + 2.4 GHz +10 dBm Radio Board")]
        xG28_BRD4401B,
        [Description("EFR32xG28 868/915 MHz +14 dBm + 2.4 GHz +10 dBm Radio Board")]
        xG28_BRD4400C,
        [Description("EFR32xG28 868/915 MHz +20 dBm + 2.4 GHz +10 dBm Radio Board")]
        xG28_BRD4401C,
        [Description("EFR32xG28 868/915 MHz +20 dBm + 2.4 GHz +10 dBm Radio Board")]
        xG28_BRD2705A,
    }
}
