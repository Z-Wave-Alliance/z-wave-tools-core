/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using Utils.Events;
using ZWave.Layers;

namespace ZWave.ZipApplication
{
    public class Dtls1ClientTransportLayer : IDtlsTransportLayer, IDisposable
    {
        public event EventHandler<EventArgs<DataChunk>> DataTransmitted;
        public bool SuppressDebugOutput { get; set; }
        public ISocketListener Listener { get; set; }
        public ISocketListener ListenerSecond { get; set; }

        public ITransportClient CreateClient(ushort sessionId)
        {
            return new Dtls1ClientTransportClient(dataChunk => DataTransmitted?.Invoke(this, new EventArgs<DataChunk>(dataChunk)))
            {
                SuppressDebugOutput = SuppressDebugOutput,
                SessionId = sessionId
            };
        }

        #region IDisposable Members

        public void Dispose()
        {
            Listener?.Dispose();
            Listener = null;
            ListenerSecond?.Dispose();
            ListenerSecond = null;
        }

        #endregion
    }
}
