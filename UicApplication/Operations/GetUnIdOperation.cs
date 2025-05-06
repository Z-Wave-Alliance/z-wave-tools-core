/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using UicApplication.Data;

namespace ZWave.UicApplication
{
    public class GetUnIdOperation : RequestDataOperation
    {
        public GetUnIdOperation() : base("ucl/by-unid/+", "/ProtocolController/NetworkManagement") { }
        protected override void OnReceived(DataReceivedUnit ou)
        {
            SpecificResult.Topic = ((ou.DataFrame as DataFrame).MqttTopic);
            SpecificResult.UnId = SpecificResult.Topic.Split('/')[2];
        }

        public override string AboutMe()
        {
            return string.Format("UnId={0}", SpecificResult.UnId);
        }

        public new GetUnIdResult SpecificResult
        {
            get { return (GetUnIdResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new GetUnIdResult();
        }
    }

    public class GetUnIdResult : ActionResult
    {
        public string Topic { get; set; }
        public string UnId { get; set; }
    }
}
