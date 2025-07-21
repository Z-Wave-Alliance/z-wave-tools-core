/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;
using ZWave.BasicApplication.Enums;
using ZWave.Devices;
using ZWave.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class SerialApiGetLRNodesOperation : ApiOperation
    {
        internal int TimeoutMs { get; set; }
        public SerialApiGetLRNodesOperation(NetworkViewPoint network)
           : base(true, CommandTypes.CmdSerialApiGetLRNodes, false)
        {
            _network = network;
            TimeoutMs = 200;
        }

        protected ApiMessage message;
        private ApiHandler handler;

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, TimeoutMs, message));
            ActionUnits.Add(new DataReceivedUnit(handler, OnReceived));
        }

        byte offset = 0;
        protected override void CreateInstance()
        {

            message = new ApiMessage(SerialApiCommands[0], offset);
            handler = new ApiHandler(FrameTypes.Response, SerialApiCommands[0]);
        }

        protected void OnReceived(DataReceivedUnit ou)
        {
            if (ou.DataFrame?.Payload != null && ou.DataFrame.Payload.Length > 1)
            {
                ParseResponse(ou.DataFrame.Payload);
                if (ou.DataFrame.Payload[0] > 0 && offset < 4)
                {
                    message = new ApiMessage(SerialApiCommands[0], ++offset);
                    ou.SetNextActionItems(message);
                }
                else
                {
                    SetStateCompleted(ou);
                }
            }
            else
            {
                SetStateFailed(ou);
            }
        }

        List<NodeTag> includedNodes = new List<NodeTag>();
        private void ParseResponse(byte[] payload)
        {
            ushort nodeIdx = (ushort)(128 * offset + 0xFF);
            for (int i = 0; i < payload[2]; i++)
            {
                byte availabilityMask = payload[3 + i];
                for (byte bit = 0; bit < 8; bit++)
                {
                    nodeIdx++;
                    if ((availabilityMask & (1 << bit)) > 0)
                    {
                        includedNodes.Add(new NodeTag(nodeIdx));
                    }
                }
            }
            SpecificResult.IncludedNodes = includedNodes.ToArray();
        }

        public SerialApiGetLRNodesResult SpecificResult
        {
            get { return (SerialApiGetLRNodesResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new SerialApiGetLRNodesResult();
        }
    }

    public class SerialApiGetLRNodesResult : ActionResult
    {
        public NodeTag[] IncludedNodes { get; set; }
    }
}
