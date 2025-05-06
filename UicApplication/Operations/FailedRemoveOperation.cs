/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using UicApplication;
using UicApplication.Data;
using UicApplication.Enums;
using UicApplication.Clusters;
using System.Collections.Generic;
using System.Threading;

namespace ZWave.UicApplication
{
    class FailedRemoveOperation : RequestDataOperation
    {
        private bool IsNetworkStatesFailedRemove = false;
        private bool IsNetworkStatesIdle = false;
        private string Dsk { get; set; }

        public FailedRemoveOperation(string unId, string removeUnId, int timeoutMs) :
            base("ucl/by-unid/" + unId + "/ProtocolController/" + ProtocolController.NetworkManagement.ID + "/Write",
                  new ProtocolController.Payload()
                  {
                      State = NetworkStates.RemoveFailed, StateParameters = new ProtocolController.StateParameters() { unid = removeUnId }
                  }, ProtocolController.ID, timeoutMs, 2)
        { }

        protected override void OnReceived(DataReceivedUnit ou)
        {
            string mqttReceivedTopic = (ou.DataFrame as DataFrame).MqttTopic;
            string mqttReceivedPayload = (ou.DataFrame as DataFrame).MqttPayload;

            if (mqttReceivedPayload.Contains(NetworkStates.RemoveFailed))
            {
                IsNetworkStatesFailedRemove = true;
                SpecificResult.mqttReceivedTopic = mqttReceivedTopic;
                SpecificResult.mqttReceivedPayload = mqttReceivedPayload;

                if (IsNetworkStatesIdle)
                {
                    base.SetStateCompleted(ou);
                }
            }
            if (mqttReceivedPayload.Contains(NetworkStates.Idle))
            {
                IsNetworkStatesIdle = true;
                if (IsNetworkStatesFailedRemove)
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
}
