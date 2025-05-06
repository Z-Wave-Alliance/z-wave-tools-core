/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;

namespace ZWave.Devices
{
    public struct NodeTag : IEquatable<NodeTag>, IComparable<NodeTag>, IComparable, ICloneable
    {
        public ushort Id;
        public byte EndPointId;
        public bool IsBitAddress;

        public NodeTag(ushort node, byte endpoint, bool isBitAddress)
        {
            Id = node;
            EndPointId = endpoint;
            IsBitAddress = isBitAddress;
        }

        public NodeTag(ushort node, byte endpoint)
        {
            Id = node;
            EndPointId = endpoint;
            IsBitAddress = false;
        }

        public NodeTag(ushort nodeId)
        {
            Id = nodeId;
            EndPointId = 0;
            IsBitAddress = false;
        }

        public NodeTag Parent
        {
            get { return new NodeTag(Id); }
        }

        public static NodeTag FF = new NodeTag(0xFF);
        public static NodeTag Empty = new NodeTag(0);

        public override int GetHashCode()
        {
            return (Id << 8) + EndPointId;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is NodeTag))
                return false;

            NodeTag nodeId = (NodeTag)obj;
            return Equals(nodeId);
        }

        public bool Equals(NodeTag other)
        {
            return Id == other.Id && EndPointId == other.EndPointId;
        }

        public static bool operator !=(NodeTag nodeId1, NodeTag nodeId2)
        {
            return !nodeId1.Equals(nodeId2);
        }

        public static bool operator ==(NodeTag nodeId1, NodeTag nodeId2)
        {
            return nodeId1.Equals(nodeId2);
        }

        //public static implicit operator NodeTag(byte nodeId)
        //{
        //    return new NodeTag(nodeId);
        //}

        public override string ToString()
        {
            if (EndPointId == 0)
            {
                return Id.ToString();
            }
            else
            {
                return Id.ToString() + "." + EndPointId.ToString();
            }
        }

        public int CompareTo(NodeTag other)
        {
            if (Id < other.Id)
                return -1;
            else if (Id > other.Id)
                return 1;
            else if (EndPointId < other.EndPointId)
                return -1;
            else if (EndPointId > other.EndPointId)
                return 1;
            else
                return 0;
        }

        public int CompareTo(object obj)
        {
            if (!(obj is NodeTag))
                return 0;

            NodeTag nodeId = (NodeTag)obj;
            return CompareTo(nodeId);
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
