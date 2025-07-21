/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using ZWave.Enums;
using Utils;

namespace ZWave.Layers
{
    public interface ITransportClient : IDisposable
    {
        event Action<ITransportClient> Connected;
        event Action<ITransportClient> Disconnected;

        ushort SessionId { get; set; }
        ApiTypes ApiType { get; set; }
        IDataSource DataSource { get; set; }
        bool IsOpen { get; }
        CommunicationStatuses Connect(IDataSource dataSource);
        CommunicationStatuses Connect();
        void Disconnect();
        int WriteData(byte[] data);
        /// <summary>
        /// bool value indicates if received data is from file
        /// </summary>
        Action<DataChunk, bool> ReceiveDataCallback { get; set; }
        bool SuppressDebugOutput { get; set; }
    }
}
