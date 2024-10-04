/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using ZWave.Layers;
using ZWave.Layers.Transport;
using ZWave.Enums;
using Moq;

namespace ZWaveTests
{
    //[TestFixture]
    public class SerialPortTransportClientTests
    {
        private ITransportClient _serialPortTransportClient;
        private byte[] _data = new byte[] { 0x0, 0x1, 0x2 };
        private const int _sessionId = 0x79;
        private const ApiTypes _apiType = ApiTypes.Basic;
        private DataChunk _transmitedDataChunk;
        private DataChunk _receivedDataChunk;

        [SetUp]
        public void SetUp()
        {
            _serialPortTransportClient = new SerialPortTransportClient((x) => _transmitedDataChunk = x)
            {
                DataSource = new SerialPortDataSource("COM1"),
                SessionId = _sessionId,
                ApiType = _apiType
            };
            _transmitedDataChunk = null;
            _receivedDataChunk = null;
        }

        [TearDown]
        public void TearDown()
        {
            _serialPortTransportClient.Dispose();
        }

        [Test]
        public void Connect_ConnectionFailed_CommunicationStatusIsBusy()
        {
            // Arrange.
            var serialPortProviderMock = new Mock<ISerialPortProvider>();
            serialPortProviderMock.Setup(x => x.Open(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<PInvokeParity>(), It.IsAny<int>(), It.IsAny<PInvokeStopBits>())).
                Returns(false);

            ((SerialPortTransportClient)_serialPortTransportClient).SerialPortProvider = serialPortProviderMock.Object;

            // Act.
            var status = _serialPortTransportClient.Connect();

            // Assert.
            Assert.AreEqual(CommunicationStatuses.Busy, status);
        }

        [Test]
        public void Connect_BeforeConnectionEstablished_SerialPortCloseCalled()
        {
            // Arrange.
            var serialPortProviderMock = new Mock<ISerialPortProvider>();
            serialPortProviderMock.SetupGet(x => x.IsOpen).Returns(true);
            ((SerialPortTransportClient)_serialPortTransportClient).SerialPortProvider = serialPortProviderMock.Object;

            // Act.
            _serialPortTransportClient.Connect();

            // Assert.
            serialPortProviderMock.Verify(x => x.Close(), Times.Once);
        }

        [Test]
        public void Connect_SerialPortConnectedSuccessfuly_CommunicationStatusIsDone()
        {
            // Arrange.
            var serialPortProviderMock = new Mock<ISerialPortProvider>();
            serialPortProviderMock.Setup(x => x.Open(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<PInvokeParity>(), It.IsAny<int>(), It.IsAny<PInvokeStopBits>())).
               Returns(true);
            ((SerialPortTransportClient)_serialPortTransportClient).SerialPortProvider = serialPortProviderMock.Object;

            // Act.
            var status = _serialPortTransportClient.Connect();

            // Assert.
            Assert.AreEqual(CommunicationStatuses.Done, status);
        }

        [Test]
        public void Disconnect_CommunicationStatusIsFailed_SerialPortCloseCalled()
        {
            // Arrange.
            var serialPortProviderMock = new Mock<ISerialPortProvider>();
            ((SerialPortTransportClient)_serialPortTransportClient).SerialPortProvider = serialPortProviderMock.Object;

            // Act.
            _serialPortTransportClient.Disconnect();

            // Assert.
            serialPortProviderMock.Verify(x => x.Close(), Times.Once);
        }

        [Test]
        public void Disconnect_CommunicationStatusIsDone_SerialPortCloseCalled()
        {
            // Arrange.
            var serialPortProviderMock = new Mock<ISerialPortProvider>();
            serialPortProviderMock.Setup(x => x.Open(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<PInvokeParity>(), It.IsAny<int>(), It.IsAny<PInvokeStopBits>())).
               Returns(true);

            serialPortProviderMock.Setup(provider => provider.Open(null, 0, PInvokeParity.None, 0, PInvokeStopBits.One)).
                Callback(() => serialPortProviderMock.SetupGet(x => x.IsOpen).Returns(true));

            serialPortProviderMock.Setup(x => x.Read(It.IsAny<byte[]>(), It.IsAny<int>())).
               Returns(-1);

            serialPortProviderMock.Setup(provider => provider.Close()).
                Callback(() => serialPortProviderMock.SetupGet(x => x.IsOpen).Returns(false));

            ((SerialPortTransportClient)_serialPortTransportClient).SerialPortProvider = serialPortProviderMock.Object;

            // Act.
            _serialPortTransportClient.Connect();
            _serialPortTransportClient.Disconnect();

            // Assert.
            serialPortProviderMock.Verify(x => x.Close(), Times.Exactly(2));
        }

        [Test]
        public void WriteData_CommunicationStatusDoesntMatter_SerialPortProviderWriteDataCalled()
        {
            // Arrange.
            var serialPortProviderMock = new Mock<ISerialPortProvider>();
            serialPortProviderMock.Setup(x => x.Write(It.IsAny<byte[]>(), It.IsAny<int>())).Returns(-1);
            ((SerialPortTransportClient)_serialPortTransportClient).SerialPortProvider = serialPortProviderMock.Object;

            // Act.
            var written = _serialPortTransportClient.WriteData(_data);

            // Assert.
            serialPortProviderMock.Verify(x => x.Write(It.IsAny<byte[]>(), It.IsAny<int>()), Times.Once);
            Assert.AreEqual(-1, written);
        }

        [Test]
        public void WriteData_CommunicationStatusDoesntMatter_CallDataTransmitedCallback()
        {
            // Arrange.
            var serialPortProviderMock = new Mock<ISerialPortProvider>();
            ((SerialPortTransportClient)_serialPortTransportClient).SerialPortProvider = serialPortProviderMock.Object;

            // Act.
            _serialPortTransportClient.WriteData(_data);

            // Assert.
            Assert.AreEqual(_sessionId, _transmitedDataChunk.SessionId);
            Assert.AreEqual(_apiType, _transmitedDataChunk.ApiType);
            Assert.AreEqual(_data.Length, _transmitedDataChunk.DataBufferLength);
            Assert.IsTrue(_data.SequenceEqual(_transmitedDataChunk.GetDataBuffer()));
        }

        [Test]
        public void ReadData_ReadFromSerialPortThreadStarted_ReadDataCalled()
        {
            // Arrange.
            var serialPortProviderMock = new Mock<ISerialPortProvider>();
            serialPortProviderMock.Setup(x => x.Open(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<PInvokeParity>(), It.IsAny<int>(), It.IsAny<PInvokeStopBits>())).
               Returns(true);

            serialPortProviderMock.Setup(x => x.Read(It.IsAny<byte[]>(), It.IsAny<int>())).
                Returns<byte[], int>((x, y) => _data.Length).
                Callback<byte[], int>((x, y) =>
                    {
                        Array.Copy(_data, x, _data.Length);
                    });

            ((SerialPortTransportClient)_serialPortTransportClient).SerialPortProvider = serialPortProviderMock.Object;
            _serialPortTransportClient.ReceiveDataCallback += OnDataReceived;

            // Act.
            var status = _serialPortTransportClient.Connect(); // Connected successfuly.
            Assert.AreEqual(CommunicationStatuses.Done, status);
            Thread.Sleep(100);
            _serialPortTransportClient.Disconnect();

            // Assert.
            Assert.AreEqual(_data.Length, _receivedDataChunk.DataBufferLength);
            Assert.AreEqual(_apiType, _receivedDataChunk.ApiType);
            Assert.AreEqual(_sessionId, _receivedDataChunk.SessionId);
            Assert.IsTrue(_data.SequenceEqual(_receivedDataChunk.GetDataBuffer()));
            _serialPortTransportClient.ReceiveDataCallback -= OnDataReceived;
        }

        [Test]
        public void ReadData_ReadFromSerialPortThreadStarted_DataTransmitedCallbackCalled()
        {
            // Arrange.
            var serialPortProviderMock = new Mock<ISerialPortProvider>();
            serialPortProviderMock.Setup(x => x.Open(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<PInvokeParity>(), It.IsAny<int>(), It.IsAny<PInvokeStopBits>())).
               Returns(true);

            serialPortProviderMock.Setup(x => x.Read(It.IsAny<byte[]>(), It.IsAny<int>()))
                .Callback<byte[], int>((x, y) =>
                {
                    Array.Copy(_data, x, _data.Length);
                })
                .Returns<byte[], int>((x, y) =>
                {
                    return _data.Length;
                });

            ((SerialPortTransportClient)_serialPortTransportClient).SerialPortProvider = serialPortProviderMock.Object;

            // Act.
            var status = _serialPortTransportClient.Connect(); // Connected successfuly.
            Assert.AreEqual(CommunicationStatuses.Done, status);
            Thread.Sleep(100);
            _serialPortTransportClient.Disconnect();

            // Assert.
            Assert.AreEqual(_data.Length, _transmitedDataChunk.DataBufferLength);
            Assert.AreEqual(_apiType, _transmitedDataChunk.ApiType);
            Assert.AreEqual(_sessionId, _transmitedDataChunk.SessionId);
            Assert.IsTrue(_data.SequenceEqual(_transmitedDataChunk.GetDataBuffer()));
        }

        private void OnDataReceived(DataChunk dataChunk, bool val)
        {
            _receivedDataChunk = dataChunk;
        }
    }
}
