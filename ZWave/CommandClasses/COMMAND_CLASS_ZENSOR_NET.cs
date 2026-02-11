/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_ZENSOR_NET
    {
        public const byte ID = 0x02;
        public const byte VERSION = 1;
        public partial class BIND_ACCEPT
        {
            public const byte ID = 0x02;
            public const byte zensorNetIdBytesCount = 2;
            public byte[] zensorNetId = new byte[zensorNetIdBytesCount];
            public ByteValue sourceZensorId = 0;
            public ByteValue destinationZensorId = 0;
            public const byte destinationZensorNetIdBytesCount = 2;
            public byte[] destinationZensorNetId = new byte[destinationZensorNetIdBytesCount];
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte accept
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte broadcastId
                {
                    get { return (byte)(_value >> 1 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x1E; _value += (byte)(value << 1 & 0x1E); }
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
            public ByteValue assignZensorId = 0;
            public static implicit operator BIND_ACCEPT(byte[] data)
            {
                BIND_ACCEPT ret = new BIND_ACCEPT();
                if (data != null)
                {
                    int index = 2;
                    ret.zensorNetId = (data.Length - index) >= zensorNetIdBytesCount ? new byte[zensorNetIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.zensorNetId[0] = data[index++];
                    if (data.Length > index) ret.zensorNetId[1] = data[index++];
                    ret.sourceZensorId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.destinationZensorId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.destinationZensorNetId = (data.Length - index) >= destinationZensorNetIdBytesCount ? new byte[destinationZensorNetIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.destinationZensorNetId[0] = data[index++];
                    if (data.Length > index) ret.destinationZensorNetId[1] = data[index++];
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.assignZensorId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](BIND_ACCEPT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZENSOR_NET.ID);
                ret.Add(ID);
                if (command.zensorNetId != null)
                {
                    foreach (var tmp in command.zensorNetId)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.sourceZensorId.HasValue) ret.Add(command.sourceZensorId);
                if (command.destinationZensorId.HasValue) ret.Add(command.destinationZensorId);
                if (command.destinationZensorNetId != null)
                {
                    foreach (var tmp in command.destinationZensorNetId)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.assignZensorId.HasValue) ret.Add(command.assignZensorId);
                return ret.ToArray();
            }
        }
        public partial class BIND_COMPLETE
        {
            public const byte ID = 0x03;
            public const byte zensorNetIdBytesCount = 2;
            public byte[] zensorNetId = new byte[zensorNetIdBytesCount];
            public ByteValue sourceZensorId = 0;
            public ByteValue destinationZensorId = 0;
            public static implicit operator BIND_COMPLETE(byte[] data)
            {
                BIND_COMPLETE ret = new BIND_COMPLETE();
                if (data != null)
                {
                    int index = 2;
                    ret.zensorNetId = (data.Length - index) >= zensorNetIdBytesCount ? new byte[zensorNetIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.zensorNetId[0] = data[index++];
                    if (data.Length > index) ret.zensorNetId[1] = data[index++];
                    ret.sourceZensorId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.destinationZensorId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](BIND_COMPLETE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZENSOR_NET.ID);
                ret.Add(ID);
                if (command.zensorNetId != null)
                {
                    foreach (var tmp in command.zensorNetId)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.sourceZensorId.HasValue) ret.Add(command.sourceZensorId);
                if (command.destinationZensorId.HasValue) ret.Add(command.destinationZensorId);
                return ret.ToArray();
            }
        }
        public partial class BIND_REQUEST
        {
            public const byte ID = 0x01;
            public const byte zensorNetIdBytesCount = 2;
            public byte[] zensorNetId = new byte[zensorNetIdBytesCount];
            public ByteValue sourceZensorId = 0;
            public ByteValue destinationZensorId = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte bind
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte unbind
                {
                    get { return (byte)(_value >> 1 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x02; _value += (byte)(value << 1 & 0x02); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 2 & 0x3F); }
                    set { HasValue = true; _value &= 0xFF - 0xFC; _value += (byte)(value << 2 & 0xFC); }
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
            public ByteValue zensorCapabilities = 0;
            public static implicit operator BIND_REQUEST(byte[] data)
            {
                BIND_REQUEST ret = new BIND_REQUEST();
                if (data != null)
                {
                    int index = 2;
                    ret.zensorNetId = (data.Length - index) >= zensorNetIdBytesCount ? new byte[zensorNetIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.zensorNetId[0] = data[index++];
                    if (data.Length > index) ret.zensorNetId[1] = data[index++];
                    ret.sourceZensorId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.destinationZensorId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.zensorCapabilities = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](BIND_REQUEST command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZENSOR_NET.ID);
                ret.Add(ID);
                if (command.zensorNetId != null)
                {
                    foreach (var tmp in command.zensorNetId)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.sourceZensorId.HasValue) ret.Add(command.sourceZensorId);
                if (command.destinationZensorId.HasValue) ret.Add(command.destinationZensorId);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.zensorCapabilities.HasValue) ret.Add(command.zensorCapabilities);
                return ret.ToArray();
            }
        }
    }
}

