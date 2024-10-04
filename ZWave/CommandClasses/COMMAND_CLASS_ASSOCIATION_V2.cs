/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_ASSOCIATION_V2
    {
        public const byte ID = 0x85;
        public const byte VERSION = 2;
        public partial class ASSOCIATION_GET
        {
            public const byte ID = 0x02;
            public ByteValue groupingIdentifier = 0;
            public static implicit operator ASSOCIATION_GET(byte[] data)
            {
                ASSOCIATION_GET ret = new ASSOCIATION_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.groupingIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](ASSOCIATION_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ASSOCIATION_V2.ID);
                ret.Add(ID);
                if (command.groupingIdentifier.HasValue) ret.Add(command.groupingIdentifier);
                return ret.ToArray();
            }
        }
        public partial class ASSOCIATION_GROUPINGS_GET
        {
            public const byte ID = 0x05;
            public static implicit operator ASSOCIATION_GROUPINGS_GET(byte[] data)
            {
                ASSOCIATION_GROUPINGS_GET ret = new ASSOCIATION_GROUPINGS_GET();
                return ret;
            }
            public static implicit operator byte[](ASSOCIATION_GROUPINGS_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ASSOCIATION_V2.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class ASSOCIATION_GROUPINGS_REPORT
        {
            public const byte ID = 0x06;
            public ByteValue supportedGroupings = 0;
            public static implicit operator ASSOCIATION_GROUPINGS_REPORT(byte[] data)
            {
                ASSOCIATION_GROUPINGS_REPORT ret = new ASSOCIATION_GROUPINGS_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.supportedGroupings = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](ASSOCIATION_GROUPINGS_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ASSOCIATION_V2.ID);
                ret.Add(ID);
                if (command.supportedGroupings.HasValue) ret.Add(command.supportedGroupings);
                return ret.ToArray();
            }
        }
        public partial class ASSOCIATION_REMOVE
        {
            public const byte ID = 0x04;
            public ByteValue groupingIdentifier = 0;
            public IList<byte> nodeId = new List<byte>();
            public static implicit operator ASSOCIATION_REMOVE(byte[] data)
            {
                ASSOCIATION_REMOVE ret = new ASSOCIATION_REMOVE();
                if (data != null)
                {
                    int index = 2;
                    ret.groupingIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nodeId = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.nodeId.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](ASSOCIATION_REMOVE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ASSOCIATION_V2.ID);
                ret.Add(ID);
                if (command.groupingIdentifier.HasValue) ret.Add(command.groupingIdentifier);
                if (command.nodeId != null)
                {
                    foreach (var tmp in command.nodeId)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class ASSOCIATION_REPORT
        {
            public const byte ID = 0x03;
            public ByteValue groupingIdentifier = 0;
            public ByteValue maxNodesSupported = 0;
            public ByteValue reportsToFollow = 0;
            public IList<byte> nodeid = new List<byte>();
            public static implicit operator ASSOCIATION_REPORT(byte[] data)
            {
                ASSOCIATION_REPORT ret = new ASSOCIATION_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.groupingIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.maxNodesSupported = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.reportsToFollow = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nodeid = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.nodeid.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](ASSOCIATION_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ASSOCIATION_V2.ID);
                ret.Add(ID);
                if (command.groupingIdentifier.HasValue) ret.Add(command.groupingIdentifier);
                if (command.maxNodesSupported.HasValue) ret.Add(command.maxNodesSupported);
                if (command.reportsToFollow.HasValue) ret.Add(command.reportsToFollow);
                if (command.nodeid != null)
                {
                    foreach (var tmp in command.nodeid)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class ASSOCIATION_SET
        {
            public const byte ID = 0x01;
            public ByteValue groupingIdentifier = 0;
            public IList<byte> nodeId = new List<byte>();
            public static implicit operator ASSOCIATION_SET(byte[] data)
            {
                ASSOCIATION_SET ret = new ASSOCIATION_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.groupingIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nodeId = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.nodeId.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](ASSOCIATION_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ASSOCIATION_V2.ID);
                ret.Add(ID);
                if (command.groupingIdentifier.HasValue) ret.Add(command.groupingIdentifier);
                if (command.nodeId != null)
                {
                    foreach (var tmp in command.nodeId)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class ASSOCIATION_SPECIFIC_GROUP_GET
        {
            public const byte ID = 0x0B;
            public static implicit operator ASSOCIATION_SPECIFIC_GROUP_GET(byte[] data)
            {
                ASSOCIATION_SPECIFIC_GROUP_GET ret = new ASSOCIATION_SPECIFIC_GROUP_GET();
                return ret;
            }
            public static implicit operator byte[](ASSOCIATION_SPECIFIC_GROUP_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ASSOCIATION_V2.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class ASSOCIATION_SPECIFIC_GROUP_REPORT
        {
            public const byte ID = 0x0C;
            public ByteValue group = 0;
            public static implicit operator ASSOCIATION_SPECIFIC_GROUP_REPORT(byte[] data)
            {
                ASSOCIATION_SPECIFIC_GROUP_REPORT ret = new ASSOCIATION_SPECIFIC_GROUP_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.group = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](ASSOCIATION_SPECIFIC_GROUP_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ASSOCIATION_V2.ID);
                ret.Add(ID);
                if (command.group.HasValue) ret.Add(command.group);
                return ret.ToArray();
            }
        }
    }
}

