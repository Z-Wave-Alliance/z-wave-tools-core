/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿namespace ZWave.BasicApplication.Enums
{
    /*Gecko Bootloader v1.5.1
    * 1. upload gbl
    * 2. run
    * 3. ebl info
    * BL >
    */
    public enum XModemRunOptions : byte
    {
        UploadGbl = 0x31, // '1'
        Run = 0x32, // '2'
        EblInfo = 0x33 // '3'
    }
}