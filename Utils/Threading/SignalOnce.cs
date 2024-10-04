/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Diagnostics;
using System.Threading;

namespace Utils.Threading
{
    public class SignalOnce
    {
        private readonly object _locker = new object();
        private ManualResetEvent _signal;
        private volatile bool _isSet;

        public SignalOnce()
        {
        }

        [DebuggerHidden]
        public bool WaitOne(int timeoutMs)
        {
            bool ret = true;
            if (!_isSet)
            {
                lock (_locker)
                {
                    if (_signal == null)
                    {
                        _signal = new ManualResetEvent(false);
                    }
                }
                ret = _signal.WaitOne(timeoutMs);
                lock (_locker)
                {
                    _isSet = true;
                    _signal.Close();
                }
            }
            return ret;
        }

        [DebuggerHidden]
        public void Set()
        {
            if (!_isSet)
            {
                lock (_locker)
                {
                    _isSet = true;
                    if (_signal != null)
                    {
                        _signal.Set();
                    }
                }
            }
        }
    }
}
