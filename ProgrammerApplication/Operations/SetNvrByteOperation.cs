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
    public class SetNvrByteOperation : ActionBase
    {
        internal byte Address { get; set; }
        internal byte Data { get; set; }
        public SetNvrByteOperation(byte address, byte data)
            : base(true)
        {
            Address = address;
            Data = data;
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
            message.AddData((byte)CommandTypes.SetNvrByte, 0x00, Address, Data);

            handler = new CommandHandler();
            handler.AddConditions(new ByteIndex((byte)CommandTypes.SetNvrByte), new ByteIndex(0x00), new ByteIndex(Address), new ByteIndex(Data));

            handlerSpi = new CommandHandler();
            handlerSpi.AddConditions(new ByteIndex((byte)CommandTypes.SetNvrByte), new ByteIndex(0xFE), new ByteIndex(0x00), new ByteIndex(Address));
        }
    }
}
