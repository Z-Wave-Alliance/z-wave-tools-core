/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;

namespace ZWave
{
    public interface IActionItem
    {
        Action<IActionItem> CompletedCallback { get; set; }
        ActionBase ParentAction { get; set; }
        int DataDelay { get; set; }
    }
}
