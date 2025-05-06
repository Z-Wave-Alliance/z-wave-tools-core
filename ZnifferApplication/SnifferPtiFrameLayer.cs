/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZWave.Layers;
using ZWave.Layers.Frame;
using ZWave.Xml.FrameHeader;

namespace ZWave.ZnifferApplication
{
    public class SnifferPtiFrameLayer : FrameLayer
    {
        public FrameDefinition FrameDefinition { get; set; }
        public SnifferPtiFrameLayer(FrameDefinition frameDefinition)
            : base()
        {
            FrameDefinition = frameDefinition;
        }

        public override IFrameClient CreateClient(ushort sessionId)
        {
            IFrameClient ret = new SnifferPtiFrameClient(TransmitCallback, FrameDefinition);
            return ret;
        }
    }
}
