/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Linq;
using System.Threading;
using ZWave.BasicApplication.Security;
using ZWave.CommandClasses;
using ZWave.Devices;
using ZWave.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class SetLearnModeS0Operation : ApiOperation
    {
        #region Timeouts
        /// <summary>
        /// Time from CMD sent to CMD received (CMD - any command except NONCE commands)
        /// </summary>
        public static int CMD_TIMEOUT = 10000;
        #endregion

        private SecurityManagerInfo _securityManagerInfo;
        internal NodeTag Node { get; set; }
        internal NodeTag VirtualNode { get; set; }
        internal bool IsController { get; set; }
        internal byte SupportedSecuritySchemes { get; set; }

        internal SetLearnModeS0Operation(NetworkViewPoint network, SecurityManagerInfo securityManagerInfo, NodeTag node, byte supportedSecuritySchemes)
            : base(false, null, false)
        {
            _network = network; 
            _securityManagerInfo = securityManagerInfo;
            Node = node;
            SupportedSecuritySchemes = supportedSecuritySchemes;
            prevHomeId = _securityManagerInfo.Network.HomeId;
        }

        RequestDataOperation requestNetworkKey;
        RequestDataOperation requestSchemeInherit;
        SendDataExOperation sendSecureSchemeReport;
        SendDataExOperation sendNetworkKeyVerify;

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(OnSchemeGet, 0));
            ActionUnits.Add(new ActionCompletedUnit(requestNetworkKey, OnNetworkKeySet));
            ActionUnits.Add(new ActionCompletedUnit(requestSchemeInherit, OnSchemeInherit, sendSecureSchemeReport));
            ActionUnits.Add(new ActionCompletedUnit(sendSecureSchemeReport, OnDone));
            ActionUnits.Add(new ActionCompletedUnit(sendNetworkKeyVerify, OnDone));
        }

        protected override void CreateInstance()
        {
            requestNetworkKey = new RequestDataOperation(_network, NodeTag.Empty, NodeTag.Empty,
                new COMMAND_CLASS_SECURITY.SECURITY_SCHEME_REPORT() { supportedSecuritySchemes = _securityManagerInfo.SecuritySchemeInReportS0 }, _securityManagerInfo.TxOptions,
                new COMMAND_CLASS_SECURITY.NETWORK_KEY_SET(), 2, CMD_TIMEOUT);
            requestNetworkKey.SubstituteSettings.SetFlag(SubstituteFlags.DenySecurity);

            requestSchemeInherit = new RequestDataOperation(_network, NodeTag.Empty, NodeTag.Empty,
                new COMMAND_CLASS_SECURITY.NETWORK_KEY_VERIFY(), _securityManagerInfo.TxOptions,
                new COMMAND_CLASS_SECURITY.SECURITY_SCHEME_INHERIT(), 2, CMD_TIMEOUT);

            sendSecureSchemeReport = new SendDataExOperation(_network, NodeTag.Empty, NodeTag.Empty, new COMMAND_CLASS_SECURITY.SECURITY_SCHEME_REPORT() { supportedSecuritySchemes = _securityManagerInfo.SecuritySchemeInReportEncryptedS0 }, _securityManagerInfo.TxOptions, SecuritySchemes.S0);
            sendNetworkKeyVerify = new SendDataExOperation(_network, NodeTag.Empty, NodeTag.Empty, new COMMAND_CLASS_SECURITY.NETWORK_KEY_VERIFY(), _securityManagerInfo.TxOptions, SecuritySchemes.S0);
        }

        protected void SetStateCompletedSecurityFailed(IActionUnit ou)
        {
            _securityManagerInfo.Network.ResetSecuritySchemes();
            _securityManagerInfo.Network.ResetSecuritySchemes(Node);
            _securityManagerInfo.IsInclusion = false;
            SetStateCompleted(ou);
        }

        protected void SetStateCompletedSecurityDone(IActionUnit ou)
        {
            _securityManagerInfo.Network.SetSecuritySchemes(SecuritySchemeSet.S0);
            _securityManagerInfo.Network.SetSecuritySchemes(Node, SecuritySchemeSet.S0);
            _securityManagerInfo.IsInclusion = false;
            SetStateCompleted(ou);
        }

        protected override void SetStateFailed(IActionUnit ou)
        {
            _securityManagerInfo.Network.ResetSecuritySchemes();
            _securityManagerInfo.Network.ResetSecuritySchemes(Node);
            _securityManagerInfo.IsInclusion = false;
            base.SetStateFailed(ou);
        }

        readonly byte[] prevHomeId = null;
        private void OnSchemeGet(StartActionUnit ou)
        {
            SpecificResult.SubstituteStatus = SubstituteStatuses.Failed;
            _securityManagerInfo.IsInclusion = true;
            PrepareRequestNetworkKey(ou);
        }

        private void PrepareRequestNetworkKey(StartActionUnit ou)
        {
            if (((SupportedSecuritySchemes >> 1) & 1) == 0)
            {
                _securityManagerInfo.ActivateNetworkKeyS0Temp();
                requestNetworkKey.DstNode = Node;
                requestNetworkKey.SrcNode = VirtualNode;
                ou.SetNextActionItems(requestNetworkKey);
                if (_securityManagerInfo.DelaysS0.ContainsKey(SecurityS0Delays.SchemeReport))
                {
                    Thread.Sleep(_securityManagerInfo.DelaysS0[SecurityS0Delays.SchemeReport]);
                }
            }
            else
            {
                SpecificResult.SubstituteStatus = SubstituteStatuses.Failed;
                SetStateCompletedSecurityFailed(ou);
            }
        }

        private void OnNetworkKeySet(ActionCompletedUnit ou)
        {
            const byte keyLength = 16;
            if (requestNetworkKey.Result.State == ActionStates.Completed)
            {
                COMMAND_CLASS_SECURITY.NETWORK_KEY_SET cmd = requestNetworkKey.SpecificResult.Command;
                byte[] key = new byte[keyLength];
                if (cmd.networkKeyByte != null && cmd.networkKeyByte.Count() == keyLength)
                {
                    Array.Copy(cmd.networkKeyByte.ToArray(), 0, key, 0, Math.Min(cmd.networkKeyByte.Count, keyLength));
                    _securityManagerInfo.SetNetworkKey(key, SecuritySchemes.S0, false);
                    _securityManagerInfo.ActivateNetworkKeyS0();
                    if (VirtualNode.Id > 0 || !IsController)
                    {
                        sendNetworkKeyVerify.DstNode = Node;
                        sendNetworkKeyVerify.SrcNode = VirtualNode;
                        ou.SetNextActionItems(sendNetworkKeyVerify);
                    }
                    else
                    {
                        requestSchemeInherit.DstNode = Node;
                        requestSchemeInherit.SrcNode = VirtualNode;
                        ou.SetNextActionItems(requestSchemeInherit);
                        if (_securityManagerInfo.DelaysS0.ContainsKey(SecurityS0Delays.NetworkKeyVerify))
                        {
                            requestSchemeInherit.DataDelay = _securityManagerInfo.DelaysS0[SecurityS0Delays.NetworkKeyVerify];
                        }
                    }
                }
                else
                {
                    SpecificResult.SubstituteStatus = SubstituteStatuses.Failed;
                    SetStateCompletedSecurityFailed(ou);
                }
            }
            else
            {
                SpecificResult.SubstituteStatus = SubstituteStatuses.Failed;
                SetStateCompletedSecurityFailed(ou);
            }
        }

        private void OnSchemeInherit(ActionCompletedUnit ou)
        {
            if (requestSchemeInherit.Result.State == ActionStates.Completed)
            {
                COMMAND_CLASS_SECURITY.SECURITY_SCHEME_INHERIT cmd = requestSchemeInherit.SpecificResult.Command;
                if (((cmd.supportedSecuritySchemes >> 1) & 1) == 0)
                {
                    sendSecureSchemeReport.DstNode = Node;
                    sendSecureSchemeReport.SrcNode = VirtualNode;
                    if (_securityManagerInfo.DelaysS0.ContainsKey(SecurityS0Delays.SchemeReportEnc))
                    {
                        sendSecureSchemeReport.DataDelay = _securityManagerInfo.DelaysS0[SecurityS0Delays.SchemeReportEnc];
                    }
                }
                else
                {
                    SpecificResult.SubstituteStatus = SubstituteStatuses.Failed;
                    SetStateCompletedSecurityFailed(ou);
                }
            }
            else
            {
                SpecificResult.SubstituteStatus = SubstituteStatuses.Failed;
                SetStateCompletedSecurityFailed(ou);
            }
        }

        private void OnDone(ActionCompletedUnit tu)
        {
            if (tu.Action.Result.State == ActionStates.Completed)
            {
                SpecificResult.SubstituteStatus = SubstituteStatuses.Done;
                if (SpecificResult.SecuritySchemes == null || SpecificResult.SecuritySchemes.Length == 0)
                {
                    SpecificResult.SecuritySchemes = new SecuritySchemes[] { SecuritySchemes.S0 };
                }
                SetStateCompletedSecurityDone(tu);
            }
            else
            {
                SetStateCompletedSecurityFailed(tu);
            }
        }

        public override string AboutMe()
        {
            if (ParentAction != null)
                return string.Format("Id={0}, Security={1}", SpecificResult.Node.Id, SpecificResult.SubstituteStatus);
            else
                return "";
        }

        public SetLearnModeResult SpecificResult
        {
            get { return (SetLearnModeResult)ParentAction.Result; }
        }
    }
}
