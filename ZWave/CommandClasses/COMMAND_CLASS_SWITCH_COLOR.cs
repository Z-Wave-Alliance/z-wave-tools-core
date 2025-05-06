using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_SWITCH_COLOR
    {
        public const byte ID = 0x33;
        public const byte VERSION = 1;
        public partial class SWITCH_COLOR_SUPPORTED_GET
        {
            public const byte ID = 0x01;
            public static implicit operator SWITCH_COLOR_SUPPORTED_GET(byte[] data)
            {
                SWITCH_COLOR_SUPPORTED_GET ret = new SWITCH_COLOR_SUPPORTED_GET();
                return ret;
            }
            public static implicit operator byte[](SWITCH_COLOR_SUPPORTED_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SWITCH_COLOR.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class SWITCH_COLOR_SUPPORTED_REPORT
        {
            public const byte ID = 0x02;
            public const byte colorComponentMaskBytesCount = 2;
            public byte[] colorComponentMask = new byte[colorComponentMaskBytesCount];
            public static implicit operator SWITCH_COLOR_SUPPORTED_REPORT(byte[] data)
            {
                SWITCH_COLOR_SUPPORTED_REPORT ret = new SWITCH_COLOR_SUPPORTED_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.colorComponentMask = (data.Length - index) >= colorComponentMaskBytesCount ? new byte[colorComponentMaskBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.colorComponentMask[0] = data[index++];
                    if (data.Length > index) ret.colorComponentMask[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](SWITCH_COLOR_SUPPORTED_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SWITCH_COLOR.ID);
                ret.Add(ID);
                if (command.colorComponentMask != null)
                {
                    foreach (var tmp in command.colorComponentMask)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class SWITCH_COLOR_GET
        {
            public const byte ID = 0x03;
            public ByteValue colorComponentId = 0;
            public static implicit operator SWITCH_COLOR_GET(byte[] data)
            {
                SWITCH_COLOR_GET ret = new SWITCH_COLOR_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.colorComponentId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SWITCH_COLOR_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SWITCH_COLOR.ID);
                ret.Add(ID);
                if (command.colorComponentId.HasValue) ret.Add(command.colorComponentId);
                return ret.ToArray();
            }
        }
        public partial class SWITCH_COLOR_REPORT
        {
            public const byte ID = 0x04;
            public ByteValue colorComponentId = 0;
            public ByteValue value = 0;
            public static implicit operator SWITCH_COLOR_REPORT(byte[] data)
            {
                SWITCH_COLOR_REPORT ret = new SWITCH_COLOR_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.colorComponentId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.value = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SWITCH_COLOR_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SWITCH_COLOR.ID);
                ret.Add(ID);
                if (command.colorComponentId.HasValue) ret.Add(command.colorComponentId);
                if (command.value.HasValue) ret.Add(command.value);
                return ret.ToArray();
            }
        }
        public partial class SWITCH_COLOR_SET
        {
            public const byte ID = 0x05;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte colorComponentCount
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte reserved
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
            public class TVG1
            {
                public ByteValue colorComponentId = 0;
                public ByteValue value = 0;
            }
            public List<TVG1> vg1 = new List<TVG1>();
            public static implicit operator SWITCH_COLOR_SET(byte[] data)
            {
                SWITCH_COLOR_SET ret = new SWITCH_COLOR_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.vg1 = new List<TVG1>();
                    for (int j = 0; j < ret.properties1.colorComponentCount; j++)
                    {
                        TVG1 tmp = new TVG1();
                        tmp.colorComponentId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.value = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        ret.vg1.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](SWITCH_COLOR_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SWITCH_COLOR.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.vg1 != null)
                {
                    foreach (var item in command.vg1)
                    {
                        if (item.colorComponentId.HasValue) ret.Add(item.colorComponentId);
                        if (item.value.HasValue) ret.Add(item.value);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class SWITCH_COLOR_START_LEVEL_CHANGE
        {
            public const byte ID = 0x06;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte res1
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte ignoreStartState
                {
                    get { return (byte)(_value >> 5 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x20; _value += (byte)(value << 5 & 0x20); }
                }
                public byte upDown
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte res2
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
            public ByteValue colorComponentId = 0;
            public ByteValue startLevel = 0;
            public static implicit operator SWITCH_COLOR_START_LEVEL_CHANGE(byte[] data)
            {
                SWITCH_COLOR_START_LEVEL_CHANGE ret = new SWITCH_COLOR_START_LEVEL_CHANGE();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.colorComponentId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startLevel = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SWITCH_COLOR_START_LEVEL_CHANGE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SWITCH_COLOR.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.colorComponentId.HasValue) ret.Add(command.colorComponentId);
                if (command.startLevel.HasValue) ret.Add(command.startLevel);
                return ret.ToArray();
            }
        }
        public partial class SWITCH_COLOR_STOP_LEVEL_CHANGE
        {
            public const byte ID = 0x07;
            public ByteValue colorComponentId = 0;
            public static implicit operator SWITCH_COLOR_STOP_LEVEL_CHANGE(byte[] data)
            {
                SWITCH_COLOR_STOP_LEVEL_CHANGE ret = new SWITCH_COLOR_STOP_LEVEL_CHANGE();
                if (data != null)
                {
                    int index = 2;
                    ret.colorComponentId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SWITCH_COLOR_STOP_LEVEL_CHANGE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SWITCH_COLOR.ID);
                ret.Add(ID);
                if (command.colorComponentId.HasValue) ret.Add(command.colorComponentId);
                return ret.ToArray();
            }
        }
    }
}

