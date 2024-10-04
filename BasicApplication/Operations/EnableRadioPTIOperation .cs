/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Linq;
using Utils;
using ZWave.BasicApplication.Enums;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// FUNC_ID_ENABLE_RADIO_PTI  0xE7
    /// HOST->ZW: REQ | 0xE7 | PTIState
    /// ZW->HOST: RES | 0xE7 | RetVal
    /// </summary>
    public class EnableRadioPTIOperation : ControlNApiOperation
    {
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Initializes a new instance of the EnableRadioPTIOperation class with specified state.
        /// </summary>
        /// <param name="isEnabled">Enable/Disable Radio PTI support.</param>
        public EnableRadioPTIOperation(bool isEnabled)
            : base(CommandTypes.CmdEnableRadioPTI)
        {
            IsEnabled = isEnabled;
        }

        protected override byte[] CreateInputParameters()
        {
            return new byte[] { (byte)(IsEnabled ? 1 : 0) };
        }
    }
}