/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿namespace ZWave.BasicApplication.Enums
{
    public enum AssignStatuses
    {
        AssignComplete = 0x00,
        AssignNodeIdDone = 0x01,
        AssignRangeInfoUpdate = 0x02,
        AssignInfoPending = 0x03,
        AssignWaitingForFind = 0x04,
        AssignSmartStartInProgress = 0x05,
        AssignLearnInProgress = 0x06,
        AssignLearnModeCompletedTimeout = 0x07,
        AssignLearnModeCompletedFailed = 0x08,
    }
}
