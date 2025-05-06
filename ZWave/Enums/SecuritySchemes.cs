/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿namespace ZWave.Enums
{
    public enum SecuritySchemes
    {
        NONE = 0x00,
        S2_UNAUTHENTICATED = 0x01,
        S2_AUTHENTICATED = 0x02,
        S2_ACCESS = 0x03,
        S0 = 0x04,
        S2_TEMP = 0x9F
    }

    public static class SecuritySchemeSet
    {
        public static SecuritySchemes[] ALL = {
            SecuritySchemes.S2_ACCESS,
            SecuritySchemes.S2_AUTHENTICATED,
            SecuritySchemes.S2_UNAUTHENTICATED,
            SecuritySchemes.S0
        };

        public static SecuritySchemes[] S0 = {
            SecuritySchemes.S0
        };

        public static SecuritySchemes[] ALLS2 = {
            SecuritySchemes.S2_ACCESS,
            SecuritySchemes.S2_AUTHENTICATED,
            SecuritySchemes.S2_UNAUTHENTICATED
        };

        public static SecuritySchemes[] S0_AND_S2_UNAUTHENTICATED = {
            SecuritySchemes.S2_UNAUTHENTICATED,
            SecuritySchemes.S0
        };

        public static SecuritySchemes[] ALLS2_LR = {
            SecuritySchemes.S2_ACCESS,
            SecuritySchemes.S2_AUTHENTICATED
        };

    }
}
