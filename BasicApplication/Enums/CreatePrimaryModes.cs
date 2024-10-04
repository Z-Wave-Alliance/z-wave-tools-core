/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.Enums;

namespace ZWave.BasicApplication.Enums
{
    public enum CreatePrimaryModes
    {
        Start = Modes.NodeController,
        Stop = Modes.NodeStop,
        StopFailed = Modes.NodeStopFailed
    }
}
