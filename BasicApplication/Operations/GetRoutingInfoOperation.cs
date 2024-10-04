/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Collections.Generic;
using System.Linq;
using ZWave.BasicApplication.Enums;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0x80 | bNodeID | bRemoveBad | bRemoveNonReps | funcID
    /// ZW->HOST: RES | 0x80 | NodeMask[29]
    /// </summary>
    public class GetRoutingInfoOperation : RequestApiOperation
    {
        private NodeTag Node { get; set; }
        private byte RemoveBad { get; set; }
        private byte RemoveNonReps { get; set; }
        public GetRoutingInfoOperation(NodeTag node, byte removeBad, byte removeNonReps)
            : base(CommandTypes.CmdGetRoutingTableLine, true)
        {
            Node = node;
            RemoveBad = removeBad;
            RemoveNonReps = removeNonReps;
        }

        protected override byte[] CreateInputParameters()
        {
            return new byte[] { (byte)Node.Id, RemoveBad, RemoveNonReps };
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            byte[] res = ((DataReceivedUnit)ou).DataFrame.Payload;
            byte nodeIdx = 0;
            List<byte> routingNodes = new List<byte>();
            for (int i = 0; i < res.Length; i++)
            {
                byte availabilityMask = res[i];
                for (byte bit = 0; bit < 8; bit++)
                {
                    nodeIdx++;
                    if ((availabilityMask & (1 << bit)) > 0)
                    {
                        routingNodes.Add(nodeIdx);
                    }
                }
            }
            SpecificResult.RoutingNodes = routingNodes.Select(x => new NodeTag(x)).ToArray();
            base.SetStateCompleted(ou);
        }

        public GetRoutingInfoResult SpecificResult
        {
            get { return (GetRoutingInfoResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new GetRoutingInfoResult();
        }
    }

    public class GetRoutingInfoResult : ActionResult
    {
        public NodeTag[] RoutingNodes { get; set; }
    }
}
