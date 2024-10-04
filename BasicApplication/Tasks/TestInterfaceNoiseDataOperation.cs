/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Diagnostics;
using ZWave.BasicApplication.Operations;

namespace ZWave.BasicApplication.Tasks
{
    public class TestInterfaceNoiseDataOperation : ApiOperation
    {
        private readonly int _timeoutMs;
        private readonly byte[] _testInterfaceCmd;
        private Action<ReturnValueResult> _sendCallback;
        private int _intervalMs;

        private ActionBase _sendData;
        private TimeInterval _timeInterval;

        public TestInterfaceNoiseDataOperation(byte[] testInterfaceCmd, Action<ReturnValueResult> sendCallback, int timeoutMs = 200, int intervalMs = 100)
            :base(false, null, false)
        {
            _testInterfaceCmd = testInterfaceCmd ?? new byte[0];
            _sendCallback = sendCallback;
            _timeoutMs = timeoutMs;
            _intervalMs = intervalMs;
        }

        protected override void CreateInstance()
        {
            _sendData = new TestInterfaceSendDataOperation(_testInterfaceCmd, _timeoutMs);
            _timeInterval = new TimeInterval(GetNextCounter(), Id, _intervalMs);
        }

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 0, _sendData));
            ActionUnits.Add(new ActionCompletedUnit(_sendData, OnSendCompleted, _timeInterval));
            ActionUnits.Add(new TimeElapsedUnit(_timeInterval, null, 0, _sendData));
        }

        private void OnSendCompleted(ActionCompletedUnit ou)
        {
            SpecificResult.TotalCount++;
            if (!ou.Action.IsStateCompleted)
                SpecificResult.FailCount++;

            var sdRes = ((TestInterfaceSendDataOperation)ou.Action).SpecificResult;
            _sendCallback?.Invoke(sdRes);

            _sendData.NewToken();
            _timeInterval.IsHandled = false;
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
