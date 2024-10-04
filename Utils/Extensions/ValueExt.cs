/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿namespace Utils
{
    public static class ValueExt
    {
        public static int GetValue(this int? value)
        {
            if (value != null)
            {
                return value.Value;
            }
            return 0;
        }

        public static byte GetValue(this byte? value)
        {
            if (value != null)
            {
                return value.Value;
            }
            return 0;
        }

        public static bool GetValue(this bool? value)
        {
            if (value != null)
            {
                return value.Value;
            }
            return false;
        }
    }
}
