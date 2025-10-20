/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
namespace ZWave.Layers.Transport
{
    public interface ITcpConnection
    {
        bool Connected { get; }

        bool Connect(string hostname, int portNo, int timeoutMilliseconds = 4000);
        int Read(out byte[] data);
        int Write(byte[] data);
    }
}
