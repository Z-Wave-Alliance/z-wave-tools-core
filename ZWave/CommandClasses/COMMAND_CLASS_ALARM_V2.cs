/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_ALARM_V2
    {
        public const byte ID = 0x71;
        public const byte VERSION = 2;
        public partial class ALARM_GET
        {
            public const byte ID = 0x04;
            public ByteValue alarmType = 0;
            public ByteValue zwaveAlarmType = 0;
            public static implicit operator ALARM_GET(byte[] data)
            {
                ALARM_GET ret = new ALARM_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.alarmType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.zwaveAlarmType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](ALARM_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ALARM_V2.ID);
                ret.Add(ID);
                if (command.alarmType.HasValue) ret.Add(command.alarmType);
                if (command.zwaveAlarmType.HasValue) ret.Add(command.zwaveAlarmType);
                return ret.ToArray();
            }
        }
        public partial class ALARM_REPORT
        {
            public const byte ID = 0x05;
            public ByteValue alarmType = 0;
            public ByteValue alarmLevel = 0;
            public ByteValue zensorNetSourceNodeId = 0;
            public ByteValue zwaveAlarmStatus = 0;
            public ByteValue zwaveAlarmType = 0;
            public ByteValue zwaveAlarmEvent = 0;
            public ByteValue numberOfEventParameters = 0;
            public IList<byte> eventParameter = new List<byte>();
            public static implicit operator ALARM_REPORT(byte[] data)
            {
                ALARM_REPORT ret = new ALARM_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.alarmType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.alarmLevel = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.zensorNetSourceNodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.zwaveAlarmStatus = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.zwaveAlarmType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.zwaveAlarmEvent = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.numberOfEventParameters = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.eventParameter = new List<byte>();
                    for (int i = 0; i < ret.numberOfEventParameters; i++)
                    {
                        if (data.Length > index) ret.eventParameter.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](ALARM_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ALARM_V2.ID);
                ret.Add(ID);
                if (command.alarmType.HasValue) ret.Add(command.alarmType);
                if (command.alarmLevel.HasValue) ret.Add(command.alarmLevel);
                if (command.zensorNetSourceNodeId.HasValue) ret.Add(command.zensorNetSourceNodeId);
                if (command.zwaveAlarmStatus.HasValue) ret.Add(command.zwaveAlarmStatus);
                if (command.zwaveAlarmType.HasValue) ret.Add(command.zwaveAlarmType);
                if (command.zwaveAlarmEvent.HasValue) ret.Add(command.zwaveAlarmEvent);
                if (command.numberOfEventParameters.HasValue) ret.Add(command.numberOfEventParameters);
                if (command.eventParameter != null)
                {
                    foreach (var tmp in command.eventParameter)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class ALARM_SET
        {
            public const byte ID = 0x06;
            public ByteValue zwaveAlarmType = 0;
            public ByteValue zwaveAlarmStatus = 0;
            public static implicit operator ALARM_SET(byte[] data)
            {
                ALARM_SET ret = new ALARM_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.zwaveAlarmType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.zwaveAlarmStatus = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](ALARM_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ALARM_V2.ID);
                ret.Add(ID);
                if (command.zwaveAlarmType.HasValue) ret.Add(command.zwaveAlarmType);
                if (command.zwaveAlarmStatus.HasValue) ret.Add(command.zwaveAlarmStatus);
                return ret.ToArray();
            }
        }
        public partial class ALARM_TYPE_SUPPORTED_GET
        {
            public const byte ID = 0x07;
            public static implicit operator ALARM_TYPE_SUPPORTED_GET(byte[] data)
            {
                ALARM_TYPE_SUPPORTED_GET ret = new ALARM_TYPE_SUPPORTED_GET();
                return ret;
            }
            public static implicit operator byte[](ALARM_TYPE_SUPPORTED_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ALARM_V2.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class ALARM_TYPE_SUPPORTED_REPORT
        {
            public const byte ID = 0x08;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte numberOfBitMasks
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 5 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x60; _value += (byte)(value << 5 & 0x60); }
                }
                public byte v1Alarm
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
            public IList<byte> bitMask = new List<byte>();
            public static implicit operator ALARM_TYPE_SUPPORTED_REPORT(byte[] data)
            {
                ALARM_TYPE_SUPPORTED_REPORT ret = new ALARM_TYPE_SUPPORTED_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.bitMask = new List<byte>();
                    for (int i = 0; i < ret.properties1.numberOfBitMasks; i++)
                    {
                        if (data.Length > index) ret.bitMask.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](ALARM_TYPE_SUPPORTED_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ALARM_V2.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.bitMask != null)
                {
                    foreach (var tmp in command.bitMask)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

