/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZWave.ProgrammerApplication.Enums;
using Utils;

namespace ZWave.ProgrammerApplication.Operations
{
    public class SpiFwAssertResetNOperation : ActionBase
    {
        public SpiFwAssertResetNOperation()
            : base(true)
        {
        }

        CommandHandler handler;
        CommandMessage message;
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 2000, message));
            ActionUnits.Add(new DataReceivedUnit(handler, SetStateCompleted));
        }

        protected override void CreateInstance()
        {
            message = new CommandMessage();
            message.AddData((byte)CommandTypes.SpiFwSetPin, 0x04, 0x01, 0x00);

            handler = new CommandHandler();
            handler.AddConditions(new ByteIndex((byte)CommandTypes.SpiFwSetPin), new ByteIndex(0x04), new ByteIndex(0x01), new ByteIndex(0x00));
        }
    }
}
