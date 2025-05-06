/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Collections.Generic;
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
    public class SecurityReportTask : ApiAchOperation
    {
        #region Timeouts
        const int SEND_DATA_TIMER = 2000;
        #endregion

        private const byte MULTICAST_MASK = 0x08;
        private const byte BROADCAST_MASK = 0x04;

        private SecurityS0CryptoProvider _securityS0CryptoProvider;
        private SecurityManagerInfo _securityManagerInfo;
        internal SecurityReportTask(NetworkViewPoint network, SecurityManagerInfo securityManagerInfo, SecurityS0CryptoProvider securityS0CryptoProvider)
            : base(network, NodeTag.Empty, NodeTag.Empty, new ByteIndex(COMMAND_CLASS_SECURITY.ID))
        {
            _securityManagerInfo = securityManagerInfo;
            _securityS0CryptoProvider = securityS0CryptoProvider;
        }

        NodeTag handlingNonceGetFromNode = NodeTag.Empty;
        protected override void OnHandledInternal(DataReceivedUnit ou)
        {
            ou.SetNextActionItems();
            if (!ou.DataFrame.IsSkippedSecurity)
            {
                if (_securityManagerInfo.IsActive && _securityManagerInfo.Network.HasSecurityScheme(SecuritySchemes.S0))
                {
                    byte[] command = ReceivedAchData.Command;
                    if (command != null && command.Length > 1)
                    {
                        byte[] dataToSend = null;
                        bool isSubstituteDenied = false;
                        bool isMulticastFrame = (ou.DataFrame.Data[2] & MULTICAST_MASK) == MULTICAST_MASK;
                        bool isBroadcastFrame = (ou.DataFrame.Data[2] & BROADCAST_MASK) == BROADCAST_MASK;

                        if (!isMulticastFrame && !isBroadcastFrame &&
                            (command[1] == COMMAND_CLASS_SECURITY.SECURITY_NONCE_GET.ID ||
                            command[1] == COMMAND_CLASS_SECURITY.SECURITY_MESSAGE_ENCAPSULATION_NONCE_GET.ID) &&
                            handlingNonceGetFromNode != ReceivedAchData.SrcNode)
                        {
                            handlingNonceGetFromNode = ReceivedAchData.SrcNode;
                            var destNodeId = ReceivedAchData.DstNode.Id > 0 ? ReceivedAchData.DstNode : _securityManagerInfo.Network.NodeTag;
                            if (_securityManagerInfo.IsSenderNonceS0Disabled)
                            {
                                handlingNonceGetFromNode = NodeTag.Empty;
                            }
                            else
                            {
                                dataToSend = _securityS0CryptoProvider.GenerateNonceReport(new OrdinalPeerNodeId(ReceivedAchData.SrcNode, destNodeId));
                            }
                            isSubstituteDenied = true;
                            if (_securityManagerInfo.DelaysS0.ContainsKey(SecurityS0Delays.NonceReport))
                            {
                                Thread.Sleep(_securityManagerInfo.DelaysS0[SecurityS0Delays.NonceReport]);
                            }
                        }
                        else if (command[1] == COMMAND_CLASS_SECURITY.SECURITY_COMMANDS_SUPPORTED_GET.ID && handlingNonceGetFromNode != ReceivedAchData.SrcNode)
                        {
                            handlingNonceGetFromNode = ReceivedAchData.SrcNode;
                            var scheme = (SecuritySchemes)ReceivedAchData.SecurityScheme;
                            if (scheme == SecuritySchemes.S0)
                            {
                                if (!_securityManagerInfo.Network.IsSecuritySchemesSpecified(ReceivedAchData.SrcNode) || _securityManagerInfo.Network.HasSecurityScheme(ReceivedAchData.SrcNode.Id, SecuritySchemes.S0))
                                {
                                    var ccReport = new COMMAND_CLASS_SECURITY.SECURITY_COMMANDS_SUPPORTED_REPORT();
                                    if (!_securityManagerInfo.Network.HasSecurityScheme(SecuritySchemeSet.ALLS2))
                                    {
                                        if (ReceivedAchData.DstNode.Id > 0 && ReceivedAchData.DstNode.Id != _securityManagerInfo.Network.NodeTag.Id)
                                        {
                                            ccReport.commandClassSupport = new List<byte>(_securityManagerInfo.Network.GetVirtualSecureCommandClasses());
                                        }
                                        else
                                        {
                                            var secureCommandClasses = _securityManagerInfo.Network.GetSecureCommandClasses();
                                            if (secureCommandClasses != null)
                                            {
                                                ccReport.commandClassSupport = new List<byte>(secureCommandClasses);
                                            }
                                        }
                                        dataToSend = ccReport;
                                    }
                                    else
                                    {
                                        dataToSend = new byte[] { 0x98, 0x03, 0x00 };
                                    }
                                }
                            }
                        }

                        if (dataToSend != null)
                        {
                            ApiOperation sendData = null;

                            if (ReceivedAchData.DstNode.Id > 0)
                            {
                                sendData = new SendDataBridgeOperation(_network, ReceivedAchData.DstNode, ReceivedAchData.SrcNode, dataToSend, _securityManagerInfo.TxOptions);
                            }
                            else
                            {
                                sendData = new SendDataOperation(_network, ReceivedAchData.SrcNode, dataToSend, _securityManagerInfo.TxOptions);
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
                    "REJECT, {0}, {1} (IsNodeSecure[S0]={2}, IsActive={3}"._DLOG(
                       _securityManagerInfo.IsInclusion,
                       _securityManagerInfo.Network.HasSecurityScheme(ReceivedAchData.SrcNode.Id, SecuritySchemes.S0),
                       _securityManagerInfo.Network.HasSecurityScheme(SecuritySchemes.S0),
                       _securityManagerInfo.IsActive);
                }
            }
        }

        public NonceResponseDataResult SpecificResult
        {
            get { return (NonceResponseDataResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new NonceResponseDataResult();
        }
    }

    public class NonceResponseDataResult : ActionResult
    {
        public int TotalCount { get; set; }
        public int FailCount { get; set; }
    }
}
