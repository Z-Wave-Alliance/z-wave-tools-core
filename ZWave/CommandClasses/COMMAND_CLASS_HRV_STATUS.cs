/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_HRV_STATUS
    {
        public const byte ID = 0x37;
        public const byte VERSION = 1;
        public partial class HRV_STATUS_GET
        {
            public const byte ID = 0x01;
            public ByteValue statusParameter = 0;
            public static implicit operator HRV_STATUS_GET(byte[] data)
            {
                HRV_STATUS_GET ret = new HRV_STATUS_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.statusParameter = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](HRV_STATUS_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_HRV_STATUS.ID);
                ret.Add(ID);
                if (command.statusParameter.HasValue) ret.Add(command.statusParameter);
                return ret.ToArray();
            }
        }
        public partial class HRV_STATUS_REPORT
        {
            public const byte ID = 0x02;
            public ByteValue statusParameter = 0;
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
            public IList<byte> value = new List<byte>();
            public static implicit operator HRV_STATUS_REPORT(byte[] data)
            {
                HRV_STATUS_REPORT ret = new HRV_STATUS_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.statusParameter = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.value = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.value.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](HRV_STATUS_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_HRV_STATUS.ID);
                ret.Add(ID);
                if (command.statusParameter.HasValue) ret.Add(command.statusParameter);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.value != null)
                {
                    foreach (var tmp in command.value)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class HRV_STATUS_SUPPORTED_GET
        {
            public const byte ID = 0x03;
            public static implicit operator HRV_STATUS_SUPPORTED_GET(byte[] data)
            {
                HRV_STATUS_SUPPORTED_GET ret = new HRV_STATUS_SUPPORTED_GET();
                return ret;
            }
            public static implicit operator byte[](HRV_STATUS_SUPPORTED_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_HRV_STATUS.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class HRV_STATUS_SUPPORTED_REPORT
        {
            public const byte ID = 0x04;
            public IList<byte> bitMask = new List<byte>();
            public static implicit operator HRV_STATUS_SUPPORTED_REPORT(byte[] data)
            {
                HRV_STATUS_SUPPORTED_REPORT ret = new HRV_STATUS_SUPPORTED_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.bitMask = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.bitMask.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](HRV_STATUS_SUPPORTED_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_HRV_STATUS.ID);
                ret.Add(ID);
                if (command.bitMask != null)
                {
                    foreach (var tmp in command.bitMask)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

