/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZWave.ProgrammerApplication.Enums
{
    public enum NVMTypes : uint
    {
        /// <summary>
        /// Bus and protocol type mask.
        /// </summary>
        NVMTypeMask = 0xFF000000,
        /// <summary>
        /// Bus and protocol type bits position.
        /// </summary>
        NVMTypePos = 24,
        /// <summary>
        /// Chip Manufacturer ID mask. 0x00 - unknown manufacturer id.
        /// </summary>
        NVMManufacturerMask = 0x00FF0000,
        /// <summary>
        /// Chip Manufacturer ID bits position.
        /// </summary>
        NVMManufacturerPos = 16,
        /// <summary>
        /// Chip Device ID mask. 0x00 - unknown device id.
        /// </summary>
        NVMDeviceMask = 0x0000FF00,
        /// <summary>
        /// Chip Device ID bits position.
        /// </summary>
        NVMDevicePos = 8,
        /// <summary>
        /// Chip Memory size mask. Memory size = num * NVMSizeUnit KBytes. 0x00 - unknown size.
        /// </summary>
        NVMSizeMask = 0x000000FF,
        /// <summary>
        /// Chip Memory size bits position.
        /// </summary>
        NVMSizePos = 0,
        /// <summary>
        /// Chip Memory size unit in kilo bytes (KB).
        /// </summary>
        NVMSizeUnit = 8,

        /// <summary>
        /// Invalid, cant detect, error.
        /// </summary>
        NVMInvalid = 0x00000000,
        /// <summary>
        /// No errors, but chip type is unknown at this stage.
        /// </summary>
        NVMUnknown = 0x00010000,
        /// <summary>
        /// Serial EEPROM chip with SPI bus protocol like in Atmel AT25128 serial EEPROM and compatible (ST95128, etc.).
        /// </summary>
        NVMSerialEEPROM = 0x01000000,
        /// <summary>
        /// Serial Flash chip on SPI bus.
        /// </summary>
        NVMSerialFlash = 0x02000000,
        /// <summary>
        /// Numonyx M25PE10
        /// </summary>
        NVMNumonyxM25PE10 = 0x02208010,
        /// <summary>
        /// Numonyx M25PE20
        /// </summary>
        NVMNumonyxM25PE20 = 0x02208020,
        /// <summary>
        /// Numonyx M25PE40
        /// </summary>
        NVMNumonyxM25PE40 = 0x02208040,
    }
}
