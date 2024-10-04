/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZWave.ProgrammerApplication.Enums;
using Utils;

namespace ZWave.ProgrammerApplication.Operations
{
    public class SpiFwGetVersionOperation : ActionBase
    {
        public SpiFwGetVersionOperation()
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
            message.AddData((byte)CommandTypes.SpiFwGetVersion, 0x00, 0x00, 0x00);

            handler = new CommandHandler();
            handler.AddConditions(new ByteIndex((byte)CommandTypes.SpiFwGetVersion), new ByteIndex(0x03), ByteIndex.AnyValue, ByteIndex.AnyValue);
        }
    }
}
