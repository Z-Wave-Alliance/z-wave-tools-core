using System;
using System.Linq;
using NUnit.Framework;
using ZWave.Enums;
using ZWave.Security;
using ZWave.CommandClasses;
using ZWave.BasicApplication;
using ZWave.BasicApplication.Operations;
using ZWave.BasicApplication.Security;
using ZWave.Configuration;
using ZWave.Devices;

namespace BasicApplicationTests.Security.SendingData
{
    [TestFixture]
    public class SendingDataS2Tests : TestBase
    {
        private SecurityManager SecurityManagerFirst
        {
            get { return (SecurityManager)_ctrlFirst.SessionClient.GetSubstituteManager(typeof(SecurityManager)); }
        }

        private SecurityManager SecurityManagerSecond
        {
            get { return (SecurityManager)_ctrlSecond.SessionClient.GetSubstituteManager(typeof(SecurityManager)); }
        }

        [SetUp]
        public void SetUp()
        {
            _transport.SetUpModulesNetwork(_ctrlFirst.SessionId, _ctrlSecond.SessionId);
            _ctrlFirst.MemoryGetId();
            _ctrlSecond.MemoryGetId();

            var smFirst = new SecurityManager(_ctrlFirst.Network,
                new[]
                {
                    new NetworkKey(NKEY1_S2_C0), new NetworkKey(NKEY1_S2_C1), new NetworkKey(NKEY1_S2_C2), null, null, null, null, new NetworkKey(NKEY1_S0)
                }, 
                new byte[]
                {
                    0x77, 0x07, 0x6d, 0x0a, 0x73, 0x18, 0xa5, 0x7d, 0x3c, 0x16, 0xc1, 0x72, 0x51, 0xb2, 0x66, 0x45,
                    0xdf, 0x4c, 0x2f, 0x87, 0xeb, 0xc0, 0x99, 0x2a, 0xb1, 0x77, 0xfb, 0xa5, 0x1d, 0xb9, 0x2c, 0x2a
                });
            _ctrlFirst.SessionClient.AddSubstituteManager(smFirst);
            _ctrlFirst.Network.IsEnabledS2_ACCESS = true;
            _ctrlFirst.Network.SetSecuritySchemes(NODE_ID_2, SecuritySchemeSet.ALLS2);
            _ctrlFirst.SessionClient.ExecuteAsync(smFirst.CreateSecurityS2ReportTask());

            var smSecond = new SecurityManager(_ctrlSecond.Network,
                new[]
                {
                    new NetworkKey(NKEY1_S2_C0), new NetworkKey(NKEY1_S2_C1), new NetworkKey(NKEY1_S2_C2), null, null, null, null, new NetworkKey(NKEY1_S0)
                }
                , new byte[]
                {
                    0x77, 0x07, 0x6d, 0x0a, 0x73, 0x18, 0xa5, 0x7d, 0x3c, 0x16, 0xc1, 0x72, 0x51, 0xb2, 0x66, 0x45,
                    0xdf, 0x4c, 0x2f, 0x87, 0xeb, 0xc0, 0x99, 0x2a, 0xb1, 0x77, 0xfb, 0xa5, 0x1d, 0xb9, 0x2c, 0x2a
                });
            _ctrlSecond.SessionClient.AddSubstituteManager(smSecond);
            _ctrlSecond.Network.IsEnabledS2_ACCESS = true;
            _ctrlSecond.Network.SetSecuritySchemes(NODE_ID_1, SecuritySchemeSet.ALLS2);
            _ctrlSecond.SessionClient.ExecuteAsync(smSecond.CreateSecurityS2ReportTask());
        }

        [Test]
        public void A_SendData_DevicesAreOutOfSync_SyncDevicesAndDecryptsData()
        {
            // Arange.

            // Act.
            byte[] primaryCmd = CreateDataAsBasicSet();
            var expectToken = _ctrlSecond.ExpectData(primaryCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(new NodeTag(_ctrlSecond.Id), primaryCmd, TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsTrue(primaryCmd.SequenceEqual(expectRes.Command));
            Assert.IsTrue(SecurityManagerFirst.SecurityManagerInfo.SpanTable.CheckNonceExists(PEER_NODE_ID_2));
            Assert.IsTrue(SecurityManagerSecond.SecurityManagerInfo.SpanTable.CheckNonceExists(PEER_NODE_ID_1));

            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_GET.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID)
            });
        }

        [Test]
        public void SendData_GotSyncWithOneKeyDataSentWithAnotherOne_SyncDevicesAndDecryptsData()
        {
            // Arange.

            // Act.
            byte[] primaryCmd = CreateDataAsBasicSet();
            var expectToken = _ctrlSecond.ExpectData(primaryCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(new NodeTag(_ctrlSecond.Id), primaryCmd, TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsTrue(expectRes.Command.SequenceEqual(primaryCmd));
            Assert.IsTrue(SecurityManagerFirst.SecurityManagerInfo.SpanTable.CheckNonceExists(PEER_NODE_ID_2));
            Assert.IsTrue(SecurityManagerSecond.SecurityManagerInfo.SpanTable.CheckNonceExists(PEER_NODE_ID_1));

            // Change sender network key to class2 security.
            SecurityManagerFirst.SecurityManagerInfo.ActivateNetworkKeyS2ForNode(PEER_NODE_ID_2, SecuritySchemes.S2_AUTHENTICATED, false);

            expectToken = _ctrlSecond.ExpectData(primaryCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(new NodeTag(_ctrlSecond.Id), primaryCmd, TXO);
            expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsTrue(primaryCmd.SequenceEqual(expectRes.Command));
            Assert.IsTrue(SecurityManagerFirst.SecurityManagerInfo.SpanTable.CheckNonceExists(PEER_NODE_ID_2));
            Assert.IsTrue(SecurityManagerSecond.SecurityManagerInfo.SpanTable.CheckNonceExists(PEER_NODE_ID_1));

            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_GET.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_GET.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID)
            });
        }

        [Test]
        public void SendData_WrongSenderNonceOnSender_SenderStopsDataSendingAfterOneRetransmission()
        {
            // Arange.
            byte[] primaryCmd = CreateDataAsBasicSet();
            SecurityManagerFirst.SecurityManagerInfo.SetTestSenderEntropyInputS2(
                new byte[] { 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01 });

            // Act.
            var expectToken = _ctrlSecond.ExpectData(primaryCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsNull(expectRes.Command);
            Assert.IsFalse(SecurityManagerFirst.SecurityManagerInfo.SpanTable.CheckNonceExists(PEER_NODE_ID_2));
            Assert.IsTrue(SecurityManagerSecond.SecurityManagerInfo.SpanTable.CheckNonceExists(PEER_NODE_ID_1));

            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
                {
                    FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_GET.ID),
                    FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                    FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID),
                    FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                    FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID),
                    FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID)
                });
        }

        [Test]
        public void SendData_WrongSenderNonceOnSenderWithCounter_ResynchronizationAfterFirstNonceReport()
        {
            // Arange.

            byte[] primaryCmd = CreateDataAsBasicSet();
            SecurityManagerFirst.SecurityManagerInfo.SetTestSenderEntropyInputS2(
                new byte[] { 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01 }, 1);

            // Act.
            var expectToken = _ctrlSecond.ExpectData(primaryCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO);
            expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsTrue(SecurityManagerFirst.SecurityManagerInfo.SpanTable.CheckNonceExists(PEER_NODE_ID_2));
            Assert.IsTrue(SecurityManagerSecond.SecurityManagerInfo.SpanTable.CheckNonceExists(PEER_NODE_ID_1));

            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_GET.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID),
            });
        }

        [Test]
        public void SendData_WrongSpanOnSender_SenderStopsDataSendingAfterOneRetransmission()
        {
            // Arange.
            byte[] primaryCmd = CreateDataAsBasicSet();
            SecurityManagerFirst.SecurityManagerInfo.SetTestSpanS2(
                new byte[] { 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01 });

            // Act.
            var expectToken = _ctrlSecond.ExpectData(primaryCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsNull(expectRes.Command);
            Assert.IsFalse(SecurityManagerFirst.SecurityManagerInfo.SpanTable.CheckNonceExists(PEER_NODE_ID_2));
            Assert.IsTrue(SecurityManagerSecond.SecurityManagerInfo.SpanTable.CheckNonceExists(PEER_NODE_ID_1));

            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_GET.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID)
            });
        }

        [Test]
        public void SendData_WrongSpanOnSenderWithCounter_ResynchronizationAfterFirstNonceReport()
        {
            // Arange.

            byte[] primaryCmd = CreateDataAsBasicSet();
            SecurityManagerFirst.SecurityManagerInfo.SetTestSpanS2(
                new byte[] { 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01 }, 1);

            // Act.
            var expectToken = _ctrlSecond.ExpectData(primaryCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO);
            expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsTrue(SecurityManagerFirst.SecurityManagerInfo.SpanTable.CheckNonceExists(PEER_NODE_ID_2));
            Assert.IsTrue(SecurityManagerSecond.SecurityManagerInfo.SpanTable.CheckNonceExists(PEER_NODE_ID_1));

            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_GET.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID),
            });
        }

        [Test]
        public void SendData_WrongReceiverNonceOnSender_SenderStopsDataSendingAfterOneRetransmission()
        {
            // Arange.

            byte[] primaryCmd = CreateDataAsBasicSet();
            SecurityManagerFirst.SecurityManagerInfo.SetTestReceiverEntropyS2(
                new byte[] { 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01 });

            // Act.
            var expectToken = _ctrlSecond.ExpectData(primaryCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsNull(expectRes.Command);
            Assert.IsFalse(SecurityManagerFirst.SecurityManagerInfo.SpanTable.CheckNonceExists(PEER_NODE_ID_2));
            Assert.IsTrue(SecurityManagerSecond.SecurityManagerInfo.SpanTable.CheckNonceExists(PEER_NODE_ID_1));

            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_GET.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID)
            });
        }

        [Test]
        public void SendData_WrongReceiverNonceOnSenderWithCounter_ResynchronizationAfterFirstNonceReport()
        {
            // Arange.

            byte[] primaryCmd = CreateDataAsBasicSet();
            SecurityManagerFirst.SecurityManagerInfo.SetTestReceiverEntropyS2(
                new byte[] { 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01 }, 1);

            // Act.
            var expectToken = _ctrlSecond.ExpectData(primaryCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO);
            expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsTrue(SecurityManagerFirst.SecurityManagerInfo.SpanTable.CheckNonceExists(PEER_NODE_ID_2));
            Assert.IsTrue(SecurityManagerSecond.SecurityManagerInfo.SpanTable.CheckNonceExists(PEER_NODE_ID_1));

            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_GET.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID),
            });
        }

        [Test]
        public void SendData_OutOfSyncOccured_SyncAgainAndDecryptData()
        {
            // Arange.

            byte[] primaryCmd = CreateDataAsBasicSet();

            // Act.
            var tempToken = _ctrlSecond.ExpectData(primaryCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO);
            tempToken.WaitCompletedSignal();

            // Remove nonce record in table on receiver.
            SecurityManagerSecond.SecurityManagerInfo.SpanTable.SetNonceFree(PEER_NODE_ID_1);

            var expectToken = _ctrlSecond.ExpectData(primaryCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsTrue(expectRes.Command.SequenceEqual(primaryCmd));
            Assert.IsTrue(SecurityManagerFirst.SecurityManagerInfo.SpanTable.CheckNonceExists(PEER_NODE_ID_2));
            Assert.IsTrue(SecurityManagerSecond.SecurityManagerInfo.SpanTable.CheckNonceExists(PEER_NODE_ID_1));

            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_GET.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID)
            });
        }

        [Test]
        public void SendData_DiffersForNotMoreThanFiveNonceGenerations_DecryptsData()
        {
            // Arange.

            byte[] primaryCmd = CreateDataAsBasicSet();

            // Act.
            var tempToken = _ctrlSecond.ExpectData(primaryCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO);
            tempToken.WaitCompletedSignal();

            var spanContainer = SecurityManagerFirst.SecurityManagerInfo.SpanTable.GetContainer(PEER_NODE_ID_2);
            for (int i = 0; i < 4; i++)
            {
                spanContainer.NextNonce();
            }

            var expectToken = _ctrlSecond.ExpectData(primaryCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsTrue(expectRes.Command.SequenceEqual(primaryCmd));
            Assert.IsTrue(SecurityManagerFirst.SecurityManagerInfo.SpanTable.CheckNonceExists(PEER_NODE_ID_2));
            Assert.IsTrue(SecurityManagerSecond.SecurityManagerInfo.SpanTable.CheckNonceExists(PEER_NODE_ID_1));

            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_GET.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID)
            });
        }

        [Test]
        public void SendData_DiffersForMoreThanFiveNonceGenerations_SyncDevicesAndDecryptsData()
        {
            // Arange.

            byte[] primaryCmd = CreateDataAsBasicSet();

            // Act.
            var tempToken = _ctrlSecond.ExpectData(primaryCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO);
            tempToken.WaitCompletedSignal();

            var spanContainer = SecurityManagerFirst.SecurityManagerInfo.SpanTable.GetContainer(PEER_NODE_ID_2);
            for (int i = 0; i < 7; i++)
            {
                spanContainer.NextNonce();
            }

            var expectToken = _ctrlSecond.ExpectData(primaryCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsTrue(expectRes.Command.SequenceEqual(primaryCmd));
            Assert.IsTrue(SecurityManagerFirst.SecurityManagerInfo.SpanTable.CheckNonceExists(PEER_NODE_ID_2));
            Assert.IsTrue(SecurityManagerSecond.SecurityManagerInfo.SpanTable.CheckNonceExists(PEER_NODE_ID_1));

            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_GET.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID)
            });
        }

        [Test]
        public void SendData_DifferentKeys_SenderStopsDataSendingAfterOneRetransmission()
        {
            // Arange.

            byte[] primaryCmd = CreateDataAsBasicSet();

            // Act.
            var expectToken = _ctrlSecond.ExpectData(primaryCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO);
            expectToken.WaitCompletedSignal();

            var activeScheme = _ctrlFirst.Network.GetCurrentOrSwitchToHighestSecurityScheme(NODE_ID_2);
            SecurityManagerSecond.SecurityManagerInfo.SetTestNetworkKey(
                new byte[] { 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01 },
                activeScheme, false);

            SecurityManagerSecond.SecurityManagerInfo.ActivateNetworkKeyS2ForNode(new InvariantPeerNodeId(), activeScheme, false);

            var rTable = SecurityManagerFirst.SecurityManagerInfo.RetransmissionTableS2;
            Assert.IsFalse(rTable[PEER_NODE_ID_2].Counter <= 0);

            expectToken = _ctrlSecond.ExpectData(primaryCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsNull(expectRes.Command);
            Assert.IsFalse(rTable.ContainsKey(new InvariantPeerNodeId(NodeTag.Empty, NODE_ID_2)));
            Assert.IsFalse(SecurityManagerFirst.SecurityManagerInfo.SpanTable.CheckNonceExists(PEER_NODE_ID_2));
            Assert.IsTrue(SecurityManagerSecond.SecurityManagerInfo.SpanTable.CheckNonceExists(PEER_NODE_ID_1));
            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_GET.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID)
            });
        }

        [Test]
        public void SendData_DuplicatesBySequenceNumber_FrameDiscarded()
        {
            // Arange.
            byte[] primaryCmd = CreateDataAsBasicSet();

            // Act.
            var expectToken = _ctrlSecond.ExpectData(primaryCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            expectToken = _ctrlSecond.ExpectData(primaryCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO);
            expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            var nonceContainer = SecurityManagerFirst.SecurityManagerInfo.SpanTable.GetContainer(PEER_NODE_ID_2);
            var prevSequenceNumber = nonceContainer.TxSequenceNumber;
            var prevSpan = new byte[nonceContainer.Span.Length];
            Array.Copy(nonceContainer.Span, prevSpan, nonceContainer.Span.Length);

            SecurityManagerFirst.SecurityManagerInfo.SetTestSpanS2(prevSpan);
            SecurityManagerFirst.SecurityManagerInfo.SetTestSequenceNumberS2(prevSequenceNumber);

            expectToken = _ctrlSecond.ExpectData(primaryCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO);
            expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsFalse(expectRes);
            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_GET.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID)
            });
        }

        [Test]
        public void SendData_NonceReportAsS0Message_MessageEncapsulationNotSent()
        {
            // Arange.
            byte[] primaryCmd = CreateDataAsBasicSet();
            SecurityManagerSecond.SecurityManagerInfo.SetTestFrameCommandS2(SecurityS2TestFrames.NonceReport,
                new COMMAND_CLASS_SECURITY.SECURITY_NONCE_REPORT());

            // Act.
            var expectToken = _ctrlSecond.ExpectData(primaryCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsFalse(expectRes);
            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_GET.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY.SECURITY_NONCE_REPORT.ID)
            });
        }

        [Test]
        public void SendData_NonceReportDelayed_MessageEncapsulationNotSent()
        {
            // Arange.
            SendDataSecureS2Task.NONCE_REQUEST_TIMER = EXPECT_TIMEOUT;

            byte[] primaryCmd = CreateDataAsBasicSet();
            SecurityManagerSecond.SecurityManagerInfo.SetTestFrameDelayS2(SecurityS2TestFrames.NonceReport, EXPECT_TIMEOUT + 200);

            // Act.
            var expectToken = _ctrlSecond.ExpectData(primaryCmd, EXPECT_TIMEOUT + 300, null);
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsFalse(expectRes);
            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_GET.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID)
            });
        }

        [Test]
        public void SendData_NonceReportEncryptedWithTemp_MessageEncapsulationNotSent()
        {
            // Arange.
            _transport.MaxFrameSize = 100;

            byte[] primaryCmd = CreateDataAsBasicSet();
            SecurityManagerSecond.SecurityManagerInfo.SetTestFrameEncryptedS2(SecurityS2TestFrames.NonceReport, true);
            SecurityManagerSecond.SecurityManagerInfo.SetTestFrameNetworkKeyS2(SecurityS2TestFrames.NonceReport, null, true);

            // Act.
            var expectToken = _ctrlSecond.ExpectData(primaryCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsFalse(expectRes);
            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_GET.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID)
            });
        }

        [Test]
        public void SendData_NonceReportEncryptedWithS2Access_MessageEncapsulationNotSent()
        {
            // Arange.
            _transport.MaxFrameSize = 100;

            byte[] primaryCmd = CreateDataAsBasicSet();
            SecurityManagerSecond.SecurityManagerInfo.SetTestFrameEncryptedS2(SecurityS2TestFrames.NonceReport, true);
            SecurityManagerSecond.SecurityManagerInfo.SetTestFrameNetworkKeyS2(SecurityS2TestFrames.NonceReport, NKEY1_S2_C2, false);

            // Act.
            var expectToken = _ctrlSecond.ExpectData(primaryCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsFalse(expectRes);
            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_GET.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID)
            });
        }

        [Test]
        public void SendData_MessageEncapsulationCCMCipherObjectNotEncrypted_MessageWasntDecrypted()
        {
            // Arange.
            byte[] primaryCmd = CreateDataAsBasicSet();
            SecurityManagerFirst.SecurityManagerInfo.SetTestFrameEncryptedS2(SecurityS2TestFrames.MessageEncapsulation, false);

            // Act.
            var expectToken = _ctrlSecond.ExpectData(primaryCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsFalse(expectRes);
            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_GET.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
            });
        }

        [TestCase(0x13, true, true)]
        [TestCase(0x0D, false, true)]
        public void SendData_WithInvalidEncryptedExtension_FrameDiscarded(byte length, bool isMoreToFollow, bool isCritical)
        {
            // Arange.
            byte[] primaryCmd = CreateDataAsBasicSet();

            // Act.
            var expectToken = _ctrlSecond.ExpectData(primaryCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();
            SecurityManagerFirst.SecurityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.Mpan, ExtensionAppliedActions.Add)
            {
                IsEncrypted = true,
                IsEncryptedSpecified = true,
                ExtensionLength = length,
                ExtensionLengthSpecified = true,
                Value = new byte[] { 0x01, 0x23, 0xD9, 0xA2, 0xB7, 0xF1, 0xB0, 0x56, 0xF9, 0x84, 0x19, 0x7D, 0x5F, 0xB3, 0xBF, 0x15, 0xBA },
                ValueSpecified = true,
                IsCritical = isCritical,
                IsCriticalSpecified = true,
                IsMoreToFollow = isMoreToFollow,
                IsMoreToFollowSpecified = true
            });

            expectToken = _ctrlSecond.ExpectData(primaryCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO);
            expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsFalse(expectRes);
            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_GET.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID)
            });

            SecurityManagerFirst.SecurityManagerInfo.ClearTestExtensionsS2();
        }

        [TestCase(ExtensionTypes.Mos)]
        [TestCase(ExtensionTypes.MpanGrp)]
        [TestCase(ExtensionTypes.Span)]
        [TestCase(ExtensionTypes.Test)]
        public void SendData_WithInvalidEncryptedExtensionType_FrameDiscarded(ExtensionTypes extensionType)
        {
            // Arange.
            byte[] primaryCmd = CreateDataAsBasicSet();

            // Act.
            var expectToken = _ctrlSecond.ExpectData(primaryCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            SecurityManagerFirst.SecurityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, extensionType, ExtensionAppliedActions.Add)
            {
                IsEncrypted = true,
                IsEncryptedSpecified = true,
                IsCritical = true,
                IsCriticalSpecified = true,
                IsMoreToFollow = false,
                IsMoreToFollowSpecified = true
            });

            expectToken = _ctrlSecond.ExpectData(primaryCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO);
            expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsFalse(expectRes);
            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_GET.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID)
            });

            SecurityManagerFirst.SecurityManagerInfo.ClearTestExtensionsS2();
        }

        [Test]
        public void SendData_WithInvalidExtensionType_FrameDiscarded()
        {
            // Arange.
            byte[] primaryCmd = CreateDataAsBasicSet();

            // Act.
            var expectToken = _ctrlSecond.ExpectData(primaryCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            SecurityManagerFirst.SecurityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.Test, ExtensionAppliedActions.Add)
            {
                IsEncrypted = false,
                IsEncryptedSpecified = true,
                ExtensionLength = 0x05,
                ExtensionLengthSpecified = true,
                Value = new byte[] { 0x01, 0x01, 0x23 },
                ValueSpecified = true,
                IsCritical = true,
                IsCriticalSpecified = true,
                IsMoreToFollow = false,
                IsMoreToFollowSpecified = true
            });

            expectToken = _ctrlSecond.ExpectData(primaryCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO);
            expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsFalse(expectRes);
            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_GET.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID)
            });

            SecurityManagerFirst.SecurityManagerInfo.ClearTestExtensionsS2();
        }

        [TestCase(ExtensionTypes.Test, null)]
        public void SendData_WithInvalidExtensionIsCriticalFlag_FrameDiscarded(ExtensionTypes type, byte[] value)
        {
            // Arange.
            byte[] primaryCmd = CreateDataAsBasicSet();

            // Act.
            var expectToken = _ctrlSecond.ExpectData(primaryCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            bool isCritical = true;
            SecurityManagerFirst.SecurityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, type, ExtensionAppliedActions.Add)
            {
                IsEncrypted = false,
                IsEncryptedSpecified = true,
                Value = value,
                ValueSpecified = true,
                IsCritical = isCritical,
                IsCriticalSpecified = true,
                IsMoreToFollow = false,
                IsMoreToFollowSpecified = true
            });

            expectToken = _ctrlSecond.ExpectData(primaryCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO);
            expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsFalse(expectRes);
            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_GET.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID)
            });

            SecurityManagerFirst.SecurityManagerInfo.ClearTestExtensionsS2();
        }

        [Test]
        public void SendData_MpanGrpExtensionWithIsCriticalFlagSetToZero_FrameNotDiscarded()
        {
            // Arange.
            byte[] primaryCmd = CreateDataAsBasicSet();

            // Act.
            var expectToken = _ctrlSecond.ExpectData(primaryCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            SecurityManagerFirst.SecurityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.MpanGrp, ExtensionAppliedActions.Add)
            {
                IsEncrypted = false,
                IsEncryptedSpecified = true,
                Value = new byte[] { 0x55 },
                ValueSpecified = true,
                IsCritical = false,
                IsCriticalSpecified = true,
                IsMoreToFollow = false,
                IsMoreToFollowSpecified = true
            });

            var waitSecondFrameTokenReady = new System.Threading.AutoResetEvent(false);
            ZWave.ActionToken expectToken2 = null;
            var expectToken1 = _ctrlSecond.ExpectData(NODE_ID_1, primaryCmd, new ExtensionTypes[] { ExtensionTypes.MpanGrp }, 1000, action =>
            {
                expectToken2 = _ctrlSecond.ExpectData(NODE_ID_1, primaryCmd, new ExtensionTypes[] { ExtensionTypes.MpanGrp }, 1000, null);
                waitSecondFrameTokenReady.Set();
            });
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO);

            // Assert.
            var expectRes1 = (ExpectDataResult)expectToken1.WaitCompletedSignal();
            waitSecondFrameTokenReady.WaitOne();
            var expectRes2 = (ExpectDataResult)expectToken2.WaitCompletedSignal();
            Assert.IsTrue(expectRes1);
            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_GET.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID), // NOT discarded frame.
                //// Reaction to not expected frame.
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID)
            });

            SecurityManagerFirst.SecurityManagerInfo.ClearTestExtensionsS2();
        }

        [TestCase(ExtensionTypes.Span, new byte[] { 0x01, 0x23, 0xD9, 0xA2, 0xB7, 0xF1, 0xB0, 0x56, 0xF9, 0x84, 0x19, 0x7D, 0x5F, 0xB3, 0xBF, 0x15 }, 0x05)]
        [TestCase(ExtensionTypes.MpanGrp, new byte[] { 0x55 }, 0x05)]
        [TestCase(ExtensionTypes.Mos, null, 0x05)]
        public void SendData_WithInvalidExtensionLength_FrameDiscarded(ExtensionTypes type, byte[] value, byte length)
        {
            // Arange.
            byte[] primaryCmd = CreateDataAsBasicSet();

            // Act.
            var expectToken = _ctrlSecond.ExpectData(primaryCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            bool isCritical = type != ExtensionTypes.Mos;
            SecurityManagerFirst.SecurityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, type, ExtensionAppliedActions.Add)
            {
                IsEncrypted = false,
                IsEncryptedSpecified = true,
                ExtensionLength = length,
                ExtensionLengthSpecified = true,
                Value = value,
                ValueSpecified = true,
                IsCritical = isCritical,
                IsCriticalSpecified = true,
                IsMoreToFollow = false,
                IsMoreToFollowSpecified = true
            });

            expectToken = _ctrlSecond.ExpectData(primaryCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO);
            expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsFalse(expectRes);
            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_GET.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID)
            });

            SecurityManagerFirst.SecurityManagerInfo.ClearTestExtensionsS2();
        }

        [Test]
        public void SendData_WithUnencryptedMpanExtensionType_FrameDiscarded()
        {
            // Arange.
            byte[] primaryCmd = CreateDataAsBasicSet();

            // Act.
            var expectToken = _ctrlSecond.ExpectData(primaryCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            SecurityManagerFirst.SecurityManagerInfo.AddTestExtensionS2(new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.Mpan, ExtensionAppliedActions.Add)
            {
                IsEncrypted = false,
                IsEncryptedSpecified = true
            });

            expectToken = _ctrlSecond.ExpectData(primaryCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO);
            expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsFalse(expectRes);
            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_GET.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID)
            });

            SecurityManagerFirst.SecurityManagerInfo.ClearTestExtensionsS2();
        }

        [Test]
        public void SendData_EncryptedSpan_Add_MessageReceived()
        {
            // Arrange
            SyncSpan();
            SecurityManagerFirst.SecurityManagerInfo.ClearTestExtensionsS2();
            SecurityManagerFirst.SecurityManagerInfo.AddTestExtensionS2(
                new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.Span, ExtensionAppliedActions.Add)
                {
                    Value = new byte[16],
                    IsEncrypted = true,
                    NumOfUsage = 1
                });

            // Act
            var expectToken = _ctrlSecond.ExpectData(new COMMAND_CLASS_BASIC.BASIC_GET(), EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2,
                new COMMAND_CLASS_BASIC.BASIC_GET(),
                TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();
            // Assert
            Assert.IsTrue(expectRes);
        }

        [Test]
        public void SendData_EncryptedSpan_AddOrModify_MessageReceived()
        {
            // Arrange
            SyncSpan();
            SecurityManagerFirst.SecurityManagerInfo.AddTestExtensionS2(
                new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.Span, ExtensionAppliedActions.AddOrModify)
                {
                    Value = new byte[16],
                    IsEncrypted = true,
                    NumOfUsage = 1
                });

            // Act
            var expectToken = _ctrlSecond.ExpectData(new COMMAND_CLASS_BASIC.BASIC_GET(), EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2,
                new COMMAND_CLASS_BASIC.BASIC_GET(),
                TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();
            // Assert
            Assert.IsTrue(expectRes);
        }

        [Test]
        public void SendData_SpanNotEncrypted_MoreToFollowDisabled_AddOrModify_MessageReceived()
        {
            // Arrange
            SyncSpan();
            SecurityManagerFirst.SecurityManagerInfo.AddTestExtensionS2(
                new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.Span, ExtensionAppliedActions.AddOrModify)
                {
                    Value = new byte[16],
                    IsEncrypted = false,
                    NumOfUsage = 1,
                    IsMoreToFollow = false
                });

            // Act
            var expectToken = _ctrlSecond.ExpectData(new COMMAND_CLASS_BASIC.BASIC_GET(), EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2,
                new COMMAND_CLASS_BASIC.BASIC_GET(),
                TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();
            // Assert
            Assert.IsTrue(expectRes);
        }

        [Test]
        public void SendData_SpanNotEncrypted_MoreToFollowDisabled_Add_MessageReceived()
        {
            // Arrange
            SyncSpan();
            SecurityManagerFirst.SecurityManagerInfo.ClearTestExtensionsS2();
            SecurityManagerFirst.SecurityManagerInfo.AddTestExtensionS2(
                new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.Span, ExtensionAppliedActions.Add)
                {
                    Value = new byte[16],
                    IsEncrypted = false,
                    NumOfUsage = 1,
                    IsMoreToFollow = false
                });

            // Act
            var expectToken = _ctrlSecond.ExpectData(new COMMAND_CLASS_BASIC.BASIC_GET(), EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2,
                new COMMAND_CLASS_BASIC.BASIC_GET(),
                TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();
            // Assert
            Assert.IsTrue(expectRes);
        }

        //[Test]
        public void SendData_SpanNotEncryptedIsMoreToFollow_TestCriticalDisabledMoreToFollowDisabledEncryptedDisabled_MessageReceived()
        {
            // Arrange
            SyncSpan();
            SecurityManagerFirst.SecurityManagerInfo.AddTestExtensionS2(
                new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.Span, ExtensionAppliedActions.AddOrModify)
                {
                    Value = new byte[16],
                    IsMoreToFollow = true,
                    NumOfUsage = 1,
                    IsEncrypted = false
                });
            SecurityManagerFirst.SecurityManagerInfo.AddTestExtensionS2(
                new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.Test, ExtensionAppliedActions.Add)
                {
                    Value = new byte[16],
                    IsCritical = false,
                    IsMoreToFollow = false,
                    NumOfUsage = 1,
                    IsEncrypted = false
                });

            // Act
            var expectToken = _ctrlSecond.ExpectData(new COMMAND_CLASS_BASIC.BASIC_GET(), EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2,
                new COMMAND_CLASS_BASIC.BASIC_GET(),
                TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();
            // Assert
            Assert.IsTrue(expectRes);
        }

        private byte[] FakeGetPersonalizationData()
        {
            return new byte[] { 0xce, 0xbb, 0x8c, 0xce, 0xb3, 0x07, 0x14, 0x7e,
                0xa6, 0x4b, 0x57, 0xc0, 0x9f, 0x11, 0xea, 0x9d,
                0x40, 0x43, 0x8b, 0xeb, 0x49, 0xc6, 0x97, 0xb5,
                0x3b, 0xe4, 0xae, 0xce, 0x69, 0x9e, 0x99, 0x3e};
        }

        private void SyncSpan()
        {
            var versionGetCmd = CreateDataAsVersionGet();
            // We're assuming it works - not actually a part of unit test. Need to be moved to plain nonce instantiation.
            var expectVersinGetToken2 = _ctrlSecond.ExpectData(NODE_ID_1, versionGetCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2, versionGetCmd, TXO);
            var expectRes2 = (ExpectDataResult)expectVersinGetToken2.WaitCompletedSignal();
        }

        private byte[] CreateDataAsVersionGet()
        {
            return new COMMAND_CLASS_VERSION.VERSION_GET();
        }
    }
}
