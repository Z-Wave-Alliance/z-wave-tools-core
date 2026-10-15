// SPDX-License-Identifier: BSD-3-Clause
// SPDX-FileCopyrightText: Z-Wave Alliance <https://z-wavealliance.org>
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using NUnit.Framework;
using ZWave.ZipApplication;

namespace ZWaveTests
{
    /// <summary>
    /// Exercises DtlsClient and DtlsListener against the native ziptrans library over the
    /// loopback interface. The subject is the managed side of the interop: the lifetime of the
    /// native handle and of the callback delegates. A mistake there does not fail an assertion,
    /// it takes the whole test host down, which is what these tests guard against.
    /// </summary>
    [TestFixture]
    public class DtlsInteropTests
    {
        private const string Psk = "123456789012345678901234567890AA";
        private const string Address = "127.0.0.1";
        private const int Timeout = 10000;
        // Each round waits for the native close notification: milliseconds on loopback,
        // but on a CI runner occasionally about a second, so the count is kept low.
        private const int GarbageCollectionRounds = 5;

        private ushort _port;
        private DtlsListener _listener;
        private DtlsClient _client;

        [SetUp]
        public void SetUp()
        {
            _port = GetFreeUdpPort();
            _listener = new DtlsListener();
            _client = new DtlsClient();
        }

        [TearDown]
        public void TearDown()
        {
            _client.Close();
            _listener.Stop();
        }

        // A fixed port would collide when the net10.0 and net48 test hosts of this
        // assembly run at the same time, which is what "dotnet test" on the solution does.
        private static ushort GetFreeUdpPort()
        {
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                socket.Bind(new IPEndPoint(IPAddress.Loopback, 0));
                return (ushort)((IPEndPoint)socket.LocalEndPoint).Port;
            }
        }

        private void StartListener()
        {
            Assert.AreEqual(0, _listener.Start(Psk, Address, _port), "The listener could not be started.");
        }

        private void Connect()
        {
            Assert.IsTrue(_client.Connect(Psk, Address, _port), "The client could not connect.");
        }

        // Raised on the native thread, possibly after the test has disposed the event; an
        // exception there cannot be caught by the test and would take the process down.
        private static void SetEventSafely(ManualResetEvent waitEvent)
        {
            try
            {
                waitEvent.Set();
            }
            catch (ObjectDisposedException)
            {
            }
        }

        [Test]
        public void Close_CalledTwice_SecondCallIsIgnored()
        {
            StartListener();
            Connect();
            _client.Close();
            Assert.DoesNotThrow(() => _client.Close());
            Assert.IsFalse(_client.IsConnected);
        }

        [Test]
        public void SendAndReceive_AfterClose_ReturnMinusOne()
        {
            StartListener();
            Connect();
            _client.Close();
            Assert.AreEqual(-1, _client.Send(new byte[] { 0x01, 0x02, 0x03 }));
            Assert.AreEqual(-1, _client.Receive(new byte[8]));
        }

        [Test]
        public void Connect_AfterClose_SameInstanceConnectsAndRaisesConnectedAgain()
        {
            StartListener();
            int connectedCount = 0;
            _client.Connected += client => Interlocked.Increment(ref connectedCount);
            Connect();
            _client.Close();
            Connect();
            Assert.IsTrue(_client.IsConnected);
            Assert.AreEqual(2, connectedCount);
        }

        [Test]
        public void Start_AfterStop_SameInstanceListensAndRaisesClientConnectedAgain()
        {
            using (var connected = new ManualResetEvent(false))
            {
                _listener.ClientConnected += (address, port) => SetEventSafely(connected);
                for (int round = 1; round <= 2; round++)
                {
                    connected.Reset();
                    StartListener();
                    Connect();
                    Assert.IsTrue(connected.WaitOne(Timeout), $"ClientConnected was not raised in round {round}.");
                    _client.Close();
                    _listener.Stop();
                }
            }
        }

        // Regression test for a crash of the test host. The delegates handed to the native library
        // used to be dropped in Stop() and inside the Closed callback, so a garbage collection
        // between the managed close and the last native callback freed the function pointers the
        // library was about to call. Forcing full collections in that window turns a rare fault into
        // a likely one. The process dying here, rather than an assertion failing, is the regression.
        [Test]
        public void ConnectAndClose_RepeatedlyUnderForcedGarbageCollection_DoesNotCrashTheProcess()
        {
            using (var closed = new ManualResetEvent(false))
            {
                _listener.ClientClosed += (address, port) => SetEventSafely(closed);
                for (int round = 1; round <= GarbageCollectionRounds; round++)
                {
                    closed.Reset();
                    StartListener();
                    Connect();
                    _client.Close();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    Assert.IsTrue(closed.WaitOne(Timeout), $"ClientClosed was not raised in round {round}.");
                    _listener.Stop();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
        }
    }
}
