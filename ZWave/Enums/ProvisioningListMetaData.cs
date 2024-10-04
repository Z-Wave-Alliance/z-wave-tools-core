/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;

namespace ZWave.Enums
{
    /// <summary>
    /// MetaData options for provisioning list
    /// </summary>
    [Flags]
    public enum PLMetaData
    {
        CriticalFlag = 0x01,
        ProductType = 0x00,
        ProductId = 0x01,
        MaxInclusionRequestInterval = 0x02,
        UUID16 = 0x03,
        Name = 0x32,
        Location = 0x33,
        InclusionStatus = 0x34,
        AdvancedJoining = 0x35,
        BootstrappingMode = 0x36,
        NetworkStatus = 0x37,
    }

    public enum PListStatus
    {
        Pending = 0x00,
        Included = 0x01,
        Passive = 0x02,
        Ignore = 0x03,
    }

    public enum PLBootstrappingModeOpt
    {
        Security2 = 0x00,
        SmartStart = 0x01,
    }
}