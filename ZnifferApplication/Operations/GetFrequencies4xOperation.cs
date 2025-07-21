/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using ZWave.ZnifferApplication.Enums;

namespace ZWave.ZnifferApplication.Operations
{
    public class GetFrequencies4xOperation : ActionBase
    {
        public byte CurrentFrequency { get; set; }
        public byte[] Frequencies { get; set; }

        public GetFrequencies4xOperation()
            : base(true)
        {
        }
        public ZnifferApiMessage GetFrequencies4xMessage { get; set; }
        public ZnifferApiHandler GetFrequencies4xHandler { get; set; }

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 1000, GetFrequencies4xMessage));
            ActionUnits.Add(new DataReceivedUnit(GetFrequencies4xHandler, SetStateCompleted));
        }

        protected override void CreateInstance()
        {
            GetFrequencies4xMessage = new ZnifferApiMessage(CommandTypes.GetFrequencies4x, new byte[] { 0x00 });
            GetFrequencies4xHandler = new ZnifferApiHandler(CommandTypes.GetFrequencies4x);
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            byte[] payload = ((DataReceivedUnit)ou).DataFrame.Payload;
            if (payload != null && payload.Length > 1)
            {
                CurrentFrequency = payload[0];
                Frequencies = new byte[payload.Length - 1];
                Array.Copy(payload, 1, Frequencies, 0, payload.Length - 1);
            }
            base.SetStateCompleted(ou);
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ActionResult();
        }
    }
}
