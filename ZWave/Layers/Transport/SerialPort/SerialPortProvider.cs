// SPDX-License-Identifier: BSD-3-Clause
// SPDX-FileCopyrightText: Silicon Laboratories Inc. <https://www.silabs.com>
// SPDX-FileCopyrightText: Z-Wave Alliance <https://z-wavealliance.org>
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

        // Guards the changes of _port, _portName and _reservedPortName. It is never held while a port is disposed.
        private readonly object _stateLock = new object();

        // Close() and Dispose() set this to null while the read thread is using the port, so Read(), ReadExisting() and Write()
        // work on a copy without the lock.
        private volatile SerialPort _port;
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
                SerialPort port = _port;
                return port != null && port.IsOpen;
            }
        }

        public bool Open(string portName, int baudRate, PInvokeParity parity, int dataBits, PInvokeStopBits stopBits)
        {
            lock (_stateLock)
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
            ReleasePortName(_reservedPortName);
            _reservedPortName = null;
        }

        private static void ReleasePortName(string portName)
        {
            if (portName != null)
            {
                lock (_access)
                {
                    _openPortNames.Remove(portName);
                }
            }
        }

        private bool TryOpenPort()
        {
            var ret = false;
            // The port is opened under the lock, so that Close() and Dispose() cannot take it away in between and leave an open
            // port nobody owns.
            lock (_stateLock)
            {
                // Only a provider that still holds its port name may (re)open the port.
                SerialPort port = _port;
                if (port != null && _reservedPortName != null)
                {
                    try
                    {
                        port.Open();
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
                SerialPort port = _port;
                for (int i = 0; i < 2; i++)
                {
                    // A read that failed because the port was closed meanwhile is not repeated.
                    if (port == null || port != _port)
                    {
                        break;
                    }
                    try
                    {
                        ret = port.Read(buffer, 0, bufferLen);
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
            SerialPort port = _port;
            if (port == null || !port.IsOpen)
            {
                return null;
            }
            try
            {
                // Read the raw bytes. SerialPort.ReadExisting() decodes them as text and would
                // replace every byte above 0x7F.
                int available = port.BytesToRead;
                if (available > 0)
                {
                    var buffer = new byte[available];
                    int readLen = port.Read(buffer, 0, available);
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
                SerialPort port = _port;
                if (port == null)
                {
                    return ret;
                }
                try
                {
                    port.Write(buffer, 0, bufferLen);
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
            SerialPort port = null;
            string portName;
            string reservedPortName;
            lock (_stateLock)
            {
                portName = _portName;
                reservedPortName = _reservedPortName;
                _reservedPortName = null;
                if (IsOpen)
                {
                    port = _port;
                    _port = null;
                    _portName = null;
                }
            }

            if (port != null)
            {
                try
                {
                    port.DiscardInBuffer();
                    port.DiscardOutBuffer();
                }
                catch (IOException)
                {

                }
                catch (InvalidOperationException)
                {
                }
                DisposePort(port, portName);
                Thread.Sleep(200);
            }

            // The name stays in _openPortNames until here, so no provider opens the port while it is being closed.
            ReleasePortName(reservedPortName);
        }

        public void Dispose()
        {
            SerialPort port;
            string portName;
            string reservedPortName;
            lock (_stateLock)
            {
                port = _port;
                portName = _portName;
                reservedPortName = _reservedPortName;
                _port = null;
                _reservedPortName = null;
            }

            if (port != null)
            {
                DisposePort(port, portName);
            }

            ReleasePortName(reservedPortName);
        }

        // The dispose of a System.IO.Ports.SerialPort waits for its event loop thread, which some USB serial drivers never
        // let end, so the thread doing it is given up after this time.
        private const int DISPOSE_TIMEOUT_MS = 3000;

        /// <summary>
        /// Disposes the port on a background thread and waits <see cref="DISPOSE_TIMEOUT_MS"/> for it at most. A port whose
        /// dispose did not return in time is given up and stays open in this process.
        /// </summary>
        private static void DisposePort(SerialPort port, string portName)
        {
            var disposeThread = new Thread(() =>
            {
                try
                {
                    port.Dispose();
                }
                catch (IOException)
                {
                }
            })
            {
                IsBackground = true,
                Name = "SerialPortDispose " + portName
            };
            disposeThread.Start();
            if (!disposeThread.Join(DISPOSE_TIMEOUT_MS))
            {
                "The dispose of the serial port {0} did not return within {1} ms, the port is given up"._DLOG(portName, DISPOSE_TIMEOUT_MS);
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
