/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Linq;
using Utils;
using ZWave.CommandClasses;
using ZWave.Devices;
using ZWave.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class RequestDataExOperation : ApiOperation
    {
        internal NodeTag SrcNode { get; set; }
        internal NodeTag DestNode { get; set; }
        internal byte[] Data { get; set; }
        internal TransmitOptions TxOptions { get; private set; }
        internal TransmitSecurityOptions TxSecOptions { get; private set; }
        internal SecuritySchemes SecurityScheme { get; private set; }
        internal TransmitOptions2 TxOptions2 { get; private set; }
        private byte CmdClass { get; set; }
        private byte Cmd { get; set; }
        private int TimeoutMs { get; set; }

        public RequestDataExOperation(NetworkViewPoint network, NodeTag srcNode, NodeTag destNode, byte[] data, TransmitOptions txOptions, TransmitSecurityOptions txSecOptions, SecuritySchemes scheme, TransmitOptions2 txOptions2, byte cmdClass, byte cmd, int timeoutMs)
            : base(false, null, false)
        {
            _network = network;
            SrcNode = srcNode;
            DestNode = destNode;
            Data = data;
            TxOptions = txOptions;
            TxSecOptions = txSecOptions;
            SecurityScheme = scheme;
            TxOptions2 = txOptions2;
            CmdClass = cmdClass;
            Cmd = cmd;
            TimeoutMs = timeoutMs;
        }

        SendDataExOperation sendData;
        ExpectDataOperation expectData;
        // detect not decrypted case requires expect 2 different nonce reports
        ExpectDataOperation expectNonceReport;

        public bool IsSendDataCompleted()
        {
            bool res = false;
            res = sendData != null && sendData.IsStateCompleted;
            return res;
        }

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(OnStart, 0, expectNonceReport, expectData, sendData));
            ActionUnits.Add(new ActionCompletedUnit(sendData, OnSendDataComleted));
            ActionUnits.Add(new ActionCompletedUnit(expectData, OnExpectDataComleted));
            ActionUnits.Add(new ActionCompletedUnit(expectNonceReport, OnExpectNonceReportComleted));
        }

        private void OnStart(StartActionUnit ou)
        {
            _nonceReportData = null;
            expectData.SrcNode = SrcNode;
            expectNonceReport.SrcNode = SrcNode;
            sendData.DstNode = DestNode;
            sendData.Data = Data;
        }

        private void OnSendDataComleted(ActionCompletedUnit ou)
        {
            AddTraceLogItems(ou.Action.Result.TraceLog);
            SpecificResult.CopyFrom(ou.Action.Result as SendDataResult);
            if (ou.Action.Result.State == ActionStates.Completed)
            {
                if (expectData.Result)
                {
                    SpecificResult.Node = expectData.SpecificResult.SrcNode;
                    SpecificResult.Command = expectData.SpecificResult.Command;
                    SpecificResult.RxRssi = expectData.SpecificResult.Rssi;
                    SpecificResult.RxSecurityScheme = expectData.SpecificResult.SecurityScheme;
                    SpecificResult.RxSubstituteStatus = expectData.SpecificResult.SubstituteStatus;
                    SetStateCompleted(ou);
                }
            }
            else
            {
                SetStateFailed(ou);
            }
        }

        private byte[] _nonceReportData = null;
        private void OnExpectNonceReportComleted(ActionCompletedUnit ou)
        {
            var nonce = (ou.Action.Result as ExpectDataResult)?.Command;
            var expRes = ou.Action.Result as ExpectDataResult;
            if (nonce != null && expRes != null)
            {
                nonce = ((COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT)nonce).receiversEntropyInput?.ToArray();
            }
            if (nonce != null && nonce.Length > 0 && ParentAction as RequestNodeInfoSecureTask != null)
            {
                var sm = ((RequestNodeInfoSecureTask)ParentAction).GetSecurityManagerInfo();
                if (sm.SpanTable.GetSpanState(new ZWave.Security.InvariantPeerNodeId(expRes.SrcNode, expRes.DestNode)) == ZWave.Security.SpanStates.Span)
                {
                    _nonceReportData = nonce;
                }
                if (_nonceReportData != null && !_nonceReportData.SequenceEqual(nonce))
                {
                    SetStateFailed(ou);
                }
            }
            expectNonceReport.NewToken();
            ou.SetNextActionItems(expectNonceReport);
        }

        private void OnExpectDataComleted(IActionUnit ou)
        {
            AddTraceLogItems(expectData.Result.TraceLog);
            if (expectData.Result)
            {
                SpecificResult.Node = expectData.SpecificResult.SrcNode;
                SpecificResult.Command = expectData.SpecificResult.Command;
                SpecificResult.RxRssi = expectData.SpecificResult.Rssi;
                SpecificResult.RxSecurityScheme = expectData.SpecificResult.SecurityScheme;
                SpecificResult.RxSubstituteStatus = expectData.SpecificResult.SubstituteStatus;
                if (sendData.Result)
                {
                    SetStateCompleted(ou);
                }
                else
                {
                    ou.Reset(500);
                    "Wait for completed callback"._DLOG();
                }
            }
            else
            {
                SetStateExpired(ou);
            }
        }

        protected override void CreateInstance()
        {
            sendData = new SendDataExOperation(_network, DestNode, Data, TxOptions, TxSecOptions, SecurityScheme, TxOptions2);
            sendData.SubstituteSettings = SubstituteSettings;
            expectData = new ExpectDataOperation(_network, NodeTag.Empty, DestNode, new byte[] { CmdClass, Cmd }, 2, TimeoutMs);
            expectNonceReport = new ExpectDataOperation(_network, NodeTag.Empty, DestNode, new COMMAND_CLASS_SECURITY_2.SECURITY_2_NONCE_REPORT(), 2, TimeoutMs);
        }

        public RequestDataResult SpecificResult
        {
            get { return (RequestDataResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new RequestDataResult();
        }
    }
}
