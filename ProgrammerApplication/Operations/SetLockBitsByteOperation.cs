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
    public class SetLockBitsByteOperation : ActionBase
    {
        internal byte ByteNumber { get; set; }
        internal byte Data { get; set; }
        public SetLockBitsByteOperation(byte byteNumber, byte data)
            : base(true)
        {
            ByteNumber = byteNumber;
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
            message.AddData((byte)CommandTypes.SetLockBits, ByteNumber, 0xFF, Data);

            handler = new CommandHandler();
            handler.AddConditions(new ByteIndex((byte)CommandTypes.SetLockBits), new ByteIndex(ByteNumber), new ByteIndex(0xFF), new ByteIndex(Data));

            handlerSpi = new CommandHandler();
            handlerSpi.AddConditions(new ByteIndex((byte)CommandTypes.SetLockBits), new ByteIndex(0xF0), new ByteIndex(ByteNumber), new ByteIndex(0xFF));
        }
    }
}
