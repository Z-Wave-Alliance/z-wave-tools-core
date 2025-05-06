/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Threading;
using Utils;
using ZWave.Enums;
using ZWave.Layers;

namespace ZWave.BasicApplication.EmulatedLink
{
    public class BasicLinkTransportClient : TransportClientBase
    {
        CancellationTokenSource _cts = new CancellationTokenSource();

        public override event Action<ITransportClient> Connected;
        public override event Action<ITransportClient> Disconnected;

        private Thread _workerThread;
        private readonly object _lockObject = new object();
        private bool _isCancelled = false;
        private readonly byte[] _buffer = new byte[512];
        private readonly byte[] _ack = { 0x06 };
        private AutoResetEvent _readDataSignal = new AutoResetEvent(false);
        private readonly Action<DataChunk> _transmitCallback;
        private readonly Func<ushort, byte[], int> _writeDataCallback;
        private readonly Func<ushort, byte[], CancellationToken, int> _readDataCallback;
        private readonly Action<ushort> _disposeClient;

        private bool _isOpen;
        public override bool IsOpen
        {
            get { return _isOpen; }
        }

        public BasicLinkTransportClient(Action<DataChunk> transmitCallback,
            Func<ushort, byte[], int> writeDataCallback,
            Func<ushort, byte[], CancellationToken, int> readDataCallback,
            Action<ushort> disposeClient)
        {
            _transmitCallback = transmitCallback;
            _writeDataCallback = writeDataCallback;
            _readDataCallback = readDataCallback;
            _disposeClient = disposeClient;
        }

        protected override CommunicationStatuses InnerConnect(IDataSource ds)
        {
            var ret = CommunicationStatuses.Busy;
            DataSource = ds;
            InnerDisconnect();
            _cts = new CancellationTokenSource();
            _readDataSignal.Reset();
            if (ds != null && ds is SerialPortDataSource)
            {
                var spds = (SerialPortDataSource)ds;
                {
                    ret = CommunicationStatuses.Done;
                    _isOpen = true;
                    Connected?.Invoke(this);
                    _isCancelled = false;
                    _workerThread = new Thread(DoWork);
                    _workerThread.Name = "Emulated Port Client";
                    _workerThread.IsBackground = true;
                    _workerThread.Start();
                }
                "{0:X2} {1} {2}@{3}.{4} {5}"._DLOG(SessionId, ApiType, spds.SourceName, spds.BaudRate, spds.StopBits, ret);
            }
            return ret;
        }

        protected override void InnerDisconnect()
        {
            _isOpen = false;
            lock (_lockObject)
            {
                _isCancelled = true;
            }
            if (_cts != null)
            {
                _cts.Cancel();
                if (_workerThread != null && Thread.CurrentThread.ManagedThreadId != _workerThread.ManagedThreadId)
                {
                    _workerThread.Join();
                }

                _cts.Dispose();
                _cts = null;
            }
            Disconnected?.Invoke(this);
            var spds = (SerialPortDataSource)DataSource;
            if (spds != null)
            {
                "{0:X2} {1} {2}@{3}.{4}"._DLOG(SessionId, ApiType, spds.SourceName, spds.BaudRate, spds.StopBits);
            }
        }

        protected override int InnerWriteData(byte[] data)
        {
            if (!SuppressDebugOutput)
            {
                "{0:X2} {1} {2} >> {3}"._DLOG(SessionId, ApiType, DataSource.SourceName, Tools.GetHex(data));
            }
            var dc = new DataChunk(data, SessionId, true, ApiType);
            _transmitCallback?.Invoke(dc);

            return _writeDataCallback(SessionId, data);
        }

        private void DoWork()
        {
            while (!_isCancelled)
            {
                try
                {
                    var readData = _readDataCallback(SessionId, _buffer, _cts.Token);
                    if (readData > 0)
                    {
                        var data = new byte[readData];
                        Array.Copy(_buffer, 0, data, 0, readData);
                        if (!SuppressDebugOutput)
                        {
                            "{0:X2} {1} {2} << {3}"._DLOG(SessionId, ApiType, DataSource.SourceName, Tools.GetHex(data));
                        }
                        DataChunk dc = new DataChunk(data, SessionId, false, ApiType);
                        _transmitCallback?.Invoke(dc);
                        ReceiveDataCallback?.Invoke(dc, false);
                    }
                }
                catch (OperationCanceledException)
                {
                }
            }
        }

        protected override void InnerDispose()
        {
            InnerDisconnect();
            _disposeClient(SessionId);
        }
    }
}
