/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
namespace ZWave.Enums
{
    /// <summary>
    /// Configuration Channel Types.
    /// This value describes the RF profile mappings configuration (see A.4.2 MPDU formats, in G.9959 and PHY and MAC Layer Specification)
    /// </summary>
    public enum ChannelTypes
    {
        /// <summary>
        /// Classic 1, 2 Channels.
        /// </summary>
        Configuration12,
        /// <summary>
        /// Classic 3 Channel.
        /// </summary>
        Configuration3,
        /// <summary>
        /// Long Range.
        /// </summary>
        LongRange,
        /// <summary>
        /// Configuration 12 with extended CRC.
        /// </summary>
        Configuration0,
    }
}