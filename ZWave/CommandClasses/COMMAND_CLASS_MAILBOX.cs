/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_MAILBOX
    {
        public const byte ID = 0x69;
        public const byte VERSION = 1;
        public partial class MAILBOX_CONFIGURATION_GET
        {
            public const byte ID = 0x01;
            public static implicit operator MAILBOX_CONFIGURATION_GET(byte[] data)
            {
                MAILBOX_CONFIGURATION_GET ret = new MAILBOX_CONFIGURATION_GET();
                return ret;
            }
            public static implicit operator byte[](MAILBOX_CONFIGURATION_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_MAILBOX.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class MAILBOX_CONFIGURATION_SET
        {
            public const byte ID = 0x02;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte mode
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 3 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0xF8; _value += (byte)(value << 3 & 0xF8); }
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
            public const byte forwardingDestinationIpv6AddressBytesCount = 16;
            public byte[] forwardingDestinationIpv6Address = new byte[forwardingDestinationIpv6AddressBytesCount];
            public const byte udpPortNumberBytesCount = 2;
            public byte[] udpPortNumber = new byte[udpPortNumberBytesCount];
            public static implicit operator MAILBOX_CONFIGURATION_SET(byte[] data)
            {
                MAILBOX_CONFIGURATION_SET ret = new MAILBOX_CONFIGURATION_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.forwardingDestinationIpv6Address = (data.Length - index) >= forwardingDestinationIpv6AddressBytesCount ? new byte[forwardingDestinationIpv6AddressBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.forwardingDestinationIpv6Address[0] = data[index++];
                    if (data.Length > index) ret.forwardingDestinationIpv6Address[1] = data[index++];
                    if (data.Length > index) ret.forwardingDestinationIpv6Address[2] = data[index++];
                    if (data.Length > index) ret.forwardingDestinationIpv6Address[3] = data[index++];
                    if (data.Length > index) ret.forwardingDestinationIpv6Address[4] = data[index++];
                    if (data.Length > index) ret.forwardingDestinationIpv6Address[5] = data[index++];
                    if (data.Length > index) ret.forwardingDestinationIpv6Address[6] = data[index++];
                    if (data.Length > index) ret.forwardingDestinationIpv6Address[7] = data[index++];
                    if (data.Length > index) ret.forwardingDestinationIpv6Address[8] = data[index++];
                    if (data.Length > index) ret.forwardingDestinationIpv6Address[9] = data[index++];
                    if (data.Length > index) ret.forwardingDestinationIpv6Address[10] = data[index++];
                    if (data.Length > index) ret.forwardingDestinationIpv6Address[11] = data[index++];
                    if (data.Length > index) ret.forwardingDestinationIpv6Address[12] = data[index++];
                    if (data.Length > index) ret.forwardingDestinationIpv6Address[13] = data[index++];
                    if (data.Length > index) ret.forwardingDestinationIpv6Address[14] = data[index++];
                    if (data.Length > index) ret.forwardingDestinationIpv6Address[15] = data[index++];
                    ret.udpPortNumber = (data.Length - index) >= udpPortNumberBytesCount ? new byte[udpPortNumberBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.udpPortNumber[0] = data[index++];
                    if (data.Length > index) ret.udpPortNumber[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](MAILBOX_CONFIGURATION_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_MAILBOX.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.forwardingDestinationIpv6Address != null)
                {
                    foreach (var tmp in command.forwardingDestinationIpv6Address)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.udpPortNumber != null)
                {
                    foreach (var tmp in command.udpPortNumber)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class MAILBOX_CONFIGURATION_REPORT
        {
            public const byte ID = 0x03;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte mode
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte supportedModes
                {
                    get { return (byte)(_value >> 3 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x18; _value += (byte)(value << 3 & 0x18); }
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
            public const byte mailboxCapacityBytesCount = 2;
            public byte[] mailboxCapacity = new byte[mailboxCapacityBytesCount];
            public const byte forwardingDestinationIpv6AddressBytesCount = 16;
            public byte[] forwardingDestinationIpv6Address = new byte[forwardingDestinationIpv6AddressBytesCount];
            public const byte udpPortNumberBytesCount = 2;
            public byte[] udpPortNumber = new byte[udpPortNumberBytesCount];
            public static implicit operator MAILBOX_CONFIGURATION_REPORT(byte[] data)
            {
                MAILBOX_CONFIGURATION_REPORT ret = new MAILBOX_CONFIGURATION_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.mailboxCapacity = (data.Length - index) >= mailboxCapacityBytesCount ? new byte[mailboxCapacityBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.mailboxCapacity[0] = data[index++];
                    if (data.Length > index) ret.mailboxCapacity[1] = data[index++];
                    ret.forwardingDestinationIpv6Address = (data.Length - index) >= forwardingDestinationIpv6AddressBytesCount ? new byte[forwardingDestinationIpv6AddressBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.forwardingDestinationIpv6Address[0] = data[index++];
                    if (data.Length > index) ret.forwardingDestinationIpv6Address[1] = data[index++];
                    if (data.Length > index) ret.forwardingDestinationIpv6Address[2] = data[index++];
                    if (data.Length > index) ret.forwardingDestinationIpv6Address[3] = data[index++];
                    if (data.Length > index) ret.forwardingDestinationIpv6Address[4] = data[index++];
                    if (data.Length > index) ret.forwardingDestinationIpv6Address[5] = data[index++];
                    if (data.Length > index) ret.forwardingDestinationIpv6Address[6] = data[index++];
                    if (data.Length > index) ret.forwardingDestinationIpv6Address[7] = data[index++];
                    if (data.Length > index) ret.forwardingDestinationIpv6Address[8] = data[index++];
                    if (data.Length > index) ret.forwardingDestinationIpv6Address[9] = data[index++];
                    if (data.Length > index) ret.forwardingDestinationIpv6Address[10] = data[index++];
                    if (data.Length > index) ret.forwardingDestinationIpv6Address[11] = data[index++];
                    if (data.Length > index) ret.forwardingDestinationIpv6Address[12] = data[index++];
                    if (data.Length > index) ret.forwardingDestinationIpv6Address[13] = data[index++];
                    if (data.Length > index) ret.forwardingDestinationIpv6Address[14] = data[index++];
                    if (data.Length > index) ret.forwardingDestinationIpv6Address[15] = data[index++];
                    ret.udpPortNumber = (data.Length - index) >= udpPortNumberBytesCount ? new byte[udpPortNumberBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.udpPortNumber[0] = data[index++];
                    if (data.Length > index) ret.udpPortNumber[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](MAILBOX_CONFIGURATION_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_MAILBOX.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.mailboxCapacity != null)
                {
                    foreach (var tmp in command.mailboxCapacity)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.forwardingDestinationIpv6Address != null)
                {
                    foreach (var tmp in command.forwardingDestinationIpv6Address)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.udpPortNumber != null)
                {
                    foreach (var tmp in command.udpPortNumber)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class MAILBOX_QUEUE
        {
            public const byte ID = 0x04;
            public ByteValue sequenceNumber = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte operation
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte last
                {
                    get { return (byte)(_value >> 3 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x08; _value += (byte)(value << 3 & 0x08); }
                }
                public byte reserved
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
            public ByteValue queueHandle = 0;
            public IList<byte> mailboxEntry = new List<byte>();
            public static implicit operator MAILBOX_QUEUE(byte[] data)
            {
                MAILBOX_QUEUE ret = new MAILBOX_QUEUE();
                if (data != null)
                {
                    int index = 2;
                    ret.sequenceNumber = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.queueHandle = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.mailboxEntry = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.mailboxEntry.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](MAILBOX_QUEUE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_MAILBOX.ID);
                ret.Add(ID);
                if (command.sequenceNumber.HasValue) ret.Add(command.sequenceNumber);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.queueHandle.HasValue) ret.Add(command.queueHandle);
                if (command.mailboxEntry != null)
                {
                    foreach (var tmp in command.mailboxEntry)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class MAILBOX_WAKEUP_NOTIFICATION
        {
            public const byte ID = 0x05;
            public ByteValue queueHandle = 0;
            public static implicit operator MAILBOX_WAKEUP_NOTIFICATION(byte[] data)
            {
                MAILBOX_WAKEUP_NOTIFICATION ret = new MAILBOX_WAKEUP_NOTIFICATION();
                if (data != null)
                {
                    int index = 2;
                    ret.queueHandle = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](MAILBOX_WAKEUP_NOTIFICATION command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_MAILBOX.ID);
                ret.Add(ID);
                if (command.queueHandle.HasValue) ret.Add(command.queueHandle);
                return ret.ToArray();
            }
        }
        public partial class MAILBOX_NODE_FAILING
        {
            public const byte ID = 0x06;
            public ByteValue queueHandle = 0;
            public static implicit operator MAILBOX_NODE_FAILING(byte[] data)
            {
                MAILBOX_NODE_FAILING ret = new MAILBOX_NODE_FAILING();
                if (data != null)
                {
                    int index = 2;
                    ret.queueHandle = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](MAILBOX_NODE_FAILING command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_MAILBOX.ID);
                ret.Add(ID);
                if (command.queueHandle.HasValue) ret.Add(command.queueHandle);
                return ret.ToArray();
            }
        }
    }
}

