/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using ZWave.Enums;
using ZWave.Layers;
using ZWave.BasicApplication.Enums;
using ZWave.BasicApplication.Operations;
using ZWave.Devices;
using System.Linq;

namespace ZWave.BasicApplication.Devices
{
    public class BridgeController : Controller
    {
        internal BridgeController(ushort sessionId, ISessionClient sc, IFrameClient fc, ITransportClient tc, bool preInitNodes = true)
            : base(sessionId, sc, fc, tc, preInitNodes)
        { }

        public GetVirtualNodesResult GetVirtualNodes()
        {
            return (GetVirtualNodesResult)Execute(new GetVirtualNodesOperation());
        }

        public IsVirtualNodeResult IsVirtualNode(NodeTag node)
        {
            return (IsVirtualNodeResult)Execute(new IsVirtualNodeOperation(Network, node));
        }

        public SendDataResult SendDataBridge(NodeTag srcNode, NodeTag destNode, byte[] data, TransmitOptions transmitOptions)
        {
            return (SendDataResult)Execute(new SendDataBridgeOperation(Network, srcNode, destNode, data, transmitOptions));
        }

        public ActionToken SendDataBridge(NodeTag srcNode, NodeTag destNode, byte[] data, TransmitOptions transmitOptions, Action<IActionItem> completedCallback)
        {
            SendDataBridgeOperation operation = new SendDataBridgeOperation(Network, srcNode, destNode, data, transmitOptions);
            return ExecuteAsync(operation, completedCallback);
        }

        public TransmitResult SendDataMetaBridge(NodeTag srcNode, NodeTag destNode, byte[] data, TransmitOptions transmitOptions)
        {
            return (TransmitResult)Execute(new SendDataMetaBridgeOperation(Network, srcNode, destNode, data, transmitOptions));
        }

        public TransmitResult SendDataMultiBridge(NodeTag srcNode, NodeTag[] nodes, byte[] data, TransmitOptions transmitOptions)
        {
            return (TransmitResult)Execute(new SendDataMultiBridgeOperation(Network, srcNode, nodes, data, transmitOptions));
        }

        public TransmitResult SendDataMultiBridge(NodeTag srcNode, byte[] receiverNodeIds, byte[] data, TransmitOptions transmitOptions)
        {
            var nodes = new NodeTag[0];
            if (receiverNodeIds != null && receiverNodeIds.Length > 0)
            {
                nodes = receiverNodeIds.Select(x => new NodeTag(x)).ToArray();
            }
            return (TransmitResult)Execute(new SendDataMultiBridgeOperation(Network, srcNode, nodes, data, transmitOptions));
        }

        public TransmitResult SendDataMultiBridge(NodeTag srcNode, NodeTag[] nodes, byte[] data, TransmitOptions txOptions, out ActionToken token)
        {
            return SendDataMultiBridge(srcNode, nodes, data, txOptions, new SubstituteSettings(), out token);
        }

        public TransmitResult SendDataMultiBridge(NodeTag srcNode, NodeTag[] nodes, byte[] data, TransmitOptions txOptions, SubstituteSettings substituteSettings, out ActionToken token)
        {
            token = SendDataMultiBridge(srcNode, nodes, data, txOptions, substituteSettings, null);
            return (TransmitResult)WaitCompletedSignal(token);
        }

        public ActionToken SendDataMultiBridge(NodeTag srcNode, NodeTag[] nodes, byte[] data, TransmitOptions transmitOptions, Action<IActionItem> completedCallback)
        {
            return SendDataMultiBridge(srcNode, nodes, data, transmitOptions, new SubstituteSettings(), completedCallback);
        }

        public ActionToken SendDataMultiBridge(NodeTag srcNode, NodeTag[] nodes, byte[] data, TransmitOptions transmitOptions, SubstituteSettings substituteSettings, Action<IActionItem> completedCallback)
        {
            SendDataMultiBridgeOperation operation = new SendDataMultiBridgeOperation(Network, srcNode, nodes, data, transmitOptions)
                {
                    SubstituteSettings = substituteSettings
                };
            return ExecuteAsync(operation, completedCallback);
        }

        public TransmitResult SendVirtualDeviceNodeInformation(NodeTag srcNode, NodeTag destNode, TransmitOptions transmitOptions)
        {
            return (TransmitResult)Execute(new SendVirtualDeviceNodeInformationOperation(srcNode, destNode, transmitOptions));
        }

        public SetLearnModeResult SetVirtualDeviceLearnMode(NodeTag node, VirtualDeviceLearnModes mode, int timeoutMs)
        {
            return (SetLearnModeResult)Execute(new SetVirtualDeviceLearnModeOperation(Network, node, mode, SetAssignStatusSignal, timeoutMs));
        }

        public ActionToken SetVirtualDeviceLearnMode(NodeTag node, VirtualDeviceLearnModes mode, int timeoutMs, Action<IActionItem> completedCallback)
        {
            SetVirtualDeviceLearnModeOperation oper = new SetVirtualDeviceLearnModeOperation(Network, node, mode, SetAssignStatusSignal, timeoutMs);
            return ExecuteAsync(oper, completedCallback);
        }

        public ActionResult ApplicationVirtualDeviceNodeInformation(bool isListening, byte generic, byte specific, byte[] cmdClasses)
        {
            return ApplicationVirtualDeviceNodeInformation(NodeTag.Empty, isListening, generic, specific, cmdClasses);
        }

        public ActionResult ApplicationVirtualDeviceNodeInformation(NodeTag endDeviceNode, bool isListening, byte generic, byte specific, byte[] cmdClasses)
        {
            return Execute(new ApplicationVirtualDeviceNodeInformationOperation(Network, endDeviceNode, isListening, generic, specific, cmdClasses));
        }
    }
}
