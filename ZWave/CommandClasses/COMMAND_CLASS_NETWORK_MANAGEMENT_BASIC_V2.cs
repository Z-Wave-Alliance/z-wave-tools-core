/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_NETWORK_MANAGEMENT_BASIC_V2
    {
        public const byte ID = 0x4D;
        public const byte VERSION = 2;
        public partial class LEARN_MODE_SET
        {
            public const byte ID = 0x01;
            public ByteValue seqNo = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte returnInterviewStatus
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 1 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0xFE; _value += (byte)(value << 1 & 0xFE); }
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
            public ByteValue mode = 0;
            public static implicit operator LEARN_MODE_SET(byte[] data)
            {
                LEARN_MODE_SET ret = new LEARN_MODE_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.mode = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](LEARN_MODE_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_BASIC_V2.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.properties1.HasValue) ret.Add(command.properties1);
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
            public ByteValue grantedKeys = 0;
            public ByteValue kexFailType = 0;
            public const byte dskBytesCount = 16;
            public byte[] dsk = new byte[dskBytesCount];
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
                    ret.grantedKeys = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.kexFailType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.dsk = (data.Length - index) >= dskBytesCount ? new byte[dskBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.dsk[0] = data[index++];
                    if (data.Length > index) ret.dsk[1] = data[index++];
                    if (data.Length > index) ret.dsk[2] = data[index++];
                    if (data.Length > index) ret.dsk[3] = data[index++];
                    if (data.Length > index) ret.dsk[4] = data[index++];
                    if (data.Length > index) ret.dsk[5] = data[index++];
                    if (data.Length > index) ret.dsk[6] = data[index++];
                    if (data.Length > index) ret.dsk[7] = data[index++];
                    if (data.Length > index) ret.dsk[8] = data[index++];
                    if (data.Length > index) ret.dsk[9] = data[index++];
                    if (data.Length > index) ret.dsk[10] = data[index++];
                    if (data.Length > index) ret.dsk[11] = data[index++];
                    if (data.Length > index) ret.dsk[12] = data[index++];
                    if (data.Length > index) ret.dsk[13] = data[index++];
                    if (data.Length > index) ret.dsk[14] = data[index++];
                    if (data.Length > index) ret.dsk[15] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](LEARN_MODE_SET_STATUS command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_BASIC_V2.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.status.HasValue) ret.Add(command.status);
                if (command.reserved.HasValue) ret.Add(command.reserved);
                if (command.newNodeId.HasValue) ret.Add(command.newNodeId);
                if (command.grantedKeys.HasValue) ret.Add(command.grantedKeys);
                if (command.kexFailType.HasValue) ret.Add(command.kexFailType);
                if (command.dsk != null)
                {
                    foreach (var tmp in command.dsk)
                    {
                        ret.Add(tmp);
                    }
                }
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
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_BASIC_V2.ID);
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
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_BASIC_V2.ID);
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
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_BASIC_V2.ID);
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
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_BASIC_V2.ID);
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
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_BASIC_V2.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.status.HasValue) ret.Add(command.status);
                return ret.ToArray();
            }
        }
        public partial class DSK_GET
        {
            public const byte ID = 0x08;
            public ByteValue seqNo = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte addMode
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 1 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0xFE; _value += (byte)(value << 1 & 0xFE); }
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
            public static implicit operator DSK_GET(byte[] data)
            {
                DSK_GET ret = new DSK_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](DSK_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_BASIC_V2.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                return ret.ToArray();
            }
        }
        public partial class DSK_REPORT
        {
            public const byte ID = 0x09;
            public ByteValue seqNo = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte addMode
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 1 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0xFE; _value += (byte)(value << 1 & 0xFE); }
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
            public const byte dskBytesCount = 16;
            public byte[] dsk = new byte[dskBytesCount];
            public static implicit operator DSK_REPORT(byte[] data)
            {
                DSK_REPORT ret = new DSK_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.seqNo = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.dsk = (data.Length - index) >= dskBytesCount ? new byte[dskBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.dsk[0] = data[index++];
                    if (data.Length > index) ret.dsk[1] = data[index++];
                    if (data.Length > index) ret.dsk[2] = data[index++];
                    if (data.Length > index) ret.dsk[3] = data[index++];
                    if (data.Length > index) ret.dsk[4] = data[index++];
                    if (data.Length > index) ret.dsk[5] = data[index++];
                    if (data.Length > index) ret.dsk[6] = data[index++];
                    if (data.Length > index) ret.dsk[7] = data[index++];
                    if (data.Length > index) ret.dsk[8] = data[index++];
                    if (data.Length > index) ret.dsk[9] = data[index++];
                    if (data.Length > index) ret.dsk[10] = data[index++];
                    if (data.Length > index) ret.dsk[11] = data[index++];
                    if (data.Length > index) ret.dsk[12] = data[index++];
                    if (data.Length > index) ret.dsk[13] = data[index++];
                    if (data.Length > index) ret.dsk[14] = data[index++];
                    if (data.Length > index) ret.dsk[15] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](DSK_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NETWORK_MANAGEMENT_BASIC_V2.ID);
                ret.Add(ID);
                if (command.seqNo.HasValue) ret.Add(command.seqNo);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.dsk != null)
                {
                    foreach (var tmp in command.dsk)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

