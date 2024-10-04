/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZWave.ZnifferApplication.Enums
{
    public enum Frame3xReceiveStates
    {
        SOF_HUNT,
        TIME_STAMP_1,
        TIME_STAMP_2,
        DATA_MARKER,
        SPEED,
        DATA_SEPARATOR,
        DATA,
        WAKE_UP_MARKER,
        WAKE_UP_1,
        CMD_DATA,
        CMD_DATA_49,
        CMD_DATA_46,
        CMD_DATA_46_LEN,
    }
}
