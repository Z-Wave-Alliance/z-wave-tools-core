/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿namespace Utils
{
    public struct BitArray256
    {
        private const int Len = 32;
        public static int MaxCount = Len * 8;
        private int _part0;
        private int _part1;
        private int _part2;
        private int _part3;
        private int _part4;
        private int _part5;
        private int _part6;
        private int _part7;

        public bool this[int index]
        {
            get
            {
                bool ret = false;
                int col = index % Len;
                int mask = 1 << col;
                switch (index / Len)
                {
                    case 0:
                        ret = (_part0 & mask) != 0;
                        break;
                    case 1:
                        ret = (_part1 & mask) != 0;
                        break;
                    case 2:
                        ret = (_part2 & mask) != 0;
                        break;
                    case 3:
                        ret = (_part3 & mask) != 0;
                        break;
                    case 4:
                        ret = (_part4 & mask) != 0;
                        break;
                    case 5:
                        ret = (_part5 & mask) != 0;
                        break;
                    case 6:
                        ret = (_part6 & mask) != 0;
                        break;
                    case 7:
                        ret = (_part7 & mask) != 0;
                        break;
                }
                return ret;
            }
            set
            {
                int col = index % Len;
                int mask = 1 << col;
                int setMask = 0;
                int clearMask = -1;
                if (value)
                {
                    setMask = mask;
                }
                else
                {
                    clearMask = ~mask;
                }

                switch (index / Len)
                {
                    case 0:
                        _part0 |= setMask;
                        _part0 &= clearMask;
                        break;
                    case 1:
                        _part1 |= setMask;
                        _part1 &= clearMask;
                        break;
                    case 2:
                        _part2 |= setMask;
                        _part2 &= clearMask;
                        break;
                    case 3:
                        _part3 |= setMask;
                        _part3 &= clearMask;
                        break;
                    case 4:
                        _part4 |= setMask;
                        _part4 &= clearMask;
                        break;
                    case 5:
                        _part5 |= setMask;
                        _part5 &= clearMask;
                        break;
                    case 6:
                        _part6 |= setMask;
                        _part6 &= clearMask;
                        break;
                    case 7:
                        _part7 |= setMask;
                        _part7 &= clearMask;
                        break;
                }
            }
        }

        public int GetValuesCount()
        {
            int ret = 0;
            for (int i = 0; i < Len; i++)
            {
                int mask = 1 << i;
                ret += (_part0 & mask) != 0 ? 1 : 0;
                ret += (_part1 & mask) != 0 ? 1 : 0;
                ret += (_part2 & mask) != 0 ? 1 : 0;
                ret += (_part3 & mask) != 0 ? 1 : 0;
                ret += (_part4 & mask) != 0 ? 1 : 0;
                ret += (_part5 & mask) != 0 ? 1 : 0;
                ret += (_part6 & mask) != 0 ? 1 : 0;
                ret += (_part7 & mask) != 0 ? 1 : 0;
            }
            return ret;
        }

        public void SetAll(bool value)
        {
            if (value)
            {
                _part0 = -1;
                _part1 = -1;
                _part2 = -1;
                _part3 = -1;
                _part4 = -1;
                _part5 = -1;
                _part6 = -1;
                _part7 = -1;
            }
            else
            {
                _part0 = 0;
                _part1 = 0;
                _part2 = 0;
                _part3 = 0;
                _part4 = 0;
                _part5 = 0;
                _part6 = 0;
                _part7 = 0;
            }
        }
    }
}
