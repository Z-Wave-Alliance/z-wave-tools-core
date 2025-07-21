/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.Layers;
using ZWave.Layers.Frame;

namespace ZWave.TextApplication
{
    public class TextFrameLayer : FrameLayer
    {
        public override IFrameClient CreateClient(ushort sessionId)
        {
            IFrameClient ret = new TextFrameClient(TransmitCallback);
            return ret;
        }
    }
}
