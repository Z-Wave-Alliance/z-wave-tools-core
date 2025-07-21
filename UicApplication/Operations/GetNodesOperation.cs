/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using UicApplication;
using UicApplication.Data;
using UicApplication.Enums;
using UicApplication.Clusters;
using System.Collections.Generic;

namespace ZWave.UicApplication
{
    public class GetNodesOperation : UicApiOperation
    {
        internal string _homeId;
        internal string _unid;
        internal string _cluster;
        internal string _expectedTopic;
        internal int _timeout = 10000;

        private UicApiMessage message;
        private UicApiHandler expectReceived;

        public GetNodesOperation(string unid) : base(false)
        {
            _unid = "ucl/by-unid/"+unid;
            _cluster = "/"+State.ID;
        }

        public GetNodesOperation(string unid, int timeoutMs) : base(false)
        {
            _homeId = unid.Split('-')[1];
            _timeout = timeoutMs;
            _unid = "ucl/by-unid/" + "+";
            _cluster = "/" + State.ID + "/#";
        }

        protected override void CreateInstance()
        {

            message = new UicApiMessage(_unid, _cluster);
            expectReceived = new UicApiHandler(_expectedTopic);
        }

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, _timeout, message));
            ActionUnits.Add(new DataReceivedUnit(expectReceived, OnReceived));
            
            //ActionUnits.Add(new TimeElapsedUnit(5000, SetStateCompleted, 5000, null));
        }

        protected virtual void OnReceived(DataReceivedUnit ou)
        {
            string mqttReceivedPayload = (ou.DataFrame as DataFrame).MqttPayload ;
            string mqttReceivedTopic = (ou.DataFrame as DataFrame).MqttTopic;
            if (mqttReceivedTopic != null && mqttReceivedTopic.Length > 0 && mqttReceivedTopic.Contains(_homeId))
            {
                if (!SpecificResult.UnIds.Contains(mqttReceivedTopic.Split('/')[2]))
                {
                    SpecificResult.UnIds.Add(mqttReceivedTopic.Split('/')[2]);
                }
            }
        }

        public new GetNodesResult SpecificResult
        {
            get { return (GetNodesResult)Result; }
        }
        protected override ActionResult CreateOperationResult()
        {
            return new GetNodesResult();
        }
    }

    public class GetNodesResult : ActionResult
    {
        public List<string> UnIds = new List<string>();
    }
}
