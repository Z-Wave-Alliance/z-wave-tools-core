/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZWave.Xml.FrameHeader;
using ZWave.ZnifferApplication.Enums;
using ZWave.ZnifferApplication.Operations;

namespace ZWave.ZnifferApplication.Devices
{
    public interface ISnifferDevice
    {
        byte[] Frequencies { get; set; }
        byte Frequency { get; set; }
        ActionToken RestartListening(Func<ActionToken, DataItem, bool> onListenDataItemReceived);
        ActionToken ExpectData(byte[] homeId, byte? nodeId, Func<ActionToken, DataItem, bool> dataItemPredicate, int timeoutMs, Action<IActionItem> completedCallback);
        ActionToken CollectData(byte[] homeId, byte? nodeId, Func<ActionToken, DataItem, bool> dataItemPredicate, int timeoutMs, Action<IActionItem> completedCallback);
        ActionToken ListenData(ListenDataOperation.DataItemCallbackDelegate dataItemCallback, Action<IActionItem> completedCallback);
    }
}
