/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;

namespace Utils
{
    public static class RefTypeExt
    {
        public static void ThrowIfNull<T>(this T target, string message) where T : class
        {
            if (target == null)
            {
                throw new NullReferenceException(message);
            }
        }
    }
}
