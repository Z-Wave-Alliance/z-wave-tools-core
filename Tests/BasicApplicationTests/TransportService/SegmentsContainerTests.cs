/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Linq;
using NUnit.Framework;
using ZWave.BasicApplication.TransportService;
using ZWave.CommandClasses;

namespace BasicApplicationTests.TransportService
{
    [TestFixture]
    public class SegmentsContainerTests
    {
        [Test]
        public void CreateSegmentsContainer_AddFirstSegment_NotCompletedHasDatagramSizeAndSessionId()
        {
            // Arrange.
            var segmentsContainer = new SegmentsContainer(DataProvider.SegmentCmds[0]);

            // Act.

            // Assert.
            Assert.IsFalse(segmentsContainer.IsLastSegmentReceived);
            Assert.IsFalse(segmentsContainer.IsCompleted);
            Assert.AreEqual(DataProvider.Datagram.Count, segmentsContainer.DatagramSize);
            Assert.AreEqual(DataProvider.SessionId, segmentsContainer.SessionId);
            var cmd = (COMMAND_CLASS_TRANSPORT_SERVICE_V2.COMMAND_FIRST_SEGMENT)DataProvider.SegmentCmds[0];
            Assert.IsTrue(cmd.payload.SequenceEqual(segmentsContainer.Segments[0]));
        }

        [Test]
        public void CreateSegmentsContainer_FirstSegmentContainsAllData_IsCompletedHasFullDatagram()
        {
            // Arrange.
            var firstSegment = new byte[] { 0x55, 0xC0, 0x02, 0x10, COMMAND_CLASS_VERSION_V2.ID, COMMAND_CLASS_VERSION_V2.VERSION_GET.ID, 0x2C, 0x36 };
            byte[] versionGetCmd = new COMMAND_CLASS_VERSION_V2.VERSION_GET();
            var segmentsContainer = new SegmentsContainer(firstSegment);

            // Act.

            // Assert.
            Assert.IsTrue(segmentsContainer.IsLastSegmentReceived);
            Assert.IsTrue(segmentsContainer.IsCompleted);
            Assert.AreEqual(versionGetCmd.Length, segmentsContainer.DatagramSize);
            Assert.AreEqual(1, segmentsContainer.SessionId);
            Assert.IsTrue(versionGetCmd.SequenceEqual(segmentsContainer.GetDatagram()));
        }

        [Test]
        public void FillWithSegments_AllSegmentsPassed_IsCompletedHasFullDatagram()
        {
            // Arrange.
            var segmentsContainer = new SegmentsContainer(DataProvider.SegmentCmds[0]);

            // Act.
            for (int i = 1; i < DataProvider.SegmentCmds.Count; i++)
            {
                segmentsContainer.AddSegment(DataProvider.SegmentCmds[i]);
            }

            // Assert.
            Assert.IsTrue(segmentsContainer.IsLastSegmentReceived);
            Assert.IsTrue(segmentsContainer.IsCompleted);
            Assert.IsTrue(DataProvider.Datagram.SequenceEqual(segmentsContainer.GetDatagram()));
            Assert.AreEqual(0, segmentsContainer.GetFirstMissingFragmentOffset());
        }

        [Test]
        public void AddSegment_SegmentWithWrongSessionId_SegmentIsIgnored()
        {
            // Arrange.
            var segmentsContainer = new SegmentsContainer(DataProvider.SegmentCmds[0]);
            var segment = new byte[DataProvider.SegmentCmds[1].Length];
            DataProvider.SegmentCmds[1].CopyTo(segment, 0);
            var sessionId = DataProvider.SessionId + 7; // sessionId = 9 is 00001001
            segment[3] = 0x01; // Extenssion bit.
            segment[3] = (byte)(segment[3] << 3);
            segment[3] = (byte)(segment[3] | (sessionId << 4));
            // segment[4] must be 10011000

            // Act.
            segmentsContainer.AddSegment(segment);

            // Assert.
            Assert.IsFalse(segmentsContainer.IsCompleted);
            Assert.AreEqual(1, segmentsContainer.Segments.Count);
        }

        [Test]
        public void GetFirstMissingFragmentOffset_ThrirdIsMissed_ReturnsThirdFragmentOffset()
        {
            // Arrange.
            var segmentsContainer = new SegmentsContainer(DataProvider.SegmentCmds[0]);

            // Act.
            for (int i = 1; i < DataProvider.SegmentCmds.Count; i++)
            {
                if (i == 2)
                    continue;
                segmentsContainer.AddSegment(DataProvider.SegmentCmds[i]);
            }

            ushort missingOffset = segmentsContainer.GetFirstMissingFragmentOffset();

            // Assert.
            Assert.IsTrue(segmentsContainer.IsLastSegmentReceived);
            Assert.IsFalse(segmentsContainer.IsCompleted);
            Assert.IsNull(segmentsContainer.GetDatagram());
            Assert.AreEqual(DataProvider.SegmentsByOffsets.Keys.ToArray()[2], segmentsContainer.GetFirstMissingFragmentOffset());
        }

        [Test]
        public void FillWithSegments_HasMissingSegmentsFillWithMissedSegments_IsCompletedHasFullDatagram()
        {
            // Arrange.
            var segmentsContainer = new SegmentsContainer(DataProvider.SegmentCmds[0]);
            int missedIndex1 = 2;
            int missedIndex2 = 4;
            int missedIndex3 = 6;

            // Act.
            for (int i = 1; i < DataProvider.SegmentCmds.Count; i++)
            {
                if (i == missedIndex1 || i == missedIndex2 || i == missedIndex3)
                    continue;
                segmentsContainer.AddSegment(DataProvider.SegmentCmds[i]);
            }
            Assert.IsTrue(segmentsContainer.IsLastSegmentReceived);
            Assert.IsFalse(segmentsContainer.IsCompleted);

            ushort missingOffset = segmentsContainer.GetFirstMissingFragmentOffset();
            segmentsContainer.AddSegment(DataProvider.SegmentsByOffsets[missingOffset]);
            Assert.IsFalse(segmentsContainer.IsCompleted);

            missingOffset = segmentsContainer.GetFirstMissingFragmentOffset();
            segmentsContainer.AddSegment(DataProvider.SegmentsByOffsets[missingOffset]);
            Assert.IsFalse(segmentsContainer.IsCompleted);

            missingOffset = segmentsContainer.GetFirstMissingFragmentOffset();
            segmentsContainer.AddSegment(DataProvider.SegmentsByOffsets[missingOffset]);
            
            // Assert.
            Assert.IsTrue(segmentsContainer.IsCompleted);
            Assert.IsTrue(DataProvider.Datagram.SequenceEqual(segmentsContainer.GetDatagram()));
            Assert.AreEqual(0, segmentsContainer.GetFirstMissingFragmentOffset());
        }

        [Test]
        public void CheckForLastSegment_EmptyContainer_ReturnsFalse()
        {
            // Arrange.
            var segmentsContainer = new SegmentsContainer(DataProvider.SegmentCmds[0]);

            // Act.
            bool isLastSegment = segmentsContainer.CheckForLastSegment(DataProvider.SegmentCmds.Last());

            // Assert.
            Assert.IsFalse(isLastSegment);
        }

        [Test]
        public void CheckForLastSegment_AllSegmentsPassedAndLastSegmentAreBeingChecking_ReturnsTrue()
        {
            // Arrange.
            var segmentsContainer = new SegmentsContainer(DataProvider.SegmentCmds[0]);

            // Act.
            for (int i = 1; i < DataProvider.SegmentCmds.Count; i++)
            {
                segmentsContainer.AddSegment(DataProvider.SegmentCmds[i]);
            }
            bool isLastSegment = segmentsContainer.CheckForLastSegment(DataProvider.SegmentCmds.Last());

            // Assert.
            Assert.IsTrue(isLastSegment);
        }

        [Test]
        public void CheckForLastSegment_NotAllSegmentsPassedAndLastSegmentAreBeingChecking_ReturnsFalse()
        {
            // Arrange.
            var segmentsContainer = new SegmentsContainer(DataProvider.SegmentCmds[0]);

            // Act.
            for (int i = 1; i < DataProvider.SegmentCmds.Count / 2; i++)
            {
                segmentsContainer.AddSegment(DataProvider.SegmentCmds[i]);
            }
            bool isLastSegment = segmentsContainer.CheckForLastSegment(DataProvider.SegmentCmds.Last());

            // Assert.
            Assert.IsFalse(isLastSegment);
        }
    }
}
