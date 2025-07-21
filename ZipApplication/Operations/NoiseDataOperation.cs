/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZWave.ZipApplication.Data;
using System.Threading;

namespace ZWave.ZipApplication.Operations
{
    public class NoiseDataOperation : ZipApiOperation
    {
        private byte[] Data { get; set; }
        private byte CmdClass { get; set; }
        private byte Cmd { get; set; }
        private int IntervalMs { get; set; }
        private int TimeoutMs { get; set; }
        public NoiseDataOperation(byte[] headerExtension, byte[] data, byte cmdClass, byte cmd, int intervalMs, int timeoutMs)
            : base(false)
        {
            Data = data;
            _headerExtension = headerExtension;
            CmdClass = cmdClass;
            Cmd = cmd;
            IntervalMs = intervalMs;
            TimeoutMs = timeoutMs;
        }

        public NoiseDataOperation(byte[] headerExtension, byte[] data, int intervalMs)
            : this(headerExtension, data, 0, 0, intervalMs, 0)
        {
        }

        SendDataOperation sendFirst;
        ActionCompletedUnit sendCompleted;
        RequestDataOperation requestFirst;
        ActionCompletedUnit requestCompleted;

        protected override void CreateWorkflow()
        {
            if (CmdClass == 0)
            {
                ActionUnits.Add(new StartActionUnit(null, 0, sendFirst));
                ActionUnits.Add(sendCompleted);
            }
            else
            {
                ActionUnits.Add(new StartActionUnit(null, 0, requestFirst));
                ActionUnits.Add(requestCompleted);
            }
        }

        protected override void CreateInstance()
        {
            sendFirst = new SendDataOperation(_headerExtension, Data);
            sendCompleted = new ActionCompletedUnit(sendFirst, OnSendCompleted);

            requestFirst = new RequestDataOperation(_headerExtension, Data, CmdClass, Cmd, TimeoutMs);
            requestCompleted = new ActionCompletedUnit(requestFirst, OnRequestCompleted);
        }

        private void OnSendCompleted(ActionCompletedUnit tu)
        {
            OnOperationCompletedBefore(tu);
            SendDataOperation op = new SendDataOperation(_headerExtension, Data);
            OnOperationCompletedAfter(tu, sendCompleted, op);
        }

        private void OnRequestCompleted(ActionCompletedUnit tu)
        {
            OnOperationCompletedBefore(tu);
            RequestDataOperation op = new RequestDataOperation(_headerExtension, Data, CmdClass, Cmd, TimeoutMs);
            OnOperationCompletedAfter(tu, requestCompleted, op);
        }

        private void OnOperationCompletedBefore(ActionCompletedUnit tu)
        {
            if (IntervalMs > 0)
                Thread.Sleep(IntervalMs);
            SpecificResult.TotalCount++;
            if (!tu.Action.IsStateCompleted)
                SpecificResult.FailCount++;
        }

        private void OnOperationCompletedAfter(ActionCompletedUnit taskUnit, ActionCompletedUnit completedTaskUnit, ActionBase op)
        {
            op.NewToken();
            taskUnit.SetNextActionItems(op);
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
