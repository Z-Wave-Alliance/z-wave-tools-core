/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using Utils;

namespace ZWave.Security
{
    internal struct NonceS0
    {
        public NonceS0(byte nodeId, byte[] value)
        {
            mId = value[0];
            mNodeId = nodeId;
            mCreatedAt = DateTime.Now;
            mValue = 0;
            Value = value;
        }

        private byte mId;
        public byte Id
        {
            get { return mId; }
            set { mId = value; }
        }

        private byte mNodeId;
        public byte NodeId
        {
            get { return mNodeId; }
            set { mNodeId = value; }
        }

        private DateTime mCreatedAt;
        public DateTime CreatedAt
        {
            get { return mCreatedAt; }
            set { mCreatedAt = value; }
        }

        private ulong mValue;
        public byte[] Value
        {
            get { return BitConverter.GetBytes(mValue); }
            set { mValue = BitConverter.ToUInt64(value, 0); }
        }

        public ushort Key
        {
            get { return GetKey(NodeId, Id); }
        }

        public override string ToString()
        {
            return Tools.FormatStr("{0}:{1} <{2}>", mNodeId, Value.GetHex(), CreatedAt.ToString("HH:mm:ss:fff"));
        }

        public static ushort GetKey(byte nodeId, byte id)
        {
            return (ushort)((nodeId << 8) + id);
        }

    }
}
