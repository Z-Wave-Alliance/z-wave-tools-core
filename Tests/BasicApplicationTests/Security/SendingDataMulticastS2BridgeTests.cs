/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using NUnit.Framework;
using System.Linq;
using ZWave.Enums;
using ZWave.BasicApplication.Operations;
using System.Threading;
using ZWave.CommandClasses;
using ZWave.BasicApplication;
using ZWave.BasicApplication.Devices;
using ZWave.Configuration;
using ZWave.BasicApplication.Security;
using ZWave.Security;
using System.Collections.Generic;
using Utils.UI;
using ZWave.Layers;
using ZWave.Devices;

namespace BasicApplicationTests.Security
{
    [TestFixture]
    [Ignore("not verified")]
    public class SendingDataMulticastS2BridgeTests : TestBase
    {
        #region Hardcodes
        private const int FAKE_MAX_FRAME_SIZE = 200;
        private readonly byte[] fakeMpanState = new byte[] { 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00 };

        private const int MPAN_SYNC_TIMEOUT = 22;
        #endregion

        protected new BridgeController _ctrlFirst;
        private SecurityManagerInfo _smiFirst;
        private SecurityManagerInfo _smiSecond;
        private SecurityManagerInfo _smiThird;

        [SetUp]
        public void Setup()
        {
            _ctrlFirst = _app.CreateBridgeController();
            _ctrlFirst.Connect(new SerialPortDataSource("COM1"));
            _ctrlFirst.MemoryGetId();

            _transport.MaxFrameSize = FAKE_MAX_FRAME_SIZE;
            _transport.SetUpModulesNetwork(_ctrlFirst.SessionId, _ctrlSecond.SessionId, _ctrlThird.SessionId);
            _ctrlFirst.MemoryGetId();
            _ctrlSecond.MemoryGetId();
            _ctrlThird.MemoryGetId();

            _ctrlFirst.SessionClient.AddSubstituteManager(new SupervisionManager(new NetworkViewPoint(), (x, y) => true));
            _ctrlSecond.SessionClient.AddSubstituteManager(new SupervisionManager(new NetworkViewPoint(), (x, y) => true));
            _ctrlThird.SessionClient.AddSubstituteManager(new SupervisionManager(new NetworkViewPoint(), (x, y) => true));


            SetUpSecurity(_ctrlFirst, NKEY1_S2_C0, NKEY1_S2_C1, NKEY1_S2_C2, NKEY1_S0);
            SetUpSecurity(_ctrlSecond, NKEY2_S2_C0, NKEY2_S2_C1, NKEY2_S2_C2, NKEY2_S0);
            SetUpSecurity(_ctrlThird, NKEY3_S2_C0, NKEY3_S2_C1, NKEY3_S2_C2, NKEY3_S0);

            _ctrlFirst.Network.IsEnabledS0 = true;
            _ctrlFirst.Network.IsEnabledS2_ACCESS = true;
            _ctrlFirst.Network.IsEnabledS2_AUTHENTICATED = true;
            _ctrlFirst.Network.IsEnabledS2_UNAUTHENTICATED = true;
            _ctrlSecond.Network.IsEnabledS0 = true;
            _ctrlSecond.Network.IsEnabledS2_ACCESS = true;
            _ctrlSecond.Network.IsEnabledS2_AUTHENTICATED = true;
            _ctrlSecond.Network.IsEnabledS2_UNAUTHENTICATED = true;
            _ctrlThird.Network.IsEnabledS0 = true;
            _ctrlThird.Network.IsEnabledS2_ACCESS = true;
            _ctrlThird.Network.IsEnabledS2_AUTHENTICATED = true;
            _ctrlThird.Network.IsEnabledS2_UNAUTHENTICATED = true;

            _smiFirst = ((SecurityManager)_ctrlFirst.SessionClient.GetSubstituteManager(typeof(SecurityManager))).SecurityManagerInfo;
            _smiSecond = ((SecurityManager)_ctrlSecond.SessionClient.GetSubstituteManager(typeof(SecurityManager))).SecurityManagerInfo;
            _smiThird = ((SecurityManager)_ctrlThird.SessionClient.GetSubstituteManager(typeof(SecurityManager))).SecurityManagerInfo;

            _ctrlFirst.Network.SetSecuritySchemes(NODE_ID_2, SecuritySchemeSet.ALL);
            _ctrlFirst.Network.SetSecuritySchemes(NODE_ID_3, SecuritySchemeSet.ALL);
        }

        [Test]
        public void A_NonSecureSendMulticastData_VersionReportCmd_ReceiversGetData()
        {
            // Arrange.
            _ctrlFirst.SessionClient.ClearSubstituteManagers();
            _ctrlSecond.SessionClient.ClearSubstituteManagers();
            _ctrlThird.SessionClient.ClearSubstituteManagers();
            var expectedCmd = CreateDataAsVersionWakeUpNotify();
            var expectToken2 = _ctrlSecond.ExpectData(NODE_ID_1, expectedCmd, EXPECT_TIMEOUT, null);
            var expectToken3 = _ctrlThird.ExpectData(NODE_ID_1, expectedCmd, EXPECT_TIMEOUT, null);

            // Act.
            _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2, NODE_ID_3 }, expectedCmd, TXO);

            var expectRes2 = (ExpectDataResult)expectToken2.WaitCompletedSignal();
            var expectRes3 = (ExpectDataResult)expectToken3.WaitCompletedSignal();

            // Assert.
            Assert.IsTrue(expectRes2);
            Assert.IsTrue(expectRes3);
            Assert.IsTrue(expectRes2.Command.SequenceEqual(expectedCmd));
            Assert.IsTrue(expectRes3.Command.SequenceEqual(expectedCmd));
        }

        [Test]
        public void B_SendBroadcastData_BroadcastMessageSent_MpanGroupsAreCreated()
        {
            // Arrange.
            var wakeUpNotifyCmd = CreateDataAsVersionWakeUpNotify();
            var supervisionGetCmd = CreateDataAsSupervisionGet();

            // Act.
            // 1. Sync MPAN between sender and receivers.
            SyncSpan();

            _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2,/* NODE_ID_3*/ }, wakeUpNotifyCmd, TXO);

            Thread.Sleep(MPAN_SYNC_TIMEOUT);

            // 2. Get groups from devices
            var groupsOnSender = _smiFirst.MpanTable.SelectGroupIds(NODE_ID_1);
            var groupsOnReceiver = _smiSecond.MpanTable.SelectGroupIds(NODE_ID_1);

            // Assert.
            Assert.NotNull(groupsOnSender);
            Assert.AreEqual(1, groupsOnSender.Length);
            Assert.NotNull(groupsOnReceiver);
            Assert.AreEqual(1, groupsOnReceiver.Length);
        }

        [Test]
        public void C_SendBroadcastData_BroadcastMessageSent_MpanGroupsAreInSync()
        {
            // Arrange.
            var wakeUpNotifyCmd = CreateDataAsVersionWakeUpNotify();
            var supervisionGetCmd = CreateDataAsSupervisionGet();

            // Act.
            // 1. Sync MPAN between sender and receivers.
            SyncSpan();

            _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2,/* NODE_ID_3*/ }, wakeUpNotifyCmd, TXO);

            Thread.Sleep(MPAN_SYNC_TIMEOUT);

            // 2. Get groups from devices
            var groupsOnSender = _smiFirst.MpanTable.SelectGroupIds(NODE_ID_1);
            var groupsOnReceiver = _smiSecond.MpanTable.SelectGroupIds(NODE_ID_1);

            // Assert.
            Assert.IsFalse(_smiFirst.MpanTable.IsRecordInMOSState(new NodeGroupId(NODE_ID_1, groupsOnSender[0])));
            Assert.IsFalse(_smiSecond.MpanTable.IsRecordInMOSState(new NodeGroupId(NODE_ID_1, groupsOnReceiver[0])));
        }

        [Test]
        [Ignore("")]
        public void D_SendBroadcastData_NoncesAreOutOfSync_BroadcastMessageDecrypted()
        {
            // Arrange.
            var wakeUpNotifyCmd = CreateDataAsVersionWakeUpNotify();
            var versionGetCmd = CreateDataAsVersionGet();
            var supervisionGetCmd = CreateDataAsSupervisionGet();


            // Act.
            // 1. Sync MPAN and SPAN using followup between sender and receiver.
            _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2,/* NODE_ID_3*/ }, wakeUpNotifyCmd, TXO);

            Thread.Sleep(MPAN_SYNC_TIMEOUT);

            var expectVersinGetToken2 = _ctrlSecond.ExpectData(NODE_ID_1, versionGetCmd, EXPECT_TIMEOUT, null);

            // 2. Send multicast. Receivers MUST get and decrypt it.
            _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2 /*, NODE_ID_3*/ }, versionGetCmd, TXO);

            var expectRes2 = (ExpectDataResult)expectVersinGetToken2.WaitCompletedSignal();

            // Assert.
            Assert.IsTrue(expectRes2);
            Assert.IsTrue(expectRes2.Command.SequenceEqual(versionGetCmd));
        }

        [Test]
        public void SendBroadcastData_SpanIsSynchronized_BroadcastMessageDecrypted()
        {
            // Arrange.
            var wakeUpNotifyCmd = CreateDataAsVersionWakeUpNotify();
            var supervisionGetCmd = CreateDataAsSupervisionGet();

            // Act.
            // 1. Sync MPAN between sender and receivers
            SyncSpan();
            _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2,/* NODE_ID_3*/ }, wakeUpNotifyCmd, TXO);

            Thread.Sleep(MPAN_SYNC_TIMEOUT);

            var expectVersinGetToken2 = _ctrlSecond.ExpectData(NODE_ID_1, wakeUpNotifyCmd, EXPECT_TIMEOUT, null);
            // 2. Send multicast. Receivers MUST get and decrypt it.
            _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2 /*, NODE_ID_3*/ }, wakeUpNotifyCmd, TXO);
            var expectRes2 = (ExpectDataResult)expectVersinGetToken2.WaitCompletedSignal();

            // Assert.
            Assert.IsTrue(expectRes2);
            Assert.IsTrue(expectRes2.Command.SequenceEqual(wakeUpNotifyCmd));
        }

        [Test]
        public void SendBroadcastData_SenderSetFakeMpan_Resynchronized()
        {
            // Arrange.
            var wakeUpNotifyCmd = CreateDataAsVersionWakeUpNotify();
            var supervisionGetCmd = CreateDataAsSupervisionGet();

            // Act.
            // 1. Sync MPAN between sender and receivers.
            SyncSpan();

            _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2,/* NODE_ID_3*/ }, wakeUpNotifyCmd, TXO);

            Thread.Sleep(MPAN_SYNC_TIMEOUT);

            // 2. Check MPAN exists on both sides.
            var groupsOnSender = _smiFirst.MpanTable.SelectGroupIds(NODE_ID_1);
            var groupsOnReceiver = _smiSecond.MpanTable.SelectGroupIds(NODE_ID_1);

            // 3. Set fake MPAN on sender
            var senderMpanTable = _smiFirst.MpanTable.GetContainer(new NodeGroupId(NODE_ID_1, groupsOnSender[0]));
            senderMpanTable.SetMpanState(fakeMpanState);

            // 4. Send Multicast receiver sets MOS
            _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2,/* NODE_ID_3*/ }, wakeUpNotifyCmd, TXO);

            Thread.Sleep(MPAN_SYNC_TIMEOUT);

            // Assert.
            Assert.IsFalse(_smiSecond.MpanTable.IsRecordInMOSState(new NodeGroupId(NODE_ID_1, groupsOnReceiver[0])));
        }

        [Test]
        public void SendBroadcastData_SenderResetSpan_ReceiverDoesntSetMos()
        {
            // Arrange.
            var wakeUpNotifyCmd = CreateDataAsVersionWakeUpNotify();
            var supervisionGetCmd = CreateDataAsSupervisionGet();

            // Act.
            // 1. Sync MPAN between sender and receivers.
            SyncSpan();

            _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2,/* NODE_ID_3*/ }, wakeUpNotifyCmd, TXO);

            Thread.Sleep(MPAN_SYNC_TIMEOUT);

            // 2. Check MPAN exists on both sides.
            var groupsOnSender = _smiFirst.MpanTable.SelectGroupIds(NODE_ID_1);
            var groupsOnReceiver = _smiSecond.MpanTable.SelectGroupIds(NODE_ID_1);

            // 3. Reset SPAN on sender
            _smiFirst.SpanTable.SetNonceFree(PEER_NODE_ID_2);

            // 4. Send Multicast
            _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2,/* NODE_ID_3*/ }, wakeUpNotifyCmd, TXO);

            Thread.Sleep(MPAN_SYNC_TIMEOUT);

            // Assert.
            Assert.IsFalse(_smiSecond.MpanTable.IsRecordInMOSState(new NodeGroupId(NODE_ID_1, groupsOnReceiver[0])));
        }

        [Test]
        public void SendBroadcastData_ReceiverResetSpan_ReceiverDoesntSetMos()
        {
            // Arrange.
            var wakeUpNotifyCmd = CreateDataAsVersionWakeUpNotify();
            var supervisionGetCmd = CreateDataAsSupervisionGet();

            // Act.
            // 1. Sync MPAN between sender and receivers.
            SyncSpan();

            _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2,/* NODE_ID_3*/ }, wakeUpNotifyCmd, TXO);

            Thread.Sleep(MPAN_SYNC_TIMEOUT);

            // 2. Check MPAN exists on both sides.
            var groupsOnSender = _smiFirst.MpanTable.SelectGroupIds(NODE_ID_1);
            var groupsOnReceiver = _smiSecond.MpanTable.SelectGroupIds(NODE_ID_1);

            // 3. Reset SPAN on receiver
            _smiSecond.SpanTable.SetNonceFree(PEER_NODE_ID_1);

            // 4. Send Multicast
            _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2,/* NODE_ID_3*/ }, wakeUpNotifyCmd, TXO);

            Thread.Sleep(MPAN_SYNC_TIMEOUT);

            // Assert.
            Assert.IsFalse(_smiSecond.MpanTable.IsRecordInMOSState(new NodeGroupId(NODE_ID_1, groupsOnReceiver[0])));
        }

        [Test]
        public void SendBroadcastData_ReceiverSetFakeMpan_Resynchronized()
        {
            // Arrange.
            var wakeUpNotifyCmd = CreateDataAsVersionWakeUpNotify();
            var supervisionGetCmd = CreateDataAsSupervisionGet();

            // Act.
            // 1. Sync MPAN between sender and receivers.
            SyncSpan();

            _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2,/* NODE_ID_3*/ }, wakeUpNotifyCmd, TXO);

            Thread.Sleep(MPAN_SYNC_TIMEOUT);

            // 2. Check MPAN exists on both sides.
            var groupsOnSender = _smiFirst.MpanTable.SelectGroupIds(NODE_ID_1);
            var groupsOnReceiver = _smiSecond.MpanTable.SelectGroupIds(NODE_ID_1);

            // 3. Remove record from MPAN on receiver
            var receiverMpanTable = _smiSecond.MpanTable.GetContainer(new NodeGroupId(NODE_ID_1, groupsOnReceiver[0]));
            receiverMpanTable.SetMpanState(fakeMpanState);

            // 4. Send Multicast
            _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2,/* NODE_ID_3*/ }, wakeUpNotifyCmd, TXO);

            Thread.Sleep(MPAN_SYNC_TIMEOUT);

            // Assert.
            Assert.IsFalse(_smiSecond.MpanTable.IsRecordInMOSState(new NodeGroupId(NODE_ID_1, groupsOnReceiver[0])));
        }

        [Test]
        public void SendBroadcastData_ReceiverClearesSpan_ReceiverSentSos()
        {
            // Arrange.
            var wakeUpNotifyCmd = CreateDataAsVersionWakeUpNotify();
            var supervisionGetCmd = CreateDataAsSupervisionGet();

            // Act.
            SyncSpan();
            // Sync MPAN
            _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2, }, wakeUpNotifyCmd, TXO);

            Thread.Sleep(MPAN_SYNC_TIMEOUT);

            //2. Out of sync both
            _smiSecond.SpanTable.SetNonceFree(PEER_NODE_ID_1);

            var expectNonceReportToken = _ctrlFirst.ExpectData(NODE_ID_2, new COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT(), EXPECT_TIMEOUT, null);

            _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2, }, wakeUpNotifyCmd, TXO);

            Thread.Sleep(MPAN_SYNC_TIMEOUT);

            // Assert.
            var expectNonceReportRes = (ExpectDataResult)expectNonceReportToken.WaitCompletedSignal();
            //Assert.IsTrue(expectNonceReportRes);

            //var nonceReportCC = (COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT)expectNonceReportRes.Command;
            //Assert.IsTrue(nonceReportCC.properties1.mos == 0);
            //Assert.IsTrue(nonceReportCC.properties1.sos > 0);
        }

        [Test]
        public void SendBroadcastData_MpanShiftedForFiveGenerationsAhead_Resynchronized()
        {
            // Arrange.
            var wakeUpNotifyCmd = CreateDataAsVersionWakeUpNotify();
            var supervisionGetCmd = CreateDataAsSupervisionGet();

            // Act.
            SyncSpan();
            // Sync MPAN.
            _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2, }, wakeUpNotifyCmd, TXO);

            Thread.Sleep(MPAN_SYNC_TIMEOUT);

            //2. Shift MPAN for five generations ahead.

            _ctrlSecond.SetRFReceiveMode(0);

            for (int i = 0; i < 2; i++) // MPAN is incremented for broadcast as well as for follow-up singlecast so it will be 4 generations ahead.
            {
                _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2, }, wakeUpNotifyCmd, TXO);

                Thread.Sleep(MPAN_SYNC_TIMEOUT);
            }

            _ctrlSecond.SetRFReceiveMode(1);

            var expectNonceReportToken = _ctrlFirst.ExpectData(NODE_ID_2, new COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT(), EXPECT_TIMEOUT, null);
            _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2, }, wakeUpNotifyCmd, TXO);

            Thread.Sleep(MPAN_SYNC_TIMEOUT);

            // Assert.
            var expectNonceReportRes = (ExpectDataResult)expectNonceReportToken.WaitCompletedSignal();
            Assert.IsFalse(expectNonceReportRes); // Receiver MUST NOT send nonce report.
            var groupsOnReceiver = _smiSecond.MpanTable.SelectGroupIds(NODE_ID_1);
            Assert.IsNotNull(groupsOnReceiver);
            Assert.IsTrue(groupsOnReceiver.Length == 1);
            Assert.IsFalse(_smiSecond.MpanTable.IsRecordInMOSState(new NodeGroupId(NODE_ID_1, groupsOnReceiver[0])));
        }

        [Test]
        public void SendBroadcastData_ReceiverSetNoncesOutOfSync_ReceiverSentMosAndSos()
        {
            // Arrange.
            var wakeUpNotifyCmd = CreateDataAsVersionWakeUpNotify();
            var supervisionGetCmd = CreateDataAsSupervisionGet();
            //GetSecurityManager(_ctrlFirst).SerialApiVersion = 8;
            //GetSecurityManager(_ctrlSecond).SerialApiVersion = 8;
            // Act.
            SyncSpan();
            // Sync MPAN
            _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2, }, wakeUpNotifyCmd, TXO);

            Thread.Sleep(MPAN_SYNC_TIMEOUT);

            //2. Out of sync both
            var groupsOnReceiver = _smiSecond.MpanTable.SelectGroupIds(NODE_ID_1);
            _smiSecond.MpanTable.GetContainer(new NodeGroupId(NODE_ID_1, groupsOnReceiver[0])).SetMosState(true);
            _smiSecond.SpanTable.SetNonceFree(PEER_NODE_ID_1);
            var expectNonceReportToken = _ctrlFirst.ExpectData(NODE_ID_2, new COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT(), EXPECT_TIMEOUT, null);

            _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2, }, wakeUpNotifyCmd, TXO);

            Thread.Sleep(MPAN_SYNC_TIMEOUT);

            var expectNonceReportRes = (ExpectDataResult)expectNonceReportToken.WaitCompletedSignal();

            // Assert.
            Assert.IsTrue(expectNonceReportRes);
            var nonceReportCC = (COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT)expectNonceReportRes.Command;

            Assert.IsTrue(nonceReportCC.properties1.mos > 0);
            Assert.IsTrue(nonceReportCC.properties1.sos > 0);
        }

        [Test]
        public void SendBroadcastData_ReplaceMpanGrpExtensionWithTestExtension_FrameDiscarded()
        {
            // Arrange.
            var wakeUpNotifyCmd = CreateDataAsVersionWakeUpNotify();

            // Act.
            SyncSpan();
            _smiFirst.AddTestExtensionS2(
                new TestExtensionS2Settings
                {
                    Counter = new ValueEntity<int>(),
                    MessageTypeV = MessageTypes.MulticastAll,
                    ExtensionTypeV = ExtensionTypes.MpanGrp,
                    ActionV = ExtensionAppliedActions.Delete,
                });
            _smiFirst.AddTestExtensionS2(
                new TestExtensionS2Settings(MessageTypes.MulticastAll, ExtensionTypes.Test, ExtensionAppliedActions.Add)
                {
                    Value = new byte[1],
                    IsEncrypted = false,
                    IsCritical = true,
                    ExtensionLength = 3,
                    NumOfUsage = 1
                });
            _smiFirst.AddTestExtensionS2(
                new TestExtensionS2Settings(MessageTypes.SinglecastWithMpanGrp, ExtensionTypes.MpanGrp, ExtensionAppliedActions.Delete));
            _smiFirst.AddTestExtensionS2(
                new TestExtensionS2Settings(MessageTypes.SinglecastWithMpanGrp, ExtensionTypes.Test, ExtensionAppliedActions.Add)
                {
                    Value = new byte[1],
                    IsEncrypted = false,
                    IsCritical = true,
                    ExtensionLength = 3,
                    NumOfUsage = 1
                });
            var expectVersinGetToken = _ctrlSecond.ExpectData(NODE_ID_1, wakeUpNotifyCmd, EXPECT_TIMEOUT, null);

            _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2,/* NODE_ID_3*/ }, wakeUpNotifyCmd, TXO);

            var groupIds = _smiSecond.MpanTable.SelectGroupIds(NODE_ID_1);

            // Assert.
            var res = expectVersinGetToken.WaitCompletedSignal();
            Assert.IsFalse(res);
            Assert.AreEqual(0, groupIds.Length);
        }

        [Test]
        public void SendBroadcastData_AfterMosReportedGot_S2_SC_WithoutMPAN_MpanRecordRemovedFromTable() // [CC:009F.01.00.11.099]
        {
            // Arrange.
            var wakeUpNotifyCmd = CreateDataAsVersionWakeUpNotify();

            // Act.
            // 1. Sync MPAN between sender and receivers.
            SyncSpan();

            _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2,/* NODE_ID_3*/ }, wakeUpNotifyCmd, TXO);

            Thread.Sleep(MPAN_SYNC_TIMEOUT);

            // 2. Set MPAN record to MOS state.
            var groupId = _smiSecond.MpanTable.SelectGroupIds(NODE_ID_1)[0];
            var container = _smiSecond.MpanTable.GetContainer(new NodeGroupId(NODE_ID_1, groupId));
            container.SetMosState(true);

            // Remove all MPAN extensions from frames.
            _smiFirst.AddTestExtensionS2(
                new TestExtensionS2Settings(MessageTypes.SinglecastWithMpan, ExtensionTypes.Mpan, ExtensionAppliedActions.Delete));

            // S2 SC-F -->
            // Nonce Report MOS = 1 <--
            // S2 SC without MPAN extension -->

            _smiFirst.AddTestExtensionS2(
                new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.MpanGrp, ExtensionAppliedActions.Add)
                {
                    Value = new byte[] { groupId },
                    ValueSpecified = true,
                    NumOfUsage = 1,
                    NumOfUsageSpecified = true
                });
            _ctrlFirst.SendData(NODE_ID_2, wakeUpNotifyCmd, TXO);
            Thread.Sleep(EXPECT_TIMEOUT);

            // Assert.
            Assert.IsFalse(_smiSecond.MpanTable.CheckMpanExists(new NodeGroupId(NODE_ID_1, groupId)));
        }

        [Test]
        public void SendBroadcastData_AfterMosReportedGot_S2_SC_F_MpanRecordRemainsInTable() // [CC:009F.01.00.11.099]
        {
            // Arrange.
            var wakeUpNotifyCmd = CreateDataAsVersionWakeUpNotify();

            // Act.
            // 1. Sync MPAN between sender and receivers.
            SyncSpan();

            _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2,/* NODE_ID_3*/ }, wakeUpNotifyCmd, TXO);

            Thread.Sleep(MPAN_SYNC_TIMEOUT);

            // 2. Set MPAN record to MOS state.
            var groupId = _smiSecond.MpanTable.SelectGroupIds(NODE_ID_1)[0];
            var container = _smiSecond.MpanTable.GetContainer(new NodeGroupId(NODE_ID_1, groupId));
            container.SetMosState(true);

            // Remove all MPAN extensions from frames.
            _smiFirst.AddTestExtensionS2(
                new TestExtensionS2Settings(MessageTypes.SinglecastWithMpan, ExtensionTypes.Mpan, ExtensionAppliedActions.Delete));

            // S2 SC-F -->
            // Nonce Report MOS = 1 <--
            // S2 SC-F -->

            _smiFirst.AddTestExtensionS2(
                new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.MpanGrp, ExtensionAppliedActions.Add)
                {
                    Value = new byte[] { groupId },
                    ValueSpecified = true
                });
            _ctrlFirst.SendData(NODE_ID_2, wakeUpNotifyCmd, TXO);
            Thread.Sleep(EXPECT_TIMEOUT);

            // Assert.
            Assert.IsTrue(_smiSecond.MpanTable.GetContainer(new NodeGroupId(NODE_ID_1, groupId)).IsMosReportedState);
            Assert.IsTrue(_smiSecond.MpanTable.CheckMpanExists(new NodeGroupId(NODE_ID_1, groupId)));
        }

        [Test]
        public void SendBroadcastData_MgrpSinglecastAll_ActionAdd_MessageRejected()
        {
            _smiFirst.ClearTestExtensionsS2();
            _smiFirst.AddTestExtensionS2(
                new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.MpanGrp, ExtensionAppliedActions.Add)
                {
                    Value = new byte[1],
                    IsEncrypted = false,
                    IsCritical = true,
                    ExtensionLength = 3,
                    IsMoreToFollow = false,
                    NumOfUsage = 1
                });

            // Act
            var expectVersinGetToken = _ctrlSecond.ExpectData(NODE_ID_1, new COMMAND_CLASS_BASIC.BASIC_GET(), EXPECT_TIMEOUT, null);

            _ctrlFirst.SendDataMulti(new[] { NODE_ID_2 },
                new COMMAND_CLASS_BASIC.BASIC_GET(),
                TXO);

            Thread.Sleep(200);

            // Assert
            var res = (ExpectDataResult)expectVersinGetToken.WaitCompletedSignal();
            Assert.IsFalse(res);
        }

        [Test]
        public void SendBroadcastData_MgrpSinglecastAll_ActionAddOrModify_MessageRejected()
        {
            // Arrange
            _smiFirst.AddTestExtensionS2(
                new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.MpanGrp, ExtensionAppliedActions.AddOrModify)
                {
                    Value = new byte[1],
                    IsEncrypted = false,
                    IsCritical = true,
                    ExtensionLength = 3,
                    IsMoreToFollow = false,
                    NumOfUsage = 1
                });

            // Act
            var expectVersinGetToken = _ctrlSecond.ExpectData(NODE_ID_1, new COMMAND_CLASS_BASIC.BASIC_GET(), EXPECT_TIMEOUT, null);

            _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2 }, new COMMAND_CLASS_BASIC.BASIC_GET(), TXO);

            Thread.Sleep(200);

            // Assert
            var res = (ExpectDataResult)expectVersinGetToken.WaitCompletedSignal();
            Assert.IsFalse(res);
        }

        [Test]
        public void SendBroadcastData_MgrpMpan_MosFalse()
        {
            // Arrange
            SyncSpan();
            _smiFirst.AddTestExtensionS2(
                new TestExtensionS2Settings(MessageTypes.SinglecastWithMpanGrp, ExtensionTypes.MpanGrp, ExtensionAppliedActions.Add)
                {
                    Value = new byte[1],
                    IsEncrypted = false,
                    IsCritical = true,
                    ExtensionLength = 3,
                    IsMoreToFollow = false,
                    NumOfUsage = 1
                });
            _smiFirst.AddTestExtensionS2(
                new TestExtensionS2Settings(MessageTypes.SinglecastWithMpanGrp, ExtensionTypes.MpanGrp, ExtensionAppliedActions.Add)
                {
                    Value = new byte[1],
                    IsEncrypted = false,
                    IsCritical = true,
                    ExtensionLength = 16,
                    IsMoreToFollow = false,
                    NumOfUsage = 1
                });

            // Act
            var expectVersinGetToken = _ctrlFirst.ExpectData(NODE_ID_2, new COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT(), EXPECT_TIMEOUT, null);
            _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2 }, new COMMAND_CLASS_BASIC.BASIC_GET(), TXO);

            //Assert
            var res = (ExpectDataResult)expectVersinGetToken.WaitCompletedSignal();
            var nonceReportResult = (COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT)res.Command;
            Assert.IsTrue(nonceReportResult.properties1.mos == 0x00);
        }

        [Test]
        public void SendBroadcastData_MgrpEcrypted_Add_MosFalse()
        {
            // Arrange
            SyncSpan();
            _smiFirst.ClearTestExtensionsS2();
            _smiFirst.AddTestExtensionS2(
                new TestExtensionS2Settings(MessageTypes.SinglecastWithMpanGrp, ExtensionTypes.MpanGrp, ExtensionAppliedActions.Add)
                {
                    Value = new byte[1],
                    IsEncrypted = true,
                    IsCritical = true,
                    ExtensionLength = 3,
                    IsMoreToFollow = false,
                    NumOfUsage = 1
                });

            // Act
            var expectVersinGetToken = _ctrlFirst.ExpectData(NODE_ID_2, new COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT(), EXPECT_TIMEOUT, null);
            _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2 }, new COMMAND_CLASS_BASIC.BASIC_GET(), TXO);

            // Assert
            var res = (ExpectDataResult)expectVersinGetToken.WaitCompletedSignal();
            var nonceReportResult = (COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT)res.Command;
            Assert.IsTrue(nonceReportResult.properties1.mos == 0x00);
        }

        public void SendBroadcastData_MgrpEcrypted_AddOfModify_MosFalse()
        {
            // Arrange
            SyncSpan();
            _smiFirst.AddTestExtensionS2(
                new TestExtensionS2Settings(MessageTypes.SinglecastWithMpanGrp, ExtensionTypes.MpanGrp, ExtensionAppliedActions.AddOrModify)
                {
                    Value = new byte[1],
                    IsEncrypted = true,
                    IsCritical = true,
                    ExtensionLength = 3,
                    IsMoreToFollow = false,
                    NumOfUsage = 1
                });

            // Act
            var expectVersinGetToken = _ctrlFirst.ExpectData(NODE_ID_2, new COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT(), EXPECT_TIMEOUT, null);
            _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2 }, new COMMAND_CLASS_BASIC.BASIC_GET(), TXO);

            // Assert
            var res = (ExpectDataResult)expectVersinGetToken.WaitCompletedSignal();
            var nonceReportResult = (COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT)res.Command;
            Assert.IsTrue(nonceReportResult.properties1.mos == 0x00);
        }

        [Test]
        public void SendBroadcastData_MgrpLength_Add_MosFalse()
        {
            // Arrange
            SyncSpan();
            _smiFirst.ClearTestExtensionsS2();
            _smiFirst.AddTestExtensionS2(
                new TestExtensionS2Settings(MessageTypes.SinglecastWithMpanGrp, ExtensionTypes.MpanGrp, ExtensionAppliedActions.Add)
                {
                    Value = new byte[1],
                    IsEncrypted = false,
                    IsCritical = true,
                    ExtensionLength = 0xff,
                    IsMoreToFollow = false,
                    NumOfUsage = 1
                });

            // Act
            var expectVersinGetToken = _ctrlFirst.ExpectData(NODE_ID_2, new COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT(), EXPECT_TIMEOUT, null);
            _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2 }, new COMMAND_CLASS_BASIC.BASIC_GET(), TXO);

            // Assert
            var res = (ExpectDataResult)expectVersinGetToken.WaitCompletedSignal();
            var nonceReportResult = (COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT)res.Command;
            Assert.IsTrue(nonceReportResult.properties1.mos == 0x00);
        }

        [Test]
        public void SendBroadcastData_MgrpLength_AddOrModify_MosFalse()
        {
            // Arrange
            SyncSpan();
            _smiFirst.AddTestExtensionS2(
                new TestExtensionS2Settings(MessageTypes.SinglecastWithMpanGrp, ExtensionTypes.MpanGrp, ExtensionAppliedActions.AddOrModify)
                {
                    Value = new byte[1],
                    IsEncrypted = false,
                    IsCritical = true,
                    ExtensionLength = 0xff,
                    IsMoreToFollow = false,
                    NumOfUsage = 1
                });

            // Act
            var expectVersinGetToken = _ctrlFirst.ExpectData(NODE_ID_2, new COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT(), EXPECT_TIMEOUT, null);
            _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2 }, new COMMAND_CLASS_BASIC.BASIC_GET(), TXO);

            // Assert
            var res = (ExpectDataResult)expectVersinGetToken.WaitCompletedSignal();
            var nonceReportResult = (COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT)res.Command;
            Assert.IsTrue(nonceReportResult.properties1.mos == 0x00);
        }

        [Test]
        public void SendBroadcastData_MgrpType_Add_MosFalse()
        {
            // Arrange
            SyncSpan();
            _smiFirst.ClearTestExtensionsS2();
            _smiFirst.AddTestExtensionS2(
                new TestExtensionS2Settings(MessageTypes.SinglecastWithMpanGrp, ExtensionTypes.MpanGrp, ExtensionAppliedActions.Add)
                {
                    Value = new byte[1],
                    IsEncrypted = false,
                    IsCritical = true,
                    ExtensionLength = 3,
                    IsMoreToFollow = false,
                    NumOfUsage = 1
                });

            // Act
            var expectVersinGetToken = _ctrlFirst.ExpectData(NODE_ID_2, new COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT(), EXPECT_TIMEOUT, null);
            _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2 }, new COMMAND_CLASS_BASIC.BASIC_GET(), TXO);

            // Assert
            var res = (ExpectDataResult)expectVersinGetToken.WaitCompletedSignal();
            var nonceReportResult = (COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT)res.Command;
            Assert.IsTrue(nonceReportResult.properties1.mos == 0x00);
        }

        public void SendBroadcastData_MgrpType_AddOrModify_MosFalse()
        {
            // Arrange
            SyncSpan();
            _smiFirst.AddTestExtensionS2(
                new TestExtensionS2Settings(MessageTypes.SinglecastWithMpanGrp, ExtensionTypes.MpanGrp, ExtensionAppliedActions.AddOrModify)
                {
                    Value = new byte[1],
                    IsEncrypted = false,
                    IsCritical = true,
                    ExtensionLength = 3,
                    IsMoreToFollow = false,
                    NumOfUsage = 1
                });

            // Act
            var expectVersinGetToken = _ctrlFirst.ExpectData(NODE_ID_2, new COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT(), EXPECT_TIMEOUT, null);
            _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2 }, new COMMAND_CLASS_BASIC.BASIC_GET(), TXO);

            // Assert
            var res = (ExpectDataResult)expectVersinGetToken.WaitCompletedSignal();
            var nonceReportResult = (COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT)res.Command;
            Assert.IsTrue(nonceReportResult.properties1.mos == 0x00);
        }

        //[Test]
        //public void SendBroadcastData_MgrpCritial_MosFalse()
        //{
        //    // Arrange
        //    SyncSpan();
        //    _smiFirst.ClearTestExtensionsS2();
        //    _smiFirst.AddTestExtensionS2(
        //        new TestExtensionS2Settings(MessageTypes.SinglecastAll, ExtensionTypes.MpanGrp, ExtensionAppliedActions.AddOrModify)
        //        {
        //            Value = new byte[1],
        //            IsEncrypted = false,
        //            IsCritical = false,
        //            ExtensionLength = 3,
        //            IsMoreToFollow = false,
        //            NumOfUsage = 1
        //        });

        //    // Act
        //    var expectVersinGetToken = _ctrlFirst.ExpectData(NODE_ID_2, new COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT(), EXPECT_TIMEOUT, null);
        //    _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2 },
        //        new COMMAND_CLASS_BASIC.BASIC_GET(),
        //        TXO);

        //    // Assert
        //    var res = (ExpectDataResult)expectVersinGetToken.WaitCompletedSignal();
        //    var nonceReportResult = (COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT)res.Command;
        //    Assert.IsTrue(nonceReportResult.properties1.mos == 0x00);
        //}

        [Test]
        public void SendBroadcastData_EncryptedgMos_ActionAdd_MpanReceived()
        {
            // Arrange
            SyncSpan();
            _smiFirst.ClearTestExtensionsS2();
            _smiFirst.AddTestExtensionS2(
                new TestExtensionS2Settings(MessageTypes.SinglecastWithMos, ExtensionTypes.Mos, ExtensionAppliedActions.Add)
                {
                    Value = new byte[1],
                    IsEncrypted = true,
                    IsCritical = false,
                    ExtensionLength = 0x02,
                    IsMoreToFollow = false,
                    NumOfUsage = 1
                });

            List<Extensions> _ctrlFirstCommandList = new List<Extensions>();

            List<Extensions> _ctrlSecondCommandList = new List<Extensions>();

            void firstCollectDataDelegate(AchData x)
            {
                _ctrlFirstCommandList.Add(x.Extensions);
            }

            void secondCollectDataDelegate(AchData x)
            {
                _ctrlSecondCommandList.Add(x.Extensions);
            }

            var _ctrlFirstListenToken = _ctrlFirst.ListenData(firstCollectDataDelegate);
            var _ctrlSecondListenToken = _ctrlSecond.ListenData(secondCollectDataDelegate);

            // Act
            _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2 }, new COMMAND_CLASS_BASIC.BASIC_GET(), TXO);

            Thread.Sleep(200);

            _ctrlSecond.Cancel(_ctrlSecondListenToken);
            _ctrlFirst.Cancel(_ctrlFirstListenToken);

            // Assert
            var res = (ListenDataResult)_ctrlSecondListenToken.WaitCompletedSignal();
            Assert.IsTrue(_ctrlSecondCommandList[2].EncryptedExtensionsList[0].extension != null);
        }

        [Test]
        public void SendBroadcastData_EncryptedMos_ActionAddOrModify_MpanReceived()
        {
            // Arrange
            SyncSpan();
            _smiFirst.AddTestExtensionS2(
                new TestExtensionS2Settings(MessageTypes.SinglecastWithMos, ExtensionTypes.Mos, ExtensionAppliedActions.AddOrModify)
                {
                    Value = new byte[1],
                    IsEncrypted = true,
                    IsCritical = false,
                    ExtensionLength = 0x02,
                    IsMoreToFollow = false,
                    NumOfUsage = 1
                });

            List<Extensions> _ctrlFirstCommandList = new List<Extensions>();
            List<Extensions> _ctrlSecondCommandList = new List<Extensions>();

            void firstCollectDataDelegate(AchData x)
            {
                _ctrlFirstCommandList.Add(x.Extensions);
            }

            void secondCollectDataDelegate(AchData x)
            {
                _ctrlSecondCommandList.Add(x.Extensions);
            }

            // Act
            var _ctrlFirstListenToken = _ctrlFirst.ListenData(firstCollectDataDelegate);
            var _ctrlSecondListenToken = _ctrlSecond.ListenData(secondCollectDataDelegate);

            _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2 }, new COMMAND_CLASS_BASIC.BASIC_GET(), TXO);

            Thread.Sleep(200);

            _ctrlSecond.Cancel(_ctrlSecondListenToken);
            _ctrlFirst.Cancel(_ctrlFirstListenToken);

            var res = (ListenDataResult)_ctrlSecondListenToken.WaitCompletedSignal();

            // Assert
            Assert.IsTrue(_ctrlSecondCommandList[2].EncryptedExtensionsList[0].extension != null);
        }

        [Test]

        public void SendBroadcastData_LengthMos_ActionAddOrModify_MpanReceived()
        {
            // Arrange
            SyncSpan();
            _smiFirst.AddTestExtensionS2(
                new TestExtensionS2Settings(MessageTypes.SinglecastWithMos, ExtensionTypes.Mos, ExtensionAppliedActions.AddOrModify)
                {
                    Value = new byte[1],
                    IsEncrypted = false,
                    IsCritical = false,
                    ExtensionLength = 0xFF,
                    IsMoreToFollow = false,
                    NumOfUsage = 1
                });

            List<Extensions> _ctrlFirstCommandList = new List<Extensions>();
            List<Extensions> _ctrlSecondCommandList = new List<Extensions>();

            void firstCollectDataDelegate(AchData x)
            {
                _ctrlFirstCommandList.Add(x.Extensions);
            }

            void secondCollectDataDelegate(AchData x)
            {
                _ctrlSecondCommandList.Add(x.Extensions);
            }

            // Act
            var _ctrlFirstListenToken = _ctrlFirst.ListenData(firstCollectDataDelegate);
            var _ctrlSecondListenToken = _ctrlSecond.ListenData(secondCollectDataDelegate);

            _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2 }, new COMMAND_CLASS_BASIC.BASIC_GET(), TXO);

            Thread.Sleep(200);

            _ctrlSecond.Cancel(_ctrlSecondListenToken);
            _ctrlFirst.Cancel(_ctrlFirstListenToken);

            var res = (ListenDataResult)_ctrlSecondListenToken.WaitCompletedSignal();

            // Assert
            Assert.IsTrue(_ctrlSecondCommandList[2].EncryptedExtensionsList[0].extension != null);
        }

        [Test]

        public void SendBroadcastData_LengthMos_ActionAdd_MpanReceived()
        {
            // Arrange
            SyncSpan();
            _smiFirst.ClearTestExtensionsS2();
            _smiFirst.AddTestExtensionS2(
                new TestExtensionS2Settings(MessageTypes.SinglecastWithMos, ExtensionTypes.Mos, ExtensionAppliedActions.Add)
                {
                    Value = new byte[1],
                    IsEncrypted = false,
                    IsCritical = false,
                    ExtensionLength = 0xFF,
                    IsMoreToFollow = false,
                    NumOfUsage = 1
                });

            List<Extensions> _ctrlFirstCommandList = new List<Extensions>();
            List<Extensions> _ctrlSecondCommandList = new List<Extensions>();

            void firstCollectDataDelegate(AchData x)
            {
                _ctrlFirstCommandList.Add(x.Extensions);
            }

            void secondCollectDataDelegate(AchData x)
            {
                _ctrlSecondCommandList.Add(x.Extensions);
            }

            // Act
            var _ctrlFirstListenToken = _ctrlFirst.ListenData(firstCollectDataDelegate);
            var _ctrlSecondListenToken = _ctrlSecond.ListenData(secondCollectDataDelegate);

            _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2 }, new COMMAND_CLASS_BASIC.BASIC_GET(), TXO);

            Thread.Sleep(200);

            _ctrlSecond.Cancel(_ctrlSecondListenToken);
            _ctrlFirst.Cancel(_ctrlFirstListenToken);

            var res = (ListenDataResult)_ctrlSecondListenToken.WaitCompletedSignal();

            // Assert
            Assert.IsTrue(_ctrlSecondCommandList[2].EncryptedExtensionsList[0].extension != null);
        }
        [Test]

        public void SendBroadcastData_MosExtensionTypeTest_ActionAdd_MpanReceived()
        {
            // Arrange
            SyncSpan();
            _smiFirst.ClearTestExtensionsS2();
            _smiFirst.AddTestExtensionS2(
                new TestExtensionS2Settings(MessageTypes.SinglecastWithMos, ExtensionTypes.Test, ExtensionAppliedActions.Add)
                {
                    Value = new byte[] { },
                    IsEncrypted = false,
                    IsCritical = false,
                    ExtensionLength = 0x02,
                    IsMoreToFollow = false,
                    NumOfUsage = 1
                });

            List<Extensions> _ctrlFirstCommandList = new List<Extensions>();
            List<Extensions> _ctrlSecondCommandList = new List<Extensions>();

            void firstCollectDataDelegate(AchData x)
            {
                _ctrlFirstCommandList.Add(x.Extensions);
            }

            void secondCollectDataDelegate(AchData x)
            {
                _ctrlSecondCommandList.Add(x.Extensions);
            }

            // Act
            var _ctrlFirstListenToken = _ctrlFirst.ListenData(firstCollectDataDelegate);
            var _ctrlSecondListenToken = _ctrlSecond.ListenData(secondCollectDataDelegate);

            _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2 }, new COMMAND_CLASS_BASIC.BASIC_GET(), TXO);

            Thread.Sleep(200);

            _ctrlSecond.Cancel(_ctrlSecondListenToken);
            _ctrlFirst.Cancel(_ctrlFirstListenToken);

            var res = (ListenDataResult)_ctrlSecondListenToken.WaitCompletedSignal();

            // Assert
            Assert.IsTrue(_ctrlSecondCommandList[2].EncryptedExtensionsList[0].extension != null);
        }

        [Test]
        public void SendBroadcastData_MosExtensionTypeTest_ActionAddOrModify_MpanReceived()
        {
            // Arrange
            SyncSpan();
            _smiFirst.AddTestExtensionS2(
                new TestExtensionS2Settings(MessageTypes.SinglecastWithMos, ExtensionTypes.Test, ExtensionAppliedActions.AddOrModify)
                {
                    Value = new byte[] { },
                    IsEncrypted = false,
                    IsCritical = false,
                    ExtensionLength = 0x02,
                    IsMoreToFollow = false,
                    NumOfUsage = 1
                });

            List<Extensions> _ctrlFirstCommandList = new List<Extensions>();
            List<Extensions> _ctrlSecondCommandList = new List<Extensions>();

            void firstCollectDataDelegate(AchData x)
            {
                _ctrlFirstCommandList.Add(x.Extensions);
            }

            void secondCollectDataDelegate(AchData x)
            {
                _ctrlSecondCommandList.Add(x.Extensions);
            }

            // Act
            var _ctrlFirstListenToken = _ctrlFirst.ListenData(firstCollectDataDelegate);
            var _ctrlSecondListenToken = _ctrlSecond.ListenData(secondCollectDataDelegate);

            _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2 }, new COMMAND_CLASS_BASIC.BASIC_GET(), TXO);

            Thread.Sleep(200);

            _ctrlSecond.Cancel(_ctrlSecondListenToken);
            _ctrlFirst.Cancel(_ctrlFirstListenToken);

            var res = (ListenDataResult)_ctrlSecondListenToken.WaitCompletedSignal();

            // Assert
            Assert.IsTrue(_ctrlSecondCommandList[2].EncryptedExtensionsList[0].extension != null);
        }

        [Test]
        public void SendBroadcastData_MpanDecrypted_AddOrModify_MpanReceived()
        {
            // Arrange
            SyncSpan();
            _smiFirst.AddTestExtensionS2(
                new TestExtensionS2Settings(MessageTypes.SinglecastWithMpan, ExtensionTypes.Mpan, ExtensionAppliedActions.AddOrModify)
                {
                    IsEncrypted = false,
                    NumOfUsage = 1
                });
            List<Extensions> _ctrlSecondCommandList = new List<Extensions>();

            void secondCollectDataDelegate(AchData x)
            {
                _ctrlSecondCommandList.Add(x.Extensions);
            }

            var _ctrlSecondListenToken = _ctrlSecond.ListenData(secondCollectDataDelegate);

            // Act
            _ctrlFirst.SendDataMultiBridge(NodeTag.Empty, new[] { NODE_ID_2 }, new COMMAND_CLASS_BASIC.BASIC_GET(), TXO);

            Thread.Sleep(200);

            _ctrlSecond.Cancel(_ctrlSecondListenToken);

            var res = (ListenDataResult)_ctrlSecondListenToken.WaitCompletedSignal();

            // Assert
            Assert.IsTrue(_ctrlSecondCommandList[2].EncryptedExtensionsList[0].extension != null);
        }

        private void SyncSpan()
        {
            var versionGetCmd = CreateDataAsVersionGet();
            // We're assuming it works - not actually a part of unit test. Need to be moved to plain nonce instantiation.
            var expectVersinGetToken2 = _ctrlSecond.ExpectData(NODE_ID_1, versionGetCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2, versionGetCmd, TXO);
            var expectRes2 = (ExpectDataResult)expectVersinGetToken2.WaitCompletedSignal();
        }

        private byte[] CreateDataAsVersionWakeUpNotify()
        {
            return new COMMAND_CLASS_WAKE_UP.WAKE_UP_NOTIFICATION();
        }

        private byte[] CreateDataAsVersionGet()
        {
            return new COMMAND_CLASS_VERSION.VERSION_GET();
        }

        private byte[] CreateDataAsSupervisionGet()
        {
            return new COMMAND_CLASS_SUPERVISION.SUPERVISION_GET()
            {
                properties1 = new COMMAND_CLASS_SUPERVISION.SUPERVISION_GET.Tproperties1()
                {
                    sessionId = 0xAA
                }
            };
        }
    }
}
