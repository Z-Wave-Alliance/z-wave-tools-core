/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Linq;
using NUnit.Framework;
using ZWave.BasicApplication.Operations;
using ZWave.CommandClasses;
using ZWave.BasicApplication;
using System;
using Utils;
using ZWave.Devices;

namespace BasicApplicationTests.Crc16Encap
{
    [TestFixture]
    public class SendDataCrc16EncapTests : TestBase
    {
        [SetUp]
        public void SetUp()
        {
            _transport.SetUpModulesNetwork(_ctrlFirst.SessionId, _ctrlSecond.SessionId);
            _ctrlFirst.SessionClient.AddSubstituteManager(new Crc16EncapManager(new NetworkViewPoint()));
            _ctrlSecond.SessionClient.AddSubstituteManager(new Crc16EncapManager(new NetworkViewPoint()));
        }

        [Test]
        public void A_Crc16EncapSendData_EncapAdded_DataReceived()
        {
            // Arange.
            byte[] primaryCmd = CreateDataAsBasicSet();
            byte[] encapCmd = Crc16Encap(primaryCmd);
            var expectToken = _ctrlSecond.ExpectData(primaryCmd, 1000, null);

            // Act.
            _ctrlFirst.SendData(NODE_ID_2, encapCmd, TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsTrue(expectRes.Command.SequenceEqual(primaryCmd));
            AssertCmdSequence(_ctrlFirst.SessionId, new FrameLogRecord[]
            {
                FrameLogRecord.Create(new NodeTag(0), NODE_ID_2, COMMAND_CLASS_CRC_16_ENCAP.CRC_16_ENCAP.ID),
            });
        }

        [Test]
        public void SendData_NoEncap_ReceiverGetsData()
        {
            // Arrange
            byte[] primaryCmd = CreateDataAsBasicSet();
            var expectToken = _ctrlSecond.ExpectData(NODE_ID_1, primaryCmd, 1000, null);

            // Act.
            _ctrlFirst.SendData(NODE_ID_2, primaryCmd, TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsTrue(expectRes.Command.SequenceEqual(primaryCmd));
        }

        protected byte[] Crc16Encap(byte[] data)
        {
            byte[] ret = new byte[data.Length + 4];
            ret[0] = COMMAND_CLASS_CRC_16_ENCAP.ID;
            ret[1] = COMMAND_CLASS_CRC_16_ENCAP.CRC_16_ENCAP.ID;
            Array.Copy(data, 0, ret, 2, data.Length);
            Array.Copy(Tools.CalculateCrc16Array(ret, 0, ret.Length - 2), 0, ret, ret.Length - 2, 2);
            return ret;
        }
    }
}
