/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Linq;

namespace Utils
{
    public class BigInteger
    {
        public const uint NumSize = 16;

        private readonly uint[] _number;

        public static BigInteger One
        {
            get
            {
                return new BigInteger(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 });
            }
        }

        public static BigInteger Zero
        {
            get
            {
                return new BigInteger(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            }
        }

        public static BigInteger MaxValue
        {
            get
            {
                return new BigInteger(new byte[] { 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 });
            }
        }

        public BigInteger(byte[] initBytes)
        {
            _number = new uint[NumSize / sizeof(uint)];
            int intNo = _number.Length - 1;
            int startIndex = initBytes.Length > (int)NumSize ? initBytes.Length - (int)NumSize : 0;
            int initIndex = initBytes.Length - 1;
            while (initIndex >= startIndex)
            {
                byte[] bytesForInt = new byte[sizeof(uint)];
                int bytesForIntIndex = 0;
                while (initIndex >= startIndex && bytesForIntIndex < sizeof(uint))
                {
                    bytesForInt[bytesForIntIndex] = initBytes[initIndex];
                    initIndex--;
                    bytesForIntIndex++;
                }
                _number[intNo] = BitConverter.ToUInt32(bytesForInt, 0);
                intNo--;
            }
        }

        public BigInteger Increment()
        {
            int intIndex = _number.Length - 1;
            do
            {
                _number[intIndex]++;
                if (_number[intIndex] == 0)
                    intIndex--;
                else
                    break;
            }
            while (intIndex >= 0);
            return this;
        }

        public BigInteger Decrement()
        {
            int intIndex = _number.Length - 1;
            do
            {
                _number[intIndex]--;
                if (_number[intIndex] == uint.MaxValue)
                    intIndex--;
                else
                    break;
            }
            while (intIndex >= 0);
            return this;
        }

        public byte[] GetBytes()
        {
            var bytes = new byte[NumSize];
            for (int i = 0; i < _number.Length; i++)
            {
                for (int j = 0; j < sizeof(int); j++)
                {
                    bytes[i * sizeof(int) + j] = (byte)((_number[i] >> 8 * (sizeof(int) - 1 - j)) & 0xff);
                }
            }
            return bytes;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            var bigInt = obj as BigInteger;
            if (bigInt is null)
                return false;

            return _number.SequenceEqual(bigInt._number);
        }

        public override int GetHashCode()
        {
            return (int)(_number[0] ^ _number[1] ^ _number[2] ^ _number[3]);
        }

        public static bool operator ==(BigInteger num1, BigInteger num2)
        {
            return num1.Equals(num2);
        }

        public static bool operator !=(BigInteger num1, BigInteger num2)
        {
            return !num1.Equals(num2);
        }
    }
}
