using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Utils;
using ZWave;
using ZWave.BasicApplication.Devices;
using ZWave.BasicApplication.Operations;
using ZWave.BasicApplication.TransportService;
using ZWave.BasicApplication.TransportService.Operations;
using ZWave.CommandClasses;
using ZWave.Devices;

namespace BasicApplicationTests.TransportService
{
    [TestFixture]
    public class TransportServiceTests : TestBase
    {
        private const byte SESSION_ID = 0x07;
        private readonly int _segmentsCout = DataProvider.SegmentCmds.Count;

        private const int TIMEOUT_PER_FRAME = 222;

        private Func<NodeTag, bool> CreateAlwaysTrueFakeSendDataSubstitutionCallback()
        {
            return new Func<NodeTag, bool>(x => (true));
        }

        [SetUp]
        public void SetUp()
        {
            _ctrlFirst.SessionClient.AddSubstituteManager(
                new TransportServiceManager(new NetworkViewPoint() { TransportServiceMaxSegmentSize = DataProvider.MAX_FRAME_SIZE },
                    new TransportServiceManagerInfo(TXO,
                        CreateAlwaysTrueFakeSendDataSubstitutionCallback())));

            _ctrlSecond.SessionClient.AddSubstituteManager(
                new TransportServiceManager(new NetworkViewPoint() { TransportServiceMaxSegmentSize = DataProvider.MAX_FRAME_SIZE },
                    new TransportServiceManagerInfo(TXO,
                        CreateAlwaysTrueFakeSendDataSubstitutionCallback())));

            _transport.SetUpModulesNetwork(_ctrlFirst.SessionId, _ctrlSecond.SessionId, _ctrlThird.SessionId);
        }

        private TransportServiceManager GetTransportServiceManager(Controller ctrl)
        {
            return (TransportServiceManager)ctrl.SessionClient.GetSubstituteManager(typeof(TransportServiceManager));
        }

        [Test]
        public void A_SendLongDataFrame_DefaultCase_DataFrameSuccessfullyReasembled()
        {
            // Arrange.
            var expectToken = _ctrlSecond.ExpectData(DataProvider.Datagram.ToArray(), _segmentsCout * TIMEOUT_PER_FRAME, null);

            // Act.
            var sendDataRes = _ctrlFirst.SendData(NODE_ID_2, DataProvider.Datagram.ToArray(), TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsTrue(sendDataRes.State == ActionStates.Completed);
            Assert.IsTrue(DataProvider.Datagram.ToArray().SequenceEqual(expectRes.Command));
        }

        [Test]
        public void SendDataFrame_FirstSegmentContainsAllData_DataFrameSuccessfullyReasembled()
        {
            // Arrange.
            var firstSegment = new byte[] { 0x55, 0xC0, 0x02, 0x10, COMMAND_CLASS_VERSION_V2.ID, COMMAND_CLASS_VERSION_V2.VERSION_GET.ID, 0x2C, 0x36 };
            byte[] versionGetCmd = new COMMAND_CLASS_VERSION_V2.VERSION_GET();
            var expectToken = _ctrlSecond.ExpectData(versionGetCmd, _segmentsCout * TIMEOUT_PER_FRAME, null);

            // Act.
            var sendDataRes = _ctrlFirst.SendData(NODE_ID_2, firstSegment, TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsTrue(sendDataRes.State == ActionStates.Completed);
            Assert.IsTrue(versionGetCmd.SequenceEqual(expectRes.Command));
        }

        [Test]
        public void RequestLongDataFrame_DefaultCase_DataFrameSuccessfullyReasembled()
        {
            // Arrange.
            var basicReportCmd = new COMMAND_CLASS_BASIC.BASIC_REPORT();
            var basicGetCmd = new COMMAND_CLASS_BASIC.BASIC_GET();
            var longBasicGetCmd = new byte[DataProvider.Datagram.Count];
            DataProvider.Datagram.CopyTo(longBasicGetCmd, 0);
            longBasicGetCmd[0] = COMMAND_CLASS_BASIC.ID;
            longBasicGetCmd[1] = COMMAND_CLASS_BASIC.BASIC_GET.ID;

            // Act.
            var expectToken = _ctrlSecond.ResponseData(basicReportCmd, TXO, longBasicGetCmd);
            var res = _ctrlFirst.RequestData(NODE_ID_2, basicGetCmd, TXO, basicReportCmd, _segmentsCout * TIMEOUT_PER_FRAME);
            var resLong = _ctrlFirst.RequestData(NODE_ID_2, longBasicGetCmd, TXO, basicReportCmd, _segmentsCout * TIMEOUT_PER_FRAME);

            // Assert.
            Assert.IsTrue(res.IsStateCompleted);
            Assert.IsTrue(resLong.IsStateCompleted);
        }

        [Test]
        public void SendLongDataFrame_LosingFirstFragment_WaitSegmentIsSentAndDataFrameIsReasembled()
        {
            // Arrange.
            var expectToken = _ctrlSecond.ExpectData(DataProvider.Datagram.ToArray(),
                _segmentsCout * SendDataTransportTask.WAIT_TIMEOUT + _segmentsCout * TIMEOUT_PER_FRAME, null);
            GetTransportServiceManager(_ctrlFirst).
                TransportServiceManagerInfo.
                SetTestFirstFragmentCRC16(new byte[] { 0xff, 0xff }, 1);

            // Act.
            var sendDataRes = _ctrlFirst.SendData(NODE_ID_2, DataProvider.Datagram.ToArray(), TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsTrue(sendDataRes.State == ActionStates.Completed);
            Assert.IsTrue(DataProvider.Datagram.ToArray().SequenceEqual(expectRes.Command));
        }

        [Test]
        public void SendLongDataFrame_ReceiverIgnoresFirstFragment_WaitSegmentIsSentAndDataFrameIsReasembled()
        {
            // Arrange.
            var expectToken = _ctrlSecond.ExpectData(DataProvider.Datagram.ToArray(),
                _segmentsCout * SendDataTransportTask.WAIT_TIMEOUT + _segmentsCout * TIMEOUT_PER_FRAME, null);
            GetTransportServiceManager(_ctrlSecond).
                TransportServiceManagerInfo.
                SetTestIgnoreFirstSegment(1);

            // Act.
            var sendDataRes = _ctrlFirst.SendData(NODE_ID_2, DataProvider.Datagram.ToArray(), TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsTrue(sendDataRes.State == ActionStates.Completed);
            Assert.IsTrue(DataProvider.Datagram.ToArray().SequenceEqual(expectRes.Command));
        }

        [Test]
        public void SendLongDataFrame_LosingSubsequentFragment_DataFrameIsReasembledAfterSegmentRequest()
        {
            // Arrange.
            var expectToken = _ctrlSecond.ExpectData(DataProvider.Datagram.ToArray(),
                _segmentsCout * TIMEOUT_PER_FRAME + 2 * TIMEOUT_PER_FRAME, null);
            GetTransportServiceManager(_ctrlFirst).
                TransportServiceManagerInfo.
                SetTestSubsequentFragmentCRC16(new byte[] { 0xff, 0xff }, DataProvider.MAX_FRAME_SIZE * 3, 1);

            // Act.
            var sendDataRes = _ctrlFirst.SendData(NODE_ID_2, DataProvider.Datagram.ToArray(), TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsTrue(sendDataRes.State == ActionStates.Completed);
            Assert.IsTrue(DataProvider.Datagram.ToArray().SequenceEqual(expectRes.Command));
        }

        [Test]
        public void SendLongDataFrame_ReceiverIgnoresSubsequentFragment_DataFrameIsReasembledAfterSegmentRequest()
        {
            // Arrange.
            var expectToken = _ctrlSecond.ExpectData(DataProvider.Datagram.ToArray(),
                _segmentsCout * TIMEOUT_PER_FRAME + 2 * TIMEOUT_PER_FRAME, null);
            GetTransportServiceManager(_ctrlSecond).
                TransportServiceManagerInfo.
                SetTestIgnoreSubsequentSegment(DataProvider.MAX_FRAME_SIZE * 3, 1);

            // Act.
            var sendDataRes = _ctrlFirst.SendData(NODE_ID_2, DataProvider.Datagram.ToArray(), TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsTrue(sendDataRes.State == ActionStates.Completed);
            Assert.IsTrue(DataProvider.Datagram.ToArray().SequenceEqual(expectRes.Command));
        }

        [Test]
        public void SendLongDataFrame_LosingSegmentComplete_RetransmitsLastSegmentAndGetSegmentComplete()
        {
            // Arrange.
            var expectToken = _ctrlSecond.ExpectData(DataProvider.Datagram.ToArray(),
                _segmentsCout * TIMEOUT_PER_FRAME + DefaultTimeouts.TRANSPORT_SERVICE_SEGMENT_COMPLETE_TIMEOUT + 2 * TIMEOUT_PER_FRAME, null);

            var invalidSegCompleteCmd = new COMMAND_CLASS_TRANSPORT_SERVICE_V2.COMMAND_SEGMENT_COMPLETE { properties2 = 0xE0 };
            var expectWrongSegmentCompleteToken = _ctrlFirst.ExpectData(invalidSegCompleteCmd, ((byte[])invalidSegCompleteCmd).Length,
                _segmentsCout * TIMEOUT_PER_FRAME + DefaultTimeouts.TRANSPORT_SERVICE_SEGMENT_COMPLETE_TIMEOUT + 2 * TIMEOUT_PER_FRAME, null);
            var correctSegCompleteCmd = new COMMAND_CLASS_TRANSPORT_SERVICE_V2.COMMAND_SEGMENT_COMPLETE { properties2 = 0x10 };
            var expectCorrrectSegmentCompleteToken = _ctrlFirst.ExpectData(correctSegCompleteCmd, ((byte[])correctSegCompleteCmd).Length,
                _segmentsCout * TIMEOUT_PER_FRAME + DefaultTimeouts.TRANSPORT_SERVICE_SEGMENT_COMPLETE_TIMEOUT + 2 * TIMEOUT_PER_FRAME, null);

            GetTransportServiceManager(_ctrlSecond).
                TransportServiceManagerInfo.
                SetTestSegmentCompleteCmdSessionId(0x0E, 1);

            // Act.
            var sendDataRes = _ctrlFirst.SendData(NODE_ID_2, DataProvider.Datagram.ToArray(), TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            var expectWrongSegmentCompleteRes = (ExpectDataResult)expectWrongSegmentCompleteToken.WaitCompletedSignal();
            var expectCorrrectSegmentCompleteRes = (ExpectDataResult)expectCorrrectSegmentCompleteToken.WaitCompletedSignal();

            // Assert.
            Assert.IsTrue(expectWrongSegmentCompleteRes);
            Assert.IsTrue(expectCorrrectSegmentCompleteRes);
            Assert.IsTrue(sendDataRes.State == ActionStates.Completed);
            Assert.IsTrue(DataProvider.Datagram.ToArray().SequenceEqual(expectRes.Command));
        }

        [Test]
        public void SendLongDataFrame_LosingLastFragment_DataFrameIsReasembledAfterSegmentRequest()
        {
            // Arrange.
            var expectToken = _ctrlSecond.ExpectData(DataProvider.Datagram.ToArray(),
                _segmentsCout * TIMEOUT_PER_FRAME + DefaultTimeouts.TRANSPORT_SERVICE_SEGMENT_COMPLETE_TIMEOUT + 2 * TIMEOUT_PER_FRAME, null);
            GetTransportServiceManager(_ctrlFirst).
                TransportServiceManagerInfo.
                SetTestSubsequentFragmentCRC16(new byte[] { 0xff, 0xff }, DataProvider.Datagram.ToArray().Length - 1, 1);

            // Act.
            var sendDataRes = _ctrlFirst.SendData(NODE_ID_2, DataProvider.Datagram.ToArray(), TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsTrue(sendDataRes.State == ActionStates.Completed);
            Assert.IsTrue(DataProvider.Datagram.ToArray().SequenceEqual(expectRes.Command));
        }

        [Test]
        public void SendLongDataFrame_ReceiverIgnoresLastFragment_DataFrameIsReasembledAfterSegmentRequest()
        {
            // Arrange.
            var expectToken = _ctrlSecond.ExpectData(DataProvider.Datagram.ToArray(),
                _segmentsCout * TIMEOUT_PER_FRAME + DefaultTimeouts.TRANSPORT_SERVICE_SEGMENT_COMPLETE_TIMEOUT + 2 * TIMEOUT_PER_FRAME, null);
            GetTransportServiceManager(_ctrlSecond).
                TransportServiceManagerInfo.
                SetTestIgnoreSubsequentSegment(DataProvider.Datagram.ToArray().Length - 1, 1);

            // Act.
            var sendDataRes = _ctrlFirst.SendData(NODE_ID_2, DataProvider.Datagram.ToArray(), TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsTrue(sendDataRes.State == ActionStates.Completed);
            Assert.IsTrue(DataProvider.Datagram.ToArray().SequenceEqual(expectRes.Command));
        }

        [Test]
        [Ignore("Not fixed yet")]
        public void SendLongDataFrame_SenderTurnedOff_ReceiverDiscardsAllReceivedFragments()
        {
            // Arrange.
            var expectToken = _ctrlSecond.ExpectData(DataProvider.Datagram.ToArray(),
                _segmentsCout * TIMEOUT_PER_FRAME + 2 * DefaultTimeouts.TRANSPORT_SERVICE_SEGMENT_COMPLETE_TIMEOUT + 2 * TIMEOUT_PER_FRAME, null);

            // Act.
            var sendDataRes = _ctrlFirst.SendData(NODE_ID_2, DataProvider.Datagram.ToArray(), TXO, new SubstituteSettings(), null);
            Thread.Sleep(100);

            _ctrlFirst.SetRFReceiveMode(0);

            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsNull(expectRes.Command);
            Assert.IsNull(GetTransportServiceManager(_ctrlSecond).ReassemblingData);
            Assert.AreEqual(0, GetTransportServiceManager(_ctrlSecond).SrcNode);
            Assert.AreEqual(NODE_ID_1.Id, GetTransportServiceManager(_ctrlSecond).NodeIdWaitResponded); // Wait(0) was sent to sender.
        }

        [Test]
        public void SendLongDataFrame_ReceiverTurnedOff_SenderRetransmitsLastFragmentAndFailed()
        {
            // Arrange.
            var expectToken = _ctrlSecond.ExpectData(DataProvider.Datagram.ToArray(),
                _segmentsCout * TIMEOUT_PER_FRAME + 2 * DefaultTimeouts.TRANSPORT_SERVICE_SEGMENT_COMPLETE_TIMEOUT + 2 * TIMEOUT_PER_FRAME, null);

            // Act.
            var sendDataRes = _ctrlFirst.SendData(NODE_ID_2, DataProvider.Datagram.ToArray(), TXO, new SubstituteSettings(), null);

            //Thread.Sleep(100);

            _ctrlSecond.SetRFReceiveMode(0);

            sendDataRes.WaitCompletedSignal();
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsTrue(sendDataRes.State == ActionStates.Failed);
        }

        [Test]
        public void SendLongDataFrame_ReceiverIsReasembelingData_SenderWaitsAndRepeatsSession()
        {
            // Arrange.
            NodeTag tempCtrlNode = new NodeTag(0x03);
            _ctrlThird.SessionClient.AddSubstituteManager(
                new TransportServiceManager(new NetworkViewPoint() { TransportServiceMaxSegmentSize = DataProvider.MAX_FRAME_SIZE },
                    new TransportServiceManagerInfo(TXO,
                        CreateAlwaysTrueFakeSendDataSubstitutionCallback())));

            // Act.
            var expectToken1 = _ctrlSecond.ExpectData(NODE_ID_1, DataProvider.Datagram.ToArray(), _segmentsCout * TIMEOUT_PER_FRAME, null);
            var sendDataToken1 = _ctrlFirst.SendData(NODE_ID_2, DataProvider.Datagram.ToArray(), TXO, new SubstituteSettings(), null);
            Thread.Sleep(100);
            var sendDataToken2 = _ctrlThird.SendData(NODE_ID_2, DataProvider.Datagram.ToArray(), TXO, new SubstituteSettings(), null);

            var sendDataRes1 = sendDataToken1.WaitCompletedSignal();
            var expectToken2 = _ctrlSecond.ExpectData(tempCtrlNode, DataProvider.Datagram.ToArray(),
                _segmentsCout * TIMEOUT_PER_FRAME + _segmentsCout * SendDataTransportTask.WAIT_TIMEOUT, null);

            var sendDataRes2 = sendDataToken2.WaitCompletedSignal();
            var expectRes1 = (ExpectDataResult)expectToken1.WaitCompletedSignal();
            var expectRes2 = (ExpectDataResult)expectToken2.WaitCompletedSignal();

            // Assert.
            Assert.IsTrue(sendDataRes1.State == ActionStates.Completed);
            Assert.IsTrue(sendDataRes2.State == ActionStates.Completed);
            Assert.IsTrue(DataProvider.Datagram.ToArray().SequenceEqual(expectRes1.Command));
            Assert.IsTrue(DataProvider.Datagram.ToArray().SequenceEqual(expectRes2.Command));
        }

        [Test]
        public void RequestLongDataFrame_SendFirstFragmentTwice_SegmentRequestNotSent()
        {
            // Arrange.
            var expectToken = _ctrlSecond.ExpectData(DataProvider.Datagram.ToArray(),
                _segmentsCout * SendDataTransportTask.WAIT_TIMEOUT + _segmentsCout * TIMEOUT_PER_FRAME, null);
            var requestExpectToken =
                _ctrlFirst.ExpectData(new COMMAND_CLASS_TRANSPORT_SERVICE_V2.COMMAND_SEGMENT_REQUEST(),
                    TransportServiceManager.FRAGMENT_RX_TIMEOUT + 200, null);
            var firstFragmentBytes = Tools.GetBytes("55 C0 3A 00 9F 03 C6 00 DB 0C C7 D8 F9 65 A9 BC 88 A7 E0 A7 DD");
            var crc16 = Tools.GetBytes(Tools.ZW_CreateCrc16(null, 0, firstFragmentBytes, (byte)firstFragmentBytes.Length));
            var allBytes = new List<byte>(firstFragmentBytes);
            allBytes.AddRange(crc16);


            // Act.
            var sendFirstSegmentRes = _ctrlFirst.SendData(NODE_ID_2, allBytes.ToArray(), TXO);
            Thread.Sleep(TransportServiceManager.FRAGMENT_RX_TIMEOUT - 200);

            var sendDataRes = _ctrlFirst.SendData(NODE_ID_2, DataProvider.Datagram.ToArray(), TXO);
            var expectRes = (ExpectDataResult)expectToken.WaitCompletedSignal();
            var requestExpectRes = (ExpectDataResult)requestExpectToken.WaitCompletedSignal();

            // Assert.
            Assert.IsTrue(sendDataRes.State == ActionStates.Completed);
            Assert.IsTrue(DataProvider.Datagram.ToArray().SequenceEqual(expectRes.Command));
            Assert.IsFalse(requestExpectRes);
        }

        [Test]
        public void SendLongDataFrame_RxTimedOut_SenderReceivesWait0AndRestartsTransmission()
        {
            // Arrange.
            _transport.AddSendDataDelay(_ctrlFirst.SessionId, 3, 1000);
            var expectDataToken = _ctrlSecond.ExpectData(DataProvider.Datagram.ToArray(), 10000, null);
            var expectWaitToken = _ctrlFirst.ExpectData(new COMMAND_CLASS_TRANSPORT_SERVICE_V2.COMMAND_SEGMENT_WAIT(), 6000, null);

            // Act.
            _ctrlFirst.SendData(NODE_ID_2, DataProvider.Datagram.ToArray(), TXO, new SubstituteSettings(), null);

            var expectWaitRes = (ExpectDataResult)expectWaitToken.WaitCompletedSignal();
            var expectDataRes = (ExpectDataResult)expectDataToken.WaitCompletedSignal();


            // Assert.
            Assert.IsTrue(expectWaitRes);
            var waitCmd = (COMMAND_CLASS_TRANSPORT_SERVICE_V2.COMMAND_SEGMENT_WAIT)expectWaitRes.Command;
            Assert.AreEqual(0, waitCmd.pendingFragments.Value);
            Assert.IsTrue(expectDataRes);
        }
    }
}