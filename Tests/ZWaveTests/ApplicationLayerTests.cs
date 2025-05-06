using System;
using NUnit.Framework;
using ZWave.Layers;
using ZWave.Layers.Session;
using ZWave.Enums;

namespace ZWaveTests
{
    //[TestFixture]
    public class ApplicationLayerTests
    {
        private const int MAX_SESSIONS = 0x7F;

        private const ApiTypes ApiType = ApiTypes.Basic;
        private FakeApplicationLayer _applicationLayer;
        ISessionLayer _sessionLayer;

        //[SetUp]
        //public void SetUp()
        //{
        //    _sessionLayer = new SessionLayer();
        //    _applicationLayer = new FakeApplicationLayer(ApiType,
        //        Substitute.For<ISessionLayer>(),
        //        Substitute.For<IFrameLayer>(),
        //        Substitute.For<ITransportLayer>()
        //        );
        //}

        //[Test]
        //public void NextSessionId_IncrementsSessionId()
        //{
        //    // Arrange.
        //    const int max = 10;

        //    // Act.
        //    ushort sessionId = 0;
        //    for (int i = 0; i < max; i++)
        //    {
        //        sessionId = _sessionLayer.NextSessionId();
        //    }

        //    // Assert.
        //    Assert.AreEqual(max, sessionId);
        //}

        //[Test]
        //public void ResetSessionIdCounter_ResetsSessionIdToZero()
        //{
        //    // Arrange.
        //    const int max = 10;

        //    // Act.
        //    ushort sessionId = 0;
        //    for (int i = 0; i < max; i++)
        //    {
        //        sessionId = _sessionLayer.NextSessionId();
        //    }
        //    Assert.IsTrue(sessionId > 0);

        //    _sessionLayer.ResetSessionIdCounter();
        //    sessionId = _sessionLayer.NextSessionId();

        //    // Assert.
        //    Assert.AreEqual(1, sessionId);
        //}

        //[Test]
        //public void ReleaseSessionIdAndCallNextSessionId_ReturnsReleasedValue()
        //{
        //    // Arrange.
        //    const byte released = 5;

        //    // Act.
        //    var sessionId = 0;
        //    for (int i = 0; i < SessionLayer.MAX_SESSIONS; i++)
        //    {
        //        sessionId = _sessionLayer.NextSessionId();
        //    }

        //    _sessionLayer.ReleaseSessionId(released);
        //    sessionId = _sessionLayer.NextSessionId();

        //    // Assert.
        //    Assert.AreEqual(released, sessionId);
        //}

        //[Test]
        //public void FullTable_CallNextSessionId_ReturnsException()
        //{
        //    // Arrange.

        //    // Act.
        //    var sessionId = 0;
        //    for (int i = 0; i < SessionLayer.MAX_SESSIONS; i++)
        //    {
        //        sessionId = _sessionLayer.NextSessionId();
        //    }

        //    Assert.Throws(typeof(ZWave.Exceptions.ZWaveException), () => _sessionLayer.NextSessionId());
        //}

        //[Test]
        //public void CreateClient_EveryLayerCreatesCorrespondingClient()
        //{
        //    // Act.
        //    var client = _applicationLayer.CreateClient();

        //    // Assert.
        //    _applicationLayer.SessionLayer.Received(1).CreateClient(Arg.Any<ushort>());
        //    _applicationLayer.FrameLayer.Received(1).CreateClient(Arg.Any<ushort>());
        //    _applicationLayer.TransportLayer.Received(1).CreateClient(Arg.Any<ushort>());
        //}

        //[Test]
        //public void CreateClient_CreatedClientHasNextFreeSessionId()
        //{
        //    // Arrange.
        //    const int max = 10;

        //    // Act.
        //    var sessionId = 0;
        //    for (int i = 0; i < max; i++)
        //    {
        //        sessionId = _sessionLayer.NextSessionId();
        //    }
        //    _sessionLayer.ReleaseSessionId(max / 2);
        //    sessionId = _sessionLayer.NextSessionId();

        //    // Assert.
        //    Assert.AreEqual(max + 1, sessionId);
        //}

        //[Test]
        //public void CreateClient_CreatedClientHasCorrectApiType()
        //{
        //    // Act.
        //    var client = _applicationLayer.CreateClient();

        //    // Assert.
        //    Assert.AreEqual(ApiType, client.ApiType);
        //}
    }
}
