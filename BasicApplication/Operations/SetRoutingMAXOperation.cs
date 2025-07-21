/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0xD4 | maxRoutesTries
    /// ZW->HOST: RES | 0xD4 | TRUE
    /// </summary>
    class SetRoutingMAXOperation : ControlApiOperation
    {
        private byte MaxRouteTries { get; set; }
        public SetRoutingMAXOperation(byte maxRouteTries)
            : base(CommandTypes.CmdZWaveSetRoutingMAX, false)
        {
            MaxRouteTries = maxRouteTries;
        }

        protected override byte[] CreateInputParameters()
        {
            return new byte[] { MaxRouteTries };
        }
    }
}
