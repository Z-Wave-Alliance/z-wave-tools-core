/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZWave.ProgrammerApplication.Enums
{
    public enum FrameReceiveStates
    {
        FRS_SOF_HUNT = 0,
        FRS_DATA = 1,
        FRS_EOF = 2,
    }
}
