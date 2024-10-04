/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.Enums;

namespace ZWave.Devices
{
    public interface IController : IDevice
    {
        ControllerRoles NetworkRole { get; }
        NodeTag[] IncludedNodes { get; set; }
    }
}
