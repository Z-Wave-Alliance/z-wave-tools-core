// SPDX-License-Identifier: BSD-3-Clause
// SPDX-FileCopyrightText: Z-Wave Alliance <https://z-wavealliance.org>
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Utils
{
    /// <summary>A serial port exposed by a USB device, as found in sysfs.</summary>
    internal sealed class UsbSerialPortInfo
    {
        /// <summary>Device node, e.g. "/dev/ttyACM0".</summary>
        public string PortName { get; set; }
        /// <summary>USB vendor id as four hex digits, e.g. "0658".</summary>
        public string VendorId { get; set; }
        /// <summary>USB product id as four hex digits, e.g. "0200".</summary>
        public string ProductId { get; set; }
        /// <summary>USB serial string as reported by the device, or null when it has none.</summary>
        public string Serial { get; set; }
    }

    /// <summary>
    /// Linux/macOS back end of <see cref="UsbSerialPortLocator"/>.
    ///
    /// There is no WMI, so the USB topology is read from sysfs. /sys/bus/usb/devices lists every
    /// USB device and every USB interface; a device carries the idVendor/idProduct/serial
    /// attributes and its interfaces carry the tty the UART is exposed as. Reading sysfs
    /// directly keeps this free of udev and needs no extra privileges.
    ///
    /// The scan only ever walks downwards, so it neither resolves symlinks nor relies on any API
    /// beyond .NET Framework - it compiles and is unit tested for every target framework.
    /// </summary>
    internal static class UsbSerialPortLocatorUnix
    {
        private const string SysfsUsbDevicesRoot = "/sys/bus/usb/devices";
        private const string DeviceRoot = "/dev";

        /// <param name="vendorId">Four hex digits without prefix, e.g. "0658".</param>
        /// <param name="productId">Four hex digits without prefix, e.g. "0200".</param>
        internal static string[] GetPortNames(string vendorId, string productId)
        {
            return MatchPorts(Scan(SysfsUsbDevicesRoot, DeviceRoot), vendorId, productId);
        }

        /// <summary>Picks the ports of the USB devices with the vendor and product id.</summary>
        internal static string[] MatchPorts(IEnumerable<UsbSerialPortInfo> ports, string vendorId, string productId)
        {
            if (ports == null || string.IsNullOrEmpty(vendorId) || string.IsNullOrEmpty(productId))
                return new string[0];

            // sysfs reports the ids in lower case.
            return ports
                .Where(x => string.Equals(x.VendorId, vendorId, StringComparison.OrdinalIgnoreCase) &&
                            string.Equals(x.ProductId, productId, StringComparison.OrdinalIgnoreCase))
                .Select(x => x.PortName)
                .ToArray();
        }

        /// <summary>
        /// Scans a sysfs USB device tree. The roots are parameters so the scan can be unit tested
        /// against a fixture directory on any platform.
        /// </summary>
        internal static List<UsbSerialPortInfo> Scan(string usbDevicesRoot, string deviceRoot)
        {
            var ret = new List<UsbSerialPortInfo>();
            if (string.IsNullOrEmpty(usbDevicesRoot) || !Directory.Exists(usbDevicesRoot))
                return ret;

            foreach (var usbDevice in Directory.EnumerateDirectories(usbDevicesRoot).OrderBy(x => x, StringComparer.Ordinal))
            {
                try
                {
                    // Interfaces ("1-1:1.0") are listed here as well - they carry no idVendor.
                    var vendorId = ReadAttribute(usbDevice, "idVendor");
                    var productId = ReadAttribute(usbDevice, "idProduct");
                    if (vendorId == null || productId == null)
                        continue;

                    var serial = ReadAttribute(usbDevice, "serial");
                    foreach (var tty in FindTtyNames(usbDevice).ToArray())
                    {
                        ret.Add(new UsbSerialPortInfo
                        {
                            // Device nodes are always "/dev/<name>", independent of the host this runs on.
                            PortName = deviceRoot.TrimEnd('/') + "/" + tty,
                            VendorId = vendorId,
                            ProductId = productId,
                            Serial = serial
                        });
                    }
                }
                catch (Exception ex)
                {
                    "UsbSerialPortLocatorUnix error for {0}: {1}"._DLOG(usbDevice, ex.Message);
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
                // device brings its own ids and is listed at the top level anyway.
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
