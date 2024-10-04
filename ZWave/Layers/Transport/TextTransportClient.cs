/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Text;
using ZWave.Enums;
using Utils;

namespace ZWave.Layers.Transport
{
    public class TextTransportClient : TransportClientBase
    {
        public override event Action<ITransportClient> Connected
        {
            add { throw new NotSupportedException(); }
            remove { }
        }
        public override event Action<ITransportClient> Disconnected
        {
            add { throw new NotSupportedException(); }
            remove { }
        }

        public Encoding TextEncoding { get; set; }
        private const int BUFFER_LENGTH = 1024 * 1024;

        private readonly Action<DataChunk> _transmitCallback;
        public TextTransportClient(Action<DataChunk> transmitCallback)
        {
            _transmitCallback = transmitCallback;
            TextEncoding = Encoding.ASCII;
        }

        private bool _isOpen;
        public override bool IsOpen
        {
            get { return _isOpen; }
        }

        protected override CommunicationStatuses InnerConnect(IDataSource ds)
        {
            CommunicationStatuses ret = CommunicationStatuses.Busy;
            DataSource = ds;
            _isOpen = true;
            ret = CommunicationStatuses.Done;
            "{0:X2} {1} {2}@{3} {4}"._DLOG(SessionId, ApiType, ds.SourceName, ds.Args, ret);
            return ret;
        }

        protected override void InnerDisconnect()
        {
            _isOpen = false;
            "{0:X2} {1}"._DLOG(SessionId, ApiType);
        }

        protected override int InnerWriteData(byte[] data)
        {
            if (!SuppressDebugOutput)
            {
                "{0:X2} {1} {2} >> {3}"._DLOG(SessionId, ApiType, DataSource != null ? DataSource.SourceName : string.Empty, TextEncoding.GetString(data));
            }
            DataChunk dc = new DataChunk(data, SessionId, true, ApiType);
            _transmitCallback?.Invoke(dc);
            return data.Length;
        }

        protected override void InnerDispose()
        {
            InnerDisconnect();
        }
    }
}
