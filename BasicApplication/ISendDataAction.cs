/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZWave.Devices;

namespace ZWave.BasicApplication
{
    public interface ISendDataAction : IActionItem
    {
        SubstituteSettings SubstituteSettings { get; set; }
        Action<IActionItem> CompletedCallback { get; set; }
        byte[] Data { get; set; }
        NodeTag DstNode { get; set; }
    }
}
