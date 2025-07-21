/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using Moq;
using NUnit.Framework;
using Utils;
using ZWave;
using ZWave.Enums;
using ZWave.Layers;
using ZWave.Layers.Application;

namespace ZWaveTests
{
    [TestFixture]
    public class ApplicationClientTests
    {
        private const byte SessionId = 0x3;
        private const ApiTypes ApiType = ApiTypes.Basic;
        private ApplicationClient _applicationClient;
        private Mock<ITransportClient> _transportClientMock;
        private Mock<IFrameClient> _frameClientMock;
        private Mock<ISessionClient> _sessionClientMock;

        [SetUp]
        public void SetUp()
        {
            _transportClientMock = new Mock<ITransportClient>();
            _transportClientMock.SetupAllProperties();

            _frameClientMock = new Mock<IFrameClient>();
            _frameClientMock.SetupAllProperties();

            _sessionClientMock = new Mock<ISessionClient>();
            _sessionClientMock.SetupAllProperties();

            _applicationClient = new ApplicationClient(ApiType,
                SessionId,
                _sessionClientMock.Object,
                _frameClientMock.Object,
                _transportClientMock.Object);
        }

        [Test]
        public void SetUp_SavesApiTypeAndSessionId()
        {
            // Assert.
            Assert.AreEqual(ApiType, _applicationClient.ApiType);
            Assert.AreEqual(SessionId, _applicationClient.SessionId);
        }

        [Test]
        public void DataSource_ReturnsDataSourceFromTransportClient()
        {
            // Arrange.
            var dataSource = new SocketDataSource("1.1.1.1", 41230, "1234567890");
            _transportClientMock.Setup(x => x.DataSource).Returns(dataSource);

            // Assert.
            Assert.AreSame(dataSource, _applicationClient.DataSource);
        }

        [Test]
        public void Connect_NoArguments_TransportClientConnectCalled()
        {
            // Arrange.

            // Act.
            _applicationClient.Connect();

            // Assert.
            _transportClientMock.Verify(x => x.Connect(), Times.Once());
        }

        [Test]
        public void Connect_WithArguments_TransportClientConnectCalled()
        {
            // Arrange.
            var dataSource = new SocketDataSource("1.1.1.1", 41230, "1234567890");
            _transportClientMock.Setup(x => x.DataSource).Returns(dataSource);

            // Act.
            _applicationClient.Connect(dataSource);

            // Assert.
            _transportClientMock.Verify(x => x.Connect(It.Is<IDataSource>(y => y.SourceName == dataSource.SourceName)));
        }

        [Test]
        public void Disconnect_TransportClientDisconnectCalled()
        {
            // Arrange.

            // Act.
            _applicationClient.Disconnect();

            // Assert.
            _transportClientMock.Verify(x => x.Disconnect(), Times.Once());
        }

        [Test]
        public void SetUp_LayersInitializedWithSessionIdAndApiType()
        {
            // Assert.
            Assert.AreEqual(SessionId, _applicationClient.SessionClient.SessionId);
            Assert.AreEqual(SessionId, _applicationClient.FrameClient.SessionId);
            Assert.AreEqual(SessionId, _applicationClient.TransportClient.SessionId);
            Assert.AreEqual(ApiType, _applicationClient.TransportClient.ApiType);
        }

        [Test]
        public void SetUp_OnSendData_SessionLayerPassDataToFrameLayer()
        {
            // Arrange.
            var ahr = new ActionHandlerResult(new FakeAction());
            _applicationClient.SessionClient.SendFramesCallback(ahr);

            // Assert
            _frameClientMock.Verify(x => x.SendFrames(It.Is<ActionHandlerResult>(y => y == ahr)));
        }

        [Test]
        public void SetUp_OnSendData_FrameLayerPassDataToTransportLayer()
        {
            // Arrange.
            byte[] data = new byte[] { 0x1 };
            _applicationClient.FrameClient.SendDataCallback(data);

            // Assert
            _transportClientMock.Verify(x => x.WriteData(It.Is<byte[]>(y => y == data)));
        }

        [Test]
        public void SetUp_OnReceiveData_FrameLayerPassDataToSessionLayer()
        {
            // Arrange.
            var dataFrame = new FakeDataFrame(SessionId, DataFrameTypes.Data, true, true, DateTime.Now);
            _applicationClient.FrameClient.ReceiveFrameCallback(dataFrame);

            // Assert
            _sessionClientMock.Verify(x => x.HandleActionCase(It.Is<IActionCase>(y => y == dataFrame)));
        }

        [Test]
        public void SetUp_OnReceiveData_TransportLayerPassDataToFrameLayer()
        {
            // Arrange.
            var dataChunk = new DataChunk(new byte[] { 0x1 }, SessionId, false, ApiType);
            _applicationClient.TransportClient.ReceiveDataCallback(dataChunk, false);

            // Assert
            _frameClientMock.Verify(x => x.HandleData(It.Is<DataChunk>(y => y == dataChunk), It.Is<bool>(y => y == false)));
        }

        [Test]
        public void Dispose_UnbindLayers_AllLayersIsDisposed()
        {
            // Act.
            _applicationClient.Dispose();

            // Assert.
            _sessionClientMock.Verify(x=>x.Dispose());
            _frameClientMock.Verify(x => x.Dispose());
            _transportClientMock.Verify(x => x.Dispose());

            Assert.AreEqual(_applicationClient.SessionClient.SendFramesCallback, null);
            Assert.AreEqual(_applicationClient.FrameClient.ReceiveFrameCallback, null);
            Assert.AreEqual(_applicationClient.FrameClient.SendDataCallback, null);
            Assert.AreEqual(_applicationClient.TransportClient.ReceiveDataCallback, null);
        }
    }
}
