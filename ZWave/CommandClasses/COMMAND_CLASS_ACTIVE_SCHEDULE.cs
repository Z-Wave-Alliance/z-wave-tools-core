/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_ACTIVE_SCHEDULE
    {
        public const byte ID = 0xA4;
        public const byte VERSION = 1;
        public partial class ACTIVE_SCHEDULE_CAPABILITIES_GET
        {
            public const byte ID = 0x01;
            public static implicit operator ACTIVE_SCHEDULE_CAPABILITIES_GET(byte[] data)
            {
                ACTIVE_SCHEDULE_CAPABILITIES_GET ret = new ACTIVE_SCHEDULE_CAPABILITIES_GET();
                return ret;
            }
            public static implicit operator byte[](ACTIVE_SCHEDULE_CAPABILITIES_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ACTIVE_SCHEDULE.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class ACTIVE_SCHEDULE_CAPABILITIES_REPORT
        {
            public const byte ID = 0x02;
            public ByteValue numberOfSupportedTargetCcs = 0;
            public IList<byte> supportedTargetCc = new List<byte>();
            public class TVG1NUMBEROFSUPPORTEDTARGETS
            {
                public const byte numberOfSupportedTargetsBytesCount = 2;
                public byte[] numberOfSupportedTargets = new byte[numberOfSupportedTargetsBytesCount];
            }
            public List<TVG1NUMBEROFSUPPORTEDTARGETS> vg1NumberOfSupportedTargets = new List<TVG1NUMBEROFSUPPORTEDTARGETS>();
            public class TVG2NUMBEROFSUPPORTEDYEARDAYSCHEDULESPERTARGET
            {
                public const byte numberOfSupportedYearDaySchedulesPerTargetBytesCount = 2;
                public byte[] numberOfSupportedYearDaySchedulesPerTarget = new byte[numberOfSupportedYearDaySchedulesPerTargetBytesCount];
            }
            public List<TVG2NUMBEROFSUPPORTEDYEARDAYSCHEDULESPERTARGET> vg2NumberOfSupportedYearDaySchedulesPerTarget = new List<TVG2NUMBEROFSUPPORTEDYEARDAYSCHEDULESPERTARGET>();
            public class TVG3NUMBEROFSUPPORTEDDAILYREPEATINGSCHEDULESPERTARGET
            {
                public const byte numberOfSupportedDailyRepeatingSchedulesPerTargetBytesCount = 2;
                public byte[] numberOfSupportedDailyRepeatingSchedulesPerTarget = new byte[numberOfSupportedDailyRepeatingSchedulesPerTargetBytesCount];
            }
            public List<TVG3NUMBEROFSUPPORTEDDAILYREPEATINGSCHEDULESPERTARGET> vg3NumberOfSupportedDailyRepeatingSchedulesPerTarget = new List<TVG3NUMBEROFSUPPORTEDDAILYREPEATINGSCHEDULESPERTARGET>();
            public static implicit operator ACTIVE_SCHEDULE_CAPABILITIES_REPORT(byte[] data)
            {
                ACTIVE_SCHEDULE_CAPABILITIES_REPORT ret = new ACTIVE_SCHEDULE_CAPABILITIES_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.numberOfSupportedTargetCcs = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.supportedTargetCc = new List<byte>();
                    for (int i = 0; i < ret.numberOfSupportedTargetCcs; i++)
                    {
                        if (data.Length > index) ret.supportedTargetCc.Add(data[index++]);
                    }
                    ret.vg1NumberOfSupportedTargets = new List<TVG1NUMBEROFSUPPORTEDTARGETS>();
                    for (int j = 0; j < ret.numberOfSupportedTargetCcs; j++)
                    {
                        TVG1NUMBEROFSUPPORTEDTARGETS tmp = new TVG1NUMBEROFSUPPORTEDTARGETS();
                        tmp.numberOfSupportedTargets = (data.Length - index) >= TVG1NUMBEROFSUPPORTEDTARGETS.numberOfSupportedTargetsBytesCount ? new byte[TVG1NUMBEROFSUPPORTEDTARGETS.numberOfSupportedTargetsBytesCount] : new byte[data.Length - index];
                        if (data.Length > index) tmp.numberOfSupportedTargets[0] = data[index++];
                        if (data.Length > index) tmp.numberOfSupportedTargets[1] = data[index++];
                        ret.vg1NumberOfSupportedTargets.Add(tmp);
                    }
                    ret.vg2NumberOfSupportedYearDaySchedulesPerTarget = new List<TVG2NUMBEROFSUPPORTEDYEARDAYSCHEDULESPERTARGET>();
                    for (int j = 0; j < ret.numberOfSupportedTargetCcs; j++)
                    {
                        TVG2NUMBEROFSUPPORTEDYEARDAYSCHEDULESPERTARGET tmp = new TVG2NUMBEROFSUPPORTEDYEARDAYSCHEDULESPERTARGET();
                        tmp.numberOfSupportedYearDaySchedulesPerTarget = (data.Length - index) >= TVG2NUMBEROFSUPPORTEDYEARDAYSCHEDULESPERTARGET.numberOfSupportedYearDaySchedulesPerTargetBytesCount ? new byte[TVG2NUMBEROFSUPPORTEDYEARDAYSCHEDULESPERTARGET.numberOfSupportedYearDaySchedulesPerTargetBytesCount] : new byte[data.Length - index];
                        if (data.Length > index) tmp.numberOfSupportedYearDaySchedulesPerTarget[0] = data[index++];
                        if (data.Length > index) tmp.numberOfSupportedYearDaySchedulesPerTarget[1] = data[index++];
                        ret.vg2NumberOfSupportedYearDaySchedulesPerTarget.Add(tmp);
                    }
                    ret.vg3NumberOfSupportedDailyRepeatingSchedulesPerTarget = new List<TVG3NUMBEROFSUPPORTEDDAILYREPEATINGSCHEDULESPERTARGET>();
                    for (int j = 0; j < ret.numberOfSupportedTargetCcs; j++)
                    {
                        TVG3NUMBEROFSUPPORTEDDAILYREPEATINGSCHEDULESPERTARGET tmp = new TVG3NUMBEROFSUPPORTEDDAILYREPEATINGSCHEDULESPERTARGET();
                        tmp.numberOfSupportedDailyRepeatingSchedulesPerTarget = (data.Length - index) >= TVG3NUMBEROFSUPPORTEDDAILYREPEATINGSCHEDULESPERTARGET.numberOfSupportedDailyRepeatingSchedulesPerTargetBytesCount ? new byte[TVG3NUMBEROFSUPPORTEDDAILYREPEATINGSCHEDULESPERTARGET.numberOfSupportedDailyRepeatingSchedulesPerTargetBytesCount] : new byte[data.Length - index];
                        if (data.Length > index) tmp.numberOfSupportedDailyRepeatingSchedulesPerTarget[0] = data[index++];
                        if (data.Length > index) tmp.numberOfSupportedDailyRepeatingSchedulesPerTarget[1] = data[index++];
                        ret.vg3NumberOfSupportedDailyRepeatingSchedulesPerTarget.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](ACTIVE_SCHEDULE_CAPABILITIES_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ACTIVE_SCHEDULE.ID);
                ret.Add(ID);
                if (command.numberOfSupportedTargetCcs.HasValue) ret.Add(command.numberOfSupportedTargetCcs);
                if (command.supportedTargetCc != null)
                {
                    foreach (var tmp in command.supportedTargetCc)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.vg1NumberOfSupportedTargets != null)
                {
                    foreach (var item in command.vg1NumberOfSupportedTargets)
                    {
                        if (item.numberOfSupportedTargets != null)
                        {
                            foreach (var tmp in item.numberOfSupportedTargets)
                            {
                                ret.Add(tmp);
                            }
                        }
                    }
                }
                if (command.vg2NumberOfSupportedYearDaySchedulesPerTarget != null)
                {
                    foreach (var item in command.vg2NumberOfSupportedYearDaySchedulesPerTarget)
                    {
                        if (item.numberOfSupportedYearDaySchedulesPerTarget != null)
                        {
                            foreach (var tmp in item.numberOfSupportedYearDaySchedulesPerTarget)
                            {
                                ret.Add(tmp);
                            }
                        }
                    }
                }
                if (command.vg3NumberOfSupportedDailyRepeatingSchedulesPerTarget != null)
                {
                    foreach (var item in command.vg3NumberOfSupportedDailyRepeatingSchedulesPerTarget)
                    {
                        if (item.numberOfSupportedDailyRepeatingSchedulesPerTarget != null)
                        {
                            foreach (var tmp in item.numberOfSupportedDailyRepeatingSchedulesPerTarget)
                            {
                                ret.Add(tmp);
                            }
                        }
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class ACTIVE_SCHEDULE_ENABLE_SET
        {
            public const byte ID = 0x03;
            public ByteValue targetCc = 0;
            public const byte targetIdBytesCount = 2;
            public byte[] targetId = new byte[targetIdBytesCount];
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte enabled
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 1 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0xFE; _value += (byte)(value << 1 & 0xFE); }
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
            public static implicit operator ACTIVE_SCHEDULE_ENABLE_SET(byte[] data)
            {
                ACTIVE_SCHEDULE_ENABLE_SET ret = new ACTIVE_SCHEDULE_ENABLE_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.targetCc = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.targetId = (data.Length - index) >= targetIdBytesCount ? new byte[targetIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.targetId[0] = data[index++];
                    if (data.Length > index) ret.targetId[1] = data[index++];
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](ACTIVE_SCHEDULE_ENABLE_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ACTIVE_SCHEDULE.ID);
                ret.Add(ID);
                if (command.targetCc.HasValue) ret.Add(command.targetCc);
                if (command.targetId != null)
                {
                    foreach (var tmp in command.targetId)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties1.HasValue) ret.Add(command.properties1);
                return ret.ToArray();
            }
        }
        public partial class ACTIVE_SCHEDULE_ENABLE_GET
        {
            public const byte ID = 0x04;
            public ByteValue targetCc = 0;
            public const byte targetIdBytesCount = 2;
            public byte[] targetId = new byte[targetIdBytesCount];
            public static implicit operator ACTIVE_SCHEDULE_ENABLE_GET(byte[] data)
            {
                ACTIVE_SCHEDULE_ENABLE_GET ret = new ACTIVE_SCHEDULE_ENABLE_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.targetCc = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.targetId = (data.Length - index) >= targetIdBytesCount ? new byte[targetIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.targetId[0] = data[index++];
                    if (data.Length > index) ret.targetId[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](ACTIVE_SCHEDULE_ENABLE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ACTIVE_SCHEDULE.ID);
                ret.Add(ID);
                if (command.targetCc.HasValue) ret.Add(command.targetCc);
                if (command.targetId != null)
                {
                    foreach (var tmp in command.targetId)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class ACTIVE_SCHEDULE_ENABLE_REPORT
        {
            public const byte ID = 0x05;
            public ByteValue targetCc = 0;
            public const byte targetIdBytesCount = 2;
            public byte[] targetId = new byte[targetIdBytesCount];
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte enabled
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte reportCode
                {
                    get { return (byte)(_value >> 1 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x0E; _value += (byte)(value << 1 & 0x0E); }
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
            public static implicit operator ACTIVE_SCHEDULE_ENABLE_REPORT(byte[] data)
            {
                ACTIVE_SCHEDULE_ENABLE_REPORT ret = new ACTIVE_SCHEDULE_ENABLE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.targetCc = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.targetId = (data.Length - index) >= targetIdBytesCount ? new byte[targetIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.targetId[0] = data[index++];
                    if (data.Length > index) ret.targetId[1] = data[index++];
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](ACTIVE_SCHEDULE_ENABLE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ACTIVE_SCHEDULE.ID);
                ret.Add(ID);
                if (command.targetCc.HasValue) ret.Add(command.targetCc);
                if (command.targetId != null)
                {
                    foreach (var tmp in command.targetId)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties1.HasValue) ret.Add(command.properties1);
                return ret.ToArray();
            }
        }
        public partial class ACTIVE_SCHEDULE_YEAR_DAY_SCHEDULE_SET
        {
            public const byte ID = 0x06;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte setAction
                {
                    get { return (byte)(_value >> 0 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x03; _value += (byte)(value << 0 & 0x03); }
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
            public ByteValue targetCc = 0;
            public const byte targetIdBytesCount = 2;
            public byte[] targetId = new byte[targetIdBytesCount];
            public const byte scheduleSlotIdBytesCount = 2;
            public byte[] scheduleSlotId = new byte[scheduleSlotIdBytesCount];
            public const byte startYearBytesCount = 2;
            public byte[] startYear = new byte[startYearBytesCount];
            public ByteValue startMonth = 0;
            public ByteValue startDay = 0;
            public ByteValue startHour = 0;
            public ByteValue startMinute = 0;
            public const byte stopYearBytesCount = 2;
            public byte[] stopYear = new byte[stopYearBytesCount];
            public ByteValue stopMonth = 0;
            public ByteValue stopDay = 0;
            public ByteValue stopHour = 0;
            public ByteValue stopMinute = 0;
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte metadataLength
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 3 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0xF8; _value += (byte)(value << 3 & 0xF8); }
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
            public IList<byte> metadata = new List<byte>();
            public static implicit operator ACTIVE_SCHEDULE_YEAR_DAY_SCHEDULE_SET(byte[] data)
            {
                ACTIVE_SCHEDULE_YEAR_DAY_SCHEDULE_SET ret = new ACTIVE_SCHEDULE_YEAR_DAY_SCHEDULE_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.targetCc = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.targetId = (data.Length - index) >= targetIdBytesCount ? new byte[targetIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.targetId[0] = data[index++];
                    if (data.Length > index) ret.targetId[1] = data[index++];
                    ret.scheduleSlotId = (data.Length - index) >= scheduleSlotIdBytesCount ? new byte[scheduleSlotIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.scheduleSlotId[0] = data[index++];
                    if (data.Length > index) ret.scheduleSlotId[1] = data[index++];
                    ret.startYear = (data.Length - index) >= startYearBytesCount ? new byte[startYearBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.startYear[0] = data[index++];
                    if (data.Length > index) ret.startYear[1] = data[index++];
                    ret.startMonth = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startDay = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startHour = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startMinute = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopYear = (data.Length - index) >= stopYearBytesCount ? new byte[stopYearBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.stopYear[0] = data[index++];
                    if (data.Length > index) ret.stopYear[1] = data[index++];
                    ret.stopMonth = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopDay = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopHour = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopMinute = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.metadata = new List<byte>();
                    for (int i = 0; i < ret.properties2.metadataLength; i++)
                    {
                        if (data.Length > index) ret.metadata.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](ACTIVE_SCHEDULE_YEAR_DAY_SCHEDULE_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ACTIVE_SCHEDULE.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.targetCc.HasValue) ret.Add(command.targetCc);
                if (command.targetId != null)
                {
                    foreach (var tmp in command.targetId)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.scheduleSlotId != null)
                {
                    foreach (var tmp in command.scheduleSlotId)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.startYear != null)
                {
                    foreach (var tmp in command.startYear)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.startMonth.HasValue) ret.Add(command.startMonth);
                if (command.startDay.HasValue) ret.Add(command.startDay);
                if (command.startHour.HasValue) ret.Add(command.startHour);
                if (command.startMinute.HasValue) ret.Add(command.startMinute);
                if (command.stopYear != null)
                {
                    foreach (var tmp in command.stopYear)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.stopMonth.HasValue) ret.Add(command.stopMonth);
                if (command.stopDay.HasValue) ret.Add(command.stopDay);
                if (command.stopHour.HasValue) ret.Add(command.stopHour);
                if (command.stopMinute.HasValue) ret.Add(command.stopMinute);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.metadata != null)
                {
                    foreach (var tmp in command.metadata)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class ACTIVE_SCHEDULE_YEAR_DAY_SCHEDULE_GET
        {
            public const byte ID = 0x07;
            public ByteValue targetCc = 0;
            public const byte targetIdBytesCount = 2;
            public byte[] targetId = new byte[targetIdBytesCount];
            public const byte scheduleSlotIdBytesCount = 2;
            public byte[] scheduleSlotId = new byte[scheduleSlotIdBytesCount];
            public static implicit operator ACTIVE_SCHEDULE_YEAR_DAY_SCHEDULE_GET(byte[] data)
            {
                ACTIVE_SCHEDULE_YEAR_DAY_SCHEDULE_GET ret = new ACTIVE_SCHEDULE_YEAR_DAY_SCHEDULE_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.targetCc = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.targetId = (data.Length - index) >= targetIdBytesCount ? new byte[targetIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.targetId[0] = data[index++];
                    if (data.Length > index) ret.targetId[1] = data[index++];
                    ret.scheduleSlotId = (data.Length - index) >= scheduleSlotIdBytesCount ? new byte[scheduleSlotIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.scheduleSlotId[0] = data[index++];
                    if (data.Length > index) ret.scheduleSlotId[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](ACTIVE_SCHEDULE_YEAR_DAY_SCHEDULE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ACTIVE_SCHEDULE.ID);
                ret.Add(ID);
                if (command.targetCc.HasValue) ret.Add(command.targetCc);
                if (command.targetId != null)
                {
                    foreach (var tmp in command.targetId)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.scheduleSlotId != null)
                {
                    foreach (var tmp in command.scheduleSlotId)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class ACTIVE_SCHEDULE_YEAR_DAY_SCHEDULE_REPORT
        {
            public const byte ID = 0x08;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte reportCode
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
            public ByteValue targetCc = 0;
            public const byte targetIdBytesCount = 2;
            public byte[] targetId = new byte[targetIdBytesCount];
            public const byte scheduleSlotIdBytesCount = 2;
            public byte[] scheduleSlotId = new byte[scheduleSlotIdBytesCount];
            public const byte nextScheduleSlotIdBytesCount = 2;
            public byte[] nextScheduleSlotId = new byte[nextScheduleSlotIdBytesCount];
            public const byte startYearBytesCount = 2;
            public byte[] startYear = new byte[startYearBytesCount];
            public ByteValue startMonth = 0;
            public ByteValue startDay = 0;
            public ByteValue startHour = 0;
            public ByteValue startMinute = 0;
            public const byte stopYearBytesCount = 2;
            public byte[] stopYear = new byte[stopYearBytesCount];
            public ByteValue stopMonth = 0;
            public ByteValue stopDay = 0;
            public ByteValue stopHour = 0;
            public ByteValue stopMinute = 0;
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte metadataLength
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 3 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0xF8; _value += (byte)(value << 3 & 0xF8); }
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
            public IList<byte> metadata = new List<byte>();
            public static implicit operator ACTIVE_SCHEDULE_YEAR_DAY_SCHEDULE_REPORT(byte[] data)
            {
                ACTIVE_SCHEDULE_YEAR_DAY_SCHEDULE_REPORT ret = new ACTIVE_SCHEDULE_YEAR_DAY_SCHEDULE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.targetCc = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.targetId = (data.Length - index) >= targetIdBytesCount ? new byte[targetIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.targetId[0] = data[index++];
                    if (data.Length > index) ret.targetId[1] = data[index++];
                    ret.scheduleSlotId = (data.Length - index) >= scheduleSlotIdBytesCount ? new byte[scheduleSlotIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.scheduleSlotId[0] = data[index++];
                    if (data.Length > index) ret.scheduleSlotId[1] = data[index++];
                    ret.nextScheduleSlotId = (data.Length - index) >= nextScheduleSlotIdBytesCount ? new byte[nextScheduleSlotIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.nextScheduleSlotId[0] = data[index++];
                    if (data.Length > index) ret.nextScheduleSlotId[1] = data[index++];
                    ret.startYear = (data.Length - index) >= startYearBytesCount ? new byte[startYearBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.startYear[0] = data[index++];
                    if (data.Length > index) ret.startYear[1] = data[index++];
                    ret.startMonth = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startDay = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startHour = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startMinute = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopYear = (data.Length - index) >= stopYearBytesCount ? new byte[stopYearBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.stopYear[0] = data[index++];
                    if (data.Length > index) ret.stopYear[1] = data[index++];
                    ret.stopMonth = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopDay = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopHour = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopMinute = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.metadata = new List<byte>();
                    for (int i = 0; i < ret.properties2.metadataLength; i++)
                    {
                        if (data.Length > index) ret.metadata.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](ACTIVE_SCHEDULE_YEAR_DAY_SCHEDULE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ACTIVE_SCHEDULE.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.targetCc.HasValue) ret.Add(command.targetCc);
                if (command.targetId != null)
                {
                    foreach (var tmp in command.targetId)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.scheduleSlotId != null)
                {
                    foreach (var tmp in command.scheduleSlotId)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.nextScheduleSlotId != null)
                {
                    foreach (var tmp in command.nextScheduleSlotId)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.startYear != null)
                {
                    foreach (var tmp in command.startYear)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.startMonth.HasValue) ret.Add(command.startMonth);
                if (command.startDay.HasValue) ret.Add(command.startDay);
                if (command.startHour.HasValue) ret.Add(command.startHour);
                if (command.startMinute.HasValue) ret.Add(command.startMinute);
                if (command.stopYear != null)
                {
                    foreach (var tmp in command.stopYear)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.stopMonth.HasValue) ret.Add(command.stopMonth);
                if (command.stopDay.HasValue) ret.Add(command.stopDay);
                if (command.stopHour.HasValue) ret.Add(command.stopHour);
                if (command.stopMinute.HasValue) ret.Add(command.stopMinute);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.metadata != null)
                {
                    foreach (var tmp in command.metadata)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class ACTIVE_SCHEDULE_DAILY_REPEATING_SCHEDULE_SET
        {
            public const byte ID = 0x09;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte setAction
                {
                    get { return (byte)(_value >> 0 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x03; _value += (byte)(value << 0 & 0x03); }
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
            public ByteValue targetCc = 0;
            public const byte targetIdBytesCount = 2;
            public byte[] targetId = new byte[targetIdBytesCount];
            public const byte scheduleSlotIdBytesCount = 2;
            public byte[] scheduleSlotId = new byte[scheduleSlotIdBytesCount];
            public ByteValue weekDayBitmask = 0;
            public ByteValue startHour = 0;
            public ByteValue startMinute = 0;
            public ByteValue durationHour = 0;
            public ByteValue durationMinute = 0;
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte metadataLength
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 3 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0xF8; _value += (byte)(value << 3 & 0xF8); }
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
            public IList<byte> metadata = new List<byte>();
            public static implicit operator ACTIVE_SCHEDULE_DAILY_REPEATING_SCHEDULE_SET(byte[] data)
            {
                ACTIVE_SCHEDULE_DAILY_REPEATING_SCHEDULE_SET ret = new ACTIVE_SCHEDULE_DAILY_REPEATING_SCHEDULE_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.targetCc = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.targetId = (data.Length - index) >= targetIdBytesCount ? new byte[targetIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.targetId[0] = data[index++];
                    if (data.Length > index) ret.targetId[1] = data[index++];
                    ret.scheduleSlotId = (data.Length - index) >= scheduleSlotIdBytesCount ? new byte[scheduleSlotIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.scheduleSlotId[0] = data[index++];
                    if (data.Length > index) ret.scheduleSlotId[1] = data[index++];
                    ret.weekDayBitmask = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startHour = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startMinute = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.durationHour = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.durationMinute = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.metadata = new List<byte>();
                    for (int i = 0; i < ret.properties2.metadataLength; i++)
                    {
                        if (data.Length > index) ret.metadata.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](ACTIVE_SCHEDULE_DAILY_REPEATING_SCHEDULE_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ACTIVE_SCHEDULE.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.targetCc.HasValue) ret.Add(command.targetCc);
                if (command.targetId != null)
                {
                    foreach (var tmp in command.targetId)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.scheduleSlotId != null)
                {
                    foreach (var tmp in command.scheduleSlotId)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.weekDayBitmask.HasValue) ret.Add(command.weekDayBitmask);
                if (command.startHour.HasValue) ret.Add(command.startHour);
                if (command.startMinute.HasValue) ret.Add(command.startMinute);
                if (command.durationHour.HasValue) ret.Add(command.durationHour);
                if (command.durationMinute.HasValue) ret.Add(command.durationMinute);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.metadata != null)
                {
                    foreach (var tmp in command.metadata)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class ACTIVE_SCHEDULE_DAILY_REPEATING_SCHEDULE_GET
        {
            public const byte ID = 0x0A;
            public ByteValue targetCc = 0;
            public const byte targetIdBytesCount = 2;
            public byte[] targetId = new byte[targetIdBytesCount];
            public const byte scheduleSlotIdBytesCount = 2;
            public byte[] scheduleSlotId = new byte[scheduleSlotIdBytesCount];
            public static implicit operator ACTIVE_SCHEDULE_DAILY_REPEATING_SCHEDULE_GET(byte[] data)
            {
                ACTIVE_SCHEDULE_DAILY_REPEATING_SCHEDULE_GET ret = new ACTIVE_SCHEDULE_DAILY_REPEATING_SCHEDULE_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.targetCc = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.targetId = (data.Length - index) >= targetIdBytesCount ? new byte[targetIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.targetId[0] = data[index++];
                    if (data.Length > index) ret.targetId[1] = data[index++];
                    ret.scheduleSlotId = (data.Length - index) >= scheduleSlotIdBytesCount ? new byte[scheduleSlotIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.scheduleSlotId[0] = data[index++];
                    if (data.Length > index) ret.scheduleSlotId[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](ACTIVE_SCHEDULE_DAILY_REPEATING_SCHEDULE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ACTIVE_SCHEDULE.ID);
                ret.Add(ID);
                if (command.targetCc.HasValue) ret.Add(command.targetCc);
                if (command.targetId != null)
                {
                    foreach (var tmp in command.targetId)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.scheduleSlotId != null)
                {
                    foreach (var tmp in command.scheduleSlotId)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class ACTIVE_SCHEDULE_DAILY_REPEATING_SCHEDULE_REPORT
        {
            public const byte ID = 0x0B;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte reportCode
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
            public ByteValue targetCc = 0;
            public const byte targetIdBytesCount = 2;
            public byte[] targetId = new byte[targetIdBytesCount];
            public const byte scheduleSlotIdBytesCount = 2;
            public byte[] scheduleSlotId = new byte[scheduleSlotIdBytesCount];
            public const byte nextScheduleSlotIdBytesCount = 2;
            public byte[] nextScheduleSlotId = new byte[nextScheduleSlotIdBytesCount];
            public ByteValue weekDayBitmask = 0;
            public ByteValue startHour = 0;
            public ByteValue startMinute = 0;
            public ByteValue durationHour = 0;
            public ByteValue durationMinute = 0;
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte metadataLength
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 3 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0xF8; _value += (byte)(value << 3 & 0xF8); }
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
            public IList<byte> metadata = new List<byte>();
            public static implicit operator ACTIVE_SCHEDULE_DAILY_REPEATING_SCHEDULE_REPORT(byte[] data)
            {
                ACTIVE_SCHEDULE_DAILY_REPEATING_SCHEDULE_REPORT ret = new ACTIVE_SCHEDULE_DAILY_REPEATING_SCHEDULE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.targetCc = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.targetId = (data.Length - index) >= targetIdBytesCount ? new byte[targetIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.targetId[0] = data[index++];
                    if (data.Length > index) ret.targetId[1] = data[index++];
                    ret.scheduleSlotId = (data.Length - index) >= scheduleSlotIdBytesCount ? new byte[scheduleSlotIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.scheduleSlotId[0] = data[index++];
                    if (data.Length > index) ret.scheduleSlotId[1] = data[index++];
                    ret.nextScheduleSlotId = (data.Length - index) >= nextScheduleSlotIdBytesCount ? new byte[nextScheduleSlotIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.nextScheduleSlotId[0] = data[index++];
                    if (data.Length > index) ret.nextScheduleSlotId[1] = data[index++];
                    ret.weekDayBitmask = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startHour = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startMinute = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.durationHour = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.durationMinute = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.metadata = new List<byte>();
                    for (int i = 0; i < ret.properties2.metadataLength; i++)
                    {
                        if (data.Length > index) ret.metadata.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](ACTIVE_SCHEDULE_DAILY_REPEATING_SCHEDULE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ACTIVE_SCHEDULE.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.targetCc.HasValue) ret.Add(command.targetCc);
                if (command.targetId != null)
                {
                    foreach (var tmp in command.targetId)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.scheduleSlotId != null)
                {
                    foreach (var tmp in command.scheduleSlotId)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.nextScheduleSlotId != null)
                {
                    foreach (var tmp in command.nextScheduleSlotId)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.weekDayBitmask.HasValue) ret.Add(command.weekDayBitmask);
                if (command.startHour.HasValue) ret.Add(command.startHour);
                if (command.startMinute.HasValue) ret.Add(command.startMinute);
                if (command.durationHour.HasValue) ret.Add(command.durationHour);
                if (command.durationMinute.HasValue) ret.Add(command.durationMinute);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.metadata != null)
                {
                    foreach (var tmp in command.metadata)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

