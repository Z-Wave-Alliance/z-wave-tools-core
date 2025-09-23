/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_ZIP_6LOWPAN
    {
        public const byte ID = 0x4F;
        public const byte VERSION = 1;
        public partial class LOWPAN_FIRST_FRAGMENT
        {
            public const byte ID = 0xC0;
            public const byte ID_MASK = 0xF8;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte datagramSize1
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
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
            public ByteValue datagramSize2 = 0;
            public ByteValue datagramTag = 0;
            public IList<byte> payload = new List<byte>();
            public static implicit operator LOWPAN_FIRST_FRAGMENT(byte[] data)
            {
                LOWPAN_FIRST_FRAGMENT ret = new LOWPAN_FIRST_FRAGMENT();
                if (data != null)
                {
                    int index = 1;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.datagramSize2 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.datagramTag = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.payload = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.payload.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](LOWPAN_FIRST_FRAGMENT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZIP_6LOWPAN.ID);
                ret.Add((byte)((ID & ID_MASK) + command.properties1));
                if (command.datagramSize2.HasValue) ret.Add(command.datagramSize2);
                if (command.datagramTag.HasValue) ret.Add(command.datagramTag);
                if (command.payload != null)
                {
                    foreach (var tmp in command.payload)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class LOWPAN_SUBSEQUENT_FRAGMENT
        {
            public const byte ID = 0xE0;
            public const byte ID_MASK = 0xF8;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte datagramSize1
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
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
            public ByteValue datagramSize2 = 0;
            public ByteValue datagramTag = 0;
            public ByteValue datagramOffset = 0;
            public IList<byte> payload = new List<byte>();
            public static implicit operator LOWPAN_SUBSEQUENT_FRAGMENT(byte[] data)
            {
                LOWPAN_SUBSEQUENT_FRAGMENT ret = new LOWPAN_SUBSEQUENT_FRAGMENT();
                if (data != null)
                {
                    int index = 1;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.datagramSize2 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.datagramTag = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.datagramOffset = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.payload = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.payload.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](LOWPAN_SUBSEQUENT_FRAGMENT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZIP_6LOWPAN.ID);
                ret.Add((byte)((ID & ID_MASK) + command.properties1));
                if (command.datagramSize2.HasValue) ret.Add(command.datagramSize2);
                if (command.datagramTag.HasValue) ret.Add(command.datagramTag);
                if (command.datagramOffset.HasValue) ret.Add(command.datagramOffset);
                if (command.payload != null)
                {
                    foreach (var tmp in command.payload)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

