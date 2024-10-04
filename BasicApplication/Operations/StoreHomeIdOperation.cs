/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0x84 | pHomeID[0] | pHomeID[1] | pHomeID[2] | pHomeID[3] | bNodeID
    /// </summary>
    public class StoreHomeIdOperation : ControlNApiOperation
    {
        private byte[] HomeId { get; set; }
        private byte NodeId { get; set; }
        public StoreHomeIdOperation(byte[] homeId, byte nodeId)
            : base(CommandTypes.CmdStoreHomeId)
        {
            HomeId = homeId;
            NodeId = nodeId;
        }

        protected override byte[] CreateInputParameters()
        {
            return new byte[] { HomeId[0], HomeId[1], HomeId[2], HomeId[3], NodeId };
        }
    }
}
