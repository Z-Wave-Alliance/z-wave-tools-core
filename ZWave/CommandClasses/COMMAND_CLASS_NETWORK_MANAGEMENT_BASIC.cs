/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_NETWORK_MANAGEMENT_BASIC
    {
        public const byte ID = 0x4D;
        public const byte VERSION = 1;
        public partial class LEARN_MODE_SET
        {
            public const byte ID = 0x01;
            public ByteValue seqNo = 0;
            public ByteValue reserved = 0;
            public ByteValue mode = 0;
            public static implicit operator LEARN_MODE_SET(byte[] data)
            {
                LEARN_MODE_SET ret = new LEARN_MODE_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.reserved = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.mode = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](LEARN_MODE_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_BASIC.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.reserved.HasValue) ret.Add(command.reserved);
                if (command.mode.HasValue) ret.Add(command.mode);
                return ret.ToArray();
            }
        }
        public partial class LEARN_MODE_SET_STATUS
        {
            public const byte ID = 0x02;
            public ByteValue seqNo = 0;
            public ByteValue status = 0;
            public ByteValue reserved = 0;
            public ByteValue newNodeId = 0;
            public static implicit operator LEARN_MODE_SET_STATUS(byte[] data)
            {
                LEARN_MODE_SET_STATUS ret = new LEARN_MODE_SET_STATUS();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.status = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.reserved = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.newNodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](LEARN_MODE_SET_STATUS command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_BASIC.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.status.HasValue) ret.Add(command.status);
                if (command.reserved.HasValue) ret.Add(command.reserved);
                if (command.newNodeId.HasValue) ret.Add(command.newNodeId);
                return ret.ToArray();
            }
        }
        public partial class NODE_INFORMATION_SEND
        {
            public const byte ID = 0x05;
            public ByteValue seqNo = 0;
            public ByteValue reserved = 0;
            public ByteValue destinationNodeId = 0;
            public ByteValue txOptions = 0;
            public static implicit operator NODE_INFORMATION_SEND(byte[] data)
            {
                NODE_INFORMATION_SEND ret = new NODE_INFORMATION_SEND();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.reserved = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.destinationNodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.txOptions = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](NODE_INFORMATION_SEND command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_BASIC.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.reserved.HasValue) ret.Add(command.reserved);
                if (command.destinationNodeId.HasValue) ret.Add(command.destinationNodeId);
                if (command.txOptions.HasValue) ret.Add(command.txOptions);
                return ret.ToArray();
            }
        }
        public partial class NETWORK_UPDATE_REQUEST
        {
            public const byte ID = 0x03;
            public ByteValue seqNo = 0;
            public static implicit operator NETWORK_UPDATE_REQUEST(byte[] data)
            {
                NETWORK_UPDATE_REQUEST ret = new NETWORK_UPDATE_REQUEST();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](NETWORK_UPDATE_REQUEST command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_BASIC.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                return ret.ToArray();
            }
        }
        public partial class NETWORK_UPDATE_REQUEST_STATUS
        {
            public const byte ID = 0x04;
            public ByteValue seqNo = 0;
            public ByteValue status = 0;
            public static implicit operator NETWORK_UPDATE_REQUEST_STATUS(byte[] data)
            {
                NETWORK_UPDATE_REQUEST_STATUS ret = new NETWORK_UPDATE_REQUEST_STATUS();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.status = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](NETWORK_UPDATE_REQUEST_STATUS command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_BASIC.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.status.HasValue) ret.Add(command.status);
                return ret.ToArray();
            }
        }
        public partial class DEFAULT_SET
        {
            public const byte ID = 0x06;
            public ByteValue seqNo = 0;
            public static implicit operator DEFAULT_SET(byte[] data)
            {
                DEFAULT_SET ret = new DEFAULT_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](DEFAULT_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_BASIC.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                return ret.ToArray();
            }
        }
        public partial class DEFAULT_SET_COMPLETE
        {
            public const byte ID = 0x07;
            public ByteValue seqNo = 0;
            public ByteValue status = 0;
            public static implicit operator DEFAULT_SET_COMPLETE(byte[] data)
            {
                DEFAULT_SET_COMPLETE ret = new DEFAULT_SET_COMPLETE();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.status = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](DEFAULT_SET_COMPLETE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_BASIC.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.status.HasValue) ret.Add(command.status);
                return ret.ToArray();
            }
        }
    }
}

