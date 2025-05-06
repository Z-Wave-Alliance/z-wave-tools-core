using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_NETWORK_MANAGEMENT_PROXY_V4
    {
        public const byte ID = 0x52;
        public const byte VERSION = 4;
        public partial class NODE_INFO_CACHED_GET
        {
            public const byte ID = 0x03;
            public ByteValue seqNo = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte maxAge
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
            public ByteValue nodeId = 0;
            public const byte extendedNodeidBytesCount = 2;
            public byte[] extendedNodeid = new byte[extendedNodeidBytesCount];
            public static implicit operator NODE_INFO_CACHED_GET(byte[] data)
            {
                NODE_INFO_CACHED_GET ret = new NODE_INFO_CACHED_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.nodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.extendedNodeid = (data.Length - index) >= extendedNodeidBytesCount ? new byte[extendedNodeidBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.extendedNodeid[0] = data[index++];
                    if (data.Length > index) ret.extendedNodeid[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](NODE_INFO_CACHED_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_PROXY_V4.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.nodeId.HasValue) ret.Add(command.nodeId);
                if (command.extendedNodeid != null)
                {
                    foreach (var tmp in command.extendedNodeid)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class NODE_INFO_CACHED_REPORT
        {
            public const byte ID = 0x04;
            public ByteValue seqNo = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte age
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte status
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
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte zWaveProtocolSpecificPart1
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte listening
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
            public struct Tproperties3
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties3 Empty { get { return new Tproperties3() { _value = 0, HasValue = false }; } }
                public byte zWaveProtocolSpecificPart2
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte opt
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties3(byte data)
                {
                    Tproperties3 ret = new Tproperties3();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties3 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties3 properties3 = 0;
            public ByteValue grantedKeys = 0;
            public ByteValue basicDeviceClass = 0;
            public ByteValue genericDeviceClass = 0;
            public ByteValue specificDeviceClass = 0;
            public IList<byte> commandClass = new List<byte>();
            public static implicit operator NODE_INFO_CACHED_REPORT(byte[] data)
            {
                NODE_INFO_CACHED_REPORT ret = new NODE_INFO_CACHED_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.properties3 = data.Length > index ? (Tproperties3)data[index++] : Tproperties3.Empty;
                    ret.grantedKeys = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.basicDeviceClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
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
            public static implicit operator byte[](NODE_INFO_CACHED_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_PROXY_V4.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.properties3.HasValue) ret.Add(command.properties3);
                if (command.grantedKeys.HasValue) ret.Add(command.grantedKeys);
                if (command.basicDeviceClass.HasValue) ret.Add(command.basicDeviceClass);
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
        public partial class NODE_LIST_GET
        {
            public const byte ID = 0x01;
            public ByteValue seqNo = 0;
            public static implicit operator NODE_LIST_GET(byte[] data)
            {
                NODE_LIST_GET ret = new NODE_LIST_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](NODE_LIST_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_PROXY_V4.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                return ret.ToArray();
            }
        }
        public partial class NODE_LIST_REPORT
        {
            public const byte ID = 0x02;
            public ByteValue seqNo = 0;
            public ByteValue status = 0;
            public ByteValue nodeListControllerId = 0;
            public const byte nodeListDataBytesCount = 29;
            public byte[] nodeListData = new byte[nodeListDataBytesCount];
            public const byte extendedNodeListLengthBytesCount = 2;
            public byte[] extendedNodeListLength = new byte[extendedNodeListLengthBytesCount];
            public IList<byte> extendedNodeList = new List<byte>();
            public static implicit operator NODE_LIST_REPORT(byte[] data)
            {
                NODE_LIST_REPORT ret = new NODE_LIST_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.status = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nodeListControllerId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nodeListData = (data.Length - index) >= nodeListDataBytesCount ? new byte[nodeListDataBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.nodeListData[0] = data[index++];
                    if (data.Length > index) ret.nodeListData[1] = data[index++];
                    if (data.Length > index) ret.nodeListData[2] = data[index++];
                    if (data.Length > index) ret.nodeListData[3] = data[index++];
                    if (data.Length > index) ret.nodeListData[4] = data[index++];
                    if (data.Length > index) ret.nodeListData[5] = data[index++];
                    if (data.Length > index) ret.nodeListData[6] = data[index++];
                    if (data.Length > index) ret.nodeListData[7] = data[index++];
                    if (data.Length > index) ret.nodeListData[8] = data[index++];
                    if (data.Length > index) ret.nodeListData[9] = data[index++];
                    if (data.Length > index) ret.nodeListData[10] = data[index++];
                    if (data.Length > index) ret.nodeListData[11] = data[index++];
                    if (data.Length > index) ret.nodeListData[12] = data[index++];
                    if (data.Length > index) ret.nodeListData[13] = data[index++];
                    if (data.Length > index) ret.nodeListData[14] = data[index++];
                    if (data.Length > index) ret.nodeListData[15] = data[index++];
                    if (data.Length > index) ret.nodeListData[16] = data[index++];
                    if (data.Length > index) ret.nodeListData[17] = data[index++];
                    if (data.Length > index) ret.nodeListData[18] = data[index++];
                    if (data.Length > index) ret.nodeListData[19] = data[index++];
                    if (data.Length > index) ret.nodeListData[20] = data[index++];
                    if (data.Length > index) ret.nodeListData[21] = data[index++];
                    if (data.Length > index) ret.nodeListData[22] = data[index++];
                    if (data.Length > index) ret.nodeListData[23] = data[index++];
                    if (data.Length > index) ret.nodeListData[24] = data[index++];
                    if (data.Length > index) ret.nodeListData[25] = data[index++];
                    if (data.Length > index) ret.nodeListData[26] = data[index++];
                    if (data.Length > index) ret.nodeListData[27] = data[index++];
                    if (data.Length > index) ret.nodeListData[28] = data[index++];
                    ret.extendedNodeListLength = (data.Length - index) >= extendedNodeListLengthBytesCount ? new byte[extendedNodeListLengthBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.extendedNodeListLength[0] = data[index++];
                    if (data.Length > index) ret.extendedNodeListLength[1] = data[index++];
                    ret.extendedNodeList = new List<byte>();
                    for (int i = 0; i < (ret.extendedNodeListLength[0] << 8) + ret.extendedNodeListLength[1]; i++)
                    {
                        if (data.Length > index) ret.extendedNodeList.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](NODE_LIST_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_PROXY_V4.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.status.HasValue) ret.Add(command.status);
                if (command.nodeListControllerId.HasValue) ret.Add(command.nodeListControllerId);
                if (command.nodeListData != null)
                {
                    foreach (var tmp in command.nodeListData)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.extendedNodeListLength != null)
                {
                    foreach (var tmp in command.extendedNodeListLength)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.extendedNodeList != null)
                {
                    foreach (var tmp in command.extendedNodeList)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class NM_MULTI_CHANNEL_END_POINT_GET
        {
            public const byte ID = 0x05;
            public ByteValue seqNo = 0;
            public ByteValue nodeid = 0;
            public const byte extendedNodeidBytesCount = 2;
            public byte[] extendedNodeid = new byte[extendedNodeidBytesCount];
            public static implicit operator NM_MULTI_CHANNEL_END_POINT_GET(byte[] data)
            {
                NM_MULTI_CHANNEL_END_POINT_GET ret = new NM_MULTI_CHANNEL_END_POINT_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nodeid = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.extendedNodeid = (data.Length - index) >= extendedNodeidBytesCount ? new byte[extendedNodeidBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.extendedNodeid[0] = data[index++];
                    if (data.Length > index) ret.extendedNodeid[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](NM_MULTI_CHANNEL_END_POINT_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_PROXY_V4.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.nodeid.HasValue) ret.Add(command.nodeid);
                if (command.extendedNodeid != null)
                {
                    foreach (var tmp in command.extendedNodeid)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class NM_MULTI_CHANNEL_END_POINT_REPORT
        {
            public const byte ID = 0x06;
            public ByteValue seqNo = 0;
            public ByteValue nodeid = 0;
            public ByteValue reserved = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte individualEndPoints
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte res1
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
                public byte aggregatedEndPoints
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
            public const byte extendedNodeidBytesCount = 2;
            public byte[] extendedNodeid = new byte[extendedNodeidBytesCount];
            public static implicit operator NM_MULTI_CHANNEL_END_POINT_REPORT(byte[] data)
            {
                NM_MULTI_CHANNEL_END_POINT_REPORT ret = new NM_MULTI_CHANNEL_END_POINT_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nodeid = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.reserved = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.extendedNodeid = (data.Length - index) >= extendedNodeidBytesCount ? new byte[extendedNodeidBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.extendedNodeid[0] = data[index++];
                    if (data.Length > index) ret.extendedNodeid[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](NM_MULTI_CHANNEL_END_POINT_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_PROXY_V4.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.nodeid.HasValue) ret.Add(command.nodeid);
                if (command.reserved.HasValue) ret.Add(command.reserved);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.extendedNodeid != null)
                {
                    foreach (var tmp in command.extendedNodeid)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class NM_MULTI_CHANNEL_CAPABILITY_GET
        {
            public const byte ID = 0x07;
            public ByteValue seqNo = 0;
            public ByteValue nodeid = 0;
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
                public byte res1
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
            public const byte extendedNodeidBytesCount = 2;
            public byte[] extendedNodeid = new byte[extendedNodeidBytesCount];
            public static implicit operator NM_MULTI_CHANNEL_CAPABILITY_GET(byte[] data)
            {
                NM_MULTI_CHANNEL_CAPABILITY_GET ret = new NM_MULTI_CHANNEL_CAPABILITY_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nodeid = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.extendedNodeid = (data.Length - index) >= extendedNodeidBytesCount ? new byte[extendedNodeidBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.extendedNodeid[0] = data[index++];
                    if (data.Length > index) ret.extendedNodeid[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](NM_MULTI_CHANNEL_CAPABILITY_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_PROXY_V4.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.nodeid.HasValue) ret.Add(command.nodeid);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.extendedNodeid != null)
                {
                    foreach (var tmp in command.extendedNodeid)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class NM_MULTI_CHANNEL_CAPABILITY_REPORT
        {
            public const byte ID = 0x08;
            public ByteValue seqNo = 0;
            public ByteValue nodeid = 0;
            public ByteValue commandClassLength = 0;
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
                public byte res1
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
            public const byte extendedNodeidBytesCount = 2;
            public byte[] extendedNodeid = new byte[extendedNodeidBytesCount];
            public static implicit operator NM_MULTI_CHANNEL_CAPABILITY_REPORT(byte[] data)
            {
                NM_MULTI_CHANNEL_CAPABILITY_REPORT ret = new NM_MULTI_CHANNEL_CAPABILITY_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nodeid = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.commandClassLength = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.genericDeviceClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.specificDeviceClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.commandClass = new List<byte>();
                    while (data.Length - 2 > index)
                    {
                        if (data.Length > index) ret.commandClass.Add(data[index++]);
                    }
                    ret.extendedNodeid = (data.Length - index) >= extendedNodeidBytesCount ? new byte[extendedNodeidBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.extendedNodeid[0] = data[index++];
                    if (data.Length > index) ret.extendedNodeid[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](NM_MULTI_CHANNEL_CAPABILITY_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_PROXY_V4.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.nodeid.HasValue) ret.Add(command.nodeid);
                if (command.commandClassLength.HasValue) ret.Add(command.commandClassLength);
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
                if (command.extendedNodeid != null)
                {
                    foreach (var tmp in command.extendedNodeid)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class NM_MULTI_CHANNEL_AGGREGATED_MEMBERS_GET
        {
            public const byte ID = 0x09;
            public ByteValue seqNo = 0;
            public ByteValue nodeid = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte aggregatedEndPoint
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte res1
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
            public const byte extendedNodeidBytesCount = 2;
            public byte[] extendedNodeid = new byte[extendedNodeidBytesCount];
            public static implicit operator NM_MULTI_CHANNEL_AGGREGATED_MEMBERS_GET(byte[] data)
            {
                NM_MULTI_CHANNEL_AGGREGATED_MEMBERS_GET ret = new NM_MULTI_CHANNEL_AGGREGATED_MEMBERS_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nodeid = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.extendedNodeid = (data.Length - index) >= extendedNodeidBytesCount ? new byte[extendedNodeidBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.extendedNodeid[0] = data[index++];
                    if (data.Length > index) ret.extendedNodeid[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](NM_MULTI_CHANNEL_AGGREGATED_MEMBERS_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_PROXY_V4.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.nodeid.HasValue) ret.Add(command.nodeid);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.extendedNodeid != null)
                {
                    foreach (var tmp in command.extendedNodeid)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class NM_MULTI_CHANNEL_AGGREGATED_MEMBERS_REPORT
        {
            public const byte ID = 0x0A;
            public ByteValue seqNo = 0;
            public ByteValue nodeid = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte aggregatedEndPoint
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte res1
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
            public ByteValue numberOfMembers = 0;
            public class TVG1
            {
                public struct Tproperties1
                {
                    private byte _value;
                    public bool HasValue { get; private set; }
                    public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                    public byte memberEndpoint
                    {
                        get { return (byte)(_value >> 0 & 0x7F); }
                        set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
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
            }
            public List<TVG1> vg1 = new List<TVG1>();
            public const byte extendedNodeidBytesCount = 2;
            public byte[] extendedNodeid = new byte[extendedNodeidBytesCount];
            public static implicit operator NM_MULTI_CHANNEL_AGGREGATED_MEMBERS_REPORT(byte[] data)
            {
                NM_MULTI_CHANNEL_AGGREGATED_MEMBERS_REPORT ret = new NM_MULTI_CHANNEL_AGGREGATED_MEMBERS_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nodeid = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.numberOfMembers = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.vg1 = new List<TVG1>();
                    while (data.Length - 2 > index)
                    {
                        TVG1 tmp = new TVG1();
                        tmp.properties1 = data.Length > index ? (TVG1.Tproperties1)data[index++] : TVG1.Tproperties1.Empty;
                        ret.vg1.Add(tmp);
                    }
                    ret.extendedNodeid = (data.Length - index) >= extendedNodeidBytesCount ? new byte[extendedNodeidBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.extendedNodeid[0] = data[index++];
                    if (data.Length > index) ret.extendedNodeid[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](NM_MULTI_CHANNEL_AGGREGATED_MEMBERS_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_PROXY_V4.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.nodeid.HasValue) ret.Add(command.nodeid);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.numberOfMembers.HasValue) ret.Add(command.numberOfMembers);
                if (command.vg1 != null)
                {
                    foreach (var item in command.vg1)
                    {
                        if (item.properties1.HasValue) ret.Add(item.properties1);
                    }
                }
                if (command.extendedNodeid != null)
                {
                    foreach (var tmp in command.extendedNodeid)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class FAILED_NODE_LIST_GET
        {
            public const byte ID = 0x0B;
            public ByteValue seqNo = 0;
            public static implicit operator FAILED_NODE_LIST_GET(byte[] data)
            {
                FAILED_NODE_LIST_GET ret = new FAILED_NODE_LIST_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](FAILED_NODE_LIST_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_PROXY_V4.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                return ret.ToArray();
            }
        }
        public partial class FAILED_NODE_LIST_REPORT
        {
            public const byte ID = 0x0C;
            public ByteValue seqNo = 0;
            public const byte failedNodeListDataBytesCount = 29;
            public byte[] failedNodeListData = new byte[failedNodeListDataBytesCount];
            public const byte extendedFailedNodeListLengthBytesCount = 2;
            public byte[] extendedFailedNodeListLength = new byte[extendedFailedNodeListLengthBytesCount];
            public IList<byte> extendedFailedNodeList = new List<byte>();
            public static implicit operator FAILED_NODE_LIST_REPORT(byte[] data)
            {
                FAILED_NODE_LIST_REPORT ret = new FAILED_NODE_LIST_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.failedNodeListData = (data.Length - index) >= failedNodeListDataBytesCount ? new byte[failedNodeListDataBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.failedNodeListData[0] = data[index++];
                    if (data.Length > index) ret.failedNodeListData[1] = data[index++];
                    if (data.Length > index) ret.failedNodeListData[2] = data[index++];
                    if (data.Length > index) ret.failedNodeListData[3] = data[index++];
                    if (data.Length > index) ret.failedNodeListData[4] = data[index++];
                    if (data.Length > index) ret.failedNodeListData[5] = data[index++];
                    if (data.Length > index) ret.failedNodeListData[6] = data[index++];
                    if (data.Length > index) ret.failedNodeListData[7] = data[index++];
                    if (data.Length > index) ret.failedNodeListData[8] = data[index++];
                    if (data.Length > index) ret.failedNodeListData[9] = data[index++];
                    if (data.Length > index) ret.failedNodeListData[10] = data[index++];
                    if (data.Length > index) ret.failedNodeListData[11] = data[index++];
                    if (data.Length > index) ret.failedNodeListData[12] = data[index++];
                    if (data.Length > index) ret.failedNodeListData[13] = data[index++];
                    if (data.Length > index) ret.failedNodeListData[14] = data[index++];
                    if (data.Length > index) ret.failedNodeListData[15] = data[index++];
                    if (data.Length > index) ret.failedNodeListData[16] = data[index++];
                    if (data.Length > index) ret.failedNodeListData[17] = data[index++];
                    if (data.Length > index) ret.failedNodeListData[18] = data[index++];
                    if (data.Length > index) ret.failedNodeListData[19] = data[index++];
                    if (data.Length > index) ret.failedNodeListData[20] = data[index++];
                    if (data.Length > index) ret.failedNodeListData[21] = data[index++];
                    if (data.Length > index) ret.failedNodeListData[22] = data[index++];
                    if (data.Length > index) ret.failedNodeListData[23] = data[index++];
                    if (data.Length > index) ret.failedNodeListData[24] = data[index++];
                    if (data.Length > index) ret.failedNodeListData[25] = data[index++];
                    if (data.Length > index) ret.failedNodeListData[26] = data[index++];
                    if (data.Length > index) ret.failedNodeListData[27] = data[index++];
                    if (data.Length > index) ret.failedNodeListData[28] = data[index++];
                    ret.extendedFailedNodeListLength = (data.Length - index) >= extendedFailedNodeListLengthBytesCount ? new byte[extendedFailedNodeListLengthBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.extendedFailedNodeListLength[0] = data[index++];
                    if (data.Length > index) ret.extendedFailedNodeListLength[1] = data[index++];
                    ret.extendedFailedNodeList = new List<byte>();
                    for (int i = 0; i < (ret.extendedFailedNodeListLength[0] << 8) + ret.extendedFailedNodeListLength[1]; i++)
                    {
                        if (data.Length > index) ret.extendedFailedNodeList.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](FAILED_NODE_LIST_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_PROXY_V4.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.failedNodeListData != null)
                {
                    foreach (var tmp in command.failedNodeListData)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.extendedFailedNodeListLength != null)
                {
                    foreach (var tmp in command.extendedFailedNodeListLength)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.extendedFailedNodeList != null)
                {
                    foreach (var tmp in command.extendedFailedNodeList)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

