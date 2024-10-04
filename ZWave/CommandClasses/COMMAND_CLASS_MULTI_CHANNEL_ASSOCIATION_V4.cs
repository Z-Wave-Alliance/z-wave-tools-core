/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_MULTI_CHANNEL_ASSOCIATION_V4
    {
        public const byte ID = 0x8E;
        public const byte VERSION = 4;
        public partial class MULTI_CHANNEL_ASSOCIATION_GET
        {
            public const byte ID = 0x02;
            public ByteValue groupingIdentifier = 0;
            public static implicit operator MULTI_CHANNEL_ASSOCIATION_GET(byte[] data)
            {
                MULTI_CHANNEL_ASSOCIATION_GET ret = new MULTI_CHANNEL_ASSOCIATION_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.groupingIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](MULTI_CHANNEL_ASSOCIATION_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_MULTI_CHANNEL_ASSOCIATION_V4.ID);
                ret.Add(ID);
                if (command.groupingIdentifier.HasValue) ret.Add(command.groupingIdentifier);
                return ret.ToArray();
            }
        }
        public partial class MULTI_CHANNEL_ASSOCIATION_GROUPINGS_GET
        {
            public const byte ID = 0x05;
            public static implicit operator MULTI_CHANNEL_ASSOCIATION_GROUPINGS_GET(byte[] data)
            {
                MULTI_CHANNEL_ASSOCIATION_GROUPINGS_GET ret = new MULTI_CHANNEL_ASSOCIATION_GROUPINGS_GET();
                return ret;
            }
            public static implicit operator byte[](MULTI_CHANNEL_ASSOCIATION_GROUPINGS_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_MULTI_CHANNEL_ASSOCIATION_V4.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class MULTI_CHANNEL_ASSOCIATION_GROUPINGS_REPORT
        {
            public const byte ID = 0x06;
            public ByteValue supportedGroupings = 0;
            public static implicit operator MULTI_CHANNEL_ASSOCIATION_GROUPINGS_REPORT(byte[] data)
            {
                MULTI_CHANNEL_ASSOCIATION_GROUPINGS_REPORT ret = new MULTI_CHANNEL_ASSOCIATION_GROUPINGS_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.supportedGroupings = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](MULTI_CHANNEL_ASSOCIATION_GROUPINGS_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_MULTI_CHANNEL_ASSOCIATION_V4.ID);
                ret.Add(ID);
                if (command.supportedGroupings.HasValue) ret.Add(command.supportedGroupings);
                return ret.ToArray();
            }
        }
        public partial class MULTI_CHANNEL_ASSOCIATION_REMOVE
        {
            public const byte ID = 0x04;
            public ByteValue groupingIdentifier = 0;
            public IList<byte> nodeId = new List<byte>();
            private byte[] marker = {0x00};
            public class TVG
            {
                public ByteValue multiChannelNodeId = 0;
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
                    public byte bitAddress
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
            public static implicit operator MULTI_CHANNEL_ASSOCIATION_REMOVE(byte[] data)
            {
                MULTI_CHANNEL_ASSOCIATION_REMOVE ret = new MULTI_CHANNEL_ASSOCIATION_REMOVE();
                if (data != null)
                {
                    int index = 2;
                    ret.groupingIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nodeId = new List<byte>();
                    while (data.Length - 0 > index && (data.Length - 1 < index || data[index + 0] != ret.marker[0]))
                    {
                        if (data.Length > index) ret.nodeId.Add(data[index++]);
                    }
                    ret.marker = (data.Length - index) >= 1 ? new byte[1] : new byte[data.Length - index]; //Marker
                    if (data.Length > index) ret.marker[0] = data[index++]; //Marker
                    ret.vg = new List<TVG>();
                    while (data.Length - 0 > index)
                    {
                        TVG tmp = new TVG();
                        tmp.multiChannelNodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.properties1 = data.Length > index ? (TVG.Tproperties1)data[index++] : TVG.Tproperties1.Empty;
                        ret.vg.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](MULTI_CHANNEL_ASSOCIATION_REMOVE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_MULTI_CHANNEL_ASSOCIATION_V4.ID);
                ret.Add(ID);
                if (command.groupingIdentifier.HasValue) ret.Add(command.groupingIdentifier);
                if (command.nodeId != null)
                {
                    foreach (var tmp in command.nodeId)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.marker != null && command.marker.Length > 0) ret.Add(command.marker[0]);
                if (command.vg != null)
                {
                    foreach (var item in command.vg)
                    {
                        if (item.multiChannelNodeId.HasValue) ret.Add(item.multiChannelNodeId);
                        if (item.properties1.HasValue) ret.Add(item.properties1);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class MULTI_CHANNEL_ASSOCIATION_REPORT
        {
            public const byte ID = 0x03;
            public ByteValue groupingIdentifier = 0;
            public ByteValue maxNodesSupported = 0;
            public ByteValue reportsToFollow = 0;
            public IList<byte> nodeId = new List<byte>();
            private byte[] marker = {0x00};
            public class TVG
            {
                public ByteValue multiChannelNodeId = 0;
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
                    public byte bitAddress
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
            public static implicit operator MULTI_CHANNEL_ASSOCIATION_REPORT(byte[] data)
            {
                MULTI_CHANNEL_ASSOCIATION_REPORT ret = new MULTI_CHANNEL_ASSOCIATION_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.groupingIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.maxNodesSupported = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.reportsToFollow = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nodeId = new List<byte>();
                    while (data.Length - 0 > index && (data.Length - 1 < index || data[index + 0] != ret.marker[0]))
                    {
                        if (data.Length > index) ret.nodeId.Add(data[index++]);
                    }
                    ret.marker = (data.Length - index) >= 1 ? new byte[1] : new byte[data.Length - index]; //Marker
                    if (data.Length > index) ret.marker[0] = data[index++]; //Marker
                    ret.vg = new List<TVG>();
                    while (data.Length - 0 > index)
                    {
                        TVG tmp = new TVG();
                        tmp.multiChannelNodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.properties1 = data.Length > index ? (TVG.Tproperties1)data[index++] : TVG.Tproperties1.Empty;
                        ret.vg.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](MULTI_CHANNEL_ASSOCIATION_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_MULTI_CHANNEL_ASSOCIATION_V4.ID);
                ret.Add(ID);
                if (command.groupingIdentifier.HasValue) ret.Add(command.groupingIdentifier);
                if (command.maxNodesSupported.HasValue) ret.Add(command.maxNodesSupported);
                if (command.reportsToFollow.HasValue) ret.Add(command.reportsToFollow);
                if (command.nodeId != null)
                {
                    foreach (var tmp in command.nodeId)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.marker != null && command.marker.Length > 0) ret.Add(command.marker[0]);
                if (command.vg != null)
                {
                    foreach (var item in command.vg)
                    {
                        if (item.multiChannelNodeId.HasValue) ret.Add(item.multiChannelNodeId);
                        if (item.properties1.HasValue) ret.Add(item.properties1);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class MULTI_CHANNEL_ASSOCIATION_SET
        {
            public const byte ID = 0x01;
            public ByteValue groupingIdentifier = 0;
            public IList<byte> nodeId = new List<byte>();
            private byte[] marker = {0x00};
            public class TVG
            {
                public ByteValue multiChannelNodeId = 0;
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
                    public byte bitAddress
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
            public static implicit operator MULTI_CHANNEL_ASSOCIATION_SET(byte[] data)
            {
                MULTI_CHANNEL_ASSOCIATION_SET ret = new MULTI_CHANNEL_ASSOCIATION_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.groupingIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nodeId = new List<byte>();
                    while (data.Length - 0 > index && (data.Length - 1 < index || data[index + 0] != ret.marker[0]))
                    {
                        if (data.Length > index) ret.nodeId.Add(data[index++]);
                    }
                    ret.marker = (data.Length - index) >= 1 ? new byte[1] : new byte[data.Length - index]; //Marker
                    if (data.Length > index) ret.marker[0] = data[index++]; //Marker
                    ret.vg = new List<TVG>();
                    while (data.Length - 0 > index)
                    {
                        TVG tmp = new TVG();
                        tmp.multiChannelNodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.properties1 = data.Length > index ? (TVG.Tproperties1)data[index++] : TVG.Tproperties1.Empty;
                        ret.vg.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](MULTI_CHANNEL_ASSOCIATION_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_MULTI_CHANNEL_ASSOCIATION_V4.ID);
                ret.Add(ID);
                if (command.groupingIdentifier.HasValue) ret.Add(command.groupingIdentifier);
                if (command.nodeId != null)
                {
                    foreach (var tmp in command.nodeId)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.marker != null && command.marker.Length > 0) ret.Add(command.marker[0]);
                if (command.vg != null)
                {
                    foreach (var item in command.vg)
                    {
                        if (item.multiChannelNodeId.HasValue) ret.Add(item.multiChannelNodeId);
                        if (item.properties1.HasValue) ret.Add(item.properties1);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

