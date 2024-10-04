/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using Utils;
using ZWave.BasicApplication.Enums;
using ZWave.BasicApplication.Operations;
using ZWave.CommandClasses;
using ZWave.Devices;
using ZWave.Enums;

namespace ZWave.BasicApplication.Tasks
{
    class SetupNodeLifelineTask : ActionSerialGroup
    {
        private readonly TransmitOptions _txOptions =
            TransmitOptions.TransmitOptionAcknowledge |
            TransmitOptions.TransmitOptionAutoRoute |
            TransmitOptions.TransmitOptionExplore;

        private readonly NetworkViewPoint _network;
        private NodeInfoTask _requestNodeInfo;
        private RequestDataOperation _requestRoleType;
        private SetSucSelfOperation _setSelfAsSIS; // RT:00.11.0008.1 | RT:00.11.0006.1
        private SetSucNodeIdOperation _setNodeAsSIS;
        private DeleteReturnRouteOperation _deleteReturnRoute;
        private AssignReturnRouteOperation _assignReturnRoute;
        private DeleteSucReturnRouteOperation _deleteSucReturnRoute;
        private AssignSucReturnRouteOperation _assignSucReturnRoute;
        private SendDataOperation _sendAssociationCreate;
        private SendDataOperation _sendMultichannelAssociationCreate;
        private RequestDataOperation _requestWakeUpCapabilities;
        private SendDataOperation _sendWakeUpInterval;

        public SetupNodeLifelineSettings SetupNodeLifelineSettings { get; set; }

        public int WakeUpInterval { get; set; }
        public bool IsFullSetup { get; set; }
        public byte BasicDeviceType { get; set; }

        private NodeTag _sucNode;
        public NodeTag SucNode
        {
            get
            {
                return _sucNode;
            }
            set
            {
                _sucNode = value;
                var associationCmd = new COMMAND_CLASS_ASSOCIATION.ASSOCIATION_SET();
                associationCmd.groupingIdentifier = 0x01;
                associationCmd.nodeId = new List<byte>();
                associationCmd.nodeId.Add(_sucNode.Id > 0 ? (byte)_sucNode.Id : (byte)_node.Id);
                _sendAssociationCreate.Data = associationCmd;

                var multichannelAssociationCmd = new COMMAND_CLASS_MULTI_CHANNEL_ASSOCIATION_V3.MULTI_CHANNEL_ASSOCIATION_SET();
                multichannelAssociationCmd.groupingIdentifier = 0x01;
                multichannelAssociationCmd.nodeId = new List<byte>();

                multichannelAssociationCmd.nodeId.Add(_sucNode.Id > 0 ? (byte)_sucNode.Id : (byte)_network.NodeTag.Id);
                _sendMultichannelAssociationCreate.Data = multichannelAssociationCmd;

                if (SucNode.Id > 0)
                {
                    _setSelfAsSIS.SetCancelled();
                    _setNodeAsSIS.SetCancelled();
                }
            }
        }

        private NodeTag _node;
        public NodeTag Node
        {
            get
            {
                return _node;
            }
            set
            {
                _node = value;
                _assignReturnRoute.DestNode = _node;
            }
        }

        private NodeTag _targetNode;
        public NodeTag TargetNode
        {
            get
            {
                return _targetNode;
            }
            set
            {
                _targetNode = value;
                _requestNodeInfo.Node = _targetNode;
                _requestRoleType.DstNode = _targetNode;
                _setNodeAsSIS.Node = _targetNode;
                _deleteReturnRoute.Node = _targetNode;
                _assignReturnRoute.SrcNode = _targetNode;
                _deleteSucReturnRoute.Node = _targetNode;
                _assignSucReturnRoute.SrcNode = _targetNode;
                _sendAssociationCreate.DstNode = _targetNode;
                _sendMultichannelAssociationCreate.DstNode = _targetNode;
                _requestWakeUpCapabilities.DstNode = _targetNode;
                _sendWakeUpInterval.DstNode = _targetNode;
            }
        }

        public SetupNodeLifelineTask(NetworkViewPoint network) :
            base()
        {
            _network = network;
            WakeUpInterval = 5 * 60; // In Seconds.
            IsFullSetup = true;

            _requestNodeInfo = new NodeInfoTask(_network, NodeTag.Empty);
            _requestRoleType = new RequestDataOperation(network, NodeTag.Empty, NodeTag.Empty,
               new COMMAND_CLASS_ZWAVEPLUS_INFO_V2.ZWAVEPLUS_INFO_GET(), _txOptions,
               new COMMAND_CLASS_ZWAVEPLUS_INFO_V2.ZWAVEPLUS_INFO_REPORT(), 2,
               network.RequestTimeoutMs)
            { Name = "ReguestData (ZWavePlus)" };
            _setSelfAsSIS = new SetSucSelfOperation(_network, _network.NodeTag, 0x01, false, 0x01);
            _setNodeAsSIS = new SetSucNodeIdOperation(_network, Node, 0x01, false, 0x01);
            _deleteReturnRoute = new DeleteReturnRouteOperation(_network, NodeTag.Empty);
            _assignReturnRoute = new AssignReturnRouteOperation(_network, NodeTag.Empty, NodeTag.Empty);
            _deleteSucReturnRoute = new DeleteSucReturnRouteOperation(_network, NodeTag.Empty);
            _assignSucReturnRoute = new AssignSucReturnRouteOperation(_network, NodeTag.Empty);
            _sendAssociationCreate = new SendDataOperation(network, NodeTag.Empty, null, _txOptions);
            _sendMultichannelAssociationCreate = new SendDataOperation(network, NodeTag.Empty, null, _txOptions);
            _requestWakeUpCapabilities = new RequestDataOperation(network, NodeTag.Empty, NodeTag.Empty,
               new COMMAND_CLASS_WAKE_UP_V2.WAKE_UP_INTERVAL_CAPABILITIES_GET(), _txOptions,
               new COMMAND_CLASS_WAKE_UP_V2.WAKE_UP_INTERVAL_CAPABILITIES_REPORT(), 2,
               network.RequestTimeoutMs)
            { Name = "ReguestData (WakeUpCapabilites)" };
            _sendWakeUpInterval = new SendDataOperation(network, NodeTag.Empty, null, _txOptions);

            SpecificResult.NodeInfo = _requestNodeInfo.SpecificResult;
            SpecificResult.SetWakeUpInterval = _sendWakeUpInterval.SpecificResult;
            SpecificResult.RequestRoleType = _requestRoleType.SpecificResult;

            Actions = new ActionBase[] {
                _requestNodeInfo,
                _requestRoleType,
                _setSelfAsSIS,
                _setNodeAsSIS,
                _deleteReturnRoute,
                _assignReturnRoute,
                _deleteSucReturnRoute,
                _assignSucReturnRoute,
                _sendAssociationCreate,
                _sendMultichannelAssociationCreate,
                _requestWakeUpCapabilities,
                _sendWakeUpInterval
            };

            _onActionCompleted = OnActionCompleted;
        }

        private bool IsEndNodeRoleType()
        {
            if (_requestRoleType.SpecificResult)
            {
                COMMAND_CLASS_ZWAVEPLUS_INFO_V2.ZWAVEPLUS_INFO_REPORT zwaveInfo = SpecificResult.RequestRoleType.Command;
                var roleType = (RoleTypes)zwaveInfo.roleType.Value;
                return roleType == RoleTypes.END_NODE_ALWAYS_ON ||
                    roleType == RoleTypes.END_NODE_PORTABLE ||
                    roleType == RoleTypes.END_NODE_SLEEPING_LISTENING ||
                    roleType == RoleTypes.END_NODE_SLEEPING_REPORTING;
            }
            return BasicDeviceType != 0x04; //BASIC_TYPE_ROUTING_END_NODE
        }

        private void OnActionCompleted(ActionBase action, ActionResult result)
        {
            if (action is SetSucNodeIdOperation)
            {
                if (_setSelfAsSIS.Result)
                {
                    _setNodeAsSIS.SetCancelled();
                }
            }
            else if (action is DeleteReturnRouteOperation)
            {
                if (_setSelfAsSIS.SpecificResult)
                {
                    SucNode = Node;
                }
                if (_setNodeAsSIS.SpecificResult)
                {
                    SucNode = TargetNode;
                }

                if (!IsEndNodeRoleType())
                {
                    _deleteReturnRoute.SetCancelled();
                    _assignReturnRoute.SetCancelled();
                    _deleteSucReturnRoute.SetCancelled();
                    _assignSucReturnRoute.SetCancelled();
                }
                else
                {
                    if (SucNode.Id > 0)
                    {
                        _deleteReturnRoute.SetCancelled();
                        _assignReturnRoute.SetCancelled();
                    }
                    else
                    {
                        _deleteSucReturnRoute.SetCancelled();
                        _assignSucReturnRoute.SetCancelled();
                    }
                }
            }

            else if (result is NodeInfoResult)
            {
                if (!result || !IsFullSetup || ((NodeInfoResult)result).RequestNodeInfo.NodeInfo == null)
                {
                    _deleteReturnRoute.SetCancelled();
                    _assignReturnRoute.SetCancelled();
                    _deleteSucReturnRoute.SetCancelled();
                    _assignSucReturnRoute.SetCancelled();
                    _sendAssociationCreate.SetCancelled();
                    _sendMultichannelAssociationCreate.SetCancelled();
                    _requestWakeUpCapabilities.SetCancelled();
                    _sendWakeUpInterval.SetCancelled();
                    SpecificResult.IsSetupLifelineCancelled = true;
                }
                else if (SetupNodeLifelineSettings != SetupNodeLifelineSettings.Default)
                {
                    if (!SetupNodeLifelineSettings.HasFlag(SetupNodeLifelineSettings.IsSetAsSisAutomatically) || !SetupNodeLifelineSettings.HasFlag(SetupNodeLifelineSettings.IsBasedOnZwpRoleType))
                    {
                        _setSelfAsSIS.SetCancelled();
                        _setNodeAsSIS.SetCancelled();
                    }
                    if (!SetupNodeLifelineSettings.HasFlag(SetupNodeLifelineSettings.IsDeleteReturnRoute))
                    {
                        _deleteReturnRoute.SetCancelled();
                        _deleteSucReturnRoute.SetCancelled();
                    }
                    if (!SetupNodeLifelineSettings.HasFlag(SetupNodeLifelineSettings.IsAssignReturnRoute))
                    {
                        _assignReturnRoute.SetCancelled();
                        _assignSucReturnRoute.SetCancelled();
                    }
                    if (!SetupNodeLifelineSettings.HasFlag(SetupNodeLifelineSettings.IsAssociationCreate))
                    {
                        _sendAssociationCreate.SetCancelled();
                    }
                    if (!SetupNodeLifelineSettings.HasFlag(SetupNodeLifelineSettings.IsMultichannelAssociationCreate))
                    {
                        _sendMultichannelAssociationCreate.SetCancelled();
                    }
                    if (!SetupNodeLifelineSettings.HasFlag(SetupNodeLifelineSettings.IsWakeUpCapabilities))
                    {
                        _requestWakeUpCapabilities.SetCancelled();
                    }
                    if (!SetupNodeLifelineSettings.HasFlag(SetupNodeLifelineSettings.IsWakeUpInterval))
                    {
                        _sendWakeUpInterval.SetCancelled();
                    }
                }

                if (_requestNodeInfo.SpecificResult.RequestNodeInfo.CommandClasses == null ||
                    !_requestNodeInfo.SpecificResult.RequestNodeInfo.CommandClasses.Contains(COMMAND_CLASS_ZWAVEPLUS_INFO_V2.ID))
                {
                    _requestRoleType.SetCancelled();
                    if (SetupNodeLifelineSettings != SetupNodeLifelineSettings.Default &&
                        SetupNodeLifelineSettings.HasFlag(SetupNodeLifelineSettings.IsSetAsSisAutomatically) &&
                        SetupNodeLifelineSettings.HasFlag(SetupNodeLifelineSettings.IsBasedOnZwpRoleType))
                    {
                        _setNodeAsSIS.SetCancelled();
                    }
                }

                if ((_requestNodeInfo.SpecificResult.RequestNodeInfo.CommandClasses == null ||
                    !_requestNodeInfo.SpecificResult.RequestNodeInfo.CommandClasses.Contains(COMMAND_CLASS_ASSOCIATION.ID)) &&
                    (_requestNodeInfo.SpecificResult.RequestNodeInfo.SecureCommandClasses == null ||
                    !_requestNodeInfo.SpecificResult.RequestNodeInfo.SecureCommandClasses.Contains(COMMAND_CLASS_ASSOCIATION.ID)))
                {
                    _sendAssociationCreate.SetCancelled();
                }

                if ((_requestNodeInfo.SpecificResult.RequestNodeInfo.CommandClasses == null ||
                    !_requestNodeInfo.SpecificResult.RequestNodeInfo.CommandClasses.Contains(COMMAND_CLASS_MULTI_CHANNEL_ASSOCIATION_V3.ID)) &&
                    (_requestNodeInfo.SpecificResult.RequestNodeInfo.SecureCommandClasses == null ||
                    !_requestNodeInfo.SpecificResult.RequestNodeInfo.SecureCommandClasses.Contains(COMMAND_CLASS_MULTI_CHANNEL_ASSOCIATION_V3.ID)))
                {
                    _sendMultichannelAssociationCreate.SetCancelled();
                }
                else
                {
                    _sendAssociationCreate.SetCancelled();
                }

                if ((_requestNodeInfo.SpecificResult.RequestNodeInfo.CommandClasses == null ||
                    !_requestNodeInfo.SpecificResult.RequestNodeInfo.CommandClasses.Contains(COMMAND_CLASS_WAKE_UP_V2.ID)) &&
                    (_requestNodeInfo.SpecificResult.RequestNodeInfo.SecureCommandClasses == null ||
                    !_requestNodeInfo.SpecificResult.RequestNodeInfo.SecureCommandClasses.Contains(COMMAND_CLASS_WAKE_UP_V2.ID)))
                {
                    _requestWakeUpCapabilities.SetCancelled();
                    _sendWakeUpInterval.SetCancelled();
                }
            }
            else if (result is RequestDataResult)
            {
                var requestRes = (RequestDataResult)result;
                if (result)
                {
                    if (requestRes.Command != null && requestRes.Command.Length > 2)
                    {
                        if (requestRes.Command[0] == COMMAND_CLASS_WAKE_UP_V2.ID
                            && requestRes.Command[1] == COMMAND_CLASS_WAKE_UP_V2.WAKE_UP_INTERVAL_CAPABILITIES_REPORT.ID)
                        {
                            var wakeUpCapabilitiesReport = (COMMAND_CLASS_WAKE_UP_V2.WAKE_UP_INTERVAL_CAPABILITIES_REPORT)requestRes.Command;
                            int minVal = Tools.GetInt32(wakeUpCapabilitiesReport.minimumWakeUpIntervalSeconds);
                            if (minVal > WakeUpInterval)
                            {
                                WakeUpInterval = minVal;
                            }

                            COMMAND_CLASS_WAKE_UP.WAKE_UP_INTERVAL_SET cmd = new COMMAND_CLASS_WAKE_UP.WAKE_UP_INTERVAL_SET();
                            cmd.seconds = Tools.GetBytes(WakeUpInterval).Skip(1).ToArray();
                            cmd.nodeid = (byte)Node.Id;
                            _sendWakeUpInterval.Data = cmd;
                            SpecificResult.WakeUpIntervalValueSeconds = WakeUpInterval;
                        }
                        else if (requestRes.Command[0] == COMMAND_CLASS_ZWAVEPLUS_INFO.ID
                            && requestRes.Command[1] == COMMAND_CLASS_ZWAVEPLUS_INFO.ZWAVEPLUS_INFO_REPORT.ID)
                        {
                            var roleReport = (COMMAND_CLASS_ZWAVEPLUS_INFO.ZWAVEPLUS_INFO_REPORT)requestRes.Command;
                            if (roleReport != null && roleReport.roleType == (byte)RoleTypes.CONTROLLER_CENTRAL_STATIC)
                            {
                                _setSelfAsSIS.SetCancelled();
                            }
                        }
                    }
                }
                else
                {
                    COMMAND_CLASS_WAKE_UP.WAKE_UP_INTERVAL_SET cmd = new COMMAND_CLASS_WAKE_UP.WAKE_UP_INTERVAL_SET();
                    cmd.seconds = Tools.GetBytes(WakeUpInterval).Skip(1).ToArray();
                    cmd.nodeid = (byte)Node.Id;
                    _sendWakeUpInterval.Data = cmd;
                    SpecificResult.WakeUpIntervalValueSeconds = WakeUpInterval;
                }
            }
        }

        public SetupNodeLifelineResult SpecificResult
        {
            get { return (SetupNodeLifelineResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new SetupNodeLifelineResult();
        }
    }

    public class SetupNodeLifelineResult : ActionResult
    {
        public NodeInfoResult NodeInfo { get; set; }
        public SendDataResult SetWakeUpInterval { get; set; }
        public int WakeUpIntervalValueSeconds { get; set; }
        public RequestDataResult RequestRoleType { get; set; }
        public bool IsSetupLifelineCancelled { get; set; }
    }
}
