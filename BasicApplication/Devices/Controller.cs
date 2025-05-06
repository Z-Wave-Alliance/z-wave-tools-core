/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com

//#define MORE_DEBUG_LOGS

using System;
using System.Threading;
using ZWave.BasicApplication.Enums;
using ZWave.BasicApplication.Operations;
using ZWave.BasicApplication.Tasks;
using ZWave.Devices;
using ZWave.Enums;
using ZWave.Layers;

namespace ZWave.BasicApplication.Devices
{
    public class Controller : Device, IController
    {
#if MORE_DEBUG_LOGS
        private void PrintTimeStampAndName(string method)
        {
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + " Controller: " +  method);
        }
#endif

        private byte mControllerCapability;
        public byte ControllerCapability
        {
            get { return mControllerCapability; }
            set
            {
                mControllerCapability = value;
                Notify("ControllerCapability");
            }
        }

        public ControllerRoles NetworkRole
        {
            get
            {
                ControllerRoles result = ControllerRoles.None;
                if ((ControllerCapability & (byte)ControllerCapabilities.IS_SECONDARY) != 0)
                {
                    result |= ControllerRoles.Secondary;
                }
                if ((ControllerCapability & (byte)ControllerCapabilities.IS_SUC) != 0)
                {
                    result |= ControllerRoles.SUC;
                }
                if ((ControllerCapability & (byte)ControllerCapabilities.ON_OTHER_NETWORK) != 0)
                {
                    result |= ControllerRoles.OtherNetwork;
                }
                if ((ControllerCapability & (byte)ControllerCapabilities.IS_REAL_PRIMARY) != 0)
                {
                    result |= ControllerRoles.RealPrimary;
                }
                if ((ControllerCapability & (byte)ControllerCapabilities.NODEID_SERVER_PRESENT) != 0)
                {
                    result |= ControllerRoles.NodeIdServerPresent;
                }
                if ((result & ControllerRoles.SUC) != 0 && (result & ControllerRoles.NodeIdServerPresent) != 0)
                {
                    result |= ControllerRoles.SIS;
                }
                if ((result & ControllerRoles.SIS) == 0 && (result & ControllerRoles.SUC) == 0 && (result & ControllerRoles.NodeIdServerPresent) != 0)
                {
                    result |= ControllerRoles.Inclusion;
                }
                return result;
            }
        }

        internal Controller(ushort sessionId, ISessionClient sc, IFrameClient fc, ITransportClient tc, bool preInitNodes = true)
            : base(sessionId, sc, fc, tc, preInitNodes)
        {
        }

        public NodeTag[] IncludedNodes { get; set; }

        public override ActionToken ExecuteAsync(IActionItem actionItem, Action<IActionItem> completedCallback)
        {
            var actionBase = actionItem as ActionBase;
            if (actionBase != null)
            {
                if (actionBase is GetControllerCapabilitiesOperation)
                {
                    actionBase.CompletedCallback = (x) =>
                    {
                        var action = x as ActionBase;
                        if (action != null)
                        {
                            GetControllerCapabilitiesResult res = (GetControllerCapabilitiesResult)action.Result;
                            if (res)
                                ControllerCapability = res.ControllerCapability;
                            completedCallback?.Invoke(action);
                        }
                    };
                }
                else if (actionBase is GetSucNodeIdOperation)
                {
                    actionBase.CompletedCallback = (x) =>
                    {
                        var action = x as ActionBase;
                        if (action != null)
                        {
                            GetSucNodeIdResult res = (GetSucNodeIdResult)action.Result;
                            if (res)
                                SucNodeId = res.SucNode.Id;
                            completedCallback?.Invoke(action);
                        }
                    };
                }
                else
                    actionBase.CompletedCallback = completedCallback;
            }
            return base.ExecuteAsync(actionBase, actionBase.CompletedCallback);
        }

        public override ActionResult Execute(IActionItem action)
        {
            ActionResult ret = base.Execute(action);
            if (action is GetControllerCapabilitiesOperation)
            {
                GetControllerCapabilitiesResult res = (GetControllerCapabilitiesResult)ret;
                if (res)
                {
                    ControllerCapability = res.ControllerCapability;
                }
            }
            else if (action is GetSucNodeIdOperation)
            {
                GetSucNodeIdResult res = (GetSucNodeIdResult)ret;
                if (res)
                {
                    SucNodeId = res.SucNode.Id;
                }
            }
            return ret;
        }

        private ActionToken _smartStartSupportToken;
        public void StartSmartListener(Func<byte, byte[], NodeProvision?> dskNeededCallback, Action<bool, byte[], ActionResult> busyCallback, int delayBeforeStartMs, int inclusionTimeoutMs)
        {
            if (SerialApiVersion >= 8 &&
                (NetworkRole.HasFlag(ControllerRoles.SIS) ||
                (NetworkRole.HasFlag(ControllerRoles.RealPrimary) && Network.SucNodeId == 0)))
            {
                SetSmartStartMode(true, delayBeforeStartMs);
                if (_smartStartSupportToken != null)
                {
                    Cancel(_smartStartSupportToken);
                    WaitCompletedSignal(_smartStartSupportToken);
                }
                _smartStartSupportToken = SmartStartSupport(dskNeededCallback, busyCallback, delayBeforeStartMs, inclusionTimeoutMs);
            }
        }

        public void StopSmartListener(int delay = 500)
        {
            if (SerialApiVersion >= 8)
            {
                SetSmartStartMode(false, 0);
                if (_smartStartSupportToken != null)
                {
                    Cancel(_smartStartSupportToken);
                    WaitCompletedSignal(_smartStartSupportToken);
                    _smartStartSupportToken = null;
                    Thread.Sleep(delay); // Wait a little for device change it state (Otherwise it can corrupts Transfer Presentation frame).
                }
            }
        }

        #region GetProtocolInfo

        public GetNodeProtocolInfoResult GetProtocolInfo(NodeTag node)
        {
            return (GetNodeProtocolInfoResult)Execute(new GetNodeProtocolInfoOperation(_network, node));
        }

        public ActionToken GetProtocolInfo(NodeTag node, Action<IActionItem> completedCallback)
        {
            return ExecuteAsync(new GetNodeProtocolInfoOperation(_network, node), completedCallback);
        }

        #endregion

        #region AddNodeToNetwork

        public AddRemoveNodeResult AddNodeToNetwork(Modes mode, int timeoutMs)
        {
            ResetNodeStatusSignals();
            return (AddRemoveNodeResult)Execute(new AddNodeOperation(Network, mode, SetNodeStatusSignal, timeoutMs));
        }

        public ActionToken AddNodeToNetworkNonSecure(Modes mode, int timeoutMs, Action<IActionItem> completedCallback)
        {
            ResetNodeStatusSignals();
            return ExecuteAsync(new AddNodeOperation(Network, mode, SetNodeStatusSignal, timeoutMs)
            {
                SubstituteSettings = new SubstituteSettings(SubstituteFlags.DenySecurity, 0)
            }, completedCallback);
        }

        public ActionToken AddNodeToNetwork(Modes mode, int timeoutMs, Action<IActionItem> completedCallback)
        {
            ResetNodeStatusSignals();
            return ExecuteAsync(new AddNodeOperation(Network, mode, SetNodeStatusSignal, timeoutMs), completedCallback);
        }

        public ActionToken AddNodeToNetwork(Modes mode, bool isModeStopEnabled, int timeoutMs, Action<IActionItem> completedCallback)
        {
            ResetNodeStatusSignals();
            return ExecuteAsync(new AddNodeOperation(Network, mode, SetNodeStatusSignal, isModeStopEnabled, timeoutMs), completedCallback);
        }

        #endregion

        #region IncludeNode

        public InclusionResult IncludeNode(Modes mode, int timeoutMs)
        {
            ResetNodeStatusSignals();
            AddNodeOperation AddNodeOperation = new AddNodeOperation(Network, mode, SetNodeStatusSignal, timeoutMs);
            return (InclusionResult)Execute(new InclusionTask(Network, AddNodeOperation));
        }

        public ActionToken IncludeNode(Modes mode, int timeoutMs, Action<IActionItem> completedCallback, SetupNodeLifelineSettings setupNodeLifelineSettings = SetupNodeLifelineSettings.Default)
        {
            ResetNodeStatusSignals();
            AddNodeOperation addNodeOperation = new AddNodeOperation(Network, mode, SetNodeStatusSignal, timeoutMs);

            addNodeOperation.IsLegacySisAssign = setupNodeLifelineSettings != SetupNodeLifelineSettings.Default ?
                setupNodeLifelineSettings.HasFlag(SetupNodeLifelineSettings.IsSetAsSisAutomatically)
                && !setupNodeLifelineSettings.HasFlag(SetupNodeLifelineSettings.IsBasedOnZwpRoleType) : false;

            return ExecuteAsync(new InclusionTask(Network, addNodeOperation, setupNodeLifelineSettings), completedCallback);
        }

        #endregion

        #region RemoveNodeFromNetwork

        public AddRemoveNodeResult RemoveNodeFromNetwork(Modes mode, int timeoutMs)
        {
            ResetNodeStatusSignals();
            return (AddRemoveNodeResult)Execute(new RemoveNodeOperation(Network, mode, SetNodeStatusSignal, timeoutMs));
        }

        public ActionToken RemoveNodeFromNetwork(Modes mode, int timeoutMs, Action<IActionItem> completedCallback)
        {
            return ExecuteAsync(new RemoveNodeOperation(Network, mode, SetNodeStatusSignal, timeoutMs), completedCallback);
        }

        #endregion

        #region ExcludeNode

        public ExclusionResult ExcludeNode(Modes mode, int timeoutMs)
        {
            ResetNodeStatusSignals();
            return (ExclusionResult)Execute(new ExclusionTask(Network, mode, SetNodeStatusSignal, timeoutMs));
        }

        public ActionToken ExcludeNode(Modes mode, int timeoutMs, Action<IActionItem> completedCallback)
        {
            ResetNodeStatusSignals();
            return ExecuteAsync(new ExclusionTask(Network, mode, SetNodeStatusSignal, timeoutMs), completedCallback);
        }

        #endregion

        #region RemoveNodeIdFromNetwork

        public AddRemoveNodeResult RemoveNodeIdFromNetwork(Modes mode, NodeTag node, int timeoutMs)
        {
            return (AddRemoveNodeResult)Execute(new RemoveNodeIdOperation(Network, mode, node, SetNodeStatusSignal, timeoutMs));
        }

        public ActionToken RemoveNodeIdFromNetwork(Modes mode, NodeTag node, int timeoutMs, Action<IActionItem> completedCallback)
        {
            return ExecuteAsync(new RemoveNodeIdOperation(Network, mode, node, SetNodeStatusSignal, timeoutMs), completedCallback);
        }

        #endregion

        #region SetLearnMode

        public override SetLearnModeResult SetLearnMode(LearnModes mode, bool isSubstituteDenied, int timeoutMs)
        {
            SetLearnModeResult ret = null;
            ResetNodeStatusSignals();
            var action = new SetLearnModeControllerOperation(Network, mode, SetNodeStatusSignal, timeoutMs);
            if (isSubstituteDenied)
            {
                action.SubstituteSettings.SetFlag(SubstituteFlags.DenySecurity);
            }
            ret = (SetLearnModeResult)Execute(action);
            return ret;
        }

        public override SetLearnModeResult SetLearnMode(LearnModes mode, int timeoutMs)
        {
            return SetLearnMode(mode, false, timeoutMs);
        }

        public override ActionToken SetLearnMode(LearnModes mode, bool isSubstituteDenied, int timeoutMs, Action<IActionItem> completedCallback)
        {
#if MORE_DEBUG_LOGS
            PrintTimeStampAndName("SetLearnMode");
#endif
            ActionToken ret = null;
            ResetNodeStatusSignals();
            SetLearnModeControllerOperation action = new SetLearnModeControllerOperation(Network, mode, SetNodeStatusSignal, timeoutMs);
            if (isSubstituteDenied)
            {
                action.SubstituteSettings.SetFlag(SubstituteFlags.DenySecurity);
            }
            learnModeOperation = action;
            ret = ExecuteAsync(action, completedCallback);
            return ret;
        }

        public override ActionToken SetLearnMode(LearnModes mode, int timeoutMs, Action<IActionItem> completedCallback)
        {
            return SetLearnMode(mode, false, timeoutMs, completedCallback);
        }

        #endregion

        public ActionResult EnableSUC(bool isEnable, byte capabilities)
        {
            return Execute(new EnableSucOperation(Convert.ToByte(isEnable), capabilities));
        }

        public SetSucNodeIdResult SetSucNodeID(NodeTag node, bool isEnable, bool isLowPower, byte capabilities)
        {
            if (node.Id != Id)
            {
                var res = (SetSucNodeIdResult)Execute(new SetSucNodeIdOperation(
                    Network,
                    node,
                    Convert.ToByte(isEnable),
                    isLowPower,
                    capabilities));
                if (res)
                {
                    SucNodeId = node.Id;
                }
                return res;
            }
            else
            {
                var ret = (ReturnValueResult)Execute(new SetSucSelfOperation(
                    Network,
                    node,
                    Convert.ToByte(isEnable),
                    isLowPower,
                    capabilities));
                if (ret)
                {
                    SucNodeId = node.Id;
                }
                return new SetSucNodeIdResult(ret)
                {
                    RetVal = SetSucReturnValues.SucSetSucceeded
                };
            }
        }

        public ReturnValueResult AreNodesNeighbours(NodeTag node1, NodeTag node2)
        {
            return (ReturnValueResult)Execute(new AreNodesNeighboursOperation(node1, node2));
        }

        public AssignReturnRouteResult AssignReturnRoute(NodeTag node1, NodeTag node2, out ActionToken token)
        {
            token = ExecuteAsync(new AssignReturnRouteOperation(Network, node1, node2), null);
            return (AssignReturnRouteResult)WaitCompletedSignal(token);
        }

        public AssignReturnRouteResult AssignPriorityReturnRoute(NodeTag source, NodeTag destination,
            NodeTag priorityRoute0, NodeTag priorityRoute1, NodeTag priorityRoute2, NodeTag priorityRoute3, byte routespeed, out ActionToken token)
        {
            token = ExecuteAsync(new AssignPriorityReturnRouteOperation(Network, source, destination,
                priorityRoute0, priorityRoute1, priorityRoute2, priorityRoute3, routespeed), null);
            return (AssignReturnRouteResult)WaitCompletedSignal(token);
        }

        public ActionResult AssignSucReturnRoute(NodeTag node, out ActionToken token)
        {
            token = ExecuteAsync(new AssignSucReturnRouteOperation(Network, node), null);
            return WaitCompletedSignal(token);
        }

        public ActionResult AssignPrioritySucReturnRoute(NodeTag source, NodeTag repeater0, NodeTag repeater1, NodeTag repeater2, NodeTag repeater3, byte routespeed, out ActionToken token)
        {
            token = ExecuteAsync(new AssignPrioritySucReturnRouteOperation(Network, source, repeater0, repeater1, repeater2, repeater3, routespeed), null);
            return WaitCompletedSignal(token);
        }

        public ActionResult DeleteReturnRoute(NodeTag node, out ActionToken token)
        {
            token = ExecuteAsync(new DeleteReturnRouteOperation(Network, node), null);
            return WaitCompletedSignal(token);
        }

        public ActionResult DeleteSucReturnRoute(NodeTag node, out ActionToken token)
        {
            token = ExecuteAsync(new DeleteSucReturnRouteOperation(Network, node), null);
            return WaitCompletedSignal(token);
        }

        public GetControllerCapabilitiesResult GetControllerCapabilities()
        {
            return (GetControllerCapabilitiesResult)Execute(new GetControllerCapabilitiesOperation());
        }

        public ActionToken GetControllerCapabilities(Action<IActionItem> completedCallback)
        {
            return ExecuteAsync(new GetControllerCapabilitiesOperation(), completedCallback);
        }

        public GetNeighborCountResult GetNeighborCount(NodeTag node)
        {
            return (GetNeighborCountResult)Execute(new GetNeighborCountOperation(node));
        }

        public GetRoutingInfoResult GetRoutingInfo(NodeTag node, byte removeBad, byte removeNonReps)
        {
            return (GetRoutingInfoResult)Execute(new GetRoutingInfoOperation(node, removeBad, removeNonReps));
        }

        public GetSucNodeIdResult GetSucNodeId()
        {
            return (GetSucNodeIdResult)Execute(new GetSucNodeIdOperation(_network));
        }

        public ActionToken GetSucNodeId(Action<IActionItem> completedCallback)
        {
            return ExecuteAsync(new GetSucNodeIdOperation(_network), completedCallback);
        }

        public ActionToken IsFailedNode(NodeTag node, Action<IActionItem> completedCallback)
        {
            return ExecuteAsync(new IsFailedNodeOperation(_network, node), completedCallback);
        }

        public IsFailedNodeResult IsFailedNode(NodeTag node)
        {
            return (IsFailedNodeResult)Execute(new IsFailedNodeOperation(_network, node));
        }

        public ActionResult RemoveFailedNodeId(NodeTag node)
        {
            return Execute(new RemoveFailedNodeIdOperation(_network, node));
        }

        public ActionToken RemoveFailedNodeId(NodeTag node, Action<IActionItem> completedCallback)
        {
            return ExecuteAsync(new RemoveFailedNodeIdOperation(_network, node), completedCallback);
        }

        public InclusionResult ReplaceFailedNode(NodeTag node)
        {
            ResetFailedNodeStatusSignals();
            ReplaceFailedNodeOperation ReplaceFailedNodeOperation = new ReplaceFailedNodeOperation(Network, node);
            return (InclusionResult)Execute(new InclusionTask(Network, ReplaceFailedNodeOperation));
        }

        public ActionToken ReplaceFailedNode(NodeTag node, Action<IActionItem> completedCallback)
        {
            ResetFailedNodeStatusSignals();
            ReplaceFailedNodeOperation ReplaceFailedNodeOperation = new ReplaceFailedNodeOperation(Network, node, SetFailedNodeStatusSignal, 60000);
            return ExecuteAsync(new InclusionTask(Network, ReplaceFailedNodeOperation), completedCallback);
        }

        public ActionToken ReplaceFailedNode(NodeTag node, Action<IActionItem> completedCallback, int timeoutMs)
        {
            ResetFailedNodeStatusSignals();
            ReplaceFailedNodeOperation ReplaceFailedNodeOperation = new ReplaceFailedNodeOperation(Network, node, SetFailedNodeStatusSignal, timeoutMs);
            return ExecuteAsync(new InclusionTask(Network, ReplaceFailedNodeOperation), completedCallback);
        }

        public ActionResult ReplicationReceiveComplete()
        {
            return Execute(new ReplicationReceiveCompleteOperation());
        }

        public TransmitResult ReplicationSend(NodeTag node, byte[] data, TransmitOptions txOptions)
        {
            return (TransmitResult)Execute(new ReplicationSendOperation(node, data, txOptions));
        }

        public RequestNodeNeighborUpdateResult RequestNodeNeighborUpdate(NodeTag node, int timeoutMs)
        {
            return (RequestNodeNeighborUpdateResult)Execute(new RequestNodeNeighborUpdateOperation(Network, node, timeoutMs));
        }

        public ActionToken RequestNodeNeighborUpdate(NodeTag node, int timeoutMs, Action<IActionItem> completedCallback)
        {
            return ExecuteAsync(new RequestNodeNeighborUpdateOperation(Network, node, timeoutMs), completedCallback);
        }

        public RequestNodeNeighborUpdateResult RequestNodeNeighborUpdate(NodeTag node, int timeoutMs, out ActionToken token)
        {
            token = RequestNodeNeighborUpdate(node, timeoutMs, null);
            return (RequestNodeNeighborUpdateResult)WaitCompletedSignal(token);
        }

        public RequestNodeNeighborUpdateResult RequestNodeNeighborDiscovery(NodeTag node, byte nodeType, int timeoutMs)
        {
            return (RequestNodeNeighborUpdateResult)Execute(new RequestNodeNeighborDiscoveryOperation(Network, node, nodeType, timeoutMs));
        }

        public ActionToken RequestNodeNeighborDiscovery(NodeTag node, byte nodeType, int timeoutMs, Action<IActionItem> completedCallback)
        {
            return ExecuteAsync(new RequestNodeNeighborDiscoveryOperation(Network, node, nodeType, timeoutMs), completedCallback);
        }

        public RequestNodeNeighborUpdateResult RequestNodeNeighborDiscovery(NodeTag node, byte nodeType, int timeoutMs, out ActionToken token)
        {
            token = RequestNodeNeighborDiscovery(node, nodeType, timeoutMs, null);
            return (RequestNodeNeighborUpdateResult)WaitCompletedSignal(token);
        }

        public TransmitResult SendSucId(NodeTag node, TransmitOptions txOptions)
        {
            return (TransmitResult)Execute(new SendSucIdOperation(node, txOptions));
        }

        public ActionResult SetRoutingInfo(NodeTag node, byte[] nodeMask)
        {
            return Execute(new SetRoutingInfoOperation(node, nodeMask));
        }

        public SetPriorityRouteResult SetPriorityRoute(NodeTag destination, NodeTag repeater0, NodeTag repeater1, NodeTag repeater2, NodeTag repeater3, byte routespeed)
        {
            return (SetPriorityRouteResult)Execute(new SetPriorityRouteOperation(Network, destination, repeater0, repeater1, repeater2, repeater3, routespeed));
        }

        public GetPriorityRouteResult GetPriorityRoute(NodeTag destination)
        {
            return (GetPriorityRouteResult)Execute(new GetPriorityRouteOperation(Network, destination));
        }

        #region ControllerChange

        public AddRemoveNodeResult ControllerChange(ControllerChangeModes mode, int timeoutMs)
        {
            ResetNodeStatusSignals();
            return (AddRemoveNodeResult)Execute(new ControllerChangeOperation(Network, mode, SetNodeStatusSignal, timeoutMs));
        }

        public ActionToken ControllerChange(ControllerChangeModes mode, int timeoutMs, Action<IActionItem> completedCallback)
        {
            ResetNodeStatusSignals();
            return ExecuteAsync(new ControllerChangeOperation(Network, mode, SetNodeStatusSignal, timeoutMs), completedCallback);
        }

        public ActionToken ControllerChange(ControllerChangeModes mode, bool isModeStopEnabled, int timeoutMs, Action<IActionItem> completedCallback)
        {
            ResetNodeStatusSignals();
            return ExecuteAsync(new ControllerChangeOperation(Network, mode, SetNodeStatusSignal, isModeStopEnabled, timeoutMs), completedCallback);
        }

        #endregion

        #region CreateNewPrimary

        public AddRemoveNodeResult CreateNewPrimary(CreatePrimaryModes mode, int timeoutMs)
        {
            return (AddRemoveNodeResult)Execute(new CreateNewPrimaryCtrlOperation(Network, mode, SetNodeStatusSignal, timeoutMs));
        }

        public ActionToken CreateNewPrimary(CreatePrimaryModes mode, int timeoutMs, Action<IActionItem> completedCallback)
        {
            return ExecuteAsync(new CreateNewPrimaryCtrlOperation(Network, mode, SetNodeStatusSignal, timeoutMs), completedCallback);
        }

        #endregion

        #region Allocated for NUNIT test

        public ActionResult NUnitCmd()
        {
            return Execute(new NUnitCmdOperation());
        }
        public ActionResult NUnitInit()
        {
            return Execute(new NUnitInitOperation());
        }
        public ActionResult NUnitList()
        {
            return Execute(new NUnitListOperation());
        }
        public NUnitRunResult NUnitRun(byte scenarioId)
        {
            return (NUnitRunResult)Execute(new NUnitRunOperation(scenarioId));
        }
        public ActionResult NUnitEnd()
        {
            return Execute(new NUnitEndOperation());
        }
        public ActionResult IoPortStatus(byte enable)
        {
            return Execute(new IoPortStatusOperation(enable));
        }
        public ActionResult IoPortPinSet(byte portPin, byte value)
        {
            return null;// Execute(new IoPortOperation(enable));
        }

        #endregion

        public ActionResult SetSmartStartMode(bool isStart)
        {
            return Execute(new SetSmartStartAction(isStart, 0));
        }

        public ActionResult SetSmartStartMode(bool isStart, int delayBeforeStart)
        {
            return Execute(new SetSmartStartAction(isStart, delayBeforeStart));
        }

        public ActionToken SmartStartSupport(Func<byte, byte[], NodeProvision?> dskNeededCallback, Action<bool, byte[], ActionResult> busyCallback, int delayBeforeStartMs, int inclusionTimeoutMs, SetupNodeLifelineSettings lifelineSettings)
        {
            return ExecuteAsync(new SmartStartSupport(Network, SetNodeStatusSignal, dskNeededCallback, busyCallback, delayBeforeStartMs, inclusionTimeoutMs, lifelineSettings), null);
        }

        public ActionToken SmartStartSupport(Func<byte, byte[], NodeProvision?> dskNeededCallback, Action<bool, byte[], ActionResult> busyCallback, int delayBeforeStartMs, int inclusionTimeoutMs)
        {
            return ExecuteAsync(new SmartStartSupport(Network, SetNodeStatusSignal, dskNeededCallback, busyCallback, delayBeforeStartMs, inclusionTimeoutMs), null);
        }
    }
}
