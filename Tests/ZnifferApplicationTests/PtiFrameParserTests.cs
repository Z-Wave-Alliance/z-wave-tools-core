/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance <https://z-wavealliance.org>
using System;
using NUnit.Framework;
using ZWave.Enums;
using ZWave.ZnifferApplication;

namespace ZnifferApplicationTests
{
    [TestFixture]
    public class PtiFrameParserTests
    {
        private const byte DchVersion2 = 0x02;
        private const byte HwRxStart = 0xF8;
        private const byte HwRxSuccess = 0xF9;
        private const byte ZWaveProtocol = 0x06;
        private const int DchHeaderLength = 12;
        private const int AppendixLength = 6;

        private const byte RegionEu = 0x01;
        private const byte RegionUsLr3 = 0x0E;
        private const byte RegionEuLr1 = 0x0F;
        private const byte RegionEuLr2 = 0x10;
        private const byte RegionEuLr3 = 0x11;

        private const byte Speed9600 = 0;
        private const byte Speed40K = 1;
        private const byte Speed100K = 2;
        private const byte SpeedLr = 3;

        /// Builds one DCH version 2 PTI frame received on <paramref name="channel"/>
        /// of <paramref name="region"/>, carrying a 10 byte Z-Wave payload.
        private static byte[] BuildPtiFrame(
            byte region,
            byte channel,
            byte rssi = 0x40,
            byte appendedInfoCfg = 0x00)
        {
            var data = new byte[DchHeaderLength + 10 + AppendixLength];
            data[0] = DchVersion2;
            data[DchHeaderLength - 1] = HwRxStart;
            for (int i = 0; i < 10; i++)
            {
                // Stay clear of 0x55, which marks a beam and parses differently
                data[DchHeaderLength + i] = (byte)(i + 1);
            }
            data[data.Length - 6] = HwRxSuccess;
            data[data.Length - 5] = rssi;
            data[data.Length - 4] = region;
            data[data.Length - 3] = channel;
            data[data.Length - 2] = ZWaveProtocol;
            data[data.Length - 1] = appendedInfoCfg;
            return data;
        }

        private static DataItem Parse(
            byte region,
            byte channel,
            byte rssi = 0x40,
            byte appendedInfoCfg = 0x00)
        {
            return PtiFrameParser.GetDataItem(
                ApiTypes.Pti,
                DateTime.Now,
                null,
                1,
                BuildPtiFrame(region, channel, rssi, appendedInfoCfg));
        }

        [TestCase(RegionEuLr2, 3)]
        [TestCase(RegionEuLr3, 0)]
        [TestCase(RegionEuLr3, 1)]
        public void GetDataItem_RegionIdAboveOneNibble_IsParsed(byte region, byte channel)
        {
            var dataItem = Parse(region, channel);

            Assert.AreEqual(region, dataItem.Frequency);
            Assert.AreEqual(SpeedLr, dataItem.Speed);
        }

        [TestCase(RegionEu, 0, Speed100K)]
        [TestCase(RegionEu, 1, Speed40K)]
        [TestCase(RegionEu, 2, Speed9600)]
        [TestCase(RegionUsLr3, 1, SpeedLr)]
        [TestCase(RegionEuLr1, 3, SpeedLr)]
        public void GetDataItem_RegionIdWithinOneNibble_IsParsed(byte region, byte channel, byte speed)
        {
            var dataItem = Parse(region, channel);

            Assert.AreEqual(region, dataItem.Frequency);
            Assert.AreEqual(speed, dataItem.Speed);
        }

        [Test]
        public void GetDataItem_AppendedInfoVersion0_KeepsRawRssi()
        {
            const byte rawRssi = 0x40;
            var dataItem = Parse(RegionEu, 0, rawRssi, appendedInfoCfg: 0x00);

            Assert.AreEqual(rawRssi, dataItem.Rssi);
            Assert.AreEqual((sbyte)rawRssi, (sbyte)dataItem.Rssi);
        }

        [TestCase(0x01)]
        [TestCase(0x07)]
        public void GetDataItem_AppendedInfoVersionAtLeast1_SubtractsRssiCompensation(byte appendedInfoCfg)
        {
            const byte rawRssi = 0x40; // +64 as signed
            const byte expectedRssi = unchecked((byte)(64 - 0x32)); // +14 dBm

            var dataItem = Parse(RegionEu, 0, rawRssi, appendedInfoCfg);

            Assert.AreEqual(expectedRssi, dataItem.Rssi);
            Assert.AreEqual(14, (sbyte)dataItem.Rssi);
        }

        [Test]
        public void GetDataItem_AppendedInfoVersionAtLeast1_PreservesNegativeRssiAsSignedByte()
        {
            // Raw -20 dBm (0xEC), compensated: -20 - 50 = -70 dBm (0xBA)
            const byte rawRssi = 0xEC;
            const byte expectedRssi = unchecked((byte)(-20 - 0x32));

            var dataItem = Parse(RegionEu, 0, rawRssi, appendedInfoCfg: 0x01);

            Assert.AreEqual(expectedRssi, dataItem.Rssi);
            Assert.AreEqual(-70, (sbyte)dataItem.Rssi);
        }
    }
}
