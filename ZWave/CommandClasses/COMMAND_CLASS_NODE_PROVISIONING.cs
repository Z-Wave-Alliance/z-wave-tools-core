using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_NODE_PROVISIONING
    {
        public const byte ID = 0x78;
        public const byte VERSION = 1;
        public partial class NODE_PROVISIONING_SET
        {
            public const byte ID = 0x01;
            public ByteValue seqNo = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte dskLength
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte reserved1
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
            public IList<byte> dsk = new List<byte>();
            public class TVG1
            {
                public struct Tproperties1
                {
                    private byte _value;
                    public bool HasValue { get; private set; }
                    public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                    public byte critical
                    {
                        get { return (byte)(_value >> 0 & 0x01); }
                        set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                    }
                    public byte metaDataType
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
                public ByteValue length = 0;
                public IList<byte> value = new List<byte>();
            }
            public List<TVG1> vg1 = new List<TVG1>();
            public static implicit operator NODE_PROVISIONING_SET(byte[] data)
            {
                NODE_PROVISIONING_SET ret = new NODE_PROVISIONING_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.dsk = new List<byte>();
                    for (int i = 0; i < ret.properties1.dskLength; i++)
                    {
                        if (data.Length > index) ret.dsk.Add(data[index++]);
                    }
                    ret.vg1 = new List<TVG1>();
                    while (data.Length - 0 > index)
                    {
                        TVG1 tmp = new TVG1();
                        tmp.properties1 = data.Length > index ? (TVG1.Tproperties1)data[index++] : TVG1.Tproperties1.Empty;
                        tmp.length = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.value = new List<byte>();
                        for (int i = 0; i < tmp.length; i++)
                        {
                            if (data.Length > index) tmp.value.Add(data[index++]);
                        }
                        ret.vg1.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](NODE_PROVISIONING_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NODE_PROVISIONING.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.dsk != null)
                {
                    foreach (var tmp in command.dsk)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.vg1 != null)
                {
                    foreach (var item in command.vg1)
                    {
                        if (item.properties1.HasValue) ret.Add(item.properties1);
                        if (item.length.HasValue) ret.Add(item.length);
                        if (item.value != null)
                        {
                            foreach (var tmp in item.value)
                            {
                                ret.Add(tmp);
                            }
                        }
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class NODE_PROVISIONING_DELETE
        {
            public const byte ID = 0x02;
            public ByteValue seqNo = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte dskLength
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte reserved1
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
            public IList<byte> dsk = new List<byte>();
            public static implicit operator NODE_PROVISIONING_DELETE(byte[] data)
            {
                NODE_PROVISIONING_DELETE ret = new NODE_PROVISIONING_DELETE();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.dsk = new List<byte>();
                    for (int i = 0; i < ret.properties1.dskLength; i++)
                    {
                        if (data.Length > index) ret.dsk.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](NODE_PROVISIONING_DELETE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NODE_PROVISIONING.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.dsk != null)
                {
                    foreach (var tmp in command.dsk)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class NODE_PROVISIONING_LIST_ITERATION_GET
        {
            public const byte ID = 0x03;
            public ByteValue seqNo = 0;
            public ByteValue remainingCounter = 0;
            public static implicit operator NODE_PROVISIONING_LIST_ITERATION_GET(byte[] data)
            {
                NODE_PROVISIONING_LIST_ITERATION_GET ret = new NODE_PROVISIONING_LIST_ITERATION_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.remainingCounter = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](NODE_PROVISIONING_LIST_ITERATION_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NODE_PROVISIONING.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.remainingCounter.HasValue) ret.Add(command.remainingCounter);
                return ret.ToArray();
            }
        }
        public partial class NODE_PROVISIONING_LIST_ITERATION_REPORT
        {
            public const byte ID = 0x04;
            public ByteValue seqNo = 0;
            public ByteValue remainingCount = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte dskLength
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte reserved1
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
            public IList<byte> dsk = new List<byte>();
            public class TVG1
            {
                public struct Tproperties1
                {
                    private byte _value;
                    public bool HasValue { get; private set; }
                    public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                    public byte critical
                    {
                        get { return (byte)(_value >> 0 & 0x01); }
                        set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                    }
                    public byte metaDataType
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
                public ByteValue length = 0;
                public IList<byte> value = new List<byte>();
            }
            public List<TVG1> vg1 = new List<TVG1>();
            public static implicit operator NODE_PROVISIONING_LIST_ITERATION_REPORT(byte[] data)
            {
                NODE_PROVISIONING_LIST_ITERATION_REPORT ret = new NODE_PROVISIONING_LIST_ITERATION_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.remainingCount = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.dsk = new List<byte>();
                    for (int i = 0; i < ret.properties1.dskLength; i++)
                    {
                        if (data.Length > index) ret.dsk.Add(data[index++]);
                    }
                    ret.vg1 = new List<TVG1>();
                    while (data.Length - 0 > index)
                    {
                        TVG1 tmp = new TVG1();
                        tmp.properties1 = data.Length > index ? (TVG1.Tproperties1)data[index++] : TVG1.Tproperties1.Empty;
                        tmp.length = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.value = new List<byte>();
                        for (int i = 0; i < tmp.length; i++)
                        {
                            if (data.Length > index) tmp.value.Add(data[index++]);
                        }
                        ret.vg1.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](NODE_PROVISIONING_LIST_ITERATION_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NODE_PROVISIONING.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.remainingCount.HasValue) ret.Add(command.remainingCount);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.dsk != null)
                {
                    foreach (var tmp in command.dsk)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.vg1 != null)
                {
                    foreach (var item in command.vg1)
                    {
                        if (item.properties1.HasValue) ret.Add(item.properties1);
                        if (item.length.HasValue) ret.Add(item.length);
                        if (item.value != null)
                        {
                            foreach (var tmp in item.value)
                            {
                                ret.Add(tmp);
                            }
                        }
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class NODE_PROVISIONING_GET
        {
            public const byte ID = 0x05;
            public ByteValue seqNo = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte dskLength
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte reserved1
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
            public IList<byte> dsk = new List<byte>();
            public static implicit operator NODE_PROVISIONING_GET(byte[] data)
            {
                NODE_PROVISIONING_GET ret = new NODE_PROVISIONING_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.dsk = new List<byte>();
                    for (int i = 0; i < ret.properties1.dskLength; i++)
                    {
                        if (data.Length > index) ret.dsk.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](NODE_PROVISIONING_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NODE_PROVISIONING.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.dsk != null)
                {
                    foreach (var tmp in command.dsk)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class NODE_PROVISIONING_REPORT
        {
            public const byte ID = 0x06;
            public ByteValue seqNo = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte dskLength
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
            public IList<byte> dsk = new List<byte>();
            public class TVG1
            {
                public struct Tproperties1
                {
                    private byte _value;
                    public bool HasValue { get; private set; }
                    public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                    public byte critical
                    {
                        get { return (byte)(_value >> 0 & 0x01); }
                        set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                    }
                    public byte metaDataType
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
                public ByteValue length = 0;
                public IList<byte> value = new List<byte>();
            }
            public List<TVG1> vg1 = new List<TVG1>();
            public static implicit operator NODE_PROVISIONING_REPORT(byte[] data)
            {
                NODE_PROVISIONING_REPORT ret = new NODE_PROVISIONING_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.dsk = new List<byte>();
                    for (int i = 0; i < ret.properties1.dskLength; i++)
                    {
                        if (data.Length > index) ret.dsk.Add(data[index++]);
                    }
                    ret.vg1 = new List<TVG1>();
                    while (data.Length - 0 > index)
                    {
                        TVG1 tmp = new TVG1();
                        tmp.properties1 = data.Length > index ? (TVG1.Tproperties1)data[index++] : TVG1.Tproperties1.Empty;
                        tmp.length = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.value = new List<byte>();
                        for (int i = 0; i < tmp.length; i++)
                        {
                            if (data.Length > index) tmp.value.Add(data[index++]);
                        }
                        ret.vg1.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](NODE_PROVISIONING_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NODE_PROVISIONING.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.dsk != null)
                {
                    foreach (var tmp in command.dsk)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.vg1 != null)
                {
                    foreach (var item in command.vg1)
                    {
                        if (item.properties1.HasValue) ret.Add(item.properties1);
                        if (item.length.HasValue) ret.Add(item.length);
                        if (item.value != null)
                        {
                            foreach (var tmp in item.value)
                            {
                                ret.Add(tmp);
                            }
                        }
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

