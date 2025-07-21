/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZWave.ZnifferApplication.Enums
{
    public enum CommandTypes
    {
        DataHandler = 0x00,

        // Zwave NCP Zniffer (Serial) Commands
        GetVersion4x = 0x01,
        SetFrequency4x = 0x02, 
        GetFrequencies4x = 0x03,
        Start4x = 0x04,
        Stop4x = 0x05,
        SetLRChConfig = 0x06,
        GetLRChConfigs = 0x07,
        GetLRRegions = 0x08,
        SetBaudRate4x = 0x0E,
        GetFrequencyStr4x = 0x13,
        GetLRChConfigStr = 0x14,
        
        // UZB Zniffer Commands
        GetDeviceInfo3x = 0x49,
        SetFrequency3x = 0x46,
        GetDeviceInfo3xResponse = 0x26,
        SetFrequency3xResponse = 0x2E
    }
}
