// SPDX-License-Identifier: BSD-3-Clause
// SPDX-FileCopyrightText: Z-Wave Alliance <https://z-wavealliance.org>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Utils
{
    /// <summary>
    /// Windows back end of <see cref="UsbSerialPortLocator"/>. Reads the serial ports from WMI.
    ///
    /// The WMI query is guarded rather than the class being annotated with
    /// [SupportedOSPlatform("windows")], because that attribute does not exist on .NET
    /// Framework. The guard keeps the platform compatibility analyzer happy on .NET (Core)
    /// and makes the class harmless if it is ever called on a non Windows host.
    /// </summary>
    internal static class UsbSerialPortLocatorWindows
    {
        private const string Scope = @"root\CIMV2";
        // Ports class, limited to devices that work (ConfigManagerErrorCode 0).
        private const string SerialPortEntityQuery =
            @"SELECT DeviceID, Name FROM Win32_PnPEntity WHERE ClassGuid = '{4D36E978-E325-11CE-BFC1-08002BE10318}' AND ConfigManagerErrorCode = 0";

        private static readonly Regex PortNameRegex = new Regex(@"\((COM\d+)\)", RegexOptions.Compiled);

        /// <param name="vendorId">Four hex digits without prefix, e.g. "0658".</param>
        /// <param name="productId">Four hex digits without prefix, e.g. "0200".</param>
        internal static string[] GetPortNames(string vendorId, string productId)
        {
            return MatchPorts(QuerySerialPortEntities(), vendorId, productId);
        }

        /// <summary>
        /// Picks the ports whose DeviceID names the vendor and product id. USB devices report
        /// "USB\VID_0658&amp;PID_0200\...", FTDI devices "FTDIBUS\VID_0403+PID_6001+...", so only
        /// the order of the two ids is relied upon, not the separator.
        /// </summary>
        /// <param name="serialPortEntities">Win32_PnPEntity DeviceID -&gt; Name pairs of the Ports class.</param>
        internal static string[] MatchPorts(IEnumerable<KeyValuePair<string, string>> serialPortEntities, string vendorId, string productId)
        {
            var ret = new List<string>();
            if (serialPortEntities == null || string.IsNullOrEmpty(vendorId) || string.IsNullOrEmpty(productId))
                return ret.ToArray();

            var vendorToken = "VID_" + vendorId;
            var productToken = "PID_" + productId;
            foreach (var entity in serialPortEntities)
            {
                var deviceId = entity.Key;
                var name = entity.Value;
                if (string.IsNullOrEmpty(deviceId) || string.IsNullOrEmpty(name))
                    continue;

                int vendorIndex = deviceId.IndexOf(vendorToken, StringComparison.OrdinalIgnoreCase);
                if (vendorIndex < 0)
                    continue;

                if (deviceId.IndexOf(productToken, vendorIndex + vendorToken.Length, StringComparison.OrdinalIgnoreCase) < 0)
                    continue;

                // The port name is only part of the label, e.g. "UZB (COM5)".
                var match = PortNameRegex.Match(name);
                if (match.Success)
                    ret.Add(match.Groups[1].Value);
            }

            return ret.Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
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
                    "UsbSerialPortLocatorWindows Win32_PnPEntity error: {0}"._DLOG(ex.Message);
                }
            }

            return ret;
        }
    }
}
