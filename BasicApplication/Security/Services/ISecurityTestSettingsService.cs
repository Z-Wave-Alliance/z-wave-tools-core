/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Operations;

namespace ZWave.BasicApplication.Security
{
    public interface ISecurityTestSettingsService
    {
        bool ActivateTestPropertiesForFrame(SecurityS2TestFrames testFrameType, ISendDataAction apiOperation);
    }
}