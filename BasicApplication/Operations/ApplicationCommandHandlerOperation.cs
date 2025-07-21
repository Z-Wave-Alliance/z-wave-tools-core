/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using ZWave.BasicApplication.Enums;
using ZWave.Enums;

namespace ZWave.BasicApplication.Operations
{
    [Obsolete]
    public class ApplicationCommandHandlerOperation : ApiOperation
    {
        public Action<byte[]> ReceivedCallback { get; set; }
        public ApplicationCommandHandlerOperation()
            : base(false, CommandTypes.CmdApplicationCommandHandler, false)
        {
        }

        private ApiHandler applicationCommandHandler;
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 0));
            ActionUnits.Add(new DataReceivedUnit(applicationCommandHandler, OnReceived));
        }

        protected override void CreateInstance()
        {
            applicationCommandHandler = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
        }

        private void OnReceived(DataReceivedUnit ou)
        {
            ReceivedCallback(ou.DataFrame.Payload);
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ActionResult();
        }
    }
}
