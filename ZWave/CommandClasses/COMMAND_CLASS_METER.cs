/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_METER
    {
        public const byte ID = 0x32;
        public const byte VERSION = 1;
        public partial class METER_GET
        {
            public const byte ID = 0x01;
            public static implicit operator METER_GET(byte[] data)
            {
                METER_GET ret = new METER_GET();
                return ret;
            }
            public static implicit operator byte[](METER_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_METER.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class METER_REPORT
        {
            public const byte ID = 0x02;
            public ByteValue meterType = 0;
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
                public byte scale
                {
                    get { return (byte)(_value >> 3 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x18; _value += (byte)(value << 3 & 0x18); }
                }
                public byte precision
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
            public IList<byte> meterValue = new List<byte>();
            public static implicit operator METER_REPORT(byte[] data)
            {
                METER_REPORT ret = new METER_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.meterType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.meterValue = new List<byte>();
                    for (int i = 0; i < ret.properties1.size; i++)
                    {
                        if (data.Length > index) ret.meterValue.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](METER_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_METER.ID);
                ret.Add(ID);
                if (command.meterType.HasValue) ret.Add(command.meterType);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.meterValue != null)
                {
                    foreach (var tmp in command.meterValue)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

