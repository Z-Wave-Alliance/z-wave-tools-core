/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using UicApplication;
using UicApplication.Data;
using UicApplication.Enums;
using UicApplication.Clusters;
using System;
using System.Text;
using Utils;
using System.Collections.Generic;

namespace ZWave.UicApplication
{
    class IncludeClassicOperation : UicApiOperation
    {
        private UicApiMessage message_Initiate;
        private UicApiHandler expectReceived_PinRequest;
        private UicApiHandler expectReceived_Status;

        private string Dsk { get; set; }
        internal int _timeout = 10000;
        internal string _unid;
        public IncludeClassicOperation(string unid, int timeoutMs) :
            base(false)
        {
            _timeout = timeoutMs;
            _unid = "ucl/by-unid/" + unid + "/";
        }
        protected override void CreateInstance()
        {
            ProtocolController.NetworkManagement InitMessage = new ProtocolController.NetworkManagement();
            InitMessage.payload.State = NetworkStates.AddNode;

            message_Initiate = new UicApiMessage(_unid, InitMessage.topic, InitMessage.payload);
            expectReceived_PinRequest = new UicApiHandler( _unid + ProtocolController.ID);            

            expectReceived_Status = new UicApiHandler("State");
        }
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, _timeout, message_Initiate));
            ActionUnits.Add(new DataReceivedUnit(expectReceived_PinRequest, OnPinRequestReceived));
            ActionUnits.Add(new DataReceivedUnit(expectReceived_Status, OnStatusReceived));
        }
        protected void OnPinRequestReceived(DataReceivedUnit ou)
        {
            string mqttReceivedTopic = (ou.DataFrame as DataFrame).MqttTopic;
            string mqttReceivedPayload = (ou.DataFrame as DataFrame).MqttPayload;

            if (mqttReceivedPayload.Contains("SecurityCode"))
            {
                SpecificResult.IsRequestingPing = true;
                base.SetStateCompleted(ou);
            }
        }

        protected void OnStatusReceived(DataReceivedUnit ou)
        {
            string mqttReceivedTopic = (ou.DataFrame as DataFrame).MqttTopic;
            string mqttReceivedPayload = (ou.DataFrame as DataFrame).MqttPayload;

            if (mqttReceivedPayload.Contains(NetworkStatesValues.Included)|| mqttReceivedPayload.Contains(NetworkStatesValues.OnlineFunctional))
            {
                State.Payload payload = mqttReceivedPayload;
                SpecificResult.Security = payload.Security;
                SpecificResult.UnId = mqttReceivedTopic.Split('/')[2];
                SpecificResult.IsRequestingPing = false;
                base.SetStateCompleted(ou);
            }
            
        }

        public override string AboutMe()
        {
            return string.Format("Included device DSK={0}", SpecificResult.mqttReceivedTopic);
        }

        public new InclusionResult SpecificResult
        {
            get { return (InclusionResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new InclusionResult();
        }
    }
}
