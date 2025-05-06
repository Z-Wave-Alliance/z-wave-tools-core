/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Collections.Generic;
using System.Linq;
using ZWave.Enums;
using ZWave.CommandClasses;
using ZWave.BasicApplication.Security;
using ZWave.Security;
using ZWave.Devices;
using System;

namespace ZWave.BasicApplication.Operations
{
    public class RequestNodeInfoSecureTask : ActionSerialGroup
    {
        public static int START_DELAY = 500;

        private readonly TransmitOptions _txOptions =
           TransmitOptions.TransmitOptionAcknowledge |
           TransmitOptions.TransmitOptionAutoRoute |
           TransmitOptions.TransmitOptionExplore;
        private SecurityManagerInfo _securityManagerInfo;

        private RequestNodeInfoOperation _nodeInfo;
        private RequestDataExOperation _supportedS0;
        private RequestDataExOperation _supportedS2_ACCESS;
        private RequestDataExOperation _supportedS2_AUTHENTICATED;
        private RequestDataExOperation _supportedS2_UNAUTHENTICATED;
        private RequestDataOperation _zwavePlusInfo;
        private readonly DelayOperation _delayBeforeStart;
        private bool _isInclusionTask;
        private ActionBase _lastProbingOperation;
        private NetworkViewPoint _network;

        public RequestNodeInfoSecureTask(NetworkViewPoint network, SecurityManagerInfo securityManagerInfo, RequestNodeInfoOperation action, bool isInclusionTask)
        {
            _network = network;
            _allowFailed = true;
            _securityManagerInfo = securityManagerInfo;
            _nodeInfo = action;
            _isInclusionTask = isInclusionTask;

            var tm = securityManagerInfo.Network.RequestTimeoutMs;

            _delayBeforeStart = new DelayOperation(START_DELAY);

            _supportedS0 = new RequestDataExOperation(_network, NodeTag.Empty, NodeTag.Empty,
                    new COMMAND_CLASS_SECURITY.SECURITY_COMMANDS_SUPPORTED_GET(), _txOptions,
                     TransmitSecurityOptions.S2_TXOPTION_VERIFY_DELIVERY, SecuritySchemes.S0, TransmitOptions2.NONE,
                    COMMAND_CLASS_SECURITY.ID, COMMAND_CLASS_SECURITY.SECURITY_COMMANDS_SUPPORTED_REPORT.ID, tm);

            _supportedS2_ACCESS = new RequestDataExOperation(_network, NodeTag.Empty, NodeTag.Empty,
               new COMMAND_CLASS_SECURITY_2.SECURITY_2_COMMANDS_SUPPORTED_GET(), _txOptions,
               TransmitSecurityOptions.S2_TXOPTION_VERIFY_DELIVERY, SecuritySchemes.S2_ACCESS, TransmitOptions2.NONE,
               COMMAND_CLASS_SECURITY_2.ID, COMMAND_CLASS_SECURITY_2.SECURITY_2_COMMANDS_SUPPORTED_REPORT.ID, tm);

            _supportedS2_AUTHENTICATED = new RequestDataExOperation(_network, NodeTag.Empty, NodeTag.Empty,
                new COMMAND_CLASS_SECURITY_2.SECURITY_2_COMMANDS_SUPPORTED_GET(), _txOptions,
                TransmitSecurityOptions.S2_TXOPTION_VERIFY_DELIVERY, SecuritySchemes.S2_AUTHENTICATED, TransmitOptions2.NONE,
                COMMAND_CLASS_SECURITY_2.ID, COMMAND_CLASS_SECURITY_2.SECURITY_2_COMMANDS_SUPPORTED_REPORT.ID, tm);

            _supportedS2_UNAUTHENTICATED = new RequestDataExOperation(_network, NodeTag.Empty, NodeTag.Empty,
                new COMMAND_CLASS_SECURITY_2.SECURITY_2_COMMANDS_SUPPORTED_GET(), _txOptions,
                TransmitSecurityOptions.S2_TXOPTION_VERIFY_DELIVERY, SecuritySchemes.S2_UNAUTHENTICATED, TransmitOptions2.NONE,
                COMMAND_CLASS_SECURITY_2.ID, COMMAND_CLASS_SECURITY_2.SECURITY_2_COMMANDS_SUPPORTED_REPORT.ID, tm);

            _zwavePlusInfo = new RequestDataOperation(_network, NodeTag.Empty, NodeTag.Empty,
               new COMMAND_CLASS_ZWAVEPLUS_INFO_V2.ZWAVEPLUS_INFO_GET(), _txOptions,
               new COMMAND_CLASS_ZWAVEPLUS_INFO_V2.ZWAVEPLUS_INFO_REPORT(), 2,
               tm);

            List<ActionBase> list = new List<ActionBase>();
            list.Add(_delayBeforeStart);
            list.Add(_nodeInfo);
            if (_securityManagerInfo.Network.HasSecurityScheme(SecuritySchemeSet.ALLS2))
            {
                if (_securityManagerInfo.Network.IsSecuritySchemesSpecified(_nodeInfo.Node))
                {
                    if (_securityManagerInfo.Network.HasSecurityScheme(SecuritySchemes.S2_ACCESS))
                    {
                        if (!_isInclusionTask || _securityManagerInfo.Network.HasSecurityScheme(_nodeInfo.Node, SecuritySchemes.S2_ACCESS))
                        {
                            list.Add(_supportedS2_ACCESS);
                        }
                    }
                    if (_securityManagerInfo.Network.HasSecurityScheme(SecuritySchemes.S2_AUTHENTICATED))
                    {
                        if (!_isInclusionTask || _securityManagerInfo.Network.HasSecurityScheme(_nodeInfo.Node, SecuritySchemes.S2_AUTHENTICATED))
                        {
                            list.Add(_supportedS2_AUTHENTICATED);
                        }
                    }
                    if (_securityManagerInfo.Network.HasSecurityScheme(SecuritySchemes.S2_UNAUTHENTICATED))
                    {
                        if (!_isInclusionTask || _securityManagerInfo.Network.HasSecurityScheme(_nodeInfo.Node, SecuritySchemes.S2_UNAUTHENTICATED))
                        {
                            list.Add(_supportedS2_UNAUTHENTICATED);
                        }
                    }
                    if (_securityManagerInfo.Network.HasSecurityScheme(SecuritySchemes.S0))
                    {
                        if (!_isInclusionTask || _securityManagerInfo.Network.HasSecurityScheme(_nodeInfo.Node, SecuritySchemes.S0))
                        {
                            if (!list.Contains(_supportedS0))
                            {
                                list.Add(_supportedS0);
                            }
                        }
                    }
                }
                else
                {
                    if (_securityManagerInfo.Network.HasSecurityScheme(SecuritySchemes.S2_ACCESS))
                    {
                        list.Add(_supportedS2_ACCESS);
                    }
                    if (_securityManagerInfo.Network.HasSecurityScheme(SecuritySchemes.S2_AUTHENTICATED))
                    {
                        list.Add(_supportedS2_AUTHENTICATED);
                    }
                    if (_securityManagerInfo.Network.HasSecurityScheme(SecuritySchemes.S2_UNAUTHENTICATED))
                    {
                        list.Add(_supportedS2_UNAUTHENTICATED);
                    }
                }
            }
            if (_securityManagerInfo.Network.HasSecurityScheme(SecuritySchemes.S0))
            {
                if (_securityManagerInfo.Network.IsSecuritySchemesSpecified(_nodeInfo.Node))
                {
                    if (_securityManagerInfo.Network.HasSecurityScheme(_nodeInfo.Node, SecuritySchemes.S0) &&
                        !_securityManagerInfo.Network.HasSecurityScheme(_nodeInfo.Node, SecuritySchemeSet.ALLS2))
                    {
                        if (!list.Contains(_supportedS0))
                        {
                            list.Add(_supportedS0);
                        }
                    }
                }
                else if (!list.Contains(_supportedS0))
                {
                    list.Add(_supportedS0);
                }
            }

            if (!_isInclusionTask && list.Count > 0)
            {
                _lastProbingOperation = list.Last();
                list.Add(_zwavePlusInfo);
            }
            Actions = list.ToArray();
        }

        internal SecurityManagerInfo GetSecurityManagerInfo()
        {
            return _securityManagerInfo;
        }

        public RequestNodeInfoResult SpecificResult
        {
            get { return (RequestNodeInfoResult)Result; }
        }

        protected override void OnCompletedInternal(ActionCompletedUnit ou)
        {
            if (ou.Action.Result is RequestNodeInfoResult)
            {
                if (ou.Action.Result)
                {
                    _securityManagerInfo.Network.ResetCurrentSecurityScheme(_nodeInfo.Node);
                    var res = (RequestNodeInfoResult)ou.Action.Result;
                    _securityManagerInfo.Network.SetCommandClasses(res.Node, res.CommandClasses);
                    res.CopyTo(SpecificResult);
                    _zwavePlusInfo.DstNode = _nodeInfo.Node;
                    _supportedS0.DestNode = _nodeInfo.Node;
                    _supportedS2_ACCESS.DestNode = _nodeInfo.Node;
                    _supportedS2_AUTHENTICATED.DestNode = _nodeInfo.Node;
                    _supportedS2_UNAUTHENTICATED.DestNode = _nodeInfo.Node;

                    if (res.CommandClasses == null ||
                        !res.CommandClasses.Contains(COMMAND_CLASS_SECURITY_2.ID))
                    {
                        _supportedS2_ACCESS.SetCancelled();
                        _supportedS2_AUTHENTICATED.SetCancelled();
                        _supportedS2_UNAUTHENTICATED.SetCancelled();
                    }
                    else if (!_isInclusionTask && _securityManagerInfo.Network.HasSecurityScheme(SecuritySchemeSet.ALLS2))
                    {
                        _securityManagerInfo.Network.ResetSecuritySchemes(_nodeInfo.Node);
                        _securityManagerInfo.Network.SetSecuritySchemes(_nodeInfo.Node, SecuritySchemeSet.ALL);
                    }

                    if (res.CommandClasses == null ||
                        !res.CommandClasses.Contains(COMMAND_CLASS_SECURITY.ID))
                    {
                        _supportedS0.SetCancelled();
                    }
                    else if (!_isInclusionTask && _securityManagerInfo.Network.HasSecurityScheme(SecuritySchemes.S0))
                    {
                        var schs = _securityManagerInfo.Network.GetSecuritySchemes(_nodeInfo.Node);
                        _securityManagerInfo.Network.ResetSecuritySchemes(_nodeInfo.Node);
                        if (schs != null)
                        {
                            _securityManagerInfo.Network.SetSecuritySchemes(_nodeInfo.Node, SecuritySchemeSet.ALL);
                        }
                        else
                        {
                            _securityManagerInfo.Network.SetSecuritySchemes(_nodeInfo.Node, SecuritySchemeSet.S0);
                        }
                    }
                }
                else
                {
                    _allowFailed = false;
                    _supportedS0.SetCancelled();
                    _supportedS2_ACCESS.SetCancelled();
                    _supportedS2_AUTHENTICATED.SetCancelled();
                    _supportedS2_UNAUTHENTICATED.SetCancelled();
                }
            }
            else if (ou.Action.Result is RequestDataResult)
            {
                var res = (RequestDataResult)ou.Action.Result;
                if (res.State == ActionStates.Completed)
                {
                    if (SpecificResult.SecuritySchemes == null)
                    {
                        SpecificResult.SecuritySchemes = new[] { res.RxSecurityScheme };
                    }
                    else
                    {
                        SpecificResult.SecuritySchemes = SpecificResult.SecuritySchemes.Union(new[] { res.RxSecurityScheme }).ToArray();
                    }
                    if (_isInclusionTask)
                    {
                        switch (res.RxSecurityScheme)
                        {
                            case SecuritySchemes.NONE:
                                break;
                            case SecuritySchemes.S2_UNAUTHENTICATED:
                                _supportedS0.SetCancelled();
                                break;
                            case SecuritySchemes.S2_AUTHENTICATED:
                                _supportedS2_UNAUTHENTICATED.SetCancelled();
                                _supportedS0.SetCancelled();
                                break;
                            case SecuritySchemes.S2_ACCESS:
                                _supportedS2_AUTHENTICATED.SetCancelled();
                                _supportedS2_UNAUTHENTICATED.SetCancelled();
                                _supportedS0.SetCancelled();
                                break;
                            case SecuritySchemes.S0:
                                break;
                            default:
                                break;
                        }
                    }
                }

                if (ou.Action is RequestDataExOperation)
                {
                    var reqExAction = (RequestDataExOperation)ou.Action;
                    if (reqExAction.Result.State == ActionStates.Expired && !reqExAction.IsSendDataCompleted())
                    {
                        if (_supportedS2_ACCESS.Token.IsStateActive)
                        {
                            _supportedS2_ACCESS.SetCancelled();
                        }
                        if (_supportedS2_AUTHENTICATED.Token.IsStateActive)
                        {
                            _supportedS2_AUTHENTICATED.SetCancelled();
                        }
                        if (_supportedS2_UNAUTHENTICATED.Token.IsStateActive)
                        {
                            _supportedS2_UNAUTHENTICATED.SetCancelled();
                        }
                    }
                }

                if (!_isInclusionTask)
                {
                    if ((ou.Action is RequestDataExOperation) && _lastProbingOperation.Equals((RequestDataExOperation)ou.Action))
                    {
                        _securityManagerInfo.Network.SetSecuritySchemesSpecified(_nodeInfo.Node);
                        var schemes = GetProbingResults(out byte[] secureCommandClasses);
                        if (schemes.Length > 0)
                        {
                            _securityManagerInfo.Network.SetSecuritySchemes(_nodeInfo.Node, schemes.ToArray());
                            _securityManagerInfo.Network.SetSecureCommandClasses(_nodeInfo.Node, secureCommandClasses);
                            SpecificResult.SecureCommandClasses = secureCommandClasses;
                            if (SecuritySchemeSet.ALLS2.Contains(schemes[0]))
                            {
                                var peerNodeId = new InvariantPeerNodeId(_securityManagerInfo.Network.NodeTag, _nodeInfo.Node);
                                _securityManagerInfo.ActivateNetworkKeyS2ForNode(peerNodeId, schemes[0], 
                                    _network.IsLongRange(_nodeInfo.Node) && _network.IsLongRangeEnabled(_nodeInfo.Node));
                            }
                            else
                            {
                                _zwavePlusInfo.SetCancelled();
                            }
                        }
                        else
                        {
                            _zwavePlusInfo.SetCancelled();
                            _securityManagerInfo.Network.ResetSecuritySchemes(_nodeInfo.Node);
                            SpecificResult.SecureCommandClasses = null;
                        }
                    }
                    else if (ou.Action is RequestDataOperation)
                    {
                        var reqAction = (RequestDataOperation)ou.Action;
                        var requestRes = reqAction.SpecificResult;
                        if (requestRes.Command != null &&
                            requestRes.Command.Length > 2 &&
                            requestRes.Command[0] == COMMAND_CLASS_ZWAVEPLUS_INFO.ID &&
                            requestRes.Command[1] == COMMAND_CLASS_ZWAVEPLUS_INFO.ZWAVEPLUS_INFO_REPORT.ID)
                        {
                            var report = (COMMAND_CLASS_ZWAVEPLUS_INFO.ZWAVEPLUS_INFO_REPORT)requestRes.Command;
                            _securityManagerInfo.Network.SetRoleType(_nodeInfo.Node, (RoleTypes)report.roleType.Value);
                            _securityManagerInfo.Network.SetNodeType(_nodeInfo.Node, (NodeTypes)report.nodeType.Value);
                        }
                    }
                }
            }
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            base.SetStateCompleted(ou);
            if (_isInclusionTask)
            {
                _securityManagerInfo.Network.SetSecuritySchemesSpecified(_nodeInfo.Node);
                var schemes = GetProbingResults(out byte[] secureCommandClasses);
                if (schemes.Length > 0)
                {
                    _securityManagerInfo.Network.SetSecureCommandClasses(_nodeInfo.Node, secureCommandClasses);
                    SpecificResult.SecureCommandClasses = secureCommandClasses;
                    if (SecuritySchemeSet.ALLS2.Contains(schemes[0]))
                    {
                        var peerNodeId = new InvariantPeerNodeId(_securityManagerInfo.Network.NodeTag, _nodeInfo.Node);
                        _securityManagerInfo.ActivateNetworkKeyS2ForNode(peerNodeId, schemes[0],
                            _network.IsLongRange(_nodeInfo.Node) && _network.IsLongRangeEnabled(_nodeInfo.Node));
                    }
                }
            }
        }

        private SecuritySchemes[] GetProbingResults(out byte[] secureCommandClasses)
        {
            secureCommandClasses = null;
            var schemes = new List<SecuritySchemes>();
            if (_supportedS2_ACCESS.IsSendDataCompleted())
            {
                if (_supportedS2_ACCESS.Result)
                {
                    if (_supportedS2_ACCESS.SpecificResult.Command != null)
                    {
                        var commandClasses = ((COMMAND_CLASS_SECURITY_2.SECURITY_2_COMMANDS_SUPPORTED_REPORT)_supportedS2_ACCESS.SpecificResult.Command).
                            commandClass.TakeWhile(x => x != 0xEF).ToArray();
                        if (commandClasses.Length > 0)
                        {
                            secureCommandClasses = commandClasses;
                        }
                    }
                    schemes.Add(SecuritySchemes.S2_ACCESS);
                }
            }
            else if (Actions.Contains(_supportedS2_ACCESS))
            {
                _supportedS2_AUTHENTICATED.SetCancelled();
                _supportedS2_UNAUTHENTICATED.SetCancelled();
            }

            if (_supportedS2_AUTHENTICATED.IsSendDataCompleted())
            {
                if (_supportedS2_AUTHENTICATED.Result)
                {
                    if (_supportedS2_AUTHENTICATED.SpecificResult.Command != null)
                    {
                        var commandClasses = ((COMMAND_CLASS_SECURITY_2.SECURITY_2_COMMANDS_SUPPORTED_REPORT)_supportedS2_AUTHENTICATED.SpecificResult.Command).
                            commandClass.TakeWhile(x => x != 0xEF).ToArray();
                        if (commandClasses.Length > 0)
                        {
                            secureCommandClasses = commandClasses;
                        }
                    }
                    schemes.Add(SecuritySchemes.S2_AUTHENTICATED);
                }
                else if (_supportedS2_AUTHENTICATED.Result.State == ActionStates.Cancelled &&
                        _securityManagerInfo.Network.HasSecurityScheme(_nodeInfo.Node, SecuritySchemes.S2_AUTHENTICATED))
                {
                    schemes.Add(SecuritySchemes.S2_AUTHENTICATED);
                }
            }
            else if (Actions.Contains(_supportedS2_AUTHENTICATED))
            {
                _supportedS2_UNAUTHENTICATED.SetCancelled();
            }

            if (_supportedS2_UNAUTHENTICATED.IsSendDataCompleted())
            {
                if (_supportedS2_UNAUTHENTICATED.Result)
                {
                    if (_supportedS2_UNAUTHENTICATED.SpecificResult.Command != null)
                    {
                        var commandClasses = ((COMMAND_CLASS_SECURITY_2.SECURITY_2_COMMANDS_SUPPORTED_REPORT)_supportedS2_UNAUTHENTICATED.SpecificResult.Command).
                            commandClass.TakeWhile(x => x != 0xEF).ToArray();
                        if (commandClasses.Length > 0)
                        {
                            secureCommandClasses = commandClasses;
                        }
                    }
                    schemes.Add(SecuritySchemes.S2_UNAUTHENTICATED);
                }
                else if (_supportedS2_UNAUTHENTICATED.Result.State == ActionStates.Cancelled &&
                    _securityManagerInfo.Network.HasSecurityScheme(_nodeInfo.Node, SecuritySchemes.S2_UNAUTHENTICATED))
                {
                    schemes.Add(SecuritySchemes.S2_UNAUTHENTICATED);
                }
            }

            if (_supportedS0.IsSendDataCompleted())
            {
                if (_supportedS0.Result)
                {
                    if (_supportedS0.SpecificResult.Command != null)
                    {
                        var commandClasses = ((COMMAND_CLASS_SECURITY.SECURITY_COMMANDS_SUPPORTED_REPORT)_supportedS0.SpecificResult.Command).
                            commandClassSupport.TakeWhile(x => x != 0xEF).ToArray();
                        if (commandClasses.Length > 0)
                        {
                            secureCommandClasses = commandClasses;
                        }
                    }
                    schemes.Add(SecuritySchemes.S0);
                }
            }

            return schemes.ToArray();
        }
    }
}
