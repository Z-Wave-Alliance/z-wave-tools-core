// SPDX-License-Identifier: BSD-3-Clause
// SPDX-FileCopyrightText: Z-Wave Alliance <https://z-wavealliance.org>
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Utils
{
    /// <summary>
    /// Maps the serial port of every connected SEGGER J-Link adapter (Gamma board) to its
    /// J-Link serial number.
    ///
    /// Windows reads the USB topology from WMI, Linux/macOS read it from sysfs. Both back ends
    /// report the canonical J-Link serial number, i.e. the USB serial string without its leading
    /// zeros ("000440244321" is reported as "440244321"), and the port name in the form the rest
    /// of the library uses for the platform ("COM7" on Windows, "/dev/ttyACM0" on Linux).
    ///
    /// Everything here compiles for every target framework, so the same code and the same tests
    /// apply to .NET Framework and .NET (Core) alike.
    /// </summary>
    public static class JLinkBoardLocator
    {
        /// <summary>
        /// Returns (port name, J-Link serial number) for every connected adapter.
        /// Never throws - an empty list means nothing was detected.
        /// </summary>
        public static List<Tuple<string, string>> GetBoardLinks()
        {
            try
            {
                // RuntimeInformation.IsOSPlatform is available on every target framework - unlike
                // OperatingSystem.IsWindows() - and is understood by the platform compatibility
                // analyzer just the same.
                return RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                    ? JLinkBoardLocatorWindows.GetBoardLinks()
                    : JLinkBoardLocatorUnix.GetBoardLinks();
            }
            catch (Exception ex)
            {
                "JLinkBoardLocator.GetBoardLinks error: {0}"._DLOG(ex.Message);
                return new List<Tuple<string, string>>();
            }
        }
    }
}
