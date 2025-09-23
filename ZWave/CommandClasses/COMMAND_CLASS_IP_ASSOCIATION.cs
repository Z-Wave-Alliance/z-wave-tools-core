/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_IP_ASSOCIATION
    {
        public const byte ID = 0x5C;
        public const byte VERSION = 1;
        public partial class IP_ASSOCIATION_SET
        {
            public const byte ID = 0x01;
            public ByteValue groupingIdentifier = 0;
            public const byte ipv6AddressBytesCount = 16;
            public byte[] ipv6Address = new byte[ipv6AddressBytesCount];
            public ByteValue endPoint = 0;
            public static implicit operator IP_ASSOCIATION_SET(byte[] data)
            {
                IP_ASSOCIATION_SET ret = new IP_ASSOCIATION_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.groupingIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
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
                    ret.endPoint = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](IP_ASSOCIATION_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IP_ASSOCIATION.ID);
                ret.Add(ID);
                if (command.groupingIdentifier.HasValue) ret.Add(command.groupingIdentifier);
                if (command.ipv6Address != null)
                {
                    foreach (var tmp in command.ipv6Address)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.endPoint.HasValue) ret.Add(command.endPoint);
                return ret.ToArray();
            }
        }
        public partial class IP_ASSOCIATION_GET
        {
            public const byte ID = 0x02;
            public ByteValue groupingIdentifier = 0;
            public ByteValue index = 0;
            public static implicit operator IP_ASSOCIATION_GET(byte[] data)
            {
                IP_ASSOCIATION_GET ret = new IP_ASSOCIATION_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.groupingIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.index = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](IP_ASSOCIATION_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IP_ASSOCIATION.ID);
                ret.Add(ID);
                if (command.groupingIdentifier.HasValue) ret.Add(command.groupingIdentifier);
                if (command.index.HasValue) ret.Add(command.index);
                return ret.ToArray();
            }
        }
        public partial class IP_ASSOCIATION_REPORT
        {
            public const byte ID = 0x03;
            public ByteValue groupingIdentifier = 0;
            public ByteValue index = 0;
            public ByteValue actualNodes = 0;
            public const byte ipv6AddressBytesCount = 16;
            public byte[] ipv6Address = new byte[ipv6AddressBytesCount];
            public ByteValue endPoint = 0;
            public static implicit operator IP_ASSOCIATION_REPORT(byte[] data)
            {
                IP_ASSOCIATION_REPORT ret = new IP_ASSOCIATION_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.groupingIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.index = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.actualNodes = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
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
                    ret.endPoint = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](IP_ASSOCIATION_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IP_ASSOCIATION.ID);
                ret.Add(ID);
                if (command.groupingIdentifier.HasValue) ret.Add(command.groupingIdentifier);
                if (command.index.HasValue) ret.Add(command.index);
                if (command.actualNodes.HasValue) ret.Add(command.actualNodes);
                if (command.ipv6Address != null)
                {
                    foreach (var tmp in command.ipv6Address)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.endPoint.HasValue) ret.Add(command.endPoint);
                return ret.ToArray();
            }
        }
        public partial class IP_ASSOCIATION_REMOVE
        {
            public const byte ID = 0x04;
            public ByteValue groupingIdentifier = 0;
            public const byte ipv6AddressBytesCount = 16;
            public byte[] ipv6Address = new byte[ipv6AddressBytesCount];
            public ByteValue endPoint = 0;
            public static implicit operator IP_ASSOCIATION_REMOVE(byte[] data)
            {
                IP_ASSOCIATION_REMOVE ret = new IP_ASSOCIATION_REMOVE();
                if (data != null)
                {
                    int index = 2;
                    ret.groupingIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
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
                    ret.endPoint = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](IP_ASSOCIATION_REMOVE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IP_ASSOCIATION.ID);
                ret.Add(ID);
                if (command.groupingIdentifier.HasValue) ret.Add(command.groupingIdentifier);
                if (command.ipv6Address != null)
                {
                    foreach (var tmp in command.ipv6Address)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.endPoint.HasValue) ret.Add(command.endPoint);
                return ret.ToArray();
            }
        }
    }
}

