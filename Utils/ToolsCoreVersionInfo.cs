// SPDX-License-Identifier: BSD-3-Clause
// SPDX-FileCopyrightText: Z-Wave Alliance <https://z-wavealliance.org>

namespace Utils
{
    /// <summary>
    /// Product, company, copyright and version of all Z-Wave Tools Core assemblies. Every other project of this repository
    /// references Utils; the AssemblyInfo.cs files reference these constants.
    /// </summary>
    public static class ToolsCoreVersionInfo
    {
        // The following constants should be updated for each release.
        // The copyright starts in 2024, the year Silicon Labs donated Z-Wave Tools Core to the Z-Wave Alliance; keep that
        // year and set the end year to the year of the last change.
        public const string COPYRIGHT = "© 2024–2026 Z-Wave Alliance, Inc. All rights reserved.";
        public const string VERSION = "25.1.1.0"; // keep all four digits

        // The following constants should not be changed.
        public const string COMPANY_NAME = "Z-Wave Alliance";
        public const string PRODUCT = "Z-Wave Tools Core";
    }
}
