/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_CONFIGURATION
    {
        public const byte ID = 0x70;
        public const byte VERSION = 1;
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
                ret.Add(COMMAND_CLASS_CONFIGURATION.ID);
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
                ret.Add(COMMAND_CLASS_CONFIGURATION.ID);
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
                ret.Add(COMMAND_CLASS_CONFIGURATION.ID);
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

