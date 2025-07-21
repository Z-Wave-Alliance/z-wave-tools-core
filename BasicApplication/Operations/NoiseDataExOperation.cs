/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.Enums;
using System.Threading;
using ZWave.Devices;
using System;

namespace ZWave.BasicApplication.Operations
{
    public class NoiseDataExOperation : ApiOperation
    {
        protected TransmitOptions TxOptions { get; set; }
        protected TransmitOptions2 TxOptions2 { get; set; }
        internal NodeTag[] Nodes { get; set; }
        internal byte[] Data { get; set; }
        internal byte CmdClass { get; private set; }
        internal byte Cmd { get; private set; }
        internal int IntervalMs { get; private set; }
        internal int TimeoutMs { get; private set; }
        internal bool IsPoisson { get; private set; } = false;

        private Action<SendDataResult> _sendCallback;
        private Action<RequestDataResult> _requestCallback;
        internal TransmitSecurityOptions TxSecOptions { get; private set; }
        internal SecuritySchemes SecurityScheme { get; private set; }
        internal int _currentIndex = 0;

        public NoiseDataExOperation(NetworkViewPoint network, NodeTag[] nodes, byte[] data, TransmitOptions txOptions, byte cmdClass, byte cmd,
            int intervalMs, int timeoutMs, SecuritySchemes securityScheme, TransmitSecurityOptions txSecOptions, TransmitOptions2 txOptions2, bool isPoisson = false)
            : base(false, null, false)
        {
            _network = network;
            Nodes = nodes;
            Data = data;
            TxOptions = txOptions;
            CmdClass = cmdClass;
            Cmd = cmd;
            IntervalMs = intervalMs;
            TimeoutMs = timeoutMs;
            SecurityScheme = securityScheme;
            TxSecOptions = txSecOptions;
            TxOptions2 = txOptions2;
            IsPoisson = isPoisson;
        }

        public NoiseDataExOperation(NetworkViewPoint network, NodeTag[] nodes, byte[] data, TransmitOptions txOptions, int intervalMs, int timeoutMs,
            SecuritySchemes securityScheme, TransmitSecurityOptions txSecOptions, TransmitOptions2 txOptions2, bool isPoisson = false)
            : base(false, null, false)
        {
            _network = network;
            Nodes = nodes;
            Data = data;
            TxOptions = txOptions;
            IntervalMs = intervalMs;
            TimeoutMs = timeoutMs;
            SecurityScheme = securityScheme;
            TxSecOptions = txSecOptions;
            TxOptions2 = txOptions2;
            IsPoisson = isPoisson;
        }

        SendDataExOperation sendData;
        RequestDataExOperation requestData;
        ITimeoutItem timeInterval;
        //PoissonTimeInterval poissonTimeInterval;

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
                    ActionUnits.Add(new ActionCompletedUnit(sendData, OnSendCompleted, timeInterval));
                    ActionUnits.Add(new TimeElapsedUnit(timeInterval, null, 0, requestData));
                }
            }
        }

        protected override void CreateInstance()
        {
            sendData = new SendDataExOperation(_network, Nodes[0], Data, TxOptions, TxSecOptions, SecurityScheme, TxOptions2);
            sendData.SubstituteSettings = SubstituteSettings;

            requestData = new RequestDataExOperation(_network, NodeTag.Empty, Nodes[0], Data, TxOptions, TxSecOptions, SecurityScheme, TxOptions2, CmdClass, Cmd, TimeoutMs);
            requestData.SubstituteSettings = SubstituteSettings;

            if (IsPoisson)
            {
                timeInterval = new PoissonTimeInterval(GetNextCounter(), Id, IntervalMs);
            }
            else
            {
                timeInterval = new TimeInterval(GetNextCounter(), Id, IntervalMs);
            }
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
            requestData.DestNode = Nodes[NextNodeIndex()];
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
}
