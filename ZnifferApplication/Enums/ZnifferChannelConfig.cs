/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave Alliance https://z-wavealliance.org

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZWave.ZnifferApplication.Enums
{
    public enum ZnifferChannelConfig : byte
    {
        ClassicChannelsAndLRChannelA = 1,
        ClassicChannelsAndLRChannelB = 2,
        LRChannelsAAndBOnly = 3,
    }
}
