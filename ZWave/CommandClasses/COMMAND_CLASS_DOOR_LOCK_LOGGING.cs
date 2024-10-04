/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_DOOR_LOCK_LOGGING
    {
        public const byte ID = 0x4C;
        public const byte VERSION = 1;
        public partial class DOOR_LOCK_LOGGING_RECORDS_SUPPORTED_GET
        {
            public const byte ID = 0x01;
            public static implicit operator DOOR_LOCK_LOGGING_RECORDS_SUPPORTED_GET(byte[] data)
            {
                DOOR_LOCK_LOGGING_RECORDS_SUPPORTED_GET ret = new DOOR_LOCK_LOGGING_RECORDS_SUPPORTED_GET();
                return ret;
            }
            public static implicit operator byte[](DOOR_LOCK_LOGGING_RECORDS_SUPPORTED_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_DOOR_LOCK_LOGGING.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class DOOR_LOCK_LOGGING_RECORDS_SUPPORTED_REPORT
        {
            public const byte ID = 0x02;
            public ByteValue maxRecordsStored = 0;
            public static implicit operator DOOR_LOCK_LOGGING_RECORDS_SUPPORTED_REPORT(byte[] data)
            {
                DOOR_LOCK_LOGGING_RECORDS_SUPPORTED_REPORT ret = new DOOR_LOCK_LOGGING_RECORDS_SUPPORTED_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.maxRecordsStored = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](DOOR_LOCK_LOGGING_RECORDS_SUPPORTED_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_DOOR_LOCK_LOGGING.ID);
                ret.Add(ID);
                if (command.maxRecordsStored.HasValue) ret.Add(command.maxRecordsStored);
                return ret.ToArray();
            }
        }
        public partial class RECORD_GET
        {
            public const byte ID = 0x03;
            public ByteValue recordNumber = 0;
            public static implicit operator RECORD_GET(byte[] data)
            {
                RECORD_GET ret = new RECORD_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.recordNumber = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](RECORD_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_DOOR_LOCK_LOGGING.ID);
                ret.Add(ID);
                if (command.recordNumber.HasValue) ret.Add(command.recordNumber);
                return ret.ToArray();
            }
        }
        public partial class RECORD_REPORT
        {
            public const byte ID = 0x04;
            public ByteValue recordNumber = 0;
            public const byte yearBytesCount = 2;
            public byte[] year = new byte[yearBytesCount];
            public ByteValue month = 0;
            public ByteValue day = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte hourLocalTime
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte recordStatus
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
            public ByteValue minuteLocalTime = 0;
            public ByteValue secondLocalTime = 0;
            public ByteValue eventType = 0;
            public ByteValue userIdentifier = 0;
            public ByteValue userCodeLength = 0;
            public IList<byte> userCode = new List<byte>();
            public static implicit operator RECORD_REPORT(byte[] data)
            {
                RECORD_REPORT ret = new RECORD_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.recordNumber = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.year = (data.Length - index) >= yearBytesCount ? new byte[yearBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.year[0] = data[index++];
                    if (data.Length > index) ret.year[1] = data[index++];
                    ret.month = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.day = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.minuteLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.secondLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.eventType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.userIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.userCodeLength = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.userCode = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.userCode.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](RECORD_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_DOOR_LOCK_LOGGING.ID);
                ret.Add(ID);
                if (command.recordNumber.HasValue) ret.Add(command.recordNumber);
                if (command.year != null)
                {
                    foreach (var tmp in command.year)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.month.HasValue) ret.Add(command.month);
                if (command.day.HasValue) ret.Add(command.day);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.minuteLocalTime.HasValue) ret.Add(command.minuteLocalTime);
                if (command.secondLocalTime.HasValue) ret.Add(command.secondLocalTime);
                if (command.eventType.HasValue) ret.Add(command.eventType);
                if (command.userIdentifier.HasValue) ret.Add(command.userIdentifier);
                if (command.userCodeLength.HasValue) ret.Add(command.userCodeLength);
                if (command.userCode != null)
                {
                    foreach (var tmp in command.userCode)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

