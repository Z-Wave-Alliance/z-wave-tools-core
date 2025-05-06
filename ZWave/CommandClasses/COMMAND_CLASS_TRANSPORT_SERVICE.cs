using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_TRANSPORT_SERVICE
    {
        public const byte ID = 0x55;
        public const byte VERSION = 1;
        public partial class COMMAND_FIRST_FRAGMENT
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
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte sequenceNo
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 4 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0xF0; _value += (byte)(value << 4 & 0xF0); }
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
            public IList<byte> payload = new List<byte>();
            public const byte checksumBytesCount = 2;
            public byte[] checksum = new byte[checksumBytesCount];
            public static implicit operator COMMAND_FIRST_FRAGMENT(byte[] data)
            {
                COMMAND_FIRST_FRAGMENT ret = new COMMAND_FIRST_FRAGMENT();
                if (data != null)
                {
                    int index = 1;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.datagramSize2 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.payload = new List<byte>();
                    while (data.Length - 2 > index)
                    {
                        if (data.Length > index) ret.payload.Add(data[index++]);
                    }
                    ret.checksum = (data.Length - index) >= checksumBytesCount ? new byte[checksumBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.checksum[0] = data[index++];
                    if (data.Length > index) ret.checksum[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](COMMAND_FIRST_FRAGMENT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_TRANSPORT_SERVICE.ID);
                ret.Add((byte)((ID & ID_MASK) + command.properties1));
                if (command.datagramSize2.HasValue) ret.Add(command.datagramSize2);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.payload != null)
                {
                    foreach (var tmp in command.payload)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.checksum != null)
                {
                    foreach (var tmp in command.checksum)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class COMMAND_SUBSEQUENT_FRAGMENT
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
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte datagramOffset1
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte sequenceNo
                {
                    get { return (byte)(_value >> 3 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x78; _value += (byte)(value << 3 & 0x78); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
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
            public ByteValue datagramOffset2 = 0;
            public IList<byte> payload = new List<byte>();
            public const byte checksumBytesCount = 2;
            public byte[] checksum = new byte[checksumBytesCount];
            public static implicit operator COMMAND_SUBSEQUENT_FRAGMENT(byte[] data)
            {
                COMMAND_SUBSEQUENT_FRAGMENT ret = new COMMAND_SUBSEQUENT_FRAGMENT();
                if (data != null)
                {
                    int index = 1;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.datagramSize2 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.datagramOffset2 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.payload = new List<byte>();
                    while (data.Length - 2 > index)
                    {
                        if (data.Length > index) ret.payload.Add(data[index++]);
                    }
                    ret.checksum = (data.Length - index) >= checksumBytesCount ? new byte[checksumBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.checksum[0] = data[index++];
                    if (data.Length > index) ret.checksum[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](COMMAND_SUBSEQUENT_FRAGMENT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_TRANSPORT_SERVICE.ID);
                ret.Add((byte)((ID & ID_MASK) + command.properties1));
                if (command.datagramSize2.HasValue) ret.Add(command.datagramSize2);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.datagramOffset2.HasValue) ret.Add(command.datagramOffset2);
                if (command.payload != null)
                {
                    foreach (var tmp in command.payload)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.checksum != null)
                {
                    foreach (var tmp in command.checksum)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

