/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_ZIP_GATEWAY
    {
        public const byte ID = 0x5F;
        public const byte VERSION = 1;
        public partial class GATEWAY_MODE_SET
        {
            public const byte ID = 0x01;
            public ByteValue mode = 0;
            public static implicit operator GATEWAY_MODE_SET(byte[] data)
            {
                GATEWAY_MODE_SET ret = new GATEWAY_MODE_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.mode = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](GATEWAY_MODE_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZIP_GATEWAY.ID);
                ret.Add(ID);
                if (command.mode.HasValue) ret.Add(command.mode);
                return ret.ToArray();
            }
        }
        public partial class GATEWAY_MODE_GET
        {
            public const byte ID = 0x02;
            public static implicit operator GATEWAY_MODE_GET(byte[] data)
            {
                GATEWAY_MODE_GET ret = new GATEWAY_MODE_GET();
                return ret;
            }
            public static implicit operator byte[](GATEWAY_MODE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZIP_GATEWAY.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class GATEWAY_MODE_REPORT
        {
            public const byte ID = 0x03;
            public ByteValue mode = 0;
            public static implicit operator GATEWAY_MODE_REPORT(byte[] data)
            {
                GATEWAY_MODE_REPORT ret = new GATEWAY_MODE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.mode = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](GATEWAY_MODE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZIP_GATEWAY.ID);
                ret.Add(ID);
                if (command.mode.HasValue) ret.Add(command.mode);
                return ret.ToArray();
            }
        }
        public partial class GATEWAY_PEER_SET
        {
            public const byte ID = 0x04;
            public ByteValue peerProfile = 0;
            public const byte ipv6AddressBytesCount = 16;
            public byte[] ipv6Address = new byte[ipv6AddressBytesCount];
            public const byte portBytesCount = 2;
            public byte[] port = new byte[portBytesCount];
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte peerNameLength
                {
                    get { return (byte)(_value >> 0 & 0x3F); }
                    set { HasValue = true; _value &= 0xFF - 0x3F; _value += (byte)(value << 0 & 0x3F); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 6 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0xC0; _value += (byte)(value << 6 & 0xC0); }
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
            public IList<byte> peerName = new List<byte>();
            public static implicit operator GATEWAY_PEER_SET(byte[] data)
            {
                GATEWAY_PEER_SET ret = new GATEWAY_PEER_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.peerProfile = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
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
                    ret.port = (data.Length - index) >= portBytesCount ? new byte[portBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.port[0] = data[index++];
                    if (data.Length > index) ret.port[1] = data[index++];
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.peerName = new List<byte>();
                    for (int i = 0; i < ret.properties1.peerNameLength; i++)
                    {
                        if (data.Length > index) ret.peerName.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](GATEWAY_PEER_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZIP_GATEWAY.ID);
                ret.Add(ID);
                if (command.peerProfile.HasValue) ret.Add(command.peerProfile);
                if (command.ipv6Address != null)
                {
                    foreach (var tmp in command.ipv6Address)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.port != null)
                {
                    foreach (var tmp in command.port)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.peerName != null)
                {
                    foreach (var tmp in command.peerName)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class GATEWAY_PEER_GET
        {
            public const byte ID = 0x05;
            public ByteValue peerProfile = 0;
            public static implicit operator GATEWAY_PEER_GET(byte[] data)
            {
                GATEWAY_PEER_GET ret = new GATEWAY_PEER_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.peerProfile = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](GATEWAY_PEER_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZIP_GATEWAY.ID);
                ret.Add(ID);
                if (command.peerProfile.HasValue) ret.Add(command.peerProfile);
                return ret.ToArray();
            }
        }
        public partial class GATEWAY_PEER_REPORT
        {
            public const byte ID = 0x06;
            public ByteValue peerProfile = 0;
            public ByteValue peerCount = 0;
            public const byte ipv6AddressBytesCount = 16;
            public byte[] ipv6Address = new byte[ipv6AddressBytesCount];
            public const byte portBytesCount = 2;
            public byte[] port = new byte[portBytesCount];
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte peerNameLength
                {
                    get { return (byte)(_value >> 0 & 0x3F); }
                    set { HasValue = true; _value &= 0xFF - 0x3F; _value += (byte)(value << 0 & 0x3F); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 6 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0xC0; _value += (byte)(value << 6 & 0xC0); }
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
            public IList<byte> peerName = new List<byte>();
            public static implicit operator GATEWAY_PEER_REPORT(byte[] data)
            {
                GATEWAY_PEER_REPORT ret = new GATEWAY_PEER_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.peerProfile = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.peerCount = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
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
                    ret.port = (data.Length - index) >= portBytesCount ? new byte[portBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.port[0] = data[index++];
                    if (data.Length > index) ret.port[1] = data[index++];
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.peerName = new List<byte>();
                    for (int i = 0; i < ret.properties1.peerNameLength; i++)
                    {
                        if (data.Length > index) ret.peerName.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](GATEWAY_PEER_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZIP_GATEWAY.ID);
                ret.Add(ID);
                if (command.peerProfile.HasValue) ret.Add(command.peerProfile);
                if (command.peerCount.HasValue) ret.Add(command.peerCount);
                if (command.ipv6Address != null)
                {
                    foreach (var tmp in command.ipv6Address)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.port != null)
                {
                    foreach (var tmp in command.port)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.peerName != null)
                {
                    foreach (var tmp in command.peerName)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class GATEWAY_LOCK_SET
        {
            public const byte ID = 0x07;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte mlock
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte show
                {
                    get { return (byte)(_value >> 1 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x02; _value += (byte)(value << 1 & 0x02); }
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
            public static implicit operator GATEWAY_LOCK_SET(byte[] data)
            {
                GATEWAY_LOCK_SET ret = new GATEWAY_LOCK_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](GATEWAY_LOCK_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZIP_GATEWAY.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                return ret.ToArray();
            }
        }
        public partial class UNSOLICITED_DESTINATION_SET
        {
            public const byte ID = 0x08;
            public const byte unsolicitedIpv6DestinationBytesCount = 16;
            public byte[] unsolicitedIpv6Destination = new byte[unsolicitedIpv6DestinationBytesCount];
            public const byte unsolicitedDestinationPortBytesCount = 2;
            public byte[] unsolicitedDestinationPort = new byte[unsolicitedDestinationPortBytesCount];
            public static implicit operator UNSOLICITED_DESTINATION_SET(byte[] data)
            {
                UNSOLICITED_DESTINATION_SET ret = new UNSOLICITED_DESTINATION_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.unsolicitedIpv6Destination = (data.Length - index) >= unsolicitedIpv6DestinationBytesCount ? new byte[unsolicitedIpv6DestinationBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.unsolicitedIpv6Destination[0] = data[index++];
                    if (data.Length > index) ret.unsolicitedIpv6Destination[1] = data[index++];
                    if (data.Length > index) ret.unsolicitedIpv6Destination[2] = data[index++];
                    if (data.Length > index) ret.unsolicitedIpv6Destination[3] = data[index++];
                    if (data.Length > index) ret.unsolicitedIpv6Destination[4] = data[index++];
                    if (data.Length > index) ret.unsolicitedIpv6Destination[5] = data[index++];
                    if (data.Length > index) ret.unsolicitedIpv6Destination[6] = data[index++];
                    if (data.Length > index) ret.unsolicitedIpv6Destination[7] = data[index++];
                    if (data.Length > index) ret.unsolicitedIpv6Destination[8] = data[index++];
                    if (data.Length > index) ret.unsolicitedIpv6Destination[9] = data[index++];
                    if (data.Length > index) ret.unsolicitedIpv6Destination[10] = data[index++];
                    if (data.Length > index) ret.unsolicitedIpv6Destination[11] = data[index++];
                    if (data.Length > index) ret.unsolicitedIpv6Destination[12] = data[index++];
                    if (data.Length > index) ret.unsolicitedIpv6Destination[13] = data[index++];
                    if (data.Length > index) ret.unsolicitedIpv6Destination[14] = data[index++];
                    if (data.Length > index) ret.unsolicitedIpv6Destination[15] = data[index++];
                    ret.unsolicitedDestinationPort = (data.Length - index) >= unsolicitedDestinationPortBytesCount ? new byte[unsolicitedDestinationPortBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.unsolicitedDestinationPort[0] = data[index++];
                    if (data.Length > index) ret.unsolicitedDestinationPort[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](UNSOLICITED_DESTINATION_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZIP_GATEWAY.ID);
                ret.Add(ID);
                if (command.unsolicitedIpv6Destination != null)
                {
                    foreach (var tmp in command.unsolicitedIpv6Destination)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.unsolicitedDestinationPort != null)
                {
                    foreach (var tmp in command.unsolicitedDestinationPort)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class UNSOLICITED_DESTINATION_GET
        {
            public const byte ID = 0x09;
            public static implicit operator UNSOLICITED_DESTINATION_GET(byte[] data)
            {
                UNSOLICITED_DESTINATION_GET ret = new UNSOLICITED_DESTINATION_GET();
                return ret;
            }
            public static implicit operator byte[](UNSOLICITED_DESTINATION_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZIP_GATEWAY.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class UNSOLICITED_DESTINATION_REPORT
        {
            public const byte ID = 0x0A;
            public const byte unsolicitedIpv6DestinationBytesCount = 16;
            public byte[] unsolicitedIpv6Destination = new byte[unsolicitedIpv6DestinationBytesCount];
            public const byte unsolicitedDestinationPortBytesCount = 2;
            public byte[] unsolicitedDestinationPort = new byte[unsolicitedDestinationPortBytesCount];
            public static implicit operator UNSOLICITED_DESTINATION_REPORT(byte[] data)
            {
                UNSOLICITED_DESTINATION_REPORT ret = new UNSOLICITED_DESTINATION_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.unsolicitedIpv6Destination = (data.Length - index) >= unsolicitedIpv6DestinationBytesCount ? new byte[unsolicitedIpv6DestinationBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.unsolicitedIpv6Destination[0] = data[index++];
                    if (data.Length > index) ret.unsolicitedIpv6Destination[1] = data[index++];
                    if (data.Length > index) ret.unsolicitedIpv6Destination[2] = data[index++];
                    if (data.Length > index) ret.unsolicitedIpv6Destination[3] = data[index++];
                    if (data.Length > index) ret.unsolicitedIpv6Destination[4] = data[index++];
                    if (data.Length > index) ret.unsolicitedIpv6Destination[5] = data[index++];
                    if (data.Length > index) ret.unsolicitedIpv6Destination[6] = data[index++];
                    if (data.Length > index) ret.unsolicitedIpv6Destination[7] = data[index++];
                    if (data.Length > index) ret.unsolicitedIpv6Destination[8] = data[index++];
                    if (data.Length > index) ret.unsolicitedIpv6Destination[9] = data[index++];
                    if (data.Length > index) ret.unsolicitedIpv6Destination[10] = data[index++];
                    if (data.Length > index) ret.unsolicitedIpv6Destination[11] = data[index++];
                    if (data.Length > index) ret.unsolicitedIpv6Destination[12] = data[index++];
                    if (data.Length > index) ret.unsolicitedIpv6Destination[13] = data[index++];
                    if (data.Length > index) ret.unsolicitedIpv6Destination[14] = data[index++];
                    if (data.Length > index) ret.unsolicitedIpv6Destination[15] = data[index++];
                    ret.unsolicitedDestinationPort = (data.Length - index) >= unsolicitedDestinationPortBytesCount ? new byte[unsolicitedDestinationPortBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.unsolicitedDestinationPort[0] = data[index++];
                    if (data.Length > index) ret.unsolicitedDestinationPort[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](UNSOLICITED_DESTINATION_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZIP_GATEWAY.ID);
                ret.Add(ID);
                if (command.unsolicitedIpv6Destination != null)
                {
                    foreach (var tmp in command.unsolicitedIpv6Destination)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.unsolicitedDestinationPort != null)
                {
                    foreach (var tmp in command.unsolicitedDestinationPort)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class COMMAND_APPLICATION_NODE_INFO_SET
        {
            public const byte ID = 0x0B;
            public IList<byte> nonSecureCommandClass = new List<byte>();
            private byte[] securityScheme0Mark = {0xF1, 0x00};
            public IList<byte> securityScheme0CommandClass = new List<byte>();
            public static implicit operator COMMAND_APPLICATION_NODE_INFO_SET(byte[] data)
            {
                COMMAND_APPLICATION_NODE_INFO_SET ret = new COMMAND_APPLICATION_NODE_INFO_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.nonSecureCommandClass = new List<byte>();
                    while (data.Length - 0 > index && (data.Length - 2 < index || data[index + 0] != ret.securityScheme0Mark[0] || data[index + 1] != ret.securityScheme0Mark[1]))
                    {
                        if (data.Length > index) ret.nonSecureCommandClass.Add(data[index++]);
                    }
                    ret.securityScheme0Mark = (data.Length - index) >= 2 ? new byte[2] : new byte[data.Length - index]; //Marker
                    if (data.Length > index) ret.securityScheme0Mark[0] = data[index++]; //Marker
                    if (data.Length > index) ret.securityScheme0Mark[1] = data[index++]; //Marker
                    ret.securityScheme0CommandClass = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.securityScheme0CommandClass.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](COMMAND_APPLICATION_NODE_INFO_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZIP_GATEWAY.ID);
                ret.Add(ID);
                if (command.nonSecureCommandClass != null)
                {
                    foreach (var tmp in command.nonSecureCommandClass)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.securityScheme0Mark != null && command.securityScheme0Mark.Length > 0) ret.Add(command.securityScheme0Mark[0]);
                if (command.securityScheme0Mark != null && command.securityScheme0Mark.Length > 1) ret.Add(command.securityScheme0Mark[1]);
                if (command.securityScheme0CommandClass != null)
                {
                    foreach (var tmp in command.securityScheme0CommandClass)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class COMMAND_APPLICATION_NODE_INFO_GET
        {
            public const byte ID = 0x0C;
            public static implicit operator COMMAND_APPLICATION_NODE_INFO_GET(byte[] data)
            {
                COMMAND_APPLICATION_NODE_INFO_GET ret = new COMMAND_APPLICATION_NODE_INFO_GET();
                return ret;
            }
            public static implicit operator byte[](COMMAND_APPLICATION_NODE_INFO_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZIP_GATEWAY.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class COMMAND_APPLICATION_NODE_INFO_REPORT
        {
            public const byte ID = 0x0D;
            public IList<byte> nonSecureCommandClass = new List<byte>();
            private byte[] securityScheme0Mark = {0xF1, 0x00};
            public IList<byte> securityScheme0CommandClass = new List<byte>();
            public static implicit operator COMMAND_APPLICATION_NODE_INFO_REPORT(byte[] data)
            {
                COMMAND_APPLICATION_NODE_INFO_REPORT ret = new COMMAND_APPLICATION_NODE_INFO_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.nonSecureCommandClass = new List<byte>();
                    while (data.Length - 0 > index && (data.Length - 2 < index || data[index + 0] != ret.securityScheme0Mark[0] || data[index + 1] != ret.securityScheme0Mark[1]))
                    {
                        if (data.Length > index) ret.nonSecureCommandClass.Add(data[index++]);
                    }
                    ret.securityScheme0Mark = (data.Length - index) >= 2 ? new byte[2] : new byte[data.Length - index]; //Marker
                    if (data.Length > index) ret.securityScheme0Mark[0] = data[index++]; //Marker
                    if (data.Length > index) ret.securityScheme0Mark[1] = data[index++]; //Marker
                    ret.securityScheme0CommandClass = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.securityScheme0CommandClass.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](COMMAND_APPLICATION_NODE_INFO_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZIP_GATEWAY.ID);
                ret.Add(ID);
                if (command.nonSecureCommandClass != null)
                {
                    foreach (var tmp in command.nonSecureCommandClass)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.securityScheme0Mark != null && command.securityScheme0Mark.Length > 0) ret.Add(command.securityScheme0Mark[0]);
                if (command.securityScheme0Mark != null && command.securityScheme0Mark.Length > 1) ret.Add(command.securityScheme0Mark[1]);
                if (command.securityScheme0CommandClass != null)
                {
                    foreach (var tmp in command.securityScheme0CommandClass)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

