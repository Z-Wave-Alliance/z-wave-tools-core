/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;

namespace ZWave.BasicApplication.Enums
{
    [Flags]
    public enum SetupNodeLifelineSettings
    {
        Default = 0,
        SkipAllSteps = 1,
        IsDeleteReturnRoute = 2,
        IsAssignReturnRoute = 4,
        IsAssociationCreate = 8,
        IsMultichannelAssociationCreate = 16,
        IsWakeUpCapabilities = 32,
        IsWakeUpInterval = 64,
        IsSetAsSisAutomatically = 128,
        IsBasedOnZwpRoleType = 256,
        SetAllSteps = IsDeleteReturnRoute | IsAssignReturnRoute | IsAssociationCreate | IsMultichannelAssociationCreate | IsWakeUpCapabilities | IsWakeUpInterval | IsSetAsSisAutomatically | IsBasedOnZwpRoleType,
        DisableAutoSisOnly = SetAllSteps & ~IsSetAsSisAutomatically
    }
}
