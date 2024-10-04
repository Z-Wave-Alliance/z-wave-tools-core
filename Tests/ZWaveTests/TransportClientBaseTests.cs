/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using NUnit.Framework;
using ZWave.Layers;

namespace ZWaveTests
{
    [TestFixture]
    public class TransportClientBaseTests
    {
        [Test]
        public void Connect_DataSourceIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange.
            ITransportClient fakeTransportClient = new FakeTransportClient();

            // Act.
            Assert.That(() => fakeTransportClient.Connect(null), Throws.TypeOf<ArgumentNullException>());

            // Assert.
        }

        [Test]
        public void Connect_DataSourceValidationFailed_ArgumentExceptionIsThrown()
        {
            // Arrange.
            ITransportClient fakeTransportClient = new FakeTransportClient
            {
                DataSource = new SerialPortDataSource()
            };

            // Act.
            Assert.IsFalse(fakeTransportClient.DataSource.Validate()); // Data source should be not valid.
            Assert.That(() => fakeTransportClient.Connect(), Throws.TypeOf<ArgumentException>());

            // Assert.
        }

        [Test]
        public void Connect_CorrectDataSource_SavesDataSource()
        {
            // Arrange.
            ITransportClient fakeTransportClient = new FakeTransportClient();
            var inputDataSource = new SocketDataSource("1.1.1.1", 41230, "1234567890");

            // Act.
            var status = fakeTransportClient.Connect(inputDataSource);

            // Assert.
            Assert.AreSame(inputDataSource, fakeTransportClient.DataSource);
        }

        [Test]
        public void Connect_CorrectDataSource_InnerConnectCalled()
        {
            // Arrange.
            FakeTransportClient fakeTransportClient = new FakeTransportClient();
            var inputDataSource = new SocketDataSource("1.1.1.1", 41230, "1234567890");

            // Act.
            var status = fakeTransportClient.Connect(inputDataSource);

            // Assert.
            Assert.IsTrue(fakeTransportClient.InnerConnectCalled);
        }

        [Test]
        public void Connect_CorrectDataSource_SavesConnectionStatus()
        {
            // Arrange.
            ITransportClient fakeTransportClient = new FakeTransportClient();
            var inputDataSource = new SocketDataSource("1.1.1.1", 41230, "1234567890");

            // Act.
            fakeTransportClient.Connect(inputDataSource);

            // Assert.
            Assert.AreEqual(true, fakeTransportClient.IsOpen);
        }

        [Test]
        public void Disconnect_ConnectionStatusIsDone_InnerDisconnectCalled()
        {
            // Arrange.
            FakeTransportClient fakeTransportClient = new FakeTransportClient();
            var inputDataSource = new SocketDataSource("1.1.1.1", 41230, "1234567890");

            // Act.
            fakeTransportClient.Connect(inputDataSource);
            fakeTransportClient.Disconnect();

            // Assert.
            Assert.IsTrue(fakeTransportClient.InnerDisconnectCalled);
        }

        [Test]
        public void WriteData_CorrectData_InnerWriteDataCalled()
        {
            // Arrange.
            FakeTransportClient fakeTransportClient = new FakeTransportClient();
            const int DATA_LENGTH = 3;
            var inputData = new byte[DATA_LENGTH];

            // Act.
            var written = fakeTransportClient.WriteData(inputData);

            // Assert.
            Assert.IsTrue(fakeTransportClient.InnerWriteDataCalled);
        }

        [Test]
        public void WriteData_DataIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange.
            ITransportClient fakeTransportClient = new FakeTransportClient();

            // Act.
            Assert.That(() => fakeTransportClient.WriteData(null), Throws.TypeOf<ArgumentNullException>());

            // Assert.
        }

        [Test]
        public void WriteData_EmptyData_ArgumentExceptionIsThrown()
        {
            // Arrange.
            ITransportClient fakeTransportClient = new FakeTransportClient();

            // Act.
            Assert.That(() => fakeTransportClient.WriteData(new byte[] { }), Throws.TypeOf<ArgumentException>());

            // Assert.
        }

        [Test]
        public void Dispose_AnyCondition_InnerDisposeCalled()
        {
            // Arrange.
            FakeTransportClient fakeTransportClient = new FakeTransportClient();

            // Act.
            fakeTransportClient.Dispose();

            // Assert.
            Assert.IsTrue(fakeTransportClient.InnerDisposeCalled);
        }
    }
}
