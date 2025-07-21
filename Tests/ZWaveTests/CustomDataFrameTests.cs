/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Linq;
using NUnit.Framework;
using ZWave.Enums;
using Utils;

namespace ZWaveTests
{
    [TestFixture]
    public class CustomDataFrameTests
    {
        private FakeCustomDataFrame _fakeCustomDataFrame;

        private const byte SessionId = 0x5;
        private const DataFrameTypes TypeData = DataFrameTypes.Data;
        private const bool IsHandled = true;
        private const bool IsOutcome = true;
        private readonly DateTime TimeNow = DateTime.Now;

        [SetUp]
        public void SetUp()
        {
            _fakeCustomDataFrame = new FakeCustomDataFrame(SessionId, TypeData, IsHandled, IsOutcome, TimeNow);
        }

        [Test]
        public void CreatedCustomDataFrame_AllPropertiesInitializedCorrectly()
        {
            // Assert.
            Assert.AreEqual(SessionId, _fakeCustomDataFrame.SessionId);
            Assert.AreEqual(TypeData, _fakeCustomDataFrame.DataFrameType);
            Assert.AreEqual(IsHandled, _fakeCustomDataFrame.IsHandled);
            Assert.AreEqual(IsOutcome, _fakeCustomDataFrame.IsOutcome);
            Assert.AreEqual(TimeNow, _fakeCustomDataFrame.SystemTimeStamp);
        }

        [Test]
        public void SetBuffer_CorrectlyCopeisBuffer()
        {
            // Arrange.
            var incomeBuffer = new byte[] { 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7 };
            var resultBuffer = new byte[] { 0x3, 0x4, 0x5 };

            // Act.
            _fakeCustomDataFrame.SetBuffer(incomeBuffer, 2, 3);

            // Assert.
            Assert.IsTrue(resultBuffer.SequenceEqual(_fakeCustomDataFrame.Buffer));
        }

        [Test]
        public void SetBuffer_RefreshDataAndRefreshPayloadAreCalled()
        {
            // Arrange.
            var incomeBuffer = new byte[] { 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7 };
            var resultBuffer = new byte[] { 0x3, 0x4, 0x5 };

            // Act.
            _fakeCustomDataFrame.SetBuffer(incomeBuffer, 2, 3);

            // Assert.
            Assert.IsTrue(_fakeCustomDataFrame.RefreshDataCalled);
            Assert.IsTrue(_fakeCustomDataFrame.RefreshPayloadCalled);
            Assert.IsTrue(resultBuffer.SequenceEqual(_fakeCustomDataFrame.Data));
            Assert.IsTrue(resultBuffer.SequenceEqual(_fakeCustomDataFrame.Payload));
        }
    }
}
