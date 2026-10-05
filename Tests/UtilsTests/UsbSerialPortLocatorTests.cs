// SPDX-License-Identifier: BSD-3-Clause
// SPDX-FileCopyrightText: Z-Wave Alliance <https://z-wavealliance.org>
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Utils;

namespace UtilsTests
{
    /// <summary>
    /// Covers the vendor/product id lookup of USB serial ports. The sysfs scan runs against a
    /// stand-in directory and the WMI matching against canned DeviceID/Name pairs, so the tests
    /// need no hardware and pass on every host.
    /// </summary>
    [TestFixture]
    public class UsbSerialPortLocatorTests
    {
        private string _root;

        [SetUp]
        public void SetUp()
        {
            _root = Path.Combine(Path.GetTempPath(), "zwtc-sysfs-" + Path.GetRandomFileName());
            Directory.CreateDirectory(_root);
        }

        [TearDown]
        public void TearDown()
        {
            Directory.Delete(_root, true);
        }

        /// <summary>
        /// Adds a USB device with one interface. The interface directory is really named
        /// "&lt;device&gt;:1.0", but ':' is not a valid NTFS character; the scan keys on the
        /// bInterfaceNumber attribute, not on the name.
        /// </summary>
        private void AddDevice(string device, string vendorId, string productId, string tty, bool usbSerialLayout = false)
        {
            var deviceDirectory = Path.Combine(_root, device);
            Directory.CreateDirectory(deviceDirectory);
            File.WriteAllText(Path.Combine(deviceDirectory, "idVendor"), vendorId + "\n");
            File.WriteAllText(Path.Combine(deviceDirectory, "idProduct"), productId + "\n");

            var interfaceDirectory = Path.Combine(deviceDirectory, device + "_1.0");
            Directory.CreateDirectory(interfaceDirectory);
            File.WriteAllText(Path.Combine(interfaceDirectory, "bInterfaceNumber"), "00\n");
            if (tty != null)
            {
                // cdc_acm exposes "<interface>/tty/ttyACM0", the usb-serial drivers "<interface>/ttyUSB0".
                Directory.CreateDirectory(usbSerialLayout
                    ? Path.Combine(interfaceDirectory, tty)
                    : Path.Combine(interfaceDirectory, "tty", tty));
            }
        }

        private string[] MatchUnixPorts(string vendorId, string productId)
        {
            return UsbSerialPortLocatorUnix.MatchPorts(UsbSerialPortLocatorUnix.Scan(_root, "/dev"), vendorId, productId);
        }

        private static KeyValuePair<string, string> PnpEntity(string deviceId, string name)
        {
            return new KeyValuePair<string, string>(deviceId, name);
        }

        #region Id normalization

        [Test]
        public void NormalizeId_WindowsStylePrefix_IsStripped()
        {
            Assert.AreEqual("0658", UsbSerialPortLocator.NormalizeId("VID_0658", "VID_"));
            Assert.AreEqual("0200", UsbSerialPortLocator.NormalizeId(" pid_0200 ", "PID_"));
            Assert.AreEqual("0658", UsbSerialPortLocator.NormalizeId("0658", "VID_"));
        }

        [Test]
        public void NormalizeId_MissingId_ReturnsNull()
        {
            Assert.IsNull(UsbSerialPortLocator.NormalizeId(null, "VID_"));
            Assert.IsNull(UsbSerialPortLocator.NormalizeId("", "VID_"));
            Assert.IsNull(UsbSerialPortLocator.NormalizeId("VID_", "VID_"));
        }

        [Test]
        public void GetPortNames_MissingId_ReturnsEmptyArray()
        {
            CollectionAssert.IsEmpty(UsbSerialPortLocator.GetPortNames(null, "PID_0200"));
            CollectionAssert.IsEmpty(UsbSerialPortLocator.GetPortNames("VID_0658", ""));
        }

        #endregion

        #region Linux sysfs

        [Test]
        public void Unix_MatchingDevice_ReturnsItsDeviceNode()
        {
            AddDevice("1-1", "0658", "0200", "ttyACM0");
            AddDevice("1-2", "0403", "6001", "ttyUSB0", usbSerialLayout: true);

            CollectionAssert.AreEqual(new[] { "/dev/ttyACM0" }, MatchUnixPorts("0658", "0200"));
        }

        /// <summary>sysfs reports the ids in lower case, Windows style ids are upper case.</summary>
        [Test]
        public void Unix_HexLetters_AreComparedCaseInsensitively()
        {
            AddDevice("1-1", "10c4", "ea60", "ttyUSB0", usbSerialLayout: true);

            CollectionAssert.AreEqual(new[] { "/dev/ttyUSB0" }, MatchUnixPorts("10C4", "EA60"));
        }

        [Test]
        public void Unix_SeveralMatchingDevices_ReturnsAllOfThem()
        {
            AddDevice("1-1", "0658", "0200", "ttyACM0");
            AddDevice("1-2", "0658", "0200", "ttyACM1");

            CollectionAssert.AreEqual(new[] { "/dev/ttyACM0", "/dev/ttyACM1" }, MatchUnixPorts("0658", "0200"));
        }

        [Test]
        public void Unix_SameVendorOtherProduct_IsIgnored()
        {
            AddDevice("1-1", "0658", "0280", "ttyACM0");

            CollectionAssert.IsEmpty(MatchUnixPorts("0658", "0200"));
        }

        [Test]
        public void Unix_MatchingDeviceWithoutTty_IsIgnored()
        {
            AddDevice("1-1", "0658", "0200", null);

            CollectionAssert.IsEmpty(MatchUnixPorts("0658", "0200"));
        }

        [Test]
        public void Unix_MissingSysfsRoot_ReturnsEmptyList()
        {
            var result = UsbSerialPortLocatorUnix.Scan(Path.Combine(_root, "missing"), "/dev");

            CollectionAssert.IsEmpty(result);
        }

        #endregion

        #region Windows WMI

        [Test]
        public void Windows_MatchingDevice_ReturnsComPortFromName()
        {
            var result = UsbSerialPortLocatorWindows.MatchPorts(new[]
            {
                PnpEntity(@"USB\VID_0658&PID_0200\6&1A2B3C4D&0&2", "UZB (COM5)"),
                PnpEntity(@"USB\VID_1366&PID_1024\000440244322", "JLink CDC UART Port (COM8)")
            }, "0658", "0200");

            CollectionAssert.AreEqual(new[] { "COM5" }, result);
        }

        /// <summary>A port enumerated on an interface of a composite device carries an MI_* suffix.</summary>
        [Test]
        public void Windows_CompositeInterface_IsMatched()
        {
            var result = UsbSerialPortLocatorWindows.MatchPorts(new[]
            {
                PnpEntity(@"USB\VID_0658&PID_0200&MI_00\7&2F1A3B4C&0&0000", "USB Serial Device (COM12)")
            }, "0658", "0200");

            CollectionAssert.AreEqual(new[] { "COM12" }, result);
        }

        /// <summary>FTDI devices separate the ids with '+' instead of '&amp;'.</summary>
        [Test]
        public void Windows_FtdiDeviceId_IsMatched()
        {
            var result = UsbSerialPortLocatorWindows.MatchPorts(new[]
            {
                PnpEntity(@"FTDIBUS\VID_0403+PID_6001+A50285BIA\0000", "USB Serial Port (COM3)")
            }, "0403", "6001");

            CollectionAssert.AreEqual(new[] { "COM3" }, result);
        }

        [Test]
        public void Windows_HexLetters_AreComparedCaseInsensitively()
        {
            var result = UsbSerialPortLocatorWindows.MatchPorts(new[]
            {
                PnpEntity(@"USB\VID_10C4&PID_EA60\0001", "Silicon Labs CP210x USB to UART Bridge (COM4)")
            }, "10c4", "ea60");

            CollectionAssert.AreEqual(new[] { "COM4" }, result);
        }

        [Test]
        public void Windows_SameVendorOtherProduct_IsIgnored()
        {
            var result = UsbSerialPortLocatorWindows.MatchPorts(new[]
            {
                PnpEntity(@"USB\VID_0658&PID_0280\0001", "ZCOM (COM6)")
            }, "0658", "0200");

            CollectionAssert.IsEmpty(result);
        }

        /// <summary>The product id must follow the vendor id, not merely appear somewhere.</summary>
        [Test]
        public void Windows_ProductIdBeforeVendorId_IsIgnored()
        {
            var result = UsbSerialPortLocatorWindows.MatchPorts(new[]
            {
                PnpEntity(@"ROOT\PID_0200&VID_0658\0001", "Other (COM7)")
            }, "0658", "0200");

            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void Windows_NameWithoutComPort_IsIgnored()
        {
            var result = UsbSerialPortLocatorWindows.MatchPorts(new[]
            {
                PnpEntity(@"USB\VID_0658&PID_0200\0001", "UZB"),
                PnpEntity(@"USB\VID_0658&PID_0200\0002", "Printer Port (LPT1)"),
                PnpEntity(@"USB\VID_0658&PID_0200\0003", null),
                PnpEntity(null, "UZB (COM5)")
            }, "0658", "0200");

            CollectionAssert.IsEmpty(result);
        }

        #endregion
    }
}
