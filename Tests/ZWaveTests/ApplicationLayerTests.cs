/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using NUnit.Framework;
using ZWave.Layers;
using ZWave.Layers.Session;
using ZWave.Enums;
using Moq;

namespace ZWaveTests
{
    [TestFixture]
    public class ApplicationLayerTests
    {
        private const ApiTypes ApiType = ApiTypes.Basic;
        private FakeApplicationLayer _applicationLayer;
        private Mock<ITransportLayer> _transportLayerMock;
        private Mock<IFrameLayer> _frameLayerMock;
        private Mock<ISessionLayer> _sessionLayerMock;

        private ISessionLayer _sessionLayer;

        [SetUp]
        public void SetUp()
        {
            _sessionLayer = new SessionLayer();

            _transportLayerMock = new Mock<ITransportLayer>();
            _transportLayerMock
                .Setup(x => x.CreateClient(It.IsAny<ushort>()))
                .Returns(() => Mock.Of<ITransportClient>());

            _frameLayerMock = new Mock<IFrameLayer>();
            _frameLayerMock
                .Setup(x => x.CreateClient(It.IsAny<ushort>()))
                .Returns(() => Mock.Of<IFrameClient>());

            _sessionLayerMock = new Mock<ISessionLayer>();
            _sessionLayerMock
                .Setup(x => x.CreateClient(It.IsAny<ushort>()))
                .Returns(() => Mock.Of<ISessionClient>());

            _applicationLayer = new FakeApplicationLayer(ApiType,
                _sessionLayerMock.Object,
                _frameLayerMock.Object,
                _transportLayerMock.Object);
        }

        [Test]
        public void NextSessionId_IncrementsSessionId()
        {
            // Arrange.
            const int max = 10;

            // Act.
            ushort sessionId = 0;
            for (int i = 0; i < max; i++)
            {
                sessionId = _sessionLayer.NextSessionId();
            }

            // Assert.
            Assert.AreEqual(max, sessionId);
        }

        [Test]
        public void ResetSessionIdCounter_ResetsSessionIdToZero()
        {
            // Arrange.
            const int max = 10;

            // Act.
            ushort sessionId = 0;
            for (int i = 0; i < max; i++)
            {
                sessionId = _sessionLayer.NextSessionId();
            }
            Assert.IsTrue(sessionId > 0);

            _sessionLayer.ResetSessionIdCounter();
            sessionId = _sessionLayer.NextSessionId();

            // Assert.
            Assert.AreEqual(1, sessionId);
        }

        [Test]
        public void ReleaseSessionIdAndCallNextSessionId_ReturnsReleasedValue()
        {
            // Arrange.
            const byte released = 5;

            // Act.
            var sessionId = 0;
            for (int i = 0; i < SessionLayer.MAX_SESSIONS; i++)
            {
                sessionId = _sessionLayer.NextSessionId();
            }

            _sessionLayer.ReleaseSessionId(released);
            sessionId = _sessionLayer.NextSessionId();

            // Assert.
            Assert.AreEqual(released, sessionId);
        }

        [Test]
        public void FullTable_CallNextSessionId_ReturnsException()
        {
            // Arrange.

            // Act.
            var sessionId = 0;
            for (int i = 0; i < SessionLayer.MAX_SESSIONS; i++)
            {
                sessionId = _sessionLayer.NextSessionId();
            }

            Assert.Throws(typeof(ZWave.Exceptions.ZWaveException), () => _sessionLayer.NextSessionId());
        }

        [Test]
        public void CreateClient_EveryLayerCreatesCorrespondingClient()
        {
            // Act.
            var client = _applicationLayer.CreateClient();

            // Assert.
            _sessionLayerMock.Verify(x => x.CreateClient(It.IsAny<ushort>()), Times.Once());
            _frameLayerMock.Verify(x => x.CreateClient(It.IsAny<ushort>()), Times.Once());
            _transportLayerMock.Verify(x => x.CreateClient(It.IsAny<ushort>()), Times.Once());
        }

        [Test]
        public void CreateClient_CreatedClientHasNextFreeSessionId()
        {
            // Arrange.
            const int max = 10;

            // Act.
            var sessionId = 0;
            for (int i = 0; i < max; i++)
            {
                sessionId = _sessionLayer.NextSessionId();
            }
            _sessionLayer.ReleaseSessionId(max / 2);
            sessionId = _sessionLayer.NextSessionId();

            // Assert.
            Assert.AreEqual(max + 1, sessionId);
        }

        [Test]
        public void CreateClient_CreatedClientHasCorrectApiType()
        {
            // Act.
            var client = _applicationLayer.CreateClient();

            // Assert.
            Assert.AreEqual(ApiType, client.ApiType);
        }
    }
}
