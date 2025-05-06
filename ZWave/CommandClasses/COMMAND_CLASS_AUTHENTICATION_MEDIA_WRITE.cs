using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_AUTHENTICATION_MEDIA_WRITE
    {
        public const byte ID = 0xA2;
        public const byte VERSION = 1;
        public partial class AUTHENTICATION_MEDIA_CAPABILITY_GET
        {
            public const byte ID = 0x01;
            public static implicit operator AUTHENTICATION_MEDIA_CAPABILITY_GET(byte[] data)
            {
                AUTHENTICATION_MEDIA_CAPABILITY_GET ret = new AUTHENTICATION_MEDIA_CAPABILITY_GET();
                return ret;
            }
            public static implicit operator byte[](AUTHENTICATION_MEDIA_CAPABILITY_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_AUTHENTICATION_MEDIA_WRITE.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class AUTHENTICATION_MEDIA_CAPABILITY_REPORT
        {
            public const byte ID = 0x02;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte dgs
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte reserved1
                {
                    get { return (byte)(_value >> 1 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0xFE; _value += (byte)(value << 1 & 0xFE); }
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
            public static implicit operator AUTHENTICATION_MEDIA_CAPABILITY_REPORT(byte[] data)
            {
                AUTHENTICATION_MEDIA_CAPABILITY_REPORT ret = new AUTHENTICATION_MEDIA_CAPABILITY_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](AUTHENTICATION_MEDIA_CAPABILITY_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_AUTHENTICATION_MEDIA_WRITE.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                return ret.ToArray();
            }
        }
        public partial class AUTHENTICATION_MEDIA_WRITE_START
        {
            public const byte ID = 0x03;
            public ByteValue sequenceNumber = 0;
            public ByteValue timeout = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte reserved1
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte generateData
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
            public ByteValue dataLength = 0;
            public IList<byte> data = new List<byte>();
            public static implicit operator AUTHENTICATION_MEDIA_WRITE_START(byte[] data)
            {
                AUTHENTICATION_MEDIA_WRITE_START ret = new AUTHENTICATION_MEDIA_WRITE_START();
                if (data != null)
                {
                    int index = 2;
                    ret.sequenceNumber = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.timeout = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.dataLength = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.data = new List<byte>();
                    for (int i = 0; i < ret.dataLength; i++)
                    {
                        if (data.Length > index) ret.data.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](AUTHENTICATION_MEDIA_WRITE_START command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_AUTHENTICATION_MEDIA_WRITE.ID);
                ret.Add(ID);
                if (command.sequenceNumber.HasValue) ret.Add(command.sequenceNumber);
                if (command.timeout.HasValue) ret.Add(command.timeout);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.dataLength.HasValue) ret.Add(command.dataLength);
                if (command.data != null)
                {
                    foreach (var tmp in command.data)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class AUTHENTICATION_MEDIA_WRITE_STOP
        {
            public const byte ID = 0x04;
            public static implicit operator AUTHENTICATION_MEDIA_WRITE_STOP(byte[] data)
            {
                AUTHENTICATION_MEDIA_WRITE_STOP ret = new AUTHENTICATION_MEDIA_WRITE_STOP();
                return ret;
            }
            public static implicit operator byte[](AUTHENTICATION_MEDIA_WRITE_STOP command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_AUTHENTICATION_MEDIA_WRITE.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class AUTHENTICATION_MEDIA_WRITE_STATUS
        {
            public const byte ID = 0x05;
            public ByteValue sequenceNumber = 0;
            public ByteValue status = 0;
            public ByteValue dataLength = 0;
            public IList<byte> data = new List<byte>();
            public static implicit operator AUTHENTICATION_MEDIA_WRITE_STATUS(byte[] data)
            {
                AUTHENTICATION_MEDIA_WRITE_STATUS ret = new AUTHENTICATION_MEDIA_WRITE_STATUS();
                if (data != null)
                {
                    int index = 2;
                    ret.sequenceNumber = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.status = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.dataLength = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.data = new List<byte>();
                    for (int i = 0; i < ret.dataLength; i++)
                    {
                        if (data.Length > index) ret.data.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](AUTHENTICATION_MEDIA_WRITE_STATUS command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_AUTHENTICATION_MEDIA_WRITE.ID);
                ret.Add(ID);
                if (command.sequenceNumber.HasValue) ret.Add(command.sequenceNumber);
                if (command.status.HasValue) ret.Add(command.status);
                if (command.dataLength.HasValue) ret.Add(command.dataLength);
                if (command.data != null)
                {
                    foreach (var tmp in command.data)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

