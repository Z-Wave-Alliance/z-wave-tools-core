/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.IO;
using System.IO.Ports;
using System.Threading;
using ZWave.Layers.Transport;

namespace ZWave.Layers.Transport
{
    public class SerialPortProviderWindows : ISerialPortProvider, IDisposable
    {
        private SerialPort _port;

        private string _portName;
        public string PortName
        {
            get
            {
                return _portName;
            }
        }

        public bool IsOpen
        {
            get
            {
                return _port != null && _port.IsOpen;
            }
        }

        public bool Open(string portName, int baudRate, PInvokeParity parity, int dataBits, PInvokeStopBits stopBits)
        {
            if (!IsOpen)
            {
                _portName = portName;
                _port = new SerialPort(_portName, baudRate, (Parity)(int)parity, dataBits, (StopBits)(int)stopBits);
                _port.WriteTimeout = 500;
                return TryOpenPort();
            }
            return false;
        }

        private bool TryOpenPort()
        {
            var ret = false;
            if (_port != null)
            {
                try
                {
                    _port.Open();
                    ret = true;
                }
                catch (ArgumentException)
                {
                }
                catch (UnauthorizedAccessException)
                {
                }
                catch (IOException)
                {
                }
                catch (InvalidOperationException)
                {
                }
            }

            return ret;
        }

        public int Read(byte[] buffer, int bufferLen)
        {
            if (!IsOpen)
            {
                Thread.Sleep(1000);
            }

            // Try reconnect
            if (IsOpen || TryOpenPort())
            {
                int ret = -1;
                for (int i = 0; i < 2; i++)
                {
                    try
                    {
                        ret = _port.Read(buffer, 0, bufferLen);
                        break;
                    }
                    catch (UnauthorizedAccessException)
                    {
                    }
                    catch (OperationCanceledException)
                    {
                    }
                    catch (TimeoutException)
                    {
                    }
                    catch (InvalidOperationException)
                    {
                    }
                    catch (IOException)
                    {
                    }
                    Thread.Sleep(200);
                }
                return ret;
            }

            return -1;
        }

        public byte[] ReadExisting()
        {
            try
            {
                var bytesString = _port.ReadExisting();
                if (!string.IsNullOrEmpty(bytesString))
                {
                    return System.Text.Encoding.UTF8.GetBytes(bytesString);
                }
            }
            catch (InvalidOperationException)
            {
            }
            return null;
        }

        public int Write(byte[] buffer, int bufferLen)
        {
            if (!IsOpen)
            {
                Thread.Sleep(1000);
            }

            // Try reconnect
            if (IsOpen || TryOpenPort())
            {
                int ret = -1;
                try
                {
                    _port.Write(buffer, 0, bufferLen);
                    ret = bufferLen;
                }
                catch (IOException)
                {
                }
                catch (InvalidOperationException)
                {
                }
                catch (System.ServiceProcess.TimeoutException)
                {
                }
                catch (System.TimeoutException)
                {
                }
                return ret;
            }

            return -1;
        }

        public void Close()
        {
            if (IsOpen)
            {
                try
                {
                    _port.DiscardInBuffer();
                    _port.DiscardOutBuffer();
                    _port.Close();
                    Thread.Sleep(200);
                }
                catch (IOException)
                {

                }
                _portName = null;
            }
        }

        public void Dispose()
        {
            if (_port as IDisposable != null)
            {
                ((IDisposable)_port).Dispose();
            }
        }
    }
}
