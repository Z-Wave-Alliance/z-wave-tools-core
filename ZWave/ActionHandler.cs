/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿namespace ZWave
{
    public interface IActionHandler
    {
        HandlerStates State { get; set; }
        bool WaitingFor(IActionCase actionCase);
    }

    public abstract class ActionHandler : IActionHandler
    {
        public HandlerStates State { get; set; }
        public abstract bool WaitingFor(IActionCase actionCase);
    }
}
