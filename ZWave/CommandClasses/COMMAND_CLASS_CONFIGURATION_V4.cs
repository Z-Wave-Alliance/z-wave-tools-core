/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_CONFIGURATION_V4
    {
        public const byte ID = 0x70;
        public const byte VERSION = 4;
        public partial class CONFIGURATION_BULK_GET
        {
            public const byte ID = 0x08;
            public const byte parameterOffsetBytesCount = 2;
            public byte[] parameterOffset = new byte[parameterOffsetBytesCount];
            public ByteValue numberOfParameters = 0;
            public static implicit operator CONFIGURATION_BULK_GET(byte[] data)
            {
                CONFIGURATION_BULK_GET ret = new CONFIGURATION_BULK_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.parameterOffset = (data.Length - index) >= parameterOffsetBytesCount ? new byte[parameterOffsetBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.parameterOffset[0] = data[index++];
                    if (data.Length > index) ret.parameterOffset[1] = data[index++];
                    ret.numberOfParameters = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](CONFIGURATION_BULK_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_CONFIGURATION_V4.ID);
                ret.Add(ID);
                if (command.parameterOffset != null)
                {
                    foreach (var tmp in command.parameterOffset)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.numberOfParameters.HasValue) ret.Add(command.numberOfParameters);
                return ret.ToArray();
            }
        }
        public partial class CONFIGURATION_BULK_REPORT
        {
            public const byte ID = 0x09;
            public const byte parameterOffsetBytesCount = 2;
            public byte[] parameterOffset = new byte[parameterOffsetBytesCount];
            public ByteValue numberOfParameters = 0;
            public ByteValue reportsToFollow = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte size
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 3 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x38; _value += (byte)(value << 3 & 0x38); }
                }
                public byte handshake
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte mdefault
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
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
            public class TVG
            {
                public IList<byte> parameter = new List<byte>();
            }
            public List<TVG> vg = new List<TVG>();
            public static implicit operator CONFIGURATION_BULK_REPORT(byte[] data)
            {
                CONFIGURATION_BULK_REPORT ret = new CONFIGURATION_BULK_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.parameterOffset = (data.Length - index) >= parameterOffsetBytesCount ? new byte[parameterOffsetBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.parameterOffset[0] = data[index++];
                    if (data.Length > index) ret.parameterOffset[1] = data[index++];
                    ret.numberOfParameters = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.reportsToFollow = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.vg = new List<TVG>();
                    for (int j = 0; j < ret.numberOfParameters; j++)
                    {
                        TVG tmp = new TVG();
                        tmp.parameter = new List<byte>();
                        for (int i = 0; i < ret.properties1.size; i++)
                        {
                            if (data.Length > index) tmp.parameter.Add(data[index++]);
                        }
                        ret.vg.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](CONFIGURATION_BULK_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_CONFIGURATION_V4.ID);
                ret.Add(ID);
                if (command.parameterOffset != null)
                {
                    foreach (var tmp in command.parameterOffset)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.numberOfParameters.HasValue) ret.Add(command.numberOfParameters);
                if (command.reportsToFollow.HasValue) ret.Add(command.reportsToFollow);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.vg != null)
                {
                    foreach (var item in command.vg)
                    {
                        if (item.parameter != null)
                        {
                            foreach (var tmp in item.parameter)
                            {
                                ret.Add(tmp);
                            }
                        }
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class CONFIGURATION_BULK_SET
        {
            public const byte ID = 0x07;
            public const byte parameterOffsetBytesCount = 2;
            public byte[] parameterOffset = new byte[parameterOffsetBytesCount];
            public ByteValue numberOfParameters = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte size
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 3 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x38; _value += (byte)(value << 3 & 0x38); }
                }
                public byte handshake
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte mdefault
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
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
            public class TVG
            {
                public IList<byte> parameter = new List<byte>();
            }
            public List<TVG> vg = new List<TVG>();
            public static implicit operator CONFIGURATION_BULK_SET(byte[] data)
            {
                CONFIGURATION_BULK_SET ret = new CONFIGURATION_BULK_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.parameterOffset = (data.Length - index) >= parameterOffsetBytesCount ? new byte[parameterOffsetBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.parameterOffset[0] = data[index++];
                    if (data.Length > index) ret.parameterOffset[1] = data[index++];
                    ret.numberOfParameters = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.vg = new List<TVG>();
                    for (int j = 0; j < ret.numberOfParameters; j++)
                    {
                        TVG tmp = new TVG();
                        tmp.parameter = new List<byte>();
                        for (int i = 0; i < ret.properties1.size; i++)
                        {
                            if (data.Length > index) tmp.parameter.Add(data[index++]);
                        }
                        ret.vg.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](CONFIGURATION_BULK_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_CONFIGURATION_V4.ID);
                ret.Add(ID);
                if (command.parameterOffset != null)
                {
                    foreach (var tmp in command.parameterOffset)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.numberOfParameters.HasValue) ret.Add(command.numberOfParameters);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.vg != null)
                {
                    foreach (var item in command.vg)
                    {
                        if (item.parameter != null)
                        {
                            foreach (var tmp in item.parameter)
                            {
                                ret.Add(tmp);
                            }
                        }
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class CONFIGURATION_GET
        {
            public const byte ID = 0x05;
            public ByteValue parameterNumber = 0;
            public static implicit operator CONFIGURATION_GET(byte[] data)
            {
                CONFIGURATION_GET ret = new CONFIGURATION_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.parameterNumber = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](CONFIGURATION_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_CONFIGURATION_V4.ID);
                ret.Add(ID);
                if (command.parameterNumber.HasValue) ret.Add(command.parameterNumber);
                return ret.ToArray();
            }
        }
        public partial class CONFIGURATION_REPORT
        {
            public const byte ID = 0x06;
            public ByteValue parameterNumber = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte size
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
            public IList<byte> configurationValue = new List<byte>();
            public static implicit operator CONFIGURATION_REPORT(byte[] data)
            {
                CONFIGURATION_REPORT ret = new CONFIGURATION_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.parameterNumber = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.configurationValue = new List<byte>();
                    for (int i = 0; i < ret.properties1.size; i++)
                    {
                        if (data.Length > index) ret.configurationValue.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](CONFIGURATION_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_CONFIGURATION_V4.ID);
                ret.Add(ID);
                if (command.parameterNumber.HasValue) ret.Add(command.parameterNumber);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.configurationValue != null)
                {
                    foreach (var tmp in command.configurationValue)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class CONFIGURATION_SET
        {
            public const byte ID = 0x04;
            public ByteValue parameterNumber = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte size
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 3 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x78; _value += (byte)(value << 3 & 0x78); }
                }
                public byte mdefault
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
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
            public IList<byte> configurationValue = new List<byte>();
            public static implicit operator CONFIGURATION_SET(byte[] data)
            {
                CONFIGURATION_SET ret = new CONFIGURATION_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.parameterNumber = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.configurationValue = new List<byte>();
                    for (int i = 0; i < ret.properties1.size; i++)
                    {
                        if (data.Length > index) ret.configurationValue.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](CONFIGURATION_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_CONFIGURATION_V4.ID);
                ret.Add(ID);
                if (command.parameterNumber.HasValue) ret.Add(command.parameterNumber);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.configurationValue != null)
                {
                    foreach (var tmp in command.configurationValue)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class CONFIGURATION_NAME_GET
        {
            public const byte ID = 0x0A;
            public const byte parameterNumberBytesCount = 2;
            public byte[] parameterNumber = new byte[parameterNumberBytesCount];
            public static implicit operator CONFIGURATION_NAME_GET(byte[] data)
            {
                CONFIGURATION_NAME_GET ret = new CONFIGURATION_NAME_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.parameterNumber = (data.Length - index) >= parameterNumberBytesCount ? new byte[parameterNumberBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.parameterNumber[0] = data[index++];
                    if (data.Length > index) ret.parameterNumber[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](CONFIGURATION_NAME_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_CONFIGURATION_V4.ID);
                ret.Add(ID);
                if (command.parameterNumber != null)
                {
                    foreach (var tmp in command.parameterNumber)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class CONFIGURATION_NAME_REPORT
        {
            public const byte ID = 0x0B;
            public const byte parameterNumberBytesCount = 2;
            public byte[] parameterNumber = new byte[parameterNumberBytesCount];
            public ByteValue reportsToFollow = 0;
            public IList<byte> name = new List<byte>();
            public static implicit operator CONFIGURATION_NAME_REPORT(byte[] data)
            {
                CONFIGURATION_NAME_REPORT ret = new CONFIGURATION_NAME_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.parameterNumber = (data.Length - index) >= parameterNumberBytesCount ? new byte[parameterNumberBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.parameterNumber[0] = data[index++];
                    if (data.Length > index) ret.parameterNumber[1] = data[index++];
                    ret.reportsToFollow = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.name = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.name.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](CONFIGURATION_NAME_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_CONFIGURATION_V4.ID);
                ret.Add(ID);
                if (command.parameterNumber != null)
                {
                    foreach (var tmp in command.parameterNumber)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.reportsToFollow.HasValue) ret.Add(command.reportsToFollow);
                if (command.name != null)
                {
                    foreach (var tmp in command.name)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class CONFIGURATION_INFO_GET
        {
            public const byte ID = 0x0C;
            public const byte parameterNumberBytesCount = 2;
            public byte[] parameterNumber = new byte[parameterNumberBytesCount];
            public static implicit operator CONFIGURATION_INFO_GET(byte[] data)
            {
                CONFIGURATION_INFO_GET ret = new CONFIGURATION_INFO_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.parameterNumber = (data.Length - index) >= parameterNumberBytesCount ? new byte[parameterNumberBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.parameterNumber[0] = data[index++];
                    if (data.Length > index) ret.parameterNumber[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](CONFIGURATION_INFO_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_CONFIGURATION_V4.ID);
                ret.Add(ID);
                if (command.parameterNumber != null)
                {
                    foreach (var tmp in command.parameterNumber)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class CONFIGURATION_INFO_REPORT
        {
            public const byte ID = 0x0D;
            public const byte parameterNumberBytesCount = 2;
            public byte[] parameterNumber = new byte[parameterNumberBytesCount];
            public ByteValue reportsToFollow = 0;
            public IList<byte> info = new List<byte>();
            public static implicit operator CONFIGURATION_INFO_REPORT(byte[] data)
            {
                CONFIGURATION_INFO_REPORT ret = new CONFIGURATION_INFO_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.parameterNumber = (data.Length - index) >= parameterNumberBytesCount ? new byte[parameterNumberBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.parameterNumber[0] = data[index++];
                    if (data.Length > index) ret.parameterNumber[1] = data[index++];
                    ret.reportsToFollow = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.info = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.info.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](CONFIGURATION_INFO_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_CONFIGURATION_V4.ID);
                ret.Add(ID);
                if (command.parameterNumber != null)
                {
                    foreach (var tmp in command.parameterNumber)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.reportsToFollow.HasValue) ret.Add(command.reportsToFollow);
                if (command.info != null)
                {
                    foreach (var tmp in command.info)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class CONFIGURATION_PROPERTIES_GET
        {
            public const byte ID = 0x0E;
            public const byte parameterNumberBytesCount = 2;
            public byte[] parameterNumber = new byte[parameterNumberBytesCount];
            public static implicit operator CONFIGURATION_PROPERTIES_GET(byte[] data)
            {
                CONFIGURATION_PROPERTIES_GET ret = new CONFIGURATION_PROPERTIES_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.parameterNumber = (data.Length - index) >= parameterNumberBytesCount ? new byte[parameterNumberBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.parameterNumber[0] = data[index++];
                    if (data.Length > index) ret.parameterNumber[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](CONFIGURATION_PROPERTIES_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_CONFIGURATION_V4.ID);
                ret.Add(ID);
                if (command.parameterNumber != null)
                {
                    foreach (var tmp in command.parameterNumber)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class CONFIGURATION_PROPERTIES_REPORT
        {
            public const byte ID = 0x0F;
            public const byte parameterNumberBytesCount = 2;
            public byte[] parameterNumber = new byte[parameterNumberBytesCount];
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte size
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte format
                {
                    get { return (byte)(_value >> 3 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x38; _value += (byte)(value << 3 & 0x38); }
                }
                public byte mreadonly
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte alteringCapabilities
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
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
            public IList<byte> minValue = new List<byte>();
            public IList<byte> maxValue = new List<byte>();
            public IList<byte> defaultValue = new List<byte>();
            public const byte nextParameterNumberBytesCount = 2;
            public byte[] nextParameterNumber = new byte[nextParameterNumberBytesCount];
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte advanced
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte noBulkSupport
                {
                    get { return (byte)(_value >> 1 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x02; _value += (byte)(value << 1 & 0x02); }
                }
                public byte reserved1
                {
                    get { return (byte)(_value >> 2 & 0x3F); }
                    set { HasValue = true; _value &= 0xFF - 0xFC; _value += (byte)(value << 2 & 0xFC); }
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
            public static implicit operator CONFIGURATION_PROPERTIES_REPORT(byte[] data)
            {
                CONFIGURATION_PROPERTIES_REPORT ret = new CONFIGURATION_PROPERTIES_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.parameterNumber = (data.Length - index) >= parameterNumberBytesCount ? new byte[parameterNumberBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.parameterNumber[0] = data[index++];
                    if (data.Length > index) ret.parameterNumber[1] = data[index++];
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.minValue = new List<byte>();
                    for (int i = 0; i < ret.properties1.size; i++)
                    {
                        if (data.Length > index) ret.minValue.Add(data[index++]);
                    }
                    ret.maxValue = new List<byte>();
                    for (int i = 0; i < ret.properties1.size; i++)
                    {
                        if (data.Length > index) ret.maxValue.Add(data[index++]);
                    }
                    ret.defaultValue = new List<byte>();
                    for (int i = 0; i < ret.properties1.size; i++)
                    {
                        if (data.Length > index) ret.defaultValue.Add(data[index++]);
                    }
                    ret.nextParameterNumber = (data.Length - index) >= nextParameterNumberBytesCount ? new byte[nextParameterNumberBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.nextParameterNumber[0] = data[index++];
                    if (data.Length > index) ret.nextParameterNumber[1] = data[index++];
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](CONFIGURATION_PROPERTIES_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_CONFIGURATION_V4.ID);
                ret.Add(ID);
                if (command.parameterNumber != null)
                {
                    foreach (var tmp in command.parameterNumber)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.minValue != null)
                {
                    foreach (var tmp in command.minValue)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.maxValue != null)
                {
                    foreach (var tmp in command.maxValue)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.defaultValue != null)
                {
                    foreach (var tmp in command.defaultValue)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.nextParameterNumber != null)
                {
                    foreach (var tmp in command.nextParameterNumber)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties2.HasValue) ret.Add(command.properties2);
                return ret.ToArray();
            }
        }
        public partial class CONFIGURATION_DEFAULT_RESET
        {
            public const byte ID = 0x01;
            public static implicit operator CONFIGURATION_DEFAULT_RESET(byte[] data)
            {
                CONFIGURATION_DEFAULT_RESET ret = new CONFIGURATION_DEFAULT_RESET();
                return ret;
            }
            public static implicit operator byte[](CONFIGURATION_DEFAULT_RESET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_CONFIGURATION_V4.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
    }
}

