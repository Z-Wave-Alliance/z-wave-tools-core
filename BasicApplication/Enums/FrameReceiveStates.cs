/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿namespace ZWave.BasicApplication.Enums
{
    public enum FrameReceiveStates : byte
    {
        FRS_SOF_HUNT = 0x00,
        FRS_LENGTH = 0x01,
        FRS_TYPE = 0x02,
        FRS_COMMAND = 0x03,
        FRS_DATA = 0x04,
        FRS_CHECKSUM = 0x05,
        FRS_RX_TIMEOUT = 0x06,
        FRS_LENGHT2 = 0x07
    }
}
