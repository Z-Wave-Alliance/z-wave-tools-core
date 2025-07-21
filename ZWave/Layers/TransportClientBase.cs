/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using ZWave.Enums;
using Utils;

namespace ZWave.Layers
{
    public abstract class TransportClientBase : ITransportClient
    {
        public abstract event Action<ITransportClient> Connected;
        public abstract event Action<ITransportClient> Disconnected;

        public ushort SessionId { get; set; }
        public ApiTypes ApiType { get; set; }

        private IDataSource _dataSource;
        public IDataSource DataSource
        {
            get { return _dataSource; }
            set
            {
                $"{_dataSource?.SourceName} -+ {value?.SourceName}"._DLOG();
                _dataSource = value;
            }
        }
        public Action<DataChunk, bool> ReceiveDataCallback { get; set; }

        public bool SuppressDebugOutput { get; set; }
        public abstract bool IsOpen { get; }

        public TransportClientBase()
        {
        }

        // Create connection.
        public CommunicationStatuses Connect(IDataSource dataSource)
        {
            CommunicationStatuses ret = CommunicationStatuses.Failed;
            if (dataSource == null)
                throw new ArgumentNullException("dataSource");

            if (!dataSource.Validate())
                throw new ArgumentException("Not valid dataSource");

            if (DataSource == null || !DataSource.Equals(dataSource))
            {
                DataSource = dataSource;
            }

            ret = InnerConnect(dataSource);
            return ret;
        }

        public CommunicationStatuses Connect()
        {
            return Connect(DataSource);
        }

        protected abstract CommunicationStatuses InnerConnect(IDataSource dataSource);
        //

        // Disconnect.
        public void Disconnect()
        {
            //if (IsOpen)
            //{
            //    InnerDisconnect();
            //}
            try
            {
                InnerDisconnect();
            }
            catch (Exception e)
            {
                Console.WriteLine("Attempting to disconnect TransportClientBase: " + e.InnerException);
            }
        }
        protected abstract void InnerDisconnect();
        //

        // Write data.
        public int WriteData(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            if (data.Length == 0)
                throw new ArgumentException("Empty data");

            return InnerWriteData(data);
        }
        protected abstract int InnerWriteData(byte[] data);
        //

        #region IDisposable Members

        public void Dispose()
        {
            InnerDispose();
        }
        protected abstract void InnerDispose();

        #endregion
    }
}
