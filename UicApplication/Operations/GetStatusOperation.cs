/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Collections.Generic;
using UicApplication.Data;

namespace ZWave.UicApplication
{
    public class GetStatusOperation : RequestDataOperation
    {
        private string _requestedUnId { get; set; }

        public GetStatusOperation(string unId) : base("ucl/by-unid/+", "/ProtocolController/NetworkManagement")
        {
            _requestedUnId = unId;
        }
        protected override void OnReceived(DataReceivedUnit ou)
        {
            var receivedUnid = (ou.DataFrame as DataFrame).MqttTopic.Split('/')[2];
            var receivedPayload = (ou.DataFrame as DataFrame).MqttPayload;
            if (string.IsNullOrEmpty(_requestedUnId))
            {
                if (!SpecificResult.NetworkInfo.ContainsKey(receivedUnid))
                {
                    SpecificResult.NetworkInfo.Add(receivedUnid, receivedPayload);
                }
            }
            else if (receivedUnid.ToLowerInvariant() == _requestedUnId.ToLowerInvariant() && !receivedPayload.ToLowerInvariant().Contains("reset"))
            {
                SpecificResult.UnId = receivedUnid;
                SpecificResult.Payload = receivedPayload;
            }
        }

        public override string AboutMe()
        {
            return string.Format("UnId={0}", SpecificResult.UnId);
        }

        public new GetStatusResult SpecificResult
        {
            get { return (GetStatusResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new GetStatusResult();
        }
    }

    public class GetStatusResult : ActionResult
    {
        public Dictionary<string, string> NetworkInfo { get; set; } = new Dictionary<string, string>();
        public string UnId { get; set; }
        public string Payload { get; set; }
    }
}
