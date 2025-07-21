/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using ZWave.Enums;
using Utils;
using ZWave.Operations;
using ZWave.Layers.Session;

namespace ZWave.Layers
{
    public interface IApplicationClient : IDisposable
    {
        ushort SessionId { get; set; }
        string Version { get; set; }
        Libraries Library { get; set; }
        ChipTypes ChipType { get; set; }
        byte ChipRevision { get; set; }
        ITransportClient TransportClient { get; set; }
        IFrameClient FrameClient { get; set; }
        ISessionClient SessionClient { get; set; }
        IDataSource DataSource { get; set; }

        CommunicationStatuses Connect(IDataSource ds);
        CommunicationStatuses Connect();
        void Disconnect();
        void Cancel(ActionToken token);
        ActionToken Listen(ByteIndex[] mask, Action<byte[]> data);
        ExpectResult Expect(ByteIndex[] mask, int timeoutMs);
        ActionToken Expect(ByteIndex[] mask, int timeoutMs, Action<IActionItem> completedCallback);
        SendResult Send(byte[] data);
        ActionToken Send(byte[] data, Action<IActionItem> completedCallback);
        RequestResult Request(byte[] data, ByteIndex[] mask, int timeoutMs);
        ActionToken Request(byte[] data, ByteIndex[] mask, int timeoutMs, Action<IActionItem> completedCallback);
    }
}
