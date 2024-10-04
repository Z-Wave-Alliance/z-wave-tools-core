/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class IoPortOperation: ControlNApiOperation
    {
        public IoPortOperation()
            : base(CommandTypes.CmdZWaveIoPort, true)
        {
        }

        protected override byte[] CreateInputParameters()
        {
            return null;
        }
    }
}
