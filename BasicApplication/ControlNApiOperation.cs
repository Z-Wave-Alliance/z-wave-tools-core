/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication
{
    public abstract class ControlNApiOperation : ApiOperation
    {
        public ControlNApiOperation(CommandTypes command)
            : base(false, new CommandTypes[] { command }, false)
        {
        }

        public ControlNApiOperation(CommandTypes command, bool useSequenceNumber)
            : base(false, new CommandTypes[] { command }, useSequenceNumber)
        {
        }

        private ApiMessage message;
        protected bool _isNoAck = false;

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(SetStateCompleting, 0, message));
        }

        protected override void CreateInstance()
        {
            message = new ApiMessage(SerialApiCommands[0], CreateInputParameters()); // no seqNo
            if (IsSequenceNumberRequired)
            {
                message.SetSequenceNumber(SequenceNumber);
            }
            message.IsNoAck = _isNoAck;
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ActionResult();
        }

        protected abstract byte[] CreateInputParameters();
    }
}
