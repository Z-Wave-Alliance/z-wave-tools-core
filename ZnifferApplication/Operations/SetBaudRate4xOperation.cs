/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZWave;
using ZWave.Enums;
using ZWave.ZnifferApplication.Enums;

namespace ZWave.ZnifferApplication.Operations
{
    public class SetBaudRate4xOperation : ActionBase
    {
        public BaudRates BaudRate { get; set; }
        public SetBaudRate4xOperation(BaudRates baudRate)
            : base(true)
        {
            BaudRate = baudRate;
        }
        public ZnifferApiMessage SetBaudRate4xMessage { get; set; }
        public ZnifferApiHandler SetBaudRate4xHandler { get; set; }

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 1000, SetBaudRate4xMessage));
            ActionUnits.Add(new DataReceivedUnit(SetBaudRate4xHandler, SetStateCompleted));
        }

        protected override void CreateInstance()
        {
            byte baudCode = 0;
            switch (BaudRate)
            {
                case BaudRates.Rate_115200:
                    baudCode = 0;
                    break;
                case BaudRates.Rate_230400:
                    baudCode = 1;
                    break;
                default:
                    baudCode = 0;
                    break;
            }
            SetBaudRate4xMessage = new ZnifferApiMessage(CommandTypes.SetBaudRate4x, new byte[] { 0x01, baudCode });
            SetBaudRate4xHandler = new ZnifferApiHandler(CommandTypes.SetBaudRate4x);
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ActionResult();
        }
    }
}
