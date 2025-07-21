/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;
using UicApplication;
using UicApplication.Data;
using UicApplication.Enums;

namespace ZWave.UicApplication
{
    public class ListenDataOperation : UicApiOperation
    {
        private string Topic { get; set; }
        private int TimeoutMs { get; set; }

        private readonly ListenDataDelegate _listenCallback;

        public ListenDataOperation(string topic, int timeoutMs, ListenDataDelegate listenCallback)
            : base(false)
        {
            Topic = topic;
            TimeoutMs = timeoutMs;
            _listenCallback = listenCallback;
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
            ExpectDataResult res = new ExpectDataResult();
            res.mqttReceivedTopic = (ou.DataFrame as DataFrame).MqttTopic;
            res.mqttReceivedPayload = (ou.DataFrame as DataFrame).MqttPayload;
            SpecificResult.mqttMessages.Add(res);
            SpecificResult.TotalCount++;
            _listenCallback?.Invoke((ou.DataFrame as DataFrame));

        }

        public ListenDataResult SpecificResult
        {
            get { return (ListenDataResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ListenDataResult();
        }
    }

    public class ListenDataResult : ActionResult
    {
        public int TotalCount { get; set; }

        public List<ExpectDataResult> mqttMessages = new List<ExpectDataResult>();
        
    }

    public delegate void ListenDataDelegate(DataFrame appCmdHandlerData);
}
