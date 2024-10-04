/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.Enums;
using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication
{
    public abstract class ControlApiOperation : ApiOperation
    {
        public static int TIMEOUT = 2000;

        public byte[] OutputParameters { get; set; }
        private readonly FrameTypes HandlerType;

        public ControlApiOperation(FrameTypes handlerType, CommandTypes command, bool isSequenceNumberRequired)
            : base(true, command, isSequenceNumberRequired)
        {
            HandlerType = handlerType;
        }

        public ControlApiOperation(CommandTypes command, bool useSequenceNumber)
            : this(FrameTypes.Response, command, useSequenceNumber)
        {
        }

        public ControlApiOperation(CommandTypes command, FrameTypes handlerType)
            : this(handlerType, command, true)
        {
        }

        public ControlApiOperation(CommandTypes command)
            : this(FrameTypes.Response, command, true)
        {
        }

        private ApiMessage message;
        private ApiHandler handler;

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, TIMEOUT, message));
            ActionUnits.Add(new DataReceivedUnit(handler, OnHandler));
        }

        protected override void CreateInstance()
        {
            message = new ApiMessage(SerialApiCommands[0], CreateInputParameters());
            if (IsSequenceNumberRequired)
                message.SetSequenceNumber(SequenceNumber);

            handler = new ApiHandler(HandlerType, SerialApiCommands[0]);
        }

        protected void OnHandler(DataReceivedUnit ou)
        {
            OutputParameters = ou.DataFrame.Payload;
            SpecificResult.ByteArray = OutputParameters;
            if (SpecificResult.ByteValue > 0)
            {
                SetStateCompleted(ou);
            }
            else
            {
                SetStateFailed(ou);
            }
        }

        protected abstract byte[] CreateInputParameters();

        public ReturnValueResult SpecificResult
        {
            get { return (ReturnValueResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ReturnValueResult();
        }
    }
}
