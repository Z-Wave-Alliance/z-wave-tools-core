/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Linq;
using Utils;
using ZWave.BasicApplication.CommandClasses;
using ZWave.BasicApplication.Enums;
using ZWave.BasicApplication.Operations;
using ZWave.CommandClasses;
using ZWave.Devices;
using ZWave.Enums;

namespace ZWave.BasicApplication.Tasks
{
    public class InclusionTask : ActionParallelGroup
    {
        bool _isInitiatePerformed = false;
        const byte PROXY_INCLUSION = 0x01;
        const byte PROXY_INCLUSION_REPLACE = 0x03;

        public static int TIMEOUT = 60000;

        private NetworkViewPoint _network;
        private readonly Modes _mode;
        private readonly int _timeoutMs;
        private readonly Action<NodeStatuses> _nodeStatusCallback;
        private readonly Action<FailedNodeStatuses> _failedNodeStatusCallback;
        private readonly TransmitOptions _txOptions =
            TransmitOptions.TransmitOptionAcknowledge |
            TransmitOptions.TransmitOptionAutoRoute |
            TransmitOptions.TransmitOptionExplore;

        private FilterAchOperation _peerFilter;
        private IAddRemoveNode _addNode;
        private MemoryGetIdOperation _memoryGetId;
        private GetSucNodeIdOperation _getSucNodeId;

        private IsFailedNodeOperation _isFailedSucNodeOperation;
        private RequestNodeInfoOperation _getSucNodeInfo;
        private ActionSerialGroup _requestSucNodeInfoGroup;

        private InclusionController.Initiate _requestInclusionController;
        private readonly SerialApiGetInitDataOperation _serialApiGetInitData;
        private SetupNodeLifelineTask _setupNodeLifelineTask;

        private SendDataOperation _sendNopOperation;
        private IsFailedNodeOperation _isFailedNodeOperation;
        private RemoveFailedNodeIdOperation _removeFailedNodeIdOperation;
        private readonly bool _isSmartStart = false;

        public InclusionTask(NetworkViewPoint network, IAddRemoveNode addRemoveNode, SetupNodeLifelineSettings setupNodeLifelineSettings = SetupNodeLifelineSettings.Default)
            : this(network, addRemoveNode, false, setupNodeLifelineSettings)
        {
        }

        public InclusionTask(NetworkViewPoint network, IAddRemoveNode addRemoveNode, bool IsSmartStart, SetupNodeLifelineSettings setupNodeLifelineSettings)
            : base(false, null)
        {
            _network = network;
            _isSmartStart = IsSmartStart;
            if (addRemoveNode is AddNodeOperation)
            {
                _mode = (addRemoveNode as AddNodeOperation).InitMode;
                _nodeStatusCallback = (addRemoveNode as AddNodeOperation).NodeStatusCallback;
                _failedNodeStatusCallback = null;
                _timeoutMs = (addRemoveNode as AddNodeOperation).TimeoutMs;
                (addRemoveNode as AddNodeOperation).NodeStatusCallback = OnNodeStatus;
            }
            else if (addRemoveNode is ReplaceFailedNodeOperation)
            {
                _mode = Modes.NodeAny;
                _nodeStatusCallback = null;
                _failedNodeStatusCallback = (addRemoveNode as ReplaceFailedNodeOperation).FailedNodeStatusCallback;
                _timeoutMs = (addRemoveNode as ReplaceFailedNodeOperation).TimeoutMs;
                (addRemoveNode as ReplaceFailedNodeOperation).FailedNodeStatusCallback = OnFailedNodeStatus;
            }
            else
            {
                _mode = Modes.NodeAny;
                _timeoutMs = TIMEOUT;
            }

            _peerFilter = new FilterAchOperation(_network);
            _peerFilter.SetFilterNodeId(new NodeTag(0xFF));
            _addNode = addRemoveNode;

            _memoryGetId = new MemoryGetIdOperation(_network);

            _getSucNodeId = new GetSucNodeIdOperation(_network);
            _isFailedSucNodeOperation = new IsFailedNodeOperation(_network, NodeTag.Empty);
            _getSucNodeInfo = new RequestNodeInfoOperation(_network, NodeTag.Empty);
            _requestSucNodeInfoGroup = new ActionSerialGroup(OnSucNodeInfoCompletedGroup, _getSucNodeId, _getSucNodeInfo, _isFailedSucNodeOperation)
            {
                Name = "RequestSucNodeInfoGroup (InclusionController)",
                CompletedCallback = OnRequestSucNodeInfoGroupCompleted
            };

            _setupNodeLifelineTask = new SetupNodeLifelineTask(_network);
            _setupNodeLifelineTask.SetupNodeLifelineSettings = setupNodeLifelineSettings;

            _requestInclusionController = new InclusionController.Initiate(_network, NodeTag.Empty, NodeTag.Empty, _txOptions, _network.InclusionControllerInitiateRequestTimeoutMs ?? 240000) { Name = "RequestData (InclusionController)" };
            _serialApiGetInitData = new SerialApiGetInitDataOperation();

            SpecificResult.AddRemoveNode = _addNode.SpecificResult;
            SpecificResult.MemoryGetId = _memoryGetId.SpecificResult;
            SpecificResult.GetSucNodeId = _getSucNodeId.SpecificResult;
            SpecificResult.NodeInfo = _setupNodeLifelineTask.SpecificResult.NodeInfo;
            SpecificResult.SetWakeUpInterval = _setupNodeLifelineTask.SpecificResult.SetWakeUpInterval;

            _sendNopOperation = new SendDataOperation(_network, _addNode.SpecificResult.Node, new byte[1], _txOptions);
            _isFailedNodeOperation = new IsFailedNodeOperation(_network, _addNode.SpecificResult.Node);
            _removeFailedNodeIdOperation = new RemoveFailedNodeIdOperation(_network, _addNode.SpecificResult.Node);
            if (_isSmartStart)
            {
                _memoryGetId.SetCancelled();
                _requestSucNodeInfoGroup.SetCancelled();
                _serialApiGetInitData.SetCancelled();
                _requestInclusionController.SetCancelled();
            }
            else
            {
                _sendNopOperation.SetCancelled();
                _isFailedNodeOperation.SetCancelled();
                _removeFailedNodeIdOperation.SetCancelled();
            }

            Actions = new ActionBase[]
            {
                _peerFilter,
                new ActionSerialGroup(OnActionCompleted,
                _memoryGetId,
                _requestSucNodeInfoGroup,
                _serialApiGetInitData,
                (ActionBase)_addNode,
                _sendNopOperation,
                _isFailedNodeOperation,
                _removeFailedNodeIdOperation,
                _requestInclusionController,
                _setupNodeLifelineTask
                ) { Name = "Inclusion (Group)" }
            };
        }

        private void OnRequestSucNodeInfoGroupCompleted(IActionItem obj)
        {
            var requestSucNodeInfoGroup = (ActionSerialGroup)obj;
            if (requestSucNodeInfoGroup.IsStateCompleted)
            {
                var result = (RequestNodeInfoResult)requestSucNodeInfoGroup.Result.InnerResults.FirstOrDefault(x => x is RequestNodeInfoResult);
                if (result != null && result)
                {
                    byte[] supported = result.CommandClasses;
                    byte[] secureSupported = result.SecureCommandClasses;
                    if ((supported != null && supported.Contains(COMMAND_CLASS_INCLUSION_CONTROLLER.ID)) ||
                        (secureSupported != null && secureSupported.Contains(COMMAND_CLASS_INCLUSION_CONTROLLER.ID)))
                    {
                        (_addNode as ApiOperation).SubstituteSettings.SetFlag(SubstituteFlags.DenySecurity);
                        _requestInclusionController.DstNode = _getSucNodeId.SpecificResult.SucNode;
                        return;
                    }
                }
            }
            _requestInclusionController.SetCancelled();
        }

        private void OnSucNodeInfoCompletedGroup(ActionBase action, ActionResult result)
        {
            if (result is GetSucNodeIdResult && result)
            {
                _setupNodeLifelineTask.SucNode = _getSucNodeId.SpecificResult.SucNode;
                if (_getSucNodeId.SpecificResult.SucNode.Id > 0 && _getSucNodeId.SpecificResult.SucNode != _memoryGetId.SpecificResult.Node &&
                    _network.HasCommandClass(COMMAND_CLASS_INCLUSION_CONTROLLER.ID))
                {
                    _isFailedSucNodeOperation.Node = _getSucNodeId.SpecificResult.SucNode;
                    _getSucNodeInfo.Node = _getSucNodeId.SpecificResult.SucNode;
                    _peerFilter.SetFilterSucNodeId(_getSucNodeId.SpecificResult.SucNode);
                    _setupNodeLifelineTask.IsFullSetup = false;
                }
                else
                {
                    _requestSucNodeInfoGroup.SetCancelled();
                    _requestInclusionController.SetCancelled();
                }
            }
            else if (result is IsFailedNodeResult && result)
            {
                if (((IsFailedNodeResult)result).RetValue) // SUC failed.
                {
                    _requestSucNodeInfoGroup.SetCancelled();
                }
            }
        }

        private void OnNodeStatus(NodeStatuses nodeStatus)
        {
            if (nodeStatus == NodeStatuses.AddingRemovingController || nodeStatus == NodeStatuses.AddingRemovingEndDevice || nodeStatus == NodeStatuses.Done)
            {
                if (_addNode is ReplaceFailedNodeOperation)
                {
                    _addNode.SpecificResult.AddRemoveNodeStatus = AddRemoveNodeStatuses.Replaced;
                }
                _peerFilter.SetFilterNodeId(_addNode.SpecificResult.Node);
            }
            _nodeStatusCallback?.Invoke(nodeStatus);
        }

        private void OnFailedNodeStatus(FailedNodeStatuses failedNodeStatus)
        {
            if (failedNodeStatus == FailedNodeStatuses.NodeReplaceDone)
            {
                _addNode.SpecificResult.AddRemoveNodeStatus = AddRemoveNodeStatuses.Replaced;
            }
            if (failedNodeStatus == FailedNodeStatuses.NodeReplace)
            {
                _peerFilter.SetFilterNodeId(_addNode.SpecificResult.Node);
            }
            _failedNodeStatusCallback?.Invoke(failedNodeStatus);
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            base.SetStateCompleted(ou);
            if (SpecificResult.AddRemoveNode.State == ActionStates.Completed)
            {
                if (_setupNodeLifelineTask.SpecificResult.RequestRoleType &&
                    _setupNodeLifelineTask.SpecificResult.RequestRoleType.Command != null &&
                    _setupNodeLifelineTask.SpecificResult.RequestRoleType.Command.Length > 2)
                {
                    COMMAND_CLASS_ZWAVEPLUS_INFO_V2.ZWAVEPLUS_INFO_REPORT cmd = _setupNodeLifelineTask.SpecificResult.RequestRoleType.Command;
                    SpecificResult.AddRemoveNode.RoleType = cmd.roleType;
                    SpecificResult.AddRemoveNode.NodeType = cmd.nodeType;
                    SpecificResult.AddRemoveNode.ZWavePlusVersion = cmd.zWaveVersion;
                }

                if (_isInitiatePerformed)
                {
                    if (_setupNodeLifelineTask.SpecificResult.NodeInfo.RequestNodeInfo.SecuritySchemes?.Length > 0)
                    {
                        SpecificResult.AddRemoveNode.SubstituteStatus = SubstituteStatuses.Done;
                    }
                    else
                    {
                        SpecificResult.AddRemoveNode.SubstituteStatus = SubstituteStatuses.None;
                    }
                }
            }
        }

        private void OnActionCompleted(ActionBase action, ActionResult result)
        {
            if (result is MemoryGetIdResult && result)
            {
                _setupNodeLifelineTask.Node = _memoryGetId.SpecificResult.Node;
            }
            else if (result is SerialApiGetInitDataResult && result)
            {
                if (_addNode is AddNodeOperation)
                {
                    (_addNode as AddNodeOperation).NodesBefore = (result as SerialApiGetInitDataResult).IncludedNodes;
                }
            }
            else if (result is AddRemoveNodeResult)
            {
                if (result && _addNode.SpecificResult.Id > 0)
                {
                    _network.SetSecuritySchemesSpecified(_addNode.SpecificResult.Node);
                    // update parameters
                    _sendNopOperation.DstNode = _addNode.SpecificResult.Node;
                    _isFailedNodeOperation.Node = _addNode.SpecificResult.Node;
                    _setupNodeLifelineTask.TargetNode = _addNode.SpecificResult.Node;
                    _setupNodeLifelineTask.BasicDeviceType = _addNode.SpecificResult.Basic;
                    byte stepId = _addNode.SpecificResult.AddRemoveNodeStatus == AddRemoveNodeStatuses.Replaced ? PROXY_INCLUSION_REPLACE : PROXY_INCLUSION;
                    _requestInclusionController.SetCommandParameters(_addNode.SpecificResult.Node, stepId);

                    // correct workflow 
                    if ((result as AddRemoveNodeResult).SubstituteStatus != SubstituteStatuses.Failed)
                    {
                        _sendNopOperation.SetCancelled();
                        _isFailedNodeOperation.SetCancelled();
                        _removeFailedNodeIdOperation.SetCancelled();
                    }

                    if (_addNode.SpecificResult.Node == _getSucNodeId.SpecificResult.SucNode)
                    {
                        _requestInclusionController.SetCancelled();
                    }

                    if (_addNode.SpecificResult.AddRemoveNodeStatus == AddRemoveNodeStatuses.Replicated)
                    {
                        _requestInclusionController.SetCancelled();
                        _setupNodeLifelineTask.SetCancelled();
                    }
                }
                else
                {
                    _sendNopOperation.SetCancelled();
                    _isFailedNodeOperation.SetCancelled();
                    _removeFailedNodeIdOperation.SetCancelled();
                    _requestInclusionController.SetCancelled();
                    _setupNodeLifelineTask.SetCancelled();
                }
            }
            else if (result is RequestDataResult && result)
            {
                var requestRes = (RequestDataResult)result;
                if (requestRes.Command != null && requestRes.Command.Length > 2 &&
                    requestRes.Command[0] == COMMAND_CLASS_INCLUSION_CONTROLLER.ID &&
                    requestRes.Command[1] == COMMAND_CLASS_INCLUSION_CONTROLLER.COMPLETE.ID)
                {
                    COMMAND_CLASS_INCLUSION_CONTROLLER.COMPLETE cmd = requestRes.Command;
                    if (cmd.status == 0x01)
                    {
                        _isInitiatePerformed = true;
                    }
                    else
                    {
                        SpecificResult.AddRemoveNode.SubstituteStatus = SubstituteStatuses.Failed;
                    }
                }
            }
            else if (result is IsFailedNodeResult)
            {
                if (result && (result as IsFailedNodeResult).RetValue)
                {
                    _removeFailedNodeIdOperation.ReplacedNode = _addNode.SpecificResult.Node;
                    SpecificResult.AddRemoveNode.AddRemoveNodeStatus = AddRemoveNodeStatuses.None;
                    _requestInclusionController.SetCancelled();
                    _setupNodeLifelineTask.SetCancelled();
                }
                else
                {
                    _removeFailedNodeIdOperation.SetCancelled();
                }
            }
            SpecificResult.SetupLifelineResult = _setupNodeLifelineTask.SpecificResult;
        }

        public InclusionResult SpecificResult
        {
            get { return (InclusionResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new InclusionResult();
        }
    }

    public class InclusionResult : ActionResult
    {
        public AddRemoveNodeResult AddRemoveNode { get; set; }
        public MemoryGetIdResult MemoryGetId { get; set; }
        public GetSucNodeIdResult GetSucNodeId { get; set; }
        public NodeInfoResult NodeInfo { get; set; }
        public SendDataResult SetWakeUpInterval { get; set; }
        public SetupNodeLifelineResult SetupLifelineResult { get; set; }
    }
}