/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
namespace Utils
{
    public struct BitArray8
    {
        private const int Len = 8;
        public static int MaxCount = Len * 1;
        private byte _part0;

        public bool this[int index]
        {
            get
            {
                int col = index % Len;
                int mask = 1 << col;
                var ret = (_part0 & mask) != 0;
                return ret;
            }
            set
            {
                int col = index % Len;
                byte mask = (byte)(1 << col);
                byte setMask = 0;
                byte clearMask = 0xFF;
                if (value)
                {
                    setMask = mask;
                }
                else
                {
                    clearMask = (byte)~mask;
                }
                _part0 |= setMask;
                _part0 &= clearMask;
            }
        }

        public int GetValuesCount()
        {
            int ret = 0;
            for (int i = 0; i < Len; i++)
            {
                int mask = 1 << i;
                ret += (_part0 & mask) != 0 ? 1 : 0;
            }
            return ret;
        }

        public void SetAll(bool value)
        {
            if (value)
            {
                _part0 |= 0xFF;
            }
            else
            {
                _part0 &= 0x00;
            }
        }

        public static implicit operator BitArray8(byte value)
        {
            return new BitArray8 { _part0 = value };
        }

        public static explicit operator byte(BitArray8 value)
        {
            return value._part0;
        }
    }
}
