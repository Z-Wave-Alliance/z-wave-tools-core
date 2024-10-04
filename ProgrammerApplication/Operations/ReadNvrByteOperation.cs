/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;
using ZWave.ProgrammerApplication.Enums;

namespace ZWave.ProgrammerApplication.Operations
{
    public class ReadNvrByteOperation : ActionBase
    {
        internal byte Address { get; set; }
        public ReadNvrByteOperation(byte pageIndex, byte address)
            : base(true)
        {
            Address = address;
        }

        CommandHandler handler;
        CommandHandler handlerSpi;
        CommandMessage message;
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 2000, message));
            ActionUnits.Add(new DataReceivedUnit(handler, OnHandled));
            ActionUnits.Add(new DataReceivedUnit(handlerSpi, OnHandled));
        }

        protected override void CreateInstance()
        {
            message = new CommandMessage();
            message.AddData((byte)CommandTypes.ReadNvrByte, 0x00, Address, 0xFF);

            handler = new CommandHandler();
            handler.AddConditions(new ByteIndex((byte)CommandTypes.ReadNvrByte), new ByteIndex(0x00), new ByteIndex(Address), ByteIndex.AnyValue);

            handlerSpi = new CommandHandler();
            handlerSpi.AddConditions(new ByteIndex((byte)CommandTypes.ReadNvrByte), new ByteIndex(0xF2), new ByteIndex(0x00), ByteIndex.AnyValue);
        }

        private void OnHandled(DataReceivedUnit ou)
        {
            SpecificResult.Value = ou.DataFrame.Payload[3];
            SetStateCompleted(ou);
        }

        public ReadNvrByteResult SpecificResult
        {
            get { return (ReadNvrByteResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ReadNvrByteResult();
        }
    }

    public class ReadNvrByteResult : ActionResult
    {
        public byte Value { get; set; }
    }
}
