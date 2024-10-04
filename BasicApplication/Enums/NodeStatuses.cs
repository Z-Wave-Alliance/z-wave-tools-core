/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿namespace ZWave.BasicApplication.Enums
{
    public enum NodeStatuses
    {
        Unknown = 0,
        LearnReady = 1,
        NodeFound = 2,
        AddingRemovingEndDevice = 3,
        AddingRemovingController = 4,
        ProtocolDone = 5,
        Done = 6,
        Failed = 7,
        NotPrimary = 0x23
    }
}
