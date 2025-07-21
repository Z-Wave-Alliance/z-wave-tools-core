/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.Enums;
using ZWave.BasicApplication.Enums;
using ZWave.Devices;

namespace ZWave.BasicApplication
{
    public abstract class RequestApiOperation : ApiOperation
    {
        public static int RET_TIMEOUT = 60000;

        internal int TimeoutMs { get; set; }
        public byte[] GetOutputParameters()
        {
            return handler.DataFrame.Data;
        }

        private readonly FrameTypes HandlerType;

        public RequestApiOperation(FrameTypes handlerType, CommandTypes command, bool isSequenceNumberRequired, int timeoutMs)
            : base(true, command, isSequenceNumberRequired)
        {
            TimeoutMs = timeoutMs;
            HandlerType = handlerType;
        }

        public RequestApiOperation(FrameTypes handlerType, CommandTypes command, bool isSequenceNumberRequired)
           : this(handlerType, command, isSequenceNumberRequired, RET_TIMEOUT)
        {
        }

        public RequestApiOperation(CommandTypes command, bool useSequenceNumber)
            : this(FrameTypes.Response, command, useSequenceNumber)
        {
        }

        public RequestApiOperation(CommandTypes command)
            : this(FrameTypes.Response, command, true)
        {
        }

        public RequestApiOperation(CommandTypes command, int timeoutMs)
           : this(FrameTypes.Response, command, true, timeoutMs)
        {
        }

        protected ApiMessage message;
        private ApiHandler handler;

        /// <summary>
        /// Fills OperationUnits list. Frame retransmittion timeout=2sec, so we waiting 7 seconds in operation
        /// </summary>
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, TimeoutMs, message));
            ActionUnits.Add(new DataReceivedUnit(handler, SetStateCompleted));
        }

        protected override void CreateInstance()
        {
            message = new ApiMessage(SerialApiCommands[0], CreateInputParameters());
            if (IsSequenceNumberRequired)
                message.SetSequenceNumber(SequenceNumber);
            handler = new ApiHandler(HandlerType, SerialApiCommands[0]);
        }

        protected abstract byte[] CreateInputParameters();


    }
}
