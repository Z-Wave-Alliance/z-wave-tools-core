/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using UicApplication;
using UicApplication.Data;
using UicApplication.Enums;

namespace ZWave.UicApplication
{
    public class ExpectDataOperation : UicApiOperation
    {
        private string Topic { get; set; }
        private int TimeoutMs { get; set; }
        private int HitCount { get; set; }

        public ExpectDataOperation(string topic, int timeoutMs)
            : base(false)
        {
            Topic = topic;
            TimeoutMs = timeoutMs;
            HitCount = 1;
        }

        public ExpectDataOperation(string topic, int timeoutMs, int hitCount)
            : base(false)
        {
            Topic = topic;
            TimeoutMs = timeoutMs;
            HitCount = hitCount;
        }

        public ExpectDataOperation(string UnId, string topic, int timeoutMs)
            : base(false)
        {
            Topic = "ucl/by-unid/" + UnId + topic;
            TimeoutMs = timeoutMs;
            HitCount = 1;
        }
        public ExpectDataOperation(string MachineId, string topic)
            : base(false)
        {
            Topic = "ucl/" + MachineId + "/raspberrypi/" + topic;
            TimeoutMs = 20000;
            HitCount = 1;
        }
        public ExpectDataOperation(string MqttClient, string ClientName, string topic)
            : base(false)
        {
            Topic = "ucl/" + MqttClient + "/" + ClientName + topic;
            TimeoutMs = 20000;
            HitCount = 1;
        }

        public ExpectDataOperation(string UnId, string ep, string topic, int timeoutMs)
            : base(false)
        {
            Topic = "ucl/by-unid/" + UnId + "/" + ep  + topic;
            TimeoutMs = timeoutMs;
            HitCount = 1;
        }

        public ExpectDataOperation(string UnId, string ep, string topic, int timeoutMs, int hitCount)
            : base(false)
        {
            Topic = "ucl/by-unid/" + UnId + "/" + ep + topic;
            TimeoutMs = timeoutMs;
            HitCount = hitCount;
        }

        public ExpectDataOperation(string ep, string topic, int timeoutMs, int hitCount)
            : base(false)
        {
            Topic = ep + topic;
            TimeoutMs = timeoutMs;
            HitCount = hitCount;
        }

        private UicApiHandler expectReceived;
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, TimeoutMs));
            ActionUnits.Add(new DataReceivedUnit(expectReceived, OnReceived));
        }

        protected override void CreateInstance()
        {
            expectReceived = new UicApiHandler(Topic);
        }

        private void OnReceived(DataReceivedUnit ou)
        {
            if (HitCount == 1)
            {
                SpecificResult.mqttReceivedTopic = (ou.DataFrame as DataFrame).MqttTopic;
                SpecificResult.mqttReceivedPayload = (ou.DataFrame as DataFrame).MqttPayload;
                if (SpecificResult.mqttReceivedTopic != null && SpecificResult.mqttReceivedTopic.Length > 0)
                {

                }
                base.SetStateCompleted(ou);
            }
            else
            {
                HitCount--;
            }
        }

        public ExpectDataResult SpecificResult
        {
            get { return (ExpectDataResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ExpectDataResult();
        }
    }

    public class ExpectDataResult : ActionResult
    {
        public string mqttReceivedTopic { get; set; }
        public string mqttReceivedPayload { get; set; }
    }
}
