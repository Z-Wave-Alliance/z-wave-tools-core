/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_THERMOSTAT_OPERATING_STATE_V2
    {
        public const byte ID = 0x42;
        public const byte VERSION = 2;
        public partial class THERMOSTAT_OPERATING_STATE_GET
        {
            public const byte ID = 0x02;
            public static implicit operator THERMOSTAT_OPERATING_STATE_GET(byte[] data)
            {
                THERMOSTAT_OPERATING_STATE_GET ret = new THERMOSTAT_OPERATING_STATE_GET();
                return ret;
            }
            public static implicit operator byte[](THERMOSTAT_OPERATING_STATE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_THERMOSTAT_OPERATING_STATE_V2.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class THERMOSTAT_OPERATING_STATE_REPORT
        {
            public const byte ID = 0x03;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte operatingState
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
            public static implicit operator THERMOSTAT_OPERATING_STATE_REPORT(byte[] data)
            {
                THERMOSTAT_OPERATING_STATE_REPORT ret = new THERMOSTAT_OPERATING_STATE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](THERMOSTAT_OPERATING_STATE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_THERMOSTAT_OPERATING_STATE_V2.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                return ret.ToArray();
            }
        }
        public partial class THERMOSTAT_OPERATING_STATE_LOGGING_SUPPORTED_GET
        {
            public const byte ID = 0x01;
            public static implicit operator THERMOSTAT_OPERATING_STATE_LOGGING_SUPPORTED_GET(byte[] data)
            {
                THERMOSTAT_OPERATING_STATE_LOGGING_SUPPORTED_GET ret = new THERMOSTAT_OPERATING_STATE_LOGGING_SUPPORTED_GET();
                return ret;
            }
            public static implicit operator byte[](THERMOSTAT_OPERATING_STATE_LOGGING_SUPPORTED_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_THERMOSTAT_OPERATING_STATE_V2.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class THERMOSTAT_OPERATING_LOGGING_SUPPORTED_REPORT
        {
            public const byte ID = 0x04;
            public IList<byte> bitMask = new List<byte>();
            public static implicit operator THERMOSTAT_OPERATING_LOGGING_SUPPORTED_REPORT(byte[] data)
            {
                THERMOSTAT_OPERATING_LOGGING_SUPPORTED_REPORT ret = new THERMOSTAT_OPERATING_LOGGING_SUPPORTED_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.bitMask = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.bitMask.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](THERMOSTAT_OPERATING_LOGGING_SUPPORTED_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_THERMOSTAT_OPERATING_STATE_V2.ID);
                ret.Add(ID);
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
        public partial class THERMOSTAT_OPERATING_STATE_LOGGING_GET
        {
            public const byte ID = 0x05;
            public IList<byte> bitMask = new List<byte>();
            public static implicit operator THERMOSTAT_OPERATING_STATE_LOGGING_GET(byte[] data)
            {
                THERMOSTAT_OPERATING_STATE_LOGGING_GET ret = new THERMOSTAT_OPERATING_STATE_LOGGING_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.bitMask = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.bitMask.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](THERMOSTAT_OPERATING_STATE_LOGGING_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_THERMOSTAT_OPERATING_STATE_V2.ID);
                ret.Add(ID);
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
        public partial class THERMOSTAT_OPERATING_STATE_LOGGING_REPORT
        {
            public const byte ID = 0x06;
            public ByteValue reportsToFollow = 0;
            public class TVG1
            {
                public struct Tproperties1
                {
                    private byte _value;
                    public bool HasValue { get; private set; }
                    public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                    public byte operatingStateLogType
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
                public ByteValue usageTodayHours = 0;
                public ByteValue usageTodayMinutes = 0;
                public ByteValue usageYesterdayHours = 0;
                public ByteValue usageYesterdayMinutes = 0;
            }
            public List<TVG1> vg1 = new List<TVG1>();
            public static implicit operator THERMOSTAT_OPERATING_STATE_LOGGING_REPORT(byte[] data)
            {
                THERMOSTAT_OPERATING_STATE_LOGGING_REPORT ret = new THERMOSTAT_OPERATING_STATE_LOGGING_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.reportsToFollow = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.vg1 = new List<TVG1>();
                    while (data.Length - 0 > index)
                    {
                        TVG1 tmp = new TVG1();
                        tmp.properties1 = data.Length > index ? (TVG1.Tproperties1)data[index++] : TVG1.Tproperties1.Empty;
                        tmp.usageTodayHours = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.usageTodayMinutes = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.usageYesterdayHours = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.usageYesterdayMinutes = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        ret.vg1.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](THERMOSTAT_OPERATING_STATE_LOGGING_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_THERMOSTAT_OPERATING_STATE_V2.ID);
                ret.Add(ID);
                if (command.reportsToFollow.HasValue) ret.Add(command.reportsToFollow);
                if (command.vg1 != null)
                {
                    foreach (var item in command.vg1)
                    {
                        if (item.properties1.HasValue) ret.Add(item.properties1);
                        if (item.usageTodayHours.HasValue) ret.Add(item.usageTodayHours);
                        if (item.usageTodayMinutes.HasValue) ret.Add(item.usageTodayMinutes);
                        if (item.usageYesterdayHours.HasValue) ret.Add(item.usageYesterdayHours);
                        if (item.usageYesterdayMinutes.HasValue) ret.Add(item.usageYesterdayMinutes);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

