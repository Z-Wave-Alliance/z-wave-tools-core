/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZWave.ProgrammerApplication.Enums
{
    public class MemoryLayout
    {
        /// <summary>
        /// EEPROM Layout enumeration
        /// </summary>
        public enum EepromLayout
        {
            /// <summary>
            /// DUMMY.
            /// </summary>
            DUMMY = 0,
            /// <summary>
            /// Home Id location.
            /// </summary>
            HOMEID_LOC = 0x8
        }
        /// <summary>
        /// Flash Layout enumeration.
        /// </summary>
        public enum FlashLayout
        {
            /// <summary>
            /// Flash application settings table offset for ZW010x, ZW020x, ZW030x.
            /// </summary>
            FLASH_APPL_TABLE_OFFSET = 0x7FB0,
            /// <summary>
            /// Flash application settings table offset for ZW040x.
            /// </summary>
            FLASH_APPL_TABLE_OFFSET_ZW040X = 0xFFB0,
            /// <summary>
            /// Must contain RF_MAGIC_VALUE for table to be valid.
            /// </summary>
            FLASH_APPL_MAGIC_VALUE_OFFS = 0x00,
            /// <summary>
            /// Flash application settings frequency offset.
            /// </summary>
            FLASH_APPL_FREQ_OFFS = 0x01,

            /// <summary>
            /// Flash application settings normal power offset.
            /// </summary>
            FLASH_APPL_NORM_OFFS = 0x02,
            /// <summary>
            /// Flash application settings low power offset.
            /// </summary>
            FLASH_APPL_LOW_OFFS = 0x03,

            /// <summary>
            ///0xFFB2h - FLASH_APPL_NORM_POWER_OFFS_0 Normal power for channel 0
            /// </summary>
            FLASH_APPL_NORM_POWER_OFFS_0 = 0x02,

            /// <summary>
            ///0xFFB3h - FLASH_APPL_NORM_POWER_OFFS_1 Normal power for channel 1
            /// </summary>
            FLASH_APPL_NORM_POWER_OFFS_1 = 0x03,

            /// <summary>
            ///0xFFB4h - FLASH_APPL_NORM_POWER_OFFS_2 Normal power for channel 2
            /// </summary>
            FLASH_APPL_NORM_POWER_OFFS_2 = 0x04,

            /// <summary>
            ///0xFFB5h - FLASH_APPL_LOW_POWER_OFFS_0 Low power for channel 0
            /// </summary>
            FLASH_APPL_LOW_POWER_OFFS_0 = 0x05,

            /// <summary>
            ///0xFFB6h - FLASH_APPL_LOW_POWER_OFFS_1 Low power for channel 1
            /// </summary>
            FLASH_APPL_LOW_POWER_OFFS_1 = 0x06,

            /// <summary>
            ///0xFFB7h - FLASH_APPL_LOW_POWER_OFFS_2 Low power for channel 2
            /// </summary>
            FLASH_APPL_LOW_POWER_OFFS_2 = 0x07,



            /// <summary>
            /// Flash application settings Rx match offset.
            /// </summary>
            FLASH_APPL_RX_MATCH_OFFS = 0x04,
            /// <summary>
            /// Flash application settings Tx match offset.
            /// </summary>
            FLASH_APPL_TX_MACTH_OFFS = 0x05,
            /// <summary>
            /// Flash RF table offset for ZW010x, ZW020x, ZW030x.
            /// </summary>
            FLASH_RF_TABLE_OFFSET = 0x7F80,
            /// <summary>
            /// Flash RF table normal power offset.
            /// </summary>
            FLASH_RF_TABLE_NORM_PWR_OFFS = 0x01,
            /// <summary>
            /// Flash RF table low power offset.
            /// </summary>
            FLASH_RF_TABLE_LOW_PWR_OFFS = 0x02,
            /// <summary>
            /// Flash RF table frequency offset.
            /// </summary>
            FLASH_RF_TABLE_FREQ_OFFS = 0x08
        }

        /// <summary>
        /// RF_MAGIC_VALUE for table to be valid.
        /// </summary>
        public const byte RF_MAGIC_VALUE = 0x42;
        /// <summary>
        /// Default Frequency - use default defined in Z-WAVE lib */
        /// </summary>
        public const byte APP_DEFAULT_FREQ = RF_DEFAULT;  /* Valid values is RF_EU and RF_US */
        /// <summary>
        /// Default Normal Power - Use default defined in Z-WAVE lib
        /// </summary>
        public const byte APP_DEFAULT_NORM_POWER = 0xFF;
        /// <summary>
        /// Default Low Power - Use default defined in Z-WAVE lib
        /// </summary>
        public const byte APP_DEFAULT_LOW_POWER = 0xFF;
        /// <summary>
        /// Default RX_MACTH - Use default defined in Z-WAVE lib
        /// </summary>
        public const byte APP_DEFAULT_RX_MATCH = 0xFF;
        /// <summary>
        /// Default TX_MACTH - Use default defined in Z-WAVE lib
        /// </summary>
        public const byte APP_DEFAULT_TX_MACTH = 0xFF;

        //private byte[] validZW010xRfHLPwr = new byte[] { 0x10, 0x30, 0x5, 0x70, 0x90, 0xB0, 0xD0, 0xF0, 0x1F, 0x3F, 0x5F, 0xBF, 0xDF, 0xFF };

        //private byte[] validZW020xRfHPwr = new byte[] { 0x26, 0x27, 0x28, 0x29, 0x2A, 0xFF };
        //private byte[] validZW020xRfLPwr = new byte[] { 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0xFF };


        private const byte DEFAULT_NDIV_TX_EU = 67;
        private const byte DEFAULT_AVAL_TX_EU = 10;
        private const byte DEFAULT_NDIV_RX_EU = 67;
        private const byte DEFAULT_AVAL_RX_EU = 8;
        private const byte DEFAULT_REFDCON_EU = 0x00;

        private const byte DEFAULT_NDIV_TX_US = 70;
        private const byte DEFAULT_AVAL_TX_US = 2;
        private const byte DEFAULT_NDIV_RX_US = 70;
        private const byte DEFAULT_AVAL_RX_US = 0;
        private const byte DEFAULT_REFDCON_US = 0x00;

        private const byte DEFAULT_NDIV_TX_ANZ = 71;
        private const byte DEFAULT_AVAL_TX_ANZ = 65;
        private const byte DEFAULT_NDIV_RX_ANZ = 72;
        private const byte DEFAULT_AVAL_RX_ANZ = 0x7F;
        private const byte DEFAULT_REFDCON_ANZ = 0x00;

        private const byte DEFAULT_NDIV_TX_HK = 71;
        private const byte DEFAULT_AVAL_TX_HK = 73;
        private const byte DEFAULT_NDIV_RX_HK = 71;
        private const byte DEFAULT_AVAL_RX_HK = 71;
        private const byte DEFAULT_REFDCON_HK = 0x00;

        private const byte DEFAULT_NDIV_TX_866 = 67;
        private const byte DEFAULT_AVAL_TX_866 = 20;
        private const byte DEFAULT_NDIV_RX_866 = 67;
        private const byte DEFAULT_AVAL_RX_866 = 18;
        private const byte DEFAULT_REFDCON_866 = 0x00;

        private const byte DEFAULT_NDIV_TX_870 = 67;
        private const byte DEFAULT_AVAL_TX_870 = 00;
        private const byte DEFAULT_NDIV_RX_870 = 68;
        private const byte DEFAULT_AVAL_RX_870 = 62;
        private const byte DEFAULT_REFDCON_870 = 0x00;

        private const byte DEFAULT_NDIV_TX_906 = 70;
        private const byte DEFAULT_AVAL_TX_906 = 12;
        private const byte DEFAULT_NDIV_RX_906 = 70;
        private const byte DEFAULT_AVAL_RX_906 = 10;
        private const byte DEFAULT_REFDCON_906 = 0x00;

        private const byte DEFAULT_NDIV_TX_910 = 71;
        private const byte DEFAULT_AVAL_TX_910 = 56;
        private const byte DEFAULT_NDIV_RX_910 = 71;
        private const byte DEFAULT_AVAL_RX_910 = 54;
        private const byte DEFAULT_REFDCON_910 = 0x00;

        private const byte DEFAULT_NDIV_TX_MY = 67;
        private const byte DEFAULT_AVAL_TX_MY = 75;
        private const byte DEFAULT_NDIV_RX_MY = 67;
        private const byte DEFAULT_AVAL_RX_MY = 73;
        private const byte DEFAULT_REFDCON_MY = 0x00;

        private const byte DEFAULT_NDIV_TX_IN = 67;
        private const byte DEFAULT_AVAL_TX_IN = 90;
        private const byte DEFAULT_NDIV_RX_IN = 67;
        private const byte DEFAULT_AVAL_RX_IN = 88;
        private const byte DEFAULT_REFDCON_IN = 0x00; //?????

        private const byte DEFAULT_NDIV_TX_RU = 67;
        private const byte DEFAULT_AVAL_TX_RU = 7;
        private const byte DEFAULT_NDIV_RX_RU = 67;
        private const byte DEFAULT_AVAL_RX_RU = 5;
        private const byte DEFAULT_REFDCON_RU = 0x00; //?????

        private const byte DEFAULT_NDIV_TX_IL = 71;
        private const byte DEFAULT_AVAL_TX_IL = 28;
        private const byte DEFAULT_NDIV_RX_IL = 71;
        private const byte DEFAULT_AVAL_RX_IL = 26;
        private const byte DEFAULT_REFDCON_IL = 0x00; //?????


        /// <summary>
        /// Frequency table.
        /// </summary>
        public static byte[,] FrequencyTable = new byte[,] {
            {DEFAULT_NDIV_TX_EU, DEFAULT_AVAL_TX_EU, DEFAULT_NDIV_RX_EU, DEFAULT_AVAL_RX_EU, DEFAULT_REFDCON_EU }, 
            {DEFAULT_NDIV_TX_US, DEFAULT_AVAL_TX_US, DEFAULT_NDIV_RX_US, DEFAULT_AVAL_RX_US, DEFAULT_REFDCON_US },
            {DEFAULT_NDIV_TX_ANZ, DEFAULT_AVAL_TX_ANZ, DEFAULT_NDIV_RX_ANZ, DEFAULT_AVAL_RX_ANZ, DEFAULT_REFDCON_ANZ },
            {DEFAULT_NDIV_TX_HK, DEFAULT_AVAL_TX_HK, DEFAULT_NDIV_RX_HK, DEFAULT_AVAL_RX_HK, DEFAULT_REFDCON_HK },
            {DEFAULT_NDIV_TX_866, DEFAULT_AVAL_TX_866, DEFAULT_NDIV_RX_866, DEFAULT_AVAL_RX_866, DEFAULT_REFDCON_866 },
            {DEFAULT_NDIV_TX_870, DEFAULT_AVAL_TX_870, DEFAULT_NDIV_RX_870, DEFAULT_AVAL_RX_870, DEFAULT_REFDCON_870 },
            {DEFAULT_NDIV_TX_906, DEFAULT_AVAL_TX_906, DEFAULT_NDIV_RX_906, DEFAULT_AVAL_RX_906, DEFAULT_REFDCON_906 },
            {DEFAULT_NDIV_TX_910, DEFAULT_AVAL_TX_910, DEFAULT_NDIV_RX_910, DEFAULT_AVAL_RX_910, DEFAULT_REFDCON_910 },
            {DEFAULT_NDIV_TX_MY, DEFAULT_AVAL_TX_MY, DEFAULT_NDIV_RX_MY, DEFAULT_AVAL_RX_MY, DEFAULT_REFDCON_MY }, 
			{DEFAULT_NDIV_TX_IN, DEFAULT_AVAL_TX_IN, DEFAULT_NDIV_RX_IN, DEFAULT_AVAL_RX_IN, DEFAULT_REFDCON_IN },
            {DEFAULT_NDIV_TX_RU, DEFAULT_AVAL_TX_RU, DEFAULT_NDIV_RX_RU, DEFAULT_AVAL_RX_RU, DEFAULT_REFDCON_RU },
            {DEFAULT_NDIV_TX_IL, DEFAULT_AVAL_TX_IL, DEFAULT_NDIV_RX_IL, DEFAULT_AVAL_RX_IL, DEFAULT_REFDCON_IL }};


        //private const byte DEFAULT_NORMAL_PA_POW_EU = 0x2A;
        //private const byte DEFAULT_NORMAL_PA_POW_US = 0x1B; //??

        private const byte DEFAULT_NORMAL_PA_POW = 0x2A;
        private const byte DEFAULT_NORMAL_PA_POW_ZW040X = 0x10;

        private const byte DEFAULT_NORMAL_PA_POW_EU = DEFAULT_NORMAL_PA_POW;
        private const byte DEFAULT_NORMAL_PA_POW_US = DEFAULT_NORMAL_PA_POW;
        private const byte DEFAULT_NORMAL_PA_POW_ANZ = DEFAULT_NORMAL_PA_POW;
        private const byte DEFAULT_NORMAL_PA_POW_HK = DEFAULT_NORMAL_PA_POW;
        private const byte DEFAULT_NORMAL_PA_POW_866 = DEFAULT_NORMAL_PA_POW;
        private const byte DEFAULT_NORMAL_PA_POW_870 = DEFAULT_NORMAL_PA_POW;
        private const byte DEFAULT_NORMAL_PA_POW_906 = DEFAULT_NORMAL_PA_POW;
        private const byte DEFAULT_NORMAL_PA_POW_910 = DEFAULT_NORMAL_PA_POW;
        private const byte DEFAULT_NORMAL_PA_POW_MY = DEFAULT_NORMAL_PA_POW;
        private const byte DEFAULT_NORMAL_PA_POW_IN = DEFAULT_NORMAL_PA_POW;
        private const byte DEFAULT_NORMAL_PA_POW_RU = DEFAULT_NORMAL_PA_POW;
        private const byte DEFAULT_NORMAL_PA_POW_IL = DEFAULT_NORMAL_PA_POW;

        private const byte DEFAULT_LOW_PA_POW = 0x14;
        private const byte DEFAULT_LOW_PA_POW_ZW040X = 0x02;

        private const byte DEFAULT_LOW_PA_POW_EU = DEFAULT_LOW_PA_POW;
        private const byte DEFAULT_LOW_PA_POW_US = DEFAULT_LOW_PA_POW;
        private const byte DEFAULT_LOW_PA_POW_ANZ = DEFAULT_LOW_PA_POW;
        private const byte DEFAULT_LOW_PA_POW_HK = DEFAULT_LOW_PA_POW;
        private const byte DEFAULT_LOW_PA_POW_866 = DEFAULT_LOW_PA_POW;
        private const byte DEFAULT_LOW_PA_POW_870 = DEFAULT_LOW_PA_POW;
        private const byte DEFAULT_LOW_PA_POW_906 = DEFAULT_LOW_PA_POW;
        private const byte DEFAULT_LOW_PA_POW_910 = DEFAULT_LOW_PA_POW;
        private const byte DEFAULT_LOW_PA_POW_MY = DEFAULT_LOW_PA_POW;
        private const byte DEFAULT_LOW_PA_POW_IN = DEFAULT_LOW_PA_POW;
        private const byte DEFAULT_LOW_PA_POW_RU = DEFAULT_LOW_PA_POW;
        private const byte DEFAULT_LOW_PA_POW_IL = DEFAULT_LOW_PA_POW;


        /// <summary>
        /// Power table.
        /// </summary>
        public static byte[,] PowerTable = new byte[,] {{DEFAULT_NORMAL_PA_POW_EU, DEFAULT_LOW_PA_POW_EU},
                                            {DEFAULT_NORMAL_PA_POW_US, DEFAULT_LOW_PA_POW_US},
                                            {DEFAULT_NORMAL_PA_POW_ANZ, DEFAULT_LOW_PA_POW_ANZ},
                                            {DEFAULT_NORMAL_PA_POW_HK, DEFAULT_LOW_PA_POW_HK},
                                            {DEFAULT_NORMAL_PA_POW_866, DEFAULT_LOW_PA_POW_866},
                                            {DEFAULT_NORMAL_PA_POW_870, DEFAULT_LOW_PA_POW_870},
                                            {DEFAULT_NORMAL_PA_POW_906, DEFAULT_LOW_PA_POW_906},
                                            {DEFAULT_NORMAL_PA_POW_910, DEFAULT_LOW_PA_POW_910},
                                            {DEFAULT_NORMAL_PA_POW_MY, DEFAULT_LOW_PA_POW_MY},
                                            {DEFAULT_NORMAL_PA_POW_IN, DEFAULT_LOW_PA_POW_IN},
                                            {DEFAULT_NORMAL_PA_POW_RU, DEFAULT_LOW_PA_POW_RU},
                                            {DEFAULT_NORMAL_PA_POW_IL, DEFAULT_LOW_PA_POW_IL}};


        /// <summary>
        /// Default frequency.
        /// </summary>
        public const byte RF_DEFAULT = 0xFF;
        /// <summary>
        /// EU frequency.
        /// </summary>
        public const byte RF_EU = 0x00;
        /// <summary>
        /// US frequency.
        /// </summary>
        public const byte RF_US = 0x01;
        /// <summary>
        /// ANZ frequency.
        /// </summary>
        public const byte RF_ANZ = 0x02;
        /// <summary>
        /// HK frequency.
        /// </summary>
        public const byte RF_HK = 0x03;
        /// <summary>
        /// 866.42 frequency.
        /// </summary>
        public const byte RF_866 = 0x04; // "866.42MHz (EU_tf)"
        /// <summary>
        /// 870.42 frequency.
        /// </summary>
        public const byte RF_870 = 0x05; // "870.42MHz (tf)"
        /// <summary>
        /// 906.42 frequency.
        /// </summary>
        public const byte RF_906 = 0x06; // "906.42MHz (US_tf)"
        /// <summary>
        /// 910.42 frequency.
        /// </summary>
        public const byte RF_910 = 0x07; // "910.42MHz (tf)"
        /// <summary>
        /// MY frequency.
        /// </summary>
        public const byte RF_MY = 0x08;
        /// <summary>
        /// IN frequency.
        /// </summary>
        public const byte RF_IN = 0x09;
        /// <summary>
        /// RU frequency.
        /// </summary>
        public const byte RF_RU = 0x0A;

        /// <summary>
        /// IL frequency.
        /// </summary>
        public const byte RF_IL = 0x0B;
    }
}
