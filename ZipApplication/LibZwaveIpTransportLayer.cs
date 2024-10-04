/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using Utils.Events;
using ZWave.Layers;

namespace ZWave.ZipApplication
{
    public class LibZwaveIpTransportLayer : IDisposable
    {
        public event EventHandler<EventArgs<DataChunk>> DataTransmitted;
        public bool SuppressDebugOutput { get; set; }

        #region IDisposable Members

        public void Dispose()
        {            

        }

        #endregion
    }
}
