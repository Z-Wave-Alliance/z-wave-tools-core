/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.Enums;
using System.Threading;
using System;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    public class NoiseDataOperation : ApiOperation
    {
        protected TransmitOptions TxOptions { get; set; }
        internal NodeTag[] Nodes { get; set; }
        internal byte[] Data { get; set; }
        internal byte CmdClass { get; private set; }
        internal byte Cmd { get; private set; }
        internal int IntervalMs { get; private set; }
        internal int TimeoutMs { get; private set; }
        private Action<SendDataResult> _sendCallback;
        private Action<RequestDataResult> _requestCallback;
        internal int _currentIndex = 0;

        public NoiseDataOperation(NetworkViewPoint network, NodeTag[] nodes, byte[] data, TransmitOptions txOptions, byte cmdClass, byte cmd, Action<RequestDataResult> requestCallback, int intervalMs, int timeoutMs)
            : base(false, null, false)
        {
            _network = network;
            Nodes = nodes;
            Data = data;
            TxOptions = txOptions;
            CmdClass = cmdClass;
            Cmd = cmd;
            _requestCallback = requestCallback;
            IntervalMs = intervalMs;
            TimeoutMs = timeoutMs;
        }

        public NoiseDataOperation(NetworkViewPoint network, NodeTag[] nodes, byte[] data, TransmitOptions txOptions, Action<SendDataResult> sendCallback, int intervalMs, int timeoutMs)
            : base(false, null, false)
        {
            _network = network;
            Nodes = nodes;
            Data = data;
            TxOptions = txOptions;
            _sendCallback = sendCallback;
            IntervalMs = intervalMs;
            TimeoutMs = timeoutMs;
        }


        SendDataOperation sendData;
        RequestDataOperation requestData;
        TimeInterval timeInterval;

        protected override void CreateWorkflow()
        {
            if (Nodes != null && Nodes.Length > 0)
            {
                if (CmdClass == 0)
                {
                    ActionUnits.Add(new StartActionUnit(null, 0, sendData));
                    ActionUnits.Add(new ActionCompletedUnit(sendData, OnSendCompleted, timeInterval));
                    ActionUnits.Add(new TimeElapsedUnit(timeInterval, null, 0, sendData));
                }
                else
                {
                    ActionUnits.Add(new StartActionUnit(null, 0, requestData));
                    ActionUnits.Add(new ActionCompletedUnit(requestData, OnRequestCompleted, timeInterval));
                    ActionUnits.Add(new TimeElapsedUnit(timeInterval, null, 0, requestData));
                }
            }
        }

        protected override void CreateInstance()
        {
            sendData = new SendDataOperation(_network, Nodes[0], Data, TxOptions);
            sendData.SubstituteSettings = SubstituteSettings;

            requestData = new RequestDataOperation(_network, NodeTag.Empty, Nodes[0], Data, TxOptions, new[] { CmdClass, Cmd }, 2, TimeoutMs);
            requestData.SubstituteSettings = SubstituteSettings;

            timeInterval = new TimeInterval(GetNextCounter(), Id, IntervalMs);
        }

        private void OnSendCompleted(ActionCompletedUnit tu)
        {
            OnOperationCompletedBefore(tu);
            _sendCallback?.Invoke(sendData.SpecificResult);
            sendData.NewToken();
            sendData.Data = Data;
            sendData.DstNode = Nodes[NextNodeIndex()];
            timeInterval.IsHandled = false;
        }

        private int NextNodeIndex()
        {
            _currentIndex++;
            if (_currentIndex >= Nodes.Length)
            {
                _currentIndex = 0;
            }
            return _currentIndex;
        }

        private void OnRequestCompleted(ActionCompletedUnit tu)
        {
            OnOperationCompletedBefore(tu);
            _requestCallback?.Invoke(requestData.SpecificResult);
            requestData.NewToken();
            requestData.Data = Data;
            requestData.DstNode = Nodes[NextNodeIndex()];
            timeInterval.IsHandled = false;
        }

        private void OnOperationCompletedBefore(ActionCompletedUnit tu)
        {
            SpecificResult.TotalCount++;
            if (!tu.Action.IsStateCompleted)
                SpecificResult.FailCount++;
            else if (tu.Action.Result is TransmitResult &&
                    (tu.Action.Result as TransmitResult).TransmitStatus != TransmitStatuses.CompleteOk)
                SpecificResult.FailCount++;
        }

        public NoiseDataResult SpecificResult
        {
            get { return (NoiseDataResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new NoiseDataResult();
        }
    }

    public class NoiseDataResult : ActionResult
    {
        public int TotalCount { get; set; }
        public int FailCount { get; set; }
    }
}
