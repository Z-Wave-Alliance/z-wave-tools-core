using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_ENERGY_PRODUCTION
    {
        public const byte ID = 0x90;
        public const byte VERSION = 1;
        public partial class ENERGY_PRODUCTION_GET
        {
            public const byte ID = 0x02;
            public ByteValue parameterNumber = 0;
            public static implicit operator ENERGY_PRODUCTION_GET(byte[] data)
            {
                ENERGY_PRODUCTION_GET ret = new ENERGY_PRODUCTION_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.parameterNumber = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](ENERGY_PRODUCTION_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ENERGY_PRODUCTION.ID);
                ret.Add(ID);
                if (command.parameterNumber.HasValue) ret.Add(command.parameterNumber);
                return ret.ToArray();
            }
        }
        public partial class ENERGY_PRODUCTION_REPORT
        {
            public const byte ID = 0x03;
            public ByteValue parameterNumber = 0;
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
            public static implicit operator ENERGY_PRODUCTION_REPORT(byte[] data)
            {
                ENERGY_PRODUCTION_REPORT ret = new ENERGY_PRODUCTION_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.parameterNumber = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.value = new List<byte>();
                    for (int i = 0; i < ret.properties1.size; i++)
                    {
                        if (data.Length > index) ret.value.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](ENERGY_PRODUCTION_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ENERGY_PRODUCTION.ID);
                ret.Add(ID);
                if (command.parameterNumber.HasValue) ret.Add(command.parameterNumber);
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
    }
}

