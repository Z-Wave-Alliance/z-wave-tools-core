/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// To reset the transmit counter:
    /// HOST->ZW: REQ | 0x82|  (FUNC_ID_RESET_TX_COUNTER)
    /// </summary>
    public class ResetTransmitCountOperation : ControlNApiOperation
    {
        public ResetTransmitCountOperation()
            : base(CommandTypes.CmdResetTXCounter)
        {
        }

        protected override byte[] CreateInputParameters()
        {
            return null;
        }
    }
}
