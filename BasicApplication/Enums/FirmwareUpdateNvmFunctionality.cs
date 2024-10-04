/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿namespace ZWave.BasicApplication.Enums
{
    /// <summary>
    /// Firmware Update NVM functionality enumeration.
    /// </summary>
    public enum FirmwareUpdateNvmFunctionality
    {
        FIRMWARE_UPDATE_NVM_INIT = 0,           /* BYTE ZW_FirmwareUpdate_NVM_Init() */
        FIRMWARE_UPDATE_NVM_SET_NEW_IMAGE = 1,  /* BYTE ZW_FirmwareUpdate_NVM_Set_NEWIMAGE(BYTE bValue) */
        FIRMWARE_UPDATE_NVM_GET_NEW_IMAGE = 2,  /* BYTE ZW_FirmwareUpdate_NVM_Get_NEWIMAGE() */
        FIRMWARE_UPDATE_NVM_UPDATE_CRC16 = 3,   /* WORD ZW_firmwareUpdate_NVM_UpdateCRC16(WORD wCrc16Seed, DWORD nvmOffset, WORD blockSize) */
        FIRMWARE_UPDATE_NVM_IS_VALID_CRC16 = 4, /* BOOL ZW_FirmwareUpdate_NVM_isValidCRC16(WORD *pCRC) */
        FIRMWARE_UPDATE_NVM_WRITE = 5,          /* BYTE ZW_FirmwareUpdate_NVM_Write(BYTE *sourceBuffer, WORD fw_bufSize, DWORD firmwareOffset) */
        FIRMWARE_UPDATE_NVM_UNKNOWN = 0xFF

    }
}
