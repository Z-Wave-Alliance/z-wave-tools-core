/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;

namespace ZWave.Enums
{
    [Flags]
    public enum TransmitSecurityOptions
    {
        NONE = 0x00,
        S2_TXOPTION_VERIFY_DELIVERY = 0x01,
        S2_TXOPTION_SINGLECAST_FOLLOWUP = 0x02,
        S2_TXOPTION_FIRST_SINGLECAST_FOLLOWUP = 0x04
    }
}
