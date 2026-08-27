// SPDX-License-Identifier: BSD-3-Clause
// SPDX-FileCopyrightText: Z-Wave Alliance <https://z-wavealliance.org>
using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    /// <summary>
    /// Pure matching logic shared by the platform specific J-Link board locators.
    /// Kept free of WMI and file system access so that it can be unit tested.
    /// </summary>
    internal static class JLinkUsbMatcher
    {
        /// <summary>USB VID/PID pairs a SEGGER J-Link adapter can enumerate as.</summary>
        internal static readonly string[] VidPids = new[]
        {
            @"VID_1366&PID_0105",   // SEGGER legacy driver "JLink CDC UART Port"
            @"VID_1366&PID_1024"    // WinUSB / general USB driver (adapter FW 2.0+)
        };

        /// <summary>
        /// Turns the USB serial string into the canonical J-Link serial number.
        /// The USB descriptor zero pads the number (e.g. "000440244321" for SN 440244321).
        /// </summary>
        internal static string NormalizeSerial(string usbSerial)
        {
            if (string.IsNullOrEmpty(usbSerial))
                return null;

            var trimmed = usbSerial.Trim().TrimStart('0');
            return trimmed.Length == 0 ? null : trimmed;
        }

        /// <summary>
        /// True when the value can be a J-Link serial number rather than a Windows generated
        /// instance path such as "6&amp;2f1a3b4c&amp;0&amp;2" (adapters without a USB serial string).
        /// </summary>
        internal static bool LooksLikeSerialNumber(string candidate)
        {
            return !string.IsNullOrEmpty(candidate) && candidate.All(char.IsDigit);
        }

        /// <summary>
        /// Builds a "serial number -&gt; child interface instance ids" map from the Dependent
        /// strings of Win32_USBControllerDevice.
        /// </summary>
        internal static Dictionary<string, List<string>> BuildTopology(IEnumerable<string> dependents)
        {
            var topology = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
            if (dependents == null)
                return topology;

            string lastId = null;
            foreach (var dependent in dependents)
            {
                if (string.IsNullOrEmpty(dependent))
                    continue;

                foreach (var vidPid in VidPids)
                {
                    int inx = dependent.IndexOf(vidPid, StringComparison.OrdinalIgnoreCase);
                    if (inx < 0)
                        continue;

                    string id = dependent.Substring(inx + vidPid.Length).Replace(@"\\", @"\").Trim('\\', '"');
                    if (id.Length > 0)
                    {
                        if (id.StartsWith("&", StringComparison.Ordinal))
                        {
                            // An interface of the composite parent seen before, e.g. "&MI_00\6&2f1a3b4c&0&0000".
                            if (lastId != null && topology.ContainsKey(lastId))
                                topology[lastId].Add(id);
                        }
                        else
                        {
                            // A parent device - its instance segment carries the serial number.
                            id = id.TrimStart('0');
                            if (id.Length > 0)
                            {
                                if (!topology.ContainsKey(id))
                                    topology.Add(id, new List<string>());

                                // Always track the most recent parent, so its interfaces attach here even
                                // when the same adapter is reported twice (once per USB controller).
                                lastId = id;
                            }
                        }
                    }

                    // A Dependent can only ever carry one of the VID/PID pairs.
                    break;
                }
            }

            return topology;
        }

        /// <summary>
        /// Matches serial ports against the USB topology.
        /// </summary>
        /// <param name="topology">Result of <see cref="BuildTopology"/>.</param>
        /// <param name="serialPortEntities">Win32_PnPEntity DeviceID -&gt; Name pairs of the Ports class.</param>
        internal static List<Tuple<string, string>> MatchPorts(
            Dictionary<string, List<string>> topology,
            IEnumerable<KeyValuePair<string, string>> serialPortEntities)
        {
            var ret = new List<Tuple<string, string>>();
            if (topology == null || topology.Count == 0 || serialPortEntities == null)
                return ret;

            foreach (var entity in serialPortEntities)
            {
                var deviceId = entity.Key;
                var name = entity.Value;
                if (string.IsNullOrEmpty(deviceId) || string.IsNullOrEmpty(name))
                    continue;

                if (!VidPids.Any(x => deviceId.IndexOf(x, StringComparison.OrdinalIgnoreCase) >= 0))
                    continue;

                var port = name.Split('(', ')').FirstOrDefault(x => x.StartsWith("COM", StringComparison.Ordinal));
                if (port == null)
                    continue;

                // The instance id is the last '\'-separated segment of the DeviceID: "000440244321"
                // when the port is enumerated on the adapter itself, "6&2f1a3b4c&0&0000" when it is
                // enumerated on one of the adapter's MI_* interfaces.
                int lastSep = deviceId.LastIndexOf('\\');
                var instanceId = lastSep >= 0 ? deviceId.Substring(lastSep + 1) : deviceId;
                var normalizedInstanceId = instanceId.TrimStart('0');

                foreach (var record in topology)
                {
                    bool matched = record.Value.Any(child => deviceId.IndexOf(child, StringComparison.OrdinalIgnoreCase) >= 0);

                    // A port enumerated on the adapter itself has no MI_* interface to match, so compare
                    // the instance segment itself. It is compared in full - a prefix match would confuse
                    // adapters whose serial numbers only differ in the last digits.
                    if (!matched
                        && LooksLikeSerialNumber(record.Key)
                        && string.Equals(normalizedInstanceId, record.Key, StringComparison.OrdinalIgnoreCase))
                    {
                        matched = true;
                    }

                    if (matched)
                    {
                        ret.Add(new Tuple<string, string>(port, record.Key));
                        break;
                    }
                }
            }

            return ret;
        }
    }
}
