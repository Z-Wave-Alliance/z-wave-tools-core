/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_SUPERVISION
    {
        public const byte ID = 0x6C;
        public const byte VERSION = 1;
        public partial class SUPERVISION_GET
        {
            public const byte ID = 0x01;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte sessionId
                {
                    get { return (byte)(_value >> 0 & 0x3F); }
                    set { HasValue = true; _value &= 0xFF - 0x3F; _value += (byte)(value << 0 & 0x3F); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte statusUpdates
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
            public ByteValue encapsulatedCommandLength = 0;
            public IList<byte> encapsulatedCommand = new List<byte>();
            public static implicit operator SUPERVISION_GET(byte[] data)
            {
                SUPERVISION_GET ret = new SUPERVISION_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.encapsulatedCommandLength = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.encapsulatedCommand = new List<byte>();
                    for (int i = 0; i < ret.encapsulatedCommandLength; i++)
                    {
                        if (data.Length > index) ret.encapsulatedCommand.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](SUPERVISION_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SUPERVISION.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.encapsulatedCommandLength.HasValue) ret.Add(command.encapsulatedCommandLength);
                if (command.encapsulatedCommand != null)
                {
                    foreach (var tmp in command.encapsulatedCommand)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class SUPERVISION_REPORT
        {
            public const byte ID = 0x02;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte sessionId
                {
                    get { return (byte)(_value >> 0 & 0x3F); }
                    set { HasValue = true; _value &= 0xFF - 0x3F; _value += (byte)(value << 0 & 0x3F); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte moreStatusUpdates
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
            public ByteValue status = 0;
            public ByteValue duration = 0;
            public static implicit operator SUPERVISION_REPORT(byte[] data)
            {
                SUPERVISION_REPORT ret = new SUPERVISION_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.status = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.duration = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SUPERVISION_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SUPERVISION.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.status.HasValue) ret.Add(command.status);
                if (command.duration.HasValue) ret.Add(command.duration);
                return ret.ToArray();
            }
        }
    }
}

