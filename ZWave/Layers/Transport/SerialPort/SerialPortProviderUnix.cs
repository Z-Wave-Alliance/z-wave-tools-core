/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;

namespace ZWave.Layers.Transport
{
#if NETCOREAPP
    public class SerialPortProviderUnix : ISerialPortProvider
    {
        ISerialPortProvider _internalProvider = new NativeSerialProviderUnix();

        public string PortName
        {
            get { return _internalProvider != null ? _internalProvider.PortName : null; }
        }

        public bool IsOpen
        {
            get { return _internalProvider != null ? _internalProvider.IsOpen : false; }
        }

        public bool Open(string portName, int baudRate, PInvokeParity parity, int dataBits, PInvokeStopBits stopBits)
        {
            if (_internalProvider != null)
                return _internalProvider.Open(portName, baudRate, parity, dataBits, stopBits);
            return false;
        }

        public int Read(byte[] buffer, int bufferLen)
        {
            if (_internalProvider != null)
                return _internalProvider.Read(buffer, bufferLen);
            return -1;
        }

        public byte[] ReadExisting()
        {
            if (_internalProvider != null)
                return _internalProvider.ReadExisting();
            return null;
        }

        public int Write(byte[] buffer, int bufferLen)
        {
            if (_internalProvider != null)
                return _internalProvider.Write(buffer, bufferLen);
            return -1;
        }

        public void Close()
        {
            if (_internalProvider != null)
                _internalProvider.Close();
        }

        public void Dispose()
        {
            if (_internalProvider != null)
            {
                (_internalProvider as IDisposable).Dispose();
                _internalProvider = null;
            }
        }

        public static string[] EnumConnectedDevices()
        {
            return NativeSerialProviderUnix.EnumNativeSerialPorts();
        }
    }
#endif
}
