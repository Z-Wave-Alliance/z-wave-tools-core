/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using Utils.UI.Enums;

namespace Utils.UI.Logging
{
    public class LogSettings
    {
        public LogLevels Level { get; set; } = LogLevels.Title;
        public LogIndents IndentBefore { get; set; } = LogIndents.Current;
        public LogIndents IndentAfter { get; set; } = LogIndents.Current;
    }
}
