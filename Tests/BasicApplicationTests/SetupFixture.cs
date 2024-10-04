/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using NUnit.Framework;
using Utils;
using ZWave.BasicApplication.Operations;
using ZWave.BasicApplication;
using ZWave.BasicApplication.TransportService.Operations;
using ZWave;
using ZWave.Security;
using ZWave.Devices;

namespace BasicApplicationTests
{
    [SetUpFixture]
    public class SetupFixture
    {
        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {

            #region setup timeouts
            DefaultTimeouts.EXPIRED_EXTRA_TIMEOUT = 50;

            DefaultTimeouts.REQUEST_NODE_INFO_TIMEOUT = 500;
            DefaultTimeouts.TRANSPORT_SERVICE_SEGMENT_COMPLETE_TIMEOUT = 200;

            DefaultTimeouts.SECURITY_S2_KEX_GET_TIMEOUT = 100;
            DefaultTimeouts.SECURITY_S2_KEX_SET_TIMEOUT = 100;
            DefaultTimeouts.SECURITY_S2_NONCE_REQUEST_INCLUSION_TIMEOUT = 100;
            DefaultTimeouts.SECURITY_S2_NONCE_REQUEST_TIMEOUT = 100;
            DefaultTimeouts.SECURITY_S0_NONCE_REQUEST_INCLUSION_TIMEOUT = 100;
            DefaultTimeouts.SECURITY_S0_NONCE_REQUEST_TIMEOUT = 100;

            InclusionS2TimeoutConstants.Joining.SetTestTimeouts(100);
            InclusionS2TimeoutConstants.Including.SetTestTimeouts(100);


            #endregion



            int TIMEOUT = 3455;

            SetLearnModeS0Operation.CMD_TIMEOUT = TIMEOUT;
            AddNodeS0Operation.CMD_TIMEOUT = TIMEOUT;


            SendDataSecureTask.NONCE_REQUEST_TIMER = TIMEOUT;
            SendDataSecureTask.NONCE_REQUEST_INCLUSION_TIMER = TIMEOUT;

            SendDataSecureS2Task.NONCE_REQUEST_TIMER = TIMEOUT;
            SendDataSecureS2Task.NONCE_REQUEST_INCLUSION_TIMER = TIMEOUT;

            //RequestNodeInfoSecureTask.CMD_SUPPORTED = TIMEOUT;
            RequestNodeInfoSecureTask.START_DELAY = 15;

            CallbackApiOperation.RET_TIMEOUT = TIMEOUT;
            CallbackApiOperation.CALLBACK_TIMEOUT = TIMEOUT;

            RequestApiOperation.RET_TIMEOUT = TIMEOUT;

            


            ActionToken.DefaultTimeout = 7777;
            ActionToken.ThrowExceptionOnDefaultTimeoutExpired = true;

            Tools.IsOutputToConsole = true;
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
        }
    }
}
