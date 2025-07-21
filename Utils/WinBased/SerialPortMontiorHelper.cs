/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace Utils
{
    public static class SerialPortMontiorHelper
    {
        private static string[] _serialPorts;
        private static ManagementEventWatcher _arrival;
        private static ManagementEventWatcher _removal;

        static SerialPortMontiorHelper()
        {
            _serialPorts = GetAvailableSerialPorts();
            MonitorDeviceChanges();
        }

        /// <summary>
        /// If this method isn't called, an InvalidComObjectException will be thrown (like below):
        /// System.Runtime.InteropServices.InvalidComObjectException was unhandled
        ///Message=COM object that has been separated from its underlying RCW cannot be used.
        ///Source=mscorlib
        ///StackTrace:
        ///     at System.StubHelpers.StubHelpers.StubRegisterRCW(Object pThis, IntPtr pThread)
        ///     at System.Management.IWbemServices.CancelAsyncCall_(IWbemObjectSink pSink)
        ///     at System.Management.SinkForEventQuery.Cancel()
        ///     at System.Management.ManagementEventWatcher.Stop()
        ///     at System.Management.ManagementEventWatcher.Finalize()
        ///InnerException: 
        /// </summary>
        public static void CleanUp()
        {
            _arrival.Stop();
            _removal.Stop();
        }

        public static event EventHandler<PortsChangedArgs> PortsChanged;

        private static void MonitorDeviceChanges()
        {
            try
            {
                var deviceArrivalQuery = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2");

                var deviceRemovalQuery = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 3");

                _arrival = new ManagementEventWatcher(deviceArrivalQuery);
                _removal = new ManagementEventWatcher(deviceRemovalQuery);

                _arrival.EventArrived += (o, args) => RaisePortsChangedIfNecessary(EventType.Insertion);
                _removal.EventArrived += (sender, eventArgs) => RaisePortsChangedIfNecessary(EventType.Removal);

                // Start listening for events.
                _arrival.Start();
                _removal.Start();
            }
            catch (ManagementException err)
            {
                Tools._writeDebugDiagnosticExceptionMessage(err.Message);
            }
        }

        private static void RaisePortsChangedIfNecessary(EventType eventType)
        {
            string[] availableSerialPorts;
            if (eventType == EventType.Insertion)
            {
                lock (_serialPorts)
                {
                    availableSerialPorts = GetAvailableSerialPorts();
                    if (!_serialPorts.SequenceEqual(availableSerialPorts))
                    {
                        var serialPort = availableSerialPorts.Except(_serialPorts).FirstOrDefault();
                        "Port added {0}"._DLOG(serialPort);

                        _serialPorts = availableSerialPorts;
                        PortsChanged.Raise(null, new PortsChangedArgs(eventType, serialPort));
                    }
                }
            }
            else
            {

                PortsChanged.Raise(null, new PortsChangedArgs(eventType, "No matter. We will try."));

                lock (_serialPorts)
                {
                    availableSerialPorts = GetAvailableSerialPorts();
                    if (!_serialPorts.SequenceEqual(availableSerialPorts))
                    {
                        var serialPort = _serialPorts.Except(availableSerialPorts).FirstOrDefault();
                        _serialPorts = availableSerialPorts;
                        "Port closed "._DLOG(serialPort);
                    }
                }
            }
        }

        public static string[] GetAvailableSerialPorts()
        {
            string[] serialPorts = new string[0];
            try
            {
                List<Win32PnPEntityClass> interfaces = ComputerSystemHardwareHelper.GetWin32PnPEntityClassSerialPortDevices();
                if (interfaces != null && interfaces.Count > 0)
                {
                    serialPorts = interfaces.Select(x => x.DeviceId).ToArray();
                }
            }
            catch (Exception)
            {
                "can't get port list from System.Management"._DLOG();
            }
            return serialPorts;
        }

        /// <summary>
        /// Tell subscribers, if any, that this event has been raised.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler">The generic event handler</param>
        /// <param name="sender">this or null, usually</param>
        /// <param name="args">Whatever you want sent</param>
        public static void Raise<T>(this EventHandler<T> handler, object sender, T args) where T : EventArgs
        {
            // Copy to temp var to be thread-safe (taken from C# 3.0 Cookbook - don't know if it's true)
            handler?.Invoke(sender, args);
        }
    }

    public enum EventType
    {
        Insertion,
        Removal
    }

    public class PortsChangedArgs : EventArgs
    {
        public PortsChangedArgs(EventType eventType, string serialPorts)
        {
            EventType = eventType;
            SerialPort = serialPorts;
        }

        public string SerialPort { get; set; }

        public EventType EventType { get; set; }
    }
}
