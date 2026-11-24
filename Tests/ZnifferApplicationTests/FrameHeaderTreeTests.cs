// SPDX-License-Identifier: BSD-3-Clause
// SPDX-FileCopyrightText: Z-Wave Alliance <https://z-wavealliance.org>
using System.IO;
using System.Linq;
using NUnit.Framework;
using Utils;
using ZWave.Xml.FrameHeader;

namespace ZnifferApplicationTests
{
    [TestFixture]
    public class FrameHeaderTreeTests
    {
        private const string SinglecastLongRangeHeaderName = "SINGLECASTLR";
        private const string WakeUpBeamLongRangeHeaderName = "WAKE_UP_BEAM_LR";

        private FrameDefinition _frameDefinition;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _frameDefinition = FrameDefinition.Load(FindFrameDefinitionFile());
            Assert.IsNotNull(_frameDefinition);
        }

        // Long Range Node Information Frame from node 1 to node 258 (Properties1 00 11 02), Length 0x27, CRC 32 0C.
        [TestCase("E9 4B EB E1 00 11 02 27 81 D5 B5 FA 04 01 80 02 02 02 01 00 5E 22 85 59 70 56 5A 7A 74 72 73 6C 8A 55 86 98 9F 32 0C",
            "1", "258")]
        // Long Range No Operation (0x04 0x00) from node 1 to node 256 (Properties1 00 11 00), Length 0x10, CRC 2C AF.
        [TestCase("E9 4B EB E1 00 11 00 10 81 01 B5 FA 04 00 2C AF", "1", "256")]
        public void ProcessParameters_LongRangeSinglecast_SourceAndDestinationFromLowBitsFirst(string frame, string source, string destination)
        {
            var properties1 = GetProperties1(SinglecastLongRangeHeaderName, Tools.GetBytes(frame.Replace(" ", "")));

            Assert.AreEqual(source, GetInnerValue(properties1, "SourceNodeID").TextValueList.Single());
            Assert.AreEqual(destination, GetInnerValue(properties1, "DestinationNodeID").TextValueList.Single());
        }

        // Wake Up Beam to node 258 with TX Power index 3: Marker 0x55, then TX Power in the high nibble, then the Node ID.
        [Test]
        public void ProcessParameters_LongRangeWakeUpBeam_DestinationFromLowTwelveBits()
        {
            var properties1 = GetProperties1(WakeUpBeamLongRangeHeaderName, new byte[] { 0x55, 0x31, 0x02, 0x5A });

            var destination = GetInnerValue(properties1, "DestinationNodeID");
            var txPower = GetInnerValue(properties1, "TxPower");
            Assert.AreEqual(new byte[] { 0x01, 0x02 }, destination.ByteValueList.ToArray());
            Assert.AreEqual("258", destination.TextValueList.Single());
            Assert.AreEqual(new byte[] { 0x03 }, txPower.ByteValueList.ToArray());
        }

        [TestCase("WAKE_UP_BEAM")]
        [TestCase("WAKE_UP_BEAM_START")]
        [TestCase(WakeUpBeamLongRangeHeaderName)]
        public void Load_HeaderWithoutBaseHeaderKey_HasNoBaseHeader(string headerName)
        {
            var header = _frameDefinition.Header.Single(x => x.Name == headerName);

            Assert.IsFalse(header.BaseHeaderKeySpecified);
            Assert.IsNull(header.BaseHeaderDefinition);
        }

        private ParamValue GetProperties1(string headerName, byte[] data)
        {
            var header = _frameDefinition.Header.Single(x => x.Name == headerName);
            var headerValue = HeaderValue.CreateHeaderValue(data, header);
            headerValue.ProcessParameters();
            _frameDefinition.ProcessParametersTextValue(headerValue.ParamValues);
            return headerValue.ParamValues.Single(x => x.ParamDefinition.Name == "Properties1");
        }

        private static ParamValue GetInnerValue(ParamValue paramValue, string name)
        {
            return paramValue.InnerValues.Single(x => x.ParamDefinition.Name == name);
        }

        private static string FindFrameDefinitionFile()
        {
            var directory = new DirectoryInfo(TestContext.CurrentContext.TestDirectory);
            while (directory != null)
            {
                var path = Path.Combine(directory.FullName, "XmlFiles", "FrameDefinition.xml");
                if (File.Exists(path))
                {
                    return path;
                }

                directory = directory.Parent;
            }

            Assert.Fail("XmlFiles/FrameDefinition.xml not found above the test directory.");
            return null;
        }
    }
}
