/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using NUnit.Framework;
using System.Linq;
using ZWave.BasicApplication;
using ZWave.BasicApplication.EmulatedLink;
using ZWave.BasicApplication.Operations;
using ZWave.BasicApplication.Tasks;
using ZWave.CommandClasses;
using ZWave.Devices;
using ZWave.Enums;
using ZWave.Layers;
using ZWave.Layers.Session;

namespace BasicApplicationTests.Security
{
    [TestFixture]
    public class EmulatedTests
    {
        private readonly TransmitOptions _txOptions = TransmitOptions.TransmitOptionAcknowledge;
        private BasicLinkTransportLayer _transport;
        private BasicApplicationLayer _app;
        private SessionLayer _sessionLayer;

        [SetUp]
        public void Setup()
        {
            _transport = new BasicLinkTransportLayer();
            _sessionLayer = new SessionLayer();
            _app = new BasicApplicationLayer(_sessionLayer, new BasicFrameLayer(), _transport);
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void MemoryGetIdTest()
        {
            // Arrange
            var firstCtrl = _app.CreateController();
            var secondCtrl = _app.CreateController();

            firstCtrl.Connect(new SerialPortDataSource("COM1"));
            secondCtrl.Connect(new SerialPortDataSource("COM2"));

            // Act
            firstCtrl.MemoryGetId();
            secondCtrl.MemoryGetId();

            // Assert
            Assert.AreEqual(0x01, firstCtrl.Id);
            Assert.AreEqual(0x01, secondCtrl.Id);

            firstCtrl.Dispose();
            secondCtrl.Dispose();
        }

        [Test]
        public void SendDataTest()
        {
            // Arrange
            var firstCtrl = _app.CreateController();
            var secondCtrl = _app.CreateController();

            _transport.SetUpModulesNetwork(firstCtrl.SessionId, secondCtrl.SessionId);

            firstCtrl.Connect(new SerialPortDataSource("COM1"));
            secondCtrl.Connect(new SerialPortDataSource("COM2"));

            // Act
            var expectToken = secondCtrl.ExpectData(new NodeTag(0x01), new COMMAND_CLASS_BASIC.BASIC_SET(), 1000, null);
            var sendRes = firstCtrl.SendData(new NodeTag(0x02), new COMMAND_CLASS_BASIC.BASIC_SET(), _txOptions);
            var expectRes = expectToken.WaitCompletedSignal();

            // Assert
            Assert.IsTrue(sendRes);
            Assert.AreEqual(TransmitStatuses.CompleteOk, sendRes.TransmitStatus);
            Assert.IsTrue(expectRes);

            firstCtrl.Dispose();
            secondCtrl.Dispose();
        }

        [Test]
        public void SetDefaultTest()
        {
            // Arrange
            var firstCtrl = _app.CreateController();
            var secondCtrl = _app.CreateController();

            _transport.SetUpModulesNetwork(firstCtrl.SessionId, secondCtrl.SessionId);

            firstCtrl.Connect(new SerialPortDataSource("COM1"));
            secondCtrl.Connect(new SerialPortDataSource("COM2"));

            // Act
            secondCtrl.MemoryGetId();
            Assert.AreEqual(0x02, secondCtrl.Id);
            var setRes = secondCtrl.SetDefault();
            secondCtrl.MemoryGetId();

            // Assert
            Assert.AreEqual(0x01, secondCtrl.Id);
            Assert.IsTrue(setRes);

            firstCtrl.Dispose();
            secondCtrl.Dispose();
        }

        [Test]
        public void SendDataMulti()
        {
            // Arrange
            var firstCtrl = _app.CreateController();
            var secondCtrl = _app.CreateController();
            var thirdCtrl = _app.CreateController();

            _transport.SetUpModulesNetwork(firstCtrl.SessionId, secondCtrl.SessionId, thirdCtrl.SessionId);

            firstCtrl.Connect(new SerialPortDataSource("COM1"));
            secondCtrl.Connect(new SerialPortDataSource("COM2"));
            thirdCtrl.Connect(new SerialPortDataSource("COM3"));

            // Act
            var expectSecondToken = secondCtrl.ExpectData(new NodeTag(0x01), new COMMAND_CLASS_BASIC.BASIC_SET(), 1000, null);
            var expectThirdToken = thirdCtrl.ExpectData(new NodeTag(0x01), new COMMAND_CLASS_BASIC.BASIC_SET(), 1000, null);
            var sendRes = firstCtrl.SendDataMulti(new NodeTag[] { new NodeTag(0x02), new NodeTag(0x03) }, new COMMAND_CLASS_BASIC.BASIC_SET(), _txOptions);
            var expectSecondRes = expectSecondToken.WaitCompletedSignal();
            var expectThirdRes = expectThirdToken.WaitCompletedSignal();

            // Assert
            Assert.IsTrue(sendRes);
            Assert.AreEqual(TransmitStatuses.CompleteNoAcknowledge, sendRes.TransmitStatus);
            Assert.IsTrue(expectSecondRes);
            Assert.IsTrue(expectThirdRes);

            firstCtrl.Dispose();
            secondCtrl.Dispose();
            thirdCtrl.Dispose();
        }

        [Test]
        public void RequestNodeInfoTest()
        {
            // Arrange
            var firstCtrl = _app.CreateController();
            var secondCtrl = _app.CreateController();

            _transport.SetUpModulesNetwork(firstCtrl.SessionId, secondCtrl.SessionId);

            firstCtrl.Connect(new SerialPortDataSource("COM1"));
            secondCtrl.Connect(new SerialPortDataSource("COM2"));

            // Act
            secondCtrl.ApplicationNodeInformation(DeviceOptions.Listening, 1, 1, new byte[] { 1, 2, 3 });
            var nifRes = firstCtrl.RequestNodeInfo(new NodeTag(0x02));

            // Assert
            Assert.IsTrue(nifRes);
            Assert.IsTrue(new byte[] { 1, 2, 3 }.SequenceEqual(nifRes.CommandClasses));

            firstCtrl.Dispose();
            secondCtrl.Dispose();
        }

        [Test]
        public void AddNodeToNetworkTest()
        {
            // Arrange
            var firstCtrl = _app.CreateController();
            var secondCtrl = _app.CreateController();

            firstCtrl.Connect(new SerialPortDataSource("COM1"));
            secondCtrl.Connect(new SerialPortDataSource("COM2"));

            // Act
            var addToken = firstCtrl.AddNodeToNetwork(Modes.NodeAny, 1000, null);
            firstCtrl.WaitNodeStatusSignal(ZWave.BasicApplication.Enums.NodeStatuses.LearnReady, 1000);
            var learnToken = secondCtrl.SetLearnMode(LearnModes.LearnModeClassic, 1000, null);

            var addRes = (AddRemoveNodeResult)addToken.WaitCompletedSignal();
            var learnRes = (SetLearnModeResult)learnToken.WaitCompletedSignal();
            secondCtrl.MemoryGetId();

            // Assert
            Assert.IsTrue(addRes);
            Assert.AreEqual(0x02, addRes.Id);
            Assert.IsTrue(learnRes);
            Assert.AreEqual(0x02, secondCtrl.Id);

            firstCtrl.Dispose();
            secondCtrl.Dispose();
        }

        [Test]
        [Ignore("firstCrtl hav`n 'IsFailed' node. nothing to replace")]
        public void ReplaceFailedNodeTest()
        {
            // Arrange
            var firstCtrl = _app.CreateController();
            var secondCtrl = _app.CreateController();

            firstCtrl.Connect(new SerialPortDataSource("COM1"));
            secondCtrl.Connect(new SerialPortDataSource("COM2"));

            // Act
            var addToken = firstCtrl.ReplaceFailedNode(new NodeTag(2), null);
            var learnToken = secondCtrl.SetLearnMode(LearnModes.LearnModeClassic, 1000, null);

            var addRes = (InclusionResult)addToken.WaitCompletedSignal();
            var learnRes = (SetLearnModeResult)learnToken.WaitCompletedSignal();
            secondCtrl.MemoryGetId();

            // Assert
            Assert.IsTrue(addRes);
            Assert.AreEqual(0x02, addRes.AddRemoveNode.Id);
            Assert.IsTrue(learnRes);
            Assert.AreEqual(0x02, secondCtrl.Id);

            firstCtrl.Dispose();
            secondCtrl.Dispose();
        }

        [Test]
        public void RemoveNodeFromNetworkTest()
        {
            // Arrange
            var firstCtrl = _app.CreateController();
            var secondCtrl = _app.CreateController();

            _transport.SetUpModulesNetwork(firstCtrl.SessionId, secondCtrl.SessionId);

            firstCtrl.Connect(new SerialPortDataSource("COM1"));
            secondCtrl.Connect(new SerialPortDataSource("COM2"));

            // Act
            var removeToken = firstCtrl.RemoveNodeFromNetwork(Modes.NodeAny, 1000, null);
            firstCtrl.WaitNodeStatusSignal(ZWave.BasicApplication.Enums.NodeStatuses.LearnReady, 1000);
            var learnToken = secondCtrl.SetLearnMode(LearnModes.LearnModeClassic, 1000, null);

            var removeRes = (AddRemoveNodeResult)removeToken.WaitCompletedSignal();
            var learnRes = (SetLearnModeResult)learnToken.WaitCompletedSignal();

            secondCtrl.MemoryGetId();

            // Assert
            Assert.IsTrue(removeRes);
            Assert.AreEqual(0x02, removeRes.Id);
            Assert.IsTrue(learnRes);
            Assert.AreEqual(0x01, secondCtrl.Id);

            firstCtrl.Dispose();
            secondCtrl.Dispose();
        }
    }
}
