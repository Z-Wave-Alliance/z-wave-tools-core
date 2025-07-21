/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace ZWave.ZipApplication.Data
{
    public class ZipFrame
    {
        public ZipFrame(DataFrame dataFrame)
        {
            Command = dataFrame.ToString();
        }

        public string Command { get; set; }
    }
}
