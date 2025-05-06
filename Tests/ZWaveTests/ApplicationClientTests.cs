using System;
using NUnit.Framework;
using ZWave;
using ZWave.Enums;
using ZWave.Layers;
using ZWave.Layers.Application;

namespace ZWaveTests
{
    //[TestFixture]
    public class ApplicationClientTests
    {
        private const byte SessionId = 0x3;
        private const ApiTypes ApiType = ApiTypes.Basic;
        private ApplicationClient _applicationClient;

        //[SetUp]
        //public void SetUp()
        //{
        //    _applicationClient = new ApplicationClient(ApiType,
        //        SessionId,
        //        Substitute.For<ISessionClient>(),
        //        Substitute.For<IFrameClient>(),
        //        Substitute.For<ITransportClient>()
        //        );
        //}

        //[Test]
        //public void SetUp_SavesApiTypeAndSessionId()
        //{
        //    // Assert.
        //    Assert.AreEqual(ApiType, _applicationClient.ApiType);
        //    Assert.AreEqual(SessionId, _applicationClient.SessionId);
        //}

        //[Test]
        //public void DataSource_ReturnsDataSourceFromTransportClient()
        //{
        //    // Arrange.
        //    var dataSource = new SocketDataSource("1.1.1.1", 41230, "1234567890");
        //    _applicationClient.TransportClient.DataSource.Returns(dataSource);

        //    // Assert.
        //    Assert.AreSame(dataSource, _applicationClient.DataSource);
        //}

        //[Test]
        //public void Connect_NoArguments_TransportClientConnectCalled()
        //{
        //    // Arrange.
        //    var transportClientMock = Substitute.For<ITransportClient>();
        //    _applicationClient.TransportClient = transportClientMock;

        //    // Act.
        //    _applicationClient.Connect();

        //    // Assert.
        //    transportClientMock.Received(1).Connect();
        //}

        //[Test]
        //public void Connect_WithArguments_TransportClientConnectCalled()
        //{
        //    // Arrange.
        //    var dataSource = new SocketDataSource("1.1.1.1", 41230, "1234567890");
        //    var transportClientMock = Substitute.For<ITransportClient>();
        //    _applicationClient.TransportClient = transportClientMock;

        //    // Act.
        //    _applicationClient.Connect(dataSource);

        //    // Assert.
        //    transportClientMock.Received(1).Connect(Arg.Is(dataSource));
        //}

        //[Test]
        //public void Disconnect_TransportClientDisconnectCalled()
        //{
        //    // Arrange.
        //    var transportClientMock = Substitute.For<ITransportClient>();
        //    _applicationClient.TransportClient = transportClientMock;

        //    // Act.
        //    _applicationClient.Disconnect();

        //    // Assert.
        //    transportClientMock.Received(1).Disconnect();
        //}

        //[Test]
        //public void SetUp_LayersInitializedWithSessionIdAndApiType()
        //{
        //    // Assert.
        //    Assert.AreEqual(SessionId, _applicationClient.SessionClient.SessionId);
        //    Assert.AreEqual(SessionId, _applicationClient.FrameClient.SessionId);
        //    Assert.AreEqual(SessionId, _applicationClient.TransportClient.SessionId);
        //    Assert.AreEqual(ApiType, _applicationClient.TransportClient.ApiType);
        //}

        //[Test]
        //public void SetUp_OnSendData_SessionLayerPassDataToFrameLayer()
        //{
        //    // Arrange.
        //    var ahr = new ActionHandlerResult(new FakeAction());
        //    _applicationClient.SessionClient.SendFramesCallback(ahr);

        //    // Assert
        //    _applicationClient.FrameClient.Received(1).SendFrames(Arg.Is(ahr));
        //}

        //[Test]
        //public void SetUp_OnSendData_FrameLayerPassDataToTransportLayer()
        //{
        //    // Arrange.
        //    byte[] data = new byte[] {0x1};
        //    _applicationClient.FrameClient.SendDataCallback(data);

        //    // Assert
        //    _applicationClient.TransportClient.Received(1).WriteData(Arg.Is(data));
        //}

        //[Test]
        //public void SetUp_OnReceiveData_FrameLayerPassDataToSessionLayer()
        //{
        //    // Arrange.
        //    var dataFrame = new FakeDataFrame(SessionId, DataFrameTypes.Data, true, true, DateTime.Now);
        //    _applicationClient.FrameClient.ReceiveFrameCallback(dataFrame);

        //    // Assert
        //    _applicationClient.SessionClient.Received(1).HandleActionCase(Arg.Is(dataFrame));
        //}

        //[Test]
        //public void SetUp_OnReceiveData_TransportLayerPassDataToFrameLayer()
        //{
        //    // Arrange.
        //    var dataChunk = new DataChunk(new byte[] {0x1}, SessionId, false, ApiType);
        //    _applicationClient.TransportClient.ReceiveDataCallback(dataChunk, false);

        //    // Assert
        //    _applicationClient.FrameClient.Received(1).HandleData(Arg.Is(dataChunk), Arg.Is(false));
        //}

        //[Test]
        //public void Dispose_UnbindLayers_AllLayersIsDisposed()
        //{
        //    // Act.
        //    _applicationClient.Dispose();

        //    // Assert.
        //    _applicationClient.SessionClient.Received(1).Dispose();
        //    _applicationClient.FrameClient.Received(1).Dispose();
        //    _applicationClient.TransportClient.Received(1).Dispose();

        //    Assert.AreEqual(_applicationClient.SessionClient.SendFramesCallback, null);
        //    Assert.AreEqual(_applicationClient.FrameClient.ReceiveFrameCallback, null);
        //    Assert.AreEqual(_applicationClient.FrameClient.SendDataCallback, null);
        //    Assert.AreEqual(_applicationClient.TransportClient.ReceiveDataCallback, null);
        //}
    }
}
