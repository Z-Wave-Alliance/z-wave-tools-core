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
    public class SetFrequency3xOperation : ActionBase
    {
        public byte Frequency { get; set; }
        public SetFrequency3xOperation(byte frequency)
            : base(true)
        {
            Frequency = frequency;
            if (Frequency == 26) //for RU frequency 
                Frequency = 10;
            if (Frequency == 27) //for IL frequency 
                Frequency = 11;
        }
        public ZnifferApiHandler SetFrequency3xHandler { get; set; }
        public ZnifferApiMessage SetFrequency3xMessage { get; set; }


        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 1000, SetFrequency3xMessage));
            ActionUnits.Add(new DataReceivedUnit(SetFrequency3xHandler, SetStateCompleted));
        }

        protected override void CreateInstance()
        {
            SetFrequency3xHandler = new ZnifferApiHandler(CommandTypes.SetFrequency3xResponse);
            SetFrequency3xMessage = new ZnifferApiMessage(CommandTypes.SetFrequency3x, new byte[] { (byte)(char.Parse(Frequency.ToString("X"))) });
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ActionResult();
        }
    }
}
