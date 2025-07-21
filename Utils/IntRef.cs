/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections;

namespace Utils
{
    public class IntRef
    {
        public int Value { get; set; }
        public IntRef(int value)
        {
            Value = value;
        }
    }

    public class NetRef
    {
        public int Value { get; set; }
        public ushort Delta { get; set; }
        public ushort Reserved { get; set; }
        public NetRef(int value)
        {
            Value = value;
            Delta = ushort.MaxValue;
            Reserved = ushort.MaxValue;
        }
    }

    public class VariantBitArray
    {
        private readonly int _initialCapacity;
        private BitArray _mask;
        public bool IsActive { get; set; }

        public VariantBitArray(int initialCapacity)
        {
            _initialCapacity = initialCapacity;
            _mask = new BitArray(_initialCapacity);
        }

        public void SetPosition(int position)
        {
            lock (this)
            {
                while (_mask.Count <= position)
                {
                    var tmp = new BitArray(_mask.Count * 2);
                    for (int i = 0; i < _mask.Count; i++)
                    {
                        tmp[i] = _mask[i];
                    }
                    _mask = tmp;
                }
                _mask[position] = true;
            }
        }

        public void Clear()
        {
            _mask = new BitArray(_initialCapacity);
        }

        public bool GetPosition(int position)
        {
            bool ret = position >= 0 && position < _mask.Count && _mask[position];
            return ret;
        }

        public int GetPrevious(int position)
        {
            int ret = -1;
            int index = position - 1;
            while (index >= 0)
            {
                if (index < _mask.Count && _mask[index])
                {
                    ret = index;
                    break;
                }
                index--;
            }
            return ret;
        }

        public int GetNext(int position)
        {
            int ret = -1;
            int index = position + 1;
            index = index < 0 ? 0 : index;
            while (index < _mask.Count)
            {
                if (_mask[index])
                {
                    ret = index;
                    break;
                }
                index++;
            }
            return ret;
        }
    }
}
