/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;

namespace Utils
{
    public class BitReader
    {
        private byte[] _sourceArray;
        private byte[] _array;

        public BitReader()
        {
            Position = 0;
        }
        
        public int Position { get; private set; }
        public int Length { get; private set; }

        public BitReader(byte[] data)
        {
            if (_sourceArray == null || !_sourceArray.Equals(data))
            {
                _sourceArray = data;
                _array = new byte[data.Length * 8];
                for (int i = 0; i < data.Length; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        _array[i * 8 + j] = (byte)((data[i] >> j) & ((1 << 1) - 1));
                    }
                }
                Length = data.Length * 8;
            }
            Position = 0;
        }
        public void Data(byte[] data)
        {
            if (_sourceArray == null || !_sourceArray.Equals(data))
            {
                _sourceArray = data;
                _array = new byte[data.Length * 8];
                for (int i = 0; i < data.Length; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        _array[i * 8 + j] = (byte)((data[i] >> j) & ((1 << 1) - 1));
                    }
                }
                Length = data.Length * 8;
            }
            Position = 0;
        }

        public byte[] ReadBits(int count)
        {
            var result = new byte[count / 8 + (count % 8 > 0 ? 1 : 0)];
            int byteLength = 0;
            int byteResult = 0;
            int index = 0;
            int length = Position + count <= _array.Length ? Position + count : 0;
            for (int i = Position; i < length; i++)
            {
                if (byteLength > 7)
                {
                    result[index] = (byte)byteResult;
                    index++;
                    byteLength = 0;
                    byteResult = 0;
                }
                if (_array[i] == 1)
                {
                    byteResult += (int)Math.Pow(2, byteLength);
                }
                byteLength++;

                if (index == result.Length - 1)
                {
                    result[index] = (byte)byteResult;
                }
            }
            Position = Position + count;
            return result;
        }

        public byte[] ReadBits(int count, int offset)
        {
            var result = new byte[count / 8 + (count % 8 > 0 ? 1 : 0)];
            int byteLength = 0;
            int byteResult = 0;
            int index = 0;
            int length = offset + count <= _array.Length ? offset + count : 0;
            for (int i = offset; i < length; i++)
            {
                if (byteLength > 7)
                {
                    result[index] = (byte)byteResult;
                    index++;
                    byteLength = 0;
                    byteResult = 0;
                }
                if (_array[i] == 1)
                {
                    byteResult += (int)Math.Pow(2, byteLength);
                }
                byteLength++;

                if (index == result.Length - 1)
                {
                    result[index] = (byte)byteResult;
                }
            }
            return result;
        }
    }
}
