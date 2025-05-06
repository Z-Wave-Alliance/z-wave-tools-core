using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_CLOCK
    {
        public const byte ID = 0x81;
        public const byte VERSION = 1;
        public partial class CLOCK_GET
        {
            public const byte ID = 0x05;
            public static implicit operator CLOCK_GET(byte[] data)
            {
                CLOCK_GET ret = new CLOCK_GET();
                return ret;
            }
            public static implicit operator byte[](CLOCK_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_CLOCK.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class CLOCK_REPORT
        {
            public const byte ID = 0x06;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte hour
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte weekday
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
            public ByteValue minute = 0;
            public static implicit operator CLOCK_REPORT(byte[] data)
            {
                CLOCK_REPORT ret = new CLOCK_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.minute = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](CLOCK_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_CLOCK.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.minute.HasValue) ret.Add(command.minute);
                return ret.ToArray();
            }
        }
        public partial class CLOCK_SET
        {
            public const byte ID = 0x04;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte hour
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte weekday
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
            public ByteValue minute = 0;
            public static implicit operator CLOCK_SET(byte[] data)
            {
                CLOCK_SET ret = new CLOCK_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.minute = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](CLOCK_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_CLOCK.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.minute.HasValue) ret.Add(command.minute);
                return ret.ToArray();
            }
        }
    }
}

