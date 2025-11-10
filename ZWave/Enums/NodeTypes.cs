/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
/// SPDX-FileCopyrightText: Z-Wave Alliance https://z-wavealliance.org
namespace ZWave.Enums
{
    public enum NodeTypes : byte
    {
        None = 0xFF,
        ZWAVEPLUS_NODE = 0x00,
        ZWAVEPLUS_FOR_IP_GATEWAY = 0x02
    }
}
