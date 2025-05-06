using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_TRANSPORT_SERVICE_V2
    {
        public const byte ID = 0x55;
        public const byte VERSION = 2;
        public partial class COMMAND_FIRST_SEGMENT
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
                public byte reserved
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte ext
                {
                    get { return (byte)(_value >> 3 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x08; _value += (byte)(value << 3 & 0x08); }
                }
                public byte sessionId
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
            public ByteValue headerExtensionLength = 0;
            public IList<byte> headerExtension = new List<byte>();
            public IList<byte> payload = new List<byte>();
            public const byte frameCheckSequenceBytesCount = 2;
            public byte[] frameCheckSequence = new byte[frameCheckSequenceBytesCount];
            public static implicit operator COMMAND_FIRST_SEGMENT(byte[] data)
            {
                COMMAND_FIRST_SEGMENT ret = new COMMAND_FIRST_SEGMENT();
                if (data != null)
                {
                    int index = 1;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.datagramSize2 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    if (ret.properties2.ext > 0)
                    {
                        ret.headerExtensionLength = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    }
                    if (ret.properties2.ext > 0)
                    {
                        ret.headerExtension = new List<byte>();
                        for (int i = 0; i < ret.headerExtensionLength; i++)
                        {
                            if (data.Length > index) ret.headerExtension.Add(data[index++]);
                        }
                    }
                    ret.payload = new List<byte>();
                    while (data.Length - 2 > index)
                    {
                        if (data.Length > index) ret.payload.Add(data[index++]);
                    }
                    ret.frameCheckSequence = (data.Length - index) >= frameCheckSequenceBytesCount ? new byte[frameCheckSequenceBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.frameCheckSequence[0] = data[index++];
                    if (data.Length > index) ret.frameCheckSequence[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](COMMAND_FIRST_SEGMENT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_TRANSPORT_SERVICE_V2.ID);
                ret.Add((byte)((ID & ID_MASK) + command.properties1));
                if (command.datagramSize2.HasValue) ret.Add(command.datagramSize2);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.properties2.ext > 0)
                {
                    if (command.headerExtensionLength.HasValue) ret.Add(command.headerExtensionLength);
                }
                if (command.properties2.ext > 0)
                {
                    if (command.headerExtension != null)
                    {
                        foreach (var tmp in command.headerExtension)
                        {
                            ret.Add(tmp);
                        }
                    }
                }
                if (command.payload != null)
                {
                    foreach (var tmp in command.payload)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.frameCheckSequence != null)
                {
                    foreach (var tmp in command.frameCheckSequence)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class COMMAND_SEGMENT_COMPLETE
        {
            public const byte ID = 0xE8;
            public const byte ID_MASK = 0xF8;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte reserved
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
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte reserved2
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte sessionId
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
            public static implicit operator COMMAND_SEGMENT_COMPLETE(byte[] data)
            {
                COMMAND_SEGMENT_COMPLETE ret = new COMMAND_SEGMENT_COMPLETE();
                if (data != null)
                {
                    int index = 1;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](COMMAND_SEGMENT_COMPLETE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_TRANSPORT_SERVICE_V2.ID);
                ret.Add((byte)((ID & ID_MASK) + command.properties1));
                if (command.properties2.HasValue) ret.Add(command.properties2);
                return ret.ToArray();
            }
        }
        public partial class COMMAND_SEGMENT_REQUEST
        {
            public const byte ID = 0xC8;
            public const byte ID_MASK = 0xF8;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte reserved
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
                public byte reserved2
                {
                    get { return (byte)(_value >> 3 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x08; _value += (byte)(value << 3 & 0x08); }
                }
                public byte sessionId
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
            public ByteValue datagramOffset2 = 0;
            public static implicit operator COMMAND_SEGMENT_REQUEST(byte[] data)
            {
                COMMAND_SEGMENT_REQUEST ret = new COMMAND_SEGMENT_REQUEST();
                if (data != null)
                {
                    int index = 1;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.datagramOffset2 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](COMMAND_SEGMENT_REQUEST command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_TRANSPORT_SERVICE_V2.ID);
                ret.Add((byte)((ID & ID_MASK) + command.properties1));
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.datagramOffset2.HasValue) ret.Add(command.datagramOffset2);
                return ret.ToArray();
            }
        }
        public partial class COMMAND_SEGMENT_WAIT
        {
            public const byte ID = 0xF0;
            public const byte ID_MASK = 0xF8;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte reserved
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
            public ByteValue pendingFragments = 0;
            public static implicit operator COMMAND_SEGMENT_WAIT(byte[] data)
            {
                COMMAND_SEGMENT_WAIT ret = new COMMAND_SEGMENT_WAIT();
                if (data != null)
                {
                    int index = 1;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.pendingFragments = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](COMMAND_SEGMENT_WAIT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_TRANSPORT_SERVICE_V2.ID);
                ret.Add((byte)((ID & ID_MASK) + command.properties1));
                if (command.pendingFragments.HasValue) ret.Add(command.pendingFragments);
                return ret.ToArray();
            }
        }
        public partial class COMMAND_SUBSEQUENT_SEGMENT
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
                public byte ext
                {
                    get { return (byte)(_value >> 3 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x08; _value += (byte)(value << 3 & 0x08); }
                }
                public byte sessionId
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
            public ByteValue datagramOffset2 = 0;
            public ByteValue headerExtensionLength = 0;
            public IList<byte> headerExtension = new List<byte>();
            public IList<byte> payload = new List<byte>();
            public const byte frameCheckSequenceBytesCount = 2;
            public byte[] frameCheckSequence = new byte[frameCheckSequenceBytesCount];
            public static implicit operator COMMAND_SUBSEQUENT_SEGMENT(byte[] data)
            {
                COMMAND_SUBSEQUENT_SEGMENT ret = new COMMAND_SUBSEQUENT_SEGMENT();
                if (data != null)
                {
                    int index = 1;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.datagramSize2 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.datagramOffset2 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    if (ret.properties2.ext > 0)
                    {
                        ret.headerExtensionLength = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    }
                    if (ret.properties2.ext > 0)
                    {
                        ret.headerExtension = new List<byte>();
                        for (int i = 0; i < ret.headerExtensionLength; i++)
                        {
                            if (data.Length > index) ret.headerExtension.Add(data[index++]);
                        }
                    }
                    ret.payload = new List<byte>();
                    while (data.Length - 2 > index)
                    {
                        if (data.Length > index) ret.payload.Add(data[index++]);
                    }
                    ret.frameCheckSequence = (data.Length - index) >= frameCheckSequenceBytesCount ? new byte[frameCheckSequenceBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.frameCheckSequence[0] = data[index++];
                    if (data.Length > index) ret.frameCheckSequence[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](COMMAND_SUBSEQUENT_SEGMENT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_TRANSPORT_SERVICE_V2.ID);
                ret.Add((byte)((ID & ID_MASK) + command.properties1));
                if (command.datagramSize2.HasValue) ret.Add(command.datagramSize2);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.datagramOffset2.HasValue) ret.Add(command.datagramOffset2);
                if (command.properties2.ext > 0)
                {
                    if (command.headerExtensionLength.HasValue) ret.Add(command.headerExtensionLength);
                }
                if (command.properties2.ext > 0)
                {
                    if (command.headerExtension != null)
                    {
                        foreach (var tmp in command.headerExtension)
                        {
                            ret.Add(tmp);
                        }
                    }
                }
                if (command.payload != null)
                {
                    foreach (var tmp in command.payload)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.frameCheckSequence != null)
                {
                    foreach (var tmp in command.frameCheckSequence)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

