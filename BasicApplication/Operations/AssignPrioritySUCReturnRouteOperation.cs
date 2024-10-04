/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0x58 | bSrcNodeID | PriorityRoute | funcID
    /// ZW->HOST: RES | 0x58 | retVal
    /// ZW->HOST: REQ | 0x58 | funcID | bStatus
    /// </summary>
    /// 
    public class AssignPrioritySucReturnRouteOperation : CallbackApiOperation
    {
        public NodeTag[] Repeaters { get; set; }
        public NodeTag Source { get; set; }
        public byte RouteSpeed { get; set; }
        public AssignPrioritySucReturnRouteOperation(NetworkViewPoint network, NodeTag source, NodeTag repeater0, NodeTag repeater1, NodeTag repeater2, NodeTag repeater3, byte routespeed)
            : base(CommandTypes.CmdZWaveAssignPrioritySucReturnRoute)
        {
            _network = network;
            Repeaters = new[] { repeater0, repeater1, repeater2, repeater3 };
            Source = source;
            RouteSpeed = routespeed;
        }

        protected override byte[] CreateInputParameters()
        {
            if (_network.IsNodeIdBaseTypeLR)
            {
                byte[] ret = new byte[Repeaters.Length + 3];
                ret[0] = (byte)(Source.Id >> 8);
                ret[1] = (byte)Source.Id;
                for (int i = 0; i < Repeaters.Length; i++)
                {
                    ret[i + 2] = (byte)Repeaters[i].Id;
                }
                ret[ret.Length - 1] = RouteSpeed;
                return ret;
            }
            else
            {
                byte[] ret = new byte[Repeaters.Length + 2];
                ret[0] = (byte)Source.Id;
                for (int i = 0; i < Repeaters.Length; i++)
                {
                    ret[i + 1] = (byte)Repeaters[i].Id;
                }
                ret[ret.Length - 1] = RouteSpeed;
                return ret;
            }
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
}
