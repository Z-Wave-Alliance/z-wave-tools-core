/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Linq;
using ZWave.BasicApplication.Enums;
using ZWave.Enums;
using Utils;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0x60 | NodeID
    /// ZW->HOST: RES | 0x60 | retVal
    /// 
    /// ZW->HOST: REQ | 0x49 | bStatus | bNodeID | bLen | basic | generic | specific | commandclasses[ ]
    /// </summary>
    public class RequestNodeInfoOperation : ApiOperation
    {
        internal NodeTag Node { get; set; }

        readonly int _timeoutMs;

        public RequestNodeInfoOperation(NetworkViewPoint network, NodeTag node)
            : this(network, node, DefaultTimeouts.REQUEST_NODE_INFO_TIMEOUT)
        {
        }

        public RequestNodeInfoOperation(NetworkViewPoint network, NodeTag node, int timeoutMs)
            : base(true, CommandTypes.CmdZWaveRequestNodeInfo, false)
        {
            _network = network;
            Node = node;
            _timeoutMs = timeoutMs;
        }

        private ApiMessage messageRequest;
        private ApiHandler handlerResponseTrue;
        private ApiHandler handlerResponseFalse;

        private ApiHandler handlerNodeInfoReceived;
        private ApiHandler handlerNodeInfoReqDone;
        private ApiHandler handlerNodeInfoReqFailed;

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, _timeoutMs, messageRequest));
            //ActionUnits.Add(new DataReceivedUnit(handlerResponseTrue, null));
            ActionUnits.Add(new DataReceivedUnit(handlerResponseFalse, SetStateFailed));
            //ActionUnits.Add(new DataReceivedUnit(handlerNodeInfoReqDone, null));
            ActionUnits.Add(new DataReceivedUnit(handlerNodeInfoReqFailed, SetStateFailed));
            ActionUnits.Add(new DataReceivedUnit(handlerNodeInfoReceived, SetStateCompleted));
        }

        protected override void CreateInstance()
        {
            messageRequest = new ApiMessage(CommandTypes.CmdZWaveRequestNodeInfo, (byte)Node.Id);
            if (_network.IsNodeIdBaseTypeLR)
            {
                messageRequest = new ApiMessage(CommandTypes.CmdZWaveRequestNodeInfo, (byte)(Node.Id >> 8), (byte)Node.Id);
            }

            handlerResponseTrue = new ApiHandler(CommandTypes.CmdZWaveRequestNodeInfo);
            handlerResponseTrue.AddConditions(new ByteIndex(0x01));

            handlerResponseFalse = new ApiHandler(CommandTypes.CmdZWaveRequestNodeInfo);
            handlerResponseFalse.AddConditions(new ByteIndex(0x00));

            handlerNodeInfoReceived = new ApiHandler(FrameTypes.Request, CommandTypes.CmdApplicationControllerUpdate);
            handlerNodeInfoReceived.AddConditions(new ByteIndex((byte)ControllerUpdateStatuses.NodeInfoReceived), new ByteIndex((byte)Node.Id));

            handlerNodeInfoReqDone = new ApiHandler(FrameTypes.Request, CommandTypes.CmdApplicationControllerUpdate);
            handlerNodeInfoReqDone.AddConditions(new ByteIndex((byte)ControllerUpdateStatuses.NodeInfoReqDone), new ByteIndex((byte)Node.Id));

            handlerNodeInfoReqFailed = new ApiHandler(FrameTypes.Request, CommandTypes.CmdApplicationControllerUpdate);
            handlerNodeInfoReqFailed.AddConditions(new ByteIndex((byte)ControllerUpdateStatuses.NodeInfoReqFailed));

            if (_network.IsNodeIdBaseTypeLR)
            {
                handlerNodeInfoReceived = new ApiHandler(FrameTypes.Request, CommandTypes.CmdApplicationControllerUpdate);
                handlerNodeInfoReceived.AddConditions(new ByteIndex((byte)ControllerUpdateStatuses.NodeInfoReceived), new ByteIndex((byte)(Node.Id >> 8)), new ByteIndex((byte)Node.Id));

                handlerNodeInfoReqDone = new ApiHandler(FrameTypes.Request, CommandTypes.CmdApplicationControllerUpdate);
                handlerNodeInfoReqDone.AddConditions(new ByteIndex((byte)ControllerUpdateStatuses.NodeInfoReqDone), new ByteIndex((byte)(Node.Id >> 8)), new ByteIndex((byte)Node.Id));

                // TODO WA:
                //handlerNodeInfoReceived = new ApiHandler(FrameTypes.Request, CommandTypes.CmdApplicationControllerUpdate);
                //handlerNodeInfoReceived.AddConditions(new ByteIndex((byte)ControllerUpdateStatuses.NodeInfoReceived), new ByteIndex(0), new ByteIndex((byte)Node.Id));

                //handlerNodeInfoReqDone = new ApiHandler(FrameTypes.Request, CommandTypes.CmdApplicationControllerUpdate);
                //handlerNodeInfoReqDone.AddConditions(new ByteIndex((byte)ControllerUpdateStatuses.NodeInfoReqDone), new ByteIndex(0), new ByteIndex((byte)Node.Id));

            }
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            byte[] res = ((DataReceivedUnit)ou).DataFrame.Payload;
            if (_network.IsNodeIdBaseTypeLR ? res[3] > 0 : res[2] > 0)
            {
                try
                {
                    SpecificResult.Node = Node;
                    if (_network.IsNodeIdBaseTypeLR)
                    {
                        SpecificResult.NodeInfo = new byte[res[3]];
                        Array.Copy(res, 4, SpecificResult.NodeInfo, 0, SpecificResult.NodeInfo.Length);
                    }
                    else
                    {
                        SpecificResult.NodeInfo = new byte[res[2]];
                        Array.Copy(res, 3, SpecificResult.NodeInfo, 0, SpecificResult.NodeInfo.Length);
                    }
                    if (SpecificResult.NodeInfo.Length > 0)
                    {
                        SpecificResult.Basic = SpecificResult.NodeInfo[0];
                    }
                    if (SpecificResult.NodeInfo.Length > 1)
                    {
                        SpecificResult.Generic = SpecificResult.NodeInfo[1];
                    }
                    if (SpecificResult.NodeInfo.Length > 2)
                    {
                        SpecificResult.Specific = SpecificResult.NodeInfo[2];
                    }
                    if (SpecificResult.NodeInfo.Length > 3)
                    {
                        SpecificResult.CommandClasses = SpecificResult.NodeInfo.Skip(3).TakeWhile(x => x != 0xEF).ToArray();
                    }
                }
                catch (Exception)
                { }
            }
            base.SetStateCompleted(ou);
        }

        public RequestNodeInfoResult SpecificResult
        {
            get { return (RequestNodeInfoResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new RequestNodeInfoResult();
        }
    }

    public class RequestNodeInfoResult : ActionResult
    {
        public NodeTag Node { get; set; }
        public byte[] NodeInfo { get; set; }
        public byte Basic { get; set; }
        public byte Generic { get; set; }
        public byte Specific { get; set; }
        public byte[] CommandClasses { get; set; }
        public byte[] SecureCommandClasses { get; set; }
        public SecuritySchemes[] SecuritySchemes { get; set; }

        internal void CopyTo(RequestNodeInfoResult res)
        {
            res.Node = Node;
            res.NodeInfo = NodeInfo;
            res.Basic = Basic;
            res.Generic = Generic;
            res.Specific = Specific;
            res.CommandClasses = CommandClasses;
            res.SecureCommandClasses = SecureCommandClasses;
            res.SecuritySchemes = SecuritySchemes;
        }
    }
}
