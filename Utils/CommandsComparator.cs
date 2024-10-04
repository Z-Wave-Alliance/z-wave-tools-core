/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Linq;

namespace Utils
{
    public static class CommandsComparator
    {
        public static bool IsEquals(byte[] first, byte[] second)
        {
            return IsEquals(first, second, 2);
        }
        public static bool IsEquals(byte[] first, byte[] second, int bytesCountToCompare)
        {
            if (first.Length < bytesCountToCompare || second.Length < bytesCountToCompare)
            {
                throw new ArgumentException();
            }
            return first.Take(bytesCountToCompare).SequenceEqual(second.Take(bytesCountToCompare));
        }
    }
}
