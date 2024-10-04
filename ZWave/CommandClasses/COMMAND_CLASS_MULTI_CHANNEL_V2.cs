/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_MULTI_CHANNEL_V2
    {
        public const byte ID = 0x60;
        public const byte VERSION = 2;
        public partial class MULTI_CHANNEL_CAPABILITY_GET
        {
            public const byte ID = 0x09;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte endPoint
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte res
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
            public static implicit operator MULTI_CHANNEL_CAPABILITY_GET(byte[] data)
            {
                MULTI_CHANNEL_CAPABILITY_GET ret = new MULTI_CHANNEL_CAPABILITY_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](MULTI_CHANNEL_CAPABILITY_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_MULTI_CHANNEL_V2.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                return ret.ToArray();
            }
        }
        public partial class MULTI_CHANNEL_CAPABILITY_REPORT
        {
            public const byte ID = 0x0A;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte endPoint
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte dynamic
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
            public ByteValue genericDeviceClass = 0;
            public ByteValue specificDeviceClass = 0;
            public IList<byte> commandClass = new List<byte>();
            public static implicit operator MULTI_CHANNEL_CAPABILITY_REPORT(byte[] data)
            {
                MULTI_CHANNEL_CAPABILITY_REPORT ret = new MULTI_CHANNEL_CAPABILITY_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.genericDeviceClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.specificDeviceClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.commandClass = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.commandClass.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](MULTI_CHANNEL_CAPABILITY_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_MULTI_CHANNEL_V2.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.genericDeviceClass.HasValue) ret.Add(command.genericDeviceClass);
                if (command.specificDeviceClass.HasValue) ret.Add(command.specificDeviceClass);
                if (command.commandClass != null)
                {
                    foreach (var tmp in command.commandClass)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class MULTI_CHANNEL_CMD_ENCAP
        {
            public const byte ID = 0x0D;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte sourceEndPoint
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte res
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
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte destinationEndPoint
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte bitAddress
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
            public ByteValue commandClass = 0;
            public ByteValue command = 0;
            public IList<byte> parameter = new List<byte>();
            public static implicit operator MULTI_CHANNEL_CMD_ENCAP(byte[] data)
            {
                MULTI_CHANNEL_CMD_ENCAP ret = new MULTI_CHANNEL_CMD_ENCAP();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.commandClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.command = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.parameter = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.parameter.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](MULTI_CHANNEL_CMD_ENCAP command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_MULTI_CHANNEL_V2.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.commandClass.HasValue) ret.Add(command.commandClass);
                if (command.command.HasValue) ret.Add(command.command);
                if (command.parameter != null)
                {
                    foreach (var tmp in command.parameter)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class MULTI_CHANNEL_END_POINT_FIND
        {
            public const byte ID = 0x0B;
            public ByteValue genericDeviceClass = 0;
            public ByteValue specificDeviceClass = 0;
            public static implicit operator MULTI_CHANNEL_END_POINT_FIND(byte[] data)
            {
                MULTI_CHANNEL_END_POINT_FIND ret = new MULTI_CHANNEL_END_POINT_FIND();
                if (data != null)
                {
                    int index = 2;
                    ret.genericDeviceClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.specificDeviceClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](MULTI_CHANNEL_END_POINT_FIND command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_MULTI_CHANNEL_V2.ID);
                ret.Add(ID);
                if (command.genericDeviceClass.HasValue) ret.Add(command.genericDeviceClass);
                if (command.specificDeviceClass.HasValue) ret.Add(command.specificDeviceClass);
                return ret.ToArray();
            }
        }
        public partial class MULTI_CHANNEL_END_POINT_FIND_REPORT
        {
            public const byte ID = 0x0C;
            public ByteValue reportsToFollow = 0;
            public ByteValue genericDeviceClass = 0;
            public ByteValue specificDeviceClass = 0;
            public class TVG
            {
                public struct Tproperties1
                {
                    private byte _value;
                    public bool HasValue { get; private set; }
                    public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                    public byte endPoint
                    {
                        get { return (byte)(_value >> 0 & 0x7F); }
                        set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                    }
                    public byte res
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
            }
            public List<TVG> vg = new List<TVG>();
            public static implicit operator MULTI_CHANNEL_END_POINT_FIND_REPORT(byte[] data)
            {
                MULTI_CHANNEL_END_POINT_FIND_REPORT ret = new MULTI_CHANNEL_END_POINT_FIND_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.reportsToFollow = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.genericDeviceClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.specificDeviceClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.vg = new List<TVG>();
                    while (data.Length - 0 > index)
                    {
                        TVG tmp = new TVG();
                        tmp.properties1 = data.Length > index ? (TVG.Tproperties1)data[index++] : TVG.Tproperties1.Empty;
                        ret.vg.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](MULTI_CHANNEL_END_POINT_FIND_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_MULTI_CHANNEL_V2.ID);
                ret.Add(ID);
                if (command.reportsToFollow.HasValue) ret.Add(command.reportsToFollow);
                if (command.genericDeviceClass.HasValue) ret.Add(command.genericDeviceClass);
                if (command.specificDeviceClass.HasValue) ret.Add(command.specificDeviceClass);
                if (command.vg != null)
                {
                    foreach (var item in command.vg)
                    {
                        if (item.properties1.HasValue) ret.Add(item.properties1);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class MULTI_CHANNEL_END_POINT_GET
        {
            public const byte ID = 0x07;
            public static implicit operator MULTI_CHANNEL_END_POINT_GET(byte[] data)
            {
                MULTI_CHANNEL_END_POINT_GET ret = new MULTI_CHANNEL_END_POINT_GET();
                return ret;
            }
            public static implicit operator byte[](MULTI_CHANNEL_END_POINT_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_MULTI_CHANNEL_V2.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class MULTI_CHANNEL_END_POINT_REPORT
        {
            public const byte ID = 0x08;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte res1
                {
                    get { return (byte)(_value >> 0 & 0x3F); }
                    set { HasValue = true; _value &= 0xFF - 0x3F; _value += (byte)(value << 0 & 0x3F); }
                }
                public byte identical
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte dynamic
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
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte endPoints
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte res2
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
            public static implicit operator MULTI_CHANNEL_END_POINT_REPORT(byte[] data)
            {
                MULTI_CHANNEL_END_POINT_REPORT ret = new MULTI_CHANNEL_END_POINT_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](MULTI_CHANNEL_END_POINT_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_MULTI_CHANNEL_V2.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                return ret.ToArray();
            }
        }
        public partial class MULTI_INSTANCE_CMD_ENCAP
        {
            public const byte ID = 0x06;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte instance
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte res
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
            public ByteValue commandClass = 0;
            public ByteValue command = 0;
            public IList<byte> parameter = new List<byte>();
            public static implicit operator MULTI_INSTANCE_CMD_ENCAP(byte[] data)
            {
                MULTI_INSTANCE_CMD_ENCAP ret = new MULTI_INSTANCE_CMD_ENCAP();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.commandClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.command = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.parameter = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.parameter.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](MULTI_INSTANCE_CMD_ENCAP command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_MULTI_CHANNEL_V2.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.commandClass.HasValue) ret.Add(command.commandClass);
                if (command.command.HasValue) ret.Add(command.command);
                if (command.parameter != null)
                {
                    foreach (var tmp in command.parameter)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class MULTI_INSTANCE_GET
        {
            public const byte ID = 0x04;
            public ByteValue commandClass = 0;
            public static implicit operator MULTI_INSTANCE_GET(byte[] data)
            {
                MULTI_INSTANCE_GET ret = new MULTI_INSTANCE_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.commandClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](MULTI_INSTANCE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_MULTI_CHANNEL_V2.ID);
                ret.Add(ID);
                if (command.commandClass.HasValue) ret.Add(command.commandClass);
                return ret.ToArray();
            }
        }
        public partial class MULTI_INSTANCE_REPORT
        {
            public const byte ID = 0x05;
            public ByteValue commandClass = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte instances
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte res
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
            public static implicit operator MULTI_INSTANCE_REPORT(byte[] data)
            {
                MULTI_INSTANCE_REPORT ret = new MULTI_INSTANCE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.commandClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](MULTI_INSTANCE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_MULTI_CHANNEL_V2.ID);
                ret.Add(ID);
                if (command.commandClass.HasValue) ret.Add(command.commandClass);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                return ret.ToArray();
            }
        }
    }
}

