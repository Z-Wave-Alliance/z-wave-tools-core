/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZWave.ProgrammerApplication.Operations
{
    public class SyncByteOperation : ActionBase
    {
        public SyncByteOperation()
            : base(true)
        {
        }

        CommandMessage message;
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(SetStateCompleting, 0, message));
        }

        protected override void CreateInstance()
        {
            message = new CommandMessage();
            message.AddData(0x00);
        }
    }
}
