/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using Utils;
using ZWave.BasicApplication.Enums;
using ZWave.Devices;
using ZWave.Security;

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

        /// <summary>
        /// ZW_GetRoutingInfo is a function that can be used to read out neighbor information from the protocol.
        /// </summary>
        /// <param name="network"></param>
        /// <param name="node"> Node whom routing info is needed from.</param>
        /// <param name="removeBadLinks">Remove bad link from routing info.
        /// Bad links are a short list of nodes which recently has failed to answer a transmission.</param>
        /// <param name="removeNonRepeaters">Remove non-repeaters from the routing info.</param>
        public GetRoutingInfoOperation(NetworkViewPoint network, NodeTag node, byte removeBadLinks, byte removeNonRepeaters)
            : base(CommandTypes.CmdGetRoutingTableLine, true)
        {
            _network = network;
            Node = node;
            RemoveBad = removeBadLinks;
            RemoveNonReps = removeNonRepeaters;
        }

        protected override byte[] CreateInputParameters()
        {
            var ret = new List<byte>();
            if (_network.IsNodeIdBaseTypeLR)
            {
                ret.Add((byte)(Node.Id >> 8));
            }
            ret.Add((byte)Node.Id);
            ret.Add(RemoveBad);
            ret.Add(RemoveNonReps);
            return ret.ToArray();
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
