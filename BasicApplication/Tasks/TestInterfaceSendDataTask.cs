/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using ZWave.BasicApplication.Operations;

namespace ZWave.BasicApplication.Tasks
{
    public class TestInterfaceSendDataTask : ActionGroup
    {
        private readonly int _timeoutMs;
        private readonly byte[] _testInterfaceCmd;

        public TestInterfaceSendDataTask(byte[] testInterfaceCmd, int timeoutMs)
        {
            _testInterfaceCmd = testInterfaceCmd ?? new byte[0];
            _timeoutMs = timeoutMs;
        }

        ActionBase sendData;

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, _timeoutMs, sendData));
            ActionUnits.Add(new ActionCompletedUnit(sendData, OnRetDataReceived));
        }

        protected override void CreateInstance()
        {
            sendData = new TestInterfaceSendDataOperation(_testInterfaceCmd, _timeoutMs);
        }

        private void OnRetDataReceived(ActionCompletedUnit ou)
        {
            SpecificResult.SendData = ((TestInterfaceSendDataOperation)ou.Action).SpecificResult;
            SetStateCompleted(ou);
        }

        public TestInterfaceSendDataResult SpecificResult
        {
            get { return (TestInterfaceSendDataResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new TestInterfaceSendDataResult();
        }
    }

    public class TestInterfaceSendDataResult : ActionResult
    {
        public ReturnValueResult HasFirmWare { get; set; }
        public ReturnValueResult SendData { get; set; }
    }
}
