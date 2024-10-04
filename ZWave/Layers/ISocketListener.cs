/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Net;

namespace ZWave.Layers
{
    public class ReceivedDataArgs
    {
        public byte[] Data { get; set; }
        public string SourceName { get; set; }
        public ushort SourcePort { get; set; }
        public ushort ListenerPort { get; set; }
    }

    public interface IStartListenParams
    {
        string IpAddress { get; set; }
        ushort PortNo { get; set; }

        string InterfaceName { get; set; }
    }

    public interface IDtlsStartListenParams : IStartListenParams
    {
        string PskKey { get; set; }
    }

    public interface ISocketListener : IDisposable
    {
        event Action<ReceivedDataArgs> DataReceived;
        event Action<string, ushort> ConnectionCreated;
        event Action<string, ushort> ConnectionClosed;
        IStartListenParams ListenParams { get; }
        bool IsListening { get; }
        bool Listen(IStartListenParams listenParams);
        int ResponseTo(byte[] data, string address, ushort portNo);
        void Stop();
    }
}
