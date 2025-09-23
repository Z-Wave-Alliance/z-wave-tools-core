/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_MANUFACTURER_SPECIFIC
    {
        public const byte ID = 0x72;
        public const byte VERSION = 1;
        public partial class MANUFACTURER_SPECIFIC_GET
        {
            public const byte ID = 0x04;
            public static implicit operator MANUFACTURER_SPECIFIC_GET(byte[] data)
            {
                MANUFACTURER_SPECIFIC_GET ret = new MANUFACTURER_SPECIFIC_GET();
                return ret;
            }
            public static implicit operator byte[](MANUFACTURER_SPECIFIC_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_MANUFACTURER_SPECIFIC.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class MANUFACTURER_SPECIFIC_REPORT
        {
            public const byte ID = 0x05;
            public const byte manufacturerIdBytesCount = 2;
            public byte[] manufacturerId = new byte[manufacturerIdBytesCount];
            public const byte productTypeIdBytesCount = 2;
            public byte[] productTypeId = new byte[productTypeIdBytesCount];
            public const byte productIdBytesCount = 2;
            public byte[] productId = new byte[productIdBytesCount];
            public static implicit operator MANUFACTURER_SPECIFIC_REPORT(byte[] data)
            {
                MANUFACTURER_SPECIFIC_REPORT ret = new MANUFACTURER_SPECIFIC_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.manufacturerId = (data.Length - index) >= manufacturerIdBytesCount ? new byte[manufacturerIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.manufacturerId[0] = data[index++];
                    if (data.Length > index) ret.manufacturerId[1] = data[index++];
                    ret.productTypeId = (data.Length - index) >= productTypeIdBytesCount ? new byte[productTypeIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.productTypeId[0] = data[index++];
                    if (data.Length > index) ret.productTypeId[1] = data[index++];
                    ret.productId = (data.Length - index) >= productIdBytesCount ? new byte[productIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.productId[0] = data[index++];
                    if (data.Length > index) ret.productId[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](MANUFACTURER_SPECIFIC_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_MANUFACTURER_SPECIFIC.ID);
                ret.Add(ID);
                if (command.manufacturerId != null)
                {
                    foreach (var tmp in command.manufacturerId)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.productTypeId != null)
                {
                    foreach (var tmp in command.productTypeId)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.productId != null)
                {
                    foreach (var tmp in command.productId)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

