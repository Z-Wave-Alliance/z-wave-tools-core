/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿namespace ZWave.Enums
{
    /// <summary>
    /// Controller Capabilities enumeration
    /// </summary>
    public enum ControllerCapabilities : byte
    {
        /// <summary>
        /// Return value mask for ZWaveGetControllerCapabilities
        /// Controller is secondary on current Z-Wave network
        /// </summary>
        IS_SECONDARY = 0x01,
        /// <summary>
        /// Return value mask for ZWaveGetControllerCapabilities
        /// </summary>
        ON_OTHER_NETWORK = 0x02,
        /// <summary>
        /// Return value mask for ZWaveGetControllerCapabilities
        /// Controller is a member of a Z-Wave network with a NodeID Server present
        /// </summary>
        NODEID_SERVER_PRESENT = 0x04,
        /// <summary>
        /// Return value mask for ZWaveGetControllerCapabilities
        /// Controller is the original owner of the current Z-Wave network HomeID
        /// </summary>
        IS_REAL_PRIMARY = 0x08,
        /// <summary>
        /// Return value mask for ZWaveGetControllerCapabilities
        /// Controller is the SUC in current Z-WAve network
        /// </summary>
        IS_SUC = 0x10
    }
}
