/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Threading;
using Utils;

namespace ZWave.Layers.Transport
{
    /// <summary>
    /// Serial port access built on System.IO.Ports, which covers Windows, Linux and macOS alike.
    /// </summary>
    public class SerialPortProvider : ISerialPortProvider
    {
        public const uint BUFFER_SIZE = 512;

        // Ports opened through any provider of this process. System.IO.Ports does not guarantee
        // on every platform that a second open of the same port fails, so it is refused here.
        private static readonly object _access = new object();
        private static readonly HashSet<string> _openPortNames = new HashSet<string>(
            IsWindows ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);

        private static bool IsWindows
        {
            get { return RuntimeInformation.IsOSPlatform(OSPlatform.Windows); }
        }

        private SerialPort _port;
        // Name this provider holds in _openPortNames, null while it holds none.
        private string _reservedPortName;

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
                ReleasePortName();
                if (!ReservePortName(portName))
                {
                    return false;
                }
                _portName = portName;
                _port = new SerialPort(_portName, baudRate, (Parity)(int)parity, dataBits, (StopBits)(int)stopBits);
                _port.WriteTimeout = 500;
                if (TryOpenPort())
                {
                    return true;
                }
                ReleasePortName();
            }
            return false;
        }

        private bool ReservePortName(string portName)
        {
            if (string.IsNullOrEmpty(portName))
            {
                return false;
            }
            lock (_access)
            {
                if (!_openPortNames.Add(portName))
                {
                    return false;
                }
            }
            _reservedPortName = portName;
            return true;
        }

        private void ReleasePortName()
        {
            if (_reservedPortName != null)
            {
                lock (_access)
                {
                    _openPortNames.Remove(_reservedPortName);
                }
                _reservedPortName = null;
            }
        }

        private bool TryOpenPort()
        {
            var ret = false;
            // Only a provider that still holds its port name may (re)open the port.
            if (_port != null && _reservedPortName != null)
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
            if (!IsOpen)
            {
                return null;
            }
            try
            {
                // Read the raw bytes. SerialPort.ReadExisting() decodes them as text and would
                // replace every byte above 0x7F.
                int available = _port.BytesToRead;
                if (available > 0)
                {
                    var buffer = new byte[available];
                    int readLen = _port.Read(buffer, 0, available);
                    if (readLen > 0)
                    {
                        if (readLen < available)
                        {
                            Array.Resize(ref buffer, readLen);
                        }
                        return buffer;
                    }
                }
            }
            catch (InvalidOperationException)
            {
            }
            catch (IOException)
            {
            }
            catch (TimeoutException)
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
            ReleasePortName();
        }

        public void Dispose()
        {
            ReleasePortName();
            if (_port as IDisposable != null)
            {
                ((IDisposable)_port).Dispose();
            }
        }

        /// <summary>
        /// Returns the names of the serial ports. A vendor and product id ("VID_0658", "PID_0200")
        /// limit the result to the ports of matching USB devices.
        /// </summary>
        public static string[] GetPortNames(string vid = null, string pid = null)
        {
            if (!string.IsNullOrEmpty(vid) && !string.IsNullOrEmpty(pid))
            {
                return UsbSerialPortLocator.GetPortNames(vid, pid);
            }
            return SerialPort.GetPortNames();
        }
    }
}
