/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿namespace ZWave.Enums
{
    public enum ControllerChangeModes
    {
        Start = Modes.NodeController | Modes.NodeOptionNormalPower,
        Stop = Modes.NodeStop,
        StopFailed = Modes.NodeStopFailed
    }
}
