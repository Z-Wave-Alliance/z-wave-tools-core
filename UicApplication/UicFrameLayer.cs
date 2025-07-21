/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave;
using ZWave.Layers;
using ZWave.Layers.Frame;

namespace UicApplication
{
    public class UicFrameLayer : FrameLayer
    {
        public override IFrameClient CreateClient(ushort sessionId)
        {
            return (IFrameClient)new UicFrameClient(TransmitCallback);
        }
    }
}
