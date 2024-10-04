/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZWave.ProgrammerApplication.Enums
{
    public enum EepromSizes
    {
        /// <summary>
        /// EEPROM size for 100-300 series.
        /// </summary>
        EEPROM_CS_SIZE_ZW010x_ZW030x = 0x4000,
        /// <summary>
        /// External EEPROM size for 400 series.
        /// </summary>
        EEPROM_CS_SIZE_ZW040x = 0x4000,
        /// <summary>
        /// Internal EEPROM (MTP) size for 400 series.
        /// </summary>
        EEPROM_CS_MTP_SIZE_ZW040x = 64
    }
}
