/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class SetSecurityBypassOperation : ControlNApiOperation
    {
        private byte _bypassState;
        private const byte CMD_SET_BYPASS = 2;
        public SetSecurityBypassOperation(byte bypassState) : base(CommandTypes.CmdSerialApiTestSetup)
        {
            _bypassState = bypassState;
        }

        protected override byte[] CreateInputParameters()
        {
            return new byte[] { CMD_SET_BYPASS, (byte)_bypassState };
        }
    }
}