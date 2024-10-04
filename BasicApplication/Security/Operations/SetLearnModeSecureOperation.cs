/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using ZWave.BasicApplication.Security;
using ZWave.CommandClasses;
using ZWave.Devices;
using ZWave.Enums;
using ZWave.Security;

namespace ZWave.BasicApplication.Operations
{
    public class SetLearnModeSecureOperation : ApiOperation
    {
        private ApiOperation _learnMode;
        private SecurityManagerInfo _securityManagerInfo;
        private readonly Action _resetSecurityCallback;
        private readonly int _timeoutMs;
        private readonly byte[] _previousHomeId;
        private readonly NodeTag _previousNode;

        public SetLearnModeSecureOperation(NetworkViewPoint network, SecurityManagerInfo securityManagerInfo, SetLearnModeEndDeviceOperation learnMode, Action resetSecurityCallback)
           : this(network, securityManagerInfo, learnMode, resetSecurityCallback, learnMode.TimeoutMs)
        {
        }

        public SetLearnModeSecureOperation(NetworkViewPoint network, SecurityManagerInfo securityManagerInfo, SetLearnModeControllerOperation learnMode, Action resetSecurityCallback)
            : this(network, securityManagerInfo, learnMode, resetSecurityCallback, learnMode.TimeoutMs)
        {
        }

        public SetLearnModeSecureOperation(NetworkViewPoint network, SecurityManagerInfo securityManagerInfo, SetVirtualDeviceLearnModeOperation learnMode, Action resetSecurityCallback)
            : this(network, securityManagerInfo, learnMode, resetSecurityCallback, learnMode.TimeoutMs)
        {
        }

        private SetLearnModeSecureOperation(NetworkViewPoint network, SecurityManagerInfo securityManagerInfo, ApiOperation learnMode, Action resetSecurityCallback, int timeoutMs)
            : base(false, learnMode.SerialApiCommands, false)
        {
            _network = network;
            _timeoutMs = timeoutMs;
            _securityManagerInfo = securityManagerInfo;
            _learnMode = learnMode;
            _resetSecurityCallback = resetSecurityCallback;
            _previousHomeId = _securityManagerInfo.Network.HomeId.ToArray();
            _previousNode = _securityManagerInfo.Network.NodeTag;
        }

        private ExpectDataOperation _expectSchemeGet;
        private ExpectDataOperation _expectKexGet;
        private SetLearnModeS0Operation _learnModeS0;
        private SetLearnModeS2Operation _learnModeS2;
        private MemoryGetIdOperation _memoryGetId;
        private SerialApiGetInitDataOperation _serialApiGetInitData;

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(OnStart, 0));
            ActionUnits.Add(new ActionCompletedUnit(_learnMode, null, _memoryGetId));
            ActionUnits.Add(new ActionCompletedUnit(_memoryGetId, OnMemoryGetIdCompleted, _serialApiGetInitData));
            ActionUnits.Add(new ActionCompletedUnit(_serialApiGetInitData, OnLearnModeCompleted));
            ActionUnits.Add(new ActionCompletedUnit(_expectSchemeGet, OnExpectSchemeGetCompleted));
            ActionUnits.Add(new ActionCompletedUnit(_expectKexGet, OnExpectKexGetCompleted));
            ActionUnits.Add(new ActionCompletedUnit(_learnModeS0, OnLearnModeS0Completed));
            ActionUnits.Add(new ActionCompletedUnit(_learnModeS2, OnLearnModeS2Completed));
        }

        protected override void CreateInstance()
        {
            _expectSchemeGet = new ExpectDataOperation(_network, NodeTag.Empty, NodeTag.Empty, new COMMAND_CLASS_SECURITY.SECURITY_SCHEME_GET(), 2, _timeoutMs * 2)
            {
                Name = "Expect SCHEME_GET",
                IgnoreRxStatuses = ReceiveStatuses.TypeMulti | ReceiveStatuses.TypeBroad
            };

            _expectKexGet = new ExpectDataOperation(_network, NodeTag.Empty, NodeTag.Empty, new COMMAND_CLASS_SECURITY_2.KEX_GET(), 2, _timeoutMs * 2)
            {
                Name = "Expect KEX_GET",
                IgnoreRxStatuses = ReceiveStatuses.TypeMulti | ReceiveStatuses.TypeBroad
            };
            _memoryGetId = new MemoryGetIdOperation(_network);
            _serialApiGetInitData = new SerialApiGetInitDataOperation();
            _learnModeS2 = new SetLearnModeS2Operation(_network, _securityManagerInfo);
            _learnModeS0 = new SetLearnModeS0Operation(_network, _securityManagerInfo, NodeTag.Empty, 0);
        }

        private void OnStart(StartActionUnit unit)
        {
            List<ActionBase> nextItems = new List<ActionBase>(5);
            if (_securityManagerInfo.Network.IsEnabledS0)
            {
                nextItems.Add(_expectSchemeGet);
            }
            if (_securityManagerInfo.Network.IsEnabledS2_ACCESS ||
                _securityManagerInfo.Network.IsEnabledS2_AUTHENTICATED ||
                _securityManagerInfo.Network.IsEnabledS2_UNAUTHENTICATED)
            {
                nextItems.Add(_expectKexGet);
            }
            nextItems.Add(_learnMode);
            unit.SetNextActionItems(nextItems.ToArray());
        }

        private void OnMemoryGetIdCompleted(ActionCompletedUnit unit)
        {
            if (_memoryGetId.Result)
            {
                _network.NodeTag = _memoryGetId.SpecificResult.Node;
                _network.HomeId = _memoryGetId.SpecificResult.HomeId;
                var lmc = _learnMode as SetLearnModeControllerOperation;
                var lmv = _learnMode as SetVirtualDeviceLearnModeOperation;
                var lms = _learnMode as SetLearnModeEndDeviceOperation;

                if (lmc != null && _memoryGetId.SpecificResult.HomeId.SequenceEqual(_previousHomeId) &&
                    lmc.SpecificResult.Node == _previousNode &&
                    (lmc.Mode & LearnModes.LearnModeSmartStart) != LearnModes.LearnModeSmartStart)
                {
                    SpecificResult.LearnModeStatus = LearnModeStatuses.Replicated;
                }
                else if ((lmc != null && (lmc.SpecificResult.Node.Id == 0 || lmc.SpecificResult.Node.Id == 1) && (lmc.Mode & LearnModes.LearnModeSmartStart) != LearnModes.LearnModeSmartStart) ||
                    (lmv != null && lmv.SpecificResult.Node.Id == 0))
                {
                    SpecificResult.LearnModeStatus = LearnModeStatuses.Removed;
                }
                else
                {
                    SpecificResult.LearnModeStatus = LearnModeStatuses.Added;
                }

                if (lmc != null)
                {
                    SpecificResult.Node = _memoryGetId.SpecificResult.Node;
                }
                else if (lmv != null)
                {
                    SpecificResult.Node = lmv.SpecificResult.Node;
                }
                else if (lms != null)
                {
                    SpecificResult.Node = _memoryGetId.SpecificResult.Node;
                }
            }
        }

        private void OnExpectSchemeGetCompleted(ActionCompletedUnit unit)
        {
            if (_learnMode.Result && _memoryGetId.Result && _serialApiGetInitData.Result)
            {
                if (_expectSchemeGet.Result)
                {
                    _expectKexGet.SetCancelled();
                    _learnModeS0.Node = _expectSchemeGet.SpecificResult.SrcNode;
                    if (_learnMode is SetVirtualDeviceLearnModeOperation)
                    {
                        _learnModeS0.VirtualNode = _expectSchemeGet.SpecificResult.DestNode;
                    }
                    else
                    {
                        _learnModeS0.IsController = _learnMode is SetLearnModeControllerOperation;
                        _securityManagerInfo.Network.ResetAndEnableAndSelfRestore();
                    }
                    COMMAND_CLASS_SECURITY.SECURITY_SCHEME_GET cmd = _expectSchemeGet.SpecificResult.Command;
                    _learnModeS0.SupportedSecuritySchemes = cmd.supportedSecuritySchemes;
                    unit.SetNextActionItems(_learnModeS0);
                }
                else if (_expectSchemeGet.Result.State == ActionStates.Expired)
                {
                    SpecificResult.SubstituteStatus = SubstituteStatuses.Failed;
                    _securityManagerInfo.Network.ResetSecuritySchemes();
                    _securityManagerInfo.Network.ResetSecuritySchemes(_expectSchemeGet.SpecificResult.SrcNode);
                    SetStateCompleted(unit);
                }
            }
        }

        bool isFirstSmartStart = true; // SWPROT-2854 workaround 
        private void OnExpectKexGetCompleted(ActionCompletedUnit unit)
        {
            var lmc = _learnMode as SetLearnModeControllerOperation;
            var lms = _learnMode as SetLearnModeEndDeviceOperation;
            var mode = lmc != null ? lmc.Mode : lms != null ? lms.Mode : LearnModes.LearnModeDisable;
            if (!isFirstSmartStart || (mode & LearnModes.LearnModeSmartStart) != LearnModes.LearnModeSmartStart)
            {
                if (/*_learnMode.Result &&*/ _memoryGetId.Result && _serialApiGetInitData.Result)
                {
                    if (_expectKexGet.Result)
                    {
                        _expectSchemeGet.SetCancelled();
                        _learnModeS2.Node = _expectKexGet.SpecificResult.SrcNode;
                        if (_learnMode is SetVirtualDeviceLearnModeOperation)
                        {
                            _learnModeS2.VirtualNode = _expectKexGet.SpecificResult.DestNode;
                        }
                        else
                        {
                            _securityManagerInfo.Network.ResetAndEnableAndSelfRestore();
                        }
                        unit.SetNextActionItems(_learnModeS2);
                    }
                    else if (_expectKexGet.Result.State == ActionStates.Expired)
                    {
                        SpecificResult.SubstituteStatus = SubstituteStatuses.Failed;
                        _securityManagerInfo.Network.ResetSecuritySchemes();
                        _securityManagerInfo.Network.ResetSecuritySchemes(_expectKexGet.SpecificResult.SrcNode);
                        SetStateCompleted(unit);
                    }
                }
            }
            else
            {
                _memoryGetId.NewToken();
                _serialApiGetInitData.NewToken();
                unit.SetNextActionItems(_memoryGetId);
                isFirstSmartStart = false;
            }
        }

        private void OnLearnModeCompleted(ActionCompletedUnit unit)
        {
            if (/*_learnMode.Result disabled until end device smart start callback fixed &&*/ _memoryGetId.Result && _serialApiGetInitData.Result)
            {
                if (_expectSchemeGet.Result)
                {
                    OnExpectSchemeGetCompleted(unit);
                }
                else if (_expectKexGet.Result)
                {
                    OnExpectKexGetCompleted(unit);
                }
                else
                {
                    switch (SpecificResult.LearnModeStatus)
                    {
                        case LearnModeStatuses.None:
                            break;
                        case LearnModeStatuses.Added:
                            if (_securityManagerInfo.Network.IsEnabledS2_ACCESS | _securityManagerInfo.Network.IsEnabledS2_AUTHENTICATED | _securityManagerInfo.Network.IsEnabledS2_UNAUTHENTICATED)
                            {
                                unit.AddNextActionItems(new TimeInterval(0, _expectKexGet.Id, InclusionS2TimeoutConstants.Joining.KexGet));
                            }
                            else if (_securityManagerInfo.Network.IsEnabledS0)
                            {
                                unit.AddNextActionItems(new TimeInterval(0, _expectSchemeGet.Id, InclusionS2TimeoutConstants.Joining.PublicKeyReport));
                            }
                            else
                            {
                                SetStateCompleted(unit);
                            }
                            break;
                        case LearnModeStatuses.Removed:
                            _expectSchemeGet.SetCancelled();
                            _expectKexGet.SetCancelled();
                            if (!(_learnMode is SetVirtualDeviceLearnModeOperation))
                            {
                                _resetSecurityCallback();
                            }
                            SetStateCompleted(unit);
                            break;
                        case LearnModeStatuses.Changed:
                            break;
                        case LearnModeStatuses.Replicated:
                            _expectSchemeGet.SetCancelled();
                            _expectKexGet.SetCancelled();
                            SetStateCompleted(unit);
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                SetStateFailed(unit);
            }
        }

        private void OnLearnModeS0Completed(ActionCompletedUnit unit)
        {
            SetStateCompleted(unit);
        }

        private void OnLearnModeS2Completed(ActionCompletedUnit unit)
        {
            SetStateCompleted(unit);
        }

        public SetLearnModeResult SpecificResult
        {
            get { return (SetLearnModeResult)Result; }
        }

        public override string AboutMe()
        {
            if (Result as SetLearnModeResult != null)
            {
                return string.Format("Id={0}, Security={1}", SpecificResult.Node.Id, SpecificResult.SubstituteStatus);
            }
            else
            {
                return "";
            }
        }
    }
}
