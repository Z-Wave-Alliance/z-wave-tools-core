/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Utils;
using ZWave.BasicApplication.Enums;
using ZWave.BasicApplication.Security;
using ZWave.CommandClasses;
using ZWave.Devices;
using ZWave.Enums;
using ZWave.Security;
using ZWave.Security.S2;

namespace ZWave.BasicApplication.Operations
{
    public class AddNodeS2Operation : ApiOperation
    {
        private SecurityManagerInfo _securityManagerInfo;
        private NetworkKeyS2Flags _lastGrantedKey;
        private readonly ISecurityTestSettingsService _securityTestSettingsService;

        internal AddNodeS2Operation(NetworkViewPoint network, SecurityManagerInfo securityManagerInfo)
            : base(false, null, false)
        {
            _network = network;
            _securityManagerInfo = securityManagerInfo;
            _securityTestSettingsService = new SecurityTestSettingsService(_securityManagerInfo, true);
        }

        COMMAND_CLASS_SECURITY_2.KEX_REPORT KEX_REPORT = null;

        private bool _isClientSideAuthRequested = false;
        private bool _isClientSideAuthGranted = false;
        private bool _isSkipDskCallback = false;
        private bool _isWaitingForKexSetEcho = false;
        private InvariantPeerNodeId _peerNodeId;

        RequestDataOperation _KEXGetKEXReport;
        RequestDataOperation _KEXSetPKReport; // Receive and load tmp key.
        SendDataOperation _PKReport;
        ExpectDataOperation _KEXSetEcho;
        SendDataOperation _KEXReportEcho;
        RequestDataOperation _NKReportNKVerify; // Send and load real key, receive and load tmp key.
        SendDataOperation _TransferEnd;
        ExpectDataOperation _OnNKGet;
        ExpectDataOperation _OnTransferEnd; // Receive and load real key.
        SendDataOperation _KexFail;
        SendDataOperation _KexFailCancel;
        ResponseDataOperation _KexFailReceived;
        ResponseDataOperation _SecurityMessageReceived;
        DateTime _KexSetEchoWaitStartedAt = DateTime.MinValue;

        private bool IsOnTransferEndCancelled;

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(OnAddNodeCompleted, 775123, _KEXGetKEXReport, _KexFailReceived, _SecurityMessageReceived));
            ActionUnits.Add(new ActionCompletedUnit(_KEXGetKEXReport, OnKEXReport, _KEXSetPKReport));
            ActionUnits.Add(new ActionCompletedUnit(_KEXSetPKReport, OnReceivePKReport, _KEXSetEcho, _PKReport));
            ActionUnits.Add(new ActionCompletedUnit(_PKReport, OnSendPKReport));
            ActionUnits.Add(new ActionCompletedUnit(_KEXSetEcho, (Action<ActionCompletedUnit>)OnKEXSetEcho, _OnNKGet, _OnTransferEnd, _KEXReportEcho));
            ActionUnits.Add(new ActionCompletedUnit(_KEXReportEcho, (Action<ActionCompletedUnit>)OnKEXReportEcho));
            ActionUnits.Add(new ActionCompletedUnit(_NKReportNKVerify, (Action<ActionCompletedUnit>)OnNKVerify, _OnNKGet, _OnTransferEnd, _TransferEnd));
            ActionUnits.Add(new ActionCompletedUnit(_OnNKGet, OnNKGetNextKey));
            ActionUnits.Add(new ActionCompletedUnit(_OnTransferEnd, OnTransferEnd));
            ActionUnits.Add(new ActionCompletedUnit(_KexFail, OnKexFail));
            ActionUnits.Add(new ActionCompletedUnit(_KexFailReceived, OnKexFailReceived));
            ActionUnits.Add(new ActionCompletedUnit(_SecurityMessageReceived, OnSecurityMessageReceived));
            ActionUnits.Add(new ActionCompletedUnit(_KexFailCancel, OnKexFail));
            StopActionUnit = new StopActionUnit(_KexFailCancel);
        }

        protected override void CreateInstance()
        {
            _KexFailReceived = new ResponseDataOperation(_network, OnKexFailCallback, TransmitOptions.TransmitOptionNone, NodeTag.Empty,
                COMMAND_CLASS_SECURITY_2.ID,
                COMMAND_CLASS_SECURITY_2.KEX_FAIL.ID);

            _SecurityMessageReceived = new ResponseDataOperation(_network, OnSecurityMessageCallback, TransmitOptions.TransmitOptionNone, NodeTag.Empty,
                COMMAND_CLASS_SECURITY_2.ID,
                COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID);

            _KEXGetKEXReport = new RequestDataOperation(_network, NodeTag.Empty, NodeTag.Empty,
                null, _securityManagerInfo.TxOptions,
                new ByteIndex[] { COMMAND_CLASS_SECURITY_2.ID, COMMAND_CLASS_SECURITY_2.KEX_REPORT.ID, new ByteIndex(0x00, 0x01) },
                InclusionS2TimeoutConstants.Including.KexReport);
            _KEXGetKEXReport.Name = "RequestData KEX_GET/KEX_REPORT";
            _KEXGetKEXReport.SubstituteSettings.SetFlag(SubstituteFlags.DenySecurity);
            _KEXGetKEXReport.IgnoreRxStatuses = ReceiveStatuses.TypeMulti | ReceiveStatuses.TypeBroad;

            _KEXSetPKReport = new RequestDataOperation(_network, NodeTag.Empty, NodeTag.Empty,
                null, _securityManagerInfo.TxOptions,
                new COMMAND_CLASS_SECURITY_2.PUBLIC_KEY_REPORT(), 2,
                InclusionS2TimeoutConstants.Including.PublicKeyReport);
            _KEXSetPKReport.Name = "RequestData KEX_SET/PUBLIC_KEY_REPORT";
            _KEXSetPKReport.SubstituteSettings.SetFlag(SubstituteFlags.DenySecurity);
            _KEXSetPKReport.IgnoreRxStatuses = ReceiveStatuses.TypeMulti | ReceiveStatuses.TypeBroad;

            _PKReport = new SendDataOperation(_network, NodeTag.Empty, null, _securityManagerInfo.TxOptions);
            _PKReport.Name = "SendData PUBLIC_KEY_REPORT";
            _PKReport.SubstituteSettings.SetFlag(SubstituteFlags.DenySecurity);

            _KEXSetEcho = new ExpectDataOperation(_network, NodeTag.Empty, NodeTag.Empty,
                new COMMAND_CLASS_SECURITY_2.KEX_SET(), 2,
                InclusionS2TimeoutConstants.Including.UserInputKeyList);
            _KEXSetEcho.Name = "ExpectData KEX_SET(echo)";
            _KEXSetEcho.IgnoreRxStatuses = ReceiveStatuses.TypeMulti | ReceiveStatuses.TypeBroad;

            _KEXReportEcho = new SendDataOperation(_network, NodeTag.Empty, null, _securityManagerInfo.TxOptions);
            _KEXReportEcho.Name = "SendData KEX_REPORT(echo)";
            _KEXReportEcho.SubstituteSettings.SetFlag(SubstituteFlags.UseSecurity);

            _NKReportNKVerify = new RequestDataOperation(_network, NodeTag.Empty, NodeTag.Empty,
                null, _securityManagerInfo.TxOptions,
                new COMMAND_CLASS_SECURITY_2.SECURITY_2_NETWORK_KEY_VERIFY(), 2,
                InclusionS2TimeoutConstants.Including.NetworkKeyVerify);
            _NKReportNKVerify.Name = "RequestData NETWORK_KEY_REPORT/NETWORK_KEY_VERIFY";
            _NKReportNKVerify.SubstituteSettings.SetFlag(SubstituteFlags.UseSecurity);
            _NKReportNKVerify.IgnoreRxStatuses = ReceiveStatuses.TypeMulti | ReceiveStatuses.TypeBroad;

            _TransferEnd = new SendDataOperation(_network, NodeTag.Empty, null, _securityManagerInfo.TxOptions);
            _TransferEnd.SubstituteSettings.SetFlag(SubstituteFlags.UseSecurity);
            _TransferEnd.Name = "SendData TRANSFER_END";

            _OnNKGet = new ExpectDataOperation(_network, NodeTag.Empty, NodeTag.Empty, new COMMAND_CLASS_SECURITY_2.SECURITY_2_NETWORK_KEY_GET(), 2, InclusionS2TimeoutConstants.Including.NetworkKeyGet);
            _OnNKGet.Name = "ExpectData NETWORK_KEY_GET";
            _OnNKGet.IgnoreRxStatuses = ReceiveStatuses.TypeMulti | ReceiveStatuses.TypeBroad;

            _OnTransferEnd = new ExpectDataOperation(_network, NodeTag.Empty, NodeTag.Empty, new COMMAND_CLASS_SECURITY_2.SECURITY_2_TRANSFER_END(), 2, InclusionS2TimeoutConstants.Including.TransferEnd);
            _OnTransferEnd.Name = "ExpectData TRANSFER_END";
            _OnTransferEnd.IgnoreRxStatuses = ReceiveStatuses.TypeMulti | ReceiveStatuses.TypeBroad;

            _KexFail = new SendDataOperation(_network, NodeTag.Empty, null, _securityManagerInfo.TxOptions);
            _KexFail.Name = "SendData KEX_FAIL";
            _KexFail.SubstituteSettings.SetFlag(SubstituteFlags.DenySecurity);

            _KexFailCancel = new SendDataOperation(_network, NodeTag.Empty, null, _securityManagerInfo.TxOptions);
            _KexFailCancel.Name = "SendData KEX_FAIL_CANCEL";
            _KexFailCancel.SubstituteSettings.SetFlag(SubstituteFlags.DenySecurity);
            _KexFailCancel.Data = new COMMAND_CLASS_SECURITY_2.KEX_FAIL() { kexFailType = 0x06 };
        }

        protected void SetStateCompletedSecurityFailed(IActionUnit ou)
        {
            SpecificResult.SubstituteStatus = SubstituteStatuses.Failed;
            SpecificResult.SecuritySchemes = null;
            _securityManagerInfo.Network.ResetSecuritySchemes(SpecificResult.Node);
            _securityManagerInfo.Network.SetSecuritySchemesSpecified(SpecificResult.Node);
            _securityManagerInfo.IsInclusion = false;
            SetStateCompleted(ou);
        }

        protected void SetStateCompletedSecurityDone(IActionUnit ou)
        {
            SpecificResult.SubstituteStatus = SubstituteStatuses.Done;
            if (_transferedSchemes.Count > 0)
            {
                SpecificResult.SecuritySchemes = _transferedSchemes.ToArray();
                _securityManagerInfo.Network.SetSecuritySchemes(SpecificResult.Id, _transferedSchemes.ToArray());
                _securityManagerInfo.Network.SetSecuritySchemesSpecified(SpecificResult.Node);
                _securityManagerInfo.ActivateNetworkKeyS2ForNode(_peerNodeId, _securityManagerInfo.Network.GetCurrentOrSwitchToHighestSecurityScheme(SpecificResult.Node), _network.IsLongRangeEnabled(SpecificResult.Node));
            }
            else
            {
                SpecificResult.SecuritySchemes = _securityManagerInfo.Network.GetSecuritySchemes(SpecificResult.Id);
            }
            _securityManagerInfo.IsInclusion = false;
            SetStateCompleted(ou);
        }

        protected void SetStateCompletedSecurityNone(IActionUnit ou)
        {
            SpecificResult.SubstituteStatus = SubstituteStatuses.None;
            SpecificResult.SecuritySchemes = null;
            _securityManagerInfo.Network.ResetSecuritySchemes(SpecificResult.Node);
            _securityManagerInfo.Network.SetSecuritySchemesSpecified(SpecificResult.Node);
            _securityManagerInfo.IsInclusion = false;
            SetStateCompleted(ou);
        }

        protected override void SetStateFailed(IActionUnit ou)
        {
            _securityManagerInfo.IsInclusion = false;
            base.SetStateFailed(ou);
        }

        private NodeTag _initiateNode;

        private void OnAddNodeCompletedInternal(StartActionUnit tu)
        {
            SpecificResult.Node = _initiateNode;
            _peerNodeId = new InvariantPeerNodeId(_securityManagerInfo.Network.NodeTag, SpecificResult.Node);

            var rniRes = SpecificResult.FindInnerResults<RequestNodeInfoResult>();
            if (rniRes != null && rniRes.Count > 0)
            {
                SpecificResult.CommandClasses = rniRes[0].CommandClasses;
                SpecificResult.IsEndDevice = rniRes[0].Basic > 2;
                SpecificResult.Basic = rniRes[0].Basic;
                SpecificResult.Generic = rniRes[0].Generic;
                SpecificResult.Specific = rniRes[0].Specific;
                _securityManagerInfo.Network.SetCommandClasses(new NodeTag(SpecificResult.Id), SpecificResult.CommandClasses);
            }
            SpecificResult.SubstituteStatus = SubstituteStatuses.Failed;
            _KEXGetKEXReport.DstNode = new NodeTag(SpecificResult.Id);
            _KEXGetKEXReport.Data = new COMMAND_CLASS_SECURITY_2.KEX_GET();

            #region KEXGet
            _securityTestSettingsService.ActivateTestPropertiesForFrame(SecurityS2TestFrames.KEXGet, _KEXGetKEXReport);
            #endregion
        }

        public void SetInclusionControllerInitiateParameters(NodeTag initiateNode)
        {
            _initiateNode = initiateNode;
        }

        private void OnAddNodeCompleted(StartActionUnit tu)
        {
            _securityManagerInfo.IsInclusion = true;
            if ((ParentAction as ActionSerialGroup) != null)
            {
                ActionSerialGroup actionGroup = (ActionSerialGroup)ParentAction;
                if (_initiateNode.Id > 0)
                {
                    _KexFailCancel.DstNode = _initiateNode;
                    OnAddNodeCompletedInternal(tu);
                }
                else
                {
                    ActionResult agRes = ParentAction.Result;
                    AddTraceLogItems(agRes.InnerResults[0].TraceLog);
                    if (agRes.InnerResults[0])
                    {
                        AddRemoveNodeResult arnRes = agRes.InnerResults[0] as AddRemoveNodeResult;
                        SpecificResult.Node = arnRes.Node;
                        SpecificResult.Basic = arnRes.Basic;
                        SpecificResult.Generic = arnRes.Generic;
                        SpecificResult.Specific = arnRes.Specific;
                        SpecificResult.AddRemoveNodeStatus = arnRes.AddRemoveNodeStatus;
                        if (arnRes.CommandClasses == null)
                        {
                            var rniRes = agRes.FindInnerResults<RequestNodeInfoResult>();
                            if (rniRes != null && rniRes.Count > 0)
                            {
                                SpecificResult.CommandClasses = rniRes[0].CommandClasses;
                                SpecificResult.IsEndDevice = rniRes[0].Basic > 2;
                            }
                        }
                        else
                        {
                            SpecificResult.CommandClasses = arnRes.CommandClasses;
                            SpecificResult.IsEndDevice = arnRes.IsEndDevice;
                        }

                        _securityManagerInfo.Network.SetCommandClasses(new NodeTag(SpecificResult.Id), SpecificResult.CommandClasses);

                        _peerNodeId = new InvariantPeerNodeId(_securityManagerInfo.Network.NodeTag, SpecificResult.Node);
                        _KexFailCancel.DstNode = new NodeTag(SpecificResult.Id);

                        if (actionGroup.Actions != null && actionGroup.Actions.Length > 0 && actionGroup.Actions[0] is ReplaceFailedNodeOperation)
                        {
                            _securityManagerInfo.Network.ResetSecuritySchemes(SpecificResult.Node);
                        }

                        if (SpecificResult.SubstituteStatus != SubstituteStatuses.Done &&
                            (!_securityManagerInfo.CheckIfSupportSecurityCC ||
                            (SpecificResult.CommandClasses != null && SpecificResult.CommandClasses.Contains(COMMAND_CLASS_SECURITY_2.ID))))
                        {
                            SpecificResult.tmpSkipS0 = true;
                            if (_securityManagerInfo.Network.HasSecurityScheme(SpecificResult.Id, SecuritySchemeSet.ALLS2) ||
                                arnRes.AddRemoveNodeStatus == AddRemoveNodeStatuses.Replicated ||
                                arnRes.AddRemoveNodeStatus == AddRemoveNodeStatuses.Shifted)
                            {
                                SetStateCompletedSecurityDone(tu);
                            }
                            else
                            {
                                SpecificResult.SubstituteStatus = SubstituteStatuses.Failed;
                                _KEXGetKEXReport.DstNode = new NodeTag(SpecificResult.Id);
                                _KEXGetKEXReport.Data = new COMMAND_CLASS_SECURITY_2.KEX_GET();

                                #region KEXGet
                                _securityTestSettingsService.ActivateTestPropertiesForFrame(SecurityS2TestFrames.KEXGet, _KEXGetKEXReport);
                                #endregion

                                #region InjectCommand
                                if (_securityManagerInfo.TestFramesS2.ContainsKey(SecurityS2TestFrames.InjectCommand))
                                {
                                    var sendInjectCommand = new SendDataOperation(_network, new NodeTag(SpecificResult.Id), null, _securityManagerInfo.TxOptions);
                                    _securityTestSettingsService.ActivateTestPropertiesForFrame(SecurityS2TestFrames.InjectCommand, sendInjectCommand);
                                    tu.SetNextActionItems(sendInjectCommand, _KEXGetKEXReport);
                                }
                                #endregion

                            }
                        }
                        else
                        {
                            SetStateCompletedSecurityNone(tu);
                        }
                    }
                    else
                        SetStateFailed(tu);
                }
            }
            else
                SetStateFailed(tu);
        }

        private List<SecuritySchemes> _grantedSecuritySchemes = new List<SecuritySchemes>();
        private List<SecuritySchemes> _transferedSchemes = new List<SecuritySchemes>();
        private Dictionary<SecuritySchemes, bool> _requestedSchemesDictionary = new Dictionary<SecuritySchemes, bool>();
        private void OnKEXReport(ActionCompletedUnit ou)
        {
            if (_KEXGetKEXReport.Result)
            {
                KEX_REPORT = _KEXGetKEXReport.SpecificResult.Command;
                if (KEX_REPORT.properties1.echo == 0 && KEX_REPORT.supportedKexSchemes == 0x02 && KEX_REPORT.supportedEcdhProfiles == 0x01)
                {
                    if (ValidateKexReportKeys())
                    {
                        NetworkKeyS2Flags requestedKeysFlags = (NetworkKeyS2Flags)KEX_REPORT.requestedKeys.Value;
                        SecuritySchemes requestedScheme;
                        byte grantedKeys = KEX_REPORT.requestedKeys;
                        if (requestedKeysFlags.HasFlag(NetworkKeyS2Flags.S2Class2))
                        {
                            requestedScheme = SecurityManagerInfo.ConvertToSecurityScheme(NetworkKeyS2Flags.S2Class2);
                            _grantedSecuritySchemes.Add(requestedScheme);
                            if (_securityManagerInfo.Network.HasSecurityScheme(SecuritySchemes.S2_ACCESS))
                            {
                                _requestedSchemesDictionary.Add(requestedScheme, false);
                            }
                            else
                            {
                                grantedKeys = (byte)(KEX_REPORT.requestedKeys & ~(byte)(NetworkKeyS2Flags.S2Class2));
                                _requestedSchemesDictionary.Add(requestedScheme, true);
                            }
                        }
                        if (requestedKeysFlags.HasFlag(NetworkKeyS2Flags.S2Class1))
                        {
                            requestedScheme = SecurityManagerInfo.ConvertToSecurityScheme(NetworkKeyS2Flags.S2Class1);
                            _grantedSecuritySchemes.Add(requestedScheme);
                            if (_securityManagerInfo.Network.HasSecurityScheme(SecuritySchemes.S2_AUTHENTICATED))
                            {
                                _requestedSchemesDictionary.Add(requestedScheme, false);
                            }
                            else
                            {
                                grantedKeys = (byte)(KEX_REPORT.requestedKeys & ~(byte)(NetworkKeyS2Flags.S2Class1));
                                _requestedSchemesDictionary.Add(requestedScheme, true);
                            }
                        }
                        if (!_network.IsLongRangeEnabled(SpecificResult.Node) &&
                            requestedKeysFlags.HasFlag(NetworkKeyS2Flags.S2Class0))
                        {
                            requestedScheme = SecurityManagerInfo.ConvertToSecurityScheme(NetworkKeyS2Flags.S2Class0);
                            _grantedSecuritySchemes.Add(requestedScheme);
                            if (_securityManagerInfo.Network.HasSecurityScheme(SecuritySchemes.S2_UNAUTHENTICATED))
                            {
                                _requestedSchemesDictionary.Add(requestedScheme, false);
                            }
                            else
                            {
                                grantedKeys = (byte)(KEX_REPORT.requestedKeys & ~(byte)(NetworkKeyS2Flags.S2Class0));
                                _requestedSchemesDictionary.Add(requestedScheme, true);
                            }
                        }
                        if (!_network.IsLongRangeEnabled(SpecificResult.Node) &&
                            requestedKeysFlags.HasFlag(NetworkKeyS2Flags.S0))
                        {
                            requestedScheme = SecurityManagerInfo.ConvertToSecurityScheme(NetworkKeyS2Flags.S0);
                            _grantedSecuritySchemes.Add(requestedScheme);
                            if (_securityManagerInfo.Network.HasSecurityScheme(SecuritySchemes.S0))
                            {
                                _requestedSchemesDictionary.Add(requestedScheme, false);
                            }
                            else
                            {
                                grantedKeys = (byte)(KEX_REPORT.requestedKeys & ~(byte)(NetworkKeyS2Flags.S0));
                                _requestedSchemesDictionary.Add(requestedScheme, true);
                            }
                        }

                        _KEXSetPKReport.DstNode = new NodeTag(SpecificResult.Id);
                        _isClientSideAuthRequested = KEX_REPORT.properties1.requestCsa > 0;
                        _isClientSideAuthGranted = _isClientSideAuthRequested;

                        bool requestedKeysConfirmed = false;
                        if (ParentAction is ActionSerialGroup &&
                            ((ActionSerialGroup)ParentAction).Actions[0] is AddNodeOperation &&
                            (((AddNodeOperation)(((ActionSerialGroup)(ParentAction)).Actions[0])).InitMode & Modes.NodeModeMask) == Modes.NodeHomeId)
                        {
                            requestedKeysConfirmed = true;

                            var grantSchemesSSf = (NetworkKeyS2Flags)((IAddRemoveNode)(((ActionSerialGroup)(ParentAction)).Actions[0])).GrantSchemesValue;

                            if (!_securityManagerInfo.Network.HasSecurityScheme(SecuritySchemes.S2_ACCESS) && _grantedSecuritySchemes.Contains(SecuritySchemes.S2_ACCESS) || !grantSchemesSSf.HasFlag(NetworkKeyS2Flags.S2Class2))
                            {
                                _grantedSecuritySchemes.Remove(SecuritySchemes.S2_ACCESS);
                            }
                            if (!_securityManagerInfo.Network.HasSecurityScheme(SecuritySchemes.S2_AUTHENTICATED) && _grantedSecuritySchemes.Contains(SecuritySchemes.S2_AUTHENTICATED) || !grantSchemesSSf.HasFlag(NetworkKeyS2Flags.S2Class1))
                            {
                                _grantedSecuritySchemes.Remove(SecuritySchemes.S2_AUTHENTICATED);
                            }
                            if (!_securityManagerInfo.Network.HasSecurityScheme(SecuritySchemes.S2_UNAUTHENTICATED) && _grantedSecuritySchemes.Contains(SecuritySchemes.S2_UNAUTHENTICATED) || !grantSchemesSSf.HasFlag(NetworkKeyS2Flags.S2Class0))
                            {
                                _grantedSecuritySchemes.Remove(SecuritySchemes.S2_UNAUTHENTICATED);
                            }
                            if (!_securityManagerInfo.Network.HasSecurityScheme(SecuritySchemes.S0) && _grantedSecuritySchemes.Contains(SecuritySchemes.S0) || !grantSchemesSSf.HasFlag(NetworkKeyS2Flags.S0))
                            {
                                _grantedSecuritySchemes.Remove(SecuritySchemes.S0);
                            }
                            grantedKeys = KEXSetConfirmResult.ConvertToNetworkKeyFlags(_grantedSecuritySchemes.ToArray());
                        }
                        else if (_securityManagerInfo.KEXSetConfirmCallback != null)
                        {
                            KEXSetConfirmResult res = _securityManagerInfo.KEXSetConfirmCallback(_grantedSecuritySchemes, _isClientSideAuthRequested);
                            requestedKeysConfirmed = res.IsConfirmed;
                            _isClientSideAuthGranted = res.IsAllowedCSA;
                            _grantedSecuritySchemes = res.GrantedSchemes;

                            if (_isClientSideAuthRequested && !_isClientSideAuthGranted)
                            {
                                _grantedSecuritySchemes.Remove(SecuritySchemes.S2_ACCESS);
                                _grantedSecuritySchemes.Remove(SecuritySchemes.S2_AUTHENTICATED);
                            }
                            if (_grantedSecuritySchemes.Count == 1 && _grantedSecuritySchemes.Any(i => i == SecuritySchemes.S0))
                                ((AddNodeS2Operation)ou.Action.ParentAction).SpecificResult.BootstrapingStatuses = BootstrapingStatuses.GrandSchemeCanceled;
                            grantedKeys = KEXSetConfirmResult.ConvertToNetworkKeyFlags(_grantedSecuritySchemes.ToArray());
                        }
                        else
                        {
                            _grantedSecuritySchemes.RemoveAll(scheme =>
                                _requestedSchemesDictionary
                                .Where(item => item.Value)
                                .Select(item => item.Key)
                                .Contains(scheme));
                            requestedKeysConfirmed = true; //For using inside our apps and operations.
                            grantedKeys = KEXSetConfirmResult.ConvertToNetworkKeyFlags(_grantedSecuritySchemes.ToArray());
                        }

                        SetIsSkipDskCallback();

                        if (requestedKeysConfirmed)
                        {
                            //Show Client-Side authentication message with sender DSK
                            if (_isClientSideAuthGranted && _securityManagerInfo.CsaPinCallback != null)
                            {
                                _securityManagerInfo.CsaPinCallback();
                            }

                            _KEXSetPKReport.Data = new COMMAND_CLASS_SECURITY_2.KEX_SET()
                            {
                                properties1 = new COMMAND_CLASS_SECURITY_2.KEX_SET.Tproperties1()
                                {
                                    requestCsa = _isClientSideAuthGranted ? (byte)1 : (byte)0
                                },
                                selectedKexScheme = 2,
                                selectedEcdhProfile = 1,
                                grantedKeys = grantedKeys
                            };
                            #region KEXSet
                            var dt = _KEXSetPKReport.Data;
                            _securityTestSettingsService.ActivateTestPropertiesForFrame(SecurityS2TestFrames.KEXSet, _KEXSetPKReport);
                            if (_KEXSetPKReport.Data.Length > 0 &&
                                _KEXSetPKReport.Data.Length == dt.Length &&
                                !_KEXSetPKReport.Data.SequenceEqual(dt))
                            {
                                var cmd = (COMMAND_CLASS_SECURITY_2.KEX_SET)_KEXSetPKReport.Data;
                                _isClientSideAuthGranted = cmd.properties1.requestCsa == 1;
                                NetworkKeyS2Flags grantedKeysMask = (NetworkKeyS2Flags)cmd.grantedKeys.Value;
                                if (!grantedKeysMask.HasFlag(NetworkKeyS2Flags.S2Class2) && _grantedSecuritySchemes.Contains(SecuritySchemes.S2_ACCESS))
                                {
                                    _grantedSecuritySchemes.Remove(SecuritySchemes.S2_ACCESS);
                                }
                                if (!grantedKeysMask.HasFlag(NetworkKeyS2Flags.S2Class1) && _grantedSecuritySchemes.Contains(SecuritySchemes.S2_AUTHENTICATED))
                                {
                                    _grantedSecuritySchemes.Remove(SecuritySchemes.S2_AUTHENTICATED);
                                }
                                SetIsSkipDskCallback();
                            }
                            #endregion
                        }
                        else
                        {
                            _KexFail.Data = new COMMAND_CLASS_SECURITY_2.KEX_FAIL { kexFailType = 0x06 };
                            _KexFail.DstNode = new NodeTag(SpecificResult.Id);
                            ((AddNodeS2Operation)ou.Action.ParentAction).SpecificResult.BootstrapingStatuses = BootstrapingStatuses.GrandSchemeCanceled;
                            ou.SetNextActionItems(_KexFail);
                        }
                    }
                    else
                    {
                        _KexFail.Data = new COMMAND_CLASS_SECURITY_2.KEX_FAIL { kexFailType = 0x01 };
                        _KexFail.DstNode = new NodeTag(SpecificResult.Id);
                        ou.SetNextActionItems(_KexFail);
                    }
                }
                else
                {
                    byte currentKexFailType = KEX_REPORT.supportedKexSchemes != 0x02 ? (byte)0x02 : (byte)0x00;
                    if (currentKexFailType == 0x00)
                    {
                        currentKexFailType = KEX_REPORT.supportedEcdhProfiles != 0x01 ? (byte)0x03 : (byte)0x00;
                    }
                    if (currentKexFailType > 0)
                    {
                        _KexFail.Data = new COMMAND_CLASS_SECURITY_2.KEX_FAIL { kexFailType = currentKexFailType };
                        _KexFail.DstNode = new NodeTag(SpecificResult.Id);
                        ou.SetNextActionItems(_KexFail);
                    }
                    //else
                    //{
                    //    Drop by timeout CC: 009F.01.05.11.007 and CC:009F.01.00.11.07E
                    //}
                }
            }
            else
            {
                SetStateCompletedSecurityFailed(ou);
            }
        }

        private void SetIsSkipDskCallback()
        {
            if (_isClientSideAuthGranted ||
                (!_grantedSecuritySchemes.Contains(SecuritySchemes.S2_ACCESS) &&
                    !_grantedSecuritySchemes.Contains(SecuritySchemes.S2_AUTHENTICATED)))
            {
                _isSkipDskCallback = true;
            }
        }

        private bool ValidateKexReportKeys()
        {
            bool ret = true;
            byte tmpFlags = KEX_REPORT.requestedKeys;
            tmpFlags = (byte)(tmpFlags & ~(byte)NetworkKeyS2Flags.S0);
            tmpFlags = (byte)(tmpFlags & ~(byte)NetworkKeyS2Flags.S2Class0);
            tmpFlags = (byte)(tmpFlags & ~(byte)NetworkKeyS2Flags.S2Class1);
            tmpFlags = (byte)(tmpFlags & ~(byte)NetworkKeyS2Flags.S2Class2);

            if (tmpFlags != 0)
            {
                ret = false;
            }
            return ret;
        }

        private void OnKexFail(ActionCompletedUnit ou)
        {
            SetStateCompletedSecurityFailed(ou);
        }

        private List<byte[]> OnKexFailCallback(AchData achData)
        {
            _KexFailReceived.SetCompleted();
            if (achData.Command.Length >= 3 && achData.Command[2] == 0x06)
                SpecificResult.BootstrapingStatuses = BootstrapingStatuses.JoiningCancel;
            return null;
        }

        private List<byte[]> OnSecurityMessageCallback(AchData achData)
        {
            if (((SubstituteIncomingFlags)achData.SubstituteIncomingFlags).HasFlag(SubstituteIncomingFlags.SecurityFailed))
            {
                if (_securityManagerInfo.DSKRequestStatus == DSKRequestStatuses.Completed ||
                    _securityManagerInfo.DSKRequestStatus == DSKRequestStatuses.Cancelled)
                {
                    _SecurityMessageReceived.Token.SetCompleted();
                }
            }
            return null;
        }

        protected void OnKexFailReceived(ActionCompletedUnit ou)
        {
            if (_KexFailReceived.Result)
            {
                SetStateCompletedSecurityFailed(ou);
            }
        }

        protected void OnSecurityMessageReceived(ActionCompletedUnit ou)
        {
            if (_SecurityMessageReceived.Result)
            {
                if (_securityManagerInfo.DSKRequestStatus == DSKRequestStatuses.Completed)
                {
                    if (_isWaitingForKexSetEcho)
                    {
                        _KexFail.SubstituteSettings.ClearFlag(SubstituteFlags.UseSecurity);
                        _KexFail.SubstituteSettings.SetFlag(SubstituteFlags.DenySecurity);
                    }
                    else
                    {
                        _KexFail.SubstituteSettings.ClearFlag(SubstituteFlags.DenySecurity);
                        _KexFail.SubstituteSettings.SetFlag(SubstituteFlags.UseSecurity);
                        _securityManagerInfo.ActivateNetworkKeyS2TempForNode(_peerNodeId);
                    }
                    _KexFail.Data = new COMMAND_CLASS_SECURITY_2.KEX_FAIL { kexFailType = 0x05 }; // KEX_FAIL_DECRYPT 
                    _KexFail.DstNode = new NodeTag(SpecificResult.Id);
                    ou.SetNextActionItems(_KexFail);
                }
                else if (_securityManagerInfo.DSKRequestStatus == DSKRequestStatuses.Cancelled)
                {
                    _KexFail.Data = new COMMAND_CLASS_SECURITY_2.KEX_FAIL { kexFailType = 0x06 }; // KEX_FAIL_CANCEL
                    _KexFail.DstNode = new NodeTag(SpecificResult.Id);
                    ou.SetNextActionItems(_KexFail);
                }
            }
        }

        private void OnSendPKReport(ActionCompletedUnit ou)
        {
            _KexSetEchoWaitStartedAt = DateTime.Now;
            byte[] receiverDSK = null;
            if (SpecificResult.DSK != null && SpecificResult.DSK.Length > 16)
            {
                receiverDSK = SpecificResult.DSK.Take(16).ToArray();
            }
            _securityManagerInfo.DSKRequestStatus = DSKRequestStatuses.None;
            if (!_isSkipDskCallback)
            {
                if (ParentAction is ActionSerialGroup &&
                                ((ActionSerialGroup)ParentAction).Actions[0] is AddNodeOperation &&
                                (((AddNodeOperation)(((ActionSerialGroup)(ParentAction)).Actions[0])).InitMode & Modes.NodeModeMask) == Modes.NodeHomeId)
                {
                    _securityManagerInfo.DSKRequestStatus = DSKRequestStatuses.Started;
                    byte[] inputDSK = ((IAddRemoveNode)(((ActionSerialGroup)(ParentAction)).Actions[0])).DskValue;
                    if (inputDSK == null || inputDSK.Length == 0 || inputDSK.Length > 16)
                    {
                        _securityManagerInfo.DSKRequestStatus = DSKRequestStatuses.Cancelled;
                    }
                    else
                    {
                        Array.Copy(inputDSK, 0, SpecificResult.DSK, 0, inputDSK.Length);
                        _securityManagerInfo.DSKRequestStatus = DSKRequestStatuses.Completed;
                    }

                    _securityManagerInfo.SetNetworkKeyS2Temp(_securityManagerInfo.CalculateTempNetworkKeyS2(SpecificResult.DSK, true));
                    _securityManagerInfo.ActivateNetworkKeyS2TempForNode(_peerNodeId);
                }
                else if (_securityManagerInfo.DSKNeededCallback != null)
                {
                    _securityManagerInfo.DSKRequestStatus = DSKRequestStatuses.Started;
                    _securityManagerInfo.TempKeyActivationResetEvent.Reset();
                    ThreadPool.QueueUserWorkItem(new WaitCallback((x) =>
                    {
                        byte[] inputDSK = _securityManagerInfo.DSKNeededCallback(SpecificResult.Node, receiverDSK);
                        if (inputDSK == null || inputDSK.Length == 0 || inputDSK.Length > 16)
                        {
                            _securityManagerInfo.DSKRequestStatus = DSKRequestStatuses.Cancelled;
                            SpecificResult.BootstrapingStatuses = BootstrapingStatuses.DSKCanceled;
                        }
                        else
                        {
                            Array.Copy(inputDSK, 0, SpecificResult.DSK, 0, inputDSK.Length);
                            _securityManagerInfo.DSKRequestStatus = DSKRequestStatuses.Completed;
                        }

                        _securityManagerInfo.SetNetworkKeyS2Temp(_securityManagerInfo.CalculateTempNetworkKeyS2(SpecificResult.DSK, true));
                        _securityManagerInfo.ActivateNetworkKeyS2TempForNode(_peerNodeId);
                        _securityManagerInfo.TempKeyActivationResetEvent.Set();
                    }));
                }
            }
        }

        private void OnReceivePKReport(ActionCompletedUnit ou)
        {
            if (_KEXSetPKReport.Result &&
                _KEXSetPKReport.SpecificResult.Command != null &&
                _KEXSetPKReport.SpecificResult.Command.Length > 2)
            {
                COMMAND_CLASS_SECURITY_2.PUBLIC_KEY_REPORT rpt = _KEXSetPKReport.SpecificResult.Command;
                if (rpt.properties1.includingNode == 0)
                {
                    SpecificResult.DSK = ((List<byte>)rpt.ecdhPublicKey).ToArray();
                    if (SpecificResult.DSK != null && SpecificResult.DSK.Length == 32 &&
                        ValidatePublicKeyReport(SpecificResult.DSK))
                    {
                        var senderPublicKey = _securityManagerInfo.GetPublicKeyS2();
                        if (_securityManagerInfo.DSKNeededCallback == null || _isSkipDskCallback)
                        {
                            _securityManagerInfo.SetNetworkKeyS2Temp(_securityManagerInfo.CalculateTempNetworkKeyS2(SpecificResult.DSK, true));
                            _securityManagerInfo.ActivateNetworkKeyS2TempForNode(_peerNodeId);
                        }
                        _PKReport.DstNode = new NodeTag(SpecificResult.Id);
                        _KEXSetEcho.SrcNode = new NodeTag(SpecificResult.Id);
                        var cmd = new COMMAND_CLASS_SECURITY_2.PUBLIC_KEY_REPORT()
                        {
                            ecdhPublicKey = new List<byte>(senderPublicKey),
                            properties1 = new COMMAND_CLASS_SECURITY_2.PUBLIC_KEY_REPORT.Tproperties1() { includingNode = 1 }
                        };
                        if (_isClientSideAuthGranted && cmd.ecdhPublicKey.Count > 3)
                        {
                            cmd.ecdhPublicKey[0] = 0;
                            cmd.ecdhPublicKey[1] = 0;
                            cmd.ecdhPublicKey[2] = 0;
                            cmd.ecdhPublicKey[3] = 0;
                        }

                        _PKReport.Data = cmd;
                        _isWaitingForKexSetEcho = true;

                        #region PublicKeyReportA
                        _securityTestSettingsService.ActivateTestPropertiesForFrame(SecurityS2TestFrames.PublicKeyReportA, _PKReport);
                        #endregion
                    }
                    else
                    {
                        //CC:009F.01.00.11.05F, A2:
                        _KexFail.Data = new COMMAND_CLASS_SECURITY_2.KEX_FAIL { kexFailType = 0x06 };
                        _KexFail.DstNode = new NodeTag(SpecificResult.Id);
                        ou.SetNextActionItems(_KexFail);
                    }
                }
                else
                {
                    SetStateCompletedSecurityFailed(ou);
                }
            }
            else
            {
                SetStateCompletedSecurityFailed(ou);
            }
        }

        private bool ValidatePublicKeyReport(byte[] receiverPublicKey)
        {
            bool ret = false;
            //Validate for Smart Start Inlcusion | NHID, only for S2_ACCESS and/or S2_AUTHENTICATED
            if (!_isSkipDskCallback &&
                ParentAction is ActionSerialGroup &&
                ((ActionSerialGroup)ParentAction).Actions[0] is AddNodeOperation &&
                (((AddNodeOperation)(((ActionSerialGroup)(ParentAction)).Actions[0])).InitMode & Modes.NodeModeMask) == Modes.NodeHomeId)
            {
                byte[] DskValue = ((IAddRemoveNode)(((ActionSerialGroup)(ParentAction)).Actions[0])).DskValue;
                if (receiverPublicKey != null && receiverPublicKey.Length > 16)
                {
                    var receiverDSK = SpecificResult.DSK.Take(16).Skip(2).ToArray();
                    ret = DskValue.ContainsArray(SpecificResult.DSK.Take(16).Skip(2).ToArray());
                }
            }
            else
            {
                if (!_isSkipDskCallback && receiverPublicKey.Length > 2)
                {
                    ret = receiverPublicKey.Take(2).SequenceEqual(new byte[] { 0x00, 0x00 });
                }
                else
                {
                    ret = true;
                }
            }
            return ret;
        }

        private void OnKEXSetEcho(ActionCompletedUnit ou)
        {
            if (_KEXSetEcho.Result)
            {
                _isWaitingForKexSetEcho = false;
                if (_KEXSetEcho.SpecificResult.SecurityScheme == SecuritySchemes.S2_TEMP)
                {
                    if (ValidateKexSetEcho(_KEXSetEcho.SpecificResult.Command))
                    {
                        _KEXReportEcho.DstNode = new NodeTag(SpecificResult.Id);
                        KEX_REPORT.properties1.echo = 1;
                        _KEXReportEcho.Data = KEX_REPORT;
                        _OnNKGet.TimeoutMs = 0; //InclusionS2TimeoutConstants.Including.NetworkKeyGet
                        _OnTransferEnd.TimeoutMs = 0; //InclusionS2TimeoutConstants.Including.TransferEnd
                        #region KEXReportEcho
                        var action = _securityTestSettingsService.ActivateTestPropertiesForFrame(SecurityS2TestFrames.KEXReportEcho, _KEXReportEcho);
                        #endregion
                    }
                    else
                    {
                        _KexFail.SubstituteSettings.ClearFlag(SubstituteFlags.DenySecurity);
                        _KexFail.SubstituteSettings.SetFlag(SubstituteFlags.UseSecurity);
                        _securityManagerInfo.ActivateNetworkKeyS2TempForNode(_peerNodeId);
                        _KexFail.Data = new COMMAND_CLASS_SECURITY_2.KEX_FAIL { kexFailType = 0x07 }; // KEX_FAIL_AUTH
                        _KexFail.DstNode = new NodeTag(SpecificResult.Id);
                        ou.SetNextActionItems(_KexFail);
                    }
                }
                else
                {
                    //009F.01.00.11.097 - must wait s2 temp established with KEX_GET ECHO encrypted Temp key during 240sec else - Drop by timeout CC
                    // (ignore KEX_SET ECHO nonencrypted S2 Temp)
                    var timeSpent = (int)(DateTime.Now - _KexSetEchoWaitStartedAt).TotalMilliseconds;
                    if (timeSpent < InclusionS2TimeoutConstants.Including.UserInputKeyList)
                    {
                        var timeLeft = InclusionS2TimeoutConstants.Including.UserInputKeyList - timeSpent;
                        _KEXSetEcho.TimeoutMs = timeLeft;
                        _KEXSetEcho.NewToken();
                        ou.SetNextActionItems(_KEXSetEcho);
                    }
                    else
                    {
                        ou.SetNextActionItems(_KexFailCancel);
                    }
                }
            }
            else
                SetStateCompletedSecurityFailed(ou);
        }

        private void OnKEXReportEcho(ActionCompletedUnit ou)
        {
            _OnNKGet.TimeoutMs = InclusionS2TimeoutConstants.Including.NetworkKeyGet;
            _OnTransferEnd.TimeoutMs = InclusionS2TimeoutConstants.Including.TransferEnd;
            ou.AddNextActionItems(new TimeInterval(0, _OnNKGet.Id, InclusionS2TimeoutConstants.Including.NetworkKeyGet));
            ou.AddNextActionItems(new TimeInterval(0, _OnTransferEnd.Id, InclusionS2TimeoutConstants.Including.TransferEnd));
        }

        private bool ValidateKexSetEcho(byte[] echoCmd)
        {
            bool ret = false;
            if (_KEXSetPKReport.Data != null && echoCmd != null)
            {
                COMMAND_CLASS_SECURITY_2.KEX_SET kexSet = _KEXSetPKReport.Data;
                kexSet.properties1.echo = 1;
                ret = echoCmd.SequenceEqual((byte[])kexSet);
            }
            return ret;
        }

        private void OnNKGetNextKey(ActionCompletedUnit ou)
        {
            if (_OnNKGet.Result)
            {
                if (_OnNKGet.SpecificResult.SecurityScheme == SecuritySchemes.S2_TEMP)
                {
                    IsOnTransferEndCancelled = true;
                    COMMAND_CLASS_SECURITY_2.SECURITY_2_NETWORK_KEY_GET cmd = _OnNKGet.SpecificResult.Command;
                    NetworkKeyS2Flags verifyKey = (NetworkKeyS2Flags)cmd.requestedKey.Value;
                    if (verifyKey != NetworkKeyS2Flags.None)
                    {
                        bool getKeyCorrect = OnNGetInner(cmd);
                        if (getKeyCorrect)
                        {
                            _OnTransferEnd.SetCancelled();
                            ou.SetNextActionItems(_OnTransferEnd, _NKReportNKVerify);
                        }
                        else
                        {
                            _KexFail.SubstituteSettings.ClearFlag(SubstituteFlags.DenySecurity);
                            _KexFail.SubstituteSettings.SetFlag(SubstituteFlags.UseSecurity);
                            _securityManagerInfo.ActivateNetworkKeyS2TempForNode(_peerNodeId);
                            _KexFail.Data = new COMMAND_CLASS_SECURITY_2.KEX_FAIL { kexFailType = 0x08 }; // KEX_FAIL_KEY_GET
                            _KexFail.DstNode = new NodeTag(SpecificResult.Id);
                            ou.SetNextActionItems(_KexFail);
                        }
                    }
                    else
                    {
                        SetStateCompletedSecurityFailed(ou);
                    }
                }
                else
                {
                    _KexFail.SubstituteSettings.ClearFlag(SubstituteFlags.DenySecurity);
                    _KexFail.SubstituteSettings.SetFlag(SubstituteFlags.UseSecurity);
                    _securityManagerInfo.ActivateNetworkKeyS2TempForNode(_peerNodeId);
                    _KexFail.Data = new COMMAND_CLASS_SECURITY_2.KEX_FAIL { kexFailType = 0x07 }; // KEX_FAIL_AUTH
                    _KexFail.DstNode = new NodeTag(SpecificResult.Id);
                    ou.SetNextActionItems(_KexFail);
                }
            }
            else if (_OnTransferEnd.Token.State == ActionStates.Expired && _OnNKGet.Token.State == ActionStates.Expired)
            {
                SetStateCompletedSecurityFailed(ou);
            }
            else
            {
                ou.SetNextActionItems();
            }
        }

        private bool OnNGetInner(COMMAND_CLASS_SECURITY_2.SECURITY_2_NETWORK_KEY_GET cmd)
        {
            bool ret = false;
            NetworkKeyS2Flags verifyKey = (NetworkKeyS2Flags)cmd.requestedKey.Value;
            var scheme = SecurityManagerInfo.ConvertToSecurityScheme(verifyKey);

            if (scheme != SecuritySchemes.NONE && _grantedSecuritySchemes.Contains(scheme))
            {
                byte[] nkey = _securityManagerInfo.GetActualNetworkKey(scheme, _network.IsLongRangeEnabled(SpecificResult.Node));
                _NKReportNKVerify.NewToken();
                _NKReportNKVerify.DstNode = SpecificResult.Node;
                _NKReportNKVerify.Data = new COMMAND_CLASS_SECURITY_2.SECURITY_2_NETWORK_KEY_REPORT()
                {
                    grantedKey = cmd.requestedKey,
                    networkKey = nkey
                };
                _lastGrantedKey = (NetworkKeyS2Flags)cmd.requestedKey.Value;

                #region TestFrame Section
                switch (scheme)
                {
                    case SecuritySchemes.S0:
                        #region NetworkKeyReport
                        _securityTestSettingsService.ActivateTestPropertiesForFrame(SecurityS2TestFrames.NetworkKeyReport_S0, _NKReportNKVerify);
                        #endregion
                        break;
                    case SecuritySchemes.S2_UNAUTHENTICATED:
                        #region NetworkKeyReport
                        _securityTestSettingsService.ActivateTestPropertiesForFrame(SecurityS2TestFrames.NetworkKeyReport_S2Unauthenticated, _NKReportNKVerify);
                        #endregion
                        break;
                    case SecuritySchemes.S2_AUTHENTICATED:
                        #region NetworkKeyReport
                        _securityTestSettingsService.ActivateTestPropertiesForFrame(SecurityS2TestFrames.NetworkKeyReport_S2Authenticated, _NKReportNKVerify);
                        #endregion
                        break;
                    case SecuritySchemes.S2_ACCESS:
                        #region NetworkKeyReport
                        _securityTestSettingsService.ActivateTestPropertiesForFrame(SecurityS2TestFrames.NetworkKeyReport_S2Access, _NKReportNKVerify);
                        #endregion
                        break;
                }
                #endregion

                ret = true;
            }
            else
            {
                ret = false;
            }
            return ret;
        }

        private void OnNKVerify(ActionCompletedUnit ou)
        {
            if (_NKReportNKVerify.Result)
            {
                var receivedScheme = _NKReportNKVerify.ExpectData.SpecificResult.SecurityScheme;
                var lastGrantedScheme = SecurityManagerInfo.ConvertToSecurityScheme(_lastGrantedKey);
                if (receivedScheme != SecuritySchemes.S2_TEMP &&
                    receivedScheme != SecuritySchemes.NONE /* &&
                    receivedScheme == lastGrantedScheme*/)
                {
                    if (_grantedSecuritySchemes.Contains(receivedScheme))
                    {
                        _grantedSecuritySchemes.Remove(receivedScheme);
                        _transferedSchemes.Add(receivedScheme);
                        _OnNKGet.NewToken();
                        _TransferEnd.NewToken();
                        _OnTransferEnd.NewToken();
                        var cmd = new COMMAND_CLASS_SECURITY_2.SECURITY_2_TRANSFER_END();
                        cmd.properties1.keyVerified = 1;
                        _TransferEnd.DstNode = new NodeTag(SpecificResult.Id);
                        _TransferEnd.Data = cmd;
                        _securityManagerInfo.ActivateNetworkKeyS2TempForNode(_peerNodeId);
                        IsOnTransferEndCancelled = false;
                        #region TestFrame Section
                        COMMAND_CLASS_SECURITY_2.SECURITY_2_NETWORK_KEY_REPORT nKReportSent = _NKReportNKVerify.Data;
                        var verifyKey = (NetworkKeyS2Flags)nKReportSent.grantedKey.Value;
                        var scheme = SecurityManagerInfo.ConvertToSecurityScheme(verifyKey);
                        switch (scheme)
                        {
                            case SecuritySchemes.S0:
                                #region TransferEndA_S0
                                _securityTestSettingsService.ActivateTestPropertiesForFrame(SecurityS2TestFrames.TransferEndA_S0, _TransferEnd);
                                #endregion
                                break;
                            case SecuritySchemes.S2_UNAUTHENTICATED:
                                #region TransferEndA_S2Unauthenticated
                                _securityTestSettingsService.ActivateTestPropertiesForFrame(SecurityS2TestFrames.TransferEndA_S2Unauthenticated, _TransferEnd);
                                #endregion
                                break;
                            case SecuritySchemes.S2_AUTHENTICATED:
                                #region TransferEndA_S2Authenticated
                                _securityTestSettingsService.ActivateTestPropertiesForFrame(SecurityS2TestFrames.TransferEndA_S2Authenticated, _TransferEnd);
                                #endregion
                                break;
                            case SecuritySchemes.S2_ACCESS:
                                #region TransferEndA_S2Access
                                _securityTestSettingsService.ActivateTestPropertiesForFrame(SecurityS2TestFrames.TransferEndA_S2Access, _TransferEnd);
                                #endregion
                                break;
                        }
                        #endregion
                    }
                    else
                    {
                        _KexFail.SubstituteSettings.ClearFlag(SubstituteFlags.DenySecurity);
                        _KexFail.SubstituteSettings.SetFlag(SubstituteFlags.UseSecurity);
                        _securityManagerInfo.ActivateNetworkKeyS2TempForNode(_peerNodeId);
                        _KexFail.Data = new COMMAND_CLASS_SECURITY_2.KEX_FAIL { kexFailType = 0x09 }; // KEX_FAIL_KEY_VERIFY
                        _KexFail.DstNode = new NodeTag(SpecificResult.Id);
                        ou.SetNextActionItems(_KexFail);
                    }
                }
                else
                {
                    _KexFail.SubstituteSettings.ClearFlag(SubstituteFlags.DenySecurity);
                    _KexFail.SubstituteSettings.SetFlag(SubstituteFlags.UseSecurity);
                    //_securityManagerInfo.ActivateNetworkKeyS2TempForNode(_peerNodeId); - is wrong
                    _KexFail.Data = new COMMAND_CLASS_SECURITY_2.KEX_FAIL { kexFailType = 0x07 }; // KEX_FAIL_KEY_AUTH
                    _KexFail.DstNode = new NodeTag(SpecificResult.Id);
                    ou.SetNextActionItems(_KexFail);
                }
            }
            else
            {
                SetStateCompletedSecurityFailed(ou);
            }
        }

        private void OnTransferEnd(ActionCompletedUnit ou)
        {
            if (_OnTransferEnd.Result)
            {
                if (_OnTransferEnd.SpecificResult.SecurityScheme == SecuritySchemes.S2_TEMP)
                {
                    COMMAND_CLASS_SECURITY_2.SECURITY_2_TRANSFER_END rpt = _OnTransferEnd.SpecificResult.Command;
                    if (_grantedSecuritySchemes.Count == 0 && rpt.properties1.keyVerified == 0 && rpt.properties1.keyRequestComplete == 1)
                    {
                        _OnNKGet.SetCancelled();
                        if (_transferedSchemes.Count > 0)
                        {
                            SetStateCompletedSecurityDone(ou);
                        }
                        else
                        {
                            SetStateCompletedSecurityNone(ou);
                        }
                    }
                    else
                    {
                        // Assume temp key is in use.
                        _KexFail.SubstituteSettings.ClearFlag(SubstituteFlags.DenySecurity);
                        _KexFail.SubstituteSettings.SetFlag(SubstituteFlags.UseSecurity);
                        _securityManagerInfo.ActivateNetworkKeyS2TempForNode(_peerNodeId);
                        _KexFail.Data = new COMMAND_CLASS_SECURITY_2.KEX_FAIL { kexFailType = 0x08 }; // KEX_FAIL_KEY_GET
                        _KexFail.DstNode = new NodeTag(SpecificResult.Id);
                        ou.SetNextActionItems(_KexFail);
                    }
                }
                else
                {
                    _KexFail.SubstituteSettings.ClearFlag(SubstituteFlags.DenySecurity);
                    _KexFail.SubstituteSettings.SetFlag(SubstituteFlags.UseSecurity);
                    _securityManagerInfo.ActivateNetworkKeyS2TempForNode(_peerNodeId);
                    _KexFail.Data = new COMMAND_CLASS_SECURITY_2.KEX_FAIL { kexFailType = 0x07 }; // KEX_FAIL_AUTH
                    _KexFail.DstNode = new NodeTag(SpecificResult.Id);
                    ou.SetNextActionItems(_KexFail);
                }
            }
            else if (_OnTransferEnd.Token.State == ActionStates.Cancelled && !IsOnTransferEndCancelled)
            {
                SetStateCompletedSecurityFailed(ou);
            }
            else if (_OnTransferEnd.Token.State == ActionStates.Expired && _OnNKGet.Token.State == ActionStates.Expired)
            {
                SetStateCompletedSecurityFailed(ou);
            }
            else
            {
                ou.SetNextActionItems();
            }
        }

        public override string AboutMe()
        {
            string ret = string.Empty;
            if (ParentAction != null)
            {
                ret = $"Id={SpecificResult.Id}, Security={SpecificResult.SubstituteStatus}";
                if (SpecificResult.DSK != null && SpecificResult.DSK.Length >= 2)
                {
                    ret += $", DSK={Tools.GetInt32(SpecificResult.DSK, 0, 2)}";
                }
            }
            return ret;
        }

        public AddRemoveNodeResult SpecificResult
        {
            get { return (AddRemoveNodeResult)ParentAction.Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new AddRemoveNodeResult();
        }
    }
}
