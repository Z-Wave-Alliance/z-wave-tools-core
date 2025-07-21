/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Threading;
using System.Threading.Tasks;
using Utils;
using ZWave.Enums;

namespace ZWave.Layers.Transport
{
    public class SerialPortTransportClient : TransportClientBase
    {
        public override event Action<ITransportClient> Connected;
        public override event Action<ITransportClient> Disconnected;

        public override bool IsOpen
        {
            get { return _serialPortProvider != null ? _serialPortProvider.IsOpen : false; }
        }

        private ISerialPortProvider _serialPortProvider;
        public ISerialPortProvider SerialPortProvider
        {
            get { return _serialPortProvider ?? (_serialPortProvider = new SerialPortProvider()); }
            set { _serialPortProvider = value; }
        }

        private CancellationTokenSource _readTaskCts;
        private Task _readTask;
        private readonly byte[] _buffer = new byte[Transport.SerialPortProvider.BUFFER_SIZE];
        private readonly Action<DataChunk> _transmitCallback;
        public SerialPortTransportClient(Action<DataChunk> transmitCallback)
        {
            _transmitCallback = transmitCallback;
        }

        protected override CommunicationStatuses InnerConnect(IDataSource ds)
        {
            var ret = CommunicationStatuses.Busy;
            DataSource = ds;
            Disconnect();
            if (ds != null && ds is SerialPortDataSource)
            {
                var spds = (SerialPortDataSource)ds;
                if (SerialPortProvider.Open(spds.SourceName, (int)spds.BaudRate, PInvokeParity.None, 8, spds.StopBits))
                {
                    ret = CommunicationStatuses.Done;
                    SerialPortProvider.ReadExisting();
                    Connected?.Invoke(this);
                    _readTaskCts = new CancellationTokenSource();
                    _readTask = Task.Factory.StartNew(() => ReadLoop());
                }
                "{0:X2} {1} {2}@{3}.{4} {5}"._DLOG(SessionId, ApiType, spds.SourceName, spds.BaudRate, spds.StopBits, ret);
            }
            Thread.Sleep(200); // Wait after connect (Zniffer may not respond with GetFrequencies).
            return ret;
        }

        protected override void InnerDisconnect()
        {
            if (_readTaskCts != null)
            {
                _readTaskCts.Cancel();
            }
            SerialPortProvider.Close();
            if (_readTask != null)
            {
                _readTask.Wait();
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
                "{0:X2} {1} {2} >> {3}"._DLOG(SessionId, ApiType, DataSource?.SourceName, Tools.GetHex(data));
            }
            var dc = new DataChunk(data, SessionId, true, ApiType);
            _transmitCallback?.Invoke(dc);
            int ret = SerialPortProvider.Write(data, data.Length);
            return ret;
        }

        private void ReadLoop()
        {
            while (!_readTaskCts.IsCancellationRequested)
            {
                var readData = SerialPortProvider.Read(_buffer, (int)Transport.SerialPortProvider.BUFFER_SIZE);
                if (readData > 0)
                {
                    var data = new byte[readData];
                    Array.Copy(_buffer, 0, data, 0, readData);
                    if (!SuppressDebugOutput)
                    {
                        "{0:X2} {1} {2} << {3}"._DLOG(SessionId, ApiType, SerialPortProvider.PortName, Tools.GetHex(data));
                    }
                    DataChunk dc = new DataChunk(data, SessionId, false, ApiType);
                    _transmitCallback?.Invoke(dc);
                    ReceiveDataCallback?.Invoke(dc, false);
                }
            }
        }

        protected override void InnerDispose()
        {
            InnerDisconnect();
        }

#if NETCOREAPP
        public static string[] GetPortNames(string vid = null, string pid = null)
        {
            return Transport.SerialPortProvider.GetPortNames(vid, pid);
        }
#endif
    }
}
