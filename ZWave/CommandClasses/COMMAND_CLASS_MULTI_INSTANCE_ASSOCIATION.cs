/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_MULTI_INSTANCE_ASSOCIATION
    {
        public const byte ID = 0x8E;
        public const byte VERSION = 1;
        public partial class MULTI_INSTANCE_ASSOCIATION_GET
        {
            public const byte ID = 0x02;
            public ByteValue groupingIdentifier = 0;
            public static implicit operator MULTI_INSTANCE_ASSOCIATION_GET(byte[] data)
            {
                MULTI_INSTANCE_ASSOCIATION_GET ret = new MULTI_INSTANCE_ASSOCIATION_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.groupingIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](MULTI_INSTANCE_ASSOCIATION_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_MULTI_INSTANCE_ASSOCIATION.ID);
                ret.Add(ID);
                if (command.groupingIdentifier.HasValue) ret.Add(command.groupingIdentifier);
                return ret.ToArray();
            }
        }
        public partial class MULTI_INSTANCE_ASSOCIATION_GROUPINGS_GET
        {
            public const byte ID = 0x05;
            public static implicit operator MULTI_INSTANCE_ASSOCIATION_GROUPINGS_GET(byte[] data)
            {
                MULTI_INSTANCE_ASSOCIATION_GROUPINGS_GET ret = new MULTI_INSTANCE_ASSOCIATION_GROUPINGS_GET();
                return ret;
            }
            public static implicit operator byte[](MULTI_INSTANCE_ASSOCIATION_GROUPINGS_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_MULTI_INSTANCE_ASSOCIATION.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class MULTI_INSTANCE_ASSOCIATION_GROUPINGS_REPORT
        {
            public const byte ID = 0x06;
            public ByteValue supportedGroupings = 0;
            public static implicit operator MULTI_INSTANCE_ASSOCIATION_GROUPINGS_REPORT(byte[] data)
            {
                MULTI_INSTANCE_ASSOCIATION_GROUPINGS_REPORT ret = new MULTI_INSTANCE_ASSOCIATION_GROUPINGS_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.supportedGroupings = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](MULTI_INSTANCE_ASSOCIATION_GROUPINGS_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_MULTI_INSTANCE_ASSOCIATION.ID);
                ret.Add(ID);
                if (command.supportedGroupings.HasValue) ret.Add(command.supportedGroupings);
                return ret.ToArray();
            }
        }
        public partial class MULTI_INSTANCE_ASSOCIATION_REMOVE
        {
            public const byte ID = 0x04;
            public ByteValue groupingIdentifier = 0;
            public IList<byte> nodeId = new List<byte>();
            private byte[] marker = {0x00};
            public class TVG
            {
                public ByteValue multiInstanceNodeId = 0;
                public ByteValue instance = 0;
            }
            public List<TVG> vg = new List<TVG>();
            public static implicit operator MULTI_INSTANCE_ASSOCIATION_REMOVE(byte[] data)
            {
                MULTI_INSTANCE_ASSOCIATION_REMOVE ret = new MULTI_INSTANCE_ASSOCIATION_REMOVE();
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
                    for (int j = 0; j < ret.groupingIdentifier; j++)
                    {
                        TVG tmp = new TVG();
                        tmp.multiInstanceNodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.instance = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        ret.vg.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](MULTI_INSTANCE_ASSOCIATION_REMOVE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_MULTI_INSTANCE_ASSOCIATION.ID);
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
                        if (item.multiInstanceNodeId.HasValue) ret.Add(item.multiInstanceNodeId);
                        if (item.instance.HasValue) ret.Add(item.instance);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class MULTI_INSTANCE_ASSOCIATION_REPORT
        {
            public const byte ID = 0x03;
            public ByteValue groupingIdentifier = 0;
            public ByteValue maxNodesSupported = 0;
            public ByteValue reportsToFollow = 0;
            public IList<byte> nodeId = new List<byte>();
            private byte[] marker = {0x00};
            public class TVG
            {
                public ByteValue multiInstanceNodeId = 0;
                public ByteValue instance = 0;
            }
            public List<TVG> vg = new List<TVG>();
            public static implicit operator MULTI_INSTANCE_ASSOCIATION_REPORT(byte[] data)
            {
                MULTI_INSTANCE_ASSOCIATION_REPORT ret = new MULTI_INSTANCE_ASSOCIATION_REPORT();
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
                        tmp.multiInstanceNodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.instance = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        ret.vg.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](MULTI_INSTANCE_ASSOCIATION_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_MULTI_INSTANCE_ASSOCIATION.ID);
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
                        if (item.multiInstanceNodeId.HasValue) ret.Add(item.multiInstanceNodeId);
                        if (item.instance.HasValue) ret.Add(item.instance);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class MULTI_INSTANCE_ASSOCIATION_SET
        {
            public const byte ID = 0x01;
            public ByteValue groupingIdentifier = 0;
            public IList<byte> nodeId = new List<byte>();
            private byte[] marker = {0x00};
            public class TVG
            {
                public ByteValue multiInstanceNodeId = 0;
                public ByteValue instance = 0;
            }
            public List<TVG> vg = new List<TVG>();
            public static implicit operator MULTI_INSTANCE_ASSOCIATION_SET(byte[] data)
            {
                MULTI_INSTANCE_ASSOCIATION_SET ret = new MULTI_INSTANCE_ASSOCIATION_SET();
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
                    for (int j = 0; j < ret.groupingIdentifier; j++)
                    {
                        TVG tmp = new TVG();
                        tmp.multiInstanceNodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.instance = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        ret.vg.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](MULTI_INSTANCE_ASSOCIATION_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_MULTI_INSTANCE_ASSOCIATION.ID);
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
                        if (item.multiInstanceNodeId.HasValue) ret.Add(item.multiInstanceNodeId);
                        if (item.instance.HasValue) ret.Add(item.instance);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

