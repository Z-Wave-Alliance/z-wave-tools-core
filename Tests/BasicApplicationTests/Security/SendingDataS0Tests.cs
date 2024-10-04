/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using NUnit.Framework;
using System.Linq;
using ZWave.Enums;
using ZWave.BasicApplication.Operations;
using ZWave.BasicApplication.Security;
using ZWave.CommandClasses;
using ZWave.BasicApplication;
using ZWave.Configuration;
using System.Threading;

namespace BasicApplicationTests.Security.SendingData
{
    [TestFixture]
    public class SendingDataS0Tests : TestBase
    {
        [SetUp]
        public void SetUp()
        {
            _transport.SetUpModulesNetwork(_ctrlFirst.SessionId, _ctrlSecond.SessionId);
            _ctrlFirst.MemoryGetId();
            _ctrlSecond.MemoryGetId();

            var smFirst = new SecurityManager(_ctrlFirst.Network,
                new[]
                {
                    null, null, null, null, null, null, null, new NetworkKey(NKEY1_S0)
                },
                new byte[]
                {
                    0x77, 0x07, 0x6d, 0x0a, 0x73, 0x18, 0xa5, 0x7d, 0x3c, 0x16, 0xc1, 0x72, 0x51, 0xb2, 0x66, 0x45,
                    0xdf, 0x4c, 0x2f, 0x87, 0xeb, 0xc0, 0x99, 0x2a, 0xb1, 0x77, 0xfb, 0xa5, 0x1d, 0xb9, 0x2c, 0x2a
                });
            _ctrlFirst.SessionClient.AddSubstituteManager(smFirst);
            _ctrlFirst.Network.IsEnabledS0 = true;
            _ctrlFirst.Network.SetSecuritySchemes(NODE_ID_2, SecuritySchemeSet.S0);
            _ctrlFirst.SessionClient.ExecuteAsync(smFirst.CreateSecurityReportTask());

            var smSecond = new SecurityManager(_ctrlSecond.Network,
                new[]
                {
                    null, null, null, null, null, null, null, new NetworkKey(NKEY1_S0)
                },
                new byte[]
                {
                    0x77, 0x07, 0x6d, 0x0a, 0x73, 0x18, 0xa5, 0x7d, 0x3c, 0x16, 0xc1, 0x72, 0x51, 0xb2, 0x66, 0x45,
                    0xdf, 0x4c, 0x2f, 0x87, 0xeb, 0xc0, 0x99, 0x2a, 0xb1, 0x77, 0xfb, 0xa5, 0x1d, 0xb9, 0x2c, 0x2a
                });
            _ctrlSecond.SessionClient.AddSubstituteManager(smSecond);
            _ctrlSecond.Network.IsEnabledS0 = true;
            _ctrlSecond.Network.SetSecuritySchemes(NODE_ID_1, SecuritySchemeSet.S0);
            _ctrlSecond.SessionClient.ExecuteAsync(smSecond.CreateSecurityReportTask());
        }

        [Test]
        public void A_DataSend_DoorLookConfigurationGet_ReceiverGetsData()
        {
            // Arrange.
            byte[] primaryCmd = CreateDataAsBasicSet();
            var expectToken = _ctrlSecond.ExpectData(NODE_ID_1, primaryCmd, EXPECT_TIMEOUT, null);

            // Act.
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsTrue(expectRes.Command.SequenceEqual(primaryCmd));
        }

        [Test]
        public void B_SecureSendData_DevicesWereNotSync_SyncDevicesAndDecryptData()
        {
            // Arange.

            // Act.
            byte[] primaryCmd = CreateDataAsBasicSet();
            var expectToken = _ctrlSecond.ExpectData(primaryCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();
            Thread.Sleep(5000);
            // Assert.
            Assert.IsNotNull(expectRes.Command);
            Assert.IsTrue(expectRes.Command.SequenceEqual(primaryCmd));
            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY.SECURITY_NONCE_GET.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY.SECURITY_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY.SECURITY_MESSAGE_ENCAPSULATION.ID)
            });
        }

        [Test]
        public void C_SecureSendData_SendMsgEncapWithNonceGet_RequestDataPasses()
        {
            // Arange.
            _transport.MaxFrameSize = 100;

            // Act.
            byte[] primaryCmd = CreateDataAsVeryLongBasicSet(40);
            var expectToken = _ctrlSecond.ExpectData(primaryCmd, EXPECT_TIMEOUT, null);
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsTrue(expectRes.Command.SequenceEqual(primaryCmd));

            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY.SECURITY_NONCE_GET.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY.SECURITY_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY.SECURITY_MESSAGE_ENCAPSULATION_NONCE_GET.ID),
                FrameLogRecord.Create(NODE_ID_2, NODE_ID_0, COMMAND_CLASS_SECURITY.SECURITY_NONCE_REPORT.ID),
                FrameLogRecord.Create(NODE_ID_0, NODE_ID_2, COMMAND_CLASS_SECURITY.SECURITY_MESSAGE_ENCAPSULATION.ID)
            });
        }
    }
}
