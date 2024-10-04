/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;
using ZWave.Enums;
using Utils;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0x68 | nodeID | nodeType | funcID
    /// ZW->HOST: REQ | 0x68 | funcID | bStatus
    /// </summary>
    public class RequestNodeNeighborDiscoveryOperation : ApiOperation
    {
        //public enum nodeType
        //{
        //    nonListening,   //0x00 for non_listning_device
        //    listening,      //0x01 for listning_device
        //    flirs           //0x02 for flirs_device
        //}
        public static int TIMEOUT = 10000;

        private readonly NodeTag _node;
        private readonly byte _nodetype;
        internal int TimeoutMs { get; set; }

        /// <summary>
        /// If calling the API with nodeType 0x00, and 0x01, it will work as a normal neighbor update.
        /// If calling the API with nodeType 0x02, then the API will work as follow:
        /// The new neighbor discovery works as follows:
        /// When the API is called with the node-id of the listening node and the nodes to discovery are FLIRS nodes
        /// 1. Controller will send find_nodes_in_range for each FLIRS node in the network where the listening node is the only node in the bitmask
        /// 2. The Controller will create node-id bitmask for each FLIRS node that can reach the listening node
        /// 3. The controller will update the routing info for the listening node
        /// </summary>
        /// <param name="network">Network</param>
        /// <param name="node">Node Id</param>
        /// <param name="nodetype">Node type: 0- non listening, 1 - listening, 2 - FLiRS</param>
        /// <param name="timeoutMs"></param>
        public RequestNodeNeighborDiscoveryOperation(NetworkViewPoint network, NodeTag node, byte nodetype, int timeoutMs)
            : base(true, CommandTypes.CmdZWaveRequestNodeNeighborDiscovery, true)
        {
            _network = network;
            _node = node;
            _nodetype = nodetype;
            TimeoutMs = timeoutMs;
            if (TimeoutMs <= 0)
                TimeoutMs = TIMEOUT;
        }

        private ApiMessage messageStart;
        private ApiHandler handlerStarted;             // code=0x21
        private ApiHandler handlerDone;                // code=0x22
        private ApiHandler handlerFailed;              // code=0x23

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, TimeoutMs, messageStart));
            ActionUnits.Add(new DataReceivedUnit(handlerStarted, OnHandlerStarted));
            ActionUnits.Add(new DataReceivedUnit(handlerDone, OnHandlerDone));
            ActionUnits.Add(new DataReceivedUnit(handlerFailed, OnHandlerFailed));
        }

        protected override void CreateInstance()
        {
            var nid = new byte[] { (byte)_node.Id };
            if (_network.IsNodeIdBaseTypeLR)
            {
                nid = new byte[] { (byte)(_node.Id >> 8), (byte)_node.Id };
            }
            messageStart = new ApiMessage(SerialApiCommands[0], nid);
            messageStart.AddData(_nodetype);
            //var inpuParameters = new byte[] { (byte)(_node.Id >> 8), (byte)_node.Id, _nodetype };
            //messageStart = new ApiMessage(SerialApiCommands[0], inpuParameters);
            messageStart.SetSequenceNumber(SequenceNumber);

            handlerStarted = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            handlerStarted.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)RequestNeighborUpdateStatuses.Started));

            handlerDone = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            handlerDone.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)RequestNeighborUpdateStatuses.Done));

            handlerFailed = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            handlerFailed.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)RequestNeighborUpdateStatuses.Failed));
        }

        private void OnHandlerStarted(DataReceivedUnit ou)
        {
            SpecificResult.NeighborUpdateStatus = RequestNeighborUpdateStatuses.Started;
        }

        private void OnHandlerDone(DataReceivedUnit ou)
        {
            SpecificResult.NeighborUpdateStatus = RequestNeighborUpdateStatuses.Done;
            SetStateCompleted(ou);
        }

        private void OnHandlerFailed(DataReceivedUnit ou)
        {
            SpecificResult.NeighborUpdateStatus = RequestNeighborUpdateStatuses.Failed;
            SetStateCompleted(ou);
        }

        public RequestNodeNeighborUpdateResult SpecificResult
        {
            get { return (RequestNodeNeighborUpdateResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new RequestNodeNeighborUpdateResult();
        }
    }
}
