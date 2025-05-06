using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_CLIMATE_CONTROL_SCHEDULE
    {
        public const byte ID = 0x46;
        public const byte VERSION = 1;
        public partial class SCHEDULE_CHANGED_GET
        {
            public const byte ID = 0x04;
            public static implicit operator SCHEDULE_CHANGED_GET(byte[] data)
            {
                SCHEDULE_CHANGED_GET ret = new SCHEDULE_CHANGED_GET();
                return ret;
            }
            public static implicit operator byte[](SCHEDULE_CHANGED_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_CLIMATE_CONTROL_SCHEDULE.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class SCHEDULE_CHANGED_REPORT
        {
            public const byte ID = 0x05;
            public ByteValue changecounter = 0;
            public static implicit operator SCHEDULE_CHANGED_REPORT(byte[] data)
            {
                SCHEDULE_CHANGED_REPORT ret = new SCHEDULE_CHANGED_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.changecounter = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SCHEDULE_CHANGED_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_CLIMATE_CONTROL_SCHEDULE.ID);
                ret.Add(ID);
                if (command.changecounter.HasValue) ret.Add(command.changecounter);
                return ret.ToArray();
            }
        }
        public partial class SCHEDULE_GET
        {
            public const byte ID = 0x02;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte weekday
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 3 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0xF8; _value += (byte)(value << 3 & 0xF8); }
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
            public static implicit operator SCHEDULE_GET(byte[] data)
            {
                SCHEDULE_GET ret = new SCHEDULE_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SCHEDULE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_CLIMATE_CONTROL_SCHEDULE.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                return ret.ToArray();
            }
        }
        public partial class SCHEDULE_OVERRIDE_GET
        {
            public const byte ID = 0x07;
            public static implicit operator SCHEDULE_OVERRIDE_GET(byte[] data)
            {
                SCHEDULE_OVERRIDE_GET ret = new SCHEDULE_OVERRIDE_GET();
                return ret;
            }
            public static implicit operator byte[](SCHEDULE_OVERRIDE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_CLIMATE_CONTROL_SCHEDULE.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class SCHEDULE_OVERRIDE_REPORT
        {
            public const byte ID = 0x08;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte overrideType
                {
                    get { return (byte)(_value >> 0 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x03; _value += (byte)(value << 0 & 0x03); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 2 & 0x3F); }
                    set { HasValue = true; _value &= 0xFF - 0xFC; _value += (byte)(value << 2 & 0xFC); }
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
            public ByteValue overrideState = 0;
            public static implicit operator SCHEDULE_OVERRIDE_REPORT(byte[] data)
            {
                SCHEDULE_OVERRIDE_REPORT ret = new SCHEDULE_OVERRIDE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.overrideState = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SCHEDULE_OVERRIDE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_CLIMATE_CONTROL_SCHEDULE.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.overrideState.HasValue) ret.Add(command.overrideState);
                return ret.ToArray();
            }
        }
        public partial class SCHEDULE_OVERRIDE_SET
        {
            public const byte ID = 0x06;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte overrideType
                {
                    get { return (byte)(_value >> 0 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x03; _value += (byte)(value << 0 & 0x03); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 2 & 0x3F); }
                    set { HasValue = true; _value &= 0xFF - 0xFC; _value += (byte)(value << 2 & 0xFC); }
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
            public ByteValue overrideState = 0;
            public static implicit operator SCHEDULE_OVERRIDE_SET(byte[] data)
            {
                SCHEDULE_OVERRIDE_SET ret = new SCHEDULE_OVERRIDE_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.overrideState = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SCHEDULE_OVERRIDE_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_CLIMATE_CONTROL_SCHEDULE.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.overrideState.HasValue) ret.Add(command.overrideState);
                return ret.ToArray();
            }
        }
        public partial class SCHEDULE_REPORT
        {
            public const byte ID = 0x03;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte weekday
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 3 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0xF8; _value += (byte)(value << 3 & 0xF8); }
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
            public const byte switchpoint0BytesCount = 3;
            public byte[] switchpoint0 = new byte[switchpoint0BytesCount];
            public const byte switchpoint1BytesCount = 3;
            public byte[] switchpoint1 = new byte[switchpoint1BytesCount];
            public const byte switchpoint2BytesCount = 3;
            public byte[] switchpoint2 = new byte[switchpoint2BytesCount];
            public const byte switchpoint3BytesCount = 3;
            public byte[] switchpoint3 = new byte[switchpoint3BytesCount];
            public const byte switchpoint4BytesCount = 3;
            public byte[] switchpoint4 = new byte[switchpoint4BytesCount];
            public const byte switchpoint5BytesCount = 3;
            public byte[] switchpoint5 = new byte[switchpoint5BytesCount];
            public const byte switchpoint6BytesCount = 3;
            public byte[] switchpoint6 = new byte[switchpoint6BytesCount];
            public const byte switchpoint7BytesCount = 3;
            public byte[] switchpoint7 = new byte[switchpoint7BytesCount];
            public const byte switchpoint8BytesCount = 3;
            public byte[] switchpoint8 = new byte[switchpoint8BytesCount];
            public static implicit operator SCHEDULE_REPORT(byte[] data)
            {
                SCHEDULE_REPORT ret = new SCHEDULE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.switchpoint0 = (data.Length - index) >= switchpoint0BytesCount ? new byte[switchpoint0BytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.switchpoint0[0] = data[index++];
                    if (data.Length > index) ret.switchpoint0[1] = data[index++];
                    if (data.Length > index) ret.switchpoint0[2] = data[index++];
                    ret.switchpoint1 = (data.Length - index) >= switchpoint1BytesCount ? new byte[switchpoint1BytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.switchpoint1[0] = data[index++];
                    if (data.Length > index) ret.switchpoint1[1] = data[index++];
                    if (data.Length > index) ret.switchpoint1[2] = data[index++];
                    ret.switchpoint2 = (data.Length - index) >= switchpoint2BytesCount ? new byte[switchpoint2BytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.switchpoint2[0] = data[index++];
                    if (data.Length > index) ret.switchpoint2[1] = data[index++];
                    if (data.Length > index) ret.switchpoint2[2] = data[index++];
                    ret.switchpoint3 = (data.Length - index) >= switchpoint3BytesCount ? new byte[switchpoint3BytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.switchpoint3[0] = data[index++];
                    if (data.Length > index) ret.switchpoint3[1] = data[index++];
                    if (data.Length > index) ret.switchpoint3[2] = data[index++];
                    ret.switchpoint4 = (data.Length - index) >= switchpoint4BytesCount ? new byte[switchpoint4BytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.switchpoint4[0] = data[index++];
                    if (data.Length > index) ret.switchpoint4[1] = data[index++];
                    if (data.Length > index) ret.switchpoint4[2] = data[index++];
                    ret.switchpoint5 = (data.Length - index) >= switchpoint5BytesCount ? new byte[switchpoint5BytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.switchpoint5[0] = data[index++];
                    if (data.Length > index) ret.switchpoint5[1] = data[index++];
                    if (data.Length > index) ret.switchpoint5[2] = data[index++];
                    ret.switchpoint6 = (data.Length - index) >= switchpoint6BytesCount ? new byte[switchpoint6BytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.switchpoint6[0] = data[index++];
                    if (data.Length > index) ret.switchpoint6[1] = data[index++];
                    if (data.Length > index) ret.switchpoint6[2] = data[index++];
                    ret.switchpoint7 = (data.Length - index) >= switchpoint7BytesCount ? new byte[switchpoint7BytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.switchpoint7[0] = data[index++];
                    if (data.Length > index) ret.switchpoint7[1] = data[index++];
                    if (data.Length > index) ret.switchpoint7[2] = data[index++];
                    ret.switchpoint8 = (data.Length - index) >= switchpoint8BytesCount ? new byte[switchpoint8BytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.switchpoint8[0] = data[index++];
                    if (data.Length > index) ret.switchpoint8[1] = data[index++];
                    if (data.Length > index) ret.switchpoint8[2] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](SCHEDULE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_CLIMATE_CONTROL_SCHEDULE.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.switchpoint0 != null)
                {
                    foreach (var tmp in command.switchpoint0)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.switchpoint1 != null)
                {
                    foreach (var tmp in command.switchpoint1)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.switchpoint2 != null)
                {
                    foreach (var tmp in command.switchpoint2)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.switchpoint3 != null)
                {
                    foreach (var tmp in command.switchpoint3)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.switchpoint4 != null)
                {
                    foreach (var tmp in command.switchpoint4)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.switchpoint5 != null)
                {
                    foreach (var tmp in command.switchpoint5)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.switchpoint6 != null)
                {
                    foreach (var tmp in command.switchpoint6)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.switchpoint7 != null)
                {
                    foreach (var tmp in command.switchpoint7)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.switchpoint8 != null)
                {
                    foreach (var tmp in command.switchpoint8)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class SCHEDULE_SET
        {
            public const byte ID = 0x01;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte weekday
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 3 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0xF8; _value += (byte)(value << 3 & 0xF8); }
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
            public const byte switchpoint0BytesCount = 3;
            public byte[] switchpoint0 = new byte[switchpoint0BytesCount];
            public const byte switchpoint1BytesCount = 3;
            public byte[] switchpoint1 = new byte[switchpoint1BytesCount];
            public const byte switchpoint2BytesCount = 3;
            public byte[] switchpoint2 = new byte[switchpoint2BytesCount];
            public const byte switchpoint3BytesCount = 3;
            public byte[] switchpoint3 = new byte[switchpoint3BytesCount];
            public const byte switchpoint4BytesCount = 3;
            public byte[] switchpoint4 = new byte[switchpoint4BytesCount];
            public const byte switchpoint5BytesCount = 3;
            public byte[] switchpoint5 = new byte[switchpoint5BytesCount];
            public const byte switchpoint6BytesCount = 3;
            public byte[] switchpoint6 = new byte[switchpoint6BytesCount];
            public const byte switchpoint7BytesCount = 3;
            public byte[] switchpoint7 = new byte[switchpoint7BytesCount];
            public const byte switchpoint8BytesCount = 3;
            public byte[] switchpoint8 = new byte[switchpoint8BytesCount];
            public static implicit operator SCHEDULE_SET(byte[] data)
            {
                SCHEDULE_SET ret = new SCHEDULE_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.switchpoint0 = (data.Length - index) >= switchpoint0BytesCount ? new byte[switchpoint0BytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.switchpoint0[0] = data[index++];
                    if (data.Length > index) ret.switchpoint0[1] = data[index++];
                    if (data.Length > index) ret.switchpoint0[2] = data[index++];
                    ret.switchpoint1 = (data.Length - index) >= switchpoint1BytesCount ? new byte[switchpoint1BytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.switchpoint1[0] = data[index++];
                    if (data.Length > index) ret.switchpoint1[1] = data[index++];
                    if (data.Length > index) ret.switchpoint1[2] = data[index++];
                    ret.switchpoint2 = (data.Length - index) >= switchpoint2BytesCount ? new byte[switchpoint2BytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.switchpoint2[0] = data[index++];
                    if (data.Length > index) ret.switchpoint2[1] = data[index++];
                    if (data.Length > index) ret.switchpoint2[2] = data[index++];
                    ret.switchpoint3 = (data.Length - index) >= switchpoint3BytesCount ? new byte[switchpoint3BytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.switchpoint3[0] = data[index++];
                    if (data.Length > index) ret.switchpoint3[1] = data[index++];
                    if (data.Length > index) ret.switchpoint3[2] = data[index++];
                    ret.switchpoint4 = (data.Length - index) >= switchpoint4BytesCount ? new byte[switchpoint4BytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.switchpoint4[0] = data[index++];
                    if (data.Length > index) ret.switchpoint4[1] = data[index++];
                    if (data.Length > index) ret.switchpoint4[2] = data[index++];
                    ret.switchpoint5 = (data.Length - index) >= switchpoint5BytesCount ? new byte[switchpoint5BytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.switchpoint5[0] = data[index++];
                    if (data.Length > index) ret.switchpoint5[1] = data[index++];
                    if (data.Length > index) ret.switchpoint5[2] = data[index++];
                    ret.switchpoint6 = (data.Length - index) >= switchpoint6BytesCount ? new byte[switchpoint6BytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.switchpoint6[0] = data[index++];
                    if (data.Length > index) ret.switchpoint6[1] = data[index++];
                    if (data.Length > index) ret.switchpoint6[2] = data[index++];
                    ret.switchpoint7 = (data.Length - index) >= switchpoint7BytesCount ? new byte[switchpoint7BytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.switchpoint7[0] = data[index++];
                    if (data.Length > index) ret.switchpoint7[1] = data[index++];
                    if (data.Length > index) ret.switchpoint7[2] = data[index++];
                    ret.switchpoint8 = (data.Length - index) >= switchpoint8BytesCount ? new byte[switchpoint8BytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.switchpoint8[0] = data[index++];
                    if (data.Length > index) ret.switchpoint8[1] = data[index++];
                    if (data.Length > index) ret.switchpoint8[2] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](SCHEDULE_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_CLIMATE_CONTROL_SCHEDULE.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.switchpoint0 != null)
                {
                    foreach (var tmp in command.switchpoint0)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.switchpoint1 != null)
                {
                    foreach (var tmp in command.switchpoint1)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.switchpoint2 != null)
                {
                    foreach (var tmp in command.switchpoint2)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.switchpoint3 != null)
                {
                    foreach (var tmp in command.switchpoint3)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.switchpoint4 != null)
                {
                    foreach (var tmp in command.switchpoint4)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.switchpoint5 != null)
                {
                    foreach (var tmp in command.switchpoint5)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.switchpoint6 != null)
                {
                    foreach (var tmp in command.switchpoint6)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.switchpoint7 != null)
                {
                    foreach (var tmp in command.switchpoint7)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.switchpoint8 != null)
                {
                    foreach (var tmp in command.switchpoint8)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

