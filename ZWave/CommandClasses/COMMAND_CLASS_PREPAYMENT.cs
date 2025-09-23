/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_PREPAYMENT
    {
        public const byte ID = 0x3F;
        public const byte VERSION = 1;
        public partial class PREPAYMENT_BALANCE_GET
        {
            public const byte ID = 0x01;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte reserved
                {
                    get { return (byte)(_value >> 0 & 0x3F); }
                    set { HasValue = true; _value &= 0xFF - 0x3F; _value += (byte)(value << 0 & 0x3F); }
                }
                public byte balanceType
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
            public static implicit operator PREPAYMENT_BALANCE_GET(byte[] data)
            {
                PREPAYMENT_BALANCE_GET ret = new PREPAYMENT_BALANCE_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](PREPAYMENT_BALANCE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_PREPAYMENT.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                return ret.ToArray();
            }
        }
        public partial class PREPAYMENT_BALANCE_REPORT
        {
            public const byte ID = 0x02;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte meterType
                {
                    get { return (byte)(_value >> 0 & 0x3F); }
                    set { HasValue = true; _value &= 0xFF - 0x3F; _value += (byte)(value << 0 & 0x3F); }
                }
                public byte balanceType
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
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte scale
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte balancePrecision
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
            public const byte balanceValueBytesCount = 4;
            public byte[] balanceValue = new byte[balanceValueBytesCount];
            public struct Tproperties3
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties3 Empty { get { return new Tproperties3() { _value = 0, HasValue = false }; } }
                public byte reserved1
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte debtPrecision
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
            public const byte debtBytesCount = 4;
            public byte[] debt = new byte[debtBytesCount];
            public struct Tproperties4
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties4 Empty { get { return new Tproperties4() { _value = 0, HasValue = false }; } }
                public byte reserved2
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte emerCreditPrecision
                {
                    get { return (byte)(_value >> 5 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0xE0; _value += (byte)(value << 5 & 0xE0); }
                }
                public static implicit operator Tproperties4(byte data)
                {
                    Tproperties4 ret = new Tproperties4();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties4 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties4 properties4 = 0;
            public const byte emerCreditBytesCount = 4;
            public byte[] emerCredit = new byte[emerCreditBytesCount];
            public const byte currencyBytesCount = 3;
            public byte[] currency = new byte[currencyBytesCount];
            public ByteValue debtRecoveryPercentage = 0;
            public static implicit operator PREPAYMENT_BALANCE_REPORT(byte[] data)
            {
                PREPAYMENT_BALANCE_REPORT ret = new PREPAYMENT_BALANCE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.balanceValue = (data.Length - index) >= balanceValueBytesCount ? new byte[balanceValueBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.balanceValue[0] = data[index++];
                    if (data.Length > index) ret.balanceValue[1] = data[index++];
                    if (data.Length > index) ret.balanceValue[2] = data[index++];
                    if (data.Length > index) ret.balanceValue[3] = data[index++];
                    ret.properties3 = data.Length > index ? (Tproperties3)data[index++] : Tproperties3.Empty;
                    ret.debt = (data.Length - index) >= debtBytesCount ? new byte[debtBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.debt[0] = data[index++];
                    if (data.Length > index) ret.debt[1] = data[index++];
                    if (data.Length > index) ret.debt[2] = data[index++];
                    if (data.Length > index) ret.debt[3] = data[index++];
                    ret.properties4 = data.Length > index ? (Tproperties4)data[index++] : Tproperties4.Empty;
                    ret.emerCredit = (data.Length - index) >= emerCreditBytesCount ? new byte[emerCreditBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.emerCredit[0] = data[index++];
                    if (data.Length > index) ret.emerCredit[1] = data[index++];
                    if (data.Length > index) ret.emerCredit[2] = data[index++];
                    if (data.Length > index) ret.emerCredit[3] = data[index++];
                    ret.currency = (data.Length - index) >= currencyBytesCount ? new byte[currencyBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.currency[0] = data[index++];
                    if (data.Length > index) ret.currency[1] = data[index++];
                    if (data.Length > index) ret.currency[2] = data[index++];
                    ret.debtRecoveryPercentage = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](PREPAYMENT_BALANCE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_PREPAYMENT.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.balanceValue != null)
                {
                    foreach (var tmp in command.balanceValue)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties3.HasValue) ret.Add(command.properties3);
                if (command.debt != null)
                {
                    foreach (var tmp in command.debt)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties4.HasValue) ret.Add(command.properties4);
                if (command.emerCredit != null)
                {
                    foreach (var tmp in command.emerCredit)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.currency != null)
                {
                    foreach (var tmp in command.currency)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.debtRecoveryPercentage.HasValue) ret.Add(command.debtRecoveryPercentage);
                return ret.ToArray();
            }
        }
        public partial class PREPAYMENT_SUPPORTED_GET
        {
            public const byte ID = 0x03;
            public static implicit operator PREPAYMENT_SUPPORTED_GET(byte[] data)
            {
                PREPAYMENT_SUPPORTED_GET ret = new PREPAYMENT_SUPPORTED_GET();
                return ret;
            }
            public static implicit operator byte[](PREPAYMENT_SUPPORTED_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_PREPAYMENT.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class PREPAYMENT_SUPPORTED_REPORT
        {
            public const byte ID = 0x04;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte typesSupported
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 4 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0xF0; _value += (byte)(value << 4 & 0xF0); }
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
            public static implicit operator PREPAYMENT_SUPPORTED_REPORT(byte[] data)
            {
                PREPAYMENT_SUPPORTED_REPORT ret = new PREPAYMENT_SUPPORTED_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](PREPAYMENT_SUPPORTED_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_PREPAYMENT.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                return ret.ToArray();
            }
        }
    }
}

