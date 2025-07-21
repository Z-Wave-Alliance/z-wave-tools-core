/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;
using ZWave.ProgrammerApplication.Enums;

namespace ZWave.ProgrammerApplication.Operations
{
    public class EnableEooSModeOperation : ActionBase
    {
        public EnableEooSModeOperation()
            : base(true)
        {
        }

        CommandHandler handler;
        CommandHandler handlerSpi;
        CommandMessage message;
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 2000, message));
            ActionUnits.Add(new DataReceivedUnit(handler, SetStateCompleted));
            ActionUnits.Add(new DataReceivedUnit(handlerSpi, SetStateCompleted));
        }

        protected override void CreateInstance()
        {
            message = new CommandMessage();
            message.AddData((byte)CommandTypes.EnableEooSMode, 0x00, 0x00, 0x00);

            handler = new CommandHandler();
            handler.AddConditions(new ByteIndex((byte)CommandTypes.EnableEooSMode), new ByteIndex(0x00), new ByteIndex(0x00), new ByteIndex(0x00));

            handlerSpi = new CommandHandler();
            handlerSpi.AddConditions(new ByteIndex((byte)CommandTypes.EnableEooSMode), new ByteIndex(0xC0), new ByteIndex(0x00), new ByteIndex(0x00));
        }
    }
}
