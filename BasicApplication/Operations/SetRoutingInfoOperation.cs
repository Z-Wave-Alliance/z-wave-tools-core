/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Collections.Generic;
using ZWave.BasicApplication.Enums;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0x1B | bNodeID | NodeMask[29]
    /// ZW->HOST: RES | 0x1B | retVal
    /// </summary>
    public class SetRoutingInfoOperation : ControlApiOperation
    {
        private NodeTag Node { get; set; }
        private byte[] NodeMask { get; set; }
        public SetRoutingInfoOperation(NodeTag node, byte[] nodeMask)
            : base(CommandTypes.CmdZWaveSetRoutingInfo, false)
        {
            Node = node;
            NodeMask = nodeMask;
        }

        protected override byte[] CreateInputParameters()
        {
            List<byte> ret = new List<byte>();
            ret.Add((byte)Node.Id);
            ret.AddRange(NodeMask);
            return ret.ToArray();
        }
    }
}
