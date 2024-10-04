/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class IoPortStatusOperation: ControlNApiOperation
    {
        private byte Enable { get; set; }
        public IoPortStatusOperation(byte enable)
            : base(CommandTypes.CmdZWaveIoPortStatus, true)
        {
            Enable = enable;
        }

        protected override byte[] CreateInputParameters()
        {
            return new byte[] { Enable };
        }
    }
}
