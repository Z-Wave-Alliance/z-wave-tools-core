/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;

namespace ZWave.Layers.Transport
{
    public enum PInvokeParity : int
    {
        None = 0,
        Odd,
        Even,
        Mark,
        Space
    }

    public enum PInvokeStopBits : int
    {
        None = 0,
        One,
        Two,
        OnePointFive
    }

    public interface ISerialPortProvider : IDisposable
    {
        string PortName { get; }
        bool IsOpen { get; }
        bool Open(string portName, int baudRate, PInvokeParity parity, int dataBits, PInvokeStopBits stopBits);
        int Read(byte[] buffer, int bufferLen);
        byte[] ReadExisting();
        int Write(byte[] buffer, int bufferLen);
        void Close();
    }
}
