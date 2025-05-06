/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using ZWave.Devices;

namespace ZWave.Security
{
    public class SecurityManagerBase
    {
    }

    public struct NodeGroupId : IEquatable<NodeGroupId>
    {
        public NodeTag Node;
        public byte GroupId;
        public NodeGroupId(NodeTag node, byte groupId)
        {
            Node = node;
            GroupId = groupId;
        }

        public NodeGroupId(int value)
        {
            Node = new NodeTag((byte)(value >> 8));
            GroupId = (byte)value;
        }

        public bool Equals(NodeGroupId other)
        {
            return Node == other.Node && GroupId == other.GroupId;
        }
    }

    public struct OrdinalPeerNodeId : IEquatable<OrdinalPeerNodeId>
    {
        public NodeTag NodeId1;
        public NodeTag NodeId2;

        public OrdinalPeerNodeId(NodeTag node1, NodeTag node2)
        {
            NodeId1 = node1;
            NodeId2 = node2;
        }

        public bool Equals(OrdinalPeerNodeId other)
        {
            return GetHashCode() == other.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is OrdinalPeerNodeId))
                return false;

            return Equals((OrdinalPeerNodeId)obj);
        }

        public override int GetHashCode()
        {
            return (NodeId1.Id << 16) + NodeId2.Id;
        }

        public override string ToString()
        {
            return string.Format("{0}.{1}", NodeId1, NodeId2);
        }
    }

    public struct InvariantPeerNodeId : IEquatable<InvariantPeerNodeId>
    {
        public NodeTag NodeId1;
        public NodeTag NodeId2;

        public InvariantPeerNodeId(NodeTag node1, NodeTag node2)
        {
            NodeId1 = node1;
            NodeId2 = node2;
        }

        public bool Equals(InvariantPeerNodeId other)
        {
            return GetHashCode() == other.GetHashCode();
        }

        public bool IsEmpty
        {
            get { return NodeId1.Id == 0 && NodeId2.Id == 0; }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is InvariantPeerNodeId))
                return false;

            return Equals((InvariantPeerNodeId)obj);
        }

        public override int GetHashCode()
        {
            if (NodeId1.Id > NodeId2.Id)
            {
                return (NodeId1.Id << 16) + NodeId2.Id;
            }
            else
            {
                return (NodeId2.Id << 16) + NodeId1.Id;
            }
        }

        public override string ToString()
        {
            return string.Format("{0}.{1}", NodeId1, NodeId2);
        }
    }
}
