/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;

namespace ZWave.Enums
{
    /// <summary>
    /// Controller Roles enumeration.
    /// </summary>
    [Flags]
    public enum ControllerRoles
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,
        /// <summary>
        /// SUC role.
        /// </summary>
        SUC = 1,
        /// <summary>
        /// Real Primary role.
        /// </summary>
        RealPrimary = 2,
        /// <summary>
        /// SIS role.
        /// </summary>
        SIS = 4,
        /// <summary>
        /// Secondary Controller role.
        /// </summary>
        Secondary = 8,
        /// <summary>
        /// Inclusion Controller role.
        /// </summary>
        Inclusion = 16,
        /// <summary>
        /// SIS present flag.
        /// </summary>
        NodeIdServerPresent = 32,
        /// <summary>
        /// From other network flag.
        /// </summary>
        OtherNetwork = 64
    }
}
