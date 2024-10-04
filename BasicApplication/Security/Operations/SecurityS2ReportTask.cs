/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
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

namespace ZWave.BasicApplication.Operations
{
    public class SecurityS2ReportTask : ApiAchOperation
    {
        #region Timeouts
        const int SEND_DATA_TIMER = 2000;
        #endregion

        private const byte MULTICAST_MASK = 0x08;
        private const byte BROADCAST_MASK = 0x04;

        private SecurityS2CryptoProvider _securityS2CryptoProvider;
        private SecurityManagerInfo _securityManagerInfo;
        private MpanTable _mpanTable;
        private SpanTable _spanTable;
        private readonly ISecurityTestSettingsService _securityTestSettingsService;

        internal SecurityS2ReportTask(NetworkViewPoint network, SecurityManagerInfo securityManagerInfo,
            SecurityS2CryptoProvider securityS2CryptoProvider, SpanTable spanTable, MpanTable mpanTable)
            : base(network, NodeTag.Empty, NodeTag.Empty, new ByteIndex(COMMAND_CLASS_SECURITY_2.ID))
        {
            _securityManagerInfo = securityManagerInfo;
            _securityS2CryptoProvider = securityS2CryptoProvider;
            _spanTable = spanTable;
            _mpanTable = mpanTable;
            _securityTestSettingsService = new SecurityTestSettingsService(_securityManagerInfo, false);
        }

        NodeTag handlingNonceGetFromNode = NodeTag.Empty;
        protected override void OnHandledInternal(DataReceivedUnit ou)
        {
            var destNode = ReceivedAchData.DstNode.Id > 0 ? ReceivedAchData.DstNode : _securityManagerInfo.Network.NodeTag;
            SecuritySchemes scheme = SecuritySchemes.NONE;
            InvariantPeerNodeId peerNodeId = new InvariantPeerNodeId(destNode, ReceivedAchData.SrcNode);
            ou.SetNextActionItems();
            if (!ou.DataFrame.IsSkippedSecurity)
            {
                if (_securityManagerInfo.Network.HasSecurityScheme(SecuritySchemeSet.ALLS2) && _securityManagerInfo.IsActive)
                {
                    byte[] command = ReceivedAchData.Command;
                    bool isNonceReport = false;
                    bool isSupportedReport = false; // Only for test frame
                    Extensions extensions = null;
                    SubstituteSettings substituteSettings = null;
                    if (command != null && command.Length > 1)
                    {
                        bool isSubstituteDenied = false;
                        byte[] dataToSend = null;
                        bool isMulticastFrame = (ou.DataFrame.Data[2] & MULTICAST_MASK) == MULTICAST_MASK;
                        bool isBroadcastFrame = (ou.DataFrame.Data[2] & BROADCAST_MASK) == BROADCAST_MASK;

                        if (command[1] == COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_GET.ID && (SecuritySchemes)ReceivedAchData.SecurityScheme == SecuritySchemes.NONE)
                        {
                            if (command.Length > 2 && !isMulticastFrame && !isBroadcastFrame)
                            {
                                byte rxSequenceNumber = command[2];
                                if (handlingNonceGetFromNode != ReceivedAchData.SrcNode)
                                {
                                    handlingNonceGetFromNode = ReceivedAchData.SrcNode;
                                    var currentTxSequenceNumber = _spanTable.GetTxSequenceNumber(peerNodeId, _securityManagerInfo.GetInitialS2SequenceNumber());
                                    _spanTable.SetNonceFree(peerNodeId);

                                    //reset MPAN for owner Id
                                    foreach (byte groupId in _mpanTable.SelectGroupIds(ReceivedAchData.SrcNode))
                                    {
                                        _mpanTable.RemoveRecord(new NodeGroupId(ReceivedAchData.SrcNode, groupId));
                                    }
                                    dataToSend = _securityS2CryptoProvider.GenerateNonceReport(_spanTable, peerNodeId, ++currentTxSequenceNumber, rxSequenceNumber, true, false);
                                    isNonceReport = true;
                                    isSubstituteDenied = true;
                                }
                            }
                        }
                        else if (command[1] == COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION.ID)
                        {
                            if (command.Length > 2 && !isMulticastFrame && !isBroadcastFrame && ValidateS2MessageExtensions(command))
                            {
                                byte rxSequenceNumber = command[2];
                                var currentTxSequenceNumber = _spanTable.GetTxSequenceNumber(peerNodeId, _securityManagerInfo.GetInitialS2SequenceNumber());
                                _spanTable.SetNonceFree(peerNodeId);

                                var isMos = _securityS2CryptoProvider.CheckMpanMosForOwnerNode(_mpanTable, ReceivedAchData.SrcNode);
                                dataToSend = _securityS2CryptoProvider.GenerateNonceReport(_spanTable, peerNodeId, ++currentTxSequenceNumber, rxSequenceNumber, true, isMos);
                                isNonceReport = true;
                                isSubstituteDenied = true;
                            }
                        }
                        else if (command[1] == COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT.ID && (SecuritySchemes)ReceivedAchData.SecurityScheme == SecuritySchemes.NONE)
                        {
                            if (command.Length > 3 && !isMulticastFrame && !isBroadcastFrame)
                            {
                                COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT nonceReportCmd = command;
                                if (_securityManagerInfo.InitializingNodeId != ReceivedAchData.SrcNode) // Node must be already initialized.
                                {
                                    if (nonceReportCmd.properties1.sos > 0 && // Singlecast out of sync.
                                        nonceReportCmd.receiversEntropyInput != null &&
                                        nonceReportCmd.receiversEntropyInput.Count == 16
                                        )
                                    {
                                        if (_securityManagerInfo.RetransmissionTableS2.TryGetValue(peerNodeId, out RetransmissionRecord rrec))
                                        {
                                            if (rrec.Counter > 0)
                                            {
                                                _spanTable.AddOrReplace(peerNodeId,
                                                    nonceReportCmd.receiversEntropyInput.ToArray(),
                                                    _spanTable.GetTxSequenceNumber(peerNodeId, _securityManagerInfo.GetInitialS2SequenceNumber()),
                                                    nonceReportCmd.sequenceNumber);

                                                dataToSend = rrec.Data;
                                                scheme = rrec.SecurityScheme;
                                                substituteSettings = rrec.SubstituteSettings;
                                                rrec.Counter--;
                                            }
                                            else
                                            {
                                                if (_securityManagerInfo.RetransmissionTableS2.TryRemove(peerNodeId, out RetransmissionRecord rr))
                                                {
                                                }
                                                _spanTable.SetNonceFree(peerNodeId);
                                            }
                                        }
                                        else
                                        {
                                            _spanTable.SetNonceFree(peerNodeId);
                                        }
                                    }
                                    if (nonceReportCmd.properties1.mos > 0) // Mutlicast out of sync.
                                    {
                                        var groupId = _securityS2CryptoProvider.LastSentMulticastGroupId;

                                        extensions = new Extensions();
                                        var nodeGroupId = new NodeGroupId(destNode, groupId);
                                        if (!_mpanTable.CheckMpanExists(nodeGroupId))
                                        {
                                            _mpanTable.AddOrReplace(nodeGroupId, 0x55, null, _securityS2CryptoProvider.GetRandomData());
                                        }
                                        extensions.AddMpanExtension(_mpanTable.GetContainer(nodeGroupId).MpanState, groupId);
                                    }
                                }
                            }
                        }
                        else if (command[1] == COMMAND_CLASS_SECURITY_2.SECURITY_2_COMMANDS_SUPPORTED_GET.ID)
                        {
                            if (!isMulticastFrame && !isBroadcastFrame)
                            {
                                scheme = (SecuritySchemes)ReceivedAchData.SecurityScheme;
                                if (scheme != SecuritySchemes.NONE && scheme != SecuritySchemes.S0 && _securityManagerInfo.Network.HasSecurityScheme(scheme))
                                {
                                    if (!_securityManagerInfo.Network.HasSecurityScheme(ReceivedAchData.SrcNode.Id, SecuritySchemeSet.ALLS2) &&
                                        !_securityManagerInfo.Network.IsSecuritySchemesSpecified(ReceivedAchData.SrcNode))
                                    {
                                        _securityManagerInfo.Network.SetSecuritySchemes(ReceivedAchData.SrcNode.Id, SecuritySchemeSet.ALL);
                                    }

                                    isSupportedReport = true;
                                    var ccReport = new COMMAND_CLASS_SECURITY_2.SECURITY_2_COMMANDS_SUPPORTED_REPORT();
                                    if (ReceivedAchData.CommandType == CommandTypes.CmdApplicationCommandHandler_Bridge &&
                                       ReceivedAchData.DstNode.Id != _securityManagerInfo.Network.NodeTag.Id)
                                    {
                                        ccReport.commandClass = new List<byte>(_securityManagerInfo.Network.GetVirtualSecureCommandClasses());
                                    }
                                    else
                                    {
                                        var secureCommandClasses = _securityManagerInfo.Network.GetSecureCommandClasses();
                                        if (secureCommandClasses != null)
                                        {
                                            switch (scheme)
                                            {
                                                case SecuritySchemes.S2_UNAUTHENTICATED:
                                                    if (!_securityManagerInfo.Network.HasSecurityScheme(SecuritySchemes.S2_ACCESS) &&
                                                        !_securityManagerInfo.Network.HasSecurityScheme(SecuritySchemes.S2_AUTHENTICATED))
                                                    {
                                                        ccReport.commandClass = new List<byte>(_securityManagerInfo.Network.GetSecureCommandClasses());
                                                    }
                                                    break;
                                                case SecuritySchemes.S2_AUTHENTICATED:
                                                    if (!_securityManagerInfo.Network.HasSecurityScheme(SecuritySchemes.S2_ACCESS))
                                                    {
                                                        ccReport.commandClass = new List<byte>(_securityManagerInfo.Network.GetSecureCommandClasses());
                                                    }
                                                    break;
                                                case SecuritySchemes.S2_ACCESS:
                                                    ccReport.commandClass = new List<byte>(secureCommandClasses);
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                    }
                                    dataToSend = ccReport;
                                }
                            }
                        }

                        if (dataToSend != null || extensions != null)
                        {
                            ISendDataAction sendData = null;
                            if (SecuritySchemeSet.ALLS2.Contains(scheme))
                            {
                                sendData = new SendDataExOperation(_network, ReceivedAchData.DstNode, ReceivedAchData.SrcNode, dataToSend, _securityManagerInfo.TxOptions, scheme);
                            }
                            else
                            {
                                if (ReceivedAchData.DstNode.Id > 0)
                                {
                                    sendData = new SendDataBridgeOperation(_network, ReceivedAchData.DstNode, ReceivedAchData.SrcNode, dataToSend, _securityManagerInfo.TxOptions);
                                    if (extensions != null)
                                    {
                                        ((SendDataBridgeOperation)sendData).Extensions = extensions;
                                    }
                                }
                                else
                                {
                                    sendData = new SendDataOperation(_network, ReceivedAchData.SrcNode, dataToSend, _securityManagerInfo.TxOptions);
                                    if (extensions != null)
                                    {
                                        ((SendDataOperation)sendData).Extensions = extensions;
                                    }
                                }
                            }

                            if (substituteSettings != null)
                            {
                                sendData.SubstituteSettings = substituteSettings;
                            }

                            if (isSubstituteDenied)
                            {
                                sendData.SubstituteSettings.SetFlag(SubstituteFlags.DenySecurity);
                            }


                            sendData.CompletedCallback = (x) =>
                            {
                                var action = x as ActionBase;
                                if (action != null)
                                {
                                    handlingNonceGetFromNode = NodeTag.Empty;
                                    SpecificResult.TotalCount++;
                                    if (action.Result.State != ActionStates.Completed)
                                        SpecificResult.FailCount++;
                                }
                            };
                            #region TestFrames
                            if (isNonceReport)
                            {
                                _securityTestSettingsService.ActivateTestPropertiesForFrame(SecurityS2TestFrames.NonceReport, sendData);
                            }
                            else if (isSupportedReport)
                            {
                                _securityTestSettingsService.ActivateTestPropertiesForFrame(SecurityS2TestFrames.CommandsSupportedReport, sendData);
                            }
                            #endregion

                            ou.SetNextActionItems(sendData);
                        }
                        else
                        {
                            ou.SetNextActionItems();
                        }

                    }
                }
                else
                {
                    "REJECT, {0}, {1} (IsNodeSecureS2={2}, IsActive={3}"._DLOG(
                        _securityManagerInfo.IsInclusion,
                        _securityManagerInfo.Network.HasSecurityScheme(ReceivedAchData.SrcNode.Id, SecuritySchemeSet.ALLS2),
                        _securityManagerInfo.Network.HasSecurityScheme(SecuritySchemeSet.ALLS2),
                        _securityManagerInfo.IsActive);
                }
            }
        }

        private bool ValidateS2MessageExtensions(byte[] command)
        {
            bool ret = true;
            var commandParsed = (COMMAND_CLASS_SECURITY_2.SECURITY_2_MESSAGE_ENCAPSULATION)command;
            if (commandParsed.properties1.extension > 0)
            {
                foreach (var vg in commandParsed.vg1)
                {
                    if (vg.properties1.type == (byte)ExtensionTypes.Span)
                    {
                        ret &= vg.extensionLength == 0x12 && vg.properties1.critical == 1;
                    }
                    else if (vg.properties1.type == (byte)ExtensionTypes.Mpan)
                    {
                        ret = false;
                    }
                    else if (vg.properties1.type == (byte)ExtensionTypes.MpanGrp)
                    {
                        ret &= vg.extensionLength == 0x03 && vg.properties1.critical == 1;
                    }
                    else if (vg.properties1.type == (byte)ExtensionTypes.Mos)
                    {
                        ret &= vg.extensionLength == 0x03 && vg.properties1.critical == 0;
                    }
                }
            }
            return ret;
        }

        public NonceS2ResponseDataResult SpecificResult
        {
            get { return (NonceS2ResponseDataResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new NonceS2ResponseDataResult();
        }
    }

    public class NonceS2ResponseDataResult : ActionResult
    {
        public int TotalCount { get; set; }
        public int FailCount { get; set; }
    }
}
