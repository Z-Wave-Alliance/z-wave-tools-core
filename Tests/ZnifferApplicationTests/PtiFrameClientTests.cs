/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance <https://z-wavealliance.org>
using System;
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
        private const byte DchVersion3 = 0x03;
        private const byte HwRxStart = 0xF8;
        private const byte HwRxSuccess = 0xF9;
        private const byte ZWaveProtocol = 0x06;
        private const int DchHeaderLength = 12;
        private const int DchHeaderLengthVer3 = 19;
        private const int AppendixLength = 6;

        private static int HeaderLength(byte dchVersion)
        {
            return dchVersion == DchVersion3 ? DchHeaderLengthVer3 : DchHeaderLength;
        }

        /// <summary>
        /// Builds one PTI frame carrying a Z-Wave payload of the given length, wrapped
        /// in the debug channel framing:
        /// "[", 16 bit little endian length, data, "]".
        /// </summary>
        private static byte[] BuildFramedPtiFrame(int payloadLength, byte dchVersion = DchVersion2)
        {
            var headerLength = HeaderLength(dchVersion);
            var data = new byte[headerLength + payloadLength + AppendixLength];
            data[0] = dchVersion;
            data[headerLength - 1] = HwRxStart;
            for (int i = 0; i < payloadLength; i++)
            {
                // Non-zero cycling fill (1..254)
                data[headerLength + i] = (byte)((i % 0xFE) + 1);
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

        private static int FrameLength(int payloadLength, byte dchVersion = DchVersion2)
        {
            return HeaderLength(dchVersion) + payloadLength + AppendixLength;
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

        [Test]
        public void HandleData_CorruptFrameFollowedByValidFrame_Resyncs()
        {
            // The corrupt frame is valid except for its closing bracket
            var corrupt = BuildFramedPtiFrame(20);
            corrupt[corrupt.Length - 1] = 0x00; // should be 0x5D

            var buffer = new List<byte>(corrupt);
            buffer.AddRange(BuildFramedPtiFrame(30));

            var received = new List<int>();
            var frameClient = new SnifferPtiFrameLayer(null).CreateClient(1);
            frameClient.ReceiveFrameCallback = x => received.Add(x.Buffer.Length);
            frameClient.HandleData(new DataChunk(buffer.ToArray(), 0, false, ApiTypes.Pti), true);
            Assert.AreEqual(new[] { FrameLength(30) }, received);
        }

        [Test]
        public void HandleData_ZeroLengthFrame_ProducesNoFrameAndDoesNotThrow()
        {
            // frameLength 2 produces an empty data block, causing an IndexOutOfRange downstream
            var buffer = new byte[] { 0x5B, 0x02, 0x00, 0x5D, 0x11, 0x22 };

            var received = new List<int>();
            var frameClient = new SnifferPtiFrameLayer(null).CreateClient(1);
            frameClient.ReceiveFrameCallback = x => received.Add(x.Buffer.Length);
            Assert.DoesNotThrow(() =>
                frameClient.HandleData(new DataChunk(buffer, 0, false, ApiTypes.Pti), true));
            Assert.IsEmpty(received);
        }

        [Test]
        public void HandleData_StaleByteBeforeFrameStart_IsSkipped()
        {
            // Mid-stream attach: one leftover byte sits directly before a real "["
            var buffer = new List<byte> { 0xAA };
            buffer.AddRange(BuildFramedPtiFrame(20));

            var received = new List<int>();
            var frameClient = new SnifferPtiFrameLayer(null).CreateClient(1);
            frameClient.ReceiveFrameCallback = x => received.Add(x.Buffer.Length);
            frameClient.HandleData(new DataChunk(buffer.ToArray(), 0, false, ApiTypes.Pti), true);
            Assert.AreEqual(new[] { FrameLength(20) }, received);
        }

        [Test]
        public void HandleData_GarbageBuffer_ProducesNoFrames()
        {
            var buffer = new byte[] { 0xFF, 0xFE, 0x01, 0x02, 0x03, 0x5B, 0x00, 0x00, 0xAA, 0xBB };

            var received = new List<int>();
            var frameClient = new SnifferPtiFrameLayer(null).CreateClient(1);
            frameClient.ReceiveFrameCallback = x => received.Add(x.Buffer.Length);
            frameClient.HandleData(new DataChunk(buffer, 0, false, ApiTypes.Pti), true);
            Assert.IsEmpty(received);
        }

        [Test]
        public void HandleData_PayloadByteLooksLikeFrameStart_DoesNotStall()
        {
            // False 0x5B with an invalid DCH version byte
            var buffer = new List<byte> { 0x5B, 0x22, 0x22, 0x99 };
            buffer.AddRange(BuildFramedPtiFrame(20));

            var received = new List<int>();
            var frameClient = new SnifferPtiFrameLayer(null).CreateClient(1);
            frameClient.ReceiveFrameCallback = x => received.Add(x.Buffer.Length);
            frameClient.HandleData(new DataChunk(buffer.ToArray(), 0, false, ApiTypes.Pti), true);
            Assert.AreEqual(new[] { FrameLength(20) }, received);
        }

        [Test]
        public void HandleData_GarbageThenValidFrameInNextChunk_IsParsed()
        {
            var frameClient = new SnifferPtiFrameLayer(null).CreateClient(1);
            var received = new List<int>();
            frameClient.ReceiveFrameCallback = x => received.Add(x.Buffer.Length);

            // No 0x5B in this chunk, so nothing must carry over
            frameClient.HandleData(
                new DataChunk(new byte[] { 0xFF, 0xFE, 0xAA, 0xBB }, 0, false, ApiTypes.Pti), true);
            frameClient.HandleData(
                new DataChunk(BuildFramedPtiFrame(20), 0, false, ApiTypes.Pti), true);

            Assert.AreEqual(new[] { FrameLength(20) }, received);
        }

        [Test]
        public void HandleData_ResetParser_DropsPartialFrame()
        {
            var frameClient = new SnifferPtiFrameLayer(null).CreateClient(1);
            var received = new List<int>();
            frameClient.ReceiveFrameCallback = x => received.Add(x.Buffer.Length);

            // Reset between head and tail of a split frame
            var framed = BuildFramedPtiFrame(20);
            var head = new byte[10];
            var tail = new byte[framed.Length - head.Length];
            Array.Copy(framed, head, head.Length);
            Array.Copy(framed, head.Length, tail, 0, tail.Length);

            frameClient.HandleData(new DataChunk(head, 0, false, ApiTypes.Pti), true);
            frameClient.ResetParser();
            frameClient.HandleData(new DataChunk(tail, 0, false, ApiTypes.Pti), true);

            Assert.IsEmpty(received);
        }

        [Test]
        public void HandleData_DchVersion3Frame_IsParsed()
        {
            var frameClient = new SnifferPtiFrameLayer(null).CreateClient(1);
            var received = new List<int>();
            frameClient.ReceiveFrameCallback = x => received.Add(x.Buffer.Length);
            frameClient.HandleData(
                new DataChunk(BuildFramedPtiFrame(20, DchVersion3), 0, false, ApiTypes.Pti), true);

            Assert.AreEqual(new[] { FrameLength(20, DchVersion3) }, received);
        }

        [Test]
        public void HandleData_FrameAtMaximumLength_IsParsed()
        {
            // Largest frame a 16-bit length can describe
            const int maxPayload = ushort.MaxValue - DchHeaderLength - AppendixLength - 2;
            Assert.AreEqual(
                new[] { FrameLength(maxPayload) },
                ParseFrameLengthsInSegments(1024, maxPayload));
        }

        [Test]
        public void HandleData_ChunkEndsAfterImplausibleVersion_DoesNotBuffer()
        {
            var frameClient = new SnifferPtiFrameLayer(null).CreateClient(1);
            var received = new List<int>();
            frameClient.ReceiveFrameCallback = x => received.Add(x.Buffer.Length);

            // False start with 0xFFFF length and an invalid version byte
            frameClient.HandleData(
                new DataChunk(new byte[] { 0x5B, 0xFF, 0xFF, 0x99 }, 0, false, ApiTypes.Pti), true);
            frameClient.HandleData(
                new DataChunk(BuildFramedPtiFrame(20), 0, false, ApiTypes.Pti), true);

            Assert.AreEqual(new[] { FrameLength(20) }, received);
        }

        /// <summary>
        /// Feeds one buffer holding a frame per given payload length in segments of
        /// the given size, and returns the length of each frame the client handed back.
        /// </summary>
        private static List<int> ParseFrameLengthsInSegments(int segmentSize, params int[] payloadLengths)
        {
            return ParseFrameLengthsInSegments(segmentSize, DchVersion2, payloadLengths);
        }

        private static List<int> ParseFrameLengthsInSegments(int segmentSize, byte dchVersion, params int[] payloadLengths)
        {
            var buffer = new List<byte>();
            foreach (var payloadLength in payloadLengths)
            {
                buffer.AddRange(BuildFramedPtiFrame(payloadLength, dchVersion));
            }

            var received = new List<int>();
            var frameClient = new SnifferPtiFrameLayer(null).CreateClient(1);
            frameClient.ReceiveFrameCallback = x => received.Add(x.Buffer.Length);
            for (int offset = 0; offset < buffer.Count; offset += segmentSize)
            {
                var segment = buffer.GetRange(offset, Math.Min(segmentSize, buffer.Count - offset));
                frameClient.HandleData(new DataChunk(segment.ToArray(), 0, false, ApiTypes.Pti), true);
            }
            return received;
        }

        [Test]
        public void HandleData_FrameSplitOverSegments_IsParsed()
        {
            Assert.AreEqual(
                new[] { FrameLength(2000) },
                ParseFrameLengthsInSegments(1024, 2000));
        }

        [Test]
        public void HandleData_FramesSplitOverUnalignedSegments_AllAreParsed()
        {
            Assert.AreEqual(
                new[] { FrameLength(20), FrameLength(2000), FrameLength(20) },
                ParseFrameLengthsInSegments(1024, 20, 2000, 20));
        }

        [Test]
        public void HandleData_FrameSplitByteByByte_IsParsed()
        {
            Assert.AreEqual(
                new[] { FrameLength(300) },
                ParseFrameLengthsInSegments(1, 300));
        }

        [Test]
        public void HandleData_DchVersion3FrameSplitByteByByte_IsParsed()
        {
            // The v3 header is 19 bytes, so a wrong version offset shows up here
            Assert.AreEqual(
                new[] { FrameLength(300, DchVersion3) },
                ParseFrameLengthsInSegments(1, DchVersion3, 300));
        }

        [Test]
        public void HandleData_DchVersion3FramesSplitOverSegments_AllAreParsed()
        {
            Assert.AreEqual(
                new[] { FrameLength(20, DchVersion3), FrameLength(2000, DchVersion3) },
                ParseFrameLengthsInSegments(1024, DchVersion3, 20, 2000));
        }
    }
}
