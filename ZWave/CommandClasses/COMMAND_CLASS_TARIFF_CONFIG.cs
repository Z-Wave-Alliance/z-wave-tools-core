/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_TARIFF_CONFIG
    {
        public const byte ID = 0x4A;
        public const byte VERSION = 1;
        public partial class TARIFF_TBL_REMOVE
        {
            public const byte ID = 0x03;
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
            public static implicit operator TARIFF_TBL_REMOVE(byte[] data)
            {
                TARIFF_TBL_REMOVE ret = new TARIFF_TBL_REMOVE();
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
            public static implicit operator byte[](TARIFF_TBL_REMOVE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_TARIFF_CONFIG.ID);
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
        public partial class TARIFF_TBL_SET
        {
            public const byte ID = 0x02;
            public ByteValue rateParameterSetId = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte reserved
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte tariffPrecision
                {
                    get { return (byte)(_value >> 5 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0xE0; _value += (byte)(value << 5 & 0xE0); }
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
            public const byte tariffValueBytesCount = 4;
            public byte[] tariffValue = new byte[tariffValueBytesCount];
            public static implicit operator TARIFF_TBL_SET(byte[] data)
            {
                TARIFF_TBL_SET ret = new TARIFF_TBL_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.rateParameterSetId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.tariffValue = (data.Length - index) >= tariffValueBytesCount ? new byte[tariffValueBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.tariffValue[0] = data[index++];
                    if (data.Length > index) ret.tariffValue[1] = data[index++];
                    if (data.Length > index) ret.tariffValue[2] = data[index++];
                    if (data.Length > index) ret.tariffValue[3] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](TARIFF_TBL_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_TARIFF_CONFIG.ID);
                ret.Add(ID);
                if (command.rateParameterSetId.HasValue) ret.Add(command.rateParameterSetId);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.tariffValue != null)
                {
                    foreach (var tmp in command.tariffValue)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class TARIFF_TBL_SUPPLIER_SET
        {
            public const byte ID = 0x01;
            public const byte yearBytesCount = 2;
            public byte[] year = new byte[yearBytesCount];
            public ByteValue month = 0;
            public ByteValue day = 0;
            public ByteValue hourLocalTime = 0;
            public ByteValue minuteLocalTime = 0;
            public ByteValue secondLocalTime = 0;
            public const byte currencyBytesCount = 3;
            public byte[] currency = new byte[currencyBytesCount];
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte standingChargePeriod
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte standingChargePrecision
                {
                    get { return (byte)(_value >> 5 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0xE0; _value += (byte)(value << 5 & 0xE0); }
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
            public const byte standingChargeValueBytesCount = 4;
            public byte[] standingChargeValue = new byte[standingChargeValueBytesCount];
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte numberOfSupplierCharacters
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte reserved
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
            public IList<byte> supplierCharacter = new List<byte>();
            public static implicit operator TARIFF_TBL_SUPPLIER_SET(byte[] data)
            {
                TARIFF_TBL_SUPPLIER_SET ret = new TARIFF_TBL_SUPPLIER_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.year = (data.Length - index) >= yearBytesCount ? new byte[yearBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.year[0] = data[index++];
                    if (data.Length > index) ret.year[1] = data[index++];
                    ret.month = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.day = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.hourLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.minuteLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.secondLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.currency = (data.Length - index) >= currencyBytesCount ? new byte[currencyBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.currency[0] = data[index++];
                    if (data.Length > index) ret.currency[1] = data[index++];
                    if (data.Length > index) ret.currency[2] = data[index++];
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.standingChargeValue = (data.Length - index) >= standingChargeValueBytesCount ? new byte[standingChargeValueBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.standingChargeValue[0] = data[index++];
                    if (data.Length > index) ret.standingChargeValue[1] = data[index++];
                    if (data.Length > index) ret.standingChargeValue[2] = data[index++];
                    if (data.Length > index) ret.standingChargeValue[3] = data[index++];
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.supplierCharacter = new List<byte>();
                    for (int i = 0; i < ret.properties2.numberOfSupplierCharacters; i++)
                    {
                        if (data.Length > index) ret.supplierCharacter.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](TARIFF_TBL_SUPPLIER_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_TARIFF_CONFIG.ID);
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
                if (command.hourLocalTime.HasValue) ret.Add(command.hourLocalTime);
                if (command.minuteLocalTime.HasValue) ret.Add(command.minuteLocalTime);
                if (command.secondLocalTime.HasValue) ret.Add(command.secondLocalTime);
                if (command.currency != null)
                {
                    foreach (var tmp in command.currency)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.standingChargeValue != null)
                {
                    foreach (var tmp in command.standingChargeValue)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.supplierCharacter != null)
                {
                    foreach (var tmp in command.supplierCharacter)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

