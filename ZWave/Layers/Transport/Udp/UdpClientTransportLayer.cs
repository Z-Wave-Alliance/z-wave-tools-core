/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Collections.Generic;
using System;
using System.Collections;
using System.Net;
using Utils;
using Utils.Events;

namespace ZWave.Layers.Transport
{
    public class UdpClientTransportLayer : ISocketTransportLayer
    {
        public event EventHandler<EventArgs<DataChunk>> DataTransmitted;
        public bool SuppressDebugOutput { get; set; }

        private HashSet<ITransportClient> _listeningClientsSet = new HashSet<ITransportClient>();

        private readonly object _lisneterLock = new object();

        private UdpClientTransportListener _listener;
        public ISocketListener Listener
        {
            get { return _listener; }
            set
            {
                lock (_lisneterLock)
                {
                    if (_listener != null)
                    {
                        _listener.Stop();
                        _listener.DataReceived -= Listener_DataReceived;
                    }
                    _listener = value as UdpClientTransportListener;
                    _listener.DataReceived += Listener_DataReceived;
                }
            }
        }

        private void Listener_DataReceived(ReceivedDataArgs receivedDataArgs)
        {
            if (_listeningClientsSet.Count == 0)
            {
                return;
            }
            if (IPAddress.TryParse(receivedDataArgs.SourceName, out IPAddress sourceIpAddress))
            {
                sourceIpAddress = Tools.MapToIPv4(sourceIpAddress);
            }
            foreach (var client in _listeningClientsSet)
            {
                client.ReceiveDataCallback?.Invoke(new DataChunk(receivedDataArgs.Data, client.SessionId, false, client.ApiType), false);
            }
        }

        public ITransportClient CreateClient(ushort sessionId)
        {
            UdpClientTransportClient ret = new UdpClientTransportClient(dataChunk => DataTransmitted?.Invoke(this, new EventArgs<DataChunk>(dataChunk)))
            {
                SuppressDebugOutput = SuppressDebugOutput,
                SessionId = sessionId
            };
            return ret;
        }

        public void RegisterListeningClient(ITransportClient transportClient)
        {
            if (_listeningClientsSet.Contains(transportClient))
            {
                return;
            }
            _listeningClientsSet.Add(transportClient);
        }

        private Hashtable _clients = new Hashtable();

        public void RegisterClient(string key, ITransportClient client)
        {
            if (!_clients.Contains(key))
            {
                _clients.Add(key, client);
            }
            else
            {
                _clients[key] = client;
            }
        }

        public void UnregisterClient(string key)
        {
            if (_clients.Contains(key))
            {
                _clients.Remove(key);
            }
        }

        //public bool UnregisterListeningClient(ITransportClient transportClient)
        //{
            //return _listeningClientsSet.Remove(transportClient);
        //}
    }
}
