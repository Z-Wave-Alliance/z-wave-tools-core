/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿namespace ZWave.BasicApplication.Enums
{
    public enum SucUpdateStatuses
    {
        /// <summary>
        /// The update process succeeded
        /// </summary>
        Done = 0x00,
        /// <summary>
        /// The update process aborted because of an error
        /// </summary>
        Abort = 0x01,
        /// <summary>
        /// The SUC node is busy.
        /// </summary>
        Wait = 0x02,
        /// <summary>
        /// The SUC functionality is disabled
        /// </summary>
        Disabled = 0x03,
        /// <summary>
        /// The controller requested an update after more than 64 changes have occurred in the network
        /// </summary>
        Overflow = 0x04
    }
}
