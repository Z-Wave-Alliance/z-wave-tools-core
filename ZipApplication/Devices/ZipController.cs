/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using ZWave.CommandClasses;
using ZWave.Devices;
using ZWave.Enums;
using ZWave.Layers;
using ZWave.ZipApplication.Operations;

namespace ZWave.ZipApplication.Devices
{
    public class ZipController : ZipDevice, IController
    {
        public ControllerRoles NetworkRole { get; set; }
        public bool Discovered { get { return DataSource != null && !string.IsNullOrEmpty(DataSource.SourceName); } }

        internal ZipController(ushort sessionId, ISessionClient sc, IFrameClient fc, ITransportClient tc)
            : base(sessionId, sc, fc, tc)
        {
        }

        public ActionToken RemoveFailedNodeId(byte[] headerExtension, NodeTag node, int timeoutMs, Action<IActionItem> completedCallback)
        {
            var cmd = new COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V3.FAILED_NODE_REMOVE();
            cmd.nodeId = (byte)node.Id;
            return RequestData(headerExtension, cmd, new COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V3.FAILED_NODE_REMOVE_STATUS(), timeoutMs, completedCallback);
        }

        public ActionToken ReplaceFailedNode(byte[] headerExtension, NodeTag node, int timeoutMs, Action<IActionItem> completedCallback)
        {
            var cmd = new COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V3.FAILED_NODE_REPLACE();
            cmd.nodeId = (byte)node.Id;
            cmd.mode = (byte)Modes.NodeAny;
            return RequestData(headerExtension, cmd, new COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V3.FAILED_NODE_REPLACE_STATUS(), timeoutMs, completedCallback);
        }

        public ActionToken RequestNetworkUpdate(int timeoutMs, Action<IActionItem> completedCallback)
        {
            ActionToken ret = null;
            var op = new RequestNetworkUpdateOperation(timeoutMs);
            ret = op.Token;
            ExecuteAsync(op, completedCallback);
            return ret;
        }

        public RequestNetworkUpdateResult RequestNetworkUpdate(int timeoutMs)
        {
            RequestNetworkUpdateResult ret = (RequestNetworkUpdateResult)Execute(new RequestNetworkUpdateOperation(timeoutMs));
            return ret;
        }

        public ActionToken GetCachedNodeInfo(NodeTag node, int timeoutMs, Action<IActionItem> completedCallback)
        {
            ActionToken ret = null;
            var op = new GetCachedNodeInfoOperation(Network, node, timeoutMs);
            ret = op.Token;
            ExecuteAsync(op, completedCallback);
            return ret;
        }

        public GetCachedNodeInfoResult GetCachedNodeInfo(NodeTag node, int timeoutMs)
        {
            GetCachedNodeInfoResult ret = (GetCachedNodeInfoResult)Execute(new GetCachedNodeInfoOperation(Network, node, timeoutMs));
            return ret;
        }

        public ActionToken ControllerChange(Modes mode, TransmitOptions txOptions, int timeoutMs, Action<IActionItem> completedCallback)
        {
            ActionToken ret = null;
            ControllerChangeOperation op = new ControllerChangeOperation(mode, txOptions, timeoutMs);
            ret = op.Token;
            ExecuteAsync(op, completedCallback);
            return ret;
        }

        public ControllerChangeResult ControllerChange(Modes mode, TransmitOptions txOptions, int timeoutMs)
        {
            ControllerChangeResult ret = null;
            ControllerChangeOperation op = new ControllerChangeOperation(mode, txOptions, timeoutMs);
            ret = (ControllerChangeResult)Execute(op);
            return ret;
        }

        public ActionToken AddNodeToNetwork(Modes mode, TransmitOptions txOptions, int timeoutMs, Action<IActionItem> completedCallback)
        {
            ActionToken ret = null;
            AddNodeOperation op = new AddNodeOperation(mode, txOptions, timeoutMs);
            ret = op.Token;
            ExecuteAsync(op, completedCallback);
            return ret;
        }

        public AddNodeResult AddNodeToNetwork(Modes mode, TransmitOptions txOptions, int timeoutMs)
        {
            AddNodeResult ret = null;
            AddNodeOperation op = new AddNodeOperation(mode, txOptions, timeoutMs);
            ret = (AddNodeResult)Execute(op);
            return ret;
        }

        public ActionToken RemoveNodeFromNetwork(Modes mode, int timeoutMs, Action<IActionItem> completedCallback)
        {
            ActionToken ret = null;
            RemoveNodeOperation op = new RemoveNodeOperation(mode, timeoutMs);
            ret = op.Token;
            ExecuteAsync(op, completedCallback);
            return ret;
        }

        public RemoveNodeResult RemoveNodeFromNetwork(Modes mode, int timeoutMs)
        {
            RemoveNodeResult ret = null;
            RemoveNodeOperation op = new RemoveNodeOperation(mode, timeoutMs);
            ret = (RemoveNodeResult)Execute(op);
            return ret;
        }

        public SetLearnModeResult SetLearnMode(LearnModes mode, int timeoutMs)
        {
            SetLearnModeResult ret = null;
            SetLearnModeOperation op = new SetLearnModeOperation(mode, timeoutMs);
            ret = (SetLearnModeResult)Execute(op);
            return ret;
        }

        public ActionToken SetLearnMode(LearnModes mode, int timeoutMs, Action<IActionItem> completedCallback)
        {
            ActionToken ret = null;
            SetLearnModeOperation op = new SetLearnModeOperation(mode, timeoutMs);
            ret = op.Token;
            ExecuteAsync(op, completedCallback);
            return ret;
        }

        public GetNodeListResult GetNodeList(int timeoutMs)
        {
            GetNodeListResult ret = null;
            GetNodeListOperation op = new GetNodeListOperation(timeoutMs);
            ret = (GetNodeListResult)Execute(op);
            IncludedNodes = ret.Nodes;
            return ret;
        }

        public bool UnsolicitedDestinationSet(System.Net.IPAddress address, ushort portNo)
        {
            if (address != null)
            {
                if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    address = address.MapToIPv6();
                return SendData(null, new COMMAND_CLASS_ZIP_GATEWAY.UNSOLICITED_DESTINATION_SET
                {
                    unsolicitedIpv6Destination = address.GetAddressBytes(),
                    unsolicitedDestinationPort = Utils.Tools.GetBytes(portNo)
                });
            }
            return false;
        }
    }
}
