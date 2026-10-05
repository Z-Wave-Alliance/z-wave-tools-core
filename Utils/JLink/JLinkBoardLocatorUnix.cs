// SPDX-License-Identifier: BSD-3-Clause
// SPDX-FileCopyrightText: Z-Wave Alliance <https://z-wavealliance.org>
using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    /// <summary>
    /// Linux/macOS back end of <see cref="JLinkBoardLocator"/>.
    ///
    /// There is no WMI, so the USB topology is read from sysfs by
    /// <see cref="UsbSerialPortLocatorUnix"/>, which keeps this free of udev/JLinkExe and needs
    /// no extra privileges. It compiles and is unit tested for every target framework.
    /// </summary>
    internal static class JLinkBoardLocatorUnix
    {
        private const string SysfsUsbDevicesRoot = "/sys/bus/usb/devices";
        private const string DeviceRoot = "/dev";
        private const string JLinkVendorId = "1366";

        /// <summary>Product ids of a SEGGER J-Link, matching <see cref="JLinkUsbMatcher.VidPids"/>.</summary>
        private static readonly string[] JLinkProductIds = new[] { "0105", "1024" };

        internal static List<Tuple<string, string>> GetBoardLinks()
        {
            return Scan(SysfsUsbDevicesRoot, DeviceRoot);
        }

        /// <summary>
        /// Scans a sysfs USB device tree. The roots are parameters so the scan can be unit tested
        /// against a fixture directory on any platform.
        /// </summary>
        internal static List<Tuple<string, string>> Scan(string usbDevicesRoot, string deviceRoot)
        {
            var ret = new List<Tuple<string, string>>();
            foreach (var port in UsbSerialPortLocatorUnix.Scan(usbDevicesRoot, deviceRoot))
            {
                if (!string.Equals(port.VendorId, JLinkVendorId, StringComparison.OrdinalIgnoreCase))
                    continue;

                if (!JLinkProductIds.Contains(port.ProductId, StringComparer.OrdinalIgnoreCase))
                    continue;

                var serial = JLinkUsbMatcher.NormalizeSerial(port.Serial);
                if (serial == null)
                    continue;

                ret.Add(new Tuple<string, string>(port.PortName, serial));
            }

            return ret;
        }
    }
}
