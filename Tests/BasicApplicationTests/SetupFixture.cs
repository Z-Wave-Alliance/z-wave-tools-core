// SPDX-License-Identifier: BSD-3-Clause
// SPDX-FileCopyrightText: Silicon Laboratories Inc. <https://www.silabs.com>
// SPDX-FileCopyrightText: Z-Wave Alliance <https://z-wavealliance.org>
using System;
using System.Threading;
using NUnit.Framework;
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
            // Emulated controllers block test threads on WaitCompletedSignal() while completion
            // callbacks run via ThreadPool.QueueUserWorkItem. Slow thread-pool injection (~1-2/sec)
            // can stall those callbacks ~1s and overrun the short S2 timeouts below; a higher
            // minimum thread count keeps the tests deterministic.
            ThreadPool.GetMinThreads(out int minWorker, out int minIo);
            ThreadPool.SetMinThreads(Math.Max(minWorker, 128), Math.Max(minIo, 128));

            #region setup timeouts
            DefaultTimeouts.EXPIRED_EXTRA_TIMEOUT = 50;

            DefaultTimeouts.REQUEST_NODE_INFO_TIMEOUT = 500;
            DefaultTimeouts.TRANSPORT_SERVICE_SEGMENT_COMPLETE_TIMEOUT = 200;

            // Secure-handshake windows. 750 ms gives headroom to ride out thread-scheduling spikes
            // on constrained CI runners (the handshake itself completes in ~10 ms), while staying
            // below the frame delays the negative *Delay_FailsS2Inclusion tests inject (fixed
            // 1000 ms, or timeout-relative) so those still observe the expected timeout.
            DefaultTimeouts.SECURITY_S2_KEX_GET_TIMEOUT = 750;
            DefaultTimeouts.SECURITY_S2_KEX_SET_TIMEOUT = 750;
            DefaultTimeouts.SECURITY_S2_NONCE_REQUEST_INCLUSION_TIMEOUT = 750;
            DefaultTimeouts.SECURITY_S2_NONCE_REQUEST_TIMEOUT = 750;
            DefaultTimeouts.SECURITY_S0_NONCE_REQUEST_INCLUSION_TIMEOUT = 750;
            DefaultTimeouts.SECURITY_S0_NONCE_REQUEST_TIMEOUT = 750;

            InclusionS2TimeoutConstants.Joining.SetTestTimeouts(750);
            InclusionS2TimeoutConstants.Including.SetTestTimeouts(750);


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
