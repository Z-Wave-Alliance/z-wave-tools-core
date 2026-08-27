// SPDX-License-Identifier: BSD-3-Clause
// SPDX-FileCopyrightText: Z-Wave Alliance <https://z-wavealliance.org>
using System;
using System.Collections.Generic;
using System.Management;
using System.Runtime.InteropServices;

namespace Utils
{
    /// <summary>
    /// Windows back end of <see cref="JLinkBoardLocator"/>. Reads the USB topology from WMI.
    ///
    /// The WMI queries are guarded rather than the class being annotated with
    /// [SupportedOSPlatform("windows")], because that attribute does not exist on .NET
    /// Framework. The guard keeps the platform compatibility analyzer happy on .NET (Core)
    /// and makes the class harmless if it is ever called on a non Windows host.
    /// </summary>
    internal static class JLinkBoardLocatorWindows
    {
        private const string Scope = @"root\CIMV2";
        private const string UsbControllerDeviceQuery = @"SELECT * FROM Win32_USBControllerDevice";
        private const string SerialPortEntityQuery =
            @"SELECT * FROM Win32_PnPEntity WHERE ClassGuid = '{4D36E978-E325-11CE-BFC1-08002BE10318}'";

        internal static List<Tuple<string, string>> GetBoardLinks()
        {
            var topology = JLinkUsbMatcher.BuildTopology(QueryUsbControllerDevices());
            return JLinkUsbMatcher.MatchPorts(topology, QuerySerialPortEntities());
        }

        private static List<string> QueryUsbControllerDevices()
        {
            var ret = new List<string>();
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return ret;

            using (var searcher = new ManagementObjectSearcher(Scope, UsbControllerDeviceQuery))
            {
                try
                {
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        using (queryObj)
                        {
                            ret.Add(queryObj.GetPropertyValue("Dependent") as string);
                        }
                    }
                }
                catch (Exception ex)
                {
                    "JLinkBoardLocatorWindows Win32_USBControllerDevice error: {0}"._DLOG(ex.Message);
                }
            }

            return ret;
        }

        private static List<KeyValuePair<string, string>> QuerySerialPortEntities()
        {
            var ret = new List<KeyValuePair<string, string>>();
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return ret;

            using (var searcher = new ManagementObjectSearcher(Scope, SerialPortEntityQuery))
            {
                try
                {
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        using (queryObj)
                        {
                            ret.Add(new KeyValuePair<string, string>(
                                queryObj.GetPropertyValue("DeviceID") as string,
                                queryObj.GetPropertyValue("Name") as string));
                        }
                    }
                }
                catch (Exception ex)
                {
                    "JLinkBoardLocatorWindows Win32_PnPEntity error: {0}"._DLOG(ex.Message);
                }
            }

            return ret;
        }
    }
}
