/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.Devices;

namespace ZWave.Security
{
    public static class InclusionS2TimeoutConstants
    {
        public static class Including
        {
            private static int? _testTimeout;
            public static int KexReport { get { return _testTimeout ?? 10000; } }
            public static int PublicKeyReport { get { return _testTimeout ?? 10000; } }
            public static int NetworkKeyGet { get { return _testTimeout ?? 10000; } }
            public static int NetworkKeyVerify { get { return _testTimeout ?? 10000; } }
            public static int TransferEnd { get { return _testTimeout ?? 10000; } }
            public static int UserInputKeyList { get { return _testTimeout ?? DefaultTimeouts.SECURITY_S2_KEX_SET_TIMEOUT; } }
            public static int UserInputDsk { get { return _testTimeout ?? 240000; } }
            public static void SetTestTimeouts(int timeout)
            {
                _testTimeout = timeout;
            }
        }

        public static class Joining
        {
            private static int? _testTimeout;
            public static int KexGet { get { return _testTimeout ?? DefaultTimeouts.SECURITY_S2_KEX_GET_TIMEOUT; } }
            public static int KexSet { get { return _testTimeout ?? Including.UserInputKeyList; } }
            public static int PublicKeyReport { get { return _testTimeout ?? 10000; } }
            public static int NetworkKeyReport { get { return _testTimeout ?? 10000; } }
            public static int TransferEnd { get { return _testTimeout ?? 10000; } }
            public static int UserInputCsa { get { return _testTimeout ?? 240000; } }
            public static void SetTestTimeouts(int timeout)
            {
                _testTimeout = timeout;
            }
        }
    }
}
