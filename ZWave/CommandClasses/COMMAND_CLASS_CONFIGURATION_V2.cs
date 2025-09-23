/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_CONFIGURATION_V2
    {
        public const byte ID = 0x70;
        public const byte VERSION = 2;
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
                ret.Add(COMMAND_CLASS_CONFIGURATION_V2.ID);
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
                ret.Add(COMMAND_CLASS_CONFIGURATION_V2.ID);
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
                ret.Add(COMMAND_CLASS_CONFIGURATION_V2.ID);
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
                ret.Add(COMMAND_CLASS_CONFIGURATION_V2.ID);
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
                ret.Add(COMMAND_CLASS_CONFIGURATION_V2.ID);
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
                ret.Add(COMMAND_CLASS_CONFIGURATION_V2.ID);
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
    }
}

