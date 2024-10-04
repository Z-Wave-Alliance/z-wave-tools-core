/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_ZIP_ND_V2
    {
        public const byte ID = 0x58;
        public const byte VERSION = 2;
        public partial class ZIP_NODE_SOLICITATION
        {
            public const byte ID = 0x03;
            public ByteValue reserved = 0;
            public ByteValue nodeId = 0;
            public const byte ipv6AddressBytesCount = 16;
            public byte[] ipv6Address = new byte[ipv6AddressBytesCount];
            public static implicit operator ZIP_NODE_SOLICITATION(byte[] data)
            {
                ZIP_NODE_SOLICITATION ret = new ZIP_NODE_SOLICITATION();
                if (data != null)
                {
                    int index = 2;
                    ret.reserved = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.ipv6Address = (data.Length - index) >= ipv6AddressBytesCount ? new byte[ipv6AddressBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.ipv6Address[0] = data[index++];
                    if (data.Length > index) ret.ipv6Address[1] = data[index++];
                    if (data.Length > index) ret.ipv6Address[2] = data[index++];
                    if (data.Length > index) ret.ipv6Address[3] = data[index++];
                    if (data.Length > index) ret.ipv6Address[4] = data[index++];
                    if (data.Length > index) ret.ipv6Address[5] = data[index++];
                    if (data.Length > index) ret.ipv6Address[6] = data[index++];
                    if (data.Length > index) ret.ipv6Address[7] = data[index++];
                    if (data.Length > index) ret.ipv6Address[8] = data[index++];
                    if (data.Length > index) ret.ipv6Address[9] = data[index++];
                    if (data.Length > index) ret.ipv6Address[10] = data[index++];
                    if (data.Length > index) ret.ipv6Address[11] = data[index++];
                    if (data.Length > index) ret.ipv6Address[12] = data[index++];
                    if (data.Length > index) ret.ipv6Address[13] = data[index++];
                    if (data.Length > index) ret.ipv6Address[14] = data[index++];
                    if (data.Length > index) ret.ipv6Address[15] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](ZIP_NODE_SOLICITATION command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZIP_ND_V2.ID);
                ret.Add(ID);
                if (command.reserved.HasValue) ret.Add(command.reserved);
                if (command.nodeId.HasValue) ret.Add(command.nodeId);
                if (command.ipv6Address != null)
                {
                    foreach (var tmp in command.ipv6Address)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class ZIP_INV_NODE_SOLICITATION
        {
            public const byte ID = 0x04;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte reserved1
                {
                    get { return (byte)(_value >> 0 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x03; _value += (byte)(value << 0 & 0x03); }
                }
                public byte local
                {
                    get { return (byte)(_value >> 2 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x04; _value += (byte)(value << 2 & 0x04); }
                }
                public byte reserved2
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
            public ByteValue nodeId = 0;
            public const byte extendedNodeidBytesCount = 2;
            public byte[] extendedNodeid = new byte[extendedNodeidBytesCount];
            public static implicit operator ZIP_INV_NODE_SOLICITATION(byte[] data)
            {
                ZIP_INV_NODE_SOLICITATION ret = new ZIP_INV_NODE_SOLICITATION();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.nodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.extendedNodeid = (data.Length - index) >= extendedNodeidBytesCount ? new byte[extendedNodeidBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.extendedNodeid[0] = data[index++];
                    if (data.Length > index) ret.extendedNodeid[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](ZIP_INV_NODE_SOLICITATION command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZIP_ND_V2.ID);
                ret.Add(ID);
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
        public partial class ZIP_NODE_ADVERTISEMENT
        {
            public const byte ID = 0x01;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte validity
                {
                    get { return (byte)(_value >> 0 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x03; _value += (byte)(value << 0 & 0x03); }
                }
                public byte local
                {
                    get { return (byte)(_value >> 2 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x04; _value += (byte)(value << 2 & 0x04); }
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
            public ByteValue nodeId = 0;
            public const byte ipv6AddressBytesCount = 16;
            public byte[] ipv6Address = new byte[ipv6AddressBytesCount];
            public const byte homeIdBytesCount = 4;
            public byte[] homeId = new byte[homeIdBytesCount];
            public const byte extendedNodeidBytesCount = 2;
            public byte[] extendedNodeid = new byte[extendedNodeidBytesCount];
            public static implicit operator ZIP_NODE_ADVERTISEMENT(byte[] data)
            {
                ZIP_NODE_ADVERTISEMENT ret = new ZIP_NODE_ADVERTISEMENT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.nodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.ipv6Address = (data.Length - index) >= ipv6AddressBytesCount ? new byte[ipv6AddressBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.ipv6Address[0] = data[index++];
                    if (data.Length > index) ret.ipv6Address[1] = data[index++];
                    if (data.Length > index) ret.ipv6Address[2] = data[index++];
                    if (data.Length > index) ret.ipv6Address[3] = data[index++];
                    if (data.Length > index) ret.ipv6Address[4] = data[index++];
                    if (data.Length > index) ret.ipv6Address[5] = data[index++];
                    if (data.Length > index) ret.ipv6Address[6] = data[index++];
                    if (data.Length > index) ret.ipv6Address[7] = data[index++];
                    if (data.Length > index) ret.ipv6Address[8] = data[index++];
                    if (data.Length > index) ret.ipv6Address[9] = data[index++];
                    if (data.Length > index) ret.ipv6Address[10] = data[index++];
                    if (data.Length > index) ret.ipv6Address[11] = data[index++];
                    if (data.Length > index) ret.ipv6Address[12] = data[index++];
                    if (data.Length > index) ret.ipv6Address[13] = data[index++];
                    if (data.Length > index) ret.ipv6Address[14] = data[index++];
                    if (data.Length > index) ret.ipv6Address[15] = data[index++];
                    ret.homeId = (data.Length - index) >= homeIdBytesCount ? new byte[homeIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.homeId[0] = data[index++];
                    if (data.Length > index) ret.homeId[1] = data[index++];
                    if (data.Length > index) ret.homeId[2] = data[index++];
                    if (data.Length > index) ret.homeId[3] = data[index++];
                    ret.extendedNodeid = (data.Length - index) >= extendedNodeidBytesCount ? new byte[extendedNodeidBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.extendedNodeid[0] = data[index++];
                    if (data.Length > index) ret.extendedNodeid[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](ZIP_NODE_ADVERTISEMENT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZIP_ND_V2.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.nodeId.HasValue) ret.Add(command.nodeId);
                if (command.ipv6Address != null)
                {
                    foreach (var tmp in command.ipv6Address)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.homeId != null)
                {
                    foreach (var tmp in command.homeId)
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
    }
}

