/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
namespace ZWave.BasicApplication.Enums
{
    public enum FailedNodeStatuses
    {
        NodeOk = 0x00,
        NodeRemoved = 0x01,
        NodeNotRemoved = 0x02,
        NodeReplace = 0x03,
        NodeReplaceDone = 0x04,
        NodeReplaceFailed = 0x05
    }
}
