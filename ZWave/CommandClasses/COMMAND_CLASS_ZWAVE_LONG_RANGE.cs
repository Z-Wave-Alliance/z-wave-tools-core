/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_ZWAVE_LONG_RANGE
    {
        public const byte ID = 0x04;
        public const byte VERSION = 1;
        public partial class ZWAVE_LR_CMD_ASSIGN_IDS
        {
            public const byte ID = 0x03;
            public const byte newNodeIdBytesCount = 2;
            public byte[] newNodeId = new byte[newNodeIdBytesCount];
            public const byte newHomeIdBytesCount = 4;
            public byte[] newHomeId = new byte[newHomeIdBytesCount];
            public static implicit operator ZWAVE_LR_CMD_ASSIGN_IDS(byte[] data)
            {
                ZWAVE_LR_CMD_ASSIGN_IDS ret = new ZWAVE_LR_CMD_ASSIGN_IDS();
                if (data != null)
                {
                    int index = 2;
                    ret.newNodeId = (data.Length - index) >= newNodeIdBytesCount ? new byte[newNodeIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.newNodeId[0] = data[index++];
                    if (data.Length > index) ret.newNodeId[1] = data[index++];
                    ret.newHomeId = (data.Length - index) >= newHomeIdBytesCount ? new byte[newHomeIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.newHomeId[0] = data[index++];
                    if (data.Length > index) ret.newHomeId[1] = data[index++];
                    if (data.Length > index) ret.newHomeId[2] = data[index++];
                    if (data.Length > index) ret.newHomeId[3] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](ZWAVE_LR_CMD_ASSIGN_IDS command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZWAVE_LONG_RANGE.ID);
                ret.Add(ID);
                if (command.newNodeId != null)
                {
                    foreach (var tmp in command.newNodeId)
                    {
                        ret.Add(tmp);
                    }
                }
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
        public partial class ZWAVE_LR_CMD_NODE_INFO_FRAME
        {
            public const byte ID = 0x01;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte reserved
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
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
                public byte reserved3
                {
                    get { return (byte)(_value >> 2 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x3C; _value += (byte)(value << 2 & 0x3C); }
                }
                public byte sensor1000ms
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte reserved4
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
                public byte supportedSpeedReserved
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte supportedSpeed100Kbps
                {
                    get { return (byte)(_value >> 1 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x02; _value += (byte)(value << 1 & 0x02); }
                }
                public byte supportedSpeedReserved2
                {
                    get { return (byte)(_value >> 2 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x04; _value += (byte)(value << 2 & 0x04); }
                }
                public byte reserved5
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
            public ByteValue genericDeviceClass = 0;
            public ByteValue specificDeviceClass = 0;
            public ByteValue commandClassListLength = 0;
            public IList<byte> commandClasses = new List<byte>();
            public static implicit operator ZWAVE_LR_CMD_NODE_INFO_FRAME(byte[] data)
            {
                ZWAVE_LR_CMD_NODE_INFO_FRAME ret = new ZWAVE_LR_CMD_NODE_INFO_FRAME();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.properties3 = data.Length > index ? (Tproperties3)data[index++] : Tproperties3.Empty;
                    ret.genericDeviceClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.specificDeviceClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.commandClassListLength = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.commandClasses = new List<byte>();
                    for (int i = 0; i < ret.commandClassListLength; i++)
                    {
                        if (data.Length > index) ret.commandClasses.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](ZWAVE_LR_CMD_NODE_INFO_FRAME command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZWAVE_LONG_RANGE.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.properties3.HasValue) ret.Add(command.properties3);
                if (command.genericDeviceClass.HasValue) ret.Add(command.genericDeviceClass);
                if (command.specificDeviceClass.HasValue) ret.Add(command.specificDeviceClass);
                if (command.commandClassListLength.HasValue) ret.Add(command.commandClassListLength);
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
        public partial class ZWAVE_LR_CMD_NO_OPERATION
        {
            public const byte ID = 0x00;
            public static implicit operator ZWAVE_LR_CMD_NO_OPERATION(byte[] data)
            {
                ZWAVE_LR_CMD_NO_OPERATION ret = new ZWAVE_LR_CMD_NO_OPERATION();
                return ret;
            }
            public static implicit operator byte[](ZWAVE_LR_CMD_NO_OPERATION command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZWAVE_LONG_RANGE.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class ZWAVE_LR_CMD_REQUEST_NODE_INFO
        {
            public const byte ID = 0x02;
            public static implicit operator ZWAVE_LR_CMD_REQUEST_NODE_INFO(byte[] data)
            {
                ZWAVE_LR_CMD_REQUEST_NODE_INFO ret = new ZWAVE_LR_CMD_REQUEST_NODE_INFO();
                return ret;
            }
            public static implicit operator byte[](ZWAVE_LR_CMD_REQUEST_NODE_INFO command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZWAVE_LONG_RANGE.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class ZWAVE_LR_CMD_EXCLUDE_REQUEST
        {
            public const byte ID = 0x23;
            public static implicit operator ZWAVE_LR_CMD_EXCLUDE_REQUEST(byte[] data)
            {
                ZWAVE_LR_CMD_EXCLUDE_REQUEST ret = new ZWAVE_LR_CMD_EXCLUDE_REQUEST();
                return ret;
            }
            public static implicit operator byte[](ZWAVE_LR_CMD_EXCLUDE_REQUEST command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZWAVE_LONG_RANGE.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class ZWAVE_LR_CMD_INCLUDED_NODE_INFO
        {
            public const byte ID = 0x26;
            public const byte nwiHomeIdBytesCount = 4;
            public byte[] nwiHomeId = new byte[nwiHomeIdBytesCount];
            public static implicit operator ZWAVE_LR_CMD_INCLUDED_NODE_INFO(byte[] data)
            {
                ZWAVE_LR_CMD_INCLUDED_NODE_INFO ret = new ZWAVE_LR_CMD_INCLUDED_NODE_INFO();
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
            public static implicit operator byte[](ZWAVE_LR_CMD_INCLUDED_NODE_INFO command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZWAVE_LONG_RANGE.ID);
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
        public partial class ZWAVE_LR_CMD_SMARTSTART_PRIME
        {
            public const byte ID = 0x27;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte reserved
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
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
                public byte reserved3
                {
                    get { return (byte)(_value >> 2 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x0C; _value += (byte)(value << 2 & 0x0C); }
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
                public byte reserved4
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
            public ByteValue reserved = 0;
            public ByteValue genericDeviceClass = 0;
            public ByteValue specificDeviceClass = 0;
            public ByteValue commandClassListLength = 0;
            public IList<byte> commandClasses = new List<byte>();
            public static implicit operator ZWAVE_LR_CMD_SMARTSTART_PRIME(byte[] data)
            {
                ZWAVE_LR_CMD_SMARTSTART_PRIME ret = new ZWAVE_LR_CMD_SMARTSTART_PRIME();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.reserved = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.genericDeviceClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.specificDeviceClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.commandClassListLength = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.commandClasses = new List<byte>();
                    for (int i = 0; i < ret.commandClassListLength; i++)
                    {
                        if (data.Length > index) ret.commandClasses.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](ZWAVE_LR_CMD_SMARTSTART_PRIME command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZWAVE_LONG_RANGE.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.reserved.HasValue) ret.Add(command.reserved);
                if (command.genericDeviceClass.HasValue) ret.Add(command.genericDeviceClass);
                if (command.specificDeviceClass.HasValue) ret.Add(command.specificDeviceClass);
                if (command.commandClassListLength.HasValue) ret.Add(command.commandClassListLength);
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
        public partial class ZWAVE_LR_CMD_SMARTSTART_INCLUDE
        {
            public const byte ID = 0x28;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte reserved
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
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
                public byte reserved3
                {
                    get { return (byte)(_value >> 2 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x0C; _value += (byte)(value << 2 & 0x0C); }
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
                public byte reserved4
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
            public ByteValue reserved = 0;
            public ByteValue genericDeviceClass = 0;
            public ByteValue specificDeviceClass = 0;
            public ByteValue commandClassListLength = 0;
            public IList<byte> commandClasses = new List<byte>();
            public static implicit operator ZWAVE_LR_CMD_SMARTSTART_INCLUDE(byte[] data)
            {
                ZWAVE_LR_CMD_SMARTSTART_INCLUDE ret = new ZWAVE_LR_CMD_SMARTSTART_INCLUDE();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.reserved = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.genericDeviceClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.specificDeviceClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.commandClassListLength = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.commandClasses = new List<byte>();
                    for (int i = 0; i < ret.commandClassListLength; i++)
                    {
                        if (data.Length > index) ret.commandClasses.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](ZWAVE_LR_CMD_SMARTSTART_INCLUDE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZWAVE_LONG_RANGE.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.reserved.HasValue) ret.Add(command.reserved);
                if (command.genericDeviceClass.HasValue) ret.Add(command.genericDeviceClass);
                if (command.specificDeviceClass.HasValue) ret.Add(command.specificDeviceClass);
                if (command.commandClassListLength.HasValue) ret.Add(command.commandClassListLength);
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
        public partial class ZWAVE_LR_CMD_EXCLUDE_REQUEST_CONFIRMATION
        {
            public const byte ID = 0x29;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte requestingNodeidMsb
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte reserved1
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
            public ByteValue requestingNodeidLsb = 0;
            public const byte requestingHomeidBytesCount = 4;
            public byte[] requestingHomeid = new byte[requestingHomeidBytesCount];
            public static implicit operator ZWAVE_LR_CMD_EXCLUDE_REQUEST_CONFIRMATION(byte[] data)
            {
                ZWAVE_LR_CMD_EXCLUDE_REQUEST_CONFIRMATION ret = new ZWAVE_LR_CMD_EXCLUDE_REQUEST_CONFIRMATION();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.requestingNodeidLsb = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.requestingHomeid = (data.Length - index) >= requestingHomeidBytesCount ? new byte[requestingHomeidBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.requestingHomeid[0] = data[index++];
                    if (data.Length > index) ret.requestingHomeid[1] = data[index++];
                    if (data.Length > index) ret.requestingHomeid[2] = data[index++];
                    if (data.Length > index) ret.requestingHomeid[3] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](ZWAVE_LR_CMD_EXCLUDE_REQUEST_CONFIRMATION command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZWAVE_LONG_RANGE.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.requestingNodeidLsb.HasValue) ret.Add(command.requestingNodeidLsb);
                if (command.requestingHomeid != null)
                {
                    foreach (var tmp in command.requestingHomeid)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class ZWAVE_LR_CMD_NON_SECURE_INCLUSION_STEP_COMPLETE
        {
            public const byte ID = 0x2A;
            public static implicit operator ZWAVE_LR_CMD_NON_SECURE_INCLUSION_STEP_COMPLETE(byte[] data)
            {
                ZWAVE_LR_CMD_NON_SECURE_INCLUSION_STEP_COMPLETE ret = new ZWAVE_LR_CMD_NON_SECURE_INCLUSION_STEP_COMPLETE();
                return ret;
            }
            public static implicit operator byte[](ZWAVE_LR_CMD_NON_SECURE_INCLUSION_STEP_COMPLETE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZWAVE_LONG_RANGE.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
    }
}

