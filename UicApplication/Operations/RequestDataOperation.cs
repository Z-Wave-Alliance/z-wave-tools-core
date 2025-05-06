/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using UicApplication;
using UicApplication.Data;
using UicApplication.Enums;

namespace ZWave.UicApplication
{
    public class RequestDataOperation : UicApiOperation
    {
        internal string _unid;
        internal string _cluster;
        internal string _payload;
        internal string _expectedTopic;
        internal int _numberOfExpectedMessages = 1;
        internal int _timeout = 10000;

        private UicApiMessage message;
        private UicApiHandler expectReceived;

        public RequestDataOperation(string unid, string cluster, string payload, string expectedTopic) : base(false)
        {
            _unid = unid;
            _cluster = cluster;
            _payload = payload;
            _expectedTopic = expectedTopic;
        }

        public RequestDataOperation(string unid, string cluster, string expectedTopic) : base(false)
        {
            _unid = unid;
            _cluster = cluster;
            _expectedTopic = expectedTopic;
        }
        public RequestDataOperation(string unid, string cluster) : base(false)
        {
            _unid = unid;
            _cluster = cluster;           
        }

        public RequestDataOperation( string cluster, string payload, string expectedTopic, int timeoutMs) : base(false)
        {
            _payload = payload;
            _cluster = cluster;
            _expectedTopic = expectedTopic;
            _timeout = timeoutMs;
        }
        public RequestDataOperation(string cluster, string payload, string expectedTopic, int timeoutMs, 
            int numberOfExpectedMessages) : base(false)
        {
            _payload = payload;
            _cluster = cluster;
            _expectedTopic = expectedTopic;
            _timeout = timeoutMs;
            _numberOfExpectedMessages = numberOfExpectedMessages;
        }

        protected override void CreateInstance()
        {
            if (_payload == null)
            {
                message = new UicApiMessage(_unid, _cluster);
            }
            else
            {
                message = new UicApiMessage(_unid, _cluster, _payload);
            }
            expectReceived = new UicApiHandler(_expectedTopic);
        }

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, _timeout, message));
            ActionUnits.Add(new DataReceivedUnit(expectReceived, OnReceived));            
        }

        protected virtual void OnReceived(DataReceivedUnit ou)
        {
            if (_numberOfExpectedMessages == 1)
            {
                SpecificResult.mqttReceivedTopic = (ou.DataFrame as DataFrame).MqttTopic;
                SpecificResult.mqttReceivedPayload = (ou.DataFrame as DataFrame).MqttPayload;
                if (SpecificResult.mqttReceivedTopic != null && SpecificResult.mqttReceivedTopic.Length > 0)
                {

                }
                base.SetStateCompleted(ou);
            }
            if (_numberOfExpectedMessages > 1)
            {
                _numberOfExpectedMessages--;
            }           
        }

        public virtual RequestDataResult SpecificResult
        {
            get { return (RequestDataResult)Result; }
        }
    }

    public class RequestDataResult : ActionResult
    {
        public string mqttReceivedTopic { get; set; }
        public string mqttReceivedPayload { get; set; }
    }
}
