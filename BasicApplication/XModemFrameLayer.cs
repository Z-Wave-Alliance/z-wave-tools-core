/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.Layers;
using ZWave.Layers.Frame;

namespace ZWave.BasicApplication
{
    public class XModemFrameLayer : FrameLayer
    {
        public override IFrameClient CreateClient(ushort sessionId)
        {
            IFrameClient ret = new XModemFrameClient();
            return ret;
        }
    }
}
