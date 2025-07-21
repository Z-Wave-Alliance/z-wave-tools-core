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
    public class SetFrequency4xOperation : ActionBase
    {
        public byte Frequency { get; set; }
        public SetFrequency4xOperation(byte frequency)
            : base(true)
        {
            Frequency = frequency;
        }
        public ZnifferApiMessage SetFrequency4xMessage { get; set; }
        public ZnifferApiHandler SetFrequency4xHandler { get; set; }

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 1000, SetFrequency4xMessage));
            ActionUnits.Add(new DataReceivedUnit(SetFrequency4xHandler, SetStateCompleted));
        }

        protected override void CreateInstance()
        {
            SetFrequency4xMessage = new ZnifferApiMessage(CommandTypes.SetFrequency4x, new byte[] { 0x01, Frequency });
            SetFrequency4xHandler = new ZnifferApiHandler(CommandTypes.SetFrequency4x);
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ActionResult();
        }
    }
}
