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
    class InputPinOperation : UicApiOperation
    {
        private UicApiMessage message_InputPin;
        private UicApiHandler expectReceived_Status;
        private string Dsk { get; set; }
        internal int _timeout = 10000;
        internal string _unid;

        public InputPinOperation(string unid, string dsk, int timeoutMs) :
            base(false)
        {
            Dsk = dsk;
            _timeout = timeoutMs;
            _unid = "ucl/by-unid/" + unid + "/";
        }
        protected override void CreateInstance()
        {
            ProtocolController.NetworkManagement InputPin = new ProtocolController.NetworkManagement();
            InputPin.payload.State = NetworkStates.AddNode;
            InputPin.payload.StateParameters = new ProtocolController.StateParameters
            {
                SecurityCode = Dsk,
                UserAccept = true,
            };

            message_InputPin = new UicApiMessage(_unid, InputPin.topic, InputPin.payload);
            expectReceived_Status = new UicApiHandler("State");
        }

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, _timeout, message_InputPin));
            ActionUnits.Add(new DataReceivedUnit(expectReceived_Status, OnStatusReceived));
        }

        protected void OnStatusReceived(DataReceivedUnit ou)
        {
            string mqttReceivedTopic = (ou.DataFrame as DataFrame).MqttTopic;
            string mqttReceivedPayload = (ou.DataFrame as DataFrame).MqttPayload;
            State.Payload statePayload = mqttReceivedPayload;

            if (statePayload.NetworkStatus == NetworkStatesValues.Included)
            {
                if (mqttReceivedPayload.Contains(NetworkStatesValues.Included))
                {
                    SpecificResult.Security = statePayload.Security;
                    SpecificResult.UnId = mqttReceivedTopic.Split('/')[2];
                    base.SetStateCompleted(ou);
                }
            }
            else if (statePayload.NetworkStatus == NetworkStatesValues.OnlineFunctional)
            {
                if (mqttReceivedPayload.Contains(NetworkStatesValues.OnlineFunctional))
                {
                    SpecificResult.Security = statePayload.Security;
                    SpecificResult.UnId = mqttReceivedTopic.Split('/')[2];
                    base.SetStateCompleted(ou);
                }
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
