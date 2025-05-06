using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_GENERIC_SCHEDULE
    {
        public const byte ID = 0xA3;
        public const byte VERSION = 1;
        public partial class GENERIC_SCHEDULE_CAPABILITIES_GET
        {
            public const byte ID = 0x01;
            public static implicit operator GENERIC_SCHEDULE_CAPABILITIES_GET(byte[] data)
            {
                GENERIC_SCHEDULE_CAPABILITIES_GET ret = new GENERIC_SCHEDULE_CAPABILITIES_GET();
                return ret;
            }
            public static implicit operator byte[](GENERIC_SCHEDULE_CAPABILITIES_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_GENERIC_SCHEDULE.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class GENERIC_SCHEDULE_CAPABILITIES_REPORT
        {
            public const byte ID = 0x02;
            public const byte numberOfSupportedScheduleIdsBytesCount = 2;
            public byte[] numberOfSupportedScheduleIds = new byte[numberOfSupportedScheduleIdsBytesCount];
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte numberOfSupportedTimeRangeIds1
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte reserved1
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
            public ByteValue numberOfSupportedTimeRangeIds2 = 0;
            public ByteValue numberOfSupportedTimeRangesPerSchedule = 0;
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte hourMinute
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte date
                {
                    get { return (byte)(_value >> 1 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x02; _value += (byte)(value << 1 & 0x02); }
                }
                public byte weekdays
                {
                    get { return (byte)(_value >> 2 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x04; _value += (byte)(value << 2 & 0x04); }
                }
                public byte reserved2
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
            public static implicit operator GENERIC_SCHEDULE_CAPABILITIES_REPORT(byte[] data)
            {
                GENERIC_SCHEDULE_CAPABILITIES_REPORT ret = new GENERIC_SCHEDULE_CAPABILITIES_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.numberOfSupportedScheduleIds = (data.Length - index) >= numberOfSupportedScheduleIdsBytesCount ? new byte[numberOfSupportedScheduleIdsBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.numberOfSupportedScheduleIds[0] = data[index++];
                    if (data.Length > index) ret.numberOfSupportedScheduleIds[1] = data[index++];
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.numberOfSupportedTimeRangeIds2 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.numberOfSupportedTimeRangesPerSchedule = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](GENERIC_SCHEDULE_CAPABILITIES_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_GENERIC_SCHEDULE.ID);
                ret.Add(ID);
                if (command.numberOfSupportedScheduleIds != null)
                {
                    foreach (var tmp in command.numberOfSupportedScheduleIds)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.numberOfSupportedTimeRangeIds2.HasValue) ret.Add(command.numberOfSupportedTimeRangeIds2);
                if (command.numberOfSupportedTimeRangesPerSchedule.HasValue) ret.Add(command.numberOfSupportedTimeRangesPerSchedule);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                return ret.ToArray();
            }
        }
        public partial class GENERIC_SCHEDULE_TIME_RANGE_SET
        {
            public const byte ID = 0x03;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte timeRangeId1
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte reserved1
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
            public ByteValue timeRangeId2 = 0;
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte weekdayBitmask
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte inUse1
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
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
            public struct Tproperties3
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties3 Empty { get { return new Tproperties3() { _value = 0, HasValue = false }; } }
                public byte startYear1
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte inUse2
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties3(byte data)
                {
                    Tproperties3 ret = new Tproperties3();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties3 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties3 properties3 = 0;
            public ByteValue startYear2 = 0;
            public struct Tproperties4
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties4 Empty { get { return new Tproperties4() { _value = 0, HasValue = false }; } }
                public byte stopYear1
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte inUse3
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties4(byte data)
                {
                    Tproperties4 ret = new Tproperties4();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties4 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties4 properties4 = 0;
            public ByteValue stopYear2 = 0;
            public struct Tproperties5
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties5 Empty { get { return new Tproperties5() { _value = 0, HasValue = false }; } }
                public byte startMonth
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte reserved2
                {
                    get { return (byte)(_value >> 4 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x70; _value += (byte)(value << 4 & 0x70); }
                }
                public byte inUse4
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties5(byte data)
                {
                    Tproperties5 ret = new Tproperties5();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties5 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties5 properties5 = 0;
            public struct Tproperties6
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties6 Empty { get { return new Tproperties6() { _value = 0, HasValue = false }; } }
                public byte stopMonth
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte reserved3
                {
                    get { return (byte)(_value >> 4 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x70; _value += (byte)(value << 4 & 0x70); }
                }
                public byte inUse5
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties6(byte data)
                {
                    Tproperties6 ret = new Tproperties6();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties6 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties6 properties6 = 0;
            public struct Tproperties7
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties7 Empty { get { return new Tproperties7() { _value = 0, HasValue = false }; } }
                public byte startDay
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte reserved4
                {
                    get { return (byte)(_value >> 5 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x60; _value += (byte)(value << 5 & 0x60); }
                }
                public byte inUse6
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties7(byte data)
                {
                    Tproperties7 ret = new Tproperties7();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties7 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties7 properties7 = 0;
            public struct Tproperties8
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties8 Empty { get { return new Tproperties8() { _value = 0, HasValue = false }; } }
                public byte stopDay
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte reserved5
                {
                    get { return (byte)(_value >> 5 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x60; _value += (byte)(value << 5 & 0x60); }
                }
                public byte inUse7
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties8(byte data)
                {
                    Tproperties8 ret = new Tproperties8();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties8 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties8 properties8 = 0;
            public struct Tproperties9
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties9 Empty { get { return new Tproperties9() { _value = 0, HasValue = false }; } }
                public byte startHour
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte reserved6
                {
                    get { return (byte)(_value >> 5 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x60; _value += (byte)(value << 5 & 0x60); }
                }
                public byte inUse8
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties9(byte data)
                {
                    Tproperties9 ret = new Tproperties9();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties9 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties9 properties9 = 0;
            public struct Tproperties10
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties10 Empty { get { return new Tproperties10() { _value = 0, HasValue = false }; } }
                public byte stopHour
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte reserved7
                {
                    get { return (byte)(_value >> 5 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x60; _value += (byte)(value << 5 & 0x60); }
                }
                public byte inUse9
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties10(byte data)
                {
                    Tproperties10 ret = new Tproperties10();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties10 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties10 properties10 = 0;
            public struct Tproperties11
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties11 Empty { get { return new Tproperties11() { _value = 0, HasValue = false }; } }
                public byte startMinute
                {
                    get { return (byte)(_value >> 0 & 0x3F); }
                    set { HasValue = true; _value &= 0xFF - 0x3F; _value += (byte)(value << 0 & 0x3F); }
                }
                public byte reserved8
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte inUse10
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties11(byte data)
                {
                    Tproperties11 ret = new Tproperties11();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties11 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties11 properties11 = 0;
            public struct Tproperties12
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties12 Empty { get { return new Tproperties12() { _value = 0, HasValue = false }; } }
                public byte stopMinute
                {
                    get { return (byte)(_value >> 0 & 0x3F); }
                    set { HasValue = true; _value &= 0xFF - 0x3F; _value += (byte)(value << 0 & 0x3F); }
                }
                public byte reserved9
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte inUse11
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties12(byte data)
                {
                    Tproperties12 ret = new Tproperties12();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties12 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties12 properties12 = 0;
            public struct Tproperties13
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties13 Empty { get { return new Tproperties13() { _value = 0, HasValue = false }; } }
                public byte dailyStartHour
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte reserved10
                {
                    get { return (byte)(_value >> 5 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x60; _value += (byte)(value << 5 & 0x60); }
                }
                public byte inUse12
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties13(byte data)
                {
                    Tproperties13 ret = new Tproperties13();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties13 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties13 properties13 = 0;
            public struct Tproperties14
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties14 Empty { get { return new Tproperties14() { _value = 0, HasValue = false }; } }
                public byte dailyStopHour
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte reserved11
                {
                    get { return (byte)(_value >> 5 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x60; _value += (byte)(value << 5 & 0x60); }
                }
                public byte inUse13
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties14(byte data)
                {
                    Tproperties14 ret = new Tproperties14();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties14 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties14 properties14 = 0;
            public struct Tproperties15
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties15 Empty { get { return new Tproperties15() { _value = 0, HasValue = false }; } }
                public byte dailyStartMinute
                {
                    get { return (byte)(_value >> 0 & 0x3F); }
                    set { HasValue = true; _value &= 0xFF - 0x3F; _value += (byte)(value << 0 & 0x3F); }
                }
                public byte reserved12
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte inUse14
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties15(byte data)
                {
                    Tproperties15 ret = new Tproperties15();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties15 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties15 properties15 = 0;
            public struct Tproperties16
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties16 Empty { get { return new Tproperties16() { _value = 0, HasValue = false }; } }
                public byte dailyStopMinute
                {
                    get { return (byte)(_value >> 0 & 0x3F); }
                    set { HasValue = true; _value &= 0xFF - 0x3F; _value += (byte)(value << 0 & 0x3F); }
                }
                public byte reserved13
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte inUse15
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties16(byte data)
                {
                    Tproperties16 ret = new Tproperties16();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties16 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties16 properties16 = 0;
            public static implicit operator GENERIC_SCHEDULE_TIME_RANGE_SET(byte[] data)
            {
                GENERIC_SCHEDULE_TIME_RANGE_SET ret = new GENERIC_SCHEDULE_TIME_RANGE_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.timeRangeId2 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.properties3 = data.Length > index ? (Tproperties3)data[index++] : Tproperties3.Empty;
                    ret.startYear2 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties4 = data.Length > index ? (Tproperties4)data[index++] : Tproperties4.Empty;
                    ret.stopYear2 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties5 = data.Length > index ? (Tproperties5)data[index++] : Tproperties5.Empty;
                    ret.properties6 = data.Length > index ? (Tproperties6)data[index++] : Tproperties6.Empty;
                    ret.properties7 = data.Length > index ? (Tproperties7)data[index++] : Tproperties7.Empty;
                    ret.properties8 = data.Length > index ? (Tproperties8)data[index++] : Tproperties8.Empty;
                    ret.properties9 = data.Length > index ? (Tproperties9)data[index++] : Tproperties9.Empty;
                    ret.properties10 = data.Length > index ? (Tproperties10)data[index++] : Tproperties10.Empty;
                    ret.properties11 = data.Length > index ? (Tproperties11)data[index++] : Tproperties11.Empty;
                    ret.properties12 = data.Length > index ? (Tproperties12)data[index++] : Tproperties12.Empty;
                    ret.properties13 = data.Length > index ? (Tproperties13)data[index++] : Tproperties13.Empty;
                    ret.properties14 = data.Length > index ? (Tproperties14)data[index++] : Tproperties14.Empty;
                    ret.properties15 = data.Length > index ? (Tproperties15)data[index++] : Tproperties15.Empty;
                    ret.properties16 = data.Length > index ? (Tproperties16)data[index++] : Tproperties16.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](GENERIC_SCHEDULE_TIME_RANGE_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_GENERIC_SCHEDULE.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.timeRangeId2.HasValue) ret.Add(command.timeRangeId2);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.properties3.HasValue) ret.Add(command.properties3);
                if (command.startYear2.HasValue) ret.Add(command.startYear2);
                if (command.properties4.HasValue) ret.Add(command.properties4);
                if (command.stopYear2.HasValue) ret.Add(command.stopYear2);
                if (command.properties5.HasValue) ret.Add(command.properties5);
                if (command.properties6.HasValue) ret.Add(command.properties6);
                if (command.properties7.HasValue) ret.Add(command.properties7);
                if (command.properties8.HasValue) ret.Add(command.properties8);
                if (command.properties9.HasValue) ret.Add(command.properties9);
                if (command.properties10.HasValue) ret.Add(command.properties10);
                if (command.properties11.HasValue) ret.Add(command.properties11);
                if (command.properties12.HasValue) ret.Add(command.properties12);
                if (command.properties13.HasValue) ret.Add(command.properties13);
                if (command.properties14.HasValue) ret.Add(command.properties14);
                if (command.properties15.HasValue) ret.Add(command.properties15);
                if (command.properties16.HasValue) ret.Add(command.properties16);
                return ret.ToArray();
            }
        }
        public partial class GENERIC_SCHEDULE_TIME_RANGE_GET
        {
            public const byte ID = 0x04;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte timeRangeId1
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte reserved1
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
            public ByteValue timeRangeId2 = 0;
            public static implicit operator GENERIC_SCHEDULE_TIME_RANGE_GET(byte[] data)
            {
                GENERIC_SCHEDULE_TIME_RANGE_GET ret = new GENERIC_SCHEDULE_TIME_RANGE_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.timeRangeId2 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](GENERIC_SCHEDULE_TIME_RANGE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_GENERIC_SCHEDULE.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.timeRangeId2.HasValue) ret.Add(command.timeRangeId2);
                return ret.ToArray();
            }
        }
        public partial class GENERIC_SCHEDULE_TIME_RANGE_REPORT
        {
            public const byte ID = 0x05;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte timeRangeId1
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte reserved1
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
            public ByteValue timeRangeId2 = 0;
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte weekdayBitmask
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte inUse1
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
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
            public struct Tproperties3
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties3 Empty { get { return new Tproperties3() { _value = 0, HasValue = false }; } }
                public byte startYear1
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte inUse2
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties3(byte data)
                {
                    Tproperties3 ret = new Tproperties3();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties3 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties3 properties3 = 0;
            public ByteValue startYear2 = 0;
            public struct Tproperties4
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties4 Empty { get { return new Tproperties4() { _value = 0, HasValue = false }; } }
                public byte stopYear1
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte inUse3
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties4(byte data)
                {
                    Tproperties4 ret = new Tproperties4();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties4 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties4 properties4 = 0;
            public ByteValue stopYear2 = 0;
            public struct Tproperties5
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties5 Empty { get { return new Tproperties5() { _value = 0, HasValue = false }; } }
                public byte startMonth
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte reserved2
                {
                    get { return (byte)(_value >> 4 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x70; _value += (byte)(value << 4 & 0x70); }
                }
                public byte inUse4
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties5(byte data)
                {
                    Tproperties5 ret = new Tproperties5();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties5 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties5 properties5 = 0;
            public struct Tproperties6
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties6 Empty { get { return new Tproperties6() { _value = 0, HasValue = false }; } }
                public byte stopMonth
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte reserved3
                {
                    get { return (byte)(_value >> 4 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x70; _value += (byte)(value << 4 & 0x70); }
                }
                public byte inUse5
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties6(byte data)
                {
                    Tproperties6 ret = new Tproperties6();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties6 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties6 properties6 = 0;
            public struct Tproperties7
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties7 Empty { get { return new Tproperties7() { _value = 0, HasValue = false }; } }
                public byte startDay
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte reserved4
                {
                    get { return (byte)(_value >> 5 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x60; _value += (byte)(value << 5 & 0x60); }
                }
                public byte inUse6
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties7(byte data)
                {
                    Tproperties7 ret = new Tproperties7();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties7 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties7 properties7 = 0;
            public struct Tproperties8
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties8 Empty { get { return new Tproperties8() { _value = 0, HasValue = false }; } }
                public byte stopDay
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte reserved5
                {
                    get { return (byte)(_value >> 5 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x60; _value += (byte)(value << 5 & 0x60); }
                }
                public byte inUse7
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties8(byte data)
                {
                    Tproperties8 ret = new Tproperties8();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties8 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties8 properties8 = 0;
            public struct Tproperties9
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties9 Empty { get { return new Tproperties9() { _value = 0, HasValue = false }; } }
                public byte startHour
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte reserved6
                {
                    get { return (byte)(_value >> 5 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x60; _value += (byte)(value << 5 & 0x60); }
                }
                public byte inUse8
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties9(byte data)
                {
                    Tproperties9 ret = new Tproperties9();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties9 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties9 properties9 = 0;
            public struct Tproperties10
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties10 Empty { get { return new Tproperties10() { _value = 0, HasValue = false }; } }
                public byte stopHour
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte reserved7
                {
                    get { return (byte)(_value >> 5 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x60; _value += (byte)(value << 5 & 0x60); }
                }
                public byte inUse9
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties10(byte data)
                {
                    Tproperties10 ret = new Tproperties10();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties10 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties10 properties10 = 0;
            public struct Tproperties11
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties11 Empty { get { return new Tproperties11() { _value = 0, HasValue = false }; } }
                public byte startMinute
                {
                    get { return (byte)(_value >> 0 & 0x3F); }
                    set { HasValue = true; _value &= 0xFF - 0x3F; _value += (byte)(value << 0 & 0x3F); }
                }
                public byte reserved8
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte inUse10
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties11(byte data)
                {
                    Tproperties11 ret = new Tproperties11();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties11 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties11 properties11 = 0;
            public struct Tproperties12
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties12 Empty { get { return new Tproperties12() { _value = 0, HasValue = false }; } }
                public byte stopMinute
                {
                    get { return (byte)(_value >> 0 & 0x3F); }
                    set { HasValue = true; _value &= 0xFF - 0x3F; _value += (byte)(value << 0 & 0x3F); }
                }
                public byte reserved9
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte inUse11
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties12(byte data)
                {
                    Tproperties12 ret = new Tproperties12();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties12 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties12 properties12 = 0;
            public struct Tproperties13
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties13 Empty { get { return new Tproperties13() { _value = 0, HasValue = false }; } }
                public byte dailyStartHour
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte reserved10
                {
                    get { return (byte)(_value >> 5 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x60; _value += (byte)(value << 5 & 0x60); }
                }
                public byte inUse12
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties13(byte data)
                {
                    Tproperties13 ret = new Tproperties13();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties13 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties13 properties13 = 0;
            public struct Tproperties14
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties14 Empty { get { return new Tproperties14() { _value = 0, HasValue = false }; } }
                public byte dailyStopHour
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte reserved11
                {
                    get { return (byte)(_value >> 5 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x60; _value += (byte)(value << 5 & 0x60); }
                }
                public byte inUse13
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties14(byte data)
                {
                    Tproperties14 ret = new Tproperties14();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties14 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties14 properties14 = 0;
            public struct Tproperties15
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties15 Empty { get { return new Tproperties15() { _value = 0, HasValue = false }; } }
                public byte dailyStartMinute
                {
                    get { return (byte)(_value >> 0 & 0x3F); }
                    set { HasValue = true; _value &= 0xFF - 0x3F; _value += (byte)(value << 0 & 0x3F); }
                }
                public byte reserved12
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte inUse14
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties15(byte data)
                {
                    Tproperties15 ret = new Tproperties15();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties15 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties15 properties15 = 0;
            public struct Tproperties16
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties16 Empty { get { return new Tproperties16() { _value = 0, HasValue = false }; } }
                public byte dailyStopMinute
                {
                    get { return (byte)(_value >> 0 & 0x3F); }
                    set { HasValue = true; _value &= 0xFF - 0x3F; _value += (byte)(value << 0 & 0x3F); }
                }
                public byte reserved13
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte inUse15
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties16(byte data)
                {
                    Tproperties16 ret = new Tproperties16();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties16 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties16 properties16 = 0;
            public struct Tproperties17
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties17 Empty { get { return new Tproperties17() { _value = 0, HasValue = false }; } }
                public byte nextTimeRangeId1
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte reserved14
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties17(byte data)
                {
                    Tproperties17 ret = new Tproperties17();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties17 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties17 properties17 = 0;
            public ByteValue nextTimeRangeId2 = 0;
            public static implicit operator GENERIC_SCHEDULE_TIME_RANGE_REPORT(byte[] data)
            {
                GENERIC_SCHEDULE_TIME_RANGE_REPORT ret = new GENERIC_SCHEDULE_TIME_RANGE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.timeRangeId2 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.properties3 = data.Length > index ? (Tproperties3)data[index++] : Tproperties3.Empty;
                    ret.startYear2 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties4 = data.Length > index ? (Tproperties4)data[index++] : Tproperties4.Empty;
                    ret.stopYear2 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties5 = data.Length > index ? (Tproperties5)data[index++] : Tproperties5.Empty;
                    ret.properties6 = data.Length > index ? (Tproperties6)data[index++] : Tproperties6.Empty;
                    ret.properties7 = data.Length > index ? (Tproperties7)data[index++] : Tproperties7.Empty;
                    ret.properties8 = data.Length > index ? (Tproperties8)data[index++] : Tproperties8.Empty;
                    ret.properties9 = data.Length > index ? (Tproperties9)data[index++] : Tproperties9.Empty;
                    ret.properties10 = data.Length > index ? (Tproperties10)data[index++] : Tproperties10.Empty;
                    ret.properties11 = data.Length > index ? (Tproperties11)data[index++] : Tproperties11.Empty;
                    ret.properties12 = data.Length > index ? (Tproperties12)data[index++] : Tproperties12.Empty;
                    ret.properties13 = data.Length > index ? (Tproperties13)data[index++] : Tproperties13.Empty;
                    ret.properties14 = data.Length > index ? (Tproperties14)data[index++] : Tproperties14.Empty;
                    ret.properties15 = data.Length > index ? (Tproperties15)data[index++] : Tproperties15.Empty;
                    ret.properties16 = data.Length > index ? (Tproperties16)data[index++] : Tproperties16.Empty;
                    ret.properties17 = data.Length > index ? (Tproperties17)data[index++] : Tproperties17.Empty;
                    ret.nextTimeRangeId2 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](GENERIC_SCHEDULE_TIME_RANGE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_GENERIC_SCHEDULE.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.timeRangeId2.HasValue) ret.Add(command.timeRangeId2);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.properties3.HasValue) ret.Add(command.properties3);
                if (command.startYear2.HasValue) ret.Add(command.startYear2);
                if (command.properties4.HasValue) ret.Add(command.properties4);
                if (command.stopYear2.HasValue) ret.Add(command.stopYear2);
                if (command.properties5.HasValue) ret.Add(command.properties5);
                if (command.properties6.HasValue) ret.Add(command.properties6);
                if (command.properties7.HasValue) ret.Add(command.properties7);
                if (command.properties8.HasValue) ret.Add(command.properties8);
                if (command.properties9.HasValue) ret.Add(command.properties9);
                if (command.properties10.HasValue) ret.Add(command.properties10);
                if (command.properties11.HasValue) ret.Add(command.properties11);
                if (command.properties12.HasValue) ret.Add(command.properties12);
                if (command.properties13.HasValue) ret.Add(command.properties13);
                if (command.properties14.HasValue) ret.Add(command.properties14);
                if (command.properties15.HasValue) ret.Add(command.properties15);
                if (command.properties16.HasValue) ret.Add(command.properties16);
                if (command.properties17.HasValue) ret.Add(command.properties17);
                if (command.nextTimeRangeId2.HasValue) ret.Add(command.nextTimeRangeId2);
                return ret.ToArray();
            }
        }
        public partial class GENERIC_SCHEDULE_SET
        {
            public const byte ID = 0x06;
            public const byte scheduleIdBytesCount = 2;
            public byte[] scheduleId = new byte[scheduleIdBytesCount];
            public ByteValue numberOfTimeRangeIds = 0;
            public class TVG1
            {
                public struct Tproperties1
                {
                    private byte _value;
                    public bool HasValue { get; private set; }
                    public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                    public byte timeRangeId1
                    {
                        get { return (byte)(_value >> 0 & 0x7F); }
                        set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                    }
                    public byte include
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
                public ByteValue timeRangeId2 = 0;
            }
            public List<TVG1> vg1 = new List<TVG1>();
            public static implicit operator GENERIC_SCHEDULE_SET(byte[] data)
            {
                GENERIC_SCHEDULE_SET ret = new GENERIC_SCHEDULE_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.scheduleId = (data.Length - index) >= scheduleIdBytesCount ? new byte[scheduleIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.scheduleId[0] = data[index++];
                    if (data.Length > index) ret.scheduleId[1] = data[index++];
                    ret.numberOfTimeRangeIds = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.vg1 = new List<TVG1>();
                    for (int j = 0; j < ret.numberOfTimeRangeIds; j++)
                    {
                        TVG1 tmp = new TVG1();
                        tmp.properties1 = data.Length > index ? (TVG1.Tproperties1)data[index++] : TVG1.Tproperties1.Empty;
                        tmp.timeRangeId2 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        ret.vg1.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](GENERIC_SCHEDULE_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_GENERIC_SCHEDULE.ID);
                ret.Add(ID);
                if (command.scheduleId != null)
                {
                    foreach (var tmp in command.scheduleId)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.numberOfTimeRangeIds.HasValue) ret.Add(command.numberOfTimeRangeIds);
                if (command.vg1 != null)
                {
                    foreach (var item in command.vg1)
                    {
                        if (item.properties1.HasValue) ret.Add(item.properties1);
                        if (item.timeRangeId2.HasValue) ret.Add(item.timeRangeId2);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class GENERIC_SCHEDULE_GET
        {
            public const byte ID = 0x07;
            public const byte scheduleIdBytesCount = 2;
            public byte[] scheduleId = new byte[scheduleIdBytesCount];
            public static implicit operator GENERIC_SCHEDULE_GET(byte[] data)
            {
                GENERIC_SCHEDULE_GET ret = new GENERIC_SCHEDULE_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.scheduleId = (data.Length - index) >= scheduleIdBytesCount ? new byte[scheduleIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.scheduleId[0] = data[index++];
                    if (data.Length > index) ret.scheduleId[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](GENERIC_SCHEDULE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_GENERIC_SCHEDULE.ID);
                ret.Add(ID);
                if (command.scheduleId != null)
                {
                    foreach (var tmp in command.scheduleId)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class GENERIC_SCHEDULE_REPORT
        {
            public const byte ID = 0x08;
            public const byte scheduleIdBytesCount = 2;
            public byte[] scheduleId = new byte[scheduleIdBytesCount];
            public ByteValue numberOfTimeRangeIds = 0;
            public class TVG1
            {
                public struct Tproperties1
                {
                    private byte _value;
                    public bool HasValue { get; private set; }
                    public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                    public byte timeRangeId1
                    {
                        get { return (byte)(_value >> 0 & 0x7F); }
                        set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                    }
                    public byte include
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
                public ByteValue timeRangeId2 = 0;
            }
            public List<TVG1> vg1 = new List<TVG1>();
            public const byte nextScheduleIdBytesCount = 2;
            public byte[] nextScheduleId = new byte[nextScheduleIdBytesCount];
            public static implicit operator GENERIC_SCHEDULE_REPORT(byte[] data)
            {
                GENERIC_SCHEDULE_REPORT ret = new GENERIC_SCHEDULE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.scheduleId = (data.Length - index) >= scheduleIdBytesCount ? new byte[scheduleIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.scheduleId[0] = data[index++];
                    if (data.Length > index) ret.scheduleId[1] = data[index++];
                    ret.numberOfTimeRangeIds = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.vg1 = new List<TVG1>();
                    for (int j = 0; j < ret.numberOfTimeRangeIds; j++)
                    {
                        TVG1 tmp = new TVG1();
                        tmp.properties1 = data.Length > index ? (TVG1.Tproperties1)data[index++] : TVG1.Tproperties1.Empty;
                        tmp.timeRangeId2 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        ret.vg1.Add(tmp);
                    }
                    ret.nextScheduleId = (data.Length - index) >= nextScheduleIdBytesCount ? new byte[nextScheduleIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.nextScheduleId[0] = data[index++];
                    if (data.Length > index) ret.nextScheduleId[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](GENERIC_SCHEDULE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_GENERIC_SCHEDULE.ID);
                ret.Add(ID);
                if (command.scheduleId != null)
                {
                    foreach (var tmp in command.scheduleId)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.numberOfTimeRangeIds.HasValue) ret.Add(command.numberOfTimeRangeIds);
                if (command.vg1 != null)
                {
                    foreach (var item in command.vg1)
                    {
                        if (item.properties1.HasValue) ret.Add(item.properties1);
                        if (item.timeRangeId2.HasValue) ret.Add(item.timeRangeId2);
                    }
                }
                if (command.nextScheduleId != null)
                {
                    foreach (var tmp in command.nextScheduleId)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

