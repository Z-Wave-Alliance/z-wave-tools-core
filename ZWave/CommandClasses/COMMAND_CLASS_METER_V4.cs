/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_METER_V4
    {
        public const byte ID = 0x32;
        public const byte VERSION = 4;
        public partial class METER_GET
        {
            public const byte ID = 0x01;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte reserved
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte scale
                {
                    get { return (byte)(_value >> 3 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x38; _value += (byte)(value << 3 & 0x38); }
                }
                public byte rateType
                {
                    get { return (byte)(_value >> 6 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0xC0; _value += (byte)(value << 6 & 0xC0); }
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
            public ByteValue scale2 = 0;
            public static implicit operator METER_GET(byte[] data)
            {
                METER_GET ret = new METER_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.scale2 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](METER_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_METER_V4.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.scale2.HasValue) ret.Add(command.scale2);
                return ret.ToArray();
            }
        }
        public partial class METER_REPORT
        {
            public const byte ID = 0x02;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte meterType
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte rateType
                {
                    get { return (byte)(_value >> 5 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x60; _value += (byte)(value << 5 & 0x60); }
                }
                public byte scaleBit2
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
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte size
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte scaleBits10
                {
                    get { return (byte)(_value >> 3 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x18; _value += (byte)(value << 3 & 0x18); }
                }
                public byte precision
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
            public IList<byte> meterValue = new List<byte>();
            public const byte deltaTimeBytesCount = 2;
            public byte[] deltaTime = new byte[deltaTimeBytesCount];
            public IList<byte> previousMeterValue = new List<byte>();
            public ByteValue scale2 = 0;
            public static implicit operator METER_REPORT(byte[] data)
            {
                METER_REPORT ret = new METER_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.meterValue = new List<byte>();
                    for (int i = 0; i < ret.properties2.size; i++)
                    {
                        if (data.Length > index) ret.meterValue.Add(data[index++]);
                    }
                    ret.deltaTime = (data.Length - index) >= deltaTimeBytesCount ? new byte[deltaTimeBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.deltaTime[0] = data[index++];
                    if (data.Length > index) ret.deltaTime[1] = data[index++];
                    if (ret.deltaTime != null && ret.deltaTime.Length == deltaTimeBytesCount)
                    {
                        ret.previousMeterValue = new List<byte>();
                        for (int i = 0; i < ret.properties2.size; i++)
                        {
                            if (data.Length > index) ret.previousMeterValue.Add(data[index++]);
                        }
                    }
                    ret.scale2 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](METER_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_METER_V4.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.meterValue != null)
                {
                    foreach (var tmp in command.meterValue)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.deltaTime != null)
                {
                    foreach (var tmp in command.deltaTime)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.deltaTime != null && command.deltaTime.Length == deltaTimeBytesCount)
                {
                    if (command.previousMeterValue != null)
                    {
                        foreach (var tmp in command.previousMeterValue)
                        {
                            ret.Add(tmp);
                        }
                    }
                }
                if (command.scale2.HasValue) ret.Add(command.scale2);
                return ret.ToArray();
            }
        }
        public partial class METER_RESET
        {
            public const byte ID = 0x05;
            public static implicit operator METER_RESET(byte[] data)
            {
                METER_RESET ret = new METER_RESET();
                return ret;
            }
            public static implicit operator byte[](METER_RESET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_METER_V4.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class METER_SUPPORTED_GET
        {
            public const byte ID = 0x03;
            public static implicit operator METER_SUPPORTED_GET(byte[] data)
            {
                METER_SUPPORTED_GET ret = new METER_SUPPORTED_GET();
                return ret;
            }
            public static implicit operator byte[](METER_SUPPORTED_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_METER_V4.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class METER_SUPPORTED_REPORT
        {
            public const byte ID = 0x04;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte meterType
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte rateType
                {
                    get { return (byte)(_value >> 5 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x60; _value += (byte)(value << 5 & 0x60); }
                }
                public byte meterReset
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
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte scaleSupported0
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte mST
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
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
            public ByteValue numberOfScaleSupportedBytesToFollow = 0;
            public IList<byte> scaleSupported = new List<byte>();
            public static implicit operator METER_SUPPORTED_REPORT(byte[] data)
            {
                METER_SUPPORTED_REPORT ret = new METER_SUPPORTED_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.numberOfScaleSupportedBytesToFollow = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.scaleSupported = new List<byte>();
                    for (int i = 0; i < ret.numberOfScaleSupportedBytesToFollow; i++)
                    {
                        if (data.Length > index) ret.scaleSupported.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](METER_SUPPORTED_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_METER_V4.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.numberOfScaleSupportedBytesToFollow.HasValue) ret.Add(command.numberOfScaleSupportedBytesToFollow);
                if (command.scaleSupported != null)
                {
                    foreach (var tmp in command.scaleSupported)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

