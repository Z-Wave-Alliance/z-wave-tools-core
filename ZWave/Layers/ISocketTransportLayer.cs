/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
namespace ZWave.Layers
{
    public interface ISocketTransportLayer : ITransportLayer
    {
        ISocketListener Listener { get; set; }
    }
}
