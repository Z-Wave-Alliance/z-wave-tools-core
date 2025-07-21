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
    public class CheckStateOperation : ActionBase
    {
        public CheckStateOperation()
            : base(true)
        {
        }

        CommandHandler handler;
        CommandHandler handlerSpi;
        CommandMessage message;
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 200, message));
            ActionUnits.Add(new DataReceivedUnit(handler, OnHandled));
            ActionUnits.Add(new DataReceivedUnit(handlerSpi, OnHandled));
        }

        protected override void CreateInstance()
        {
            message = new CommandMessage();
            message.AddData((byte)CommandTypes.CheckState, 0xFE, 0x00, 0x00);

            handler = new CommandHandler();
            handler.AddConditions(new ByteIndex((byte)CommandTypes.CheckState), new ByteIndex(0xFE), new ByteIndex(0x00), ByteIndex.AnyValue);

            handlerSpi = new CommandHandler();
            handlerSpi.AddConditions(new ByteIndex((byte)CommandTypes.CheckState), new ByteIndex(0x7F), new ByteIndex(0xFE), ByteIndex.AnyValue);
        }

        private void OnHandled(DataReceivedUnit ou)
        {
            SpecificResult.Value = ou.DataFrame.Payload[3];
            SetStateCompleted(ou);
        }

        public CheckStateResult SpecificResult
        {
            get { return (CheckStateResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new CheckStateResult();
        }
    }

    public class CheckStateResult : ActionResult
    {
        public byte Value { get; set; }
    }
}
