/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using Utils.Events;

namespace ZWave.Layers.Frame
{
    public abstract class FrameLayer : IFrameLayer
    {
        public bool SuppressDebugOutput { get; set; }
        public event EventHandler<EventArgs<IDataFrame>> FrameTransmitted;
        public abstract IFrameClient CreateClient(ushort sessionId);
        protected void TransmitCallback(IDataFrame dataFrame)
        {
            FrameTransmitted?.Invoke(this, new EventArgs<IDataFrame>(dataFrame));
        }
    }
}
