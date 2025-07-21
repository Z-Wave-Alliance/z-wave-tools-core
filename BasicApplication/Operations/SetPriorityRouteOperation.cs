/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using ZWave.BasicApplication.Enums;
using Utils;
using ZWave.Devices;
using System.Linq;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0x93 | bnodeID | repeater0| repeater1| repeater2| repeater3| routerspeed
    /// ZW->HOST: RES | 0x93 | bnodeID | retVal
    /// </summary>
    public class SetPriorityRouteOperation : ApiOperation
    {
        public NodeTag[] PriorityRoute { get; set; }
        public NodeTag Destination { get; set; }
        public byte RouteSpeed { get; set; }
        public SetPriorityRouteOperation(NetworkViewPoint network, NodeTag destination, NodeTag repeater0, NodeTag repeater1, NodeTag repeater2, NodeTag repeater3, byte routespeed)
            : base(true, CommandTypes.CmdZWaveSetPriorityRoute, false)
        {
            _network = network;
            PriorityRoute = new[] { repeater0, repeater1, repeater2, repeater3 };
            Destination = destination;
            RouteSpeed = routespeed;
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
                message = new ApiMessage(CommandTypes.CmdZWaveSetPriorityRoute, (byte)(Destination.Id >> 8), (byte)Destination.Id);
            }
            else
            {
                message = new ApiMessage(CommandTypes.CmdZWaveSetPriorityRoute, (byte)Destination.Id);
            }
            message.AddData(PriorityRoute.Select(x => (byte)x.Id).ToArray());
            message.AddData(RouteSpeed);


            handler = new ApiHandler(CommandTypes.CmdZWaveSetPriorityRoute);
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
            SpecificResult.PriorityRoute = new byte[ret.Length - 1];
            Array.Copy(ret, 1, SpecificResult.PriorityRoute, 0, SpecificResult.PriorityRoute.Length);
            SetStateCompleted(ou);
        }

        public SetPriorityRouteResult SpecificResult
        {
            get { return (SetPriorityRouteResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new SetPriorityRouteResult();
        }
    }

    public class SetPriorityRouteResult : ActionResult
    {
        public byte[] PriorityRoute { get; set; }
    }


}
