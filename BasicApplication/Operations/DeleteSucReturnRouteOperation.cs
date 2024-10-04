/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0x55 | nodeID | funcID
    /// ZW->HOST: RES | 0x55 | retVal
    /// ZW->HOST: REQ | 0x55 | funcID | bStatus
    /// </summary>
    public class DeleteSucReturnRouteOperation : CallbackApiOperation
    {
        public NodeTag Node { get; set; }
        public DeleteSucReturnRouteOperation(NetworkViewPoint network, NodeTag node)
            : base(CommandTypes.CmdZWaveDeleteSucReturnRoute)
        {
            _network = network;
            Node = node;
        }

        protected override byte[] CreateInputParameters()
        {
            var ret = new byte[] { (byte)Node.Id };
            if (_network.IsNodeIdBaseTypeLR)
            {
                ret = new byte[] { (byte)(Node.Id >> 8), (byte)Node.Id };
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

        public DeleteSucReturnRouteResult SpecificResult
        {
            get { return (DeleteSucReturnRouteResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new DeleteSucReturnRouteResult();
        }
    }

    public class DeleteSucReturnRouteResult : ActionResult
    {
        public byte RetStatus { get; set; }
    }
}
