/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using UicApplication;
using UicApplication.Data;
using UicApplication.Enums;
using UicApplication.Clusters;

namespace ZWave.UicApplication
{
    public class SetDefaultOperation : RequestDataOperation
    {
        public string _unId;
        private bool IsSetDefaultReceived = false;
        private bool IsNetworkStatesValuesReceived = false;

        public SetDefaultOperation(string unid, int timeoutMs)
            : base("ucl/by-unid/" + unid + "/ProtocolController/" + ProtocolController.NetworkManagement.ID + "/Write", 
                  new ProtocolController.Payload() { State = NetworkStates.Reset }, null, timeoutMs, 2)
        {
            _unId = unid;
        }
        protected override void OnReceived(DataReceivedUnit ou)
        {
            string mqttReceivedTopic = (ou.DataFrame as DataFrame).MqttTopic;
            string mqttReceivedPayload = (ou.DataFrame as DataFrame).MqttPayload;
                        
            if (mqttReceivedPayload.Contains(NetworkStatesValues.Reset))
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

        public new SetDefaultResult SpecificResult
        {
            get { return (SetDefaultResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new SetDefaultResult();
        }
    }
    public class SetDefaultResult : RequestDataResult
    {
        public string Topic { get; set; }
        public string UnId { get; set; }
    }
}
