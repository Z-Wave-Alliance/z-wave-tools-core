/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Linq;
using NUnit.Framework;
using ZWave.Security;
using Utils;
using ZWave.Devices;

namespace ZWaveTests.Security
{
    //[TestFixture]
    public class SpanTableTests
    {
        private byte[] _fakeSpanNonce = new byte[] { 0x6d, 0xde, 0x2c, 0xdb, 0x6c, 0xb2, 0x27, 0x4b, 0x8b, 0xb4, 0xff, 0xeb, 0x3f, 0x1e, 0xb3, 0x94 };
        private byte[] _fakeReceiverNonce = new byte[] { 0x14, 0xa8, 0x26, 0x24, 0x15, 0x21, 0x3e, 0xfc, 0x65, 0xea, 0x50, 0xf2, 0x0a, 0x2f, 0x70, 0x68 };
        private readonly byte[] _fakePersonalization = new byte[] { 0xce, 0xbb,  0x8c, 0xce, 0xb3, 0x07, 0x14, 0x7e, 0xa6, 0x4b, 0x57, 0xc0, 0x9f, 0x11, 0xea, 0x9d,
                                                                    0x40, 0x43, 0x8b, 0xeb, 0x49, 0xc6, 0x97, 0xb5, 0x3b, 0xe4, 0xae, 0xce, 0x69, 0x9e, 0x99, 0x3e };
        byte[] senderNonce = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        byte[] receiverNonce = new byte[] { 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        private byte _nodeId;
        private const byte SEQUENCE_NUMBER = 1;

        [Test]
        public void Add_AddMoreThanMaxNonceTableCapacity_FirstAddedNonceRemovedFromTable()
        {
            // Arrange.
            var spanTable = new SpanTable();

            // Act.
            for (int i = 1; i <= SpanTable.MAX_RECORDS_COUNT + 1; i++)
            {
                spanTable.Add(new InvariantPeerNodeId(NodeTag.Empty, new NodeTag((ushort)i)), receiverNonce, SEQUENCE_NUMBER, SEQUENCE_NUMBER);
            }

            // Assert.
            Assert.IsFalse(spanTable.CheckNonceExists(new InvariantPeerNodeId(NodeTag.Empty, new NodeTag(1))));
        }

        [Test]
        public void Add_GeneratesReceiverNonce_AddsContainerInTableWithReceiverType()
        {
            // Arrange.
            var spanTable = new SpanTable();
            _nodeId = 3;

            // Act.
            spanTable.Add(new InvariantPeerNodeId(NodeTag.Empty, new NodeTag(_nodeId)), receiverNonce, SEQUENCE_NUMBER, SEQUENCE_NUMBER);

            // Assert.

            Assert.IsTrue(spanTable.CheckNonceExists(new InvariantPeerNodeId(NodeTag.Empty, new NodeTag(_nodeId))));
            var addedNonceContainer = spanTable.GetContainer(new InvariantPeerNodeId(NodeTag.Empty, new NodeTag(_nodeId)));
            Assert.IsTrue(receiverNonce.SequenceEqual(addedNonceContainer.ReceiversNonce));
        }

        [Test]
        public void Add_WithSpecifiedReceiverNonce_AddsContainerInTableWithReceiverType()
        {
            // Arrange.
            var spanTable = new SpanTable();
            _nodeId = 2;
            var peerId = new InvariantPeerNodeId(NodeTag.Empty, new NodeTag(_nodeId));
            // Act.
            spanTable.Add(peerId, _fakeReceiverNonce, SEQUENCE_NUMBER, SEQUENCE_NUMBER);

            // Assert.
            Assert.IsTrue(spanTable.CheckNonceExists(peerId));
            var addedNonceContainer = spanTable.GetContainer(peerId);
            Assert.IsTrue(_fakeReceiverNonce.SequenceEqual(addedNonceContainer.ReceiversNonce));
        }

        [Test]
        public void InstantiateNonce_GeneratesSenderNonce_AddsContainerInTableWithSpanType()
        {
            // Arrange.

            var spanTable = new SpanTable();

            _nodeId = 4;
            var peerId = new InvariantPeerNodeId(NodeTag.Empty, new NodeTag(_nodeId));

            // Act.
            // At sender side.
            spanTable.Add(peerId, _fakeReceiverNonce, SEQUENCE_NUMBER, SEQUENCE_NUMBER);
            var addedNonceContainer = spanTable.GetContainer(peerId);
            addedNonceContainer.InstantiateWithSenderNonce(senderNonce, _fakePersonalization);

            // Assert.
            Assert.AreEqual(16, senderNonce.Length);
            Assert.IsTrue(spanTable.CheckNonceExists(peerId));

            Assert.IsTrue(senderNonce.SequenceEqual(addedNonceContainer.Span));
        }

        [Test]
        public void InstantiateWithSenderNonce_WithSpecifiedSenderNonce_AddsContainerInTableWithSpanType()
        {
            // Arrange.
            var spanTable = new SpanTable();
            _nodeId = 11;
            var peerId = new InvariantPeerNodeId(NodeTag.Empty, new NodeTag(_nodeId));

            // Act.
            // At receiver side.
            spanTable.Add(peerId, receiverNonce, SEQUENCE_NUMBER, SEQUENCE_NUMBER);
            var spanContainer = spanTable.GetContainer(peerId);
            spanContainer.InstantiateWithSenderNonce(_fakeSpanNonce, _fakePersonalization);

            // Assert.
            Assert.IsTrue(spanTable.CheckNonceExists(peerId));
            var addedNonceContainer = spanTable.GetContainer(peerId);
            Assert.IsTrue(_fakeSpanNonce.SequenceEqual(addedNonceContainer.Span));
        }

        [Test]
        public void NextNonce_NonceExistsInTable_GeneratesDataForNonceContainerSpan()
        {
            var nextNonceData = "B5 38 6A AA 0D 61 86 F7 2F F9 F4 13 40 D7 3D 22".GetBytes();

            var spanTable = new SpanTable();

            // Arrange.
            _nodeId = 9;
            var peerId = new InvariantPeerNodeId(NodeTag.Empty, new NodeTag(_nodeId));

            // Act.
            spanTable.Add(peerId, _fakeReceiverNonce, SEQUENCE_NUMBER, SEQUENCE_NUMBER);
            var spanContainer = spanTable.GetContainer(peerId);
            spanContainer.InstantiateWithSenderNonce(senderNonce, _fakePersonalization);
            spanContainer.NextNonce();

            // Assert.
            Assert.IsTrue(nextNonceData.SequenceEqual(spanContainer.Span));
        }

        [Test]
        public void SetNonceFree_NonceExistsInTable_RemovesNonceFromTable()
        {
            // Arrange.
            var spanTable = new SpanTable();
            _nodeId = 5;
            var peerId = new InvariantPeerNodeId(NodeTag.Empty, new NodeTag(_nodeId));

            // Act.
            spanTable.Add(peerId, receiverNonce, SEQUENCE_NUMBER, SEQUENCE_NUMBER);
            spanTable.SetNonceFree(peerId);

            // Assert.
            Assert.IsFalse(spanTable.CheckNonceExists(peerId));
        }

        [Test]
        public void ClearNonceTable_NonceExistsInTable_RemovesAllNoncesFromTable()
        {
            // Arrange.
            var spanTable = new SpanTable();

            // Act.
            for (int i = 1; i <= SpanTable.MAX_RECORDS_COUNT; i++)
            {
                spanTable.Add(new InvariantPeerNodeId(new NodeTag((ushort)i), NodeTag.Empty), receiverNonce, SEQUENCE_NUMBER, SEQUENCE_NUMBER);
            }
            spanTable.ClearNonceTable();

            // Assert.
            for (int i = 1; i <= SpanTable.MAX_RECORDS_COUNT; i++)
            {
                Assert.IsFalse(spanTable.CheckNonceExists(new InvariantPeerNodeId(NodeTag.Empty, new NodeTag((ushort)i))));
            }
        }
    }
}
