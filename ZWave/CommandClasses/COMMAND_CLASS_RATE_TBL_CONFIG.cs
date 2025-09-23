/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_RATE_TBL_CONFIG
    {
        public const byte ID = 0x48;
        public const byte VERSION = 1;
        public partial class RATE_TBL_REMOVE
        {
            public const byte ID = 0x02;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte rateParameterSetIds
                {
                    get { return (byte)(_value >> 0 & 0x3F); }
                    set { HasValue = true; _value &= 0xFF - 0x3F; _value += (byte)(value << 0 & 0x3F); }
                }
                public byte reserved
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
            public IList<byte> rateParameterSetId = new List<byte>();
            public static implicit operator RATE_TBL_REMOVE(byte[] data)
            {
                RATE_TBL_REMOVE ret = new RATE_TBL_REMOVE();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.rateParameterSetId = new List<byte>();
                    for (int i = 0; i < ret.properties1.rateParameterSetIds; i++)
                    {
                        if (data.Length > index) ret.rateParameterSetId.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](RATE_TBL_REMOVE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_RATE_TBL_CONFIG.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.rateParameterSetId != null)
                {
                    foreach (var tmp in command.rateParameterSetId)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class RATE_TBL_SET
        {
            public const byte ID = 0x01;
            public ByteValue rateParameterSetId = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte numberOfRateChar
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte rateType
                {
                    get { return (byte)(_value >> 5 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x60; _value += (byte)(value << 5 & 0x60); }
                }
                public byte reserved
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
            public IList<byte> rateCharacter = new List<byte>();
            public ByteValue startHourLocalTime = 0;
            public ByteValue startMinuteLocalTime = 0;
            public const byte durationMinuteBytesCount = 2;
            public byte[] durationMinute = new byte[durationMinuteBytesCount];
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte consumptionScale
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte consumptionPrecision
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
            public const byte minConsumptionValueBytesCount = 4;
            public byte[] minConsumptionValue = new byte[minConsumptionValueBytesCount];
            public const byte maxConsumptionValueBytesCount = 4;
            public byte[] maxConsumptionValue = new byte[maxConsumptionValueBytesCount];
            public struct Tproperties3
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties3 Empty { get { return new Tproperties3() { _value = 0, HasValue = false }; } }
                public byte maxDemandScale
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte maxDemandPrecision
                {
                    get { return (byte)(_value >> 5 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0xE0; _value += (byte)(value << 5 & 0xE0); }
                }
                public static implicit operator Tproperties3(byte data)
                {
                    Tproperties3 ret = new Tproperties3();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties3 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties3 properties3 = 0;
            public const byte maxDemandValueBytesCount = 4;
            public byte[] maxDemandValue = new byte[maxDemandValueBytesCount];
            public ByteValue dcpRateId = 0;
            public static implicit operator RATE_TBL_SET(byte[] data)
            {
                RATE_TBL_SET ret = new RATE_TBL_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.rateParameterSetId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.rateCharacter = new List<byte>();
                    for (int i = 0; i < ret.properties1.numberOfRateChar; i++)
                    {
                        if (data.Length > index) ret.rateCharacter.Add(data[index++]);
                    }
                    ret.startHourLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startMinuteLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.durationMinute = (data.Length - index) >= durationMinuteBytesCount ? new byte[durationMinuteBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.durationMinute[0] = data[index++];
                    if (data.Length > index) ret.durationMinute[1] = data[index++];
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.minConsumptionValue = (data.Length - index) >= minConsumptionValueBytesCount ? new byte[minConsumptionValueBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.minConsumptionValue[0] = data[index++];
                    if (data.Length > index) ret.minConsumptionValue[1] = data[index++];
                    if (data.Length > index) ret.minConsumptionValue[2] = data[index++];
                    if (data.Length > index) ret.minConsumptionValue[3] = data[index++];
                    ret.maxConsumptionValue = (data.Length - index) >= maxConsumptionValueBytesCount ? new byte[maxConsumptionValueBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.maxConsumptionValue[0] = data[index++];
                    if (data.Length > index) ret.maxConsumptionValue[1] = data[index++];
                    if (data.Length > index) ret.maxConsumptionValue[2] = data[index++];
                    if (data.Length > index) ret.maxConsumptionValue[3] = data[index++];
                    ret.properties3 = data.Length > index ? (Tproperties3)data[index++] : Tproperties3.Empty;
                    ret.maxDemandValue = (data.Length - index) >= maxDemandValueBytesCount ? new byte[maxDemandValueBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.maxDemandValue[0] = data[index++];
                    if (data.Length > index) ret.maxDemandValue[1] = data[index++];
                    if (data.Length > index) ret.maxDemandValue[2] = data[index++];
                    if (data.Length > index) ret.maxDemandValue[3] = data[index++];
                    ret.dcpRateId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](RATE_TBL_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_RATE_TBL_CONFIG.ID);
                ret.Add(ID);
                if (command.rateParameterSetId.HasValue) ret.Add(command.rateParameterSetId);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.rateCharacter != null)
                {
                    foreach (var tmp in command.rateCharacter)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.startHourLocalTime.HasValue) ret.Add(command.startHourLocalTime);
                if (command.startMinuteLocalTime.HasValue) ret.Add(command.startMinuteLocalTime);
                if (command.durationMinute != null)
                {
                    foreach (var tmp in command.durationMinute)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.minConsumptionValue != null)
                {
                    foreach (var tmp in command.minConsumptionValue)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.maxConsumptionValue != null)
                {
                    foreach (var tmp in command.maxConsumptionValue)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties3.HasValue) ret.Add(command.properties3);
                if (command.maxDemandValue != null)
                {
                    foreach (var tmp in command.maxDemandValue)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.dcpRateId.HasValue) ret.Add(command.dcpRateId);
                return ret.ToArray();
            }
        }
    }
}

