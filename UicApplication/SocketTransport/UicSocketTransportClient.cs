/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Utils;
using ZWave.Enums;
using ZWave.Layers;
using ZWave.UicApplication;
using ZWave.UicApplication.Layers;
using ZWave.UicApplication.SocketTransport;

namespace UicApplication
{
    /// <summary>
    /// Socket client for passing messages from Zats to Socket Server with running Mqtt client 
    /// </summary>
    public class UicSocketTransportClient : TransportClientBase, IUicTransportClient
    {

        private UicWebSocketClient _uicSocketClient;
        private readonly Action<DataChunk> _transmitCallback;
        private bool _disposed = false;
        private bool _isConnected = false;
        public override event Action<ITransportClient> Connected;
        public override event Action<ITransportClient> Disconnected;

        private BlockingCollection<MqttMessageWrapper> IncomingMessagesCollection = new BlockingCollection<MqttMessageWrapper>();
        public override bool IsOpen { get { return _uicSocketClient != null && _uicSocketClient.IsConnected; } }
        public UicSocketTransportClient(Action<DataChunk> transmitCallback)
        {
            _uicSocketClient = new UicWebSocketClient();
            _transmitCallback = transmitCallback;
        }

        protected override CommunicationStatuses InnerConnect(IDataSource dataSource)
        {
            try
            {
                bool isConnected = _uicSocketClient.Connect(dataSource.SourceName);
                Task.Run(() => ProcessDeserializedData());
                _uicSocketClient.DataReceivedHandler = (receivedData) =>
                    {
                        try
                        {
                            using (var stream = new MemoryStream(receivedData))
                            {
                                IEnumerable<MqttMessageWrapper> result = ReadJson<MqttMessageWrapper>(stream);

                                foreach (var item in result)
                                {
                                    IncomingMessagesCollection.Add(item);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message, ex);
                        }
                    };
                _isConnected = true;
                return CommunicationStatuses.Done;
            }
            catch(Exception e)
            {
                return CommunicationStatuses.Failed;
            }
        }

        private void ProcessDeserializedData()
        {
            while (_isConnected)
            {
                var wrappedData = IncomingMessagesCollection.Take();

                string topic = wrappedData.Topic;
                if (!string.IsNullOrWhiteSpace(topic))
                {
                    // Payload should be byte array
                    var payloadBytes = Encoding.ASCII.GetBytes(wrappedData.Payload);
                    "Received MQTT |||| topic: {0} |||| payload: {1}".
                        _DLOG(topic, wrappedData.Payload);
                    DataChunk dc = new DataChunk(topic,
                        payloadBytes, false);
                    dc.ApiType = ApiTypes.Uic;
                    _transmitCallback(dc);
                    ReceiveDataCallback?.Invoke(dc, false);
                }
                else
                {
                    "Received empty mqtt topic"._DLOG(null);
                }
            }
        }

        private IEnumerable<TResult> ReadJson<TResult>(Stream stream)
        {
            var serializer = new JsonSerializer();

            using (var reader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(reader))
            {
                jsonReader.SupportMultipleContent = true;

                while (jsonReader.Read())
                {
                    yield return serializer.Deserialize<TResult>(jsonReader);
                }
            }
        }

        protected override void InnerDisconnect()
        {
            if(!_isConnected)
            {
                return;
            }
            if(_uicSocketClient != null)
            {
                try
                {
                    _uicSocketClient.CloseClient();
                    _isConnected = false;
                }
                catch(Exception e)
                {

                }
            }
        }
        
        protected override void InnerDispose()
        {
            InnerDisconnect();
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            // Protect from being called multiple times.
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                //Close any events being handled at the moment
            }
            // Clean up all unmanaged resources.
            if (_uicSocketClient != null)
            {
                _uicSocketClient.Dispose();
            }
            _disposed = true;
        }

        /// <summary>
        /// Will not be using this method since we won't be taking in bytes to be sent but topics and payloads
        /// </summary>
        /// <param name="data">
        /// </param>
        /// <returns></returns>
        protected override int InnerWriteData(byte[] data)
        {
            throw new NotImplementedException();
        }

        public int SubscribeTopic(string topic)
        {
            if (!_isConnected)
            {
                InnerConnect(DataSource);
            }
            try
            {                
                "Subscribing MQTT |||| topic: {0} ".
                    _DLOG(topic);
                var wrappedData = new MqttMessageWrapper()
                {
                    Topic = topic
                };
                var jsonData = JsonConvert.SerializeObject(wrappedData);
                _uicSocketClient.Send(jsonData);
            }
            catch (Exception e)
            {
                e.ToString()._DLOG();
            }
            return 0;
        }        
        public int PublishMessage(string topic, string payload, bool isRetain)
        {
            if (!_isConnected)
            {
                InnerConnect(DataSource);
            }
           try
            {
                if (payload != null)
                {
                    "Publishing MQTT |||| topic: {0} |||| payload: {1}".
                                    _DLOG(topic, payload);
                }
                else
                {
                    "Publishing MQTT |||| topic: {0}".
                                    _DLOG(topic);
                }
                var wrappedData = new MqttMessageWrapper()
                {
                    Topic = topic,
                    Payload = payload,
                    IsPublish = true
                };
                var jsonData = JsonConvert.SerializeObject(wrappedData);
                _uicSocketClient.Send(jsonData);
            }
            catch(Exception e)
            {
                e.ToString()._DLOG();
            }
            return 0;
        }
    }
}
