/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Linq;
using System.Threading;
using Utils;
using ZWave.BasicApplication.Security;
using ZWave.CommandClasses;
using ZWave.Devices;
using ZWave.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class SendDataSecureTask : ApiOperation
    {
        #region Timeouts
        /// <summary>
        /// Nonce Request Timer
        /// </summary>
        public static int NONCE_REQUEST_INCLUSION_TIMER = 10000;
        /// <summary>
        /// Nonce Request Timer
        /// </summary>
        public static int NONCE_REQUEST_TIMER = 20000;
        #endregion

        protected TransmitOptions TxOptions { get; set; }
        internal byte[] CommandToSecureSend { get; private set; }
        internal NodeTag Node { get; private set; }
        internal NodeTag? TestNode { get; set; }
        private SecurityS0CryptoProvider _securityS0CryptoProvider;
        private SecurityManagerInfo _securityManagerInfo;

        public Action<IActionUnit> OnHandledCallback { get; set; }
        internal SendDataSecureTask(NetworkViewPoint network, SecurityManagerInfo securityManagerInfo, SecurityS0CryptoProvider securityS0CryptoProvider, NodeTag node, byte[] data, TransmitOptions txOptions)
            : base(false, null, false)
        {
            _network = network;
            _securityManagerInfo = securityManagerInfo;
            _securityS0CryptoProvider = securityS0CryptoProvider;
            SubstituteSettings.SetFlag(SubstituteFlags.DenySecurity);
            CommandToSecureSend = data;
            Node = node;
            TxOptions = txOptions;
        }

        RequestDataOperation _requestNonce;
        SendDataOperation _sendEncData;

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(OnStart, 0, _requestNonce));
            ActionUnits.Add(new ActionCompletedUnit(_requestNonce, OnNonceReport, _sendEncData));
            ActionUnits.Add(new ActionCompletedUnit(_sendEncData, OnSendEncData));
        }

        protected override void CreateInstance()
        {
            _requestNonce = new RequestDataOperation(_network, NodeTag.Empty, Node,
                new COMMAND_CLASS_SECURITY.SECURITY_NONCE_GET(), TxOptions,
                new COMMAND_CLASS_SECURITY.SECURITY_NONCE_REPORT(), 2, NONCE_REQUEST_TIMER);

            _requestNonce.SubstituteSettings.SetFlag(SubstituteFlags.DenySecurity);

            _sendEncData = new SendDataOperation(_network, TestNode ?? Node, null, TxOptions);
            _sendEncData.SubstituteSettings.SetFlag(SubstituteFlags.DenyTransportService | SubstituteFlags.DenySecurity);
        }

        protected override void SetStateFailed(IActionUnit ou)
        {
            base.SetStateFailed(ou);
        }

        private void OnStart(StartActionUnit ou)
        {
            if (_securityManagerInfo.IsInclusion)
            {
                ou.AddNextActionItems(new TimeInterval(0, _requestNonce.Id, DefaultTimeouts.SECURITY_S0_NONCE_REQUEST_INCLUSION_TIMEOUT));
            }
            if (_securityManagerInfo.DelaysS0.ContainsKey(SecurityS0Delays.NonceGet))
            {
                Thread.Sleep(_securityManagerInfo.DelaysS0[SecurityS0Delays.NonceGet]);
            }
        }

        private void OnNonceReport(ActionCompletedUnit ou)
        {
            AddTraceLogItems(_requestNonce.SpecificResult.TraceLog);
            SpecificResult.TransmitStatus = (_requestNonce.Result as TransmitResult).TransmitStatus;
            if (_requestNonce.Result.State == ActionStates.Completed)
            {
                COMMAND_CLASS_SECURITY.SECURITY_NONCE_REPORT cmd = _requestNonce.SpecificResult.Command;
                "NONCE REPORT {0}"._DLOG(_requestNonce.SpecificResult.Command.GetHex());
                if (cmd.nonceByte != null && cmd.nonceByte.Count == 8)
                {
                    byte[] msg = _securityS0CryptoProvider.Encrypt(0, CommandToSecureSend, _securityManagerInfo.Network.NodeTag, Node, cmd.nonceByte.ToArray());
                    _sendEncData.Data = msg;
                    if (DataDelay > 0)
                    {
                        Thread.Sleep(DataDelay);
                    }
                    else if (_securityManagerInfo.DelaysS0.ContainsKey(SecurityS0Delays.Command))
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
            AddTraceLogItems(_sendEncData.SpecificResult.TraceLog);
            SpecificResult.CopyFrom(_requestNonce.Result as SendDataResult);
            SpecificResult.AggregateWith(_sendEncData.Result as SendDataResult);
            if (_sendEncData.Result.State == ActionStates.Completed)
            {
                SpecificResult.TxSubstituteStatus = SubstituteStatuses.Done;
                SetStateCompleted(ou);
            }
            else
            {
                SpecificResult.TxSubstituteStatus = SubstituteStatuses.Failed;
                SetStateFailed(ou);
            }
        }

        public SendDataResult SpecificResult
        {
            get { return (SendDataResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new SendDataResult();
        }
    }
}
