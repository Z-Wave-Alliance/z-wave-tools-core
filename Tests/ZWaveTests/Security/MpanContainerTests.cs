using System;
using System.Linq;
using NUnit.Framework;
using ZWave.Devices;
using ZWave.Security;

namespace ZWaveTests.Security
{
    [TestFixture]
    public class MpanContainerTests
    {
        private byte[] _mpanState = new byte[] { 0x2a, 0xa2, 0x34, 0x0f, 0x7b, 0x1e, 0x67, 0x3a, 0x72, 0xa7, 0xa2, 0xab, 0xe9, 0x99, 0x14, 0xbd };
        private readonly byte _seqNo = 211;
        private static NodeGroupId _nodeGroupId = new NodeGroupId(new NodeTag(2), 7);

        private NodeTag[] _rgh = new byte[] { 0x03, 0xca, 0xfe }.Select(x => new NodeTag(x)).ToArray();

        private MpanContainer CreateDefaultMpanContainer()
        {
            return new MpanContainer(_nodeGroupId, _mpanState, _seqNo, _rgh);
        }

        [Test]
        public void CreateMpanContainer_PassValidParameters_ObjectInitializedCorrectly()
        {
            // Arrange.

            // Act.
            var mpanContainer = CreateDefaultMpanContainer();

            // Assert.
            Assert.IsTrue(_mpanState.SequenceEqual(mpanContainer.MpanState));
            Assert.IsTrue(_rgh.SequenceEqual(mpanContainer.ReceiverGroupHandle));
            Assert.AreEqual(_seqNo, mpanContainer.SequenceNumber);
            Assert.AreEqual(_nodeGroupId.GroupId, mpanContainer.NodeGroupId.GroupId);
            Assert.AreEqual(_nodeGroupId.Node, mpanContainer.NodeGroupId.Node);
        }

        [Test]
        public void SetMpan_NewMpanValuePassed_InternalMpanStateChanges()
        {
            // Arrange.
            var newMpanState = new byte[] { 0x49, 0x55, 0x0d, 0x72, 0xac, 0x57, 0x64, 0x23, 0xf3, 0xf8, 0x7a, 0x35, 0x14, 0xb7, 0x1e, 0x03 };
            var mpanContainer = CreateDefaultMpanContainer();

            // Act.
            mpanContainer.SetMpanState(newMpanState);

            // Assert.
            Assert.IsTrue(newMpanState.SequenceEqual(mpanContainer.MpanState));
        }

        [Test]
        public void SetMpan_HasMOSState_ThrowsException()
        {
            // Arrange.
            var newMpan = new byte[] { 0x49, 0x55, 0x0d, 0x72, 0xac, 0x57, 0x64, 0x23, 0xf3, 0xf8, 0x7a, 0x35, 0x14, 0xb7, 0x1e, 0x03 };
            var mpanContainer = CreateDefaultMpanContainer();

            // Act.
            mpanContainer.SetMosState(true);
            Assert.That(() => mpanContainer.SetMpanState(newMpan), Throws.TypeOf<InvalidOperationException>());

            // Assert.
        }

        [Test]
        public void UpdateSequenceNumber_NotInMOSState_IncrementsSequenceNumber()
        {
            // Arrange.
            var mpanContainer = CreateDefaultMpanContainer();

            // Act.
            int updatesCount = 7;
            for (int i = 0; i < updatesCount; i++)
                mpanContainer.UpdateSequenceNumber();

            // Assert.
            Assert.AreEqual(_seqNo + updatesCount, mpanContainer.SequenceNumber);
        }

        [Test]
        public void UpdateSequenceNumber_InMOSState_ThrowsException()
        {
            // Arrange.
            var mpanContainer = CreateDefaultMpanContainer();

            // Act.
            mpanContainer.SetMosState(true);
            Assert.That(() => mpanContainer.UpdateSequenceNumber(), Throws.TypeOf<InvalidOperationException>());

            // Assert.
        }

        [Test]
        public void SetMosState_WasntInMOSState_BecomesInMOSState()
        {
            // Arrange.
            var mpanContainer = CreateDefaultMpanContainer();

            // Act.
            Assert.IsFalse(mpanContainer.IsMosState);
            mpanContainer.SetMosState(true);

            // Assert.
            Assert.IsTrue(mpanContainer.IsMosState);
        }

        [Test]
        public void SetMosStateReported_WasntInMOSState_ThrowsException()
        {
            // Arrange.
            var mpanContainer = CreateDefaultMpanContainer();

            // Act.
            Assert.That(() => mpanContainer.SetMosStateReported(), Throws.TypeOf<InvalidOperationException>());

            // Assert.
        }

        [Test]
        public void SetMosStateReported_WasInMOSState_BecomesInMosReportedState()
        {
            // Arrange.
            var mpanContainer = CreateDefaultMpanContainer();

            // Act.
            mpanContainer.SetMosState(true);
            Assert.IsFalse(mpanContainer.IsMosReportedState);
            mpanContainer.SetMosStateReported();

            // Assert.
            Assert.IsTrue(mpanContainer.IsMosReportedState);
        }

        [Test]
        public void DestNodesEquals_DestNodesIsTheSame_ReturnsTrueOtherwiseFalse()
        {
            // Arrange.
            var mpanContainer = CreateDefaultMpanContainer();

            // Act.
            // Assert.
            Assert.IsFalse(mpanContainer.DestNodesEquals(new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 }.Select(x => new NodeTag(x)).ToArray()));
            Assert.IsFalse(mpanContainer.DestNodesEquals(new byte[] { 0x01, 0x02, 0x03 }.Select(x => new NodeTag(x)).ToArray()));
            Assert.IsTrue(mpanContainer.DestNodesEquals(new byte[] { 0x03, 0xca, 0xfe }.Select(x => new NodeTag(x)).ToArray()));
            Assert.IsTrue(mpanContainer.DestNodesEquals(new byte[] { 0x03, 0xfe, 0xca }.Select(x => new NodeTag(x)).ToArray()));
        }

        [Test]
        public void IncrementMpanState_InitializedWithFakeMpanState_IncrementsMpanState()
        {
            // Arrange.
            var mpanContainer = new MpanContainer(_nodeGroupId, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 255 },
                _seqNo, _rgh);

            // Act.
            mpanContainer.IncrementMpanState();

            // Assert.
            Assert.IsTrue(mpanContainer.MpanState.SequenceEqual(
                new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 })
                );
        }

        [Test]
        public void DecrementMpanState_InitializedWithFakeMpanState_DecrementsMpanState()
        {
            // Arrange.
            var mpanContainer = new MpanContainer(_nodeGroupId, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0 },
                _seqNo, _rgh);

            // Act.
            mpanContainer.DecrementMpanState();

            // Assert.
            Assert.IsTrue(mpanContainer.MpanState.SequenceEqual(
                new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 255 })
                );
        }
    }
}
