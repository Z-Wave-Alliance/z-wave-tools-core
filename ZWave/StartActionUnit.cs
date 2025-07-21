/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;

namespace ZWave
{
    public class StartActionUnit : ActionUnit<StartActionUnit>
    {
        public StartActionUnit(Action<StartActionUnit> func, int timeoutMs, params IActionItem[] items)
            : base(null, func, timeoutMs, items)
        {
        }
    }
}
