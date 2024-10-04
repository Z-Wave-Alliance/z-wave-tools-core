/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class SetWutTimeoutOperation : ControlNApiOperation
    {
        private byte Timeout { get; set; }
        public SetWutTimeoutOperation(byte timeout)
            : base(CommandTypes.CmdZWaveSetWutTimeout)
        {
            Timeout = timeout;
        }

        protected override byte[] CreateInputParameters()
        {
            return new byte[] { Timeout };
        }
    }
}
