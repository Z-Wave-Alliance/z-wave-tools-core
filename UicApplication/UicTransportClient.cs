/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Utils;
using ZWave.Enums;
using ZWave.Layers;
using System.Security;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System.Text;
using ZWave.UicApplication.Layers;
using System.Security.Cryptography.X509Certificates;
using System.IO;

namespace UicApplication
{
    /// <summary>
    /// MqttClient 
    /// </summary>
    /// <param name="tcpServerName">MQTT broker ID</param>
    /// <param name="credentials"> Name and password using a dictionary. Password variable tipe is SecureString</param>
    public class UicTransportClient : TransportClientBase, IUicTransportClient
    {
        private IMqttClient _mqttClient;
        private MqttFactory mqttFactory = null;
        private volatile bool _isStopped = true;
        private readonly object _lockObject = new object();
        private readonly Action<DataChunk> _transmitCallback;
        private bool _disposed = false;
        private bool isConnected = false;
        public override event Action<ITransportClient> Connected;
        public override event Action<ITransportClient> Disconnected;

        public IMqttClient mqttClient
        {
            get { return _mqttClient ?? (_mqttClient = mqttFactory.CreateMqttClient()); }
            set
            {
                if (_mqttClient != null)
                {
                    _mqttClient.Dispose();
                }
                _mqttClient = value;
            }
        }
        public string TcpServerName { get; set; }
        public Dictionary<string, SecureString> Credentials { get; set; }
        public override bool IsOpen { get { return _mqttClient != null && _mqttClient.IsConnected; } }
        public UicTransportClient(Action<DataChunk> transmitCallback)
        {
            _transmitCallback = transmitCallback;
        }
        public void TestConnect(string TcpServer)
        {
            mqttFactory = new MqttFactory();
            _mqttClient = mqttFactory.CreateMqttClient();
            MqttClientCredentials mqttClientCredentials = new MqttClientCredentials();
            SecureString secureString = new SecureString();
            IMqttClientOptions options = new MqttClientOptionsBuilder()
                .WithTcpServer(TcpServer, 1883)
                .WithCleanSession()
                .Build();
            try
            {
                Task<MQTTnet.Client.Connecting.MqttClientConnectResult> Connecting =
                    Connect(_mqttClient, options);
                var restult = Connecting.Result;
                _isStopped = false;
                //Set the required action when receiving a Message in our client
                _mqttClient.UseApplicationMessageReceivedHandler(e =>
                {
                    "Received MQTT topic: {0} and payload: {1}".
                    _DLOG(e.ApplicationMessage.Topic, e.ApplicationMessage.Payload);
                }
                );
            }
            catch (Exception e)
            {
            }
        }
        protected override CommunicationStatuses InnerConnect(IDataSource dataSource)
        {
            mqttFactory = new MqttFactory();
            _mqttClient = mqttFactory.CreateMqttClient();
            MqttClientCredentials mqttClientCredentials = new MqttClientCredentials();

            bool useTls = false;
            var mqttPort = useTls ? 8883 : 1883;

            var optionsBuilder = new MqttClientOptionsBuilder()
                .WithTcpServer(dataSource.SourceName.Trim(), mqttPort)
                .WithCleanSession()
                .WithKeepAlivePeriod(TimeSpan.FromMinutes(5));


            if (useTls)
            {
                var caFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"tls-cert/ca.crt");
                var caPfxFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"tls-cert/client.pfx");
                var exportPassphrase = "1234";

                var caCert = X509Certificate.CreateFromCertFile(caFilePath);
                var clientCert = new X509Certificate2(caPfxFilePath, exportPassphrase);

                optionsBuilder = optionsBuilder.WithTls(new MqttClientOptionsBuilderTlsParameters
                {
                    UseTls = true,
                    SslProtocol = System.Security.Authentication.SslProtocols.Tls12,
                    CertificateValidationHandler = (mqttCallback) =>
                    {

                    /// Windows trusted certificates do not approve Self-signed. 
                    /// That's why we add this always true callback.
                    return true;
                    },
                    Certificates = new List<X509Certificate>()
                    {
                        caCert, clientCert
                    }
                });
            }

            IMqttClientOptions options = optionsBuilder.Build();


            try
            {

                Task<MQTTnet.Client.Connecting.MqttClientConnectResult> Connecting =
                    Connect(_mqttClient, options);
                var restult = Connecting.Result;
                _isStopped = false;
                //Set the required action when receiving a Message in our client
                _mqttClient.UseApplicationMessageReceivedHandler(e =>
                    {
                        try
                        {
                            string topic = e.ApplicationMessage.Topic;
                            if (string.IsNullOrWhiteSpace(topic) == false)
                            {
                                if (e.ApplicationMessage.Payload != null)
                                {
                                    string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                                    string payload_fix = payload.Replace("- payload: ", "");
                                    //Console.WriteLine($"Topic: {topic}. Message Received: {payload}");
                                    "Received MQTT |||| topic: {0} |||| payload: {1}".
                                        _DLOG(topic, payload_fix);
                                }
                                else
                                {
                                    // Allow empty payload
                                    e.ApplicationMessage.Payload = new byte[0];
                                }

                                DataChunk dc = new DataChunk(e.ApplicationMessage.Topic,
                                    e.ApplicationMessage.Payload, e.ApplicationMessage.Retain);
                                dc.ApiType = ApiTypes.Uic;
                                _transmitCallback(dc);
                                ReceiveDataCallback?.Invoke(dc, false);
                            }
                            else
                            {
                                "Received empty mqtt topic"._DLOG(null);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message, ex);
                        }
                    }
                );
                isConnected = true;
                return CommunicationStatuses.Done;
            }
            catch (Exception e)
            {
                return CommunicationStatuses.Failed;
            }
        }
        protected override void InnerDisconnect()
        {
            if (_isStopped)
            {
                return;
            }
            if (_mqttClient != null)
            {
                try
                {
                    MqttClient_Closed(_mqttClient);
                    _isStopped = true;
                }
                catch (Exception e)
                {

                }
            }
        }
        void MqttClient_Closed(IMqttClient mqttClient)
        {
            InnerDisconnect();
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
            if (_mqttClient != null)
            {
                _mqttClient.Dispose();
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
            if (!isConnected)
            {
                InnerConnect(DataSource);
            }
            try
            {
                "Subscribing MQTT |||| topic: {0} ".
                    _DLOG(topic);
                Task<MQTTnet.Client.Subscribing.MqttClientSubscribeResult> Subscribing =
                    SubscribeTopicAsynch(_mqttClient, topic);
                try
                {
                    Subscribing.Wait();
                }
                catch (Exception e)
                {
                    "Subscribing threw an exception: {0}".
                                    _DLOG(e.Message);
                    "Subscribing threw an exception stack: {0}".
                                        _DLOG(e.StackTrace);
                }
            }
            catch (Exception e)
            {
                e.ToString()._DLOG();
            }
            return 0;
        }
        public int PublishMessage(string topic, string payload, bool retain=false)
        {
            if (!isConnected)
            {
                InnerConnect(DataSource);
            }
            MqttApplicationMessage message = null;
            if (payload != null)
            {
                message = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(payload)
                    .WithRetainFlag(retain)
                    .Build();
            }
            else
            {
                message = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithRetainFlag(retain)
                    .Build();
            }
            try
            {
                if (payload != null)
                {
                    "Publishing MQTT |||| topic: {0} |||| payload: {1}".
                                    _DLOG(message.Topic, payload);
                }
                else
                {
                    "Publishing MQTT |||| topic: {0}".
                                    _DLOG(message.Topic);
                }
                Task Publishing = PublishMessageAsynch(_mqttClient, message);
            }
            catch (Exception e)
            {
                e.ToString()._DLOG();
            }
            return 0;
        }

        #region Task-await methods

        private static async Task<MQTTnet.Client.Connecting.MqttClientConnectResult> Connect
            (IMqttClient _mqttClient, IMqttClientOptions options)
        {
            return await _mqttClient.ConnectAsync(options, CancellationToken.None);
        }

        private static async Task Disconnect
            (IMqttClient _mqttClient, IMqttClientOptions options)
        {
            await _mqttClient.DisconnectAsync();
        }

        private static async Task<MQTTnet.Client.Subscribing.MqttClientSubscribeResult> SubscribeTopicAsynch
            (IMqttClient _mqttClient, string topic)
        {
            return await _mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic(topic).Build());
        }

        private static async Task PublishMessageAsynch
            (IMqttClient _mqttClient, MqttApplicationMessage message)
        {
            try
            {
                await _mqttClient.PublishAsync(message);
            }
            catch (Exception e)
            {
                "Publishing threw an exception: {0}".
                                    _DLOG(e.Message);
                "Publishing threw an exception stack: {0}".
                                    _DLOG(e.StackTrace);
            }
        }

        #endregion 

    }
}
