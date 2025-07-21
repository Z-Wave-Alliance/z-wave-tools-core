/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZWave.Enums;
using ZWave.ZnifferApplication.Enums;
using ZWave.Layers;
using Utils;

namespace ZWave.ZnifferApplication
{
    public class ZnifferApiHandler : CommandHandler
    {
        public ZnifferApiHandler(CommandTypes command)
        {
            Mask = new ByteIndex[] { new ByteIndex((byte)command) };
        }
    }
}
