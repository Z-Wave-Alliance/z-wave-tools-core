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
    public class SnifferPtiApiHandler : CommandHandler
    {
        public SnifferPtiApiHandler()
        {
            Mask = new ByteIndex[] { ByteIndex.AnyValue };
        }

        public override bool WaitingFor(IActionCase actionCase)
        {
            return base.WaitingFor(actionCase);
        }
    }
}
