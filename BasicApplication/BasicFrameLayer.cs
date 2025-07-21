/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using Utils;
using ZWave.Layers;
using ZWave.Layers.Frame;

namespace ZWave.BasicApplication
{
    public class BasicFrameLayer : FrameLayer
    {
        public override IFrameClient CreateClient(ushort sessionId)
        {
            IFrameClient ret = new BasicFrameClient(TransmitCallback);
            return ret;
        }
    }
}
