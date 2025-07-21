/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZWave;
using ZWave.Enums;
using ZWave.ZnifferApplication.Enums;

namespace ZWave.ZnifferApplication.Operations
{
    public class Start4xOperation : ActionBase
    {
        public Start4xOperation()
            : base(true)
        {
        }

        public ZnifferApiMessage Start4xMessage { get; set; }
        public ZnifferApiHandler Start4xHandler { get; set; }

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 1000, Start4xMessage));
            ActionUnits.Add(new DataReceivedUnit(Start4xHandler, SetStateCompleted));
        }

        protected override void CreateInstance()
        {
            Start4xMessage = new ZnifferApiMessage(CommandTypes.Start4x, new byte[] { 0x00 });
            Start4xHandler = new ZnifferApiHandler(CommandTypes.Start4x);
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ActionResult();
        }
    }
}
