/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class SerialApiSetTimeoutsOperation : ApiOperation
    {
        public byte OldRxAckTimeout { get; set; }
        public byte OldRxByteTimeout { get; set; }
        private readonly byte RxAckTimeout;
        private readonly byte RxByteTimeout;
        public SerialApiSetTimeoutsOperation(byte rxAckTimeout, byte rxByteTimeout)
            :base(true, CommandTypes.CmdSerialApiSetTimeouts, true)
        {
            RxAckTimeout = rxAckTimeout;
            RxByteTimeout = rxByteTimeout;
        }
        ApiMessage message;
        ApiHandler handler;

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 2000, message));
            ActionUnits.Add(new DataReceivedUnit(handler, SetStateCompleted, message));
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            OldRxAckTimeout = ((DataReceivedUnit)ou).DataFrame.Payload[0];
            OldRxByteTimeout = ((DataReceivedUnit)ou).DataFrame.Payload[1];
            base.SetStateCompleted(ou);
        }

        protected override void CreateInstance()
        {
            message = new ApiMessage(SerialApiCommands[0], RxAckTimeout, RxByteTimeout);
            message.SetSequenceNumber(SequenceNumber);

            handler = new ApiHandler(SerialApiCommands[0]);
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ActionResult();
        }
    }
}
