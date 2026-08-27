// SPDX-License-Identifier: BSD-3-Clause
// SPDX-FileCopyrightText: Z-Wave Alliance <https://z-wavealliance.org>
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Utils
{
    /// <summary>
    /// Linux/macOS back end of <see cref="JLinkBoardLocator"/>.
    ///
    /// There is no WMI, so the USB topology is read from sysfs. /sys/bus/usb/devices lists every
    /// USB device and every USB interface; a device carries the idVendor/idProduct/serial
    /// attributes and its interfaces carry the tty the CDC UART is exposed as. Reading sysfs
    /// directly keeps this free of udev/JLinkExe and needs no extra privileges.
    ///
    /// The scan only ever walks downwards, so it neither resolves symlinks nor relies on any API
    /// beyond .NET Framework - it compiles and is unit tested for every target framework.
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
            if (string.IsNullOrEmpty(usbDevicesRoot) || !Directory.Exists(usbDevicesRoot))
                return ret;

            foreach (var usbDevice in Directory.EnumerateDirectories(usbDevicesRoot).OrderBy(x => x, StringComparer.Ordinal))
            {
                try
                {
                    // Interfaces ("1-1:1.0") are listed here as well - they carry no idVendor.
                    if (!string.Equals(ReadAttribute(usbDevice, "idVendor"), JLinkVendorId, StringComparison.OrdinalIgnoreCase))
                        continue;

                    var productId = ReadAttribute(usbDevice, "idProduct");
                    if (productId == null || !JLinkProductIds.Contains(productId, StringComparer.OrdinalIgnoreCase))
                        continue;

                    var serial = JLinkUsbMatcher.NormalizeSerial(ReadAttribute(usbDevice, "serial"));
                    if (serial == null)
                        continue;

                    foreach (var tty in FindTtyNames(usbDevice))
                    {
                        // Device nodes are always "/dev/<name>", independent of the host this runs on.
                        ret.Add(new Tuple<string, string>(deviceRoot.TrimEnd('/') + "/" + tty, serial));
                    }
                }
                catch (Exception ex)
                {
                    "JLinkBoardLocatorUnix error for {0}: {1}"._DLOG(usbDevice, ex.Message);
                }
            }

            return ret;
        }

        /// <summary>
        /// Collects the tty names the interfaces of a USB device expose. cdc_acm places them in
        /// "&lt;interface&gt;/tty/ttyACM0", the usb-serial drivers directly in
        /// "&lt;interface&gt;/ttyUSB0".
        /// </summary>
        private static IEnumerable<string> FindTtyNames(string usbDevice)
        {
            foreach (var child in Directory.EnumerateDirectories(usbDevice).OrderBy(x => x, StringComparer.Ordinal))
            {
                // Only USB interfaces carry a bInterfaceNumber. Anything else - a downstream
                // device of a hub, an endpoint or power directory - is skipped; a downstream
                // device brings its own serial number and is listed at the top level anyway.
                if (!File.Exists(Path.Combine(child, "bInterfaceNumber")))
                    continue;

                var ttyDirectory = Path.Combine(child, "tty");
                if (Directory.Exists(ttyDirectory))
                {
                    foreach (var tty in Directory.EnumerateDirectories(ttyDirectory).OrderBy(x => x, StringComparer.Ordinal))
                        yield return Path.GetFileName(tty);
                }

                foreach (var tty in Directory.EnumerateDirectories(child, "tty*").OrderBy(x => x, StringComparer.Ordinal))
                {
                    var name = Path.GetFileName(tty);
                    if (!string.Equals(name, "tty", StringComparison.Ordinal))
                        yield return name;
                }
            }
        }

        private static string ReadAttribute(string directory, string attribute)
        {
            var file = Path.Combine(directory, attribute);
            if (!File.Exists(file))
                return null;

            var value = File.ReadAllText(file).Trim();
            return value.Length == 0 ? null : value;
        }
    }
}
