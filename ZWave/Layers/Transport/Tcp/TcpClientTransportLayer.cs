/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using Utils.Events;

namespace ZWave.Layers.Transport
{
    public class TcpClientTransportLayer : ISocketTransportLayer
    {
        public event EventHandler<EventArgs<DataChunk>> DataTransmitted;
        public bool SuppressDebugOutput { get; set; }
        public ISocketListener Listener { get; set; }

        public ITransportClient CreateClient(ushort sessionId)
        {
            var ret = new TcpClientTransportClient(dataChunk => DataTransmitted?.Invoke(this, new EventArgs<DataChunk>(dataChunk)))
            {
                SuppressDebugOutput = SuppressDebugOutput,
                SessionId = sessionId
            };
            return ret;
        }
    }
}
