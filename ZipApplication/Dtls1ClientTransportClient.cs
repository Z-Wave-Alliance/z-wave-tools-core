/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using Utils;
using ZWave.Enums;
using ZWave.Layers;

namespace ZWave.ZipApplication
{
    public class Dtls1ClientTransportClient : TransportClientBase
    {
        private const int KEEP_ALIVE_RESPONSE_TIMEOUT = 10000;
        private const int BUFFER_LENGTH = 1024 * 1024;

        // This is the new format of KeepALive message, that has to be acknowledged with { 0x23, 0x03, 0x40 }.
        private byte[] _keepAliveRequest = new byte[] { 0x23, 0x03, 0x80 };
        private byte[] _keepAliveResponse = new byte[] { 0x23, 0x03, 0x40 };

        private bool _disposed = false;
        private volatile bool _isClosedByTimeout;
        private Thread _connectionThread;
        private Thread _keepAliveThread;
        private AutoResetEvent _keepAliveSignal = new AutoResetEvent(false);
        private readonly object _lockObject = new object();
        private volatile bool _isStoped = true;
        private SocketDataSource _dataSource;
        private string _psk;
        private IPEndPoint _remoteEndPoint;
        private readonly Action<DataChunk> _transmitCallback;

        public override event Action<ITransportClient> Connected;
        public override event Action<ITransportClient> Disconnected;
        public Func<byte[], int> DataWriteSubstitute;
        public int ConnectionAttempts { get; set; }

        private int _keepAliveRequestTimeout = 30000;
        public int KeepAliveTimeout
        {
            get { return _keepAliveRequestTimeout; }
            set
            {
                _keepAliveRequestTimeout = value;
                _keepAliveSignal.Set();
            }
        }

        private IDtlsClient _dtlsClient;
        public IDtlsClient DtlsClient
        {
            get { return _dtlsClient ?? (_dtlsClient = new DtlsClient()); }
            set
            {
                if (_dtlsClient != null)
                {
                    _dtlsClient.Close();
                }
                _dtlsClient = value;
            }
        }

        public bool CanReconnect { get; private set; }

        public override bool IsOpen
        {
            get { return _dtlsClient != null && _dtlsClient.IsConnected; }
        }

        public Dtls1ClientTransportClient(Action<DataChunk> transmitCallback)
        {
            _transmitCallback = transmitCallback;
            ConnectionAttempts = 2;
            CanReconnect = true;
        }

        protected override CommunicationStatuses InnerConnect(IDataSource ds)
        {
            InnerDisconnect();
            DataSource = ds;
            _isClosedByTimeout = false;
            _dataSource = ds as SocketDataSource;
            var cmdLineParser = new CommandLineParser(new[] { _dataSource.Args });
            if (cmdLineParser.HasArgument("psk"))
            {
                _psk = cmdLineParser.GetArgumentString("psk");
            }
            else
            {
                _psk = _dataSource.Args;
            }
            if (IPAddress.TryParse(_dataSource.SourceName, out IPAddress ipAddress))
            {
                _remoteEndPoint = new IPEndPoint(ipAddress, _dataSource.Port);
                return CreateConnection();
            }
            return CommunicationStatuses.Failed;
        }

        private CommunicationStatuses CreateConnection()
        {
            var ret = CommunicationStatuses.Busy;
            lock (_lockObject)
            {
                try
                {
                    int attempts = ConnectionAttempts;
                    DtlsClient.Connected += DtlsClient_Connected;
                    DtlsClient.Closed += DtlsClient_Closed;
                    while (!DtlsClient.IsConnected && attempts > 0)
                    {
                        attempts--;
                        Thread.Sleep(300);
                        DtlsClient.Connect(_psk, _dataSource.SourceName, (ushort)_dataSource.Port);
                        "{0:X2} {1} {2}@{3} {4}"._DLOG(SessionId, ApiType, _dataSource.SourceName, _dataSource.Port, DtlsClient.IsConnected);
                    }
                    if (DtlsClient.IsConnected)
                    {
                        attempts--;
                        _isStoped = false;
                        ret = CommunicationStatuses.Done;

                        _connectionThread = new Thread(DoWork);
                        _connectionThread.Name = "Udp Client (Receive)";
                        _connectionThread.IsBackground = true;
                        _connectionThread.Start();
                        _keepAliveThread = new Thread(DoKeepAliveWork);
                        _keepAliveThread.Name = "Udp Client (Keep Alive)";
                        _keepAliveThread.IsBackground = true;
                        _keepAliveThread.Start();
                    }
                    else
                    {
                        attempts--;
                        DtlsClient.Connected -= DtlsClient_Connected;
                        DtlsClient.Closed -= DtlsClient_Closed;
                    }
                }
                catch (IOException ex)
                {
                    ex.Message._DLOG();
                }
            }
            "{0:X2} {1} {2}@{3} {4}"._DLOG(SessionId, ApiType, _dataSource.SourceName, _dataSource.Port, DtlsClient.IsConnected);
            return ret;
        }

        void DtlsClient_Closed(IDtlsClient dtlsClient)
        {
            InnerDisconnect();
        }

        void DtlsClient_Connected(IDtlsClient dtlsClient)
        {
            Connected?.Invoke(this);
        }

        protected override void InnerDisconnect()
        {
            lock (_lockObject)
            {
                if (_isStoped)
                {
                    return;
                }
                _isStoped = true;
                if (DtlsClient != null)
                {
                    DtlsClient.Connected -= DtlsClient_Connected;
                    DtlsClient.Closed -= DtlsClient_Closed;
                    DtlsClient.Close();
                }
                if (!_keepAliveSignal.SafeWaitHandle.IsClosed)
                {
                    _keepAliveSignal.Set();
                }
                if (_connectionThread.ThreadState != ThreadState.Unstarted && Thread.CurrentThread.ManagedThreadId != _connectionThread.ManagedThreadId)
                {
                    _connectionThread.Join();
                }
                if (_keepAliveThread.ThreadState != ThreadState.Unstarted && Thread.CurrentThread.ManagedThreadId != _keepAliveThread.ManagedThreadId)
                {
                    _keepAliveThread.Join();
                }
                "{0:X2} {1} {2}@{3} isTimeout:{4}"._DLOG(SessionId, ApiType, _dataSource.SourceName, _dataSource.Port, _isClosedByTimeout);
                Disconnected?.Invoke(this);
            }
        }

        protected override int InnerWriteData(byte[] data)
        {
            if (DataWriteSubstitute != null)
            {
                return DataWriteSubstitute(data);
            }
            else
            {
                lock (_lockObject)
                {
                    int ret = -1;
                    if (!DtlsClient.IsConnected && CanReconnect)
                    {
                        CreateConnection();
                    }

                    if (_isStoped)
                    {
                        return -1;
                    }

                    if (DtlsClient.IsConnected)
                    {
                        try
                        {
                            if (!SuppressDebugOutput)
                            {
                                "{0} >> {1}"._DLOG(DataSource.SourceName, Tools.GetHex(data));
                            }
                            var dc = new DataChunk(data, SessionId, true, ApiType);
                            _transmitCallback(dc);
                            ret = DtlsClient.Send(data);
                        }
                        catch (SocketException)
                        {
                            ret = -1;
                        }
                        catch (ObjectDisposedException)
                        {
                            // Socket was closed.
                        }
                    }
                    if (ret != data.Length)
                    {
                        "SEND FAILED"._DLOG();
                    }
                    return ret;
                }
            }
        }

        readonly byte[] buffer = new byte[4096];
        private void DoWork()
        {
            while (!_isStoped)
            {
                if (DtlsClient.IsConnected)
                {
                    try
                    {
                        int received = DtlsClient.Receive(buffer);
                        if (received > 0)
                        {
                            _keepAliveSignal.Set();

                            byte[] data = new byte[received];
                            Array.Copy(buffer, data, received);
                            if (IsKeepAliveResponse(data))
                            {
                                if (!SuppressDebugOutput)
                                {
                                    "Keep alive response: {0} << {1}"._DLOG(DataSource.SourceName, Tools.GetHex(data));
                                }
                            }
                            else
                            {
                                if (!SuppressDebugOutput)
                                {
                                    "{0} << {1}"._DLOG(DataSource.SourceName, Tools.GetHex(data));
                                }
                                var dc = new DataChunk(data, SessionId, false, ApiType);
                                _transmitCallback(dc);
                                ReceiveDataCallback?.Invoke(dc, false);
                            }
                        }
                    }
                    catch (SocketException)
                    {
                    }
                    catch (ObjectDisposedException)
                    {
                        // Socket was closed.
                    }
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
        }

        private void DoKeepAliveWork()
        {
            while (!_isStoped && !_keepAliveSignal.SafeWaitHandle.IsClosed)
            {
                if (_keepAliveSignal.WaitOne(_keepAliveRequestTimeout))
                {
                    continue;
                }
                if (DtlsClient.IsConnected)
                {
                    try
                    {
                        int ret = DtlsClient.Send(_keepAliveRequest);
                        if (!SuppressDebugOutput)
                        {
                            if (ret == _keepAliveRequest.Length)
                            {
                                "Keep alive request: {0} >> {1}"._DLOG(DataSource.SourceName, Tools.GetHex(_keepAliveRequest));
                            }
                            else
                            {
                                "Failed send alive request: {0} >> {1}"._DLOG(DataSource.SourceName, Tools.GetHex(_keepAliveRequest));
                            }
                        }
                        if (!_keepAliveSignal.WaitOne(KEEP_ALIVE_RESPONSE_TIMEOUT))
                        {
                            _isClosedByTimeout = true;
                            InnerDisconnect();
                        }
                    }
                    catch (SocketException)
                    {
                    }
                    catch (ObjectDisposedException)
                    {
                        // Socket was closed.
                    }
                }
                else
                {
                    Thread.Sleep(300);
                }
            }
        }

        private bool IsKeepAliveResponse(byte[] response)
        {
            return _keepAliveResponse.SequenceEqual(response);
        }

        protected override void InnerDispose()
        {
            InnerDisconnect();
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Protect from being called multiple times.
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                if (_keepAliveSignal != null)
                {
                    _keepAliveSignal.Close();
                }
            }

            // Clean up all unmanaged resources.
            if (_dtlsClient != null)
            {
                _dtlsClient.Close();
            }

            _disposed = true;
        }

        ~Dtls1ClientTransportClient()
        {
            Dispose(false);
        }
    }
}
