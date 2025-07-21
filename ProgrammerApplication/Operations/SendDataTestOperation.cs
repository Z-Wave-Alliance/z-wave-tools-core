/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace ZWave.ProgrammerApplication.Operations
{
    public class SendDataTestOperation : ActionBase
    {
        internal byte[] Data { get; set; }
        public SendDataTestOperation(byte[] data)
            : base(true)
        {
            Data = data;
        }

        CommandMessage message;
        CommandHandler handler;

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 2000, message));
            ActionUnits.Add(new DataReceivedUnit(handler, SetStateCompleted));
        }

        protected override void CreateInstance()
        {
            message = new CommandMessage();
            message.AddData(Data);
            handler = new CommandHandler();
            handler.AddConditions(ByteIndex.AnyValue);
        }
    }
}
