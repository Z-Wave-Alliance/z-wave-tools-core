/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_NODE_NAMING
    {
        public const byte ID = 0x77;
        public const byte VERSION = 1;
        public partial class NODE_NAMING_NODE_LOCATION_REPORT
        {
            public const byte ID = 0x06;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte charPresentation
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
            public const byte nodeLocationCharBytesCount = 16;
            public byte[] nodeLocationChar = new byte[nodeLocationCharBytesCount];
            public static implicit operator NODE_NAMING_NODE_LOCATION_REPORT(byte[] data)
            {
                NODE_NAMING_NODE_LOCATION_REPORT ret = new NODE_NAMING_NODE_LOCATION_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.nodeLocationChar = (data.Length - index) >= nodeLocationCharBytesCount ? new byte[nodeLocationCharBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.nodeLocationChar[0] = data[index++];
                    if (data.Length > index) ret.nodeLocationChar[1] = data[index++];
                    if (data.Length > index) ret.nodeLocationChar[2] = data[index++];
                    if (data.Length > index) ret.nodeLocationChar[3] = data[index++];
                    if (data.Length > index) ret.nodeLocationChar[4] = data[index++];
                    if (data.Length > index) ret.nodeLocationChar[5] = data[index++];
                    if (data.Length > index) ret.nodeLocationChar[6] = data[index++];
                    if (data.Length > index) ret.nodeLocationChar[7] = data[index++];
                    if (data.Length > index) ret.nodeLocationChar[8] = data[index++];
                    if (data.Length > index) ret.nodeLocationChar[9] = data[index++];
                    if (data.Length > index) ret.nodeLocationChar[10] = data[index++];
                    if (data.Length > index) ret.nodeLocationChar[11] = data[index++];
                    if (data.Length > index) ret.nodeLocationChar[12] = data[index++];
                    if (data.Length > index) ret.nodeLocationChar[13] = data[index++];
                    if (data.Length > index) ret.nodeLocationChar[14] = data[index++];
                    if (data.Length > index) ret.nodeLocationChar[15] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](NODE_NAMING_NODE_LOCATION_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NODE_NAMING.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.nodeLocationChar != null)
                {
                    foreach (var tmp in command.nodeLocationChar)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class NODE_NAMING_NODE_LOCATION_SET
        {
            public const byte ID = 0x04;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte charPresentation
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
            public const byte nodeLocationCharBytesCount = 16;
            public byte[] nodeLocationChar = new byte[nodeLocationCharBytesCount];
            public static implicit operator NODE_NAMING_NODE_LOCATION_SET(byte[] data)
            {
                NODE_NAMING_NODE_LOCATION_SET ret = new NODE_NAMING_NODE_LOCATION_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.nodeLocationChar = (data.Length - index) >= nodeLocationCharBytesCount ? new byte[nodeLocationCharBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.nodeLocationChar[0] = data[index++];
                    if (data.Length > index) ret.nodeLocationChar[1] = data[index++];
                    if (data.Length > index) ret.nodeLocationChar[2] = data[index++];
                    if (data.Length > index) ret.nodeLocationChar[3] = data[index++];
                    if (data.Length > index) ret.nodeLocationChar[4] = data[index++];
                    if (data.Length > index) ret.nodeLocationChar[5] = data[index++];
                    if (data.Length > index) ret.nodeLocationChar[6] = data[index++];
                    if (data.Length > index) ret.nodeLocationChar[7] = data[index++];
                    if (data.Length > index) ret.nodeLocationChar[8] = data[index++];
                    if (data.Length > index) ret.nodeLocationChar[9] = data[index++];
                    if (data.Length > index) ret.nodeLocationChar[10] = data[index++];
                    if (data.Length > index) ret.nodeLocationChar[11] = data[index++];
                    if (data.Length > index) ret.nodeLocationChar[12] = data[index++];
                    if (data.Length > index) ret.nodeLocationChar[13] = data[index++];
                    if (data.Length > index) ret.nodeLocationChar[14] = data[index++];
                    if (data.Length > index) ret.nodeLocationChar[15] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](NODE_NAMING_NODE_LOCATION_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NODE_NAMING.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.nodeLocationChar != null)
                {
                    foreach (var tmp in command.nodeLocationChar)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class NODE_NAMING_NODE_LOCATION_GET
        {
            public const byte ID = 0x05;
            public static implicit operator NODE_NAMING_NODE_LOCATION_GET(byte[] data)
            {
                NODE_NAMING_NODE_LOCATION_GET ret = new NODE_NAMING_NODE_LOCATION_GET();
                return ret;
            }
            public static implicit operator byte[](NODE_NAMING_NODE_LOCATION_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NODE_NAMING.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class NODE_NAMING_NODE_NAME_GET
        {
            public const byte ID = 0x02;
            public static implicit operator NODE_NAMING_NODE_NAME_GET(byte[] data)
            {
                NODE_NAMING_NODE_NAME_GET ret = new NODE_NAMING_NODE_NAME_GET();
                return ret;
            }
            public static implicit operator byte[](NODE_NAMING_NODE_NAME_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NODE_NAMING.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class NODE_NAMING_NODE_NAME_REPORT
        {
            public const byte ID = 0x03;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte charPresentation
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
            public const byte nodeNameCharBytesCount = 16;
            public byte[] nodeNameChar = new byte[nodeNameCharBytesCount];
            public static implicit operator NODE_NAMING_NODE_NAME_REPORT(byte[] data)
            {
                NODE_NAMING_NODE_NAME_REPORT ret = new NODE_NAMING_NODE_NAME_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.nodeNameChar = (data.Length - index) >= nodeNameCharBytesCount ? new byte[nodeNameCharBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.nodeNameChar[0] = data[index++];
                    if (data.Length > index) ret.nodeNameChar[1] = data[index++];
                    if (data.Length > index) ret.nodeNameChar[2] = data[index++];
                    if (data.Length > index) ret.nodeNameChar[3] = data[index++];
                    if (data.Length > index) ret.nodeNameChar[4] = data[index++];
                    if (data.Length > index) ret.nodeNameChar[5] = data[index++];
                    if (data.Length > index) ret.nodeNameChar[6] = data[index++];
                    if (data.Length > index) ret.nodeNameChar[7] = data[index++];
                    if (data.Length > index) ret.nodeNameChar[8] = data[index++];
                    if (data.Length > index) ret.nodeNameChar[9] = data[index++];
                    if (data.Length > index) ret.nodeNameChar[10] = data[index++];
                    if (data.Length > index) ret.nodeNameChar[11] = data[index++];
                    if (data.Length > index) ret.nodeNameChar[12] = data[index++];
                    if (data.Length > index) ret.nodeNameChar[13] = data[index++];
                    if (data.Length > index) ret.nodeNameChar[14] = data[index++];
                    if (data.Length > index) ret.nodeNameChar[15] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](NODE_NAMING_NODE_NAME_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NODE_NAMING.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.nodeNameChar != null)
                {
                    foreach (var tmp in command.nodeNameChar)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class NODE_NAMING_NODE_NAME_SET
        {
            public const byte ID = 0x01;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte charPresentation
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
            public const byte nodeNameCharBytesCount = 16;
            public byte[] nodeNameChar = new byte[nodeNameCharBytesCount];
            public static implicit operator NODE_NAMING_NODE_NAME_SET(byte[] data)
            {
                NODE_NAMING_NODE_NAME_SET ret = new NODE_NAMING_NODE_NAME_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.nodeNameChar = (data.Length - index) >= nodeNameCharBytesCount ? new byte[nodeNameCharBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.nodeNameChar[0] = data[index++];
                    if (data.Length > index) ret.nodeNameChar[1] = data[index++];
                    if (data.Length > index) ret.nodeNameChar[2] = data[index++];
                    if (data.Length > index) ret.nodeNameChar[3] = data[index++];
                    if (data.Length > index) ret.nodeNameChar[4] = data[index++];
                    if (data.Length > index) ret.nodeNameChar[5] = data[index++];
                    if (data.Length > index) ret.nodeNameChar[6] = data[index++];
                    if (data.Length > index) ret.nodeNameChar[7] = data[index++];
                    if (data.Length > index) ret.nodeNameChar[8] = data[index++];
                    if (data.Length > index) ret.nodeNameChar[9] = data[index++];
                    if (data.Length > index) ret.nodeNameChar[10] = data[index++];
                    if (data.Length > index) ret.nodeNameChar[11] = data[index++];
                    if (data.Length > index) ret.nodeNameChar[12] = data[index++];
                    if (data.Length > index) ret.nodeNameChar[13] = data[index++];
                    if (data.Length > index) ret.nodeNameChar[14] = data[index++];
                    if (data.Length > index) ret.nodeNameChar[15] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](NODE_NAMING_NODE_NAME_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NODE_NAMING.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.nodeNameChar != null)
                {
                    foreach (var tmp in command.nodeNameChar)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

