/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System;
using System.Collections.Generic;
using System.Management;
#if NETCOREAPP
using System.Runtime.Versioning;
#endif

namespace Utils
{
    /// <summary>
    /// Windows back end of <see cref="JLinkBoardLocator"/>. Reads the USB topology from WMI.
    /// Must only be called on Windows - see <see cref="JLinkBoardLocator.GetBoardLinks"/>.
    /// </summary>
#if NETCOREAPP
    [SupportedOSPlatform("windows")]
#endif
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
