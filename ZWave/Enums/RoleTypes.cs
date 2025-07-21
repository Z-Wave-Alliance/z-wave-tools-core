/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
namespace ZWave.Enums
{
    public enum RoleTypes
    {
        None = 0xFF,
        CONTROLLER_CENTRAL_STATIC = 0x00,
        CONTROLLER_SUB_STATIC = 0x01,
        CONTROLLER_PORTABLE = 0x02,
        CONTROLLER_PORTABLE_REPORTING = 0x03,
        END_NODE_PORTABLE = 0x04,
        END_NODE_ALWAYS_ON = 0x05,
        END_NODE_SLEEPING_REPORTING = 0x06,
        END_NODE_SLEEPING_LISTENING = 0x07
    }
}
