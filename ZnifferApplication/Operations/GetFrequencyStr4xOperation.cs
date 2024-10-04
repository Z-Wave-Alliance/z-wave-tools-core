/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using ZWave.ZnifferApplication.Enums;

namespace ZWave.ZnifferApplication.Operations
{
    public class GetFrequencyStr4xOperation : ActionBase
    {
        private const int TIMEOUT = 300;
        private byte Code { get; set; }

        public GetFrequencyStr4xOperation(byte frequencyCode)
            : base(true)
        {
            Code = frequencyCode;
        }
        public ZnifferApiMessage message { get; set; }
        public ZnifferApiHandler handler { get; set; }

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, TIMEOUT, message));
            ActionUnits.Add(new DataReceivedUnit(handler, SetStateCompleted));
        }

        protected override void CreateInstance()
        {
            message = new ZnifferApiMessage(CommandTypes.GetFrequencyStr4x, new byte[] { 0x01, Code });
            handler = new ZnifferApiHandler(CommandTypes.GetFrequencyStr4x);
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            byte[] payload = ((DataReceivedUnit)ou).DataFrame.Payload;
            if (payload != null && payload.Length > 2)
            {
                byte CurrentFrequency = payload[0];
                SpecificResult.Channels = payload[1];

                System.Text.UTF7Encoding utf = new System.Text.UTF7Encoding();
                byte[] res = new byte[payload.Length - 2];
                Array.Copy(payload, 2, res, 0, payload.Length - 2);
                SpecificResult.Name = utf.GetString(res, 0, res.Length);
            }
            base.SetStateCompleted(ou);
        }

        public GetFrequencyStr4xResult SpecificResult
        {
            get { return (GetFrequencyStr4xResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new GetFrequencyStr4xResult();
        }
    }

    public class GetFrequencyStr4xResult : ActionResult
    {
        public byte Channels { get; set; }
        public string Name { get; set; }
    }
}
