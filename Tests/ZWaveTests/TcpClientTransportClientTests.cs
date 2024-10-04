/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using ZWave.Layers;
using ZWave.Layers.Transport;
using ZWave.Enums;
using Moq;

namespace ZWaveTests
{
    //[TestFixture]
    public class TcpClientTransportClientTests
    {
        private ITransportClient _tcpClientTransportClient;
        private byte[] _data = new byte[] { 0x0, 0x1, 0x2 };
        private const int _sessionId = 0x79;
        private const ApiTypes _apiType = ApiTypes.Basic;
        private DataChunk _transmitedDataChunk;
        private DataChunk _receivedDataChunk;
        private Utils.IDataSource _dataSource;

        [SetUp]
        public void SetUp()
        {
            _dataSource = new SocketDataSource("192.168.1.111", 4901);
            var res = _dataSource.Validate();
            Assert.IsTrue(res);
            _tcpClientTransportClient = new TcpClientTransportClient(OnDataTransmitted)
            {
                DataSource = _dataSource,
                SessionId = _sessionId,
                ApiType = _apiType
            };
            _transmitedDataChunk = null;
            _receivedDataChunk = null;
        }

        [TearDown]
        public void TearDown()
        {
            _tcpClientTransportClient.Dispose();
        }

        [Test]
        public void Connect_ConnectionFailed_CommunicationStatusIsFailed()
        {
            // Arrange.
            var expectedStatus = CommunicationStatuses.Failed;
            var tcpConnectionMock = new Mock<ITcpConnection>();
            tcpConnectionMock.Setup(x => x.Connect(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(false);
            ((TcpClientTransportClient)_tcpClientTransportClient).TcpConnection = tcpConnectionMock.Object;

            // Act.
            var status = _tcpClientTransportClient.Connect();

            // Assert.
            Assert.AreEqual(expectedStatus, status);
        }

        [Test]
        public void Connect_ConnectionEstablished_TcpConnectionReadIsCalled()
        {
            // Arrange.
            var tcpConnectionMock = new Mock<ITcpConnection>();
            var tcpConnectionDisposableMock = tcpConnectionMock.As<IDisposable>();
            tcpConnectionMock.SetupGet(x => x.Connected).Returns(true);
            ((TcpClientTransportClient)_tcpClientTransportClient).TcpConnection = tcpConnectionMock.Object;

            // Act.
            var status = _tcpClientTransportClient.Connect();
            Thread.Sleep(100);
            // Assert.
            Assert.AreEqual(CommunicationStatuses.Done, status);
            byte[] data = null;
            tcpConnectionMock.Verify(x => x.Read(out data), Times.Once);
        }

        [Test]
        public void Disconnect_NotConnected_ReturnsSilently()
        {
            // Arrange.
            var tcpConnectionMock = new Mock<ITcpConnection>();
            var tcpConnectionDisposableMock = tcpConnectionMock.As<IDisposable>();
            ((TcpClientTransportClient)_tcpClientTransportClient).TcpConnection = tcpConnectionMock.Object;

            // Act.
            _tcpClientTransportClient.Disconnect();

            // Assert.
            tcpConnectionDisposableMock.Verify(x=>x.Dispose(), Times.Never);
        }

        [Test]
        public void Disconnect_ConnectedBefore_DisposeIsCalled()
        {
            // Arrange.
            var tcpConnectionMock = new Mock<ITcpConnection>();
            var tcpConnectionDisposableMock = tcpConnectionMock.As<IDisposable>();
            ((TcpClientTransportClient)_tcpClientTransportClient).TcpConnection = tcpConnectionMock.Object;
            tcpConnectionMock.SetupGet(x=>x.Connected).Returns(true);
            _tcpClientTransportClient.Connect();

            // Act.
            _tcpClientTransportClient.Disconnect();
            Thread.Sleep(500);

            // Assert.
            tcpConnectionDisposableMock.Verify(x => x.Dispose(), Times.Once);
        }

        [Test]
        public void IsOpen_ConnectedReturnsTrue_ReturnsTrue()
        {
            // Arrange.
            var tcpConnectionMock = new Mock<ITcpConnection>();
            ((TcpClientTransportClient)_tcpClientTransportClient).TcpConnection = tcpConnectionMock.Object;
            tcpConnectionMock.SetupGet(x => x.Connected).Returns(true);

            // Act.
            bool res = _tcpClientTransportClient.IsOpen;

            // Assert.
            Assert.IsTrue(res);
        }

        [Test]
        public void TcpConnection_TcpConnectionIsNull_ReturnsNewTcpConnection()
        {
            // Arrange.
            var tcpConnectionMock = new Mock<ITcpConnection>();
            ((TcpClientTransportClient)_tcpClientTransportClient).TcpConnection = tcpConnectionMock.Object;
            tcpConnectionMock.SetupGet(x => x.Connected).Returns(true);

            // Act.
            var res = ((TcpClientTransportClient)_tcpClientTransportClient).TcpConnection;

            // Assert.
            Assert.IsNotNull(res);
        }

        private void OnDataTransmitted(DataChunk dataChunk)
        {
            _transmitedDataChunk = dataChunk;
        }
    }
}
