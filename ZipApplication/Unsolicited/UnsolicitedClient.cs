/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using Utils;
using ZWave.ZipApplication.Devices;
using ZWave.Layers.Transport;

namespace ZWave.ZipApplication
{
    public class UnsolicitedClient: IUnsolicitedClient
    {
        private List<ActionToken> _supportedOperatoins = new List<ActionToken>();
        private ZipDevice _zipDevice;
        public ZipDevice ZipDevice => _zipDevice;
        public Action<byte[]> ReceiveData { get; set; }
        public Func<byte[], int> WriteData
        {
            get
            {
                if (_zipDevice.TransportClient is Dtls1ClientTransportClient)
                {
                    return ((Dtls1ClientTransportClient)_zipDevice.TransportClient).DataWriteSubstitute;
                }
                else
                {
                    return ((UdpClientTransportClient)_zipDevice.TransportClient).DataWriteSubstitute;
                }
            }
            set
            {
                if (_zipDevice.TransportClient is Dtls1ClientTransportClient)
                {
                    ((Dtls1ClientTransportClient)_zipDevice.TransportClient).DataWriteSubstitute = value;
                }
                else
                {
                    ((UdpClientTransportClient)_zipDevice.TransportClient).DataWriteSubstitute = value;
                }
            }
        }

        

        public UnsolicitedClient(ZipDevice zipDevice)
        {
            _zipDevice = zipDevice;
            var transportClient = _zipDevice.TransportClient;
            ReceiveData = (data) =>
            {
                $"UNSOL CLIENT SESSION ID: {_zipDevice.SessionId}"._DLOG();
                transportClient.ReceiveDataCallback?.Invoke(new Layers.DataChunk(data, _zipDevice.SessionId, false, transportClient.ApiType), false);
            };
        }

        public void AddSupportedOperation(IActionItem action)
        {
            _supportedOperatoins.Add(_zipDevice.SessionClient.ExecuteAsync(action));
        }

        public void Dispose()
        {
            if (_zipDevice != null)
            {
                foreach (var operation in _supportedOperatoins)
                {
                    if (operation != null)
                        _zipDevice.Cancel(operation);
                }
                _supportedOperatoins.Clear();
                _zipDevice.Dispose();
                _zipDevice = null;
            }
        }
    }
}
