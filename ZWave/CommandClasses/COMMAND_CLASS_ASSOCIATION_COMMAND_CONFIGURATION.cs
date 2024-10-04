/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_ASSOCIATION_COMMAND_CONFIGURATION
    {
        public const byte ID = 0x9B;
        public const byte VERSION = 1;
        public partial class COMMAND_CONFIGURATION_GET
        {
            public const byte ID = 0x04;
            public ByteValue groupingIdentifier = 0;
            public ByteValue nodeId = 0;
            public static implicit operator COMMAND_CONFIGURATION_GET(byte[] data)
            {
                COMMAND_CONFIGURATION_GET ret = new COMMAND_CONFIGURATION_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.groupingIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](COMMAND_CONFIGURATION_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ASSOCIATION_COMMAND_CONFIGURATION.ID);
                ret.Add(ID);
                if (command.groupingIdentifier.HasValue) ret.Add(command.groupingIdentifier);
                if (command.nodeId.HasValue) ret.Add(command.nodeId);
                return ret.ToArray();
            }
        }
        public partial class COMMAND_CONFIGURATION_REPORT
        {
            public const byte ID = 0x05;
            public ByteValue groupingIdentifier = 0;
            public ByteValue nodeId = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte reportsToFollow
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 4 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x70; _value += (byte)(value << 4 & 0x70); }
                }
                public byte first
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
            public ByteValue commandLength = 0;
            public ByteValue commandClassIdentifier = 0;
            public ByteValue commandIdentifier = 0;
            public IList<byte> commandByte = new List<byte>();
            public static implicit operator COMMAND_CONFIGURATION_REPORT(byte[] data)
            {
                COMMAND_CONFIGURATION_REPORT ret = new COMMAND_CONFIGURATION_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.groupingIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.commandLength = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.commandClassIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.commandIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.commandByte = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.commandByte.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](COMMAND_CONFIGURATION_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ASSOCIATION_COMMAND_CONFIGURATION.ID);
                ret.Add(ID);
                if (command.groupingIdentifier.HasValue) ret.Add(command.groupingIdentifier);
                if (command.nodeId.HasValue) ret.Add(command.nodeId);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.commandLength.HasValue) ret.Add(command.commandLength);
                if (command.commandClassIdentifier.HasValue) ret.Add(command.commandClassIdentifier);
                if (command.commandIdentifier.HasValue) ret.Add(command.commandIdentifier);
                if (command.commandByte != null)
                {
                    foreach (var tmp in command.commandByte)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class COMMAND_CONFIGURATION_SET
        {
            public const byte ID = 0x03;
            public ByteValue groupingIdentifier = 0;
            public ByteValue nodeId = 0;
            public ByteValue commandLength = 0;
            public ByteValue commandClassIdentifier = 0;
            public ByteValue commandIdentifier = 0;
            public IList<byte> commandByte = new List<byte>();
            public static implicit operator COMMAND_CONFIGURATION_SET(byte[] data)
            {
                COMMAND_CONFIGURATION_SET ret = new COMMAND_CONFIGURATION_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.groupingIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.commandLength = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.commandClassIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.commandIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.commandByte = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.commandByte.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](COMMAND_CONFIGURATION_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ASSOCIATION_COMMAND_CONFIGURATION.ID);
                ret.Add(ID);
                if (command.groupingIdentifier.HasValue) ret.Add(command.groupingIdentifier);
                if (command.nodeId.HasValue) ret.Add(command.nodeId);
                if (command.commandLength.HasValue) ret.Add(command.commandLength);
                if (command.commandClassIdentifier.HasValue) ret.Add(command.commandClassIdentifier);
                if (command.commandIdentifier.HasValue) ret.Add(command.commandIdentifier);
                if (command.commandByte != null)
                {
                    foreach (var tmp in command.commandByte)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class COMMAND_RECORDS_SUPPORTED_GET
        {
            public const byte ID = 0x01;
            public static implicit operator COMMAND_RECORDS_SUPPORTED_GET(byte[] data)
            {
                COMMAND_RECORDS_SUPPORTED_GET ret = new COMMAND_RECORDS_SUPPORTED_GET();
                return ret;
            }
            public static implicit operator byte[](COMMAND_RECORDS_SUPPORTED_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ASSOCIATION_COMMAND_CONFIGURATION.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class COMMAND_RECORDS_SUPPORTED_REPORT
        {
            public const byte ID = 0x02;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte confCmd
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte vC
                {
                    get { return (byte)(_value >> 1 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x02; _value += (byte)(value << 1 & 0x02); }
                }
                public byte maxCommandLength
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
            public const byte freeCommandRecordsBytesCount = 2;
            public byte[] freeCommandRecords = new byte[freeCommandRecordsBytesCount];
            public const byte maxCommandRecordsBytesCount = 2;
            public byte[] maxCommandRecords = new byte[maxCommandRecordsBytesCount];
            public static implicit operator COMMAND_RECORDS_SUPPORTED_REPORT(byte[] data)
            {
                COMMAND_RECORDS_SUPPORTED_REPORT ret = new COMMAND_RECORDS_SUPPORTED_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.freeCommandRecords = (data.Length - index) >= freeCommandRecordsBytesCount ? new byte[freeCommandRecordsBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.freeCommandRecords[0] = data[index++];
                    if (data.Length > index) ret.freeCommandRecords[1] = data[index++];
                    ret.maxCommandRecords = (data.Length - index) >= maxCommandRecordsBytesCount ? new byte[maxCommandRecordsBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.maxCommandRecords[0] = data[index++];
                    if (data.Length > index) ret.maxCommandRecords[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](COMMAND_RECORDS_SUPPORTED_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ASSOCIATION_COMMAND_CONFIGURATION.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.freeCommandRecords != null)
                {
                    foreach (var tmp in command.freeCommandRecords)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.maxCommandRecords != null)
                {
                    foreach (var tmp in command.maxCommandRecords)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

