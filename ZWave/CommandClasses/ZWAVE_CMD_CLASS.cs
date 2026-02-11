/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class ZWAVE_CMD_CLASS
    {
        public const byte ID = 0x01;
        public const byte VERSION = 1;
        public partial class ACCEPT_LOST
        {
            public const byte ID = 0x17;
            public ByteValue acceptStatus = 0;
            public static implicit operator ACCEPT_LOST(byte[] data)
            {
                ACCEPT_LOST ret = new ACCEPT_LOST();
                if (data != null)
                {
                    int index = 2;
                    ret.acceptStatus = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](ACCEPT_LOST command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(ZWAVE_CMD_CLASS.ID);
                ret.Add(ID);
                if (command.acceptStatus.HasValue) ret.Add(command.acceptStatus);
                return ret.ToArray();
            }
        }
        public partial class ASSIGN_ID
        {
            public const byte ID = 0x03;
            public ByteValue newNodeId = 0;
            public const byte newHomeIdBytesCount = 4;
            public byte[] newHomeId = new byte[newHomeIdBytesCount];
            public static implicit operator ASSIGN_ID(byte[] data)
            {
                ASSIGN_ID ret = new ASSIGN_ID();
                if (data != null)
                {
                    int index = 2;
                    ret.newNodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.newHomeId = (data.Length - index) >= newHomeIdBytesCount ? new byte[newHomeIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.newHomeId[0] = data[index++];
                    if (data.Length > index) ret.newHomeId[1] = data[index++];
                    if (data.Length > index) ret.newHomeId[2] = data[index++];
                    if (data.Length > index) ret.newHomeId[3] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](ASSIGN_ID command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(ZWAVE_CMD_CLASS.ID);
                ret.Add(ID);
                if (command.newNodeId.HasValue) ret.Add(command.newNodeId);
                if (command.newHomeId != null)
                {
                    foreach (var tmp in command.newHomeId)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class ASSIGN_RETURN_ROUTE
        {
            public const byte ID = 0x0C;
            public ByteValue nodeid = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte numberOfHops
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte routeNumber
                {
                    get { return (byte)(_value >> 4 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0xF0; _value += (byte)(value << 4 & 0xF0); }
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
            public IList<byte> repeaters = new List<byte>();
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte reserved1
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte destinationWakeUp
                {
                    get { return (byte)(_value >> 1 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x06; _value += (byte)(value << 1 & 0x06); }
                }
                public byte destinationSpeed
                {
                    get { return (byte)(_value >> 3 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x38; _value += (byte)(value << 3 & 0x38); }
                }
                public byte reserved2
                {
                    get { return (byte)(_value >> 6 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0xC0; _value += (byte)(value << 6 & 0xC0); }
                }
                public static implicit operator Tproperties2(byte data)
                {
                    Tproperties2 ret = new Tproperties2();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties2 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties2 properties2 = 0;
            public static implicit operator ASSIGN_RETURN_ROUTE(byte[] data)
            {
                ASSIGN_RETURN_ROUTE ret = new ASSIGN_RETURN_ROUTE();
                if (data != null)
                {
                    int index = 2;
                    ret.nodeid = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.repeaters = new List<byte>();
                    for (int i = 0; i < ret.properties1.numberOfHops; i++)
                    {
                        if (data.Length > index) ret.repeaters.Add(data[index++]);
                    }
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](ASSIGN_RETURN_ROUTE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(ZWAVE_CMD_CLASS.ID);
                ret.Add(ID);
                if (command.nodeid.HasValue) ret.Add(command.nodeid);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.repeaters != null)
                {
                    foreach (var tmp in command.repeaters)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties2.HasValue) ret.Add(command.properties2);
                return ret.ToArray();
            }
        }
        public partial class CMD_ASSIGN_SUC_RETURN_ROUTE
        {
            public const byte ID = 0x14;
            public ByteValue nodeid = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte numberOfHops
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte routeNumber
                {
                    get { return (byte)(_value >> 4 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0xF0; _value += (byte)(value << 4 & 0xF0); }
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
            public IList<byte> repeaters = new List<byte>();
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte reserved1
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte destinationWakeUp
                {
                    get { return (byte)(_value >> 1 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x06; _value += (byte)(value << 1 & 0x06); }
                }
                public byte destinationSpeed
                {
                    get { return (byte)(_value >> 3 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x38; _value += (byte)(value << 3 & 0x38); }
                }
                public byte reserved2
                {
                    get { return (byte)(_value >> 6 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0xC0; _value += (byte)(value << 6 & 0xC0); }
                }
                public static implicit operator Tproperties2(byte data)
                {
                    Tproperties2 ret = new Tproperties2();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties2 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties2 properties2 = 0;
            public static implicit operator CMD_ASSIGN_SUC_RETURN_ROUTE(byte[] data)
            {
                CMD_ASSIGN_SUC_RETURN_ROUTE ret = new CMD_ASSIGN_SUC_RETURN_ROUTE();
                if (data != null)
                {
                    int index = 2;
                    ret.nodeid = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.repeaters = new List<byte>();
                    for (int i = 0; i < ret.properties1.numberOfHops; i++)
                    {
                        if (data.Length > index) ret.repeaters.Add(data[index++]);
                    }
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](CMD_ASSIGN_SUC_RETURN_ROUTE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(ZWAVE_CMD_CLASS.ID);
                ret.Add(ID);
                if (command.nodeid.HasValue) ret.Add(command.nodeid);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.repeaters != null)
                {
                    foreach (var tmp in command.repeaters)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties2.HasValue) ret.Add(command.properties2);
                return ret.ToArray();
            }
        }
        public partial class CMD_AUTOMATIC_CONTROLLER_UPDATE_START
        {
            public const byte ID = 0x10;
            public static implicit operator CMD_AUTOMATIC_CONTROLLER_UPDATE_START(byte[] data)
            {
                CMD_AUTOMATIC_CONTROLLER_UPDATE_START ret = new CMD_AUTOMATIC_CONTROLLER_UPDATE_START();
                return ret;
            }
            public static implicit operator byte[](CMD_AUTOMATIC_CONTROLLER_UPDATE_START command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(ZWAVE_CMD_CLASS.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class CMD_NODES_EXIST
        {
            public const byte ID = 0x1F;
            public ByteValue nodeMaskType = 0;
            public ByteValue numNodeMask = 0;
            public IList<byte> nodeMask = new List<byte>();
            public static implicit operator CMD_NODES_EXIST(byte[] data)
            {
                CMD_NODES_EXIST ret = new CMD_NODES_EXIST();
                if (data != null)
                {
                    int index = 2;
                    ret.nodeMaskType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.numNodeMask = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nodeMask = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.nodeMask.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](CMD_NODES_EXIST command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(ZWAVE_CMD_CLASS.ID);
                ret.Add(ID);
                if (command.nodeMaskType.HasValue) ret.Add(command.nodeMaskType);
                if (command.numNodeMask.HasValue) ret.Add(command.numNodeMask);
                if (command.nodeMask != null)
                {
                    foreach (var tmp in command.nodeMask)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class CMD_NODES_EXIST_REPLY
        {
            public const byte ID = 0x20;
            public ByteValue nodeMaskType = 0;
            public ByteValue status = 0;
            public static implicit operator CMD_NODES_EXIST_REPLY(byte[] data)
            {
                CMD_NODES_EXIST_REPLY ret = new CMD_NODES_EXIST_REPLY();
                if (data != null)
                {
                    int index = 2;
                    ret.nodeMaskType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.status = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](CMD_NODES_EXIST_REPLY command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(ZWAVE_CMD_CLASS.ID);
                ret.Add(ID);
                if (command.nodeMaskType.HasValue) ret.Add(command.nodeMaskType);
                if (command.status.HasValue) ret.Add(command.status);
                return ret.ToArray();
            }
        }
        public partial class CMD_SET_NWI_MODE
        {
            public const byte ID = 0x22;
            public ByteValue mode = 0;
            public ByteValue timeout = 0;
            public static implicit operator CMD_SET_NWI_MODE(byte[] data)
            {
                CMD_SET_NWI_MODE ret = new CMD_SET_NWI_MODE();
                if (data != null)
                {
                    int index = 2;
                    ret.mode = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.timeout = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](CMD_SET_NWI_MODE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(ZWAVE_CMD_CLASS.ID);
                ret.Add(ID);
                if (command.mode.HasValue) ret.Add(command.mode);
                if (command.timeout.HasValue) ret.Add(command.timeout);
                return ret.ToArray();
            }
        }
        public partial class COMMAND_COMPLETE
        {
            public const byte ID = 0x07;
            public ByteValue sequenceNumber = 0;
            public static implicit operator COMMAND_COMPLETE(byte[] data)
            {
                COMMAND_COMPLETE ret = new COMMAND_COMPLETE();
                if (data != null)
                {
                    int index = 2;
                    ret.sequenceNumber = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](COMMAND_COMPLETE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(ZWAVE_CMD_CLASS.ID);
                ret.Add(ID);
                if (command.sequenceNumber.HasValue) ret.Add(command.sequenceNumber);
                return ret.ToArray();
            }
        }
        public partial class FIND_NODES_IN_RANGE
        {
            public const byte ID = 0x04;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte numberOfMaskBytes
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 5 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x60; _value += (byte)(value << 5 & 0x60); }
                }
                public byte speedPresent
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
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
            public IList<byte> maskByteS = new List<byte>();
            public ByteValue wakeUpTimeOptionalRx = 0;
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte dataRateOptionalRx
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 3 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0xF8; _value += (byte)(value << 3 & 0xF8); }
                }
                public static implicit operator Tproperties2(byte data)
                {
                    Tproperties2 ret = new Tproperties2();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties2 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties2 properties2 = 0;
            public static implicit operator FIND_NODES_IN_RANGE(byte[] data)
            {
                FIND_NODES_IN_RANGE ret = new FIND_NODES_IN_RANGE();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.maskByteS = new List<byte>();
                    for (int i = 0; i < ret.properties1.numberOfMaskBytes; i++)
                    {
                        if (data.Length > index) ret.maskByteS.Add(data[index++]);
                    }
                    ret.wakeUpTimeOptionalRx = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](FIND_NODES_IN_RANGE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(ZWAVE_CMD_CLASS.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.maskByteS != null)
                {
                    foreach (var tmp in command.maskByteS)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.wakeUpTimeOptionalRx.HasValue) ret.Add(command.wakeUpTimeOptionalRx);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                return ret.ToArray();
            }
        }
        public partial class GET_NODES_IN_RANGE
        {
            public const byte ID = 0x05;
            public ByteValue wakeUpTimeOptionalRx = 0;
            public static implicit operator GET_NODES_IN_RANGE(byte[] data)
            {
                GET_NODES_IN_RANGE ret = new GET_NODES_IN_RANGE();
                if (data != null)
                {
                    int index = 2;
                    ret.wakeUpTimeOptionalRx = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](GET_NODES_IN_RANGE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(ZWAVE_CMD_CLASS.ID);
                ret.Add(ID);
                if (command.wakeUpTimeOptionalRx.HasValue) ret.Add(command.wakeUpTimeOptionalRx);
                return ret.ToArray();
            }
        }
        public partial class LOST
        {
            public const byte ID = 0x16;
            public ByteValue nodeId = 0;
            public static implicit operator LOST(byte[] data)
            {
                LOST ret = new LOST();
                if (data != null)
                {
                    int index = 2;
                    ret.nodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](LOST command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(ZWAVE_CMD_CLASS.ID);
                ret.Add(ID);
                if (command.nodeId.HasValue) ret.Add(command.nodeId);
                return ret.ToArray();
            }
        }
        public partial class NEW_NODE_REGISTERED
        {
            public const byte ID = 0x0D;
            public ByteValue nodeId = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte protocolVersion
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte supportedSpeed96Kbps
                {
                    get { return (byte)(_value >> 3 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x08; _value += (byte)(value << 3 & 0x08); }
                }
                public byte supportedSpeed40Kbps
                {
                    get { return (byte)(_value >> 4 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x10; _value += (byte)(value << 4 & 0x10); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 5 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x20; _value += (byte)(value << 5 & 0x20); }
                }
                public byte routing
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte listening
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
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
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte reserved2
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte controller
                {
                    get { return (byte)(_value >> 1 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x02; _value += (byte)(value << 1 & 0x02); }
                }
                public byte specificDevice
                {
                    get { return (byte)(_value >> 2 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x04; _value += (byte)(value << 2 & 0x04); }
                }
                public byte routingEndNode
                {
                    get { return (byte)(_value >> 3 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x08; _value += (byte)(value << 3 & 0x08); }
                }
                public byte beamCapability
                {
                    get { return (byte)(_value >> 4 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x10; _value += (byte)(value << 4 & 0x10); }
                }
                public byte sensor250ms
                {
                    get { return (byte)(_value >> 5 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x20; _value += (byte)(value << 5 & 0x20); }
                }
                public byte sensor1000ms
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte optionalFunctionality
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties2(byte data)
                {
                    Tproperties2 ret = new Tproperties2();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties2 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties2 properties2 = 0;
            public struct Tproperties3
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties3 Empty { get { return new Tproperties3() { _value = 0, HasValue = false }; } }
                public byte speedExtension100Kbps
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte speedExtensionReserved
                {
                    get { return (byte)(_value >> 1 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x06; _value += (byte)(value << 1 & 0x06); }
                }
                public byte reserved3
                {
                    get { return (byte)(_value >> 3 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0xF8; _value += (byte)(value << 3 & 0xF8); }
                }
                public static implicit operator Tproperties3(byte data)
                {
                    Tproperties3 ret = new Tproperties3();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties3 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties3 properties3 = 0;
            public ByteValue basicDeviceClass = 0;
            public ByteValue genericDeviceClass = 0;
            public ByteValue specificDeviceClass = 0;
            public IList<byte> commandClasses = new List<byte>();
            public static implicit operator NEW_NODE_REGISTERED(byte[] data)
            {
                NEW_NODE_REGISTERED ret = new NEW_NODE_REGISTERED();
                if (data != null)
                {
                    int index = 2;
                    ret.nodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.properties3 = data.Length > index ? (Tproperties3)data[index++] : Tproperties3.Empty;
                    if (ret.properties2.controller > 0)
                    {
                        ret.basicDeviceClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    }
                    ret.genericDeviceClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    if (ret.properties2.specificDevice > 0)
                    {
                        ret.specificDeviceClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    }
                    ret.commandClasses = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.commandClasses.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](NEW_NODE_REGISTERED command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(ZWAVE_CMD_CLASS.ID);
                ret.Add(ID);
                if (command.nodeId.HasValue) ret.Add(command.nodeId);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.properties3.HasValue) ret.Add(command.properties3);
                if (command.properties2.controller > 0)
                {
                    if (command.basicDeviceClass.HasValue) ret.Add(command.basicDeviceClass);
                }
                if (command.genericDeviceClass.HasValue) ret.Add(command.genericDeviceClass);
                if (command.properties2.specificDevice > 0)
                {
                    if (command.specificDeviceClass.HasValue) ret.Add(command.specificDeviceClass);
                }
                if (command.commandClasses != null)
                {
                    foreach (var tmp in command.commandClasses)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class NEW_RANGE_REGISTERED
        {
            public const byte ID = 0x0E;
            public ByteValue nodeId = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte numberOfMaskBytes
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 5 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0xE0; _value += (byte)(value << 5 & 0xE0); }
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
            public IList<byte> maskByteS = new List<byte>();
            public static implicit operator NEW_RANGE_REGISTERED(byte[] data)
            {
                NEW_RANGE_REGISTERED ret = new NEW_RANGE_REGISTERED();
                if (data != null)
                {
                    int index = 2;
                    ret.nodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.maskByteS = new List<byte>();
                    for (int i = 0; i < ret.properties1.numberOfMaskBytes; i++)
                    {
                        if (data.Length > index) ret.maskByteS.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](NEW_RANGE_REGISTERED command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(ZWAVE_CMD_CLASS.ID);
                ret.Add(ID);
                if (command.nodeId.HasValue) ret.Add(command.nodeId);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.maskByteS != null)
                {
                    foreach (var tmp in command.maskByteS)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class NODE_INFO
        {
            public const byte ID = 0x01;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte protocolVersion
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte supportedSpeed96Kbps
                {
                    get { return (byte)(_value >> 3 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x08; _value += (byte)(value << 3 & 0x08); }
                }
                public byte supportedSpeed40Kbps
                {
                    get { return (byte)(_value >> 4 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x10; _value += (byte)(value << 4 & 0x10); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 5 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x20; _value += (byte)(value << 5 & 0x20); }
                }
                public byte routing
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte listening
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
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
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte reserved2
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte controller
                {
                    get { return (byte)(_value >> 1 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x02; _value += (byte)(value << 1 & 0x02); }
                }
                public byte specificDevice
                {
                    get { return (byte)(_value >> 2 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x04; _value += (byte)(value << 2 & 0x04); }
                }
                public byte routingEndNode
                {
                    get { return (byte)(_value >> 3 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x08; _value += (byte)(value << 3 & 0x08); }
                }
                public byte beamCapability
                {
                    get { return (byte)(_value >> 4 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x10; _value += (byte)(value << 4 & 0x10); }
                }
                public byte sensor250ms
                {
                    get { return (byte)(_value >> 5 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x20; _value += (byte)(value << 5 & 0x20); }
                }
                public byte sensor1000ms
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte optionalFunctionality
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties2(byte data)
                {
                    Tproperties2 ret = new Tproperties2();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties2 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties2 properties2 = 0;
            public struct Tproperties3
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties3 Empty { get { return new Tproperties3() { _value = 0, HasValue = false }; } }
                public byte speedExtension100Kbps
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte speedExtensionReserved
                {
                    get { return (byte)(_value >> 1 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x06; _value += (byte)(value << 1 & 0x06); }
                }
                public byte reserved3
                {
                    get { return (byte)(_value >> 3 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0xF8; _value += (byte)(value << 3 & 0xF8); }
                }
                public static implicit operator Tproperties3(byte data)
                {
                    Tproperties3 ret = new Tproperties3();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties3 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties3 properties3 = 0;
            public ByteValue basicDeviceClass = 0;
            public ByteValue genericDeviceClass = 0;
            public ByteValue specificDeviceClass = 0;
            public IList<byte> commandClasses = new List<byte>();
            public static implicit operator NODE_INFO(byte[] data)
            {
                NODE_INFO ret = new NODE_INFO();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.properties3 = data.Length > index ? (Tproperties3)data[index++] : Tproperties3.Empty;
                    if (ret.properties2.controller > 0)
                    {
                        ret.basicDeviceClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    }
                    ret.genericDeviceClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    if (ret.properties2.specificDevice > 0)
                    {
                        ret.specificDeviceClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    }
                    ret.commandClasses = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.commandClasses.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](NODE_INFO command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(ZWAVE_CMD_CLASS.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.properties3.HasValue) ret.Add(command.properties3);
                if (command.properties2.controller > 0)
                {
                    if (command.basicDeviceClass.HasValue) ret.Add(command.basicDeviceClass);
                }
                if (command.genericDeviceClass.HasValue) ret.Add(command.genericDeviceClass);
                if (command.properties2.specificDevice > 0)
                {
                    if (command.specificDeviceClass.HasValue) ret.Add(command.specificDeviceClass);
                }
                if (command.commandClasses != null)
                {
                    foreach (var tmp in command.commandClasses)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class NODE_RANGE_INFO
        {
            public const byte ID = 0x06;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte numberOfMaskBytes
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 5 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0xE0; _value += (byte)(value << 5 & 0xE0); }
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
            public IList<byte> maskByteS = new List<byte>();
            public ByteValue wakeUpTime = 0;
            public static implicit operator NODE_RANGE_INFO(byte[] data)
            {
                NODE_RANGE_INFO ret = new NODE_RANGE_INFO();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.maskByteS = new List<byte>();
                    for (int i = 0; i < ret.properties1.numberOfMaskBytes; i++)
                    {
                        if (data.Length > index) ret.maskByteS.Add(data[index++]);
                    }
                    ret.wakeUpTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](NODE_RANGE_INFO command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(ZWAVE_CMD_CLASS.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.maskByteS != null)
                {
                    foreach (var tmp in command.maskByteS)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.wakeUpTime.HasValue) ret.Add(command.wakeUpTime);
                return ret.ToArray();
            }
        }
        public partial class ZWAVE_CMD_NOP
        {
            public const byte ID = 0x00;
            public static implicit operator ZWAVE_CMD_NOP(byte[] data)
            {
                ZWAVE_CMD_NOP ret = new ZWAVE_CMD_NOP();
                return ret;
            }
            public static implicit operator byte[](ZWAVE_CMD_NOP command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(ZWAVE_CMD_CLASS.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class CMD_NOP_POWER
        {
            public const byte ID = 0x18;
            public ByteValue txPowerRegisterObsolete = 0;
            public ByteValue txPowerDampening = 0;
            public static implicit operator CMD_NOP_POWER(byte[] data)
            {
                CMD_NOP_POWER ret = new CMD_NOP_POWER();
                if (data != null)
                {
                    int index = 2;
                    ret.txPowerRegisterObsolete = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.txPowerDampening = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](CMD_NOP_POWER command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(ZWAVE_CMD_CLASS.ID);
                ret.Add(ID);
                if (command.txPowerRegisterObsolete.HasValue) ret.Add(command.txPowerRegisterObsolete);
                if (command.txPowerDampening.HasValue) ret.Add(command.txPowerDampening);
                return ret.ToArray();
            }
        }
        public partial class REQUEST_NODE_INFO
        {
            public const byte ID = 0x02;
            public static implicit operator REQUEST_NODE_INFO(byte[] data)
            {
                REQUEST_NODE_INFO ret = new REQUEST_NODE_INFO();
                return ret;
            }
            public static implicit operator byte[](REQUEST_NODE_INFO command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(ZWAVE_CMD_CLASS.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class ZWAVE_CMD_RESERVE_NODE_IDS
        {
            public const byte ID = 0x19;
            public ByteValue numberOfNodes = 0;
            public static implicit operator ZWAVE_CMD_RESERVE_NODE_IDS(byte[] data)
            {
                ZWAVE_CMD_RESERVE_NODE_IDS ret = new ZWAVE_CMD_RESERVE_NODE_IDS();
                if (data != null)
                {
                    int index = 2;
                    ret.numberOfNodes = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](ZWAVE_CMD_RESERVE_NODE_IDS command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(ZWAVE_CMD_CLASS.ID);
                ret.Add(ID);
                if (command.numberOfNodes.HasValue) ret.Add(command.numberOfNodes);
                return ret.ToArray();
            }
        }
        public partial class CMD_RESERVED_IDS
        {
            public const byte ID = 0x1A;
            public ByteValue numberOfNodes = 0;
            public IList<byte> nodeId = new List<byte>();
            public static implicit operator CMD_RESERVED_IDS(byte[] data)
            {
                CMD_RESERVED_IDS ret = new CMD_RESERVED_IDS();
                if (data != null)
                {
                    int index = 2;
                    ret.numberOfNodes = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nodeId = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.nodeId.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](CMD_RESERVED_IDS command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(ZWAVE_CMD_CLASS.ID);
                ret.Add(ID);
                if (command.numberOfNodes.HasValue) ret.Add(command.numberOfNodes);
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
        public partial class CMD_SET_SUC
        {
            public const byte ID = 0x12;
            public ByteValue state = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte serverRunning
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
            public static implicit operator CMD_SET_SUC(byte[] data)
            {
                CMD_SET_SUC ret = new CMD_SET_SUC();
                if (data != null)
                {
                    int index = 2;
                    ret.state = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](CMD_SET_SUC command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(ZWAVE_CMD_CLASS.ID);
                ret.Add(ID);
                if (command.state.HasValue) ret.Add(command.state);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                return ret.ToArray();
            }
        }
        public partial class CMD_SET_SUC_ACK
        {
            public const byte ID = 0x13;
            public ByteValue result = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte serverRunning
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
            public static implicit operator CMD_SET_SUC_ACK(byte[] data)
            {
                CMD_SET_SUC_ACK ret = new CMD_SET_SUC_ACK();
                if (data != null)
                {
                    int index = 2;
                    ret.result = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](CMD_SET_SUC_ACK command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(ZWAVE_CMD_CLASS.ID);
                ret.Add(ID);
                if (command.result.HasValue) ret.Add(command.result);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                return ret.ToArray();
            }
        }
        public partial class CMD_STATIC_ROUTE_REQUEST
        {
            public const byte ID = 0x15;
            public const byte destNodeIdBytesCount = 5;
            public byte[] destNodeId = new byte[destNodeIdBytesCount];
            public static implicit operator CMD_STATIC_ROUTE_REQUEST(byte[] data)
            {
                CMD_STATIC_ROUTE_REQUEST ret = new CMD_STATIC_ROUTE_REQUEST();
                if (data != null)
                {
                    int index = 2;
                    ret.destNodeId = (data.Length - index) >= destNodeIdBytesCount ? new byte[destNodeIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.destNodeId[0] = data[index++];
                    if (data.Length > index) ret.destNodeId[1] = data[index++];
                    if (data.Length > index) ret.destNodeId[2] = data[index++];
                    if (data.Length > index) ret.destNodeId[3] = data[index++];
                    if (data.Length > index) ret.destNodeId[4] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](CMD_STATIC_ROUTE_REQUEST command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(ZWAVE_CMD_CLASS.ID);
                ret.Add(ID);
                if (command.destNodeId != null)
                {
                    foreach (var tmp in command.destNodeId)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class CMD_SUC_NODE_ID
        {
            public const byte ID = 0x11;
            public ByteValue sucNodeId = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte serverRunning
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
            public static implicit operator CMD_SUC_NODE_ID(byte[] data)
            {
                CMD_SUC_NODE_ID ret = new CMD_SUC_NODE_ID();
                if (data != null)
                {
                    int index = 2;
                    ret.sucNodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](CMD_SUC_NODE_ID command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(ZWAVE_CMD_CLASS.ID);
                ret.Add(ID);
                if (command.sucNodeId.HasValue) ret.Add(command.sucNodeId);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                return ret.ToArray();
            }
        }
        public partial class TRANSFER_END
        {
            public const byte ID = 0x0B;
            public ByteValue status = 0;
            public static implicit operator TRANSFER_END(byte[] data)
            {
                TRANSFER_END ret = new TRANSFER_END();
                if (data != null)
                {
                    int index = 2;
                    ret.status = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](TRANSFER_END command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(ZWAVE_CMD_CLASS.ID);
                ret.Add(ID);
                if (command.status.HasValue) ret.Add(command.status);
                return ret.ToArray();
            }
        }
        public partial class TRANSFER_NEW_PRIMARY_COMPLETE
        {
            public const byte ID = 0x0F;
            public ByteValue controllerType = 0;
            public static implicit operator TRANSFER_NEW_PRIMARY_COMPLETE(byte[] data)
            {
                TRANSFER_NEW_PRIMARY_COMPLETE ret = new TRANSFER_NEW_PRIMARY_COMPLETE();
                if (data != null)
                {
                    int index = 2;
                    ret.controllerType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](TRANSFER_NEW_PRIMARY_COMPLETE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(ZWAVE_CMD_CLASS.ID);
                ret.Add(ID);
                if (command.controllerType.HasValue) ret.Add(command.controllerType);
                return ret.ToArray();
            }
        }
        public partial class TRANSFER_NODE_INFO
        {
            public const byte ID = 0x09;
            public ByteValue sequenceNumber = 0;
            public ByteValue nodeId = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte protocolVersion
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte supportedSpeed96Kbps
                {
                    get { return (byte)(_value >> 3 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x08; _value += (byte)(value << 3 & 0x08); }
                }
                public byte supportedSpeed40Kbps
                {
                    get { return (byte)(_value >> 4 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x10; _value += (byte)(value << 4 & 0x10); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 5 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x20; _value += (byte)(value << 5 & 0x20); }
                }
                public byte routing
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte listening
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
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
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte reserved2
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte controller
                {
                    get { return (byte)(_value >> 1 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x02; _value += (byte)(value << 1 & 0x02); }
                }
                public byte specificDevice
                {
                    get { return (byte)(_value >> 2 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x04; _value += (byte)(value << 2 & 0x04); }
                }
                public byte routingEndNode
                {
                    get { return (byte)(_value >> 3 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x08; _value += (byte)(value << 3 & 0x08); }
                }
                public byte beamCapability
                {
                    get { return (byte)(_value >> 4 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x10; _value += (byte)(value << 4 & 0x10); }
                }
                public byte sensor250ms
                {
                    get { return (byte)(_value >> 5 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x20; _value += (byte)(value << 5 & 0x20); }
                }
                public byte sensor1000ms
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte optionalFunctionality
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties2(byte data)
                {
                    Tproperties2 ret = new Tproperties2();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties2 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties2 properties2 = 0;
            public struct Tproperties3
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties3 Empty { get { return new Tproperties3() { _value = 0, HasValue = false }; } }
                public byte speedExtension100Kbps
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte speedExtensionReserved
                {
                    get { return (byte)(_value >> 1 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x06; _value += (byte)(value << 1 & 0x06); }
                }
                public byte reserved3
                {
                    get { return (byte)(_value >> 3 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0xF8; _value += (byte)(value << 3 & 0xF8); }
                }
                public static implicit operator Tproperties3(byte data)
                {
                    Tproperties3 ret = new Tproperties3();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties3 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties3 properties3 = 0;
            public ByteValue basicDeviceClass = 0;
            public ByteValue genericDeviceClass = 0;
            public ByteValue specificDeviceClass = 0;
            public static implicit operator TRANSFER_NODE_INFO(byte[] data)
            {
                TRANSFER_NODE_INFO ret = new TRANSFER_NODE_INFO();
                if (data != null)
                {
                    int index = 2;
                    ret.sequenceNumber = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.properties3 = data.Length > index ? (Tproperties3)data[index++] : Tproperties3.Empty;
                    if (ret.properties2.controller > 0)
                    {
                        ret.basicDeviceClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    }
                    ret.genericDeviceClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    if (ret.properties2.specificDevice > 0)
                    {
                        ret.specificDeviceClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    }
                }
                return ret;
            }
            public static implicit operator byte[](TRANSFER_NODE_INFO command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(ZWAVE_CMD_CLASS.ID);
                ret.Add(ID);
                if (command.sequenceNumber.HasValue) ret.Add(command.sequenceNumber);
                if (command.nodeId.HasValue) ret.Add(command.nodeId);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.properties3.HasValue) ret.Add(command.properties3);
                if (command.properties2.controller > 0)
                {
                    if (command.basicDeviceClass.HasValue) ret.Add(command.basicDeviceClass);
                }
                if (command.genericDeviceClass.HasValue) ret.Add(command.genericDeviceClass);
                if (command.properties2.specificDevice > 0)
                {
                    if (command.specificDeviceClass.HasValue) ret.Add(command.specificDeviceClass);
                }
                return ret.ToArray();
            }
        }
        public partial class TRANSFER_PRESENTATION
        {
            public const byte ID = 0x08;
            public ByteValue option = 0;
            public static implicit operator TRANSFER_PRESENTATION(byte[] data)
            {
                TRANSFER_PRESENTATION ret = new TRANSFER_PRESENTATION();
                if (data != null)
                {
                    int index = 2;
                    ret.option = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](TRANSFER_PRESENTATION command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(ZWAVE_CMD_CLASS.ID);
                ret.Add(ID);
                if (command.option.HasValue) ret.Add(command.option);
                return ret.ToArray();
            }
        }
        public partial class TRANSFER_RANGE_INFO
        {
            public const byte ID = 0x0A;
            public ByteValue sequenceNumber = 0;
            public ByteValue nodeId = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte numberOfMaskBytes
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 5 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0xE0; _value += (byte)(value << 5 & 0xE0); }
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
            public IList<byte> maskByteS = new List<byte>();
            public static implicit operator TRANSFER_RANGE_INFO(byte[] data)
            {
                TRANSFER_RANGE_INFO ret = new TRANSFER_RANGE_INFO();
                if (data != null)
                {
                    int index = 2;
                    ret.sequenceNumber = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.maskByteS = new List<byte>();
                    for (int i = 0; i < ret.properties1.numberOfMaskBytes; i++)
                    {
                        if (data.Length > index) ret.maskByteS.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](TRANSFER_RANGE_INFO command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(ZWAVE_CMD_CLASS.ID);
                ret.Add(ID);
                if (command.sequenceNumber.HasValue) ret.Add(command.sequenceNumber);
                if (command.nodeId.HasValue) ret.Add(command.nodeId);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.maskByteS != null)
                {
                    foreach (var tmp in command.maskByteS)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class EXCLUDE_REQUEST
        {
            public const byte ID = 0x23;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte protocolVersion
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte supportedSpeed96Kbps
                {
                    get { return (byte)(_value >> 3 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x08; _value += (byte)(value << 3 & 0x08); }
                }
                public byte supportedSpeed40Kbps
                {
                    get { return (byte)(_value >> 4 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x10; _value += (byte)(value << 4 & 0x10); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 5 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x20; _value += (byte)(value << 5 & 0x20); }
                }
                public byte routing
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte listening
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
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
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte reserved2
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte controller
                {
                    get { return (byte)(_value >> 1 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x02; _value += (byte)(value << 1 & 0x02); }
                }
                public byte specificDevice
                {
                    get { return (byte)(_value >> 2 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x04; _value += (byte)(value << 2 & 0x04); }
                }
                public byte routingEndNode
                {
                    get { return (byte)(_value >> 3 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x08; _value += (byte)(value << 3 & 0x08); }
                }
                public byte beamCapability
                {
                    get { return (byte)(_value >> 4 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x10; _value += (byte)(value << 4 & 0x10); }
                }
                public byte sensor250ms
                {
                    get { return (byte)(_value >> 5 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x20; _value += (byte)(value << 5 & 0x20); }
                }
                public byte sensor1000ms
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte optionalFunctionality
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties2(byte data)
                {
                    Tproperties2 ret = new Tproperties2();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties2 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties2 properties2 = 0;
            public struct Tproperties3
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties3 Empty { get { return new Tproperties3() { _value = 0, HasValue = false }; } }
                public byte speedExtension100Kbps
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte speedExtensionReserved
                {
                    get { return (byte)(_value >> 1 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x06; _value += (byte)(value << 1 & 0x06); }
                }
                public byte reserved3
                {
                    get { return (byte)(_value >> 3 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0xF8; _value += (byte)(value << 3 & 0xF8); }
                }
                public static implicit operator Tproperties3(byte data)
                {
                    Tproperties3 ret = new Tproperties3();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties3 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties3 properties3 = 0;
            public ByteValue basicDeviceClass = 0;
            public ByteValue genericDeviceClass = 0;
            public ByteValue specificDeviceClass = 0;
            public IList<byte> commandClasses = new List<byte>();
            public static implicit operator EXCLUDE_REQUEST(byte[] data)
            {
                EXCLUDE_REQUEST ret = new EXCLUDE_REQUEST();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.properties3 = data.Length > index ? (Tproperties3)data[index++] : Tproperties3.Empty;
                    if (ret.properties2.controller > 0)
                    {
                        ret.basicDeviceClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    }
                    ret.genericDeviceClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    if (ret.properties2.specificDevice > 0)
                    {
                        ret.specificDeviceClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    }
                    ret.commandClasses = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.commandClasses.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](EXCLUDE_REQUEST command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(ZWAVE_CMD_CLASS.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.properties3.HasValue) ret.Add(command.properties3);
                if (command.properties2.controller > 0)
                {
                    if (command.basicDeviceClass.HasValue) ret.Add(command.basicDeviceClass);
                }
                if (command.genericDeviceClass.HasValue) ret.Add(command.genericDeviceClass);
                if (command.properties2.specificDevice > 0)
                {
                    if (command.specificDeviceClass.HasValue) ret.Add(command.specificDeviceClass);
                }
                if (command.commandClasses != null)
                {
                    foreach (var tmp in command.commandClasses)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class ASSIGN_RETURN_ROUTE_PRIORITY
        {
            public const byte ID = 0x24;
            public ByteValue destinationNodeId = 0;
            public ByteValue priorityRoute = 0;
            public static implicit operator ASSIGN_RETURN_ROUTE_PRIORITY(byte[] data)
            {
                ASSIGN_RETURN_ROUTE_PRIORITY ret = new ASSIGN_RETURN_ROUTE_PRIORITY();
                if (data != null)
                {
                    int index = 2;
                    ret.destinationNodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.priorityRoute = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](ASSIGN_RETURN_ROUTE_PRIORITY command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(ZWAVE_CMD_CLASS.ID);
                ret.Add(ID);
                if (command.destinationNodeId.HasValue) ret.Add(command.destinationNodeId);
                if (command.priorityRoute.HasValue) ret.Add(command.priorityRoute);
                return ret.ToArray();
            }
        }
        public partial class ASSIGN_SUC_RETURN_ROUTE_PRIORITY
        {
            public const byte ID = 0x25;
            public ByteValue destinationNodeId = 0;
            public ByteValue priorityRoute = 0;
            public static implicit operator ASSIGN_SUC_RETURN_ROUTE_PRIORITY(byte[] data)
            {
                ASSIGN_SUC_RETURN_ROUTE_PRIORITY ret = new ASSIGN_SUC_RETURN_ROUTE_PRIORITY();
                if (data != null)
                {
                    int index = 2;
                    ret.destinationNodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.priorityRoute = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](ASSIGN_SUC_RETURN_ROUTE_PRIORITY command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(ZWAVE_CMD_CLASS.ID);
                ret.Add(ID);
                if (command.destinationNodeId.HasValue) ret.Add(command.destinationNodeId);
                if (command.priorityRoute.HasValue) ret.Add(command.priorityRoute);
                return ret.ToArray();
            }
        }
        public partial class INCLUDED_NODE_INFO
        {
            public const byte ID = 0x26;
            public const byte nwiHomeIdBytesCount = 4;
            public byte[] nwiHomeId = new byte[nwiHomeIdBytesCount];
            public static implicit operator INCLUDED_NODE_INFO(byte[] data)
            {
                INCLUDED_NODE_INFO ret = new INCLUDED_NODE_INFO();
                if (data != null)
                {
                    int index = 2;
                    ret.nwiHomeId = (data.Length - index) >= nwiHomeIdBytesCount ? new byte[nwiHomeIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.nwiHomeId[0] = data[index++];
                    if (data.Length > index) ret.nwiHomeId[1] = data[index++];
                    if (data.Length > index) ret.nwiHomeId[2] = data[index++];
                    if (data.Length > index) ret.nwiHomeId[3] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](INCLUDED_NODE_INFO command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(ZWAVE_CMD_CLASS.ID);
                ret.Add(ID);
                if (command.nwiHomeId != null)
                {
                    foreach (var tmp in command.nwiHomeId)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class SMART_START_PRIME
        {
            public const byte ID = 0x27;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte protocolVersion
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte supportedSpeed96Kbps
                {
                    get { return (byte)(_value >> 3 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x08; _value += (byte)(value << 3 & 0x08); }
                }
                public byte supportedSpeed40Kbps
                {
                    get { return (byte)(_value >> 4 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x10; _value += (byte)(value << 4 & 0x10); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 5 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x20; _value += (byte)(value << 5 & 0x20); }
                }
                public byte routing
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte listening
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
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
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte reserved2
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte controller
                {
                    get { return (byte)(_value >> 1 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x02; _value += (byte)(value << 1 & 0x02); }
                }
                public byte specificDevice
                {
                    get { return (byte)(_value >> 2 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x04; _value += (byte)(value << 2 & 0x04); }
                }
                public byte routingEndNode
                {
                    get { return (byte)(_value >> 3 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x08; _value += (byte)(value << 3 & 0x08); }
                }
                public byte beamCapability
                {
                    get { return (byte)(_value >> 4 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x10; _value += (byte)(value << 4 & 0x10); }
                }
                public byte sensor250ms
                {
                    get { return (byte)(_value >> 5 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x20; _value += (byte)(value << 5 & 0x20); }
                }
                public byte sensor1000ms
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte optionalFunctionality
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties2(byte data)
                {
                    Tproperties2 ret = new Tproperties2();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties2 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties2 properties2 = 0;
            public struct Tproperties3
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties3 Empty { get { return new Tproperties3() { _value = 0, HasValue = false }; } }
                public byte speedExtension100Kbps
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte speedExtensionReserved
                {
                    get { return (byte)(_value >> 1 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x06; _value += (byte)(value << 1 & 0x06); }
                }
                public byte reserved3
                {
                    get { return (byte)(_value >> 3 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0xF8; _value += (byte)(value << 3 & 0xF8); }
                }
                public static implicit operator Tproperties3(byte data)
                {
                    Tproperties3 ret = new Tproperties3();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties3 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties3 properties3 = 0;
            public ByteValue basicDeviceClass = 0;
            public ByteValue genericDeviceClass = 0;
            public ByteValue specificDeviceClass = 0;
            public IList<byte> commandClasses = new List<byte>();
            public static implicit operator SMART_START_PRIME(byte[] data)
            {
                SMART_START_PRIME ret = new SMART_START_PRIME();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.properties3 = data.Length > index ? (Tproperties3)data[index++] : Tproperties3.Empty;
                    if (ret.properties2.controller > 0)
                    {
                        ret.basicDeviceClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    }
                    ret.genericDeviceClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    if (ret.properties2.specificDevice > 0)
                    {
                        ret.specificDeviceClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    }
                    ret.commandClasses = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.commandClasses.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](SMART_START_PRIME command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(ZWAVE_CMD_CLASS.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.properties3.HasValue) ret.Add(command.properties3);
                if (command.properties2.controller > 0)
                {
                    if (command.basicDeviceClass.HasValue) ret.Add(command.basicDeviceClass);
                }
                if (command.genericDeviceClass.HasValue) ret.Add(command.genericDeviceClass);
                if (command.properties2.specificDevice > 0)
                {
                    if (command.specificDeviceClass.HasValue) ret.Add(command.specificDeviceClass);
                }
                if (command.commandClasses != null)
                {
                    foreach (var tmp in command.commandClasses)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class SMART_START_INCLUDE
        {
            public const byte ID = 0x28;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte protocolVersion
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte supportedSpeed96Kbps
                {
                    get { return (byte)(_value >> 3 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x08; _value += (byte)(value << 3 & 0x08); }
                }
                public byte supportedSpeed40Kbps
                {
                    get { return (byte)(_value >> 4 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x10; _value += (byte)(value << 4 & 0x10); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 5 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x20; _value += (byte)(value << 5 & 0x20); }
                }
                public byte routing
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte listening
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
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
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte reserved2
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte controller
                {
                    get { return (byte)(_value >> 1 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x02; _value += (byte)(value << 1 & 0x02); }
                }
                public byte specificDevice
                {
                    get { return (byte)(_value >> 2 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x04; _value += (byte)(value << 2 & 0x04); }
                }
                public byte routingEndNode
                {
                    get { return (byte)(_value >> 3 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x08; _value += (byte)(value << 3 & 0x08); }
                }
                public byte beamCapability
                {
                    get { return (byte)(_value >> 4 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x10; _value += (byte)(value << 4 & 0x10); }
                }
                public byte sensor250ms
                {
                    get { return (byte)(_value >> 5 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x20; _value += (byte)(value << 5 & 0x20); }
                }
                public byte sensor1000ms
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte optionalFunctionality
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties2(byte data)
                {
                    Tproperties2 ret = new Tproperties2();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties2 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties2 properties2 = 0;
            public struct Tproperties3
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties3 Empty { get { return new Tproperties3() { _value = 0, HasValue = false }; } }
                public byte speedExtension100Kbps
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte speedExtensionReserved
                {
                    get { return (byte)(_value >> 1 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x06; _value += (byte)(value << 1 & 0x06); }
                }
                public byte reserved3
                {
                    get { return (byte)(_value >> 3 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0xF8; _value += (byte)(value << 3 & 0xF8); }
                }
                public static implicit operator Tproperties3(byte data)
                {
                    Tproperties3 ret = new Tproperties3();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties3 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties3 properties3 = 0;
            public ByteValue basicDeviceClass = 0;
            public ByteValue genericDeviceClass = 0;
            public ByteValue specificDeviceClass = 0;
            public IList<byte> commandClasses = new List<byte>();
            public static implicit operator SMART_START_INCLUDE(byte[] data)
            {
                SMART_START_INCLUDE ret = new SMART_START_INCLUDE();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.properties3 = data.Length > index ? (Tproperties3)data[index++] : Tproperties3.Empty;
                    if (ret.properties2.controller > 0)
                    {
                        ret.basicDeviceClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    }
                    ret.genericDeviceClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    if (ret.properties2.specificDevice > 0)
                    {
                        ret.specificDeviceClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    }
                    ret.commandClasses = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.commandClasses.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](SMART_START_INCLUDE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(ZWAVE_CMD_CLASS.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.properties3.HasValue) ret.Add(command.properties3);
                if (command.properties2.controller > 0)
                {
                    if (command.basicDeviceClass.HasValue) ret.Add(command.basicDeviceClass);
                }
                if (command.genericDeviceClass.HasValue) ret.Add(command.genericDeviceClass);
                if (command.properties2.specificDevice > 0)
                {
                    if (command.specificDeviceClass.HasValue) ret.Add(command.specificDeviceClass);
                }
                if (command.commandClasses != null)
                {
                    foreach (var tmp in command.commandClasses)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

