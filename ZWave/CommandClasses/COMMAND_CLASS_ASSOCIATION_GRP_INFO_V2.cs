/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_ASSOCIATION_GRP_INFO_V2
    {
        public const byte ID = 0x59;
        public const byte VERSION = 2;
        public partial class ASSOCIATION_GROUP_NAME_GET
        {
            public const byte ID = 0x01;
            public ByteValue groupingIdentifier = 0;
            public static implicit operator ASSOCIATION_GROUP_NAME_GET(byte[] data)
            {
                ASSOCIATION_GROUP_NAME_GET ret = new ASSOCIATION_GROUP_NAME_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.groupingIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](ASSOCIATION_GROUP_NAME_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ASSOCIATION_GRP_INFO_V2.ID);
                ret.Add(ID);
                if (command.groupingIdentifier.HasValue) ret.Add(command.groupingIdentifier);
                return ret.ToArray();
            }
        }
        public partial class ASSOCIATION_GROUP_NAME_REPORT
        {
            public const byte ID = 0x02;
            public ByteValue groupingIdentifier = 0;
            public ByteValue lengthOfName = 0;
            public IList<byte> name = new List<byte>();
            public static implicit operator ASSOCIATION_GROUP_NAME_REPORT(byte[] data)
            {
                ASSOCIATION_GROUP_NAME_REPORT ret = new ASSOCIATION_GROUP_NAME_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.groupingIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.lengthOfName = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.name = new List<byte>();
                    for (int i = 0; i < ret.lengthOfName; i++)
                    {
                        if (data.Length > index) ret.name.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](ASSOCIATION_GROUP_NAME_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ASSOCIATION_GRP_INFO_V2.ID);
                ret.Add(ID);
                if (command.groupingIdentifier.HasValue) ret.Add(command.groupingIdentifier);
                if (command.lengthOfName.HasValue) ret.Add(command.lengthOfName);
                if (command.name != null)
                {
                    foreach (var tmp in command.name)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class ASSOCIATION_GROUP_INFO_GET
        {
            public const byte ID = 0x03;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte reserved
                {
                    get { return (byte)(_value >> 0 & 0x3F); }
                    set { HasValue = true; _value &= 0xFF - 0x3F; _value += (byte)(value << 0 & 0x3F); }
                }
                public byte listMode
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte refreshCache
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
            public ByteValue groupingIdentifier = 0;
            public static implicit operator ASSOCIATION_GROUP_INFO_GET(byte[] data)
            {
                ASSOCIATION_GROUP_INFO_GET ret = new ASSOCIATION_GROUP_INFO_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.groupingIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](ASSOCIATION_GROUP_INFO_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ASSOCIATION_GRP_INFO_V2.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.groupingIdentifier.HasValue) ret.Add(command.groupingIdentifier);
                return ret.ToArray();
            }
        }
        public partial class ASSOCIATION_GROUP_INFO_REPORT
        {
            public const byte ID = 0x04;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte groupCount
                {
                    get { return (byte)(_value >> 0 & 0x3F); }
                    set { HasValue = true; _value &= 0xFF - 0x3F; _value += (byte)(value << 0 & 0x3F); }
                }
                public byte dynamicInfo
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte listMode
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
            public class TVG1
            {
                public ByteValue groupingIdentifier = 0;
                public ByteValue mode = 0;
                public ByteValue profile1 = 0;
                public ByteValue profile2 = 0;
                public ByteValue reserved = 0;
                public const byte eventCodeBytesCount = 2;
                public byte[] eventCode = new byte[eventCodeBytesCount];
            }
            public List<TVG1> vg1 = new List<TVG1>();
            public static implicit operator ASSOCIATION_GROUP_INFO_REPORT(byte[] data)
            {
                ASSOCIATION_GROUP_INFO_REPORT ret = new ASSOCIATION_GROUP_INFO_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.vg1 = new List<TVG1>();
                    for (int j = 0; j < ret.properties1.groupCount; j++)
                    {
                        TVG1 tmp = new TVG1();
                        tmp.groupingIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.mode = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.profile1 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.profile2 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.reserved = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.eventCode = (data.Length - index) >= TVG1.eventCodeBytesCount ? new byte[TVG1.eventCodeBytesCount] : new byte[data.Length - index];
                        if (data.Length > index) tmp.eventCode[0] = data[index++];
                        if (data.Length > index) tmp.eventCode[1] = data[index++];
                        ret.vg1.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](ASSOCIATION_GROUP_INFO_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ASSOCIATION_GRP_INFO_V2.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.vg1 != null)
                {
                    foreach (var item in command.vg1)
                    {
                        if (item.groupingIdentifier.HasValue) ret.Add(item.groupingIdentifier);
                        if (item.mode.HasValue) ret.Add(item.mode);
                        if (item.profile1.HasValue) ret.Add(item.profile1);
                        if (item.profile2.HasValue) ret.Add(item.profile2);
                        if (item.reserved.HasValue) ret.Add(item.reserved);
                        if (item.eventCode != null)
                        {
                            foreach (var tmp in item.eventCode)
                            {
                                ret.Add(tmp);
                            }
                        }
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class ASSOCIATION_GROUP_COMMAND_LIST_GET
        {
            public const byte ID = 0x05;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte reserved
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte allowCache
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
            public ByteValue groupingIdentifier = 0;
            public static implicit operator ASSOCIATION_GROUP_COMMAND_LIST_GET(byte[] data)
            {
                ASSOCIATION_GROUP_COMMAND_LIST_GET ret = new ASSOCIATION_GROUP_COMMAND_LIST_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.groupingIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](ASSOCIATION_GROUP_COMMAND_LIST_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ASSOCIATION_GRP_INFO_V2.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.groupingIdentifier.HasValue) ret.Add(command.groupingIdentifier);
                return ret.ToArray();
            }
        }
        public partial class ASSOCIATION_GROUP_COMMAND_LIST_REPORT
        {
            public const byte ID = 0x06;
            public ByteValue groupingIdentifier = 0;
            public ByteValue listLength = 0;
            public IList<byte> command = new List<byte>();
            public static implicit operator ASSOCIATION_GROUP_COMMAND_LIST_REPORT(byte[] data)
            {
                ASSOCIATION_GROUP_COMMAND_LIST_REPORT ret = new ASSOCIATION_GROUP_COMMAND_LIST_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.groupingIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.listLength = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.command = new List<byte>();
                    for (int i = 0; i < ret.listLength; i++)
                    {
                        if (data.Length > index) ret.command.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](ASSOCIATION_GROUP_COMMAND_LIST_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ASSOCIATION_GRP_INFO_V2.ID);
                ret.Add(ID);
                if (command.groupingIdentifier.HasValue) ret.Add(command.groupingIdentifier);
                if (command.listLength.HasValue) ret.Add(command.listLength);
                if (command.command != null)
                {
                    foreach (var tmp in command.command)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

