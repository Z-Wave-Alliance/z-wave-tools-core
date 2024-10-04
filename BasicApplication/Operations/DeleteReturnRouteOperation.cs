/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0x47 | nodeID | funcID
    /// ZW->HOST: RES | 0x47 | retVal
    /// ZW->HOST: REQ | 0x47 | funcID | bStatus
    /// </summary>
    public class DeleteReturnRouteOperation : CallbackApiOperation
    {
        internal NodeTag Node { get; set; }
        public DeleteReturnRouteOperation(NetworkViewPoint network, NodeTag node)
            : base(CommandTypes.CmdZWaveDeleteReturnRoute)
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

        public DeleteReturnRouteResult SpecificResult
        {
            get { return (DeleteReturnRouteResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new DeleteReturnRouteResult();
        }
    }

    public class DeleteReturnRouteResult : ActionResult
    {
        public byte RetStatus { get; set; }
    }
}
