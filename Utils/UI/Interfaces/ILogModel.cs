/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Collections.Generic;
using Utils.UI.Logging;

namespace Utils.UI.Interfaces
{
    public interface ILogModel
    {
        int QCapacity { get; }
        bool IsAutoScrollLogMessages {get;set;}
        Queue<LogPacket> Queue { get; set; }
        void Clear();
    }
}