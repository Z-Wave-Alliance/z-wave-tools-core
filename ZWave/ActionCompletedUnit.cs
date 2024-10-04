/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;

namespace ZWave
{
    public class ActionCompletedUnit : ActionUnit<ActionCompletedUnit>
    {
        public ActionBase Action
        {
            get { return ((ActionCompletedHandler)ActionHandler).Action; }
        }

        public ActionCompletedUnit(ActionBase action, Action<ActionCompletedUnit> func, params IActionItem[] items)
            : base(new ActionCompletedHandler(action), func, 0, items)
        {
        }

        public ActionCompletedUnit(ActionBase action, Func<ActionCompletedUnit, bool?> predicate, IActionItem item, IActionItem falseItem, IActionItem undefinedItem)
            : base(new ActionCompletedHandler(action), predicate, 0, item, falseItem, undefinedItem)
        {

        }
    }
}
