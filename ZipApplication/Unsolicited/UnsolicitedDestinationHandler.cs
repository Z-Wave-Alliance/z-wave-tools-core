/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using ZWave.Layers;

namespace ZWave.ZipApplication
{
    public class UnsolicitedDestinationHandler<TClient> : IDisposable where TClient : IUnsolicitedReceiveClient
    {
        private readonly object _clientsAccess = new object();
        private ISocketListener _listener;
        private IUnsolicitedClientsFactory _unsolicitedClientsFactory;

        public Dictionary<string, TClient> _clients = new Dictionary<string, TClient>();

        public UnsolicitedDestinationHandler(ISocketListener listener, IUnsolicitedClientsFactory unsolicitedClientsFactory)
        {
            _listener = listener;
            _listener.DataReceived += Listener_DataReceived;
            _listener.ConnectionCreated += Listener_ConnectionCreated;
            _listener.ConnectionClosed += Listener_ConnectionClosed;
            _unsolicitedClientsFactory = unsolicitedClientsFactory;
        }

        private void Listener_ConnectionCreated(string address, ushort portNo)
        {
            var key = $"{address}:{portNo}";

            if (_clients.ContainsKey(key))
                throw new InvalidOperationException("Two unsolicited clients with the same address");

            TClient client = default(TClient);
            if (typeof(TClient) == typeof(IUnsolicitedClient))
            {
                var primaryClient = _unsolicitedClientsFactory.CreatePrimatyClient(address, portNo);
                
                if (primaryClient != null)
                {
                    primaryClient.WriteData = (x) =>
                    {
                        return _listener.ResponseTo(x, address, portNo);
                    };
                    client = (TClient)primaryClient;
                }
            }
            else
            {
                client = (TClient)_unsolicitedClientsFactory.CreateSecondaryClient(address, portNo);
            }

            if (client != null)
            {
                _clients.Add(key, client);
            }
        }

        private void Listener_DataReceived(ReceivedDataArgs receivedDataArgs)
        {
            lock (_clientsAccess)
            {
                var key = $"{receivedDataArgs.SourceName}:{receivedDataArgs.SourcePort}";
                if (_clients.ContainsKey(key))
                {
                    _clients[key].ReceiveData?.Invoke(receivedDataArgs.Data);
                }
            }
        }

        private void Listener_ConnectionClosed(string address, ushort portNo)
        {
            lock (_clientsAccess)
            {
                var key = $"{address}:{portNo}";
                if (_clients.ContainsKey(key))
                {
                    _clients[key].Dispose();
                    _clients.Remove(key);
                }
            }
        }

        public void Dispose()
        {
            lock (_clientsAccess)
            {
                if (_listener != null)
                {
                    _listener.DataReceived -= Listener_DataReceived;
                    _listener.ConnectionCreated -= Listener_ConnectionCreated;
                    _listener.ConnectionClosed -= Listener_ConnectionClosed;
                }
                foreach (var client in _clients)
                    client.Value.Dispose();
                _clients.Clear();
            }
        }
    }
}
