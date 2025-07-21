/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZWave.ProgrammerApplication.Enums
{
    public enum CommandTypes
    {
        /// <summary>
        /// Enable interface
        /// </summary>
        EnableInterface = 0xAC,
        /// <summary>
        /// Read Flash
        /// </summary>
        ReadFlash = 0x10,
        /// <summary>
        /// Read SRAM
        /// </summary>
        ReadSRAM = 0x06,
        /// <summary>
        /// Continue Read SRAM/Flash
        /// </summary>
        ContinueRead = 0xA0,
        /// <summary>
        /// Write SRAM
        /// </summary>
        WriteSRAM = 0x04,
        /// <summary>
        /// Continue Write SRAM
        /// </summary>
        ContinueWriteSRAM = 0x80,
        /// <summary>
        /// Erase Chip
        /// </summary>
        EraseChip = 0x0A,
        /// <summary>
        /// Erase Sector
        /// </summary>
        EraseSector = 0x0B,
        /// <summary>
        /// Write Flash Sector
        /// </summary>
        WriteFlashSector = 0x20,
        /// <summary>
        /// Check State
        /// </summary>
        CheckState = 0x7F,
        /// <summary>
        /// Read Signature Byte
        /// </summary>
        ReadSignatureByte = 0x30,
        /// <summary>
        /// Disable EooS Mode
        /// </summary>
        DisableEooSMode = 0xD0,
        /// <summary>
        /// Enable EooS Mode
        /// </summary>
        EnableEooSMode = 0xC0,
        /// <summary>
        /// Set Lock Bits
        /// </summary>
        SetLockBits = 0xF0,
        /// <summary>
        /// Read Lock Bits
        /// </summary>
        ReadLockBits = 0xF1,
        /// <summary>
        /// Set NVR Byte
        /// </summary>
        SetNvrByte = 0xFE,
        /// <summary>
        /// Read NVR Byte
        /// </summary>
        ReadNvrByte = 0xF2,
        /// <summary>
        /// Run CRC Check
        /// </summary>
        RunCrcCheck = 0xC3,
        /// <summary>
        /// Reset Chip
        /// </summary>
        ResetChip = 0xFF,
        /// <summary>
        /// 
        /// </summary>
        SpiFwToggleSck = 0x41,
        /// <summary>
        /// 
        /// </summary>
        SpiFwSetPin = 0x40,
        /// <summary>
        /// 
        /// </summary>
        SpiFwInit = 0x42,
        /// <summary>
        /// 
        /// </summary>
        SpiFwGetVersion = 0x43,
    }
}
