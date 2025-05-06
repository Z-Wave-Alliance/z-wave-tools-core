using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using ZWave;
using ZWave.BasicApplication;

namespace BasicApplicationTests.XModem
{
    [TestFixture]
    public class XModemFrameClientTests
    {
        private const int CHUNK_SIZE = 128;
        private byte[] _datagram = new byte[] { 0x02, 0x01, 0xFA, 0xB3, 0x01, 0x12, 0x41, 0x5D, 0xA5, 0xB2, 0xEC, 0xDE, 0x07, 0xBE, 0x17, 0x05,
                0x73, 0x9B, 0xD6, 0xAB, 0x2A, 0xFE, 0x8F, 0xFB, 0xF7, 0xC7, 0xFE, 0x69, 0xC3, 0x5A, 0xE3, 0xA8,
                0x34, 0x5F, 0xBC, 0x5E, 0xD8, 0xF6, 0xD9, 0xA5, 0x7D, 0x65, 0x59, 0xCA, 0x0F, 0x9E, 0xAE, 0xA9,
                0x2B, 0xB9, 0x9E, 0x8D, 0xF5, 0x80, 0xC7, 0x8C, 0xA3, 0x58, 0x03, 0x33, 0xA8, 0x3C, 0x7E, 0x73,
                0x97, 0x38, 0xBC, 0xC8, 0xA5, 0x2C, 0x9F, 0x1A, 0x7B, 0x68, 0x29, 0xFF, 0x30, 0x81, 0x96, 0xBD,
                0xBE, 0x05, 0x2A, 0x8B, 0xB0, 0x4E, 0x53, 0x7E, 0x2F, 0xE0, 0xDB, 0xD7, 0x36, 0x7C, 0x7C, 0xB3,
                0x14, 0xD7, 0x84, 0x95, 0xD8, 0xD5, 0x5F, 0x09, 0x38, 0x92, 0xBF, 0xCA, 0x51, 0xF5, 0x19, 0xA8,
                0x19, 0xFB, 0x3D, 0xE9, 0xD7, 0x8D, 0x31, 0xCA, 0x3D, 0xA2, 0xF8, 0xA0, 0x48, 0xA3, 0x4D, 0xFD,
                0x0B, 0xE6, 0xE8, 0x1B, 0xF3, 0xF2, 0x32, 0xFA, 0x7C, 0x4A, 0x49, 0xC7, 0xE7, 0x18, 0xB0, 0x0D,
                0x96, 0x2D, 0xFF, 0x02, 0x62, 0x99, 0x77, 0xEC, 0x29, 0x4F, 0xBE, 0x19, 0xE9, 0x04, 0xE7, 0xBA,
                0x7B, 0xD6, 0xAA, 0x0A, 0xCC, 0xE3, 0x75, 0x2B, 0xD2, 0x0C, 0xCE, 0xEF, 0x79, 0x30, 0x45, 0x41,
                0xA7, 0xF4, 0x59, 0x39, 0xBA, 0x9E, 0x44, 0x6C, 0x36, 0xED, 0x9D, 0xEF, 0x2A, 0xE4, 0x3B, 0x06,
                0x0B, 0xE6, 0xE8, 0x1B, 0xF3, 0xF2, 0x32, 0xFA, 0x7C, 0x4A, 0x49, 0xC7, 0xE7, 0x28, 0xB0, 0x0D,
                0x96, 0x2D, 0xFF, 0x02, 0x62, 0x99, 0x77, 0xEC, 0x29, 0x4F, 0xBE, 0x19, 0xE9, 0x04, 0xE7, 0xBA,
                0x11, 0xD6, 0xAA, 0x0A, 0xCC, 0xE3, 0x75, 0x2B, 0xD2, 0x0C, 0xCE, 0xEF, 0x79, 0x30, 0x45, 0x41,
                0xA7, 0xF4, 0x59, 0x39, 0xBA, 0x9E, 0x44, 0x6C, 0x36, 0xED, 0x9D, 0xEF, 0x2A, 0xE4, 0x3B, 0x06,
                0x02, 0x01, 0xFA, 0xB3, 0x01, 0x12, 0x41, 0x5D, 0xA5, 0xB2, 0xEC, 0xDE, 0x07, 0xBE, 0x17, 0x05,
                0x73, 0x9B, 0xD6, 0xAB, 0x2A, 0xFE, 0x8F, 0xFB, 0xF7, 0xC7, 0xFE, 0x69, 0xC3, 0x5A, 0xE3, 0xA8,
                0x34, 0x5F, 0xBC, 0x5E, 0xD8, 0xF6, 0xD9, 0xA5, 0x7D, 0x65, 0x59, 0xCA, 0x0F, 0x9E, 0xAE, 0xA9,
                0x2B, 0xB9, 0x9E, 0x8D, 0xF5, 0x80, 0xC7, 0x8C, 0xA3, 0x58, 0x03, 0x33, 0xA8, 0x3C, 0x7E, 0x73,
                0x97, 0x38, 0xBC, 0xC8, 0xA5, 0x2C, 0x9F, 0x1A, 0x7B, 0x68, 0x29, 0xFF, 0x30, 0x81, 0x96, 0xBD,
                0xBE, 0x05, 0x2A, 0x8B, 0xB0, 0x4E, 0x53, 0x7E, 0x2F, 0xE0, 0xDB, 0xD7, 0x36, 0x7C, 0x7C, 0xB3,
                0x14, 0xD7, 0x84, 0x95, 0xD8, 0xD5, 0x5F, 0x09, 0x38, 0x92, 0xBF, 0xCA, 0x51, 0xF5, 0x19, 0xA8,
                0x19, 0xFB, 0x3D, 0xE9, 0xD7, 0x8D, 0x31, 0xCA, 0x3D, 0xA2, 0xF8, 0xA0, 0x48, 0xA3, 0x4D, 0xFD,
                0x0B, 0xE6, 0xE8, 0x1B, 0xF3, 0xF2, 0x32, 0xFA, 0x7C, 0x4A, 0x49, 0xC7, 0xE7, 0x28, 0xB0, 0x0D,
                0x96, 0x2D, 0xFF, 0x02, 0x62, 0x99, 0x77, 0xEC, 0x29, 0x4F, 0xBE, 0x19, 0xE9, 0x04, 0xE7, 0xBA,
                0x7B, 0xD6, 0xAA, 0x0A, 0x1C, 0xFF, 0x75, 0x2B, 0xD2, 0x0C, 0xCE, 0xEF, 0x79, 0x30, 0x45, 0x41,
                0x00, 0xF4, 0x59, 0x39, 0xBA, 0x9E, 0x44, 0x6C, 0x36, 0xED, 0x9D, 0xEF, 0x2A, 0xE4, 0x3B, 0x06,
                0x0B, 0xE6, 0xE8, 0x1B, 0xF3, 0xF2, 0x32, 0xFA, 0x7C, 0x4A, 0x49, 0xC7, 0xE7, 0x28, 0xB0, 0x0D,
                0x96, 0x2D, 0xFF, 0x02, 0x62, 0x99, 0x77, 0xEC, 0x29, 0x4F, 0xBE, 0x19, 0xE9, 0x04, 0xE7, 0xAA,
                0x7B, 0xD6, 0xAA, 0x0A, 0xCC, 0xE3, 0x75, 0x2B, 0xD2, 0x0C, 0xCE, 0xEF, 0x79, 0x30, 0x45, 0x41,
                0xA7, 0xF4, 0x59, 0x39, 0xBA, 0x9E, 0x44, 0x6C, 0x36, 0xED, 0x9D, 0xEF, 0x2A, 0xE4, 0x3B, 0x60 };

        private XModemFrameClient _xModemFrameClient;
        private FakeXModemTransportClient _transportClientFake;

        [SetUp]
        public void Setup()
        {
            _transportClientFake = new FakeXModemTransportClient();
            var XModemFrameLayer = new XModemFrameLayer();

            _xModemFrameClient = (XModemFrameClient)XModemFrameLayer.CreateClient(1);
            _xModemFrameClient.SendDataCallback = _transportClientFake.WriteData;
            _transportClientFake.ReceiveDataCallback = _xModemFrameClient.HandleData;
        }

        [Test]
        public void SendInit_DeviceResponses_FrameClientReadyForUpload()
        {
            // Arrange.

            // Act.
            var task = Task.Factory.StartNew(() => _xModemFrameClient.WaitReady(4000));
            _transportClientFake.WriteData(_transportClientFake.InitFrame);

            // Assert.
            Assert.IsTrue(task.Result);
            Assert.IsTrue(_xModemFrameClient.IsReady);
        }

        [Test]
        public void CloseSession_DeviceResponses_FrameClientIsClosed()
        {
            // Arrange.
            var task = Task.Factory.StartNew(() => _xModemFrameClient.WaitReady(1000));
            _xModemFrameClient.TestSetReady();
            task.Wait();
            _transportClientFake.SetUploadNewFirmwareState();

            // Act.
            var isClosed = _xModemFrameClient.CloseSession(1000);

            // Assert.
            Assert.IsTrue(isClosed);
            Assert.IsTrue(_xModemFrameClient.IsCompleted);
            Assert.IsTrue(_transportClientFake.IsEotReceived);
        }

        [Test]
        public void UploadData_DeviceResponsesWithAcks_DataUploaded()
        {
            // Arrange.
            bool actionRes = true;
            var task = Task.Factory.StartNew(() => _xModemFrameClient.WaitReady(1000));
            _xModemFrameClient.TestSetReady();
            task.Wait();
            _transportClientFake.SetUploadNewFirmwareState();

            // Act.
            for (int i = 0; i < _datagram.Length / CHUNK_SIZE && actionRes; i++)
            {
                actionRes &= SendFrame(_datagram.Skip(i * CHUNK_SIZE).Take(CHUNK_SIZE).ToArray());
            }
            var sessionRes = _xModemFrameClient.CloseSession(1000);

            // Assert.
            Assert.IsTrue(actionRes);
            Assert.IsTrue(sessionRes);
            Assert.IsTrue(_xModemFrameClient.IsCompleted);
            Assert.IsTrue(_transportClientFake.ReceivedPayload.SequenceEqual(_datagram));
            Assert.IsTrue(_transportClientFake.IsEotReceived);
        }

        [Test]
        public void UploadLifecycle_DeviceResponses_UploadsFirmwareAndRunsApp()
        {
            // Arrange.
            bool actionRes = true;

            // Act.
            var task = Task.Factory.StartNew(() => _xModemFrameClient.WaitReady(4000));
            _transportClientFake.WriteData(_transportClientFake.InitFrame);
            Assert.IsTrue(task.Result);
            Assert.IsTrue(_xModemFrameClient.IsReady);
            for (int i = 0; i < _datagram.Length / CHUNK_SIZE && actionRes; i++)
            {
                actionRes &= SendFrame(_datagram.Skip(i * CHUNK_SIZE).Take(CHUNK_SIZE).ToArray());
            }
            var sessionRes = _xModemFrameClient.CloseSession(1000);

            // Assert.
            Assert.IsTrue(actionRes);
            Assert.IsTrue(_xModemFrameClient.IsCompleted);
            Assert.IsTrue(sessionRes);
            Assert.IsTrue(_transportClientFake.ReceivedPayload.SequenceEqual(_datagram));
            Assert.IsTrue(_transportClientFake.IsEotReceived);
            Assert.AreEqual(1, _transportClientFake.RunAppReceivedCount);
        }

        [Test]
        public void UploadData_DeviceResponsesWithNack_FrameRetransmitted()
        {
            // Arrange.
            bool actionRes = true;
            _transportClientFake.SendNackAfter(3);
            _transportClientFake.SetUploadNewFirmwareState();
            var task = Task.Factory.StartNew(() => _xModemFrameClient.WaitReady(1000));
            _xModemFrameClient.TestSetReady();
            task.Wait();

            // Act.
            for (int i = 0; i < _datagram.Length / CHUNK_SIZE && actionRes; i++)
            {
                actionRes &= SendFrame(_datagram.Skip(i * CHUNK_SIZE).Take(CHUNK_SIZE).ToArray());
            }
            var sessionRes = _xModemFrameClient.CloseSession(1000);

            // Assert.
            Assert.IsTrue(actionRes);
            Assert.IsTrue(sessionRes);
            Assert.IsTrue(_xModemFrameClient.IsCompleted);
            Assert.IsTrue(_transportClientFake.ReceivedPayload.SequenceEqual(_datagram));
            Assert.AreEqual(_datagram.Length / CHUNK_SIZE + 1, _transportClientFake.ReceivedFramesCount);
            Assert.IsTrue(_transportClientFake.IsEotReceived);
        }

        [Test]
        public void UploadData_DeviceResponsesWithCan_UploadFailsWithMessageTriesRunApp()
        {
            // Arrange.
            int expectedRunAppTries = 3;
            bool actionRes = true;
            _transportClientFake.SendCanAfter(3);
            _transportClientFake.CanStart = false;
            _transportClientFake.SetUploadNewFirmwareState();
            var task = Task.Factory.StartNew(() => _xModemFrameClient.WaitReady(1000));
            _xModemFrameClient.TestSetReady();
            task.Wait();

            // Act.
            for (int i = 0; i < _datagram.Length / CHUNK_SIZE; i++)
            {
                actionRes &= SendFrame(_datagram.Skip(i * CHUNK_SIZE).Take(CHUNK_SIZE).ToArray());
            }
            var sessionRes = _xModemFrameClient.CloseSession(1000);

            // Assert.
            Assert.IsFalse(actionRes);
            Assert.IsFalse(sessionRes);
            Assert.IsFalse(_xModemFrameClient.IsCompleted);
            Assert.IsFalse(_transportClientFake.IsEotReceived);
            Assert.AreEqual(_xModemFrameClient.CancelationMessage, "error code 0xFE");
            Assert.AreEqual(_xModemFrameClient.ErrorCode, 254);
            Assert.AreEqual(3, _transportClientFake.ReceivedFramesCount);
            Assert.AreEqual(expectedRunAppTries, _transportClientFake.RunAppReceivedCount);
        }

        [Test]
        public void UploadData_DeviceDoesntResponseWithin2Acks_FrameRetransmitted()
        {
            // Arrange.
            bool actionRes = true;
            int frameNo = 3;
            int skipedAcksCount = 2;
            _transportClientFake.SkipAcksAfter(frameNo, skipedAcksCount);
            _transportClientFake.SetUploadNewFirmwareState();
            var task = Task.Factory.StartNew(() => _xModemFrameClient.WaitReady(1000));
            _xModemFrameClient.TestSetReady();
            task.Wait();

            // Act.
            for (int i = 0; i < _datagram.Length / CHUNK_SIZE && actionRes; i++)
            {
                actionRes &= SendFrame(_datagram.Skip(i * CHUNK_SIZE).Take(CHUNK_SIZE).ToArray());
            }
            var sessionRes = _xModemFrameClient.CloseSession(1000);

            // Assert.
            Assert.IsTrue(actionRes);
            Assert.IsTrue(sessionRes);
            Assert.IsTrue(_xModemFrameClient.IsCompleted);
            Assert.IsTrue(_transportClientFake.IsEotReceived);
            Assert.IsTrue(_transportClientFake.ReceivedPayload.SequenceEqual(_datagram));
            Assert.AreEqual(_datagram.Length / CHUNK_SIZE + 2, _transportClientFake.ReceivedFramesCount);
        }

        [Test]
        public void UploadData_DeviceDoesntResponseWithin3Acks_UploadFails()
        {
            // Arrange.
            bool actionRes = true;
            int frameNo = 3;
            int skipedAcksCount = 5;
            _transportClientFake.SkipAcksAfter(frameNo, skipedAcksCount);
            _transportClientFake.SetUploadNewFirmwareState();
            var task = Task.Factory.StartNew(() => _xModemFrameClient.WaitReady(1000));
            _xModemFrameClient.TestSetReady();
            task.Wait();
            
            // Act.
            for (int i = 0; i < (_datagram.Length / CHUNK_SIZE) && actionRes; i++)
            {
                actionRes &= SendFrame(_datagram.Skip(i * CHUNK_SIZE).Take(CHUNK_SIZE).ToArray());
            }
            var sessionRes = _xModemFrameClient.CloseSession(1000);

            // Assert.
            Assert.IsFalse(actionRes);
            Assert.IsFalse(sessionRes);
            Assert.IsFalse(_xModemFrameClient.IsCompleted);
            Assert.AreEqual(frameNo + 2 /*retransmission*/, _transportClientFake.ReceivedFramesCount);
        }

        [Test]
        public void UploadData_UploadedDataNot128Multiple_LastChunkAppendedWith0()
        {
            // Arrange.
            bool actionRes = true;
            const int zeroesPaddingCount = 30;
            var newDatagram = _datagram.Take(_datagram.Length - zeroesPaddingCount).ToArray();
            _transportClientFake.SetUploadNewFirmwareState();
            var task = Task.Factory.StartNew(() => _xModemFrameClient.WaitReady(1000));
            _xModemFrameClient.TestSetReady();
            task.Wait();

            // Act.
            int sent = 0;
            while (sent < newDatagram.Length && actionRes)
            {
                var chunkSize = (sent + CHUNK_SIZE) <= newDatagram.Length ? CHUNK_SIZE : newDatagram.Length - sent;
                actionRes &= SendFrame(newDatagram.Skip(sent).Take(chunkSize).ToArray());
                sent += chunkSize;
            }
            var sessionRes = _xModemFrameClient.CloseSession(1000);

            // Assert.
            Assert.IsTrue(actionRes);
            Assert.IsTrue(sessionRes);
            Assert.IsTrue(_xModemFrameClient.IsCompleted);
            Assert.IsTrue(_transportClientFake.IsEotReceived);
            Array.Resize(ref newDatagram, newDatagram.Length + zeroesPaddingCount);
            Assert.IsTrue(_transportClientFake.ReceivedPayload.SequenceEqual(newDatagram));
        }

        private bool SendFrame(byte[] frame)
        {
            var ahr = new ActionHandlerResult(new FakeAction(true, null, 0, null));
            ahr.NextActions.Add(new CommandMessage() { Data = frame });
            return _xModemFrameClient.SendFrames(ahr);
        }
    }
}