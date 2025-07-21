/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;

namespace ZWave
{
    public class LRConfig
    {
        public byte LrConfig { get; set; }
        public string LrConfigName { get; set; }
        public LRConfig(byte lrConfig, string lrConfigName)
        {
            LrConfig = lrConfig;
            LrConfigName = lrConfigName;
        }
    }
}
