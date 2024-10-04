/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿namespace ZWave
{
    public class ActionCompletedHandler : ActionHandler
    {
        public int ActionId { get; private set; }
        public ActionBase Action { get; private set; }

        public ActionCompletedHandler(ActionBase action)
        {
            ActionId = action.Id;
        }

        public override bool WaitingFor(IActionCase actionCase)
        {
            bool ret = false;
            if (actionCase is ActionBase)
            {
                ActionBase receivedValue = (ActionBase)actionCase;
                if (ActionId == receivedValue.Id)
                {
                    Action = receivedValue;
                    ret = true;
                }
            }
            return ret;
        }
    }
}
