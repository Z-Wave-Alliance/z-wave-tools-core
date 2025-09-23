/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_HUMIDITY_CONTROL_MODE_V2
    {
        public const byte ID = 0x6D;
        public const byte VERSION = 2;
        public partial class HUMIDITY_CONTROL_MODE_SET
        {
            public const byte ID = 0x01;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte mode
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
            public static implicit operator HUMIDITY_CONTROL_MODE_SET(byte[] data)
            {
                HUMIDITY_CONTROL_MODE_SET ret = new HUMIDITY_CONTROL_MODE_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](HUMIDITY_CONTROL_MODE_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_HUMIDITY_CONTROL_MODE_V2.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                return ret.ToArray();
            }
        }
        public partial class HUMIDITY_CONTROL_MODE_GET
        {
            public const byte ID = 0x02;
            public static implicit operator HUMIDITY_CONTROL_MODE_GET(byte[] data)
            {
                HUMIDITY_CONTROL_MODE_GET ret = new HUMIDITY_CONTROL_MODE_GET();
                return ret;
            }
            public static implicit operator byte[](HUMIDITY_CONTROL_MODE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_HUMIDITY_CONTROL_MODE_V2.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class HUMIDITY_CONTROL_MODE_REPORT
        {
            public const byte ID = 0x03;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte mode
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
            public static implicit operator HUMIDITY_CONTROL_MODE_REPORT(byte[] data)
            {
                HUMIDITY_CONTROL_MODE_REPORT ret = new HUMIDITY_CONTROL_MODE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](HUMIDITY_CONTROL_MODE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_HUMIDITY_CONTROL_MODE_V2.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                return ret.ToArray();
            }
        }
        public partial class HUMIDITY_CONTROL_MODE_SUPPORTED_GET
        {
            public const byte ID = 0x04;
            public static implicit operator HUMIDITY_CONTROL_MODE_SUPPORTED_GET(byte[] data)
            {
                HUMIDITY_CONTROL_MODE_SUPPORTED_GET ret = new HUMIDITY_CONTROL_MODE_SUPPORTED_GET();
                return ret;
            }
            public static implicit operator byte[](HUMIDITY_CONTROL_MODE_SUPPORTED_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_HUMIDITY_CONTROL_MODE_V2.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class HUMIDITY_CONTROL_MODE_SUPPORTED_REPORT
        {
            public const byte ID = 0x05;
            public ByteValue bitMask = 0;
            public static implicit operator HUMIDITY_CONTROL_MODE_SUPPORTED_REPORT(byte[] data)
            {
                HUMIDITY_CONTROL_MODE_SUPPORTED_REPORT ret = new HUMIDITY_CONTROL_MODE_SUPPORTED_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.bitMask = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](HUMIDITY_CONTROL_MODE_SUPPORTED_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_HUMIDITY_CONTROL_MODE_V2.ID);
                ret.Add(ID);
                if (command.bitMask.HasValue) ret.Add(command.bitMask);
                return ret.ToArray();
            }
        }
    }
}

