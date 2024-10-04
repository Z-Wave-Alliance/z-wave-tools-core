/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_MANUFACTURER_SPECIFIC_V2
    {
        public const byte ID = 0x72;
        public const byte VERSION = 2;
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
                ret.Add(COMMAND_CLASS_MANUFACTURER_SPECIFIC_V2.ID);
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
                ret.Add(COMMAND_CLASS_MANUFACTURER_SPECIFIC_V2.ID);
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
        public partial class DEVICE_SPECIFIC_GET
        {
            public const byte ID = 0x06;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte deviceIdType
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 3 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0xF8; _value += (byte)(value << 3 & 0xF8); }
                }
                public static implicit operator Tproperties1(byte data)
                {
                    Tproperties1 ret = new Tproperties1();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties1 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties1 properties1 = 0;
            public static implicit operator DEVICE_SPECIFIC_GET(byte[] data)
            {
                DEVICE_SPECIFIC_GET ret = new DEVICE_SPECIFIC_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](DEVICE_SPECIFIC_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_MANUFACTURER_SPECIFIC_V2.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                return ret.ToArray();
            }
        }
        public partial class DEVICE_SPECIFIC_REPORT
        {
            public const byte ID = 0x07;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte deviceIdType
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 3 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0xF8; _value += (byte)(value << 3 & 0xF8); }
                }
                public static implicit operator Tproperties1(byte data)
                {
                    Tproperties1 ret = new Tproperties1();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties1 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties1 properties1 = 0;
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte deviceIdDataLengthIndicator
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte deviceIdDataFormat
                {
                    get { return (byte)(_value >> 5 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0xE0; _value += (byte)(value << 5 & 0xE0); }
                }
                public static implicit operator Tproperties2(byte data)
                {
                    Tproperties2 ret = new Tproperties2();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties2 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties2 properties2 = 0;
            public IList<byte> deviceIdData = new List<byte>();
            public static implicit operator DEVICE_SPECIFIC_REPORT(byte[] data)
            {
                DEVICE_SPECIFIC_REPORT ret = new DEVICE_SPECIFIC_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.deviceIdData = new List<byte>();
                    for (int i = 0; i < ret.properties2.deviceIdDataLengthIndicator; i++)
                    {
                        if (data.Length > index) ret.deviceIdData.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](DEVICE_SPECIFIC_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_MANUFACTURER_SPECIFIC_V2.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.deviceIdData != null)
                {
                    foreach (var tmp in command.deviceIdData)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

