/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;
using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0x83 | bNodeID | nodeInfo (nodeInfo is a NODEINFO field) | funcID
    /// ZW->HOST: RES | 0x83 | retVal
    /// ZW->HOST: REQ| 0x83 | funcID
    /// </summary>
    public class StoreNodeInfoOperation : CallbackApiOperation
    {
        private byte NodeId { get; set; }
        private byte[] NodeInfo { get; set; }
        public StoreNodeInfoOperation(byte nodeId, byte[] nodeInfo)
            : base(CommandTypes.CmdStoreNodeInfo)
        {
            NodeId = nodeId;
            NodeInfo = nodeInfo;
        }

        protected override byte[] CreateInputParameters()
        {
            List<byte> ret = new List<byte>();
            ret.Add(NodeId);
            ret.AddRange(NodeInfo);
            return ret.ToArray();
        }

        public StoreNodeInfoResult SpecificResult
        {
            get { return (StoreNodeInfoResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new StoreNodeInfoResult();
        }
    }

    public class StoreNodeInfoResult : ActionResult
    {
    }
}
