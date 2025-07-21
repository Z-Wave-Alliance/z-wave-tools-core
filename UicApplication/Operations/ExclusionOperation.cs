/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using UicApplication;
using UicApplication.Data;
using UicApplication.Enums;
using UicApplication.Clusters;
using System.Collections.Generic;
using System.Threading;

namespace ZWave.UicApplication
{
    class ExclusionOperation : RequestDataOperation
    {
        private bool IsNetworkStatesReceived = false;
        private bool IsNetworkStatesValuesReceived = false;
        private string Dsk { get; set; }

        public ExclusionOperation(string unId, List<string> removeUnId, int timeoutMs) : 
            base("ucl/by-unid/" + unId + "/ProtocolController/" + ProtocolController.NetworkManagement.ID + "/Write",
                  new ProtocolController.Payload() { State = NetworkStates.RemoveNode }, null,timeoutMs,2)
        { }
        
        protected override void OnReceived(DataReceivedUnit ou)
        {
            string mqttReceivedTopic = (ou.DataFrame as DataFrame).MqttTopic;
            string mqttReceivedPayload = (ou.DataFrame as DataFrame).MqttPayload;
            
            if (mqttReceivedPayload.Contains(NetworkStatesValues.Removed))
            {                
                IsNetworkStatesValuesReceived = true;
                SpecificResult.mqttReceivedTopic = mqttReceivedTopic;
                SpecificResult.mqttReceivedPayload = mqttReceivedPayload;

                if (IsNetworkStatesReceived)
                {
                    base.SetStateCompleted(ou);
                }  
            }
            if (mqttReceivedPayload.Contains(NetworkStates.Idle))
            {
                SpecificResult.UnId = mqttReceivedTopic.Split('/')[2];
                IsNetworkStatesReceived = true;
                if (IsNetworkStatesValuesReceived)
                {
                    base.SetStateCompleted(ou);
                }
                
            }
        }

        public override string AboutMe()
        {
            return string.Format("Excluded device DSK={0}", SpecificResult.mqttReceivedTopic);
        }

        public new ExclusionResult SpecificResult
        {
            get { return (ExclusionResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ExclusionResult();
        }
    }
    public class ExclusionResult : RequestDataResult
    {
        public string Topic { get; set; }
        public string UnId { get; set; }
    }
}
