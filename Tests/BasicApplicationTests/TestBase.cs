/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;
using NUnit.Framework;
using ZWave.Enums;
using ZWave.BasicApplication;
using ZWave.BasicApplication.Devices;
using ZWave.CommandClasses;
using ZWave.Security;
using ZWave.Layers;
using ZWave.Layers.Session;
using ZWave.Configuration;
using ZWave.BasicApplication.EmulatedLink;
using ZWave.Devices;
using ZWave;
using Utils;

namespace BasicApplicationTests
{
    public class TestBase
    {
        protected readonly byte[] NKEY1_S2_C0 = "C0111111111111111111111111111111".GetBytes();
        protected readonly byte[] NKEY1_S2_C1 = "C1111111111111111111111111111111".GetBytes();
        protected readonly byte[] NKEY1_S2_C2 = "C2111111111111111111111111111111".GetBytes();
        protected readonly byte[] NKEY1_S0 = "11111111111111111111111111111111".GetBytes();

        protected readonly byte[] NKEY2_S2_C0 = "C0222222222222222222222222222222".GetBytes();
        protected readonly byte[] NKEY2_S2_C1 = "C1222222222222222222222222222222".GetBytes();
        protected readonly byte[] NKEY2_S2_C2 = "C2222222222222222222222222222222".GetBytes();
        protected readonly byte[] NKEY2_S0 = "11222222222222222222222222222222".GetBytes();

        protected readonly byte[] NKEY3_S2_C0 = "C0333333333333333333333333333333".GetBytes();
        protected readonly byte[] NKEY3_S2_C1 = "C1333333333333333333333333333333".GetBytes();
        protected readonly byte[] NKEY3_S2_C2 = "C2333333333333333333333333333333".GetBytes();
        protected readonly byte[] NKEY3_S0 = "11333333333333333333333333333333".GetBytes();

        #region Hardcodes
        protected const int EXPECT_TIMEOUT = 2344;
        protected const int INCLUSION_TIMEOUT = 5677;

        protected static NodeTag NODE_ID_0 = new NodeTag(0x00);

        protected static NodeTag NODE_ID_1 = new NodeTag(0x01);
        protected static NodeTag NODE_ID_2 = new NodeTag(0x02);
        protected static NodeTag NODE_ID_3 = new NodeTag(0x03);
        protected readonly InvariantPeerNodeId PEER_NODE_ID_2 = new InvariantPeerNodeId(NODE_ID_1, NODE_ID_2);
        protected readonly InvariantPeerNodeId PEER_NODE_ID_1 = new InvariantPeerNodeId(NODE_ID_2, NODE_ID_1);
        protected readonly TransmitOptions TXO = TransmitOptions.TransmitOptionAcknowledge | TransmitOptions.TransmitOptionAutoRoute | TransmitOptions.TransmitOptionExplore;

        private byte[] _defaultCommandClasses =
        {
            COMMAND_CLASS_ZWAVEPLUS_INFO_V2.ID,
            COMMAND_CLASS_VERSION.ID,
            COMMAND_CLASS_MANUFACTURER_SPECIFIC.ID,
            COMMAND_CLASS_FIRMWARE_UPDATE_MD.ID,
            COMMAND_CLASS_POWERLEVEL.ID,
            COMMAND_CLASS_APPLICATION_STATUS.ID,
            COMMAND_CLASS_ASSOCIATION.ID,
            COMMAND_CLASS_ASSOCIATION_GRP_INFO.ID,
            COMMAND_CLASS_CONFIGURATION_V3.ID,
            COMMAND_CLASS_CRC_16_ENCAP.ID,
            COMMAND_CLASS_DEVICE_RESET_LOCALLY.ID,
            COMMAND_CLASS_SECURITY.ID,
            COMMAND_CLASS_SECURITY_2.ID,
            COMMAND_CLASS_SUPERVISION.ID,
            COMMAND_CLASS_TRANSPORT_SERVICE_V2.ID,
            COMMAND_CLASS_FIRMWARE_UPDATE_MD_V5.ID,
            COMMAND_CLASS_INCLUSION_CONTROLLER.ID,
            //Supported but not mandatory:
            COMMAND_CLASS_MULTI_CHANNEL_ASSOCIATION_V3.ID,
            //COMMAND_CLASS_MULTI_CHANNEL_V4.ID,
            //COMMAND_CLASS_WAKE_UP_V2.ID
        };
        #endregion

        protected FramesLoggerProvider _framesLoggerProvider;
        protected Controller _ctrlFirst;
        protected Controller _ctrlSecond;
        protected Controller _ctrlThird;
        protected TransmitOptions _txOptions = TransmitOptions.TransmitOptionAcknowledge;
        protected BasicLinkTransportLayer _transport;
        protected BasicApplicationLayer _app;
        protected SessionLayer _sessionLayer;

        [SetUp]
        public void SetUpBase()
        {

            _framesLoggerProvider = new FramesLoggerProvider();
            BasicLinkModuleMemory.ResetSharedIdCounter();
            ActionBase.ResetSharedIdCounter();
            _transport = new BasicLinkTransportLayer();
            _sessionLayer = new SessionLayer();
            _app = new BasicApplicationLayer(_sessionLayer, new BasicFrameLayer(), _transport);
            _app.FrameLayer.FrameTransmitted += _framesLoggerProvider.FrameLayer_FrameTransmitted;

            _framesLoggerProvider.ClearLog();
            _ctrlFirst = _app.CreateController();
            _ctrlSecond = _app.CreateController();
            _ctrlThird = _app.CreateController();

            _ctrlFirst.Connect(new SerialPortDataSource("COM1"));
            _ctrlSecond.Connect(new SerialPortDataSource("COM2"));
            _ctrlThird.Connect(new SerialPortDataSource("COM3"));

            _ctrlFirst.MemoryGetId();
            _ctrlFirst.Network.SetCommandClasses(_defaultCommandClasses);

            _ctrlSecond.MemoryGetId();
            _ctrlSecond.Network.SetCommandClasses(_defaultCommandClasses);

            _ctrlThird.MemoryGetId();
            _ctrlThird.Network.SetCommandClasses(_defaultCommandClasses);
        }

        [TearDown]
        public void TearDownBase()
        {
            _ctrlFirst.Dispose();
            _ctrlSecond.Dispose();
            _ctrlThird.Dispose();
        }

        protected SecurityManager SetUpSecurity(Controller ctrl, byte[] keyS2C0 = null, byte[] keyS2C1 = null, byte[] keyS2C2 = null, byte[] keyS0 = null)
        {
            NetworkKey[] nkeys = new NetworkKey[8];
            nkeys[0] = new NetworkKey(keyS2C0);
            nkeys[1] = new NetworkKey(keyS2C1);
            nkeys[2] = new NetworkKey(keyS2C2);
            nkeys[7] = new NetworkKey(keyS0);

            var securityManager = new SecurityManager(ctrl.Network,
                nkeys,
                new byte[]
                {
                        0x77, 0x07, 0x6d, 0x0a, 0x73, 0x18, 0xa5, 0x7d, 0x3c, 0x16, 0xc1, 0x72, 0x51, 0xb2, 0x66, 0x45,
                        0xdf, 0x4c, 0x2f, 0x87, 0xeb, 0xc0, 0x99, 0x2a, 0xb1, 0x77, 0xfb, 0xa5, 0x1d, 0xb9, 0x2c, 0x2a
                });

            securityManager.SecurityManagerInfo.CheckIfSupportSecurityCC = true;
            securityManager.SecurityManagerInfo.DSKNeededCallback = (nodeId, DSKFull) =>
            {
                return new byte[] { 0x85, 0x20 };
            };
            securityManager.SecurityManagerInfo.DSKVerificationOnReceiverCallback = () =>
            {
                return new byte[] { 0x85, 0x20, 0xF0, 0x09 };
            };
            securityManager.SecurityManagerInfo.SetTestSecretKeyS2(new byte[]
            {
                0x77, 0x07, 0x6d, 0x0a, 0x73, 0x18, 0xa5, 0x7d, 0x3c, 0x16, 0xc1, 0x72, 0x51, 0xb2, 0x66, 0x45,
                0xdf, 0x4c, 0x2f, 0x87, 0xeb, 0xc0, 0x99, 0x2a, 0xb1, 0x77, 0xfb, 0xa5, 0x1d, 0xb9, 0x2c, 0x2a
            });

            ctrl.SessionClient.AddSubstituteManager(securityManager);
            ctrl.SessionClient.ExecuteAsync(securityManager.CreateSecurityReportTask());
            ctrl.SessionClient.ExecuteAsync(securityManager.CreateSecurityS2ReportTask());
            ctrl.SessionClient.ExecuteAsync(securityManager.CreateInclusionControllerSupportTask(null, null));
            return securityManager;
        }

        protected void AssertCmdSequence(ushort sessionId, FrameLogRecord[] expectedCmds)
        {
            Assert.IsTrue(_framesLoggerProvider.FramesLog.ContainsKey(sessionId));
            Assert.AreEqual(expectedCmds.Length, _framesLoggerProvider.FramesLog[sessionId].Count);
            for (int i = 0; i < expectedCmds.Length; i++)
            {
                Assert.IsTrue(expectedCmds[i].Equals(_framesLoggerProvider.FramesLog[sessionId][i]));
            }
        }

        protected byte[] CreateDataAsBasicSet()
        {
            return new COMMAND_CLASS_BASIC.BASIC_SET();
        }

        protected byte[] CreateDataAsVeryLongBasicSet(int numOfAdditionalBytes)
        {
            List<byte> tmpData = new List<byte>();
            byte[] cmd = new COMMAND_CLASS_BASIC.BASIC_SET();
            tmpData.AddRange(cmd);
            for (int i = 0; i < numOfAdditionalBytes; i++)
            {
                tmpData.Add((byte)i);
            }
            return tmpData.ToArray();
        }
    }
}
