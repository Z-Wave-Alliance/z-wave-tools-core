// SPDX-License-Identifier: BSD-3-Clause
// SPDX-FileCopyrightText: Z-Wave Alliance <https://z-wavealliance.org>
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_WAKE_ON_CRITICAL_MESSAGE
    {
        public const byte ID = 0x8D;
        public const byte VERSION = 1;
        public partial class WAKE_ON_CRITICAL_MESSAGE_CONFIGURATION_SET
        {
            public const byte ID = 0x01;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte severity
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
            public static implicit operator WAKE_ON_CRITICAL_MESSAGE_CONFIGURATION_SET(byte[] data)
            {
                WAKE_ON_CRITICAL_MESSAGE_CONFIGURATION_SET ret = new WAKE_ON_CRITICAL_MESSAGE_CONFIGURATION_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](WAKE_ON_CRITICAL_MESSAGE_CONFIGURATION_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_WAKE_ON_CRITICAL_MESSAGE.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                return ret.ToArray();
            }
        }
        public partial class WAKE_ON_CRITICAL_MESSAGE_CONFIGURATION_GET
        {
            public const byte ID = 0x02;
            public static implicit operator WAKE_ON_CRITICAL_MESSAGE_CONFIGURATION_GET(byte[] data)
            {
                WAKE_ON_CRITICAL_MESSAGE_CONFIGURATION_GET ret = new WAKE_ON_CRITICAL_MESSAGE_CONFIGURATION_GET();
                return ret;
            }
            public static implicit operator byte[](WAKE_ON_CRITICAL_MESSAGE_CONFIGURATION_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_WAKE_ON_CRITICAL_MESSAGE.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class WAKE_ON_CRITICAL_MESSAGE_CONFIGURATION_REPORT
        {
            public const byte ID = 0x03;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte severity
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
            public static implicit operator WAKE_ON_CRITICAL_MESSAGE_CONFIGURATION_REPORT(byte[] data)
            {
                WAKE_ON_CRITICAL_MESSAGE_CONFIGURATION_REPORT ret = new WAKE_ON_CRITICAL_MESSAGE_CONFIGURATION_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](WAKE_ON_CRITICAL_MESSAGE_CONFIGURATION_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_WAKE_ON_CRITICAL_MESSAGE.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                return ret.ToArray();
            }
        }
        public partial class WAKE_ON_CRITICAL_MESSAGE_NOTIFY
        {
            public const byte ID = 0x04;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte severity
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
            public ByteValue payloadLength = 0;
            public IList<byte> encapsulatedPayload = new List<byte>();
            public static implicit operator WAKE_ON_CRITICAL_MESSAGE_NOTIFY(byte[] data)
            {
                WAKE_ON_CRITICAL_MESSAGE_NOTIFY ret = new WAKE_ON_CRITICAL_MESSAGE_NOTIFY();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.payloadLength = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.encapsulatedPayload = new List<byte>();
                    for (int i = 0; i < ret.payloadLength; i++)
                    {
                        if (data.Length > index) ret.encapsulatedPayload.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](WAKE_ON_CRITICAL_MESSAGE_NOTIFY command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_WAKE_ON_CRITICAL_MESSAGE.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.payloadLength.HasValue) ret.Add(command.payloadLength);
                if (command.encapsulatedPayload != null)
                {
                    foreach (var tmp in command.encapsulatedPayload)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

