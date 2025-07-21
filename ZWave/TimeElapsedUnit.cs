/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;

namespace ZWave
{
    public class TimeElapsedUnit : ActionUnit<TimeElapsedUnit>
    {
        public TimeElapsedUnit(ITimeoutItem timeInterval, Action<TimeElapsedUnit> func, int timeoutMs, params IActionItem[] items)
            : base(new TimeElapsedHandler(timeInterval.Id), func, timeoutMs, items)
        {
        }
    }
}
