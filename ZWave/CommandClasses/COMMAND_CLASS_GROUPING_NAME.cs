/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_GROUPING_NAME
    {
        public const byte ID = 0x7B;
        public const byte VERSION = 1;
        public partial class GROUPING_NAME_GET
        {
            public const byte ID = 0x02;
            public ByteValue groupingIdentifier = 0;
            public static implicit operator GROUPING_NAME_GET(byte[] data)
            {
                GROUPING_NAME_GET ret = new GROUPING_NAME_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.groupingIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](GROUPING_NAME_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_GROUPING_NAME.ID);
                ret.Add(ID);
                if (command.groupingIdentifier.HasValue) ret.Add(command.groupingIdentifier);
                return ret.ToArray();
            }
        }
        public partial class GROUPING_NAME_REPORT
        {
            public const byte ID = 0x03;
            public ByteValue groupingIdentifier = 0;
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
            public const byte groupingNameBytesCount = 16;
            public byte[] groupingName = new byte[groupingNameBytesCount];
            public static implicit operator GROUPING_NAME_REPORT(byte[] data)
            {
                GROUPING_NAME_REPORT ret = new GROUPING_NAME_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.groupingIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.groupingName = (data.Length - index) >= groupingNameBytesCount ? new byte[groupingNameBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.groupingName[0] = data[index++];
                    if (data.Length > index) ret.groupingName[1] = data[index++];
                    if (data.Length > index) ret.groupingName[2] = data[index++];
                    if (data.Length > index) ret.groupingName[3] = data[index++];
                    if (data.Length > index) ret.groupingName[4] = data[index++];
                    if (data.Length > index) ret.groupingName[5] = data[index++];
                    if (data.Length > index) ret.groupingName[6] = data[index++];
                    if (data.Length > index) ret.groupingName[7] = data[index++];
                    if (data.Length > index) ret.groupingName[8] = data[index++];
                    if (data.Length > index) ret.groupingName[9] = data[index++];
                    if (data.Length > index) ret.groupingName[10] = data[index++];
                    if (data.Length > index) ret.groupingName[11] = data[index++];
                    if (data.Length > index) ret.groupingName[12] = data[index++];
                    if (data.Length > index) ret.groupingName[13] = data[index++];
                    if (data.Length > index) ret.groupingName[14] = data[index++];
                    if (data.Length > index) ret.groupingName[15] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](GROUPING_NAME_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_GROUPING_NAME.ID);
                ret.Add(ID);
                if (command.groupingIdentifier.HasValue) ret.Add(command.groupingIdentifier);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.groupingName != null)
                {
                    foreach (var tmp in command.groupingName)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class GROUPING_NAME_SET
        {
            public const byte ID = 0x01;
            public ByteValue groupingIdentifier = 0;
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
            public const byte groupingNameBytesCount = 16;
            public byte[] groupingName = new byte[groupingNameBytesCount];
            public static implicit operator GROUPING_NAME_SET(byte[] data)
            {
                GROUPING_NAME_SET ret = new GROUPING_NAME_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.groupingIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.groupingName = (data.Length - index) >= groupingNameBytesCount ? new byte[groupingNameBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.groupingName[0] = data[index++];
                    if (data.Length > index) ret.groupingName[1] = data[index++];
                    if (data.Length > index) ret.groupingName[2] = data[index++];
                    if (data.Length > index) ret.groupingName[3] = data[index++];
                    if (data.Length > index) ret.groupingName[4] = data[index++];
                    if (data.Length > index) ret.groupingName[5] = data[index++];
                    if (data.Length > index) ret.groupingName[6] = data[index++];
                    if (data.Length > index) ret.groupingName[7] = data[index++];
                    if (data.Length > index) ret.groupingName[8] = data[index++];
                    if (data.Length > index) ret.groupingName[9] = data[index++];
                    if (data.Length > index) ret.groupingName[10] = data[index++];
                    if (data.Length > index) ret.groupingName[11] = data[index++];
                    if (data.Length > index) ret.groupingName[12] = data[index++];
                    if (data.Length > index) ret.groupingName[13] = data[index++];
                    if (data.Length > index) ret.groupingName[14] = data[index++];
                    if (data.Length > index) ret.groupingName[15] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](GROUPING_NAME_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_GROUPING_NAME.ID);
                ret.Add(ID);
                if (command.groupingIdentifier.HasValue) ret.Add(command.groupingIdentifier);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.groupingName != null)
                {
                    foreach (var tmp in command.groupingName)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

