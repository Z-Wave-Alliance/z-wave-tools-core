/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZWave.ZnifferApplication
{
    public interface IDataItemBox
    {
        ushort SeqNo { get; set; }
        byte BoxStoreLength { get; }
        void FillStore(byte[] store, int startIndex);
    }

    public struct ByteArray40
    {
        public const int SIZE = 40;
        ulong value0;
        ulong value1;
        ulong value2;
        ulong value3;
        ulong value4;

        public byte this[int index]
        {
            get
            {
                if (index >= 0 && index < SIZE)
                {
                    int idx = index / 8;
                    int off = 8 * (7 - (index % 8));
                    byte ret = 0;
                    switch (idx)
                    {
                        case 0:
                            ret = (byte)(value0 >> off);
                            break;
                        case 1:
                            ret = (byte)(value1 >> off);
                            break;
                        case 2:
                            ret = (byte)(value2 >> off);
                            break;
                        case 3:
                            ret = (byte)(value3 >> off);
                            break;
                        case 4:
                            ret = (byte)(value4 >> off);
                            break;
                    }

                    return ret;
                }
                else
                    return 0;
            }
            set
            {
                if (index >= 0 && index < SIZE)
                {
                    int idx = index / 8;
                    int off = 8 * (7 - (index % 8));
                    switch (idx)
                    {
                        case 0:
                            value0 = (value0 & (ulong)~((ulong)0xFF << off)) + (ulong)((ulong)value << off);
                            break;
                        case 1:
                            value1 = (value1 & (ulong)~((ulong)0xFF << off)) + (ulong)((ulong)value << off);
                            break;
                        case 2:
                            value2 = (value2 & (ulong)~((ulong)0xFF << off)) + (ulong)((ulong)value << off);
                            break;
                        case 3:
                            value3 = (value3 & (ulong)~((ulong)0xFF << off)) + (ulong)((ulong)value << off);
                            break;
                        case 4:
                            value4 = (value4 & (ulong)~((ulong)0xFF << off)) + (ulong)((ulong)value << off);
                            break;
                    }
                }
            }
        }

        public void CopyFrom(byte[] source)
        {
            if (source.Length != SIZE)
                throw new ArgumentOutOfRangeException();
            for (int i = 0; i < SIZE; i++)
            {
                this[i] = source[i];
            }
        }

        public byte[] ToArray()
        {
            byte[] ret = new byte[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                ret[i] = this[i];
            }
            return ret;
        }

        public void CopyTo(int sourceIndex, byte[] destination, int destinationIndex, byte length)
        {
            for (int i = 0; i < length; i++)
            {
                destination[destinationIndex + i] = this[sourceIndex + i];
            }
        }
    }

    public struct DataItemBox : IDataItemBox
    {
        public const int BOX_SIZE = 40;
        public const int DATA_INDEX = 3;
        public ByteArray40 buffer;

        private const int _seqNoIndex = 0;           // 2 bytes
        private const int _boxStoreLengthIndex = 2;      // 1 byte

        public ushort SeqNo
        {
            get
            {
                return (ushort)((buffer[_seqNoIndex] << 8) + (buffer[_seqNoIndex + 1]));
            }
            set
            {
                buffer[_seqNoIndex] = (byte)(value >> 8);
                buffer[_seqNoIndex + 1] = (byte)(value);
            }
        }

        public byte BoxStoreLength
        {
            get { return buffer[_boxStoreLengthIndex]; }
            private set { buffer[_boxStoreLengthIndex] = value; }
        }

        public void FillStore(byte[] store, int startIndex)
        {
            if (store != null)
            {
                int headerLength = store.Length - startIndex;
                if (headerLength > BOX_SIZE - DATA_INDEX)
                    headerLength = BOX_SIZE - DATA_INDEX;

                BoxStoreLength = (byte)headerLength;
                for (int i = 0; i < headerLength; i++)
                {
                    buffer[DATA_INDEX + i] = store[startIndex + i];
                }
            }
        }

        public static implicit operator byte[](DataItemBox dataItemBox)
        {
            return dataItemBox.buffer.ToArray();
        }

        public static implicit operator DataItemBox(byte[] data)
        {
            DataItemBox ret = new DataItemBox();
            ret.buffer.CopyFrom(data);
            return ret;
        }
    }

    public struct FirstDataItemBox : IDataItemBox
    {
        public const int BOX_SIZE = 40;
        public const int DATA_INDEX = 27;
        public ByteArray40 buffer;

        private const int _seqNoIndex = 0;            // 2 bytes
        private const int _boxStoreLengthIndex = 2;   // 1 byte
        private const int _boxCountIndex = 3;         // 2 bytes
        private const int _lineNoIndex = 5;           // 4 bytes
        private const int _createdAtIndex = 9;        // 7 bytes
        private const int _frequencyIndex = 16;       // 1 byte
        private const int _speedChannelIndex = 17;    // 1 byte
        private const int _systimeIndex = 18;         // 2 bytes
        private const int _rssiIndex = 20;            // 1 byte
        private const int _apiTypeIndex = 21;         // 1 byte
        private const int _headerTypeIndex = 22;      // 1 byte
        private const int _wakeupCounterIndex = 23;   // 2 bytes
        private const int _dataLengthIndex = 25;     // 2 bytes

        public ushort SeqNo
        {
            get
            {
                return (ushort)((buffer[_seqNoIndex] << 8) + (buffer[_seqNoIndex + 1]));
            }
            set
            {
                buffer[_seqNoIndex] = (byte)(value >> 8);
                buffer[_seqNoIndex + 1] = (byte)(value);
            }
        }

        public byte BoxStoreLength
        {
            get { return buffer[_boxStoreLengthIndex]; }
            private set { buffer[_boxStoreLengthIndex] = value; }
        }

        public ushort BoxCount
        {
            get
            {
                return (ushort)((buffer[_boxCountIndex] << 8) + (buffer[_boxCountIndex + 1]));
            }
            set
            {
                buffer[_boxCountIndex] = (byte)(value >> 8);
                buffer[_boxCountIndex + 1] = (byte)(value);
            }
        }

        public int LineNo
        {
            get
            {
                return (int)((buffer[_lineNoIndex] << 24) + (buffer[_lineNoIndex + 1] << 16) + (buffer[_lineNoIndex + 2] << 8) + (buffer[_lineNoIndex + 3]));
            }
            set
            {
                buffer[_lineNoIndex] = (byte)(value >> 24);
                buffer[_lineNoIndex + 1] = (byte)(value >> 16);
                buffer[_lineNoIndex + 2] = (byte)(value >> 8);
                buffer[_lineNoIndex + 3] = (byte)(value);
            }
        }

        public long CreatedAt
        {
            get
            {
                long ret = (long)0x88 << 56;
                ret += (long)(buffer[_createdAtIndex]) << 48;
                ret += (long)(buffer[_createdAtIndex + 1]) << 40;
                ret += (long)(buffer[_createdAtIndex + 2]) << 32;
                ret += (long)(buffer[_createdAtIndex + 3]) << 24;
                ret += buffer[_createdAtIndex + 4] << 16;
                ret += buffer[_createdAtIndex + 5] << 8;
                ret += buffer[_createdAtIndex + 6];
                return ret;
            }
            set
            {
                buffer[_createdAtIndex] = (byte)(value >> 48);
                buffer[_createdAtIndex + 1] = (byte)(value >> 40);
                buffer[_createdAtIndex + 2] = (byte)(value >> 32);
                buffer[_createdAtIndex + 3] = (byte)(value >> 24);
                buffer[_createdAtIndex + 4] = (byte)(value >> 16);
                buffer[_createdAtIndex + 5] = (byte)(value >> 8);
                buffer[_createdAtIndex + 6] = (byte)value;
            }
        }

        public byte Frequency
        {
            get { return buffer[_frequencyIndex]; }
            set { buffer[_frequencyIndex] = value; }
        }

        public byte Speed
        {
            get { return (byte)(buffer[_speedChannelIndex] >> 4); }
            set { buffer[_speedChannelIndex] = (byte)((value << 4) + (buffer[_speedChannelIndex] & 0x0F)); }
        }

        public byte Channel
        {
            get { return (byte)(buffer[_speedChannelIndex] & 0x0F); }
            set { buffer[_speedChannelIndex] = (byte)((value & 0x0F) + (buffer[_speedChannelIndex] & 0xF0)); }
        }

        public ushort Systime
        {
            get
            {
                return (ushort)((buffer[_systimeIndex] << 8) + (buffer[_systimeIndex + 1]));
            }
            set
            {
                buffer[_systimeIndex] = (byte)(value >> 8);
                buffer[_systimeIndex + 1] = (byte)(value);
            }
        }

        public byte Rssi
        {
            get { return buffer[_rssiIndex]; }
            set { buffer[_rssiIndex] = value; }
        }

        public byte ApiType
        {
            get { return buffer[_apiTypeIndex]; }
            set { buffer[_apiTypeIndex] = value; }
        }

        public byte HeaderType
        {
            get { return buffer[_headerTypeIndex]; }
            set { buffer[_headerTypeIndex] = value; }
        }

        public ushort WakeupCounter
        {
            get
            {
                return (ushort)((buffer[_wakeupCounterIndex] << 8) + (buffer[_wakeupCounterIndex + 1]));
            }
            set
            {
                buffer[_wakeupCounterIndex] = (byte)(value >> 8);
                buffer[_wakeupCounterIndex + 1] = (byte)(value);
            }
        }

        public ushort DataLength
        {
            get
            {
                return (ushort)((buffer[_dataLengthIndex] << 8) + (buffer[_dataLengthIndex + 1]));
            }
            set
            {
                buffer[_dataLengthIndex] = (byte)(value >> 8);
                buffer[_dataLengthIndex + 1] = (byte)(value);
            }
        }

        public void FillStore(byte[] store, int startIndex)
        {
            if (store != null)
            {
                int headerLength = store.Length - startIndex;
                if (headerLength > BOX_SIZE - DATA_INDEX)
                    headerLength = BOX_SIZE - DATA_INDEX;

                BoxStoreLength = (byte)headerLength;
                for (int i = 0; i < headerLength; i++)
                {
                    buffer[DATA_INDEX + i] = store[startIndex + i];
                }
            }
        }

        public static implicit operator byte[](FirstDataItemBox dataItemBox)
        {
            return dataItemBox.buffer.ToArray();
        }

        public static implicit operator FirstDataItemBox(byte[] data)
        {
            FirstDataItemBox ret = new FirstDataItemBox();
            ret.buffer.CopyFrom(data);
            return ret;
        }
    }

}
