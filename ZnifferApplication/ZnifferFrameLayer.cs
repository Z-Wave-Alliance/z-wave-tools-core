/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZWave.Layers;
using ZWave.Xml.FrameHeader;
using ZWave.Layers.Frame;

namespace ZWave.ZnifferApplication
{
    public class ZnifferFrameLayer : FrameLayer
    {
        public FrameDefinition FrameDefinition { get; set; }
        public ZnifferFrameLayer(FrameDefinition frameDefinition)
            : base()
        {
            FrameDefinition = frameDefinition;
        }

        public override IFrameClient CreateClient(ushort sessionId)
        {
            IFrameClient ret = new ZnifferFrameClient(TransmitCallback, FrameDefinition);
            return ret;
        }
    }
}
