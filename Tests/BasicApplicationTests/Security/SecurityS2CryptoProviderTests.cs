using System;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using ZWave.Security;
using ZWave.Enums;
using ZWave.BasicApplication;
using ZWave.BasicApplication.Enums;
using ZWave.BasicApplication.Security;
using ZWave.CommandClasses;
using ZWave.Configuration;
using ZWave;
using ZWave.Devices;

namespace BasicApplicationTests.Security
{
    [TestFixture]
    public class SecurityS2CryptoProviderTests
    {
        private SpanTable _spanTable;
        private Dictionary<InvariantPeerNodeId, SinglecastKey> _scKeys;
        private SecurityS2CryptoProvider _securityS2CryptoProvider;
        private SecurityManagerInfo _securityManagerInfo;
        private NetworkViewPoint _networkViewPoint;
        private const byte SESSION_ID = 0x07;
        private static NodeTag SOURCE_NODE_ID = new NodeTag(0x02);
        private byte[] HOME_ID = new byte[] { 0xde, 0xad, 0xbe, 0xef };
        private static NodeTag RECEIVER_NODE_ID = new NodeTag(0x02);
        private readonly InvariantPeerNodeId PEER_NODE_ID = new InvariantPeerNodeId(RECEIVER_NODE_ID, SOURCE_NODE_ID);
        private readonly byte[] RECEIVER_EI = new byte[] { 0x14, 0xa8, 0x26, 0x24, 0x15, 0x21, 0x3e, 0xfc, 0x65, 0xea, 0x50, 0xf2, 0x0a, 0x2f, 0x70, 0x68 };
        private readonly byte[] SENDER_EI = new byte[] { 0x6d, 0xde, 0x2c, 0xdb, 0x6c, 0xb2, 0x27, 0x4b, 0x8b, 0xb4, 0xff, 0xeb, 0x3f, 0x1e, 0xb3, 0x94 };
        private readonly byte[] NETWORK_KEY_S2_C0 = new byte[] { 0xc0, 0x07, 0x66, 0xd0, 0xa1, 0x66, 0x65, 0x1f, 0x64, 0x67, 0xe2, 0x01, 0xce, 0xa8, 0x07, 0x5b };
        private const byte RSSI_VAL = 0x7e; // RSSI_MAX_POWER_SATURATED
        private const int LEN_INDEX = 4;

        [SetUp]
        public void Setup()
        {
            _networkViewPoint = new NetworkViewPoint(null)
            {
                NodeTag = SOURCE_NODE_ID,
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
            _securityManagerInfo.ActivateNetworkKeyS2ForNode(PEER_NODE_ID, SecuritySchemes.S2_UNAUTHENTICATED, false);
        }

        [Test]
        public void EncryptSinglecastCommand_NoExtensions_DecryptsDataFrame()
        {
            var cmd = CreateDataAsVersionGet();

            // Act.
            SetupSpanRecord();
            var cryptedCmd = _securityS2CryptoProvider.EncryptSinglecastCommand(
                _scKeys[new InvariantPeerNodeId(_securityManagerInfo.Network.NodeTag, RECEIVER_NODE_ID)],
                _spanTable, _securityManagerInfo.Network.NodeTag, RECEIVER_NODE_ID, _securityManagerInfo.Network.HomeId, cmd, null, new SubstituteSettings());
            var cryptedDataFrame = new DataFrame(SESSION_ID, DataFrameTypes.Data, false, false, DateTime.Now);
            var dataFrameBuffer = CreateDataFrameBuffer(cryptedCmd);
            cryptedDataFrame.SetBuffer(dataFrameBuffer, 0, dataFrameBuffer.Length);
            var cryptedCmdData = GetCmdFromFrame(cryptedDataFrame);

            SetupSpanRecord();
            var spanContainer = _spanTable.GetContainer(PEER_NODE_ID);

            var isDecrypted = SecurityS2CryptoProviderBase.DecryptSinglecastFrame(
                spanContainer,
                _scKeys.ContainsKey(PEER_NODE_ID) ? _scKeys[PEER_NODE_ID] : null,
                RECEIVER_NODE_ID,
                _securityManagerInfo.Network.NodeTag,
                _securityManagerInfo.Network.HomeId,
                cryptedCmdData,
                out byte[] data,
                out Extensions extensions);

            var dataFrame = GetDataFrame(cryptedDataFrame, data);
            var plainData = GetCmdFromFrame(dataFrame);

            // Assert.
            Assert.IsTrue(isDecrypted);
            Assert.AreEqual(0, extensions.EncryptedExtensionsList.Count);
            Assert.IsTrue(cmd.SequenceEqual(plainData));
        }

        [Test]
        public void EncryptSinglecastCommand_OneTestExtensionWithoutOverride_MessagePropertiesAreCorrect()
        {
            // Arrange.
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.MpanGrp, ExtensionAppliedActions.Add)
            {
                IsEncrypted = false,
                IsEncryptedSpecified = true,
                IsCritical = true,
                IsCriticalSpecified = true,
                ExtensionLength = 3,
                ExtensionLengthSpecified = true,
                Value = new byte[] { 0x01 },
                ValueSpecified = true
            });

            var cmd = CreateDataAsVersionGet();

            // Act.
            SetupSpanRecord();
            var cryptedCmd = (COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION)_securityS2CryptoProvider.EncryptSinglecastCommand(
                _scKeys[new InvariantPeerNodeId(_securityManagerInfo.Network.NodeTag, RECEIVER_NODE_ID)],
                _spanTable, _securityManagerInfo.Network.NodeTag, RECEIVER_NODE_ID, _securityManagerInfo.Network.HomeId, cmd, null, new SubstituteSettings());

            // Assert.
            Assert.IsTrue(cryptedCmd.properties1.extension > 0);
            Assert.AreEqual(1, cryptedCmd.vg1.Count);
            Assert.AreEqual((byte)ExtensionTypes.MpanGrp, cryptedCmd.vg1[0].properties1.type);
        }

        [Test]
        public void EncryptSinglecastCommand_TwoTestExtensionsWithoutOverride_MessagePropertiesAreCorrect()
        {
            // Arrange.
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.MpanGrp, ExtensionAppliedActions.Add)
            {
                IsEncrypted = false,
                IsEncryptedSpecified = true,
                IsCritical = true,
                IsCriticalSpecified = true,
                ExtensionLength = 3,
                ExtensionLengthSpecified = true,
                Value = new byte[] { 0x01 },
                ValueSpecified = true
            });
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.MpanGrp, ExtensionAppliedActions.Add)
            {
                IsEncrypted = false,
                IsEncryptedSpecified = true,
                IsCritical = true,
                IsCriticalSpecified = true,
                ExtensionLength = 3,
                ExtensionLengthSpecified = true,
                Value = new byte[] { 0x02 },
                ValueSpecified = true
            });
            var cmd = CreateDataAsVersionGet();

            // Act.
            SetupSpanRecord();
            var cryptedCmd = (COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION)_securityS2CryptoProvider.EncryptSinglecastCommand(
                _scKeys[new InvariantPeerNodeId(_securityManagerInfo.Network.NodeTag, RECEIVER_NODE_ID)],
                _spanTable, _securityManagerInfo.Network.NodeTag, RECEIVER_NODE_ID, _securityManagerInfo.Network.HomeId, cmd, null, new SubstituteSettings());

            // Assert.
            Assert.IsTrue(cryptedCmd.properties1.extension > 0);
            Assert.AreEqual(2, cryptedCmd.vg1.Count);
            Assert.AreEqual((byte)ExtensionTypes.MpanGrp, cryptedCmd.vg1[0].properties1.type);
            Assert.IsTrue(cryptedCmd.vg1[0].properties1.moreToFollow > 0);
            Assert.AreEqual((byte)ExtensionTypes.MpanGrp, cryptedCmd.vg1[1].properties1.type);
            Assert.IsTrue(cryptedCmd.vg1[1].properties1.moreToFollow == 0);
        }

        [Test]
        public void EncryptSinglecastCommand_TwoTestExtensionsMoreToFollowFalse_OnlyOneExtensionIsParsed()
        {
            // Arrange.
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.MpanGrp, ExtensionAppliedActions.Add)
            {
                IsEncrypted = false,
                IsEncryptedSpecified = true,
                IsCritical = true,
                IsCriticalSpecified = true,
                IsMoreToFollow = false,
                IsMoreToFollowSpecified = true,
                ExtensionLength = 3,
                ExtensionLengthSpecified = true,
                Value = new byte[] { 0x01 },
                ValueSpecified = true
            });
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.MpanGrp, ExtensionAppliedActions.Add)
            {
                IsEncrypted = false,
                IsEncryptedSpecified = true,
                IsCritical = true,
                IsCriticalSpecified = true,
                ExtensionLength = 3,
                ExtensionLengthSpecified = true,
                Value = new byte[] { 0x02 },
                ValueSpecified = true
            });
            var cmd = CreateDataAsVersionGet();

            // Act.
            SetupSpanRecord();
            var cryptedCmd = (COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION)_securityS2CryptoProvider.EncryptSinglecastCommand(
                _scKeys[new InvariantPeerNodeId(_securityManagerInfo.Network.NodeTag, RECEIVER_NODE_ID)],
                _spanTable, _securityManagerInfo.Network.NodeTag, RECEIVER_NODE_ID, _securityManagerInfo.Network.HomeId, cmd, null, new SubstituteSettings());

            // Assert.
            Assert.IsTrue(cryptedCmd.properties1.extension > 0);
            Assert.AreEqual(1, cryptedCmd.vg1.Count);
            Assert.AreEqual((byte)ExtensionTypes.MpanGrp, cryptedCmd.vg1[0].properties1.type);
            Assert.AreEqual(0, cryptedCmd.vg1[0].properties1.moreToFollow);
        }

        [Test]
        public void EncryptSinglecastCommand_OneEncryptedTestExtensionWithoutOverride_MessagePropertiesAreCorrect()
        {
            // Arrange.
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.MpanGrp, ExtensionAppliedActions.Add)
            {
                IsEncrypted = true,
                IsEncryptedSpecified = true,
                IsCritical = true,
                IsCriticalSpecified = true,
                ExtensionLength = 3,
                ExtensionLengthSpecified = true,
                Value = new byte[] { 0x02 },
                ValueSpecified = true
            });
            var cmd = CreateDataAsVersionGet();

            // Act.
            SetupSpanRecord();
            var cryptedCmd = (COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION)_securityS2CryptoProvider.EncryptSinglecastCommand(
                _scKeys[new InvariantPeerNodeId(_securityManagerInfo.Network.NodeTag, RECEIVER_NODE_ID)],
                _spanTable, _securityManagerInfo.Network.NodeTag, RECEIVER_NODE_ID, _securityManagerInfo.Network.HomeId, cmd, null, new SubstituteSettings());

            // Assert.
            Assert.AreEqual(0, cryptedCmd.properties1.extension);
            Assert.IsTrue(cryptedCmd.properties1.encryptedExtension > 0);
        }

        [Test]
        public void DecryptSinglecastFrame_EmptyPayloadOneEncExtension_DecryptedExtensionAreCorrect()
        {
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.MpanGrp, ExtensionAppliedActions.Add)
            {
                IsEncrypted = true,
                IsEncryptedSpecified = true,
                IsCritical = true,
                IsCriticalSpecified = true,
                ExtensionLength = 3,
                ExtensionLengthSpecified = true,
                Value = new byte[] { 0x01 },
                ValueSpecified = true
            });
            var cmd = new byte[0];

            // Act.
            SetupSpanRecord();

            var cryptedCmd = _securityS2CryptoProvider.EncryptSinglecastCommand(
                _scKeys[new InvariantPeerNodeId(_securityManagerInfo.Network.NodeTag, RECEIVER_NODE_ID)],
                _spanTable,
                _securityManagerInfo.Network.NodeTag,
                RECEIVER_NODE_ID,
                _securityManagerInfo.Network.HomeId,
                cmd,
                null,
                new SubstituteSettings());

            var cryptedDataFrame = new DataFrame(SESSION_ID, DataFrameTypes.Data, false, false, DateTime.Now);
            var dataFrameBuffer = CreateDataFrameBuffer(cryptedCmd);
            cryptedDataFrame.SetBuffer(dataFrameBuffer, 0, dataFrameBuffer.Length);
            var cryptedCmdData = GetCmdFromFrame(cryptedDataFrame);
            SetupSpanRecord();

            var spanContainer = _spanTable.GetContainer(PEER_NODE_ID);

            var isDecrypted = SecurityS2CryptoProviderBase.DecryptSinglecastFrame(
                spanContainer,
                _scKeys.ContainsKey(PEER_NODE_ID) ? _scKeys[PEER_NODE_ID] : null,
                SOURCE_NODE_ID,
                _securityManagerInfo.Network.NodeTag,
                _securityManagerInfo.Network.HomeId,
                cryptedCmdData,
                out byte[] data,
                out Extensions extensions);

            // Assert.
            Assert.IsTrue(isDecrypted);
            Assert.AreEqual(0, data.Length);
            Assert.AreEqual(1, extensions.EncryptedExtensionsList.Count);
        }

        [Test]
        public void DecryptSinglecastFrame_EmptyPayloadOneEncExtensionInvalidMoreToFollowFlag_DecryptSucceeded()
        {
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.MpanGrp, ExtensionAppliedActions.Add)
            {
                IsEncrypted = true,
                IsEncryptedSpecified = true,
                IsCritical = true,
                IsCriticalSpecified = true,
                IsMoreToFollow = true,
                IsMoreToFollowSpecified = true,
                ExtensionLength = 3,
                ExtensionLengthSpecified = true,
                Value = new byte[] { 0x01 },
                ValueSpecified = true
            });
            var cmd = new byte[0];

            // Act.
            SetupSpanRecord();

            var cryptedCmd = _securityS2CryptoProvider.EncryptSinglecastCommand(
                _scKeys[new InvariantPeerNodeId(_securityManagerInfo.Network.NodeTag, RECEIVER_NODE_ID)],
                _spanTable,
                _securityManagerInfo.Network.NodeTag,
                RECEIVER_NODE_ID,
                _securityManagerInfo.Network.HomeId,
                cmd,
                null,
                new SubstituteSettings());

            var cryptedDataFrame = new DataFrame(SESSION_ID, DataFrameTypes.Data, false, false, DateTime.Now);
            var dataFrameBuffer = CreateDataFrameBuffer(cryptedCmd);
            cryptedDataFrame.SetBuffer(dataFrameBuffer, 0, dataFrameBuffer.Length);
            var cryptedCmdData = GetCmdFromFrame(cryptedDataFrame);
            SetupSpanRecord();

            var spanContainer = _spanTable.GetContainer(PEER_NODE_ID);

            var isDecrypted = SecurityS2CryptoProviderBase.DecryptSinglecastFrame(
                spanContainer,
                _scKeys.ContainsKey(PEER_NODE_ID) ? _scKeys[PEER_NODE_ID] : null,
                RECEIVER_NODE_ID,
                _securityManagerInfo.Network.NodeTag,
                _securityManagerInfo.Network.HomeId,
                cryptedCmdData,
                out byte[] data,
                out Extensions extensions);

            var dataFrame = GetDataFrame(cryptedDataFrame, data);

            // Assert.
            Assert.IsTrue(isDecrypted);
            Assert.IsNotNull(dataFrame);
            Assert.AreEqual(0, data.Length);
            Assert.AreEqual(1, extensions.EncryptedExtensionsList.Count);
        }

        [Test]
        public void DecryptSinglecastFrame_OneEncExtensionInvalidMoreToFollowFlag_DecryptSucceeded()
        {
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.MpanGrp, ExtensionAppliedActions.Add)
            {
                IsEncrypted = true,
                IsEncryptedSpecified = true,
                IsCritical = true,
                IsCriticalSpecified = true,
                IsMoreToFollow = true,
                IsMoreToFollowSpecified = true,
                ExtensionLength = 3,
                ExtensionLengthSpecified = true,
                Value = new byte[] { 0x01 },
                ValueSpecified = true
            });
            var cmd = CreateDataAsVersionGet();

            // Act.
            SetupSpanRecord();

            var cryptedCmd = _securityS2CryptoProvider.EncryptSinglecastCommand(
                _scKeys[new InvariantPeerNodeId(_securityManagerInfo.Network.NodeTag, RECEIVER_NODE_ID)],
                _spanTable,
                _securityManagerInfo.Network.NodeTag,
                RECEIVER_NODE_ID,
                _securityManagerInfo.Network.HomeId,
                cmd,
                null,
                new SubstituteSettings());

            var cryptedDataFrame = new DataFrame(SESSION_ID, DataFrameTypes.Data, false, false, DateTime.Now);
            var dataFrameBuffer = CreateDataFrameBuffer(cryptedCmd);
            cryptedDataFrame.SetBuffer(dataFrameBuffer, 0, dataFrameBuffer.Length);
            var cryptedCmdData = GetCmdFromFrame(cryptedDataFrame);
            SetupSpanRecord();

            var spanContainer = _spanTable.GetContainer(PEER_NODE_ID);

            var isDecrypted = SecurityS2CryptoProviderBase.DecryptSinglecastFrame(
                spanContainer,
                _scKeys.ContainsKey(PEER_NODE_ID) ? _scKeys[PEER_NODE_ID] : null,
                RECEIVER_NODE_ID,
                _securityManagerInfo.Network.NodeTag,
                _securityManagerInfo.Network.HomeId,
                cryptedCmdData,
                out byte[] data,
                out Extensions extensions);

            var dataFrame = GetDataFrame(cryptedDataFrame, data);

            // Assert.
            Assert.IsTrue(isDecrypted);
            Assert.IsNotNull(dataFrame);
            Assert.AreEqual(cmd.Length, data.Length);
            Assert.AreEqual(1, extensions.EncryptedExtensionsList.Count);
        }


        [Test]
        public void DecryptSinglecastFrame_OneEncryptedTestExtensionWithoutOverride_DecryptedPropertiesAreCorrect()
        {
            var extensionValue = new byte[] { 0x01 };
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.MpanGrp, ExtensionAppliedActions.Add)
            {
                IsEncrypted = true,
                IsEncryptedSpecified = true,
                IsCritical = true,
                IsCriticalSpecified = true,
                ExtensionLength = 3,
                ExtensionLengthSpecified = true,
                Value = extensionValue,
                ValueSpecified = true
            });
            var cmd = CreateDataAsVersionGet();

            // Act.
            SetupSpanRecord();

            var cryptedCmd = _securityS2CryptoProvider.EncryptSinglecastCommand(
                _scKeys[new InvariantPeerNodeId(_securityManagerInfo.Network.NodeTag, RECEIVER_NODE_ID)],
                _spanTable,
                _securityManagerInfo.Network.NodeTag,
                RECEIVER_NODE_ID,
                _securityManagerInfo.Network.HomeId,
                cmd,
                null,
                new SubstituteSettings());

            var cryptedDataFrame = new DataFrame(SESSION_ID, DataFrameTypes.Data, false, false, DateTime.Now);
            var dataFrameBuffer = CreateDataFrameBuffer(cryptedCmd);
            cryptedDataFrame.SetBuffer(dataFrameBuffer, 0, dataFrameBuffer.Length);
            var cryptedCmdData = GetCmdFromFrame(cryptedDataFrame);
            SetupSpanRecord();

            var spanContainer = _spanTable.GetContainer(PEER_NODE_ID);

            var isDecrypted = SecurityS2CryptoProviderBase.DecryptSinglecastFrame(
                spanContainer,
                _scKeys.ContainsKey(PEER_NODE_ID) ? _scKeys[PEER_NODE_ID] : null,
                RECEIVER_NODE_ID,
                _securityManagerInfo.Network.NodeTag,
                _securityManagerInfo.Network.HomeId,
                cryptedCmdData,
                out byte[] data,
                out Extensions extensions);

            var dataFrame = GetDataFrame(cryptedDataFrame, data);
            var plainData = GetCmdFromFrame(dataFrame);

            // Assert.
            Assert.IsTrue(isDecrypted);
            Assert.AreEqual(2, plainData.Length);
            Assert.AreEqual(1, extensions.EncryptedExtensionsList.Count);

            var ext = new COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1();
            var decryptedExt = extensions.EncryptedExtensionsList[0];
            ext.extensionLength = decryptedExt.extensionLength;
            ext.properties1 = decryptedExt.properties1;
            ext.extension = new byte[] { decryptedExt.extension[0] };

            Assert.AreEqual((byte)ExtensionTypes.MpanGrp, ext.properties1.type);
            Assert.AreEqual(0, ext.properties1.moreToFollow);
            Assert.IsTrue(ext.extension.SequenceEqual(extensionValue));
        }

        [Test]
        public void DecryptSinglecastFrame_TwoEncryptedTestExtensionWithoutOverride_DecryptedPropertiesAreCorrect()
        {
            var extensionValue = new byte[] { 0x01 };
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.MpanGrp, ExtensionAppliedActions.Add)
            {
                IsEncrypted = true,
                IsEncryptedSpecified = true,
                IsCritical = true,
                IsCriticalSpecified = true,
                ExtensionLength = 3,
                ExtensionLengthSpecified = true,
                Value = extensionValue,
                ValueSpecified = true
            });
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.MpanGrp, ExtensionAppliedActions.Add)
            {
                IsEncrypted = true,
                IsEncryptedSpecified = true,
                IsCritical = true,
                IsCriticalSpecified = true,
                ExtensionLength = 3,
                ExtensionLengthSpecified = true,
                Value = extensionValue,
                ValueSpecified = true
            });
            var cmd = CreateDataAsVersionGet();

            // Act.
            SetupSpanRecord();

            var cryptedCmd = _securityS2CryptoProvider.EncryptSinglecastCommand(
                _scKeys[new InvariantPeerNodeId(_securityManagerInfo.Network.NodeTag, RECEIVER_NODE_ID)],
                _spanTable,
                _securityManagerInfo.Network.NodeTag,
                RECEIVER_NODE_ID,
                _securityManagerInfo.Network.HomeId,
                cmd,
                null,
                new SubstituteSettings());

            var cryptedDataFrame = new DataFrame(SESSION_ID, DataFrameTypes.Data, false, false, DateTime.Now);
            var dataFrameBuffer = CreateDataFrameBuffer(cryptedCmd);
            cryptedDataFrame.SetBuffer(dataFrameBuffer, 0, dataFrameBuffer.Length);
            var cryptedCmdData = GetCmdFromFrame(cryptedDataFrame);

            SetupSpanRecord();
            var spanContainer = _spanTable.GetContainer(PEER_NODE_ID);

            var isDecrypted = SecurityS2CryptoProviderBase.DecryptSinglecastFrame(
                spanContainer,
                _scKeys.ContainsKey(PEER_NODE_ID) ? _scKeys[PEER_NODE_ID] : null,
                RECEIVER_NODE_ID,
                _securityManagerInfo.Network.NodeTag,
                _securityManagerInfo.Network.HomeId,
                cryptedCmdData,
                out byte[] data,
                out Extensions extensions);

            var dataFrame = GetDataFrame(cryptedDataFrame, data);
            var plainData = GetCmdFromFrame(dataFrame);

            // Assert.
            Assert.IsTrue(isDecrypted);
            Assert.AreEqual(2, plainData.Length);
            Assert.AreEqual(2, extensions.EncryptedExtensionsList.Count);

            var extData1 = extensions.EncryptedExtensionsList[0];
            var ext1 = new COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1();
            ext1.extensionLength = extData1.extensionLength;
            ext1.properties1 = extData1.properties1;
            ext1.extension = new byte[] { extData1.extension[0] };
            var extData2 = extensions.EncryptedExtensionsList[1];
            var ext2 = new COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.TVG1();
            ext2.extensionLength = extData2.extensionLength;
            ext2.properties1 = extData2.properties1;
            ext2.extension = new byte[] { extData2.extension[0] };

            Assert.AreEqual((byte)ExtensionTypes.MpanGrp, ext1.properties1.type);
            Assert.IsTrue(ext1.properties1.moreToFollow > 0);
            Assert.IsTrue(ext1.extension.SequenceEqual(extensionValue));
            Assert.AreEqual((byte)ExtensionTypes.MpanGrp, ext2.properties1.type);
            Assert.AreEqual(0, ext2.properties1.moreToFollow);
            Assert.IsTrue(ext2.extension.SequenceEqual(extensionValue));
        }

        [Test]
        public void DecryptSinglecastFrame_EmptyFrameWithoutExtensions_DecryptSucceeded()
        {
            var cmd = new byte[0];

            // Act.
            SetupSpanRecord();

            var cryptedCmd = _securityS2CryptoProvider.EncryptSinglecastCommand(
                _scKeys[new InvariantPeerNodeId(_securityManagerInfo.Network.NodeTag, RECEIVER_NODE_ID)],
                _spanTable,
                _securityManagerInfo.Network.NodeTag,
                RECEIVER_NODE_ID,
                _securityManagerInfo.Network.HomeId,
                cmd,
                null,
                new SubstituteSettings());

            var cryptedDataFrame = new DataFrame(SESSION_ID, DataFrameTypes.Data, false, false, DateTime.Now);
            var dataFrameBuffer = CreateDataFrameBuffer(cryptedCmd);
            cryptedDataFrame.SetBuffer(dataFrameBuffer, 0, dataFrameBuffer.Length);
            var cryptedCmdData = GetCmdFromFrame(cryptedDataFrame);

            var spanContainer = _spanTable.GetContainer(PEER_NODE_ID);

            var isDecrypted = SecurityS2CryptoProviderBase.DecryptSinglecastFrame(
                spanContainer,
                _scKeys.ContainsKey(PEER_NODE_ID) ? _scKeys[PEER_NODE_ID] : null,
                RECEIVER_NODE_ID,
                _securityManagerInfo.Network.NodeTag,
                _securityManagerInfo.Network.HomeId,
                cryptedCmdData,
                out byte[] data,
                out Extensions extensions);

            var dataFrame = GetDataFrame(cryptedDataFrame, data);

            // Assert.
            Assert.IsTrue(isDecrypted);
            Assert.IsNotNull(dataFrame);
            Assert.AreEqual(0, data.Length);
            Assert.AreEqual(0, extensions.EncryptedExtensionsList.Count);
        }

        [Test]
        public void EncryptSinglecastCommand_TwoTestExtensionsMultiAndSingleCastWithoutOverride_OnlyOneExtensionAdded()
        {
            // Arrange.
            var extensionValue = new byte[] { 0x01 };
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.MulticastAll, ExtensionTypes.MpanGrp, ExtensionAppliedActions.Add)
            {
                IsEncrypted = true,
                IsEncryptedSpecified = true,
                IsCritical = true,
                IsCriticalSpecified = true,
                ExtensionLength = 3,
                ExtensionLengthSpecified = true,
                Value = extensionValue,
                ValueSpecified = true
            });
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.MpanGrp, ExtensionAppliedActions.Add)
            {
                IsEncrypted = false,
                IsEncryptedSpecified = true,
                IsCritical = true,
                IsCriticalSpecified = true,
                ExtensionLength = 3,
                ExtensionLengthSpecified = true,
                Value = extensionValue,
                ValueSpecified = true
            });
            var cmd = CreateDataAsVersionGet();

            // Act.
            SetupSpanRecord();
            var cryptedCmd = (COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION)_securityS2CryptoProvider.EncryptSinglecastCommand(
                _scKeys[new InvariantPeerNodeId(_securityManagerInfo.Network.NodeTag, RECEIVER_NODE_ID)],
                _spanTable, _securityManagerInfo.Network.NodeTag, RECEIVER_NODE_ID, _securityManagerInfo.Network.HomeId, cmd, null, new SubstituteSettings());

            // Assert.
            Assert.IsTrue(cryptedCmd.properties1.extension > 0);
            Assert.AreEqual(1, cryptedCmd.vg1.Count);
            Assert.AreEqual((byte)ExtensionTypes.MpanGrp, cryptedCmd.vg1[0].properties1.type);
            Assert.AreEqual(0, cryptedCmd.vg1[0].properties1.moreToFollow);
        }

        [Test]
        public void EncryptSinglecastCommand_OneTestExtensionUseTwoTime_MessagePropertiesAreCorrect()
        {
            // Arrange.
            _securityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.MpanGrp, ExtensionAppliedActions.Add)
            {
                IsEncrypted = false,
                IsEncryptedSpecified = true,
                IsCritical = true,
                IsCriticalSpecified = true,
                ExtensionLength = 3,
                ExtensionLengthSpecified = true,
                Value = new byte[] { 0x01 },
                ValueSpecified = true,
                NumOfUsage = 2,
                NumOfUsageSpecified = true
            });
            var cmd = CreateDataAsVersionGet();

            // Act.
            SetupSpanRecord();
            var fisrtCryptedCmd = (COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION)_securityS2CryptoProvider.EncryptSinglecastCommand(
                _scKeys[new InvariantPeerNodeId(_securityManagerInfo.Network.NodeTag, RECEIVER_NODE_ID)],
                _spanTable, _securityManagerInfo.Network.NodeTag, RECEIVER_NODE_ID, _securityManagerInfo.Network.HomeId, cmd, null, new SubstituteSettings());
            var secondCryptedCmd = (COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION)_securityS2CryptoProvider.EncryptSinglecastCommand(
                _scKeys[new InvariantPeerNodeId(_securityManagerInfo.Network.NodeTag, RECEIVER_NODE_ID)],
                _spanTable, _securityManagerInfo.Network.NodeTag, RECEIVER_NODE_ID, _securityManagerInfo.Network.HomeId, cmd, null, new SubstituteSettings());
            var thirdCryptedCmd = (COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION)_securityS2CryptoProvider.EncryptSinglecastCommand(
                _scKeys[new InvariantPeerNodeId(_securityManagerInfo.Network.NodeTag, RECEIVER_NODE_ID)],
                _spanTable, _securityManagerInfo.Network.NodeTag, RECEIVER_NODE_ID, _securityManagerInfo.Network.HomeId, cmd, null, new SubstituteSettings());

            // Assert.
            Assert.IsTrue(fisrtCryptedCmd.properties1.extension > 0);
            Assert.AreEqual(1, fisrtCryptedCmd.vg1.Count);
            Assert.AreEqual((byte)ExtensionTypes.MpanGrp, fisrtCryptedCmd.vg1[0].properties1.type);
            Assert.IsTrue(secondCryptedCmd.properties1.extension > 0);
            Assert.AreEqual(1, secondCryptedCmd.vg1.Count);
            Assert.AreEqual((byte)ExtensionTypes.MpanGrp, secondCryptedCmd.vg1[0].properties1.type);
            Assert.AreEqual(0, thirdCryptedCmd.properties1.extension);
            Assert.AreEqual(0, thirdCryptedCmd.vg1.Count);
        }

        private void SetupSpanRecord()
        {
            _spanTable.AddOrReplace(PEER_NODE_ID, RECEIVER_EI,
                _spanTable.GetTxSequenceNumber(PEER_NODE_ID),
                _spanTable.GetTxSequenceNumber(PEER_NODE_ID));
            var spanContainer = _spanTable.GetContainer(PEER_NODE_ID);
            spanContainer.InstantiateWithSenderNonce(SENDER_EI, _scKeys[PEER_NODE_ID].Personalization);
        }

        private DataFrame GetDataFrame(DataFrame cryptedDataFrame, byte[] cryptedCmdData)
        {
            InvariantPeerNodeId peerNodeId = new InvariantPeerNodeId(RECEIVER_NODE_ID, _securityManagerInfo.Network.NodeTag);
            var dataFrame = DataFrame.CreateDataFrame(cryptedDataFrame, LEN_INDEX, cryptedCmdData,
                (byte)_scKeys[peerNodeId].SecurityScheme,
                _scKeys.ContainsKey(peerNodeId),
                (byte)_scKeys[peerNodeId].SecurityScheme);
            return dataFrame;
        }

        private byte[] GetCmdFromFrame(DataFrame dataFrame)
        {
            byte[] cmdData = new byte[dataFrame.Data[LEN_INDEX]];
            Array.Copy(dataFrame.Data, LEN_INDEX + 1, cmdData, 0, cmdData.Length);
            return cmdData;
        }

        private byte[] CreateDataAsVersionGet()
        {
            return new COMMAND_CLASS_VERSION.VERSION_GET();
        }

        private byte[] CreateDataFrameBuffer(byte[] cmd)
        {
            // Create application command handler.
            // ZW->PC: REQ | 0x04 | rxStatus | sourceNode | cmdLength | pCmd[] | rssiVal 
            const byte rxStatus = 0x00;
            var dataList = new List<byte>();
            dataList.AddRange(new byte[] { (byte)FrameTypes.Request, (byte)CommandTypes.CmdApplicationCommandHandler, rxStatus, (byte)SOURCE_NODE_ID.Id, (byte)cmd.Length });
            dataList.AddRange(cmd);
            dataList.Add(RSSI_VAL);
            //

            byte[] ret = new byte[dataList.Count + 3];
            ret[0] = (byte)HeaderTypes.Sof;
            ret[1] = (byte)(dataList.Count + 1);
            Array.Copy(dataList.ToArray(), 0, ret, 2, dataList.Count);
            ret[dataList.Count + 2] = CalculateChecksum(dataList.ToArray());
            return ret;
        }

        private byte CalculateChecksum(byte[] data)
        {
            byte calcChksum = 0xFF;
            calcChksum ^= (byte)(data.Length + 1); // Length
            for (int i = 0; i < data.Length; i++)
                calcChksum ^= data[i];      // Data
            return calcChksum;
        }
    }
}