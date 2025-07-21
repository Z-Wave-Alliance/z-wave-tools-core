/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.BasicApplication.Enums;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0x4F | bSrcNodeID | bDstNodeID | PriorityRoute | funcID
    /// ZW->HOST: RES | 0x4F | retVal
    /// ZW->HOST: REQ | 0x4F | funcID | bStatus
    /// </summary>
    public class AssignPriorityReturnRouteOperation : CallbackApiOperation
    {
        public NodeTag[] PriorityRoute { get; set; }
        public NodeTag Source { get; set; }
        public NodeTag Destination { get; set; }
        public byte RouteSpeed { get; set; }
        public AssignPriorityReturnRouteOperation(NetworkViewPoint network, NodeTag source, NodeTag destination,
            NodeTag priorityRoute0, NodeTag priorityRoute1, NodeTag priorityRoute2, NodeTag priorityRoute3, byte routespeed)
            : base(CommandTypes.CmdZWaveAssignPriorityReturnRoute)
        {
            _network = network;
            PriorityRoute = new[] { priorityRoute0, priorityRoute1, priorityRoute2, priorityRoute3 };
            Source = source;
            Destination = destination;
            RouteSpeed = routespeed;
        }

        protected override byte[] CreateInputParameters()
        {
            if (_network.IsNodeIdBaseTypeLR)
            {
                byte[] ret = new byte[PriorityRoute.Length + 5];
                ret[0] = (byte)(Source.Id >> 8);
                ret[1] = (byte)Source.Id;
                ret[2] = (byte)(Destination.Id >> 8);
                ret[3] = (byte)Destination.Id;
                for (int i = 0; i < PriorityRoute.Length; i++)
                {
                    ret[i + 4] = (byte)PriorityRoute[i].Id;
                }
                ret[ret.Length - 1] = RouteSpeed;
                return ret;
            }
            else
            {
                byte[] ret = new byte[PriorityRoute.Length + 3];
                ret[0] = (byte)Source.Id;
                ret[1] = (byte)Destination.Id;
                for (int i = 0; i < PriorityRoute.Length; i++)
                {
                    ret[i + 2] = (byte)PriorityRoute[i].Id;
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

        public AssignReturnRouteResult SpecificResult
        {
            get { return (AssignReturnRouteResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new AssignReturnRouteResult();
        }
    }

}

/*
ApiMessage message;
ApiHandler handler;

protected override void CreateWorkflow()
{
    ActionUnits.Add(new DataReceivedUnit(null, null, message, 1000));
    ActionUnits.Add(new DataReceivedUnit(handler, OnReceived));
}

protected override void CreateInstance()
{
    message = new ApiMessage(CommandTypes.CmdZWaveAssignPriorityReturnRoute, Source, Destination);
    message.AddData(LastWorkingRoute);
    message.AddData(RouteSpeed);


    handler = new ApiHandler(CommandTypes.CmdZWaveAssignPriorityReturnRoute);
    handler.AddConditions(new ByteIndex(Destination));
    handler.AddConditions(new ByteIndex(Source));
}

private void OnReceived(OperationUnit ou)
{
    byte[] ret = ou.CommandHandler.DataFrame.Payload;
    SpecificResult.Repeaters = new byte[ret.Length - 1];
    Array.Copy(ret, 1, SpecificResult.Repeaters, 0, SpecificResult.Repeaters.Length);
    SetStateCompleted(ou);
}

public AssignPriorityReturnRouteResult SpecificResult
{
    get { return (AssignPriorityReturnRouteResult)Result; }
}

protected override ActionResult CreateOperationResult()
{
    return new AssignPriorityReturnRouteResult();
}
}

public class AssignPriorityReturnRouteResult : ActionResult
{
public byte[] Repeaters { get; set; }
}

*/
