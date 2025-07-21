/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using Utils.Events;
using ZWave.Layers;


namespace UicApplication
{
    public class UicSocketTransportLayer : ITransportLayer
    {
        public event EventHandler<EventArgs<DataChunk>> DataTransmitted;
        public bool SuppressDebugOutput{ get; set; }
        public UicSocketTransportClient CreateClient()
        {
            return new UicSocketTransportClient(
                dataChunk => DataTransmitted?.Invoke(this, new EventArgs<DataChunk>(dataChunk)))
            {
            };
        }
        public ITransportClient CreateClient(ushort sessionId)
        {
            return new UicSocketTransportClient(
                dataChunk => DataTransmitted?.Invoke(this, new EventArgs<DataChunk>(dataChunk)))
            {
                
            };
        }
    }
}
