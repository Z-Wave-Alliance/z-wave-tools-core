/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_PROTECTION_V2
    {
        public const byte ID = 0x75;
        public const byte VERSION = 2;
        public partial class PROTECTION_EC_GET
        {
            public const byte ID = 0x07;
            public static implicit operator PROTECTION_EC_GET(byte[] data)
            {
                PROTECTION_EC_GET ret = new PROTECTION_EC_GET();
                return ret;
            }
            public static implicit operator byte[](PROTECTION_EC_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_PROTECTION_V2.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class PROTECTION_EC_REPORT
        {
            public const byte ID = 0x08;
            public ByteValue nodeId = 0;
            public static implicit operator PROTECTION_EC_REPORT(byte[] data)
            {
                PROTECTION_EC_REPORT ret = new PROTECTION_EC_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.nodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](PROTECTION_EC_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_PROTECTION_V2.ID);
                ret.Add(ID);
                if (command.nodeId.HasValue) ret.Add(command.nodeId);
                return ret.ToArray();
            }
        }
        public partial class PROTECTION_EC_SET
        {
            public const byte ID = 0x06;
            public ByteValue nodeId = 0;
            public static implicit operator PROTECTION_EC_SET(byte[] data)
            {
                PROTECTION_EC_SET ret = new PROTECTION_EC_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.nodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](PROTECTION_EC_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_PROTECTION_V2.ID);
                ret.Add(ID);
                if (command.nodeId.HasValue) ret.Add(command.nodeId);
                return ret.ToArray();
            }
        }
        public partial class PROTECTION_GET
        {
            public const byte ID = 0x02;
            public static implicit operator PROTECTION_GET(byte[] data)
            {
                PROTECTION_GET ret = new PROTECTION_GET();
                return ret;
            }
            public static implicit operator byte[](PROTECTION_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_PROTECTION_V2.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class PROTECTION_REPORT
        {
            public const byte ID = 0x03;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte localProtectionState
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
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte rfProtectionState
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte reserved2
                {
                    get { return (byte)(_value >> 4 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0xF0; _value += (byte)(value << 4 & 0xF0); }
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
            public static implicit operator PROTECTION_REPORT(byte[] data)
            {
                PROTECTION_REPORT ret = new PROTECTION_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](PROTECTION_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_PROTECTION_V2.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                return ret.ToArray();
            }
        }
        public partial class PROTECTION_SET
        {
            public const byte ID = 0x01;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte localProtectionState
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
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte rfProtectionState
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte reserved2
                {
                    get { return (byte)(_value >> 4 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0xF0; _value += (byte)(value << 4 & 0xF0); }
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
            public static implicit operator PROTECTION_SET(byte[] data)
            {
                PROTECTION_SET ret = new PROTECTION_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](PROTECTION_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_PROTECTION_V2.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                return ret.ToArray();
            }
        }
        public partial class PROTECTION_SUPPORTED_GET
        {
            public const byte ID = 0x04;
            public static implicit operator PROTECTION_SUPPORTED_GET(byte[] data)
            {
                PROTECTION_SUPPORTED_GET ret = new PROTECTION_SUPPORTED_GET();
                return ret;
            }
            public static implicit operator byte[](PROTECTION_SUPPORTED_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_PROTECTION_V2.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class PROTECTION_SUPPORTED_REPORT
        {
            public const byte ID = 0x05;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte timeout
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte exclusiveControl
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
            public const byte localProtectionStateBytesCount = 2;
            public byte[] localProtectionState = new byte[localProtectionStateBytesCount];
            public const byte rfProtectionStateBytesCount = 2;
            public byte[] rfProtectionState = new byte[rfProtectionStateBytesCount];
            public static implicit operator PROTECTION_SUPPORTED_REPORT(byte[] data)
            {
                PROTECTION_SUPPORTED_REPORT ret = new PROTECTION_SUPPORTED_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.localProtectionState = (data.Length - index) >= localProtectionStateBytesCount ? new byte[localProtectionStateBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.localProtectionState[0] = data[index++];
                    if (data.Length > index) ret.localProtectionState[1] = data[index++];
                    ret.rfProtectionState = (data.Length - index) >= rfProtectionStateBytesCount ? new byte[rfProtectionStateBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.rfProtectionState[0] = data[index++];
                    if (data.Length > index) ret.rfProtectionState[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](PROTECTION_SUPPORTED_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_PROTECTION_V2.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.localProtectionState != null)
                {
                    foreach (var tmp in command.localProtectionState)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.rfProtectionState != null)
                {
                    foreach (var tmp in command.rfProtectionState)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class PROTECTION_TIMEOUT_GET
        {
            public const byte ID = 0x0A;
            public static implicit operator PROTECTION_TIMEOUT_GET(byte[] data)
            {
                PROTECTION_TIMEOUT_GET ret = new PROTECTION_TIMEOUT_GET();
                return ret;
            }
            public static implicit operator byte[](PROTECTION_TIMEOUT_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_PROTECTION_V2.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class PROTECTION_TIMEOUT_REPORT
        {
            public const byte ID = 0x0B;
            public ByteValue timeout = 0;
            public static implicit operator PROTECTION_TIMEOUT_REPORT(byte[] data)
            {
                PROTECTION_TIMEOUT_REPORT ret = new PROTECTION_TIMEOUT_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.timeout = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](PROTECTION_TIMEOUT_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_PROTECTION_V2.ID);
                ret.Add(ID);
                if (command.timeout.HasValue) ret.Add(command.timeout);
                return ret.ToArray();
            }
        }
        public partial class PROTECTION_TIMEOUT_SET
        {
            public const byte ID = 0x09;
            public ByteValue timeout = 0;
            public static implicit operator PROTECTION_TIMEOUT_SET(byte[] data)
            {
                PROTECTION_TIMEOUT_SET ret = new PROTECTION_TIMEOUT_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.timeout = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](PROTECTION_TIMEOUT_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_PROTECTION_V2.ID);
                ret.Add(ID);
                if (command.timeout.HasValue) ret.Add(command.timeout);
                return ret.ToArray();
            }
        }
    }
}

