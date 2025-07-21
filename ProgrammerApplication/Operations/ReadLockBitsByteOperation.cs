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
    public class ReadLockBitsByteOperation : ActionBase
    {
        internal byte ByteNumber { get; set; }
        public ReadLockBitsByteOperation(byte pageIndex, byte byteNumber)
            : base(true)
        {
            ByteNumber = byteNumber;
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
            message.AddData((byte)CommandTypes.ReadLockBits, ByteNumber, 0xFF, 0xFF);

            handler = new CommandHandler();
            handler.AddConditions(new ByteIndex((byte)CommandTypes.ReadLockBits), new ByteIndex(ByteNumber), new ByteIndex(0xFF), ByteIndex.AnyValue);

            handlerSpi = new CommandHandler();
            handlerSpi.AddConditions(new ByteIndex((byte)CommandTypes.ReadLockBits), new ByteIndex(0xF1), new ByteIndex(ByteNumber), ByteIndex.AnyValue);
        }

        private void OnHandled(DataReceivedUnit ou)
        {
            SpecificResult.Value = ou.DataFrame.Payload[3];
            SetStateCompleted(ou);
        }

        public ReadLockBitsByteResult SpecificResult
        {
            get { return (ReadLockBitsByteResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ReadLockBitsByteResult();
        }
    }

    public class ReadLockBitsByteResult : ActionResult
    {
        public byte Value { get; set; }
    }
}
