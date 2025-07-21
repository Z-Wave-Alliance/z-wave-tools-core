/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.BasicApplication.Enums;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0x51 | bSrcNodeID | funcID | funcID
    /// The extra funcID is added to ensures backward compatible. 
    /// This parameter has been removed starting from dev. kit  4.1x. and onwards and has therefore no meaning anymore.
    /// ZW->HOST: RES | 0x51 | retVal
    /// ZW->HOST: REQ | 0x51 | funcID | bStatus
    /// </summary>
    public class AssignSucReturnRouteOperation : CallbackApiOperation
    {
        public NodeTag SrcNode { get; set; }
        public AssignSucReturnRouteOperation(NetworkViewPoint network, NodeTag srcNode)
            : base(CommandTypes.CmdZWaveAssignSucReturnRoute)
        {
            _network = network;
            SrcNode = srcNode;
        }

        protected override byte[] CreateInputParameters()
        {
            var ret = new byte[] { (byte)SrcNode.Id, SequenceNumber };
            if (_network.IsNodeIdBaseTypeLR)
            {
                ret = new byte[] { (byte)(SrcNode.Id >> 8), (byte)SrcNode.Id, SequenceNumber };
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

        public AssignSucReturnRouteResult SpecificResult
        {
            get { return (AssignSucReturnRouteResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new AssignSucReturnRouteResult();
        }
    }

    public class AssignSucReturnRouteResult : ActionResult
    {
        public byte RetStatus { get; set; }
    }
}
