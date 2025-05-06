/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿namespace ZWave.BasicApplication.Enums
{
    public enum VirtualDeviceLearnModes
    {
        /// <summary>
        /// Disable EndDeviceLearnMode (disable possibility to add/remove Virtual EndDevice nodes)
        /// Allowed when bridge is a primary controller, an inclusion controller or a
        /// secondary controller
        /// </summary>
        Disable = 0,
        /// <summary>
        /// Enable EndDeviceLearnMode - Enable possibility for including/excluding a Virtual
        /// EndDevice node by an external primary/inclusion controller Allowed when bridge
        /// is an inclusion controller or a secondary controller
        /// </summary>
        Enable = 1,
        /// <summary>
        /// Add new Virtual EndDevice node if possible Allowed when bridge is a primary or
        /// an inclusion controller EndDevice Learn function done when Callback function
        /// returns ASSIGN_NODEID_DONE
        /// </summary>
        Add = 2,
        /// <summary>
        /// Remove existing Virtual EndDevice node Allowed when bridge is a primary or an
        /// inclusion controller EndDevice Learn function done when Callback function returns
        /// ASSIGN_NODEID_DONE
        /// </summary>
        Remove = 3
    }
}
