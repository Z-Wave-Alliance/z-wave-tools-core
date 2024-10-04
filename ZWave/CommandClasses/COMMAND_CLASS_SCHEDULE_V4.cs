/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_SCHEDULE_V4
    {
        public const byte ID = 0x53;
        public const byte VERSION = 4;
        public partial class SCHEDULE_SUPPORTED_GET
        {
            public const byte ID = 0x01;
            public ByteValue scheduleIdBlock = 0;
            public static implicit operator SCHEDULE_SUPPORTED_GET(byte[] data)
            {
                SCHEDULE_SUPPORTED_GET ret = new SCHEDULE_SUPPORTED_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.scheduleIdBlock = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SCHEDULE_SUPPORTED_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SCHEDULE_V4.ID);
                ret.Add(ID);
                if (command.scheduleIdBlock.HasValue) ret.Add(command.scheduleIdBlock);
                return ret.ToArray();
            }
        }
        public partial class SCHEDULE_SUPPORTED_REPORT
        {
            public const byte ID = 0x02;
            public ByteValue numberOfSupportedScheduleId = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte startTimeSupport
                {
                    get { return (byte)(_value >> 0 & 0x3F); }
                    set { HasValue = true; _value &= 0xFF - 0x3F; _value += (byte)(value << 0 & 0x3F); }
                }
                public byte fallbackSupport
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte supportEnableDisable
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
            public ByteValue numberOfSupportedCc = 0;
            public class TVG1
            {
                public ByteValue supportedCc = 0;
                public struct Tproperties1
                {
                    private byte _value;
                    public bool HasValue { get; private set; }
                    public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                    public byte supportedCommand
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
            }
            public List<TVG1> vg1 = new List<TVG1>();
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte supportedOverrideTypes
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte overrideSupport
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
            public ByteValue scheduleIdBlock = 0;
            public ByteValue numberOfSupportedScheduleBlocks = 0;
            public static implicit operator SCHEDULE_SUPPORTED_REPORT(byte[] data)
            {
                SCHEDULE_SUPPORTED_REPORT ret = new SCHEDULE_SUPPORTED_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.numberOfSupportedScheduleId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.numberOfSupportedCc = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.vg1 = new List<TVG1>();
                    for (int j = 0; j < ret.numberOfSupportedCc; j++)
                    {
                        TVG1 tmp = new TVG1();
                        tmp.supportedCc = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.properties1 = data.Length > index ? (TVG1.Tproperties1)data[index++] : TVG1.Tproperties1.Empty;
                        ret.vg1.Add(tmp);
                    }
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.scheduleIdBlock = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.numberOfSupportedScheduleBlocks = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SCHEDULE_SUPPORTED_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SCHEDULE_V4.ID);
                ret.Add(ID);
                if (command.numberOfSupportedScheduleId.HasValue) ret.Add(command.numberOfSupportedScheduleId);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.numberOfSupportedCc.HasValue) ret.Add(command.numberOfSupportedCc);
                if (command.vg1 != null)
                {
                    foreach (var item in command.vg1)
                    {
                        if (item.supportedCc.HasValue) ret.Add(item.supportedCc);
                        if (item.properties1.HasValue) ret.Add(item.properties1);
                    }
                }
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.scheduleIdBlock.HasValue) ret.Add(command.scheduleIdBlock);
                if (command.numberOfSupportedScheduleBlocks.HasValue) ret.Add(command.numberOfSupportedScheduleBlocks);
                return ret.ToArray();
            }
        }
        public partial class COMMAND_SCHEDULE_SET
        {
            public const byte ID = 0x03;
            public ByteValue scheduleId = 0;
            public ByteValue scheduleIdBlock = 0;
            public ByteValue startYear = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte startMonth
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte recurrenceOffset
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
                public byte startDayOfMonth
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte recurrenceMode
                {
                    get { return (byte)(_value >> 5 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x60; _value += (byte)(value << 5 & 0x60); }
                }
                public byte reserved1
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
                public byte startWeekday
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte reserved2
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
            public struct Tproperties4
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties4 Empty { get { return new Tproperties4() { _value = 0, HasValue = false }; } }
                public byte startHour
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte durationType
                {
                    get { return (byte)(_value >> 5 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0xE0; _value += (byte)(value << 5 & 0xE0); }
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
            public struct Tproperties5
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties5 Empty { get { return new Tproperties5() { _value = 0, HasValue = false }; } }
                public byte startMinute
                {
                    get { return (byte)(_value >> 0 & 0x3F); }
                    set { HasValue = true; _value &= 0xFF - 0x3F; _value += (byte)(value << 0 & 0x3F); }
                }
                public byte relative
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte reserved3
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
            public const byte durationByteBytesCount = 2;
            public byte[] durationByte = new byte[durationByteBytesCount];
            public ByteValue reportsToFollow = 0;
            public ByteValue numberOfCmdToFollow = 0;
            public class TVG1
            {
                public ByteValue cmdLength = 0;
                public IList<byte> cmdByte = new List<byte>();
            }
            public List<TVG1> vg1 = new List<TVG1>();
            public static implicit operator COMMAND_SCHEDULE_SET(byte[] data)
            {
                COMMAND_SCHEDULE_SET ret = new COMMAND_SCHEDULE_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.scheduleId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.scheduleIdBlock = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startYear = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.properties3 = data.Length > index ? (Tproperties3)data[index++] : Tproperties3.Empty;
                    ret.properties4 = data.Length > index ? (Tproperties4)data[index++] : Tproperties4.Empty;
                    ret.properties5 = data.Length > index ? (Tproperties5)data[index++] : Tproperties5.Empty;
                    ret.durationByte = (data.Length - index) >= durationByteBytesCount ? new byte[durationByteBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.durationByte[0] = data[index++];
                    if (data.Length > index) ret.durationByte[1] = data[index++];
                    ret.reportsToFollow = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.numberOfCmdToFollow = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.vg1 = new List<TVG1>();
                    for (int j = 0; j < ret.numberOfCmdToFollow; j++)
                    {
                        TVG1 tmp = new TVG1();
                        tmp.cmdLength = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.cmdByte = new List<byte>();
                        for (int i = 0; i < tmp.cmdLength; i++)
                        {
                            if (data.Length > index) tmp.cmdByte.Add(data[index++]);
                        }
                        ret.vg1.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](COMMAND_SCHEDULE_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SCHEDULE_V4.ID);
                ret.Add(ID);
                if (command.scheduleId.HasValue) ret.Add(command.scheduleId);
                if (command.scheduleIdBlock.HasValue) ret.Add(command.scheduleIdBlock);
                if (command.startYear.HasValue) ret.Add(command.startYear);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.properties3.HasValue) ret.Add(command.properties3);
                if (command.properties4.HasValue) ret.Add(command.properties4);
                if (command.properties5.HasValue) ret.Add(command.properties5);
                if (command.durationByte != null)
                {
                    foreach (var tmp in command.durationByte)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.reportsToFollow.HasValue) ret.Add(command.reportsToFollow);
                if (command.numberOfCmdToFollow.HasValue) ret.Add(command.numberOfCmdToFollow);
                if (command.vg1 != null)
                {
                    foreach (var item in command.vg1)
                    {
                        if (item.cmdLength.HasValue) ret.Add(item.cmdLength);
                        if (item.cmdByte != null)
                        {
                            foreach (var tmp in item.cmdByte)
                            {
                                ret.Add(tmp);
                            }
                        }
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class COMMAND_SCHEDULE_GET
        {
            public const byte ID = 0x04;
            public ByteValue scheduleId = 0;
            public ByteValue scheduleIdBlock = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte reserved
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte aidRoCtl
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
            public static implicit operator COMMAND_SCHEDULE_GET(byte[] data)
            {
                COMMAND_SCHEDULE_GET ret = new COMMAND_SCHEDULE_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.scheduleId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.scheduleIdBlock = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](COMMAND_SCHEDULE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SCHEDULE_V4.ID);
                ret.Add(ID);
                if (command.scheduleId.HasValue) ret.Add(command.scheduleId);
                if (command.scheduleIdBlock.HasValue) ret.Add(command.scheduleIdBlock);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                return ret.ToArray();
            }
        }
        public partial class COMMAND_SCHEDULE_REPORT
        {
            public const byte ID = 0x05;
            public ByteValue scheduleId = 0;
            public ByteValue scheduleIdBlock = 0;
            public ByteValue startYear = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte startMonth
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte aidRo
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
                public byte startDayOfMonth
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte recurrenceMode
                {
                    get { return (byte)(_value >> 5 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x60; _value += (byte)(value << 5 & 0x60); }
                }
                public byte aidRoCtl
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
                public byte startWeekday
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte reserved1
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
            public struct Tproperties4
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties4 Empty { get { return new Tproperties4() { _value = 0, HasValue = false }; } }
                public byte startHour
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte durationType
                {
                    get { return (byte)(_value >> 5 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0xE0; _value += (byte)(value << 5 & 0xE0); }
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
            public struct Tproperties5
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties5 Empty { get { return new Tproperties5() { _value = 0, HasValue = false }; } }
                public byte startMinute
                {
                    get { return (byte)(_value >> 0 & 0x3F); }
                    set { HasValue = true; _value &= 0xFF - 0x3F; _value += (byte)(value << 0 & 0x3F); }
                }
                public byte relative
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte reserved2
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
            public const byte durationByteBytesCount = 2;
            public byte[] durationByte = new byte[durationByteBytesCount];
            public ByteValue reportsToFollow = 0;
            public ByteValue numberOfCmdToFollow = 0;
            public class TVG1
            {
                public ByteValue cmdLength = 0;
                public IList<byte> cmdByte = new List<byte>();
            }
            public List<TVG1> vg1 = new List<TVG1>();
            public static implicit operator COMMAND_SCHEDULE_REPORT(byte[] data)
            {
                COMMAND_SCHEDULE_REPORT ret = new COMMAND_SCHEDULE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.scheduleId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.scheduleIdBlock = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startYear = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.properties3 = data.Length > index ? (Tproperties3)data[index++] : Tproperties3.Empty;
                    ret.properties4 = data.Length > index ? (Tproperties4)data[index++] : Tproperties4.Empty;
                    ret.properties5 = data.Length > index ? (Tproperties5)data[index++] : Tproperties5.Empty;
                    ret.durationByte = (data.Length - index) >= durationByteBytesCount ? new byte[durationByteBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.durationByte[0] = data[index++];
                    if (data.Length > index) ret.durationByte[1] = data[index++];
                    ret.reportsToFollow = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.numberOfCmdToFollow = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.vg1 = new List<TVG1>();
                    for (int j = 0; j < ret.numberOfCmdToFollow; j++)
                    {
                        TVG1 tmp = new TVG1();
                        tmp.cmdLength = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.cmdByte = new List<byte>();
                        for (int i = 0; i < tmp.cmdLength; i++)
                        {
                            if (data.Length > index) tmp.cmdByte.Add(data[index++]);
                        }
                        ret.vg1.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](COMMAND_SCHEDULE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SCHEDULE_V4.ID);
                ret.Add(ID);
                if (command.scheduleId.HasValue) ret.Add(command.scheduleId);
                if (command.scheduleIdBlock.HasValue) ret.Add(command.scheduleIdBlock);
                if (command.startYear.HasValue) ret.Add(command.startYear);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.properties3.HasValue) ret.Add(command.properties3);
                if (command.properties4.HasValue) ret.Add(command.properties4);
                if (command.properties5.HasValue) ret.Add(command.properties5);
                if (command.durationByte != null)
                {
                    foreach (var tmp in command.durationByte)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.reportsToFollow.HasValue) ret.Add(command.reportsToFollow);
                if (command.numberOfCmdToFollow.HasValue) ret.Add(command.numberOfCmdToFollow);
                if (command.vg1 != null)
                {
                    foreach (var item in command.vg1)
                    {
                        if (item.cmdLength.HasValue) ret.Add(item.cmdLength);
                        if (item.cmdByte != null)
                        {
                            foreach (var tmp in item.cmdByte)
                            {
                                ret.Add(tmp);
                            }
                        }
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class SCHEDULE_REMOVE
        {
            public const byte ID = 0x06;
            public ByteValue scheduleId = 0;
            public ByteValue scheduleIdBlock = 0;
            public static implicit operator SCHEDULE_REMOVE(byte[] data)
            {
                SCHEDULE_REMOVE ret = new SCHEDULE_REMOVE();
                if (data != null)
                {
                    int index = 2;
                    ret.scheduleId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.scheduleIdBlock = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SCHEDULE_REMOVE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SCHEDULE_V4.ID);
                ret.Add(ID);
                if (command.scheduleId.HasValue) ret.Add(command.scheduleId);
                if (command.scheduleIdBlock.HasValue) ret.Add(command.scheduleIdBlock);
                return ret.ToArray();
            }
        }
        public partial class SCHEDULE_STATE_SET
        {
            public const byte ID = 0x07;
            public ByteValue scheduleId = 0;
            public ByteValue scheduleState = 0;
            public ByteValue scheduleIdBlock = 0;
            public static implicit operator SCHEDULE_STATE_SET(byte[] data)
            {
                SCHEDULE_STATE_SET ret = new SCHEDULE_STATE_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.scheduleId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.scheduleState = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.scheduleIdBlock = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SCHEDULE_STATE_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SCHEDULE_V4.ID);
                ret.Add(ID);
                if (command.scheduleId.HasValue) ret.Add(command.scheduleId);
                if (command.scheduleState.HasValue) ret.Add(command.scheduleState);
                if (command.scheduleIdBlock.HasValue) ret.Add(command.scheduleIdBlock);
                return ret.ToArray();
            }
        }
        public partial class SCHEDULE_STATE_GET
        {
            public const byte ID = 0x08;
            public ByteValue scheduleIdBlock = 0;
            public static implicit operator SCHEDULE_STATE_GET(byte[] data)
            {
                SCHEDULE_STATE_GET ret = new SCHEDULE_STATE_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.scheduleIdBlock = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SCHEDULE_STATE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SCHEDULE_V4.ID);
                ret.Add(ID);
                if (command.scheduleIdBlock.HasValue) ret.Add(command.scheduleIdBlock);
                return ret.ToArray();
            }
        }
        public partial class SCHEDULE_STATE_REPORT
        {
            public const byte ID = 0x09;
            public ByteValue numberOfSupportedScheduleId = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte moverride
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte reportsToFollow
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
            public class TVG1
            {
                public struct Tproperties1
                {
                    private byte _value;
                    public bool HasValue { get; private set; }
                    public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                    public byte activeId1
                    {
                        get { return (byte)(_value >> 0 & 0x0F); }
                        set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                    }
                    public byte activeId2
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
            }
            public List<TVG1> vg1 = new List<TVG1>();
            public ByteValue scheduleIdBlock = 0;
            public static implicit operator SCHEDULE_STATE_REPORT(byte[] data)
            {
                SCHEDULE_STATE_REPORT ret = new SCHEDULE_STATE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.numberOfSupportedScheduleId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.vg1 = new List<TVG1>();
                    while (data.Length - 1 > index)
                    {
                        TVG1 tmp = new TVG1();
                        tmp.properties1 = data.Length > index ? (TVG1.Tproperties1)data[index++] : TVG1.Tproperties1.Empty;
                        ret.vg1.Add(tmp);
                    }
                    ret.scheduleIdBlock = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SCHEDULE_STATE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SCHEDULE_V4.ID);
                ret.Add(ID);
                if (command.numberOfSupportedScheduleId.HasValue) ret.Add(command.numberOfSupportedScheduleId);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.vg1 != null)
                {
                    foreach (var item in command.vg1)
                    {
                        if (item.properties1.HasValue) ret.Add(item.properties1);
                    }
                }
                if (command.scheduleIdBlock.HasValue) ret.Add(command.scheduleIdBlock);
                return ret.ToArray();
            }
        }
        public partial class SCHEDULE_SUPPORTED_COMMANDS_GET
        {
            public const byte ID = 0x0A;
            public ByteValue scheduleIdBlock = 0;
            public static implicit operator SCHEDULE_SUPPORTED_COMMANDS_GET(byte[] data)
            {
                SCHEDULE_SUPPORTED_COMMANDS_GET ret = new SCHEDULE_SUPPORTED_COMMANDS_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.scheduleIdBlock = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SCHEDULE_SUPPORTED_COMMANDS_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SCHEDULE_V4.ID);
                ret.Add(ID);
                if (command.scheduleIdBlock.HasValue) ret.Add(command.scheduleIdBlock);
                return ret.ToArray();
            }
        }
        public partial class SCHEDULE_SUPPORTED_COMMANDS_REPORT
        {
            public const byte ID = 0x0B;
            public ByteValue scheduleIdBlock = 0;
            public ByteValue commandClassListLength = 0;
            public class TVG1
            {
                public ByteValue commandClass = 0;
                public ByteValue supportedCommandListLength = 0;
                public IList<byte> supportedCommand = new List<byte>();
            }
            public List<TVG1> vg1 = new List<TVG1>();
            public static implicit operator SCHEDULE_SUPPORTED_COMMANDS_REPORT(byte[] data)
            {
                SCHEDULE_SUPPORTED_COMMANDS_REPORT ret = new SCHEDULE_SUPPORTED_COMMANDS_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.scheduleIdBlock = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.commandClassListLength = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.vg1 = new List<TVG1>();
                    for (int j = 0; j < ret.commandClassListLength; j++)
                    {
                        TVG1 tmp = new TVG1();
                        tmp.commandClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.supportedCommandListLength = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.supportedCommand = new List<byte>();
                        for (int i = 0; i < tmp.supportedCommandListLength; i++)
                        {
                            if (data.Length > index) tmp.supportedCommand.Add(data[index++]);
                        }
                        ret.vg1.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](SCHEDULE_SUPPORTED_COMMANDS_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SCHEDULE_V4.ID);
                ret.Add(ID);
                if (command.scheduleIdBlock.HasValue) ret.Add(command.scheduleIdBlock);
                if (command.commandClassListLength.HasValue) ret.Add(command.commandClassListLength);
                if (command.vg1 != null)
                {
                    foreach (var item in command.vg1)
                    {
                        if (item.commandClass.HasValue) ret.Add(item.commandClass);
                        if (item.supportedCommandListLength.HasValue) ret.Add(item.supportedCommandListLength);
                        if (item.supportedCommand != null)
                        {
                            foreach (var tmp in item.supportedCommand)
                            {
                                ret.Add(tmp);
                            }
                        }
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

