/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;

namespace ZWave
{
    public class StopActionUnit : ActionUnit<StopActionUnit>
    {
        public StopActionUnit(Action<StopActionUnit> func, int timeoutMs, params IActionItem[] items)
            : base(null, func, 0, items)
        {
        }

        public StopActionUnit(IActionItem item)
          : base(null, null, 0, item)
        {
        }
    }
}
