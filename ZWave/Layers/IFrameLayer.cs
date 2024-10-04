/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using Utils.Events;

namespace ZWave.Layers
{
    /// <summary>
    /// Provides the features required to support Data Frame manipulation.
    /// </summary>
    public interface IFrameLayer
    {
        bool SuppressDebugOutput { get; set; }
        IFrameClient CreateClient(ushort sessionId);
        event EventHandler<EventArgs<IDataFrame>> FrameTransmitted;
    }
}
