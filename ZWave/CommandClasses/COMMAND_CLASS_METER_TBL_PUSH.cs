/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_METER_TBL_PUSH
    {
        public const byte ID = 0x3E;
        public const byte VERSION = 1;
        public partial class METER_TBL_PUSH_CONFIGURATION_GET
        {
            public const byte ID = 0x02;
            public static implicit operator METER_TBL_PUSH_CONFIGURATION_GET(byte[] data)
            {
                METER_TBL_PUSH_CONFIGURATION_GET ret = new METER_TBL_PUSH_CONFIGURATION_GET();
                return ret;
            }
            public static implicit operator byte[](METER_TBL_PUSH_CONFIGURATION_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_METER_TBL_PUSH.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class METER_TBL_PUSH_CONFIGURATION_REPORT
        {
            public const byte ID = 0x03;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte operatingStatusPushMode
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte ps
                {
                    get { return (byte)(_value >> 4 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x10; _value += (byte)(value << 4 & 0x10); }
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
            public const byte pushDatasetBytesCount = 3;
            public byte[] pushDataset = new byte[pushDatasetBytesCount];
            public ByteValue intervalMonths = 0;
            public ByteValue intervalDays = 0;
            public ByteValue intervalHours = 0;
            public ByteValue intervalMinutes = 0;
            public ByteValue pushNodeId = 0;
            public static implicit operator METER_TBL_PUSH_CONFIGURATION_REPORT(byte[] data)
            {
                METER_TBL_PUSH_CONFIGURATION_REPORT ret = new METER_TBL_PUSH_CONFIGURATION_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.pushDataset = (data.Length - index) >= pushDatasetBytesCount ? new byte[pushDatasetBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.pushDataset[0] = data[index++];
                    if (data.Length > index) ret.pushDataset[1] = data[index++];
                    if (data.Length > index) ret.pushDataset[2] = data[index++];
                    ret.intervalMonths = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.intervalDays = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.intervalHours = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.intervalMinutes = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.pushNodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](METER_TBL_PUSH_CONFIGURATION_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_METER_TBL_PUSH.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.pushDataset != null)
                {
                    foreach (var tmp in command.pushDataset)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.intervalMonths.HasValue) ret.Add(command.intervalMonths);
                if (command.intervalDays.HasValue) ret.Add(command.intervalDays);
                if (command.intervalHours.HasValue) ret.Add(command.intervalHours);
                if (command.intervalMinutes.HasValue) ret.Add(command.intervalMinutes);
                if (command.pushNodeId.HasValue) ret.Add(command.pushNodeId);
                return ret.ToArray();
            }
        }
        public partial class METER_TBL_PUSH_CONFIGURATION_SET
        {
            public const byte ID = 0x01;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte operatingStatusPushMode
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte ps
                {
                    get { return (byte)(_value >> 4 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x10; _value += (byte)(value << 4 & 0x10); }
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
            public const byte pushDatasetBytesCount = 3;
            public byte[] pushDataset = new byte[pushDatasetBytesCount];
            public ByteValue intervalMonths = 0;
            public ByteValue intervalDays = 0;
            public ByteValue intervalHours = 0;
            public ByteValue intervalMinutes = 0;
            public ByteValue pushNodeId = 0;
            public static implicit operator METER_TBL_PUSH_CONFIGURATION_SET(byte[] data)
            {
                METER_TBL_PUSH_CONFIGURATION_SET ret = new METER_TBL_PUSH_CONFIGURATION_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.pushDataset = (data.Length - index) >= pushDatasetBytesCount ? new byte[pushDatasetBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.pushDataset[0] = data[index++];
                    if (data.Length > index) ret.pushDataset[1] = data[index++];
                    if (data.Length > index) ret.pushDataset[2] = data[index++];
                    ret.intervalMonths = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.intervalDays = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.intervalHours = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.intervalMinutes = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.pushNodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](METER_TBL_PUSH_CONFIGURATION_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_METER_TBL_PUSH.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.pushDataset != null)
                {
                    foreach (var tmp in command.pushDataset)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.intervalMonths.HasValue) ret.Add(command.intervalMonths);
                if (command.intervalDays.HasValue) ret.Add(command.intervalDays);
                if (command.intervalHours.HasValue) ret.Add(command.intervalHours);
                if (command.intervalMinutes.HasValue) ret.Add(command.intervalMinutes);
                if (command.pushNodeId.HasValue) ret.Add(command.pushNodeId);
                return ret.ToArray();
            }
        }
    }
}

