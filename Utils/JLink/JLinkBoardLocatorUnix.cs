/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System;
using System.Collections.Generic;
#if NETCOREAPP
using System.IO;
using System.Linq;
#endif

namespace Utils
{
#if NETCOREAPP
    /// <summary>
    /// Linux/macOS back end of <see cref="JLinkBoardLocator"/>.
    ///
    /// There is no WMI, so the USB topology is read from sysfs: every serial port under
    /// /sys/class/tty has a "device" link into the USB interface it belongs to, and the
    /// idVendor/idProduct/serial attributes live on the USB device above that interface.
    /// Reading sysfs directly keeps this free of udev/JLinkExe and needs no extra privileges.
    /// </summary>
    internal static class JLinkBoardLocatorUnix
    {
        private const string SysfsTtyRoot = "/sys/class/tty";
        private const string DeviceRoot = "/dev";
        private const string JLinkVendorId = "1366";

        /// <summary>Product ids of a SEGGER J-Link, matching <see cref="JLinkUsbMatcher.VidPids"/>.</summary>
        private static readonly string[] JLinkProductIds = new[] { "0105", "1024" };

        /// <summary>A tty hangs off a USB interface; walking up a few levels always reaches the device.</summary>
        private const int MaxParentWalk = 8;

        internal static List<Tuple<string, string>> GetBoardLinks()
        {
            return Scan(SysfsTtyRoot, DeviceRoot);
        }

        /// <summary>
        /// Scans a sysfs tty tree. The roots are parameters so the scan can be unit tested
        /// against a fixture directory.
        /// </summary>
        internal static List<Tuple<string, string>> Scan(string sysfsTtyRoot, string deviceRoot)
        {
            var ret = new List<Tuple<string, string>>();
            if (string.IsNullOrEmpty(sysfsTtyRoot) || !Directory.Exists(sysfsTtyRoot))
                return ret;

            foreach (var ttyDirectory in Directory.EnumerateDirectories(sysfsTtyRoot))
            {
                try
                {
                    var usbDevice = FindUsbDeviceDirectory(Path.Combine(ttyDirectory, "device"));
                    if (usbDevice == null)
                        continue;

                    if (!string.Equals(ReadAttribute(usbDevice, "idVendor"), JLinkVendorId, StringComparison.OrdinalIgnoreCase))
                        continue;

                    var productId = ReadAttribute(usbDevice, "idProduct");
                    if (productId == null || !JLinkProductIds.Contains(productId, StringComparer.OrdinalIgnoreCase))
                        continue;

                    var serial = JLinkUsbMatcher.NormalizeSerial(ReadAttribute(usbDevice, "serial"));
                    if (serial == null)
                        continue;

                    // Device nodes are always "/dev/<name>", independent of the host this runs on.
                    var portName = deviceRoot.TrimEnd('/') + "/" + Path.GetFileName(ttyDirectory);
                    ret.Add(new Tuple<string, string>(portName, serial));
                }
                catch (Exception ex)
                {
                    "JLinkBoardLocatorUnix error for {0}: {1}"._DLOG(ttyDirectory, ex.Message);
                }
            }

            return ret;
        }

        /// <summary>
        /// Resolves the tty "device" link and walks up until the USB device carrying the
        /// idVendor/idProduct/serial attributes is reached.
        /// </summary>
        private static string FindUsbDeviceDirectory(string deviceLink)
        {
            var current = ResolveDirectory(deviceLink);
            for (int depth = 0; current != null && depth < MaxParentWalk; depth++)
            {
                if (File.Exists(Path.Combine(current, "idVendor")))
                    return current;

                current = Path.GetDirectoryName(current);
            }

            return null;
        }

        private static string ResolveDirectory(string path)
        {
            if (!Directory.Exists(path))
                return null;

            // Returns null when the path is a plain directory rather than a symlink.
            var target = Directory.ResolveLinkTarget(path, returnFinalTarget: true);
            return target != null ? target.FullName : Path.GetFullPath(path);
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
#endif
}
