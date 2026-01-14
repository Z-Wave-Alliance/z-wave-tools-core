/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
/// SPDX-FileCopyrightText: Z-Wave Alliance https://z-wavealliance.org
using System.Security.Cryptography;

namespace Utils.Extensions
{
    public static class RandomNumberGeneratorExt
    {
        public static byte NextByte(this RandomNumberGenerator rng)
        {
            var arr = new byte[1];
            rng.GetBytes(arr);
            return arr[0];
        }
    }
}
