/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;
using ZWave.BasicApplication.Security;
using ZWave.CommandClasses;
using ZWave.Enums;
using ZWave.Security;
using ZWave.Configuration;
using ZWave;
using ZWave.Devices;

namespace BasicApplicationTests.Security
{
    [TestFixture]
    public class ExtensionsTests
    {
        private static NodeTag SENDER_ID = new NodeTag(0x01);
        private static NodeTag RECEIVER_ID = new NodeTag(0x02);

        private SpanTable _spanTable;
        private Dictionary<InvariantPeerNodeId, SinglecastKey> _scKeys;
        private SecurityS2CryptoProvider _securityS2CryptoProvider;
        private SecurityManagerInfo _securityManagerInfo;
        private NetworkViewPoint _networkViewPoint;
        private byte[] HOME_ID = new byte[] { 0xDE, 0xAD, 0xBE, 0xEF };
        private readonly byte[] RECEIVER_EI = new byte[] { 0x14, 0xa8, 0x26, 0x24, 0x15, 0x21, 0x3e, 0xfc, 0x65, 0xea, 0x50, 0xf2, 0x0a, 0x2f, 0x70, 0x68 };
        private byte[] SENDER_EI = new byte[] { 0x6d, 0xde, 0x2c, 0xdb, 0x6c, 0xb2, 0x27, 0x4b, 0x8b, 0xb4, 0xff, 0xeb, 0x3f, 0x1e, 0xb3, 0x94 };
        private readonly byte[] MPAN_STATE = new byte[] { 0xad, 0xde, 0x21, 0x7e, 0x6c, 0xb2, 0x27, 0x4b, 0x01, 0xb4, 0xff, 0x1b, 0x3f, 0xbe, 0xb3, 0x93 };
        private readonly byte[] NETWORK_KEY_S2_C0 = new byte[] { 0xc0, 0x07, 0x66, 0xd0, 0xa1, 0x66, 0x65, 0x1f, 0x64, 0x67, 0xe2, 0x01, 0xce, 0xa8, 0x07, 0x5b };
        private readonly InvariantPeerNodeId PEER_NODE_ID = new InvariantPeerNodeId(SENDER_ID, RECEIVER_ID);
        private readonly byte[] _plainData = new byte[] { 0xFF, 0xFF, 0xFF };

        private void SetupSpanRecord()
        {
            _spanTable.AddOrReplace(PEER_NODE_ID, RECEIVER_EI,
                _spanTable.GetTxSequenceNumber(PEER_NODE_ID),
                _spanTable.GetTxSequenceNumber(PEER_NODE_ID));
            var spanContainer = _spanTable.GetContainer(PEER_NODE_ID);
            spanContainer.InstantiateWithSenderNonce(SENDER_EI, _scKeys[PEER_NODE_ID].Personalization);
        }

        [SetUp]
        public void Setup()
        {
            _networkViewPoint = new NetworkViewPoint(null)
            {
                NodeTag = SENDER_ID,
                HomeId = HOME_ID
            };
            _securityManagerInfo = new SecurityManagerInfo(_networkViewPoint, new NetworkKey[8], new byte[]
                {
                    0x77, 0x07, 0x6d, 0x0a, 0x73, 0x18, 0xa5, 0x7d, 0x3c, 0x16, 0xc1, 0x72, 0x51, 0xb2, 0x66, 0x45,
                    0xdf, 0x4c, 0x2f, 0x87, 0xeb, 0xc0, 0x99, 0x2a, 0xb1, 0x77, 0xfb, 0xa5, 0x1d, 0xb9, 0x2c, 0x2a
                });
            _securityManagerInfo.SetNetworkKey(NETWORK_KEY_S2_C0, SecuritySchemes.S2_UNAUTHENTICATED, false);
            _spanTable = new SpanTable();
            _scKeys = new Dictionary<InvariantPeerNodeId, SinglecastKey>();
            _securityS2CryptoProvider = new SecurityS2CryptoProvider(_securityManagerInfo);
            var mpanKey = new byte[SecurityS2Utils.KEY_SIZE];
            var ccmKey = new byte[SecurityS2Utils.KEY_SIZE];
            var personalization = new byte[SecurityS2Utils.PERSONALIZATION_SIZE];
            SecurityS2Utils.NetworkKeyExpand(NETWORK_KEY_S2_C0, ccmKey, personalization, mpanKey);
            _scKeys.Add(PEER_NODE_ID, new SinglecastKey(NETWORK_KEY_S2_C0[0], ccmKey, personalization, SecuritySchemes.S2_UNAUTHENTICATED));
            //_securityManagerInfo.ActivateNetworkKeyS2ForNode(PEER_NODE_ID, SecuritySchemes.S2_UNAUTHENTICATED);
        }

        [TestCase(ExtensionTypes.Mos)]
        [TestCase(ExtensionTypes.Mpan)]
        [TestCase(ExtensionTypes.MpanGrp)]
        [TestCase(ExtensionTypes.Span)]
        public void SinglecastWithoutExtensions_DeleteExtension_NoChangesToFrame(ExtensionTypes extensionType)
        {
            // Arrange.
            SetupSpanRecord();
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, extensionType, ExtensionAppliedActions.Delete));

            // Act.
            var secureS2Frame = (COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION)_securityS2CryptoProvider.EncryptSinglecastCommand(
               _scKeys[new InvariantPeerNodeId(_securityManagerInfo.Network.NodeTag, RECEIVER_ID)],
               _spanTable, _securityManagerInfo.Network.NodeTag, RECEIVER_ID, _securityManagerInfo.Network.HomeId, _plainData, null, new SubstituteSettings());

            // Assert.
            Assert.AreEqual(0, secureS2Frame.properties1.extension);
        }

        [Test]
        public void SinglecastWithSpanExtension_DeleteExtension_DeletesSpanExtensionFromFrame()
        {
            // Arrange.
            _spanTable.AddOrReplace(PEER_NODE_ID, RECEIVER_EI,
                _spanTable.GetTxSequenceNumber(PEER_NODE_ID),
                _spanTable.GetTxSequenceNumber(PEER_NODE_ID));

            // Act.
            var secureS2Frame = (COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION)_securityS2CryptoProvider.EncryptSinglecastCommand(
                _scKeys[new InvariantPeerNodeId(_securityManagerInfo.Network.NodeTag, RECEIVER_ID)],
                _spanTable, _securityManagerInfo.Network.NodeTag, RECEIVER_ID, _securityManagerInfo.Network.HomeId, _plainData, null, new SubstituteSettings());
            Assert.AreEqual(1, secureS2Frame.properties1.extension);
            Assert.AreEqual((byte)ExtensionTypes.Span, secureS2Frame.vg1[0].properties1.type);

            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.Span, ExtensionAppliedActions.Delete));
            secureS2Frame = _securityS2CryptoProvider.EncryptSinglecastCommand(
                _scKeys[new InvariantPeerNodeId(_securityManagerInfo.Network.NodeTag, RECEIVER_ID)],
                _spanTable, _securityManagerInfo.Network.NodeTag, RECEIVER_ID, _securityManagerInfo.Network.HomeId, _plainData, null, new SubstituteSettings());

            // Assert.
            Assert.AreEqual(0, secureS2Frame.properties1.extension);
        }

        [TestCase(ExtensionTypes.Mos)]
        [TestCase(ExtensionTypes.Mpan)]
        [TestCase(ExtensionTypes.MpanGrp)]
        [TestCase(ExtensionTypes.Span)]
        public void Delete_ExtensionExists_DeletesExtensionFromFrame(ExtensionTypes extensionType)
        {
            // Arrange.
            Extensions extensions = null;
            AddExtensionByType(extensionType, ref extensions);

            // Act.
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, extensionType, ExtensionAppliedActions.Delete));
            _securityS2CryptoProvider.ApplyTestExtensionsSettings(new List<MessageTypes>(new MessageTypes[] { MessageTypes.SinglecastAll }), ref extensions);

            // Assert.
            Assert.AreEqual(0, extensionType == ExtensionTypes.Mpan ?
                extensions.EncryptedExtensionsList.Count :
                extensions.ExtensionsList.Count);
        }

        [Test]
        public void Delete_MoreThanOneExtensionExists_MoreToFollowInPreviousExtensionChanges()
        {
            // Arrange.
            Extensions extensions = null;
            AddExtensionByType(ExtensionTypes.MpanGrp, ref extensions, 0x03);
            AddExtensionByType(ExtensionTypes.Span, ref extensions);

            // Act.
            Assert.AreEqual(1, extensions.ExtensionsList[0].properties1.moreToFollow);
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.Span, ExtensionAppliedActions.Delete));
            _securityS2CryptoProvider.ApplyTestExtensionsSettings(new List<MessageTypes>(new MessageTypes[] { MessageTypes.SinglecastAll }), ref extensions);

            // Assert.
            Assert.AreEqual(1, extensions.ExtensionsList.Count);
            Assert.AreEqual(0, extensions.ExtensionsList[0].properties1.moreToFollow);
        }

        [Test]
        public void Delete_DeleteExtensionSpecifyMessageTypeAndNumOfUsage_DeleteTestExtensionOnlyOnce()
        {
            // Arrange.
            Extensions extensions = null;
            AddExtensionByType(ExtensionTypes.Span, ref extensions);

            // Act.
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastWithSpan, ExtensionTypes.Span, ExtensionAppliedActions.Delete)
            {
                NumOfUsage = 1,
                NumOfUsageSpecified = true
            });
            _securityS2CryptoProvider.ApplyTestExtensionsSettings(new List<MessageTypes>(
                new MessageTypes[] { MessageTypes.SinglecastWithSpan }), ref extensions);

            // Assert.
            Assert.AreEqual(0, extensions.ExtensionsList.Count);

            extensions = null;
            AddExtensionByType(ExtensionTypes.Span, ref extensions);
            _securityS2CryptoProvider.ApplyTestExtensionsSettings(new List<MessageTypes>(
                new MessageTypes[] { MessageTypes.SinglecastWithSpan }), ref extensions);
            Assert.AreEqual(1, extensions.ExtensionsList.Count);
            var tvg1 = extensions.ExtensionsList[0];
            Assert.AreEqual((byte)ExtensionTypes.Span, tvg1.properties1.type);
            Assert.AreEqual(1, tvg1.properties1.critical);
            Assert.AreEqual(18, tvg1.extensionLength.Value);
        }

        [TestCase(ExtensionTypes.Mos)]
        [TestCase(ExtensionTypes.Mpan)]
        [TestCase(ExtensionTypes.MpanGrp)]
        [TestCase(ExtensionTypes.Span)]
        public void ModifyIfExists_ExtensionDoesntExist_NoChangesToFrame(ExtensionTypes extensionType)
        {
            // Arrange.
            Extensions extensions = null;
            AddExtensionByType(extensionType, ref extensions);

            // Act.
            byte val = (byte)extensionType;
            if (val == (byte)ExtensionTypes.Mos)
            {
                val = 0;
            }
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, (ExtensionTypes)(val + 1), ExtensionAppliedActions.ModifyIfExists));
            _securityS2CryptoProvider.ApplyTestExtensionsSettings(new List<MessageTypes>(new MessageTypes[] { MessageTypes.SinglecastAll }), ref extensions);

            // Assert.
            Assert.AreEqual(1, extensionType == ExtensionTypes.Mpan ?
                extensions.EncryptedExtensionsList.Count :
                extensions.ExtensionsList.Count);
        }

        [Test]
        public void ModifyIfExists_SpanExtensionExists_ModifiesSpanExtension()
        {
            // Arrange.
            var extValue = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            Extensions extensions = null;
            AddExtensionByType(ExtensionTypes.Span, ref extensions);

            // Act.
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.Span, ExtensionAppliedActions.ModifyIfExists)
            {
                Value = extValue,
                ValueSpecified = true,
                IsCritical = false,
                IsCriticalSpecified = true,
                IsMoreToFollow = true,
                IsMoreToFollowSpecified = true,
                ExtensionLength = 0x05,
                ExtensionLengthSpecified = true
            });
            _securityS2CryptoProvider.ApplyTestExtensionsSettings(new List<MessageTypes>(new MessageTypes[] { MessageTypes.SinglecastAll }), ref extensions);

            // Assert.
            Assert.AreEqual(1, extensions.ExtensionsList.Count);
            var tvg1 = extensions.ExtensionsList[0];
            Assert.IsTrue(extValue.SequenceEqual(tvg1.extension));
            Assert.AreEqual(0, tvg1.properties1.critical);
            Assert.AreEqual(1, tvg1.properties1.moreToFollow);
            Assert.AreEqual(0x05, tvg1.extensionLength.Value);
        }

        [Test]
        public void ModifyIfExists_MpanExtensionExists_ModifiesMpanExtension()
        {
            // Arrange.
            var extValue = new byte[] { 0xAA, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            Extensions extensions = null;
            AddExtensionByType(ExtensionTypes.Mpan, ref extensions);

            // Act.
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.Mpan, ExtensionAppliedActions.ModifyIfExists)
            {
                Value = extValue,
                ValueSpecified = true,
                IsCritical = false,
                IsCriticalSpecified = true,
                IsMoreToFollow = true,
                IsMoreToFollowSpecified = true,
                ExtensionLength = 0x05,
                ExtensionLengthSpecified = true
            });
            _securityS2CryptoProvider.ApplyTestExtensionsSettings(new List<MessageTypes>(new MessageTypes[] { MessageTypes.SinglecastAll }), ref extensions);

            // Assert.
            Assert.AreEqual(1, extensions.EncryptedExtensionsList.Count);
            var tvg1 = extensions.EncryptedExtensionsList[0];
            Assert.IsTrue(extValue.SequenceEqual(tvg1.extension));
            Assert.AreEqual(0, tvg1.properties1.critical);
            Assert.AreEqual(1, tvg1.properties1.moreToFollow);
            Assert.AreEqual(0x05, tvg1.extensionLength.Value);
        }

        [Test]
        public void ModifyIfExists_MpanGrpExtensionExists_ModifiesMpanGrpExtension()
        {
            // Arrange.
            var extValue = new byte[] { 0xAA };
            Extensions extensions = null;
            AddExtensionByType(ExtensionTypes.MpanGrp, ref extensions);

            // Act.
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.MpanGrp, ExtensionAppliedActions.ModifyIfExists)
            {
                Value = extValue,
                ValueSpecified = true,
                IsCritical = false,
                IsCriticalSpecified = true,
                IsMoreToFollow = true,
                IsMoreToFollowSpecified = true,
                ExtensionLength = 0x07,
                ExtensionLengthSpecified = true
            });
            _securityS2CryptoProvider.ApplyTestExtensionsSettings(new List<MessageTypes>(new MessageTypes[] { MessageTypes.SinglecastAll }), ref extensions);

            // Assert.
            Assert.AreEqual(1, extensions.ExtensionsList.Count);
            var tvg1 = extensions.ExtensionsList[0];
            Assert.IsTrue(extValue.SequenceEqual(tvg1.extension));
            Assert.AreEqual(0, tvg1.properties1.critical);
            Assert.AreEqual(1, tvg1.properties1.moreToFollow);
            Assert.AreEqual(0x07, tvg1.extensionLength.Value);
        }

        [Test]
        public void ModifyIfExists_MosExtensionExists_ModifiesMosExtension()
        {
            // Arrange.
            Extensions extensions = null;
            AddExtensionByType(ExtensionTypes.Mos, ref extensions);

            // Act.
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.Mos, ExtensionAppliedActions.ModifyIfExists)
            {
                IsCritical = true,
                IsCriticalSpecified = true,
                IsMoreToFollow = true,
                IsMoreToFollowSpecified = true,
                ExtensionLength = 0x03,
                ExtensionLengthSpecified = true
            });
            _securityS2CryptoProvider.ApplyTestExtensionsSettings(new List<MessageTypes>(new MessageTypes[] { MessageTypes.SinglecastAll }), ref extensions);

            // Assert.
            Assert.AreEqual(1, extensions.ExtensionsList.Count);
            var tvg1 = extensions.ExtensionsList[0];
            Assert.AreEqual(1, tvg1.properties1.critical);
            Assert.AreEqual(1, tvg1.properties1.moreToFollow);
            Assert.AreEqual(0x03, tvg1.extensionLength.Value);
        }

        [Test]
        public void AddOrModify_SpanExtensionExists_ModifiesSpanExtension()
        {
            // Arrange.
            var extValue = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            Extensions extensions = null;
            AddExtensionByType(ExtensionTypes.Span, ref extensions);

            // Act.
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.Span, ExtensionAppliedActions.AddOrModify)
            {
                Value = extValue,
                ValueSpecified = true,
                IsCritical = false,
                IsCriticalSpecified = true,
                IsMoreToFollow = true,
                IsMoreToFollowSpecified = true,
                ExtensionLength = 0x05,
                ExtensionLengthSpecified = true
            });
            _securityS2CryptoProvider.ApplyTestExtensionsSettings(new List<MessageTypes>(new MessageTypes[] { MessageTypes.SinglecastAll }), ref extensions);

            // Assert.
            Assert.AreEqual(1, extensions.ExtensionsList.Count);
            var tvg1 = extensions.ExtensionsList[0];
            Assert.IsTrue(extValue.SequenceEqual(tvg1.extension));
            Assert.AreEqual(0, tvg1.properties1.critical);
            Assert.AreEqual(1, tvg1.properties1.moreToFollow);
            Assert.AreEqual(0x05, tvg1.extensionLength.Value);
        }

        [Test]
        public void AddOrModify_MpanExtensionExists_ModifiesMpanExtension()
        {
            // Arrange.
            var extValue = new byte[] { 0xAA, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            Extensions extensions = null;
            AddExtensionByType(ExtensionTypes.Mpan, ref extensions);

            // Act.
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.Mpan, ExtensionAppliedActions.AddOrModify)
            {
                Value = extValue,
                ValueSpecified = true,
                IsCritical = false,
                IsCriticalSpecified = true,
                IsMoreToFollow = true,
                IsMoreToFollowSpecified = true,
                ExtensionLength = 0x05,
                ExtensionLengthSpecified = true
            });
            _securityS2CryptoProvider.ApplyTestExtensionsSettings(new List<MessageTypes>(new MessageTypes[] { MessageTypes.SinglecastAll }), ref extensions);

            // Assert.
            Assert.AreEqual(1, extensions.EncryptedExtensionsList.Count);
            var tvg1 = extensions.EncryptedExtensionsList[0];
            Assert.IsTrue(extValue.SequenceEqual(tvg1.extension));
            Assert.AreEqual(0, tvg1.properties1.critical);
            Assert.AreEqual(1, tvg1.properties1.moreToFollow);
            Assert.AreEqual(0x05, tvg1.extensionLength.Value);
        }

        [Test]
        public void AddOrModify_MpanGrpExtensionExists_ModifiesMpanGrpExtension()
        {
            // Arrange.
            var extValue = new byte[] { 0xAA };
            Extensions extensions = null;
            AddExtensionByType(ExtensionTypes.MpanGrp, ref extensions);

            // Act.
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.MpanGrp, ExtensionAppliedActions.AddOrModify)
            {
                Value = extValue,
                ValueSpecified = true,
                IsCritical = false,
                IsCriticalSpecified = true,
                IsMoreToFollow = true,
                IsMoreToFollowSpecified = true,
                ExtensionLength = 0x07,
                ExtensionLengthSpecified = true
            });
            _securityS2CryptoProvider.ApplyTestExtensionsSettings(new List<MessageTypes>(new MessageTypes[] { MessageTypes.SinglecastAll }), ref extensions);

            // Assert.
            Assert.AreEqual(1, extensions.ExtensionsList.Count);
            var tvg1 = extensions.ExtensionsList[0];
            Assert.IsTrue(extValue.SequenceEqual(tvg1.extension));
            Assert.AreEqual(0, tvg1.properties1.critical);
            Assert.AreEqual(1, tvg1.properties1.moreToFollow);
            Assert.AreEqual(0x07, tvg1.extensionLength.Value);
        }

        [Test]
        public void AddOrModify_MosExtensionExists_ModifiesMosExtension()
        {
            // Arrange.
            Extensions extensions = null;
            AddExtensionByType(ExtensionTypes.Mos, ref extensions);

            // Act.
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.Mos, ExtensionAppliedActions.AddOrModify)
            {
                IsCritical = true,
                IsCriticalSpecified = true,
                IsMoreToFollow = true,
                IsMoreToFollowSpecified = true,
                ExtensionLength = 0x03,
                ExtensionLengthSpecified = true
            });
            _securityS2CryptoProvider.ApplyTestExtensionsSettings(new List<MessageTypes>(new MessageTypes[] { MessageTypes.SinglecastAll }), ref extensions);

            // Assert.
            Assert.AreEqual(1, extensions.ExtensionsList.Count);
            var tvg1 = extensions.ExtensionsList[0];
            Assert.AreEqual(1, tvg1.properties1.critical);
            Assert.AreEqual(1, tvg1.properties1.moreToFollow);
            Assert.AreEqual(0x03, tvg1.extensionLength.Value);
        }

        [Test]
        public void AddOrModify_SpanExtensionDoesntExist_AddedSpanExtension()
        {
            // Arrange.
            Extensions extensions = null;

            // Act.
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.Span, ExtensionAppliedActions.AddOrModify)
            {
                Value = SENDER_EI,
                ValueSpecified = true
            });
            _securityS2CryptoProvider.ApplyTestExtensionsSettings(new List<MessageTypes>(new MessageTypes[] { MessageTypes.SinglecastAll }), ref extensions);

            // Assert.
            Assert.AreEqual(1, extensions.ExtensionsList.Count);
            var tvg1 = extensions.ExtensionsList[0];
            Assert.IsTrue(SENDER_EI.SequenceEqual(tvg1.extension));
            Assert.AreEqual(1, tvg1.properties1.critical);
            Assert.AreEqual(0, tvg1.properties1.moreToFollow);
            Assert.AreEqual(18, tvg1.extensionLength.Value);
        }

        [Test]
        public void AddOrModify_MpanExtensionDoesntExist_AddedMpanExtension()
        {
            // Arrange.
            var extValue = new byte[] { 0xAA, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            Extensions extensions = null;

            // Act.
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.Mpan, ExtensionAppliedActions.AddOrModify)
            {
                Value = extValue,
                ValueSpecified = true
            });
            _securityS2CryptoProvider.ApplyTestExtensionsSettings(new List<MessageTypes>(new MessageTypes[] { MessageTypes.SinglecastAll }), ref extensions);

            // Assert.
            Assert.AreEqual(1, extensions.EncryptedExtensionsList.Count);
            var tvg1 = extensions.EncryptedExtensionsList[0];
            Assert.IsTrue(extValue.SequenceEqual(tvg1.extension));
            Assert.AreEqual(1, tvg1.properties1.critical);
            Assert.AreEqual(0, tvg1.properties1.moreToFollow);
            Assert.AreEqual(19, tvg1.extensionLength.Value);
        }

        [Test]
        public void AddOrModify_MpanGrpDoesntExist_AddedMpanGrpExtension()
        {
            // Arrange.
            var extValue = new byte[] { 0xAA };
            Extensions extensions = null;

            // Act.
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.MpanGrp, ExtensionAppliedActions.AddOrModify)
            {
                Value = extValue,
                ValueSpecified = true
            });
            _securityS2CryptoProvider.ApplyTestExtensionsSettings(new List<MessageTypes>(new MessageTypes[] { MessageTypes.SinglecastAll }), ref extensions);

            // Assert.
            Assert.AreEqual(1, extensions.ExtensionsList.Count);
            var tvg1 = extensions.ExtensionsList[0];
            Assert.IsTrue(extValue.SequenceEqual(tvg1.extension));
            Assert.AreEqual(1, tvg1.properties1.critical);
            Assert.AreEqual(0, tvg1.properties1.moreToFollow);
            Assert.AreEqual(3, tvg1.extensionLength.Value);
        }

        [Test]
        public void AddOrModify_MosExtensionDoesntExist_AddedMosExtension()
        {
            // Arrange.
            Extensions extensions = null;

            // Act.
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.Mos, ExtensionAppliedActions.AddOrModify));
            _securityS2CryptoProvider.ApplyTestExtensionsSettings(new List<MessageTypes>(new MessageTypes[] { MessageTypes.SinglecastAll }), ref extensions);

            // Assert.
            Assert.AreEqual(1, extensions.ExtensionsList.Count);
            var tvg1 = extensions.ExtensionsList[0];
            Assert.AreEqual(0, tvg1.properties1.critical);
            Assert.AreEqual(0, tvg1.properties1.moreToFollow);
            Assert.AreEqual(2, tvg1.extensionLength.Value);
        }

        [Test]
        public void AddOrModify_AddTestSpanExtension_ModifiesOnlyNotTestSpanExtension()
        {
            // Arrange.
            Extensions extensions = null;
            AddExtensionByType(ExtensionTypes.Span, ref extensions);

            // Act.
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.Span, ExtensionAppliedActions.Add));
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.Span, ExtensionAppliedActions.AddOrModify)
            {
                IsCritical = false,
                IsCriticalSpecified = true,
            });
            _securityS2CryptoProvider.ApplyTestExtensionsSettings(new List<MessageTypes>(new MessageTypes[] { MessageTypes.SinglecastAll }), ref extensions);

            // Assert.
            Assert.AreEqual(2, extensions.ExtensionsList.Count);
            var tvg1 = extensions.ExtensionsList[0];
            Assert.AreEqual(0, tvg1.properties1.critical);
            Assert.AreEqual(1, tvg1.properties1.moreToFollow);
            var tvg2 = extensions.ExtensionsList[1];
            Assert.AreEqual(1, tvg2.properties1.critical);
            Assert.AreEqual(0, tvg2.properties1.moreToFollow);
        }

        [Test]
        public void AddOrModify_ChangeEncryptionExtensionState_ExtensionIsMovedToSpecifiedList()
        {
            // Arrange.
            Extensions extensions = null;
            AddExtensionByType(ExtensionTypes.Mpan, ref extensions, 0xFE);
            var newVal = new byte[] { 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            // Act.
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastWithMpan, ExtensionTypes.Mpan, ExtensionAppliedActions.AddOrModify)
            {
                IsEncrypted = false,
                IsEncryptedSpecified = true,
                Value = newVal,
                ValueSpecified = true
            });
            _securityS2CryptoProvider.ApplyTestExtensionsSettings(new List<MessageTypes>(new MessageTypes[] { MessageTypes.SinglecastWithMpan }), ref extensions);

            // Assert.
            Assert.AreEqual(0, extensions.EncryptedExtensionsList.Count);
            Assert.AreEqual(1, extensions.ExtensionsList.Count);
            var tvg1 = extensions.ExtensionsList[0];
            Assert.IsTrue(newVal.SequenceEqual(tvg1.extension));
        }

        [Test]
        public void AddOrModify_AddUnencrypedExtAlreadyContainsEncryptedExt_AddedUnencryptedTestExtension()
        {
            // Arrange.
            var extValue = new byte[] { 0xAA, 0xFF, 0xFF, 0xFF, 0xFF };
            Extensions extensions = null;
            AddExtensionByType(ExtensionTypes.Mpan, ref extensions);

            // Act.
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.Test, ExtensionAppliedActions.AddOrModify)
            {
                Value = extValue,
                ValueSpecified = true,
                IsCritical = true,
                IsCriticalSpecified = true,
                IsEncrypted = false,
                IsEncryptedSpecified = true
            });
            _securityS2CryptoProvider.ApplyTestExtensionsSettings(new List<MessageTypes>(new MessageTypes[] { MessageTypes.SinglecastAll }), ref extensions);

            // Assert.
            Assert.AreEqual(1, extensions.EncryptedExtensionsList.Count);
            Assert.AreEqual(1, extensions.ExtensionsList.Count);
            var tvg1 = extensions.ExtensionsList[0];
            Assert.AreEqual((byte)ExtensionTypes.Test & 0x3F, tvg1.properties1.type);
            Assert.IsTrue(extValue.SequenceEqual(tvg1.extension));
            Assert.AreEqual(1, tvg1.properties1.critical);
            Assert.AreEqual(0, tvg1.properties1.moreToFollow);
        }

        [Test]
        public void AddOrModify_AddEncryptedExtAlreadyContainsUnencryptedExt_AddedEncryptedTestExtension()
        {
            // Arrange.
            var extValue = new byte[] { 0xAA, 0xFF, 0xFF, 0xFF, 0xFF };
            Extensions extensions = null;
            AddExtensionByType(ExtensionTypes.Span, ref extensions);

            // Act.
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.Test, ExtensionAppliedActions.AddOrModify)
            {
                Value = extValue,
                ValueSpecified = true,
                IsCritical = true,
                IsCriticalSpecified = true,
                IsEncrypted = true,
                IsEncryptedSpecified = true
            });
            _securityS2CryptoProvider.ApplyTestExtensionsSettings(new List<MessageTypes>(new MessageTypes[] { MessageTypes.SinglecastAll }), ref extensions);

            // Assert.
            Assert.AreEqual(1, extensions.ExtensionsList.Count);
            Assert.AreEqual(1, extensions.EncryptedExtensionsList.Count);
            var tvg1 = extensions.EncryptedExtensionsList[0];
            Assert.AreEqual((byte)ExtensionTypes.Test & 0x3F, tvg1.properties1.type);
            Assert.IsTrue(extValue.SequenceEqual(tvg1.extension));
            Assert.AreEqual(1, tvg1.properties1.critical);
            Assert.AreEqual(0, tvg1.properties1.moreToFollow);
        }

        [Test]
        public void AddOrModify_AddUnencryptedExtAlreadyContainsUnencryptedExt_AddedUnencryptedTestExtension()
        {
            // Arrange.
            var extValue = new byte[] { 0xAA, 0xFF, 0xFF, 0xFF, 0xFF };
            Extensions extensions = null;
            AddExtensionByType(ExtensionTypes.Span, ref extensions);

            // Act.
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.Test, ExtensionAppliedActions.AddOrModify)
            {
                Value = extValue,
                ValueSpecified = true,
                IsCritical = true,
                IsCriticalSpecified = true,
                IsEncrypted = false,
                IsEncryptedSpecified = true
            });
            _securityS2CryptoProvider.ApplyTestExtensionsSettings(new List<MessageTypes>(new MessageTypes[] { MessageTypes.SinglecastAll }), ref extensions);

            // Assert.
            Assert.AreEqual(2, extensions.ExtensionsList.Count);
            var tvg1 = extensions.ExtensionsList[0];
            Assert.AreEqual(1, tvg1.properties1.moreToFollow);
            tvg1 = extensions.ExtensionsList[1];
            Assert.AreEqual((byte)ExtensionTypes.Test & 0x3F, tvg1.properties1.type);
            Assert.IsTrue(extValue.SequenceEqual(tvg1.extension));
            Assert.AreEqual(1, tvg1.properties1.critical);
            Assert.AreEqual(0, tvg1.properties1.moreToFollow);
        }

        [Test]
        public void Add_EncryptedTestExtension_AddedTestExtension()
        {
            // Arrange.
            var extValue = new byte[] { 0xAA, 0xAA, 0xAA };
            Extensions extensions = null;

            // Act.
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.Test, ExtensionAppliedActions.Add)
            {
                Value = extValue,
                ValueSpecified = true,
                IsCritical = true,
                IsCriticalSpecified = true,
                IsMoreToFollow = true,
                IsMoreToFollowSpecified = true,
                IsEncrypted = true,
                IsEncryptedSpecified = true
            });
            _securityS2CryptoProvider.ApplyTestExtensionsSettings(new List<MessageTypes>(new MessageTypes[] { MessageTypes.SinglecastAll }), ref extensions);

            // Assert.
            Assert.AreEqual(1, extensions.EncryptedExtensionsList.Count);
            var tvg1 = extensions.EncryptedExtensionsList[0];
            Assert.IsTrue(extValue.SequenceEqual(tvg1.extension));
            Assert.AreEqual(1, tvg1.properties1.critical);
            Assert.AreEqual(1, tvg1.properties1.moreToFollow);
            Assert.AreEqual(5, tvg1.extensionLength.Value);
        }

        [Test]
        public void Add_UnencryptedTestExtension_AddedTestExtension()
        {
            // Arrange.
            var extValue = new byte[] { 0xAA, 0xAA, 0xAA };
            Extensions extensions = null;

            // Act.
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.Test, ExtensionAppliedActions.Add)
            {
                Value = extValue,
                ValueSpecified = true,
                IsCritical = true,
                IsCriticalSpecified = true,
                IsMoreToFollow = true,
                IsMoreToFollowSpecified = true,
                IsEncrypted = false,
                IsEncryptedSpecified = true
            });
            _securityS2CryptoProvider.ApplyTestExtensionsSettings(new List<MessageTypes>(new MessageTypes[] { MessageTypes.SinglecastAll }), ref extensions);

            // Assert.
            Assert.AreEqual(1, extensions.ExtensionsList.Count);
            var tvg1 = extensions.ExtensionsList[0];
            Assert.IsTrue(extValue.SequenceEqual(tvg1.extension));
            Assert.AreEqual(1, tvg1.properties1.critical);
            Assert.AreEqual(1, tvg1.properties1.moreToFollow);
            Assert.AreEqual(5, tvg1.extensionLength.Value);
        }

        [TestCase(ExtensionTypes.Mos)]
        [TestCase(ExtensionTypes.Mpan)]
        [TestCase(ExtensionTypes.MpanGrp)]
        [TestCase(ExtensionTypes.Span)]
        public void Add_AlreadyHasAnExtension_AddedTestExtension(ExtensionTypes extensionType)
        {
            // Arrange.
            Extensions extensions = null;
            AddExtensionByType(extensionType, ref extensions);

            // Act.
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, extensionType, ExtensionAppliedActions.Add));
            _securityS2CryptoProvider.ApplyTestExtensionsSettings(new List<MessageTypes>(new MessageTypes[] { MessageTypes.SinglecastAll }), ref extensions);

            // Assert.
            Assert.AreEqual(2, extensionType == ExtensionTypes.Mpan ?
                extensions.EncryptedExtensionsList.Count :
                extensions.ExtensionsList.Count);
            var tvg1 = extensionType == ExtensionTypes.Mpan ? extensions.EncryptedExtensionsList[0] : extensions.ExtensionsList[0];
            Assert.AreEqual(extensionType == ExtensionTypes.Mos ? 0 : 1, tvg1.properties1.critical);
            Assert.AreEqual(1, tvg1.properties1.moreToFollow);
            Assert.AreEqual(Extensions.GetLengthByExtensionType(extensionType), tvg1.extensionLength.Value);
            var tvg2 = extensionType == ExtensionTypes.Mpan ? extensions.EncryptedExtensionsList[1] : extensions.ExtensionsList[1];
            Assert.AreEqual(extensionType == ExtensionTypes.Mos ? 0 : 1, tvg2.properties1.critical);
            Assert.AreEqual(Extensions.GetLengthByExtensionType(extensionType), tvg2.extensionLength.Value);
            Assert.AreEqual(0, tvg2.properties1.moreToFollow);
        }

        [Test]
        public void Add_UnencryptedMpanExtension_AddedUnencryptedMpanExtension()
        {
            // Arrange.
            Extensions extensions = null;

            // Act.
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.Mpan, ExtensionAppliedActions.Add)
            {
                IsEncrypted = false,
                IsEncryptedSpecified = true
            });
            _securityS2CryptoProvider.ApplyTestExtensionsSettings(new List<MessageTypes>(new MessageTypes[] { MessageTypes.SinglecastAll }), ref extensions);

            // Assert.
            Assert.AreEqual(1, extensions.ExtensionsList.Count);
            var tvg1 = extensions.ExtensionsList[0];
            Assert.AreEqual((byte)ExtensionTypes.Mpan, tvg1.properties1.type);
            Assert.AreEqual(17, tvg1.extension.Where(val => val == 0x00).Count());
            Assert.AreEqual(1, tvg1.properties1.critical);
            Assert.AreEqual(0, tvg1.properties1.moreToFollow);
            Assert.AreEqual(19, tvg1.extensionLength.Value);
        }

        [TestCase(ExtensionTypes.Mos)]
        [TestCase(ExtensionTypes.MpanGrp)]
        [TestCase(ExtensionTypes.Span)]
        public void Add_EncryptedExtension_AddedEncryptedExtension(ExtensionTypes extensionType)
        {
            // Arrange.
            Extensions extensions = null;

            // Act.
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, extensionType, ExtensionAppliedActions.Add)
            {
                IsEncrypted = true,
                IsEncryptedSpecified = true
            });
            _securityS2CryptoProvider.ApplyTestExtensionsSettings(new List<MessageTypes>(new MessageTypes[] { MessageTypes.SinglecastAll }), ref extensions);

            // Assert.
            Assert.AreEqual(1, extensions.EncryptedExtensionsList.Count);
            var tvg1 = extensions.EncryptedExtensionsList[0];
            Assert.AreEqual((byte)extensionType, tvg1.properties1.type);
            Assert.AreEqual(extensionType == ExtensionTypes.Mos ? 0 : 1, tvg1.properties1.critical);
            Assert.AreEqual(0, tvg1.properties1.moreToFollow);
            Assert.AreEqual(Extensions.GetLengthByExtensionType(extensionType), tvg1.extensionLength.Value);
        }

        [Test]
        public void Add_ReplaceExtensionSpecifyMessageTypeAndNumOfUsage_AddedTestExtension()
        {
            // Arrange.
            Extensions extensions = null;
            AddExtensionByType(ExtensionTypes.MpanGrp, ref extensions);

            // Act.
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.MulticastAll, ExtensionTypes.MpanGrp, ExtensionAppliedActions.Delete));
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.MulticastAll, ExtensionTypes.Test, ExtensionAppliedActions.Add)
            {
                Value = new byte[1],
                ValueSpecified = true,
                IsEncrypted = false,
                IsEncryptedSpecified = true,
                IsCritical = true,
                IsCriticalSpecified = true,
                ExtensionLength = 3,
                ExtensionLengthSpecified = true,
                NumOfUsage = 1,
                NumOfUsageSpecified = true
            });
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastWithMpanGrp, ExtensionTypes.MpanGrp, ExtensionAppliedActions.Delete));
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastWithMpanGrp, ExtensionTypes.Test, ExtensionAppliedActions.Add)
            {
                Value = new byte[1],
                ValueSpecified = true,
                IsEncrypted = false,
                IsEncryptedSpecified = true,
                IsCritical = true,
                IsCriticalSpecified = true,
                ExtensionLength = 3,
                ExtensionLengthSpecified = true,
                NumOfUsage = 1,
                NumOfUsageSpecified = true
            });
            _securityS2CryptoProvider.ApplyTestExtensionsSettings(new List<MessageTypes>(
                new MessageTypes[] { MessageTypes.MulticastAll }), ref extensions);

            // Assert.
            Assert.AreEqual(1, extensions.ExtensionsList.Count);
            var tvg1 = extensions.ExtensionsList[0];
            Assert.AreEqual((byte)(ExtensionTypes.Test) >> 2, tvg1.properties1.type);
            Assert.AreEqual(1, tvg1.properties1.critical);
            Assert.AreEqual(3, tvg1.extensionLength.Value);

            extensions = null;
            AddExtensionByType(ExtensionTypes.MpanGrp, ref extensions);
            _securityS2CryptoProvider.ApplyTestExtensionsSettings(new List<MessageTypes>(
                new MessageTypes[] { MessageTypes.SinglecastWithMpanGrp }), ref extensions);
            Assert.AreEqual(1, extensions.ExtensionsList.Count);
            tvg1 = extensions.ExtensionsList[0];
            Assert.AreEqual((byte)ExtensionTypes.Test >> 2, tvg1.properties1.type);
            Assert.AreEqual(1, tvg1.properties1.critical);
            Assert.AreEqual(3, tvg1.extensionLength.Value);
        }

        private void AddExtensionByType(ExtensionTypes extensionType, ref Extensions extensions, params byte[] args)
        {
            if (extensions == null)
            {
                extensions = new Extensions();
            }
            switch (extensionType)
            {
                case ExtensionTypes.Mos:
                    {
                        extensions.AddMosExtension();
                    }
                    break;
                case ExtensionTypes.Mpan:
                    {
                        if (args.Length > 0)
                        {
                            extensions.AddMpanExtension(MPAN_STATE, args[0]);
                        }
                        else
                        {
                            extensions.AddMpanExtension(MPAN_STATE, 0x01);
                        }
                    }
                    break;
                case ExtensionTypes.MpanGrp:
                    {
                        if (args.Length > 0)
                        {
                            extensions.AddMpanGrpExtension(args[0]);
                        }
                        else
                        {
                            extensions.AddMpanGrpExtension(0x01);
                        }
                    }
                    break;
                case ExtensionTypes.Span:
                    {
                        extensions.AddSpanExtension(SENDER_EI);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
