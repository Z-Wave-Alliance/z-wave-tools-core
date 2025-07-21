/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZWave.ZnifferApplication.Enums
{
    public enum Frame4xReceiveStates
    {
        SOF_HUNT,
        TYPE,
        TIME_STAMP_1,
        TIME_STAMP_2,
        WAKE_UP_START_TIME_STAMP_1,
        WAKE_UP_START_TIME_STAMP_2,
        WAKE_UP_STOP_TIME_STAMP_1,
        WAKE_UP_STOP_TIME_STAMP_2,
        SPEED_CHANNEL,
        WAKE_UP_START_SPEED_CHANNEL,
        CURRENT_FREQUENCY,
        WAKE_UP_START_CURRENT_FREQUENCY,
        RSSI,
        WAKE_UP_START_RSSI,
        DATA_MARKER,
        SOF_DATA,
        DATA_LENGTH,
        DATA,
        BEAM_TYPE,
        WAKE_UP_START_BEAM_TYPE,
        BEAM_DESTINATION,
        WAKE_UP_START_BEAM_DESTINATION,
        WAKE_UP_START_BEAM_VERSION,
        WAKE_UP_START_HOME_ID_HASH,
        WAKE_UP_STOP_RSSI,
        WAKE_UP_STOP_COUNT_1,
        WAKE_UP_STOP_COUNT_2,
        CMD_TYPE,
        CMD_LENGTH,
        CMD_DATA
    }
}
