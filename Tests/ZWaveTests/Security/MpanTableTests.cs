using System;
using System.Linq;
using NUnit.Framework;
using ZWave.Devices;
using ZWave.Security;

namespace ZWaveTests.Security
{
    [TestFixture]
    public class MpanTableTests
    {
        private readonly byte[] _mpanState = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        private readonly byte _seqNo = 211;
        private static NodeGroupId _nodeGroupId = new NodeGroupId(new NodeTag(2), 7);
        private NodeTag[] _rgh = new byte[] { 0x03, 0xca, 0xfe }.Select(x => new NodeTag(x)).ToArray();

        [Test]
        public void GetContainerByGroupIdIndex_PassValidGroupId_GetsValidContainer()
        {
            // Arrange.
            var mpanTable = new MpanTable();
            mpanTable.AddOrReplace(_nodeGroupId, _seqNo, _rgh, _mpanState);

            // Act.
            var mpanContainer = mpanTable.GetContainer(_nodeGroupId);

            // Assert.
            Assert.IsNotNull(mpanContainer);
        }

        [Test]
        [Ignore("MPAN")]
        public void GetContainerByGroupIdIndex_PassInvalidGroupId_ThrowsException()
        {
            // Arrange.
            var mpanTable = new MpanTable();
            mpanTable.AddOrReplace(_nodeGroupId, _seqNo, _rgh, _mpanState);
            var invalidGroupId = new NodeGroupId(_nodeGroupId.Node, (byte)(_nodeGroupId.GroupId - 1));

            // Act.
            MpanContainer mpanContainer;
            Assert.That(() => mpanContainer = mpanTable.GetContainer(invalidGroupId), Throws.TypeOf<IndexOutOfRangeException>());
            // Assert.
        }

        [Test]
        public void AddOrReplace_PassValidParameters_AddsMpanToTable()
        {
            // Arrange.
            var mpanTable = new MpanTable();

            // Act.
            var mpanContainer = mpanTable.AddOrReplace(_nodeGroupId, _seqNo, _rgh, _mpanState);

            // Assert.
            Assert.IsTrue(mpanTable.CheckMpanExists(_nodeGroupId));
            Assert.IsTrue(mpanContainer.MpanState.Length == 16);
            Assert.AreEqual(_nodeGroupId.GroupId, mpanContainer.NodeGroupId.GroupId);
            Assert.AreEqual(_seqNo, mpanContainer.SequenceNumber);
            Assert.AreEqual(_nodeGroupId.Node, mpanContainer.NodeGroupId.Node);
            Assert.IsTrue(_rgh.SequenceEqual(mpanContainer.ReceiverGroupHandle));
        }

        [Test]
        [Ignore("MPAN change")]
        public void Add_AddMoreThanMaxNonceTableCapacity_FirstAddedNonceRemovedFromTable()
        {
            // Arrange.
            var mpanTable = new MpanTable();

            // Act.
            for (int i = 0; i < MpanTable.MAX_RECORDS_COUNT + 1; i++)
            {
                var mpanContainer = mpanTable.AddOrReplace(new NodeGroupId(new NodeTag((byte)(_nodeGroupId.Node.Id + i)), (byte)(_nodeGroupId.GroupId + i)), _seqNo, _rgh, _mpanState);
            }

            // Assert.
            Assert.IsFalse(mpanTable.CheckMpanExists(_nodeGroupId));
        }

        [Test]
        public void AddOrReplace_Replace_AddsNewMpanToTable()
        {
            // Arrange.
            var mpanTable = new MpanTable();

            // Act.
            var mpanContainer = mpanTable.AddOrReplace(_nodeGroupId, _seqNo, _rgh, _mpanState);
            mpanContainer.IncrementMpanState();

            // Assert.
            Assert.IsTrue(mpanTable.CheckMpanExists(_nodeGroupId));
        }

        [Test]
        public void NextMpan_HasRecordInTable_IncrementsMpanStateAndGeneratesNexMpanValue()
        {
            // Arrange.
            var mpanTable = new MpanTable();

            // Act.
            var mpanContainer = mpanTable.AddOrReplace(_nodeGroupId, _seqNo, _rgh, _mpanState);

            var stateBigInt = new Utils.BigInteger(mpanTable.GetContainer(_nodeGroupId)?.MpanState);
            stateBigInt.Increment();

            mpanContainer.IncrementMpanState();

            // Assert.
            Assert.IsTrue(mpanContainer.MpanState.SequenceEqual(stateBigInt.GetBytes()));
        }

        [Test]
        public void PreviousMpan_HasRecordInTable_DecrementsMpanStateAndGeneratesPreviousMpanValue()
        {
            // Arrange.
            var mpanTable = new MpanTable();

            // Act.
            var mpanContainer = mpanTable.AddOrReplace(_nodeGroupId, _seqNo, _rgh, _mpanState);

            var stateBigInt = new Utils.BigInteger(mpanTable.GetContainer(_nodeGroupId)?.MpanState);
            stateBigInt.Decrement();

            mpanContainer.DecrementMpanState();

            // Assert.
            Assert.IsTrue(mpanContainer.MpanState.SequenceEqual(stateBigInt.GetBytes()));
        }

        [Test]
        public void CallNextThanPreviousMpan_HasRecordInTable_GetOriginalMpanState()
        {
            // Arrange.
            var mpanTable = new MpanTable();

            // Act.
            var mpanContainer = mpanTable.AddOrReplace(_nodeGroupId, _seqNo, _rgh, _mpanState);

            var stateBigInt = new Utils.BigInteger(mpanTable.GetContainer(_nodeGroupId)?.MpanState);

            for (int i = 0; i < 123; i++)
            {
                mpanContainer.IncrementMpanState();
            }

            for (int i = 0; i < 123; i++)
            {
                mpanContainer.DecrementMpanState();
            }

            // Assert.
            Assert.IsTrue(stateBigInt.GetBytes().SequenceEqual(mpanTable.GetContainer(_nodeGroupId)?.MpanState));
        }

        [Test]
        public void RemoveRecord_HasRecordInTable_RemovesRecordFromTable()
        {
            // Arrange.
            var mpanTable = new MpanTable();

            // Act.
            mpanTable.AddOrReplace(_nodeGroupId, _seqNo, _rgh, _mpanState);
            Assert.IsTrue(mpanTable.CheckMpanExists(_nodeGroupId));
            mpanTable.RemoveRecord(_nodeGroupId);

            // Assert.
            Assert.IsFalse(mpanTable.CheckMpanExists(_nodeGroupId));
        }

        [Test]
        public void ClearMpanTable_HasRecordsInTable_ClearsTable()
        {
            // Arrange.
            var mpanTable = new MpanTable();

            // Act.
            for (byte i = 0; i < MpanTable.MAX_RECORDS_COUNT; i++)
            {
                mpanTable.AddOrReplace(new NodeGroupId(_nodeGroupId.Node, (byte)(_nodeGroupId.GroupId + i)), (byte)(_seqNo + i), _rgh, _mpanState);
            }

            for (byte i = 0; i < MpanTable.MAX_RECORDS_COUNT; i++)
            {
                Assert.IsTrue(mpanTable.CheckMpanExists(new NodeGroupId(_nodeGroupId.Node, (byte)(_nodeGroupId.GroupId + i))));
            }

            mpanTable.ClearMpanTable();

            // Assert.
            for (byte i = 0; i < MpanTable.MAX_RECORDS_COUNT; i++)
            {
                Assert.IsFalse(mpanTable.CheckMpanExists(new NodeGroupId(_nodeGroupId.Node, (byte)(_nodeGroupId.GroupId + i))));
            }
        }


        [Test]
        public void IsRecordInMOSState_RecordsHasMOSState_ReturnsTrue()
        {
            // Arrange.
            var mpanTable = new MpanTable();

            // Act.
            var mpanContainer = mpanTable.AddOrReplace(_nodeGroupId, _seqNo, _rgh, _mpanState);
            Assert.IsFalse(mpanTable.IsRecordInMOSState(_nodeGroupId));
            mpanContainer.SetMosState(true);

            // Assert.
            Assert.IsTrue(mpanTable.IsRecordInMOSState(_nodeGroupId));
        }

        [Test]
        public void FindGroup_ContainsTheSameDestModes_ReturnsGroupIdOtherwiseZero()
        {
            // Arrange.
            var mpanTable = new MpanTable();
            mpanTable.AddOrReplace(_nodeGroupId, _seqNo, _rgh, _mpanState);

            // Act.
            // Assert.
            byte groupId = mpanTable.FindGroup(new byte[] { 0x03, 0xca, 0xfe }.Select(x => new NodeTag(x)).ToArray());
            Assert.AreEqual(_nodeGroupId.GroupId, groupId);
            groupId = mpanTable.FindGroup(new byte[] { 0x03, 0xfe, 0xca }.Select(x => new NodeTag(x)).ToArray());
            Assert.AreEqual(_nodeGroupId.GroupId, groupId);
            groupId = mpanTable.FindGroup(new byte[] { 0x03, 0x02, 0x01 }.Select(x => new NodeTag(x)).ToArray());
            Assert.AreEqual(0, groupId);
            groupId = mpanTable.FindGroup(new byte[] { 0x03, 0x02, 0x01, 0x00 }.Select(x => new NodeTag(x)).ToArray());
            Assert.AreEqual(0, groupId);
        }

        [Test]
        public void SelectGroupIds_ContainsSomeGroupIdsForOneOwnerId_ReturnsAllGroupIds()
        {
            // Arrange.
            byte grId1 = 0x01;
            byte grId2 = 0x05;
            byte grId3 = 0x07;
            byte grId4 = 0x10;
            NodeTag ownerNodeIdNone = new NodeTag((byte)(_nodeGroupId.Node.Id + 1));
            NodeTag otherOwnerNodeId = new NodeTag((byte)(_nodeGroupId.Node.Id + 2));
            var mpanTable = new MpanTable();
            mpanTable.AddOrReplace(new NodeGroupId(_nodeGroupId.Node, grId1), _seqNo, _rgh, _mpanState);
            mpanTable.AddOrReplace(new NodeGroupId(_nodeGroupId.Node, grId2), _seqNo, _rgh, _mpanState);
            mpanTable.AddOrReplace(new NodeGroupId(_nodeGroupId.Node, grId3), _seqNo, _rgh, _mpanState);
            mpanTable.AddOrReplace(new NodeGroupId(otherOwnerNodeId, grId4), _seqNo, _rgh, _mpanState);

            // Act.
            var groupIds = mpanTable.SelectGroupIds(_nodeGroupId.Node);
            var groupIdsEmpty = mpanTable.SelectGroupIds(ownerNodeIdNone);

            // Assert.
            Assert.IsTrue(groupIds.Length == 3);
            Assert.IsTrue(groupIds.Any(x => x == grId1));
            Assert.IsTrue(groupIds.Any(x => x == grId2));
            Assert.IsTrue(groupIds.Any(x => x == grId3));
            Assert.AreEqual(0, groupIdsEmpty.Length);
        }
    }
}
