/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using ZWave.BasicApplication.Enums;
using ZWave.Enums;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// The Z Wave protocol also calls ApplicationEndDeviceUpdate when a Node Information Frame has been received
    /// and the protocol is not in a state where it needs the node information.
    /// All end device libraries requires this function implemented within the System layer.
    /// 
    /// ZW->HOST: REQ | 0x49 | bStatus | bNodeID | bLen | basic | generic | specific | commandclasses[ ]
    /// </summary>
    [Obsolete]
    public class ApplicationEndDeviceUpdateOperation : ApiOperation
    {
        public Action<byte[]> UpdateCallback { get; set; }
        public ApplicationEndDeviceUpdateOperation()
            : base(false, CommandTypes.CmdApplicationControllerUpdate, false)
        {
        }

        private ApiHandler applicationEndDeviceUpdateHandler;
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 0));
            ActionUnits.Add(new DataReceivedUnit(applicationEndDeviceUpdateHandler, OnReceived));
        }

        protected override void CreateInstance()
        {
            applicationEndDeviceUpdateHandler = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ActionResult();
        }

        private void OnReceived(DataReceivedUnit ou)
        {
            UpdateCallback(ou.DataFrame.Payload);
        }
    }
}
