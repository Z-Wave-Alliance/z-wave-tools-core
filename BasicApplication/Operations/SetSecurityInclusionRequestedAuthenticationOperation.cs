/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.BasicApplication.Enums;
using ZWave.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class SetSecurityInclusionRequestedAuthenticationOperation : ControlApiOperation
    {
        private SecurityAuthentications SecurityAuthentication { get; set; }
        public SetSecurityInclusionRequestedAuthenticationOperation(SecurityAuthentications securityAuthentication)
            : base(CommandTypes.CmdZWaveSecuritySetup, false)
        {
            SecurityAuthentication = securityAuthentication;
        }

        protected override byte[] CreateInputParameters()
        {
            return new byte[] { 0x06, 0x01, (byte)SecurityAuthentication };
        }
    }
}
