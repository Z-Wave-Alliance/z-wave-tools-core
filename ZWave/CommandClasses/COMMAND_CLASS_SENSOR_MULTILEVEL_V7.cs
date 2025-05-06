using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_SENSOR_MULTILEVEL_V7
    {
        public const byte ID = 0x31;
        public const byte VERSION = 7;
        public partial class SENSOR_MULTILEVEL_GET
        {
            public const byte ID = 0x04;
            public ByteValue sensorType = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte reserved1
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte scale
                {
                    get { return (byte)(_value >> 3 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x18; _value += (byte)(value << 3 & 0x18); }
                }
                public byte reserved2
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
            public static implicit operator SENSOR_MULTILEVEL_GET(byte[] data)
            {
                SENSOR_MULTILEVEL_GET ret = new SENSOR_MULTILEVEL_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.sensorType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SENSOR_MULTILEVEL_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SENSOR_MULTILEVEL_V7.ID);
                ret.Add(ID);
                if (command.sensorType.HasValue) ret.Add(command.sensorType);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                return ret.ToArray();
            }
        }
        public partial class SENSOR_MULTILEVEL_REPORT
        {
            public const byte ID = 0x05;
            public ByteValue sensorType = 0;
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
            public IList<byte> sensorValue = new List<byte>();
            public static implicit operator SENSOR_MULTILEVEL_REPORT(byte[] data)
            {
                SENSOR_MULTILEVEL_REPORT ret = new SENSOR_MULTILEVEL_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.sensorType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.sensorValue = new List<byte>();
                    for (int i = 0; i < ret.properties1.size; i++)
                    {
                        if (data.Length > index) ret.sensorValue.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](SENSOR_MULTILEVEL_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SENSOR_MULTILEVEL_V7.ID);
                ret.Add(ID);
                if (command.sensorType.HasValue) ret.Add(command.sensorType);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.sensorValue != null)
                {
                    foreach (var tmp in command.sensorValue)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class SENSOR_MULTILEVEL_SUPPORTED_GET_SENSOR
        {
            public const byte ID = 0x01;
            public static implicit operator SENSOR_MULTILEVEL_SUPPORTED_GET_SENSOR(byte[] data)
            {
                SENSOR_MULTILEVEL_SUPPORTED_GET_SENSOR ret = new SENSOR_MULTILEVEL_SUPPORTED_GET_SENSOR();
                return ret;
            }
            public static implicit operator byte[](SENSOR_MULTILEVEL_SUPPORTED_GET_SENSOR command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SENSOR_MULTILEVEL_V7.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class SENSOR_MULTILEVEL_SUPPORTED_SENSOR_REPORT
        {
            public const byte ID = 0x02;
            public IList<byte> bitMask = new List<byte>();
            public static implicit operator SENSOR_MULTILEVEL_SUPPORTED_SENSOR_REPORT(byte[] data)
            {
                SENSOR_MULTILEVEL_SUPPORTED_SENSOR_REPORT ret = new SENSOR_MULTILEVEL_SUPPORTED_SENSOR_REPORT();
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
            public static implicit operator byte[](SENSOR_MULTILEVEL_SUPPORTED_SENSOR_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SENSOR_MULTILEVEL_V7.ID);
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
        public partial class SENSOR_MULTILEVEL_SUPPORTED_GET_SCALE
        {
            public const byte ID = 0x03;
            public ByteValue sensorType = 0;
            public static implicit operator SENSOR_MULTILEVEL_SUPPORTED_GET_SCALE(byte[] data)
            {
                SENSOR_MULTILEVEL_SUPPORTED_GET_SCALE ret = new SENSOR_MULTILEVEL_SUPPORTED_GET_SCALE();
                if (data != null)
                {
                    int index = 2;
                    ret.sensorType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SENSOR_MULTILEVEL_SUPPORTED_GET_SCALE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SENSOR_MULTILEVEL_V7.ID);
                ret.Add(ID);
                if (command.sensorType.HasValue) ret.Add(command.sensorType);
                return ret.ToArray();
            }
        }
        public partial class SENSOR_MULTILEVEL_SUPPORTED_SCALE_REPORT
        {
            public const byte ID = 0x06;
            public ByteValue sensorType = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte scaleBitMask
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
            public static implicit operator SENSOR_MULTILEVEL_SUPPORTED_SCALE_REPORT(byte[] data)
            {
                SENSOR_MULTILEVEL_SUPPORTED_SCALE_REPORT ret = new SENSOR_MULTILEVEL_SUPPORTED_SCALE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.sensorType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SENSOR_MULTILEVEL_SUPPORTED_SCALE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SENSOR_MULTILEVEL_V7.ID);
                ret.Add(ID);
                if (command.sensorType.HasValue) ret.Add(command.sensorType);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                return ret.ToArray();
            }
        }
    }
}

