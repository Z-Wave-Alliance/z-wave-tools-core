/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using UicApplication;
using UicApplication.Data;
using UicApplication.Enums;
using UicApplication.Clusters;
using System;
using System.Text;
using Utils;

namespace ZWave.UicApplication
{
    class IncludeOperation : RequestDataOperation
    {
        private bool IsNetworkProtocolStatusReceived = false;
        private bool IsSmartStartStatusReceived = false;
        private string Dsk { get; set; }

        public IncludeOperation(string payload, int timeoutMs) : 
            base(new SmartStart.List.Update(), payload, null, timeoutMs, 2){ }

        protected override void OnReceived(DataReceivedUnit ou)
        {
            string mqttReceivedTopic = (ou.DataFrame as DataFrame).MqttTopic;
            string mqttReceivedPayload = (ou.DataFrame as DataFrame).MqttPayload;

            if (mqttReceivedTopic.Contains("/State"))
            {
                if (mqttReceivedPayload.Contains(NetworkStatesValues.Included) || mqttReceivedPayload.Contains(NetworkStatesValues.OnlineFunctional))
                {
                    SpecificResult.UnId = mqttReceivedTopic.Split('/')[2];
                    State.Payload payload = mqttReceivedPayload;
                    SpecificResult.Security = payload.Security;
                    IsNetworkProtocolStatusReceived = true;
                    if(IsSmartStartStatusReceived)
                    {
                        base.SetStateCompleted(ou);
                    }
                }
            }
            if (mqttReceivedTopic.Contains(SmartStart.ID) && mqttReceivedTopic.Contains(SmartStart.List.Update.ID))
            {
                SpecificResult.mqttReceivedTopic = (ou.DataFrame as DataFrame).MqttTopic;
                SpecificResult.mqttReceivedPayload = (ou.DataFrame as DataFrame).MqttPayload;
                if (_numberOfExpectedMessages == 1)
                {
                    if (SpecificResult.mqttReceivedTopic != null && SpecificResult.mqttReceivedTopic.Length > 0)
                    {

                    }
                    IsSmartStartStatusReceived = true;
                    if (IsNetworkProtocolStatusReceived)
                    {
                        base.SetStateCompleted(ou);
                    }
                }
                if (_numberOfExpectedMessages == 2)
                {
                    if (SpecificResult.mqttReceivedTopic != null && SpecificResult.mqttReceivedTopic.Length > 0)
                    {

                    }
                    IsSmartStartStatusReceived = true;
                    if (IsNetworkProtocolStatusReceived)
                    {
                        base.SetStateCompleted(ou);
                    }
                }
                if (_numberOfExpectedMessages > 1)
                {
                    _numberOfExpectedMessages--;
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
    public class InclusionResult : RequestDataResult
    {
        public string Security { get; set; }
        public bool IsRequestingPing { get; set; }
        public string Topic { get; set; }
        public string UnId { get; set; }

    }
}
