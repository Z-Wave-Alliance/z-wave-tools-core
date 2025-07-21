/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.Enums;
using Utils;
using System;
using System.Threading;

namespace ZWave.Layers.Transport
{
    public class TcpClientTransportClient : TransportClientBase
    {
        public override event Action<ITransportClient> Connected;
        public override event Action<ITransportClient> Disconnected;


        private Thread _workerThread;
        private readonly object _lockObject = new object();
        private volatile bool _isStopped = true;
        private SocketDataSource _dataSource;
        private readonly Action<DataChunk> _transmitCallback;

        public override bool IsOpen
        {
            get { return _tcpConnection != null && _tcpConnection.Connected; }
        }

        private ITcpConnection _tcpConnection;
        public ITcpConnection TcpConnection
        {
            get
            { return _tcpConnection ?? (_tcpConnection = new TcpConnection()); }
            set
            {
                _tcpConnection = value;
            }
        }

        public TcpClientTransportClient(Action<DataChunk> transmitCallback)
        {
            _transmitCallback = transmitCallback;
        }

        protected override CommunicationStatuses InnerConnect(IDataSource dataSource)
        {
            InnerDisconnect();
            lock (_lockObject)
            {
                CommunicationStatuses ret = CommunicationStatuses.Failed;
                DataSource = dataSource;
                if (dataSource != null &&
                    dataSource is SocketDataSource &&
                    dataSource.Validate()
                    )
                {
                    _dataSource = (SocketDataSource)dataSource;
                    if (!TcpConnection.Connect(_dataSource.SourceName, _dataSource.Port))
                    {
                        "{0:X2} {1} {2} {3}"._DLOG(SessionId, ApiType, _dataSource, TcpConnection.Connected);
                    }
                    if (TcpConnection.Connected)
                    {
                        "{0:X2} {1} {2} {3}"._DLOG(SessionId, ApiType, _dataSource, TcpConnection.Connected);
                        ret = CommunicationStatuses.Done;
                        _isStopped = false;
                        Connected?.Invoke(this);
                        _workerThread = new Thread(WorkerThreadMethod);
                        _workerThread.Name = "Tcp Client";
                        _workerThread.IsBackground = true;
                        _workerThread.Start();
                    }

                }
                return ret;
            }
        }

        private void WorkerThreadMethod()
        {
            while (!_isStopped)
            {
                int numberOfBytesRead = TcpConnection.Read(out byte[] data);
                if (numberOfBytesRead > 0)
                {
                    if (!SuppressDebugOutput)
                    {
                        "{0:X2} {1} {2} << {3}"._DLOG(SessionId, ApiType, DataSource?.SourceName, Tools.GetHex(data));
                    }
                    var dataChunk = new DataChunk(data, SessionId, false, ApiType);
                    _transmitCallback?.Invoke(dataChunk);
                    ReceiveDataCallback?.Invoke(dataChunk, false);
                }
                else
                {
                    break;
                }
            }
        }

        protected override void InnerDisconnect()
        {
            lock (_lockObject)
            {
                if (_isStopped)
                {
                    return;
                }

                _isStopped = true;
                if (_tcpConnection != null)
                {
                    ((IDisposable)_tcpConnection).Dispose();
                    _tcpConnection = null;
                }

                if (_workerThread.ThreadState != ThreadState.Unstarted &&
                    Thread.CurrentThread.ManagedThreadId != _workerThread.ManagedThreadId
                    )
                {
                    _workerThread.Join();
                }
                Disconnected?.Invoke(this);
                "{0:X2} {1} {2}"._DLOG(SessionId, ApiType, _dataSource);
            }
        }

        protected override int InnerWriteData(byte[] data)
        {
            lock (_lockObject)
            {
                int ret = -1;
                if (_isStopped)
                {
                    return ret;
                }

                if (!SuppressDebugOutput)
                {
                    "{0:X2} {1} {2} >> {3}"._DLOG(SessionId, ApiType, DataSource?.SourceName, Tools.GetHex(data));
                }
                DataChunk dc = new DataChunk(data, SessionId, true, ApiType);
                _transmitCallback?.Invoke(dc);

                ret = TcpConnection.Write(data);
                if (ret != data.Length)
                {
                    "SEND FAILED"._DLOG();
                }
                return ret;
            }
        }

        protected override void InnerDispose()
        {
            InnerDisconnect();
        }

    }
}
