/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using Utils.Events;

namespace ZWave.Layers
{
    /// <summary>
    /// Provides the features required to support communication with connected Z-Wave Device.
    /// </summary>
    public interface ITransportLayer
    {
        event EventHandler<EventArgs<DataChunk>> DataTransmitted;
        bool SuppressDebugOutput { get; set; }
        ITransportClient CreateClient(ushort sessionId);
    }
}
