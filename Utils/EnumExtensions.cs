/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;

namespace Utils
{
    public static class EnumExtensions
    {
        public static bool HasFlag(this Enum value, Enum flagValue)
        {
            if (Enum.GetUnderlyingType(value.GetType()) == typeof(int))
                return ((int)(object)value & (int)(object)flagValue) == (int)(object)flagValue;
            if (Enum.GetUnderlyingType(value.GetType()) == typeof(byte))
                return ((byte)(object)value & (byte)(object)flagValue) == (byte)(object)flagValue;
            throw new NotImplementedException(
                string.Format("Enum '{0}' underlying type '{1}' does not supported", value.GetType(), Enum.GetUnderlyingType(value.GetType())));
        }

        public static int GetByteFlagIndex(this Enum value)
        {
            int maxIndex;
            int currValue;
            if (Enum.GetUnderlyingType(value.GetType()) == typeof(byte))
            {
                maxIndex = 8;
                currValue = (byte)(object)value;
            }
            else if (Enum.GetUnderlyingType(value.GetType()) == typeof(int))
            {
                maxIndex = 32;
                currValue = (int)(object)value;
            }
            else
                throw new NotImplementedException(
                    string.Format("Enum '{0}' underlying type '{1}' does not supported", value.GetType(), Enum.GetUnderlyingType(value.GetType())));

            for (int i = 0; i < maxIndex; i++)
            {
                var mask = Tools.GetMaskFromBits((byte)(i + 1), 0);
                if ((currValue & mask) != 0)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
