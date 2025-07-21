/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using Utils;
using ZWave.Devices;
using ZWave.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class RequestDataOperation : ApiOperation, ISendDataAction
    {
        internal int TimeoutMs { get; set; }
        internal NodeTag SrcNode { get; set; }
        public NodeTag DstNode { get; set; }
        public byte[] Data { get; set; }
        internal ReceiveStatuses RxStatuses { get; set; }
        internal ReceiveStatuses IgnoreRxStatuses { get; set; }
        internal bool IsFollowup { get; set; }
        internal TransmitOptions TxOptions { get; set; }
        private ByteIndex[] _dataToCompare;
        internal ByteIndex[] DataToCompare
        {
            get { return _dataToCompare; }
            set
            {
                _dataToCompare = value;
                if (ExpectData != null)
                {
                    ExpectData.SetDataToCompare(_dataToCompare);
                }
            }
        }

        public Action SendDataSubstituteCallback { get; set; }

        public RequestDataOperation(NetworkViewPoint network, NodeTag srcNode, NodeTag dstNode, byte[] data, TransmitOptions txOptions, byte[] dataToCompare, int bytesToCompare, int timeoutMs)
            : this(network, srcNode, dstNode, data, txOptions, timeoutMs)
        {
            _dataToCompare = new ByteIndex[bytesToCompare];
            for (int i = 0; i < bytesToCompare; i++)
            {
                _dataToCompare[i] = new ByteIndex(dataToCompare[i]);
            }
        }

        public RequestDataOperation(NetworkViewPoint network, NodeTag srcNode, NodeTag destNode, byte[] data, TransmitOptions txOptions, ByteIndex[] dataToCompare, int timeoutMs)
            : this(network, srcNode, destNode, data, txOptions, timeoutMs)
        {
            _dataToCompare = dataToCompare;
        }

        public RequestDataOperation(NetworkViewPoint network, NodeTag srcNode, NodeTag dstNode, byte[] data, TransmitOptions txOptions, int timeoutMs)
            : base(false, null, false)
        {
            _network = network;
            TimeoutMs = timeoutMs;
            SrcNode = srcNode;
            DstNode = dstNode;
            Data = data;
            TxOptions = txOptions;
        }

        private CallbackApiOperation _sendData;
        public ExpectDataOperation ExpectData { get; private set; }

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(OnStart, 0, ExpectData, _sendData));
            ActionUnits.Add(new ActionCompletedUnit(_sendData, OnSendDataComleted));
            ActionUnits.Add(new ActionCompletedUnit(ExpectData, OnExpectDataComleted));
        }

        public void SetNewExpectTimeout(int timeoutMs)
        {
            TimeoutMs = timeoutMs;
        }

        protected virtual void OnStart(StartActionUnit ou)
        {
            ExpectData.DstNode = SrcNode;
            ExpectData.RxStatuses = RxStatuses;
            ExpectData.IgnoreRxStatuses = IgnoreRxStatuses;

            var sd = _sendData as SendDataOperation;
            if (sd != null)
            {
                sd.SrcNode = SrcNode;
                sd.DstNode = DstNode;
                sd.Data = Data;
            }
            var sdb = _sendData as SendDataBridgeOperation;
            if (sdb != null)
            {
                sdb.SrcNode = SrcNode;
                sdb.DstNode = DstNode;
                sdb.Data = Data;
            }
            _sendData.DataDelay = DataDelay;
            _sendData.TimeoutMs = TimeoutMs;
        }

        private void OnSendDataComleted(ActionCompletedUnit ou)
        {
            AddTraceLogItems(ou.Action.Result.TraceLog);
            SpecificResult.CopyFrom(ou.Action.Result as SendDataResult);
            if (ou.Action.Result.State == ActionStates.Completed)
            {
                if (ExpectData.Result)
                {
                    SpecificResult.Node = ExpectData.SpecificResult.SrcNode;
                    SpecificResult.Command = ExpectData.SpecificResult.Command;
                    SpecificResult.RxRssi = ExpectData.SpecificResult.Rssi;
                    SpecificResult.RxSecurityScheme = ExpectData.SpecificResult.SecurityScheme;
                    SpecificResult.RxSubstituteStatus = ExpectData.SpecificResult.SubstituteStatus;
                    SetStateCompleted(ou);
                }
            }
            else
            {
                ExpectData.SetCancelled();
                SetStateFailed(ou);
            }
        }

        private void OnExpectDataComleted(IActionUnit ou)
        {
            AddTraceLogItems(ExpectData.Result.TraceLog);
            if (ExpectData.Result)
            {
                SpecificResult.Node = ExpectData.SpecificResult.SrcNode;
                SpecificResult.Command = ExpectData.SpecificResult.Command;
                SpecificResult.RxRssi = ExpectData.SpecificResult.Rssi;
                SpecificResult.RxSecurityScheme = ExpectData.SpecificResult.SecurityScheme;
                SpecificResult.RxSubstituteStatus = ExpectData.SpecificResult.SubstituteStatus;
                if (_sendData.Result)
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
            if (SrcNode.Id == 0)
            {
                _sendData = new SendDataOperation(_network, SrcNode, DstNode, Data, TxOptions)
                {
                    IsFollowup = IsFollowup,
                    SubstituteCallback = SendDataSubstituteCallback
                };
            }
            else
            {
                _sendData = new SendDataBridgeOperation(_network, SrcNode, DstNode, Data, TxOptions)
                {
                    IsFollowup = IsFollowup,
                    SubstituteCallback = SendDataSubstituteCallback
                };
            }
            _sendData.DataDelay = DataDelay;
            _sendData.SubstituteSettings = SubstituteSettings;

            ExpectData = new ExpectDataOperation(_network, SrcNode, DstNode, _dataToCompare, TimeoutMs);
            ExpectData.IsHandler = IsHandler;
        }

        public RequestDataResult SpecificResult
        {
            get { return (RequestDataResult)Result; }
        }

        public bool IsHandler { get; internal set; }

        protected override ActionResult CreateOperationResult()
        {
            return new RequestDataResult();
        }
    }

    public class RequestDataResult : SendDataResult
    {
        public NodeTag Node { get; set; }
        public bool IsBroadcast { get; set; }
        public byte[] Command { get; set; }
        public sbyte RxRssi { get; set; }
        public SecuritySchemes RxSecurityScheme { get; set; }
        public SubstituteStatuses RxSubstituteStatus { get; set; }
    }
}
