/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZWave.ProgrammerApplication.Enums
{
    public enum WriteRfOptionsStatuses
    {
        /// <summary>
        /// No Errors.
        /// </summary>
        NoErrors = 0,                   //0: No Errors
        /// <summary>
        /// Cannot write Application RF settings to flash page.
        /// </summary>
        CantWriteAppRfSettings = 1,     //1: Cannot write Application RF settings to flash page
        /// <summary>
        /// Cannot read Application RF settings from flash.
        /// </summary>
        CantReadAppRfSettings = 2,      //2: Cannot read Application RF settings from flash
        /// <summary>
        /// Cannot write general RF settings to flash page.
        /// </summary>
        CantWriteGeneralRfSettings = 3, //3: Cannot write general RF settings to flash page
        /// <summary>
        /// Cannot read general RF settings from flash page.
        /// </summary>
        CantReadGeneralRfSettings = 4,  //4: Cannot read general RF settings from flash page
        /// <summary>
        /// RF frequency is not selected.
        /// </summary>
        RfFrequencyNotSelected = 5,     //5: RF freq not selected
        /// <summary>
        /// Undefined RF settings.
        /// </summary>
        UndefinedRfSettings = 6,        //6: Undefined RF settings
        /// <summary>
        /// None.
        /// </summary>
        None = 7                        //7: None
    }
}
