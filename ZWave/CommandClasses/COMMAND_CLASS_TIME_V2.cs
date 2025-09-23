/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_TIME_V2
    {
        public const byte ID = 0x8A;
        public const byte VERSION = 2;
        public partial class DATE_GET
        {
            public const byte ID = 0x03;
            public static implicit operator DATE_GET(byte[] data)
            {
                DATE_GET ret = new DATE_GET();
                return ret;
            }
            public static implicit operator byte[](DATE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_TIME_V2.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class DATE_REPORT
        {
            public const byte ID = 0x04;
            public const byte yearBytesCount = 2;
            public byte[] year = new byte[yearBytesCount];
            public ByteValue month = 0;
            public ByteValue day = 0;
            public static implicit operator DATE_REPORT(byte[] data)
            {
                DATE_REPORT ret = new DATE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.year = (data.Length - index) >= yearBytesCount ? new byte[yearBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.year[0] = data[index++];
                    if (data.Length > index) ret.year[1] = data[index++];
                    ret.month = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.day = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](DATE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_TIME_V2.ID);
                ret.Add(ID);
                if (command.year != null)
                {
                    foreach (var tmp in command.year)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.month.HasValue) ret.Add(command.month);
                if (command.day.HasValue) ret.Add(command.day);
                return ret.ToArray();
            }
        }
        public partial class TIME_GET
        {
            public const byte ID = 0x01;
            public static implicit operator TIME_GET(byte[] data)
            {
                TIME_GET ret = new TIME_GET();
                return ret;
            }
            public static implicit operator byte[](TIME_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_TIME_V2.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class TIME_OFFSET_GET
        {
            public const byte ID = 0x06;
            public static implicit operator TIME_OFFSET_GET(byte[] data)
            {
                TIME_OFFSET_GET ret = new TIME_OFFSET_GET();
                return ret;
            }
            public static implicit operator byte[](TIME_OFFSET_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_TIME_V2.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class TIME_OFFSET_REPORT
        {
            public const byte ID = 0x07;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte hourTzo
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte signTzo
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
            public ByteValue minuteTzo = 0;
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte minuteOffsetDst
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte signOffsetDst
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
            public ByteValue monthStartDst = 0;
            public ByteValue dayStartDst = 0;
            public ByteValue hourStartDst = 0;
            public ByteValue monthEndDst = 0;
            public ByteValue dayEndDst = 0;
            public ByteValue hourEndDst = 0;
            public static implicit operator TIME_OFFSET_REPORT(byte[] data)
            {
                TIME_OFFSET_REPORT ret = new TIME_OFFSET_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.minuteTzo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.monthStartDst = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.dayStartDst = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.hourStartDst = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.monthEndDst = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.dayEndDst = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.hourEndDst = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](TIME_OFFSET_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_TIME_V2.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.minuteTzo.HasValue) ret.Add(command.minuteTzo);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.monthStartDst.HasValue) ret.Add(command.monthStartDst);
                if (command.dayStartDst.HasValue) ret.Add(command.dayStartDst);
                if (command.hourStartDst.HasValue) ret.Add(command.hourStartDst);
                if (command.monthEndDst.HasValue) ret.Add(command.monthEndDst);
                if (command.dayEndDst.HasValue) ret.Add(command.dayEndDst);
                if (command.hourEndDst.HasValue) ret.Add(command.hourEndDst);
                return ret.ToArray();
            }
        }
        public partial class TIME_OFFSET_SET
        {
            public const byte ID = 0x05;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte hourTzo
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte signTzo
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
            public ByteValue minuteTzo = 0;
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte minuteOffsetDst
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte signOffsetDst
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
            public ByteValue monthStartDst = 0;
            public ByteValue dayStartDst = 0;
            public ByteValue hourStartDst = 0;
            public ByteValue monthEndDst = 0;
            public ByteValue dayEndDst = 0;
            public ByteValue hourEndDst = 0;
            public static implicit operator TIME_OFFSET_SET(byte[] data)
            {
                TIME_OFFSET_SET ret = new TIME_OFFSET_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.minuteTzo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.monthStartDst = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.dayStartDst = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.hourStartDst = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.monthEndDst = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.dayEndDst = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.hourEndDst = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](TIME_OFFSET_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_TIME_V2.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.minuteTzo.HasValue) ret.Add(command.minuteTzo);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.monthStartDst.HasValue) ret.Add(command.monthStartDst);
                if (command.dayStartDst.HasValue) ret.Add(command.dayStartDst);
                if (command.hourStartDst.HasValue) ret.Add(command.hourStartDst);
                if (command.monthEndDst.HasValue) ret.Add(command.monthEndDst);
                if (command.dayEndDst.HasValue) ret.Add(command.dayEndDst);
                if (command.hourEndDst.HasValue) ret.Add(command.hourEndDst);
                return ret.ToArray();
            }
        }
        public partial class TIME_REPORT
        {
            public const byte ID = 0x02;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte hourLocalTime
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 5 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x60; _value += (byte)(value << 5 & 0x60); }
                }
                public byte rtcFailure
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
            public ByteValue minuteLocalTime = 0;
            public ByteValue secondLocalTime = 0;
            public static implicit operator TIME_REPORT(byte[] data)
            {
                TIME_REPORT ret = new TIME_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.minuteLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.secondLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](TIME_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_TIME_V2.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.minuteLocalTime.HasValue) ret.Add(command.minuteLocalTime);
                if (command.secondLocalTime.HasValue) ret.Add(command.secondLocalTime);
                return ret.ToArray();
            }
        }
    }
}

