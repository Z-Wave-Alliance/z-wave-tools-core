// SPDX-License-Identifier: BSD-3-Clause
// SPDX-FileCopyrightText: Z-Wave Alliance <https://z-wavealliance.org>
namespace ZWave.Enums
{
    /// <summary>
    /// The inclusion state of the own node. The states are the ones the Z-Wave protocol distinguishes for a Serial API end
    /// device, plus the Learn Mode, which the protocol does not treat as a state of its own.
    /// </summary>
    public enum InclusionStates
    {
        /// <summary>
        /// Not included in any network, excluded, or in Learn Mode waiting to be included.
        /// </summary>
        NotIncluded,

        /// <summary>
        /// Included without an S0/S2 key: The security bootstrapping failed, timed out or was not attempted.
        /// </summary>
        IncludedNonSecurely,

        /// <summary>
        /// Included and holding at least one S0/S2 key.
        /// </summary>
        IncludedSecurely
    }
}
