/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using UicApplication;
using UicApplication.Data;
using UicApplication.Enums;
using UicApplication.Clusters;

namespace ZWave.UicApplication
{
    public class SetIdleOperation : RequestDataOperation
    {
        public string _unId;
        private bool IsSetDefaultReceived = false;
        private bool IsNetworkStatesValuesReceived = false;

        public SetIdleOperation(string unid, int timeoutMs)
            : base("ucl/by-unid/" + unid + "/ProtocolController/" + ProtocolController.NetworkManagement.ID + "/Write", 
                  new ProtocolController.Payload() { State = NetworkStates.Idle }, null, timeoutMs, 2)
        {
            _unId = unid;
        }
        protected override void OnReceived(DataReceivedUnit ou)
        {
            string mqttReceivedTopic = (ou.DataFrame as DataFrame).MqttTopic;
            string mqttReceivedPayload = (ou.DataFrame as DataFrame).MqttPayload;
                        
            if (mqttReceivedPayload.Contains(NetworkStatesValues.Idle))
            {
                IsNetworkStatesValuesReceived = true;
                if (IsSetDefaultReceived)
                {
                    base.SetStateCompleted(ou);
                }                
            }
            if (mqttReceivedPayload.Contains(NetworkStates.Idle))
            {
                SpecificResult.Topic = ((ou.DataFrame as DataFrame).MqttTopic);
                SpecificResult.UnId = SpecificResult.Topic.Split('/')[2];

                IsSetDefaultReceived = true;
                if (IsNetworkStatesValuesReceived)
                {
                    base.SetStateCompleted(ou);
                }
            }
        }

        public override string AboutMe()
        {
            return string.Format("Reseting Uic Device ={0}", _unId);
        }      

        public new SetIdleResult SpecificResult
        {
            get { return (SetIdleResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new SetIdleResult();
        }
    }
    public class SetIdleResult : RequestDataResult
    {
        public string Topic { get; set; }
        public string UnId { get; set; }
    }
}
