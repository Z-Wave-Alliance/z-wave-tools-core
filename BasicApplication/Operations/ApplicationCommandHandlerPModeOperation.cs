/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using ZWave.BasicApplication.Enums;
using ZWave.Enums;

namespace ZWave.BasicApplication.Operations
{
    [Obsolete]
    public class ApplicationCommandHandlerOperationPMode : ApiOperation
    {
        public Action<byte[]> ReceivedCallback { get; set; }
        public ApplicationCommandHandlerOperationPMode()
            : base(false, CommandTypes.CmdApplicationCommandHandlerPMode, false)
        {
        }

        private ApiHandler applicationCommandHandlerPMode;
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 0));
            ActionUnits.Add(new DataReceivedUnit(applicationCommandHandlerPMode, OnReceivedPMode));
        }

        protected override void CreateInstance()
        {
            applicationCommandHandlerPMode = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
        }

        private void OnReceivedPMode(DataReceivedUnit ou)
        {
            ReceivedCallback(ou.DataFrame.Payload);
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ActionResult();
        }
    }
}
