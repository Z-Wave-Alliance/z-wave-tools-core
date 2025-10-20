/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.IO;
using System.IO.Ports;
using System.Threading;
using Utils;
#if NETCOREAPP
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
#endif

namespace ZWave.Layers.Transport
{
    public class SerialPortProvider : ISerialPortProvider
    {
        public const uint BUFFER_SIZE = 512;

        private ISerialPortProvider _interalProvider;

        public SerialPortProvider()
        {
#if NETCOREAPP
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                _interalProvider = new SerialPortProviderWindows();
            }
            else
            {
                _interalProvider = new SerialPortProviderUnix();
            }
#else
            _interalProvider = new SerialPortProviderWindows();
#endif
        }

        public string PortName { get { return _interalProvider.PortName; } }
        public bool IsOpen { get { return _interalProvider.IsOpen; } }

        public bool Open(string portName, int baudRate, PInvokeParity parity, int dataBits, PInvokeStopBits stopBits)
        {
            return _interalProvider.Open(portName, baudRate, parity, dataBits, stopBits);
        }

        public int Read(byte[] buffer, int bufferLen)
        {
            return _interalProvider.Read(buffer, bufferLen);
        }

        public byte[] ReadExisting()
        {
            return _interalProvider.ReadExisting();
        }

        public int Write(byte[] buffer, int bufferLen)
        {
            return _interalProvider.Write(buffer, bufferLen);
        }

        public void Close()
        {
            _interalProvider.Close();
        }

        public void Dispose()
        {
            ((IDisposable)_interalProvider).Dispose();
        }


        public static string[] GetPortNames(string vid = null, string pid = null)
        {
#if NETCOREAPP
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return SerialPort.GetPortNames();
            }
            else
            {
                var ret = new List<string>();
                if (!string.IsNullOrEmpty(vid) && !string.IsNullOrEmpty(pid))
                {
                    var endl = Environment.NewLine;
                    var listUsbSerial =
                        "for sysdevpath in $(find /sys/bus/usb/devices/usb*/ -name dev); do" + endl +
                            "(" + endl +
                                "syspath=\"${sysdevpath%/dev}\"" + endl +
                                "devname=\"$(udevadm info -q name -p $syspath)\"" + endl +
                                "[[ \"$devname\" == \"bus/\"* ]] && continue" + endl +
                                "eval \"$(udevadm info -q property --export -p $syspath)\"" + endl +
                                "[[ -z \"$ID_SERIAL\" ]] && continue" + endl +
                                "echo \"/dev/$devname - $ID_SERIAL\"" + endl +
                            ")" + endl +
                        "done";
                    listUsbSerial = listUsbSerial.Replace("\"", "\\\"");

                    var shell = Environment.GetEnvironmentVariable("SHELL");
                    if (string.IsNullOrWhiteSpace(shell))
                    {
                        try
                        {
                            var grepUsbSerialProcess = new ProcessProxy(Directory.GetCurrentDirectory(), shell, "-c \"{0} | grep {1}\"");
                            vid = Regex.Replace(vid, "[^0-9]", "");
                            pid = Regex.Replace(pid, "[^0-9]", "");
                            grepUsbSerialProcess.Start(listUsbSerial, $"{vid}_{pid}");
                            grepUsbSerialProcess.Close();
                            if (!string.IsNullOrEmpty(grepUsbSerialProcess.ProcessOutput))
                            {
                                var rx = new Regex(@"tty[A-Z0-9]+\b");
                                var matches = rx.Matches(grepUsbSerialProcess.ProcessOutput);
                                if (matches.Count > 0)
                                    ret.AddRange(matches.Select(x => $"/dev/{x.ToString()}"));
                            }
                        }
                        catch (InvalidOperationException ioex)
                        {
                            ioex.Message._DLOG();
                        }
                    }
                    return ret.ToArray();
                }
                else
                    return SerialPortProviderUnix.EnumConnectedDevices();
            }
#else
            return System.IO.Ports.SerialPort.GetPortNames();
#endif
        }
    }
}
