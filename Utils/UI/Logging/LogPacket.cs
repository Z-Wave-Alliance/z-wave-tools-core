/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using Utils.UI.Enums;

namespace Utils.UI.Logging
{
    public class LogPacket
    {
        public DateTime Timestamp { get; set; }
        public Dyes Dye { get; set; }
        public string Text { get; set; }
        public bool IsBold { get; set; }
        public LogLevels LogLevel { get; set; }
        public LogRawData LogRawData { get; set; }

        public LogPacket(string text, Dyes dye, bool isBold, LogLevels logLevel, LogRawData logRawData = null)
        {
            Timestamp = DateTime.Now;
            Dye = dye;
            Text = text;
            IsBold = isBold;
            LogLevel = logLevel;
            LogRawData = logRawData;
        }
    }
}
