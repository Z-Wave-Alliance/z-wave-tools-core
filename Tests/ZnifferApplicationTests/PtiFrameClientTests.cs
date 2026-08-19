/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance <https://z-wavealliance.org>
using System.Collections.Generic;
using NUnit.Framework;
using ZWave.Enums;
using ZWave.Layers;
using ZWave.ZnifferApplication;

namespace ZnifferApplicationTests
{
    [TestFixture]
    public class PtiFrameClientTests
    {
        private const byte DchVersion2 = 0x02;
        private const byte HwRxStart = 0xF8;
        private const byte HwRxSuccess = 0xF9;
        private const byte ZWaveProtocol = 0x06;
        private const int DchHeaderLength = 12;
        private const int AppendixLength = 6;

        /// <summary>
        /// Builds one DCH version 2 PTI frame carrying a Z-Wave payload of the given
        /// length, wrapped in the debug channel framing:
        /// "[", 16 bit little endian length, data, "]".
        /// </summary>
        private static byte[] BuildFramedPtiFrame(int payloadLength)
        {
            var data = new byte[DchHeaderLength + payloadLength + AppendixLength];
            data[0] = DchVersion2;
            data[DchHeaderLength - 1] = HwRxStart;
            for (int i = 0; i < payloadLength; i++)
            {
                // Non-zero cycling fill (1..254)
                data[DchHeaderLength + i] = (byte)((i % 0xFE) + 1);
            }
            data[data.Length - 6] = HwRxSuccess;
            data[data.Length - 5] = 0x40; // RSSI
            data[data.Length - 4] = 0x02; // Region ID
            data[data.Length - 3] = 0x01; // Channel
            data[data.Length - 2] = ZWaveProtocol;

            var frameLength = data.Length + 2;
            var framed = new List<byte> { 0x5B, (byte)(frameLength & 0xFF), (byte)(frameLength >> 8) };
            framed.AddRange(data);
            framed.Add(0x5D);
            return framed.ToArray();
        }

        /// <summary>
        /// Feeds one buffer holding a frame per given payload length and returns the
        /// length of each frame the client handed back.
        /// </summary>
        private static List<int> ParseFrameLengths(params int[] payloadLengths)
        {
            var buffer = new List<byte>();
            foreach (var payloadLength in payloadLengths)
            {
                buffer.AddRange(BuildFramedPtiFrame(payloadLength));
            }

            var received = new List<int>();
            var frameClient = new SnifferPtiFrameLayer(null).CreateClient(1);
            frameClient.ReceiveFrameCallback = x => received.Add(x.Buffer.Length);
            frameClient.HandleData(new DataChunk(buffer.ToArray(), 0, false, ApiTypes.Pti), true);
            return received;
        }

        private static int FrameLength(int payloadLength)
        {
            return DchHeaderLength + payloadLength + AppendixLength;
        }

        [Test]
        public void HandleData_ShortFrame_IsParsed()
        {
            Assert.AreEqual(new[] { FrameLength(20) }, ParseFrameLengths(20));
        }

        [Test]
        public void HandleData_FrameLongerThan255Bytes_IsParsed()
        {
            Assert.AreEqual(new[] { FrameLength(300) }, ParseFrameLengths(300));
        }

        [Test]
        public void HandleData_MultipleFrames_AllAreParsed()
        {
            Assert.AreEqual(
                new[] { FrameLength(20), FrameLength(300), FrameLength(20) },
                ParseFrameLengths(20, 300, 20));
        }
    }
}
