/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Utils;
#if NETCOREAPP
using System.IO;
#endif

namespace UtilsTests
{
    /// <summary>
    /// Covers the J-Link (Gamma board) serial number lookup. The WMI and sysfs queries themselves
    /// need real hardware, so the platform back ends feed their raw results into the pure helpers
    /// exercised here.
    /// </summary>
    [TestFixture]
    public class JLinkBoardLocatorTests
    {
        // Win32_USBControllerDevice reports its Dependent as a WMI object path in which the
        // DeviceID separators are doubled.
        private const string CompositeParent = @"USB\\VID_1366&PID_0105\\000440244321";
        private const string CompositeInterface = @"USB\\VID_1366&PID_0105&MI_00\\6&2f1a3b4c&0&0000";
        private const string WinUsbParent = @"USB\\VID_1366&PID_1024\\000440244322";
        private const string WinUsbParentNeighbour = @"USB\\VID_1366&PID_1024\\000440244323";

        // Win32_PnPEntity reports DeviceID with single separators.
        private const string CompositePortDeviceId = @"USB\VID_1366&PID_0105&MI_00\6&2f1a3b4c&0&0000";
        private const string WinUsbPortDeviceId = @"USB\VID_1366&PID_1024\000440244322";
        private const string WinUsbNeighbourPortDeviceId = @"USB\VID_1366&PID_1024\000440244323";

        private static string Dependent(string deviceId)
        {
            return @"\\PC\root\cimv2:Win32_PnPEntity.DeviceID=" + "\"" + deviceId + "\"";
        }

        private static IEnumerable<KeyValuePair<string, string>> PnpEntity(string deviceId, string name)
        {
            return new[] { new KeyValuePair<string, string>(deviceId, name) };
        }

        #region Topology

        [Test]
        public void BuildTopology_ParentInstanceSegment_BecomesSerialWithoutLeadingZeros()
        {
            var topology = JLinkUsbMatcher.BuildTopology(new[] { Dependent(CompositeParent) });

            Assert.AreEqual(1, topology.Count);
            Assert.IsTrue(topology.ContainsKey("440244321"));
        }

        [Test]
        public void BuildTopology_CompositeAdapter_InterfaceAttachedToItsParent()
        {
            var topology = JLinkUsbMatcher.BuildTopology(new[]
            {
                Dependent(CompositeParent),
                Dependent(CompositeInterface)
            });

            Assert.AreEqual(1, topology.Count);
            CollectionAssert.Contains(topology["440244321"], @"&MI_00\6&2f1a3b4c&0&0000");
        }

        /// <summary>
        /// An adapter is reported once per USB controller. The interfaces that follow the repeated
        /// entry must attach to it and not to whichever adapter happened to be seen last.
        /// </summary>
        [Test]
        public void BuildTopology_ParentReportedTwice_InterfaceAttachesToMostRecentParent()
        {
            var topology = JLinkUsbMatcher.BuildTopology(new[]
            {
                Dependent(CompositeParent),
                Dependent(WinUsbParent),
                Dependent(CompositeParent),
                Dependent(CompositeInterface)
            });

            CollectionAssert.Contains(topology["440244321"], @"&MI_00\6&2f1a3b4c&0&0000");
            CollectionAssert.IsEmpty(topology["440244322"]);
        }

        [Test]
        public void BuildTopology_InterfaceBeforeAnyParent_IsIgnored()
        {
            var topology = JLinkUsbMatcher.BuildTopology(new[] { Dependent(CompositeInterface) });

            CollectionAssert.IsEmpty(topology);
        }

        [Test]
        public void BuildTopology_ForeignVendor_IsIgnored()
        {
            var topology = JLinkUsbMatcher.BuildTopology(new[] { Dependent(@"USB\\VID_0403&PID_6001\\A50285BI") });

            CollectionAssert.IsEmpty(topology);
        }

        #endregion

        #region Matching

        [Test]
        public void MatchPorts_CompositeAdapter_PortMappedToSerial()
        {
            var topology = JLinkUsbMatcher.BuildTopology(new[]
            {
                Dependent(CompositeParent),
                Dependent(CompositeInterface)
            });

            var result = JLinkUsbMatcher.MatchPorts(topology,
                PnpEntity(CompositePortDeviceId, "JLink CDC UART Port (COM7)"));

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("COM7", result[0].Item1);
            Assert.AreEqual("440244321", result[0].Item2);
        }

        /// <summary>
        /// EDE-423: with adapter FW 2.0+ and the general USB driver there are no MI_* interfaces,
        /// so the port has to be matched through the instance segment of the DeviceID.
        /// </summary>
        [Test]
        public void MatchPorts_WinUsbAdapterWithoutInterfaces_PortMappedToSerial()
        {
            var topology = JLinkUsbMatcher.BuildTopology(new[] { Dependent(WinUsbParent) });

            var result = JLinkUsbMatcher.MatchPorts(topology,
                PnpEntity(WinUsbPortDeviceId, "JLink CDC UART Port (COM8)"));

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("COM8", result[0].Item1);
            Assert.AreEqual("440244322", result[0].Item2);
        }

        [Test]
        public void MatchPorts_AdaptersWithConsecutiveSerials_AreNotConfused()
        {
            var topology = JLinkUsbMatcher.BuildTopology(new[]
            {
                Dependent(WinUsbParent),
                Dependent(WinUsbParentNeighbour)
            });

            var result = JLinkUsbMatcher.MatchPorts(topology, new[]
            {
                new KeyValuePair<string, string>(WinUsbPortDeviceId, "JLink CDC UART Port (COM8)"),
                new KeyValuePair<string, string>(WinUsbNeighbourPortDeviceId, "JLink CDC UART Port (COM9)")
            });

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("440244322", result.Single(x => x.Item1 == "COM8").Item2);
            Assert.AreEqual("440244323", result.Single(x => x.Item1 == "COM9").Item2);
        }

        [Test]
        public void MatchPorts_NonJLinkPort_IsIgnored()
        {
            var topology = JLinkUsbMatcher.BuildTopology(new[] { Dependent(WinUsbParent) });

            var result = JLinkUsbMatcher.MatchPorts(topology,
                PnpEntity(@"USB\VID_0403&PID_6001\A50285BI", "USB Serial Port (COM3)"));

            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void MatchPorts_PortWithoutComNameInLabel_IsIgnored()
        {
            var topology = JLinkUsbMatcher.BuildTopology(new[] { Dependent(WinUsbParent) });

            var result = JLinkUsbMatcher.MatchPorts(topology,
                PnpEntity(WinUsbPortDeviceId, "JLink CDC UART Port"));

            CollectionAssert.IsEmpty(result);
        }

        /// <summary>
        /// Adapters without a USB serial string get a Windows generated instance path. That is not a
        /// serial number, so it must not be handed to the caller through the instance id fallback.
        /// </summary>
        [Test]
        public void MatchPorts_AdapterWithoutUsbSerial_InstancePathIsNotReportedAsSerial()
        {
            var topology = JLinkUsbMatcher.BuildTopology(new[] { Dependent(@"USB\\VID_1366&PID_1024\\6&1a2b3c4d&0&2") });

            var result = JLinkUsbMatcher.MatchPorts(topology,
                PnpEntity(@"USB\VID_1366&PID_1024\6&1a2b3c4d&0&2", "JLink CDC UART Port (COM8)"));

            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void NormalizeSerial_StripsLeadingZeros()
        {
            Assert.AreEqual("440244321", JLinkUsbMatcher.NormalizeSerial("000440244321"));
            Assert.AreEqual("440244321", JLinkUsbMatcher.NormalizeSerial(" 000440244321 "));
            Assert.IsNull(JLinkUsbMatcher.NormalizeSerial("0000"));
            Assert.IsNull(JLinkUsbMatcher.NormalizeSerial(null));
        }

        #endregion

#if NETCOREAPP

        #region Linux sysfs

        /// <summary>
        /// Builds a sysfs stand-in. The tty "device" entry is a plain directory rather than a
        /// symlink so the fixture needs no elevated rights; the attributes sit on its parent, which
        /// is what the scan walks up to on a real system.
        /// </summary>
        private static string CreateSysfsFixture(string tty, string vendorId, string productId, string serial)
        {
            var root = Path.Combine(Path.GetTempPath(), "zwtc-sysfs-" + Path.GetRandomFileName());
            var ttyDirectory = Path.Combine(root, tty);
            Directory.CreateDirectory(Path.Combine(ttyDirectory, "device"));

            if (vendorId != null)
                File.WriteAllText(Path.Combine(ttyDirectory, "idVendor"), vendorId + "\n");
            if (productId != null)
                File.WriteAllText(Path.Combine(ttyDirectory, "idProduct"), productId + "\n");
            if (serial != null)
                File.WriteAllText(Path.Combine(ttyDirectory, "serial"), serial + "\n");

            return root;
        }

        [Test]
        public void Scan_JLinkWinUsbAdapter_ReturnsDeviceNodeAndSerial()
        {
            var root = CreateSysfsFixture("ttyACM0", "1366", "1024", "000440244322");
            try
            {
                var result = JLinkBoardLocatorUnix.Scan(root, "/dev");

                Assert.AreEqual(1, result.Count);
                Assert.AreEqual("/dev/ttyACM0", result[0].Item1);
                Assert.AreEqual("440244322", result[0].Item2);
            }
            finally
            {
                Directory.Delete(root, true);
            }
        }

        [Test]
        public void Scan_JLinkLegacyAdapter_ReturnsDeviceNodeAndSerial()
        {
            var root = CreateSysfsFixture("ttyACM1", "1366", "0105", "000440244321");
            try
            {
                var result = JLinkBoardLocatorUnix.Scan(root, "/dev");

                Assert.AreEqual(1, result.Count);
                Assert.AreEqual("/dev/ttyACM1", result[0].Item1);
                Assert.AreEqual("440244321", result[0].Item2);
            }
            finally
            {
                Directory.Delete(root, true);
            }
        }

        [Test]
        public void Scan_ForeignVendor_IsIgnored()
        {
            var root = CreateSysfsFixture("ttyUSB0", "0403", "6001", "A50285BI");
            try
            {
                CollectionAssert.IsEmpty(JLinkBoardLocatorUnix.Scan(root, "/dev"));
            }
            finally
            {
                Directory.Delete(root, true);
            }
        }

        [Test]
        public void Scan_JLinkWithoutSerialAttribute_IsIgnored()
        {
            var root = CreateSysfsFixture("ttyACM0", "1366", "1024", null);
            try
            {
                CollectionAssert.IsEmpty(JLinkBoardLocatorUnix.Scan(root, "/dev"));
            }
            finally
            {
                Directory.Delete(root, true);
            }
        }

        [Test]
        public void Scan_MissingSysfsRoot_ReturnsEmptyList()
        {
            var result = JLinkBoardLocatorUnix.Scan(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()), "/dev");

            CollectionAssert.IsEmpty(result);
        }

        #endregion

#endif
    }
}
