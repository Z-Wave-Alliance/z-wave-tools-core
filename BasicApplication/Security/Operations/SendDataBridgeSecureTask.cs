/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Linq;
using System.Threading;
using Utils;
using ZWave.BasicApplication.Security;
using ZWave.CommandClasses;
using ZWave.Devices;
using ZWave.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class SendDataBridgeSecureTask : ApiOperation
    {
        protected TransmitOptions TxOptions { get; set; }
        internal byte[] CommandToSecureSend { get; private set; }
        internal NodeTag SrcNode { get; private set; }
        internal NodeTag DestNode { get; private set; }
        internal NodeTag? TestNode { get; set; }
        private SecurityManagerInfo _securityManagerInfo;
        private SecurityS0CryptoProvider _securityS0CryptoProvider;

        internal SendDataBridgeSecureTask(NetworkViewPoint network, SecurityManagerInfo securityManagerInfo, SecurityS0CryptoProvider securityS0CryptoProvider, NodeTag srcNode, NodeTag destNode, byte[] data, TransmitOptions txOptions)
            : base(false, null, false)
        {
            _network = network;
            _securityManagerInfo = securityManagerInfo;
            _securityS0CryptoProvider = securityS0CryptoProvider;
            SubstituteSettings.SetFlag(SubstituteFlags.DenySecurity);
            CommandToSecureSend = data;
            SrcNode = srcNode;
            DestNode = destNode;
            TxOptions = txOptions;
        }

        RequestDataOperation requestNonce;
        SendDataBridgeOperation sendEncData;

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(OnStart, 0, requestNonce));
            ActionUnits.Add(new ActionCompletedUnit(requestNonce, OnNonceReport, sendEncData));
            ActionUnits.Add(new ActionCompletedUnit(sendEncData, OnSendEncData));
        }

        protected override void CreateInstance()
        {
            requestNonce = new RequestDataOperation(_network, SrcNode, DestNode,
                new COMMAND_CLASS_SECURITY.SECURITY_NONCE_GET(), TxOptions,
                new COMMAND_CLASS_SECURITY.SECURITY_NONCE_REPORT(), 2, DefaultTimeouts.SECURITY_S0_NONCE_REQUEST_TIMEOUT);

            requestNonce.SubstituteSettings.SetFlag(SubstituteFlags.DenySecurity);

            sendEncData = new SendDataBridgeOperation(_network, SrcNode, DestNode, null, TxOptions);
            sendEncData.SubstituteSettings.SetFlag(SubstituteFlags.DenySecurity);
        }

        private void OnStart(StartActionUnit ou)
        {
            if (_securityManagerInfo.IsInclusion)
            {
                ou.AddNextActionItems(new TimeInterval(0, requestNonce.Id, DefaultTimeouts.SECURITY_S0_NONCE_REQUEST_INCLUSION_TIMEOUT));
            }
            if (_securityManagerInfo.DelaysS0.ContainsKey(SecurityS0Delays.NonceGet))
            {
                Thread.Sleep(_securityManagerInfo.DelaysS0[SecurityS0Delays.NonceGet]);
            }
        }

        private void OnNonceReport(ActionCompletedUnit ou)
        {
            AddTraceLogItems(requestNonce.SpecificResult.TraceLog);
            if (requestNonce.Result.State == ActionStates.Completed)
            {
                COMMAND_CLASS_SECURITY.SECURITY_NONCE_REPORT cmd = requestNonce.SpecificResult.Command;
                "NONCE REPORT {0}"._DLOG(requestNonce.SpecificResult.Command.GetHex());
                if (cmd.nonceByte != null && cmd.nonceByte.Count == 8)
                {
                    byte[] msg = _securityS0CryptoProvider.Encrypt(0, CommandToSecureSend, SrcNode, DestNode, cmd.nonceByte.ToArray());
                    sendEncData.Data = msg;
                    //sendEncData.OnHandledCallback = OnHandledCallback;
                    if (_securityManagerInfo.DelaysS0.ContainsKey(SecurityS0Delays.Command))
                    {
                        Thread.Sleep(_securityManagerInfo.DelaysS0[SecurityS0Delays.Command]);
                    }
                }
                else
                    SetStateFailed(ou);
            }
            else
                SetStateFailed(ou);
        }

        private void OnSendEncData(ActionCompletedUnit ou)
        {
            AddTraceLogItems(sendEncData.SpecificResult.TraceLog);
            if (sendEncData.Result.State == ActionStates.Completed)
            {
                SetStateCompleted(ou);
            }
            else
                SetStateFailed(ou);
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ActionResult();
        }
    }
}
