/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.BasicApplication.Enums;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0x46 | bSrcNodeID | bDstNodeID | funcID
    /// ZW->HOST: RES | 0x46 | retVal
    /// ZW->HOST: REQ | 0x46 | funcID | bStatus
    /// </summary>
    public class AssignReturnRouteOperation : CallbackApiOperation
    {
        internal NodeTag SrcNode { get; set; }
        internal NodeTag DestNode { get; set; }
        public AssignReturnRouteOperation(NetworkViewPoint network, NodeTag srcNode, NodeTag destNode)
            : base(CommandTypes.CmdZWaveAssignReturnRoute)
        {
            _network = network;
            SrcNode = srcNode;
            DestNode = destNode;
        }

        protected override byte[] CreateInputParameters()
        {
            var ret = new byte[] { (byte)SrcNode.Id, (byte)DestNode.Id };
            if (_network.IsNodeIdBaseTypeLR)
            {
                ret = new byte[] { (byte)(SrcNode.Id >> 8), (byte)SrcNode.Id, (byte)(DestNode.Id >> 8), (byte)DestNode.Id };
            }
            return ret;
        }

        protected override void OnCallbackInternal(DataReceivedUnit ou)
        {
            if (ou.DataFrame.Payload != null && ou.DataFrame.Payload.Length > 1)
            {
                SpecificResult.RetStatus = ou.DataFrame.Payload[1];
            }
        }

        public AssignReturnRouteResult SpecificResult
        {
            get { return (AssignReturnRouteResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new AssignReturnRouteResult();
        }
    }

    public class AssignReturnRouteResult : ActionResult
    {
        public byte RetStatus { get; set; }
    }
}
