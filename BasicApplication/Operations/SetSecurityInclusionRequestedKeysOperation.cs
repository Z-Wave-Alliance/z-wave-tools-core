/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;
using ZWave.BasicApplication.Security;
using ZWave.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class SetSecurityInclusionRequestedKeysOperation : ControlApiOperation
    {
        private SecuritySchemes[] SecuritySchemes { get; set; }
        public SetSecurityInclusionRequestedKeysOperation(SecuritySchemes[] securitySchemes)
            : base(CommandTypes.CmdZWaveSecuritySetup, false)
        {
            SecuritySchemes = securitySchemes;
        }

        protected override byte[] CreateInputParameters()
        {
            byte mask = (byte)SecurityManagerInfo.ConvertToNetworkKeyMask(SecuritySchemes);
            return new byte[] { 0x05, 0x01, mask };
        }
    }
}
