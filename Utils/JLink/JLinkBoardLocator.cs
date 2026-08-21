/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System;
using System.Collections.Generic;

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
#if NETCOREAPP
                return OperatingSystem.IsWindows()
                    ? JLinkBoardLocatorWindows.GetBoardLinks()
                    : JLinkBoardLocatorUnix.GetBoardLinks();
#else
                return JLinkBoardLocatorWindows.GetBoardLinks();
#endif
            }
            catch (Exception ex)
            {
                "JLinkBoardLocator.GetBoardLinks error: {0}"._DLOG(ex.Message);
                return new List<Tuple<string, string>>();
            }
        }
    }
}
