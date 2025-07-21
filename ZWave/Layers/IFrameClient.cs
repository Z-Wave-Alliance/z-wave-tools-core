/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using ZWave.Layers.Frame;

namespace ZWave.Layers
{
    public interface IFrameClient : IDisposable
    {
        ushort SessionId { get; set; }
        Action<CustomDataFrame> ReceiveFrameCallback { get; set; }
        Func<byte[], int> SendDataCallback { get; set; }
        void HandleData(DataChunk dataChunk, bool isFromFile);
        bool SendFrames(ActionHandlerResult frameData);
        void ResetParser();
    }
}
