/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿namespace Utils
{
    public struct Int128
    {
        private uint _x0;
        private uint _x1;
        private uint _x2;
        private uint _x3;

        public Int128(int value)
        {
            _x0 = (uint)value;
            _x1 = 0;
            _x2 = 0;
            _x3 = 0;
        }

        public Int128(uint value)
        {
            _x0 = value;
            _x1 = 0;
            _x2 = 0;
            _x3 = 0;
        }

        public Int128(byte[] value)
        {
            _x0 = 0;
            _x1 = 0;
            _x2 = 0;
            _x3 = 0;
            if (value != null)
            {
                int index = 0;
                while (index < value.Length && index < 4)
                {
                    _x0 += (uint)(value[value.Length - index - 1] << (8 * (index % 4)));
                    index++;
                }
                while (index < value.Length && index < 8)
                {
                    _x1 += (uint)(value[value.Length - index - 1] << (8 * (index % 4)));
                    index++;
                }
                while (index < value.Length && index < 12)
                {
                    _x2 += (uint)(value[value.Length - index - 1] << (8 * (index % 4)));
                    index++;
                }
                while (index < value.Length && index < 16)
                {
                    _x3 += (uint)(value[value.Length - index - 1] << (8 * (index % 4)));
                    index++;
                }
            }
        }

        public override string ToString()
        {
            return string.Format("{0:X8}{1:X8}{2:X8}{3:X8}", _x3, _x2, _x1, _x0);
        }

        public static Int128 MaxValue
        {
            get
            {
                return new Int128 { _x0 = uint.MaxValue, _x1 = uint.MaxValue, _x2 = uint.MaxValue, _x3 = uint.MaxValue };
            }
        }

        public static Int128 MinValue
        {
            get
            {
                return new Int128 { _x0 = uint.MinValue, _x1 = uint.MinValue, _x2 = uint.MinValue, _x3 = uint.MinValue };
            }
        }

        public static bool operator ==(Int128 val1, Int128 val2)
        {
            return val1._x0 == val2._x0 && val1._x1 == val2._x1 && val1._x2 == val2._x2 && val1._x3 == val2._x3;
        }

        public static bool operator !=(Int128 val1, Int128 val2)
        {
            return val1._x0 != val2._x0 || val1._x1 != val2._x1 || val1._x2 != val2._x2 || val1._x3 != val2._x3;
        }

        public static bool operator ==(Int128 val, uint ui)
        {
            return val._x0 == ui && val._x1 == 0 && val._x2 == 0 && val._x3 == 0;
        }

        public static bool operator !=(Int128 val, uint ui)
        {
            return val._x0 != ui || val._x1 != 0 || val._x2 != 0 || val._x3 != 0;
        }

        public static Int128 operator ++(Int128 val)
        {
            return val + 1;
        }

        public static Int128 operator +(Int128 val1, Int128 val2)
        {
            if (val1 == 0)
                return val2;
            if (val2 == 0)
                return val1;
            Int128 ret = new Int128(0);
            ulong sum = 0;
            sum = val1._x0 + (ulong)val2._x0 + sum;
            ret._x0 = (uint)sum;
            sum >>= 32;
            sum = val1._x1 + (ulong)val2._x1 + sum;
            ret._x1 = (uint)sum;
            sum >>= 32;
            sum = val1._x2 + (ulong)val2._x2 + sum;
            ret._x2 = (uint)sum;
            sum >>= 32;
            sum = val1._x3 + (ulong)val2._x3 + sum;
            ret._x3 = (uint)sum;
            return ret;
        }

        public static Int128 operator +(Int128 val1, int val2)
        {
            if (val1 == 0)
                return new Int128((uint)val2);
            if (val2 == 0)
                return val1;
            Int128 ret = new Int128(0);
            ulong sum = 0;
            sum = val1._x0 + (ulong)val2 + sum;
            ret._x0 = (uint)sum;
            sum >>= 32;
            sum = val1._x1 + sum;
            ret._x1 = (uint)sum;
            sum >>= 32;
            sum = val1._x2 + sum;
            ret._x2 = (uint)sum;
            sum >>= 32;
            sum = val1._x3 + sum;
            ret._x3 = (uint)sum;
            return ret;
        }

        public static Int128 operator --(Int128 val)
        {
            return val - 1;
        }

        public static Int128 operator -(Int128 val1, Int128 val2)
        {
            if (val2 == 0)
                return val1;

            if (val1 == 0)
            {
                Int128 ret = new Int128(0);
                ulong delta = 0;
                delta = delta - val2._x0;
                ret._x0 = (uint)delta;
                delta = (ulong)(int)(delta >> 32);
                delta = delta - val2._x1;
                ret._x1 = (uint)delta;
                delta = (ulong)(int)(delta >> 32);
                delta = delta - val2._x2;
                ret._x2 = (uint)delta;
                delta = (ulong)(int)(delta >> 32);
                delta = delta - val2._x3;
                ret._x3 = (uint)delta;
                return ret;
            }
            else
            {
                Int128 ret = new Int128(0);
                ulong delta = 0;
                delta = val1._x0 - (ulong)val2._x0 + delta;
                ret._x0 = (uint)delta;
                delta = (ulong)(int)(delta >> 32);
                delta = val1._x1 - (ulong)val2._x1 + delta;
                ret._x1 = (uint)delta;
                delta = (ulong)(int)(delta >> 32);
                delta = val1._x2 - (ulong)val2._x2 + delta;
                ret._x2 = (uint)delta;
                delta = (ulong)(int)(delta >> 32);
                delta = val1._x3 - (ulong)val2._x3 + delta;
                ret._x3 = (uint)delta;
                return ret;
            }
        }

        public byte[] GetBytes()
        {
            byte[] ret = new byte[16];
            int index = 0;
            while (index < 4)
            {
                ret[index] = (byte)(_x3 >> (8 * (3 - index % 4)));
                index++;
            }
            while (index < 8)
            {
                ret[index] = (byte)(_x2 >> (8 * (3 - index % 4)));
                index++;
            }
            while (index < 12)
            {
                ret[index] = (byte)(_x1 >> (8 * (3 - index % 4)));
                index++;
            }
            while (index < 16)
            {
                ret[index] = (byte)(_x0 >> (8 * (3 - index % 4)));
                index++;
            }
            return ret;
        }

        public static implicit operator Int128(uint value)
        {
            return new Int128(value);
        }

        public static implicit operator Int128(int value)
        {
            return new Int128((uint)value);
        }

        public static explicit operator uint(Int128 value)
        {
            return value._x0;
        }

        public static explicit operator ulong(Int128 value)
        {
            return ((ulong)value._x1 << 32) + value._x0;
        }


        public static explicit operator int(Int128 value)
        {
            return (int)value._x0;
        }

        public static explicit operator long(Int128 value)
        {
            return ((long)value._x1 << 32) + value._x0;
        }

        public override int GetHashCode()
        {
            uint val = 0;
            val ^= _x0;
            val ^= _x1;
            val ^= _x2;
            val ^= _x3;
            return (int)val;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj is int)
            {
                return (int)obj >= 0 && this == (int)obj;
            }

            return this == (Int128)obj;
        }
    }
}
