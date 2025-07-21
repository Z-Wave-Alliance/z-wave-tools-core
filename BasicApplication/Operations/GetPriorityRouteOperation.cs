/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using ZWave.BasicApplication.Enums;
using Utils;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0x92 | bNodeID
    /// ZW->HOST: RES | 0x92 | bNodeID | retVal | repeater0 | repeater1 | repeater2 | repeater3 | routespeed
    /// </summary>

    public class GetPriorityRouteOperation : ApiOperation
    {
        public NodeTag Destination { get; set; }
        public GetPriorityRouteOperation(NetworkViewPoint network, NodeTag destination)
            : base(true, CommandTypes.CmdZWaveGetPriorityRoute, false)
        {
            _network = network;
            Destination = destination;
        }

        ApiMessage message;
        ApiHandler handler;

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 1000, message));
            ActionUnits.Add(new DataReceivedUnit(handler, OnReceived));
        }

        protected override void CreateInstance()
        {
            if (_network.IsNodeIdBaseTypeLR)
            {
                message = new ApiMessage(CommandTypes.CmdZWaveGetPriorityRoute, (byte)(Destination.Id >> 8), (byte)Destination.Id);
            }
            else
            {
                message = new ApiMessage(CommandTypes.CmdZWaveGetPriorityRoute, (byte)Destination.Id);
            }

            handler = new ApiHandler(CommandTypes.CmdZWaveGetPriorityRoute);
            if (_network.IsNodeIdBaseTypeLR)
            {
                handler.AddConditions(new ByteIndex((byte)(Destination.Id >> 8)), new ByteIndex((byte)Destination.Id));
            }
            else
            {
                handler.AddConditions(new ByteIndex((byte)Destination.Id));
            }
        }

        private void OnReceived(DataReceivedUnit ou)
        {
            byte[] ret = ou.DataFrame.Payload;
            if (_network.IsNodeIdBaseTypeLR)
            {
                SpecificResult.NodeId = new NodeTag((ushort)((ret[0] << 8) + ret[1]));
                SpecificResult.RetVal = (ret[2] == 0x01) ? true : false;
                SpecificResult.PriorityRoute = new NodeTag[ret.Length - 4];
                for (int i = 0; i < SpecificResult.PriorityRoute.Length; i++)
                {
                    SpecificResult.PriorityRoute[i] = new NodeTag(ret[3 + i]);
                }
            }
            else
            {
                SpecificResult.NodeId = new NodeTag(ret[0]);
                SpecificResult.RetVal = (ret[1] == 0x01) ? true : false;
                SpecificResult.PriorityRoute = new NodeTag[ret.Length - 3];
                for (int i = 0; i < SpecificResult.PriorityRoute.Length; i++)
                {
                    SpecificResult.PriorityRoute[i] = new NodeTag(ret[2 + i]);
                }
            }
            SpecificResult.RouteSpeed = ret[ret.Length - 1];
            SetStateCompleted(ou);
        }

        public GetPriorityRouteResult SpecificResult
        {
            get { return (GetPriorityRouteResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new GetPriorityRouteResult();
        }
    }

    public class GetPriorityRouteResult : ActionResult
    {
        public NodeTag NodeId { get; set; }
        public bool RetVal { get; set; }
        public NodeTag[] PriorityRoute { get; set; }
        public byte RouteSpeed { get; set; }
    }


}
