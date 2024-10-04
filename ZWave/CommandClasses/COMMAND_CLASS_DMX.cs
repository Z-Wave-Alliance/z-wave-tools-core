/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_DMX
    {
        public const byte ID = 0x65;
        public const byte VERSION = 1;
        public partial class DMX_ADDRESS_SET
        {
            public const byte ID = 0x01;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte pageId
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
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
            public ByteValue channelId = 0;
            public static implicit operator DMX_ADDRESS_SET(byte[] data)
            {
                DMX_ADDRESS_SET ret = new DMX_ADDRESS_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.channelId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](DMX_ADDRESS_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_DMX.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.channelId.HasValue) ret.Add(command.channelId);
                return ret.ToArray();
            }
        }
        public partial class DMX_ADDRESS_GET
        {
            public const byte ID = 0x02;
            public static implicit operator DMX_ADDRESS_GET(byte[] data)
            {
                DMX_ADDRESS_GET ret = new DMX_ADDRESS_GET();
                return ret;
            }
            public static implicit operator byte[](DMX_ADDRESS_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_DMX.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class DMX_ADDRESS_REPORT
        {
            public const byte ID = 0x03;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte pageId
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
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
            public ByteValue channelId = 0;
            public static implicit operator DMX_ADDRESS_REPORT(byte[] data)
            {
                DMX_ADDRESS_REPORT ret = new DMX_ADDRESS_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.channelId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](DMX_ADDRESS_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_DMX.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.channelId.HasValue) ret.Add(command.channelId);
                return ret.ToArray();
            }
        }
        public partial class DMX_CAPABILITY_GET
        {
            public const byte ID = 0x04;
            public ByteValue channelId = 0;
            public static implicit operator DMX_CAPABILITY_GET(byte[] data)
            {
                DMX_CAPABILITY_GET ret = new DMX_CAPABILITY_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.channelId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](DMX_CAPABILITY_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_DMX.ID);
                ret.Add(ID);
                if (command.channelId.HasValue) ret.Add(command.channelId);
                return ret.ToArray();
            }
        }
        public partial class DMX_CAPABILITY_REPORT
        {
            public const byte ID = 0x05;
            public ByteValue channelId = 0;
            public const byte propertyIdBytesCount = 2;
            public byte[] propertyId = new byte[propertyIdBytesCount];
            public ByteValue deviceChannels = 0;
            public ByteValue maxChannels = 0;
            public static implicit operator DMX_CAPABILITY_REPORT(byte[] data)
            {
                DMX_CAPABILITY_REPORT ret = new DMX_CAPABILITY_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.channelId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.propertyId = (data.Length - index) >= propertyIdBytesCount ? new byte[propertyIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.propertyId[0] = data[index++];
                    if (data.Length > index) ret.propertyId[1] = data[index++];
                    ret.deviceChannels = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.maxChannels = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](DMX_CAPABILITY_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_DMX.ID);
                ret.Add(ID);
                if (command.channelId.HasValue) ret.Add(command.channelId);
                if (command.propertyId != null)
                {
                    foreach (var tmp in command.propertyId)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.deviceChannels.HasValue) ret.Add(command.deviceChannels);
                if (command.maxChannels.HasValue) ret.Add(command.maxChannels);
                return ret.ToArray();
            }
        }
        public partial class DMX_DATA
        {
            public const byte ID = 0x06;
            public ByteValue source = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte page
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte sequenceNo
                {
                    get { return (byte)(_value >> 4 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x30; _value += (byte)(value << 4 & 0x30); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 6 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0xC0; _value += (byte)(value << 6 & 0xC0); }
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
            public IList<byte> dmxChannel = new List<byte>();
            public static implicit operator DMX_DATA(byte[] data)
            {
                DMX_DATA ret = new DMX_DATA();
                if (data != null)
                {
                    int index = 2;
                    ret.source = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.dmxChannel = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.dmxChannel.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](DMX_DATA command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_DMX.ID);
                ret.Add(ID);
                if (command.source.HasValue) ret.Add(command.source);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.dmxChannel != null)
                {
                    foreach (var tmp in command.dmxChannel)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

