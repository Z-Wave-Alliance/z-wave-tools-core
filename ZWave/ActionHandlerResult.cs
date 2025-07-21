/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;

namespace ZWave
{
    public class ActionHandlerResult
    {
        public ActionBase Parent { get; set; }
        public Action<bool> NextFramesCompletedCallback { get; set; }
        public List<IActionItem> NextActions { get; set; }

        private ActionHandlerResult()
        {
        }

        public ActionHandlerResult(ActionBase parent)
        {
            Parent = parent;
            NextActions = new List<IActionItem>();
        }
    }
}
