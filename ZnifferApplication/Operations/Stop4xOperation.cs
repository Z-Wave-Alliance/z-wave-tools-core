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
    public class Stop4xOperation : ActionBase
    {
        public Stop4xOperation()
            : base(true)
        {
        }

        public ZnifferApiMessage Stop4xMessage { get; set; }
        public ZnifferApiHandler Stop4xHandler { get; set; }

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 200, Stop4xMessage));
            ActionUnits.Add(new DataReceivedUnit(Stop4xHandler, SetStateCompleted));
        }

        protected override void CreateInstance()
        {
            Stop4xMessage = new ZnifferApiMessage(CommandTypes.Stop4x, new byte[] { 0x00 });
            Stop4xHandler = new ZnifferApiHandler(CommandTypes.Stop4x);
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ActionResult();
        }
    }
}
