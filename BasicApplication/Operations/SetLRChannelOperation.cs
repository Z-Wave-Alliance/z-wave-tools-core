/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using ZWave.BasicApplication.Enums;
using Utils;
using ZWave.Devices;
using System.Linq;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0xDB | Channel
    /// ZW->HOST: RES | 0xDB | RetVal
    /// </summary>
    public class SetLRChannelOperation : ControlNApiOperation
    {
        public LongRangeChannels LRChannel { get; set; }
        public SetLRChannelOperation(LongRangeChannels channel)
            : base(CommandTypes.CmdSetLRChannel)
        {
            /// FUNC_ID_SET_LR_CHANNEL
            LRChannel = channel;
        }

        protected override byte[] CreateInputParameters()
        {
            return new byte[] { (byte)LRChannel };
        }

    }
}
