/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZWave.Layers;
using ZWave.Layers.Frame;

namespace ZWave.ZipApplication
{
    public class ZipFrameLayer : FrameLayer
    {
        public override IFrameClient CreateClient(ushort sessionId)
        {
            IFrameClient ret = new ZipFrameClient(TransmitCallback);
            return ret;
        }
    }
}
