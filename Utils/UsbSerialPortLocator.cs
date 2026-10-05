// SPDX-License-Identifier: BSD-3-Clause
// SPDX-FileCopyrightText: Z-Wave Alliance <https://z-wavealliance.org>
using System;
using System.Runtime.InteropServices;

namespace Utils
{
    /// <summary>
    /// Finds the serial ports of USB devices with a given vendor and product id.
    ///
    /// Windows reads the devices from WMI, Linux/macOS read them from sysfs. Both back ends
    /// report the port name in the form the rest of the library uses for the platform ("COM7" on
    /// Windows, "/dev/ttyACM0" on Linux).
    ///
    /// Everything here compiles for every target framework, so the same code and the same tests
    /// apply to .NET Framework and .NET (Core) alike.
    /// </summary>
    public static class UsbSerialPortLocator
    {
        /// <summary>
        /// Returns the serial ports exposed by USB devices with the given vendor and product id.
        /// The ids are four hex digits, with or without the "VID_"/"PID_" prefix
        /// ("VID_0658"/"PID_0200" or "0658"/"0200"). Never throws - an empty array means nothing
        /// was detected.
        /// </summary>
        public static string[] GetPortNames(string vid, string pid)
        {
            var vendorId = NormalizeId(vid, "VID_");
            var productId = NormalizeId(pid, "PID_");
            if (vendorId == null || productId == null)
                return new string[0];

            try
            {
                // RuntimeInformation.IsOSPlatform is available on every target framework - unlike
                // OperatingSystem.IsWindows() - and is understood by the platform compatibility
                // analyzer just the same.
                return RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                    ? UsbSerialPortLocatorWindows.GetPortNames(vendorId, productId)
                    : UsbSerialPortLocatorUnix.GetPortNames(vendorId, productId);
            }
            catch (Exception ex)
            {
                "UsbSerialPortLocator.GetPortNames error: {0}"._DLOG(ex.Message);
                return new string[0];
            }
        }

        /// <summary>Strips the Windows style prefix from a vendor or product id ("VID_0658" -> "0658").</summary>
        internal static string NormalizeId(string id, string prefix)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            var ret = id.Trim();
            if (ret.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                ret = ret.Substring(prefix.Length);
            return ret.Length == 0 ? null : ret;
        }
    }
}
