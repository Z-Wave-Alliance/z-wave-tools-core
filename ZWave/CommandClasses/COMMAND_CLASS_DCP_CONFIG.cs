using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_DCP_CONFIG
    {
        public const byte ID = 0x3A;
        public const byte VERSION = 1;
        public partial class DCP_LIST_REMOVE
        {
            public const byte ID = 0x04;
            public const byte yearBytesCount = 2;
            public byte[] year = new byte[yearBytesCount];
            public ByteValue month = 0;
            public ByteValue day = 0;
            public ByteValue hourLocalTime = 0;
            public ByteValue minuteLocalTime = 0;
            public ByteValue secondLocalTime = 0;
            public static implicit operator DCP_LIST_REMOVE(byte[] data)
            {
                DCP_LIST_REMOVE ret = new DCP_LIST_REMOVE();
                if (data != null)
                {
                    int index = 2;
                    ret.year = (data.Length - index) >= yearBytesCount ? new byte[yearBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.year[0] = data[index++];
                    if (data.Length > index) ret.year[1] = data[index++];
                    ret.month = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.day = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.hourLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.minuteLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.secondLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](DCP_LIST_REMOVE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_DCP_CONFIG.ID);
                ret.Add(ID);
                if (command.year != null)
                {
                    foreach (var tmp in command.year)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.month.HasValue) ret.Add(command.month);
                if (command.day.HasValue) ret.Add(command.day);
                if (command.hourLocalTime.HasValue) ret.Add(command.hourLocalTime);
                if (command.minuteLocalTime.HasValue) ret.Add(command.minuteLocalTime);
                if (command.secondLocalTime.HasValue) ret.Add(command.secondLocalTime);
                return ret.ToArray();
            }
        }
        public partial class DCP_LIST_SET
        {
            public const byte ID = 0x03;
            public const byte yearBytesCount = 2;
            public byte[] year = new byte[yearBytesCount];
            public ByteValue month = 0;
            public ByteValue day = 0;
            public ByteValue hourLocalTime = 0;
            public ByteValue minuteLocalTime = 0;
            public ByteValue secondLocalTime = 0;
            public ByteValue dcpRateId = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte numberOfDc
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
            public class TVG1
            {
                public ByteValue genericDeviceClass = 0;
                public ByteValue specificDeviceClass = 0;
            }
            public List<TVG1> vg1 = new List<TVG1>();
            public const byte startYearBytesCount = 2;
            public byte[] startYear = new byte[startYearBytesCount];
            public ByteValue startMonth = 0;
            public ByteValue startDay = 0;
            public ByteValue startHourLocalTime = 0;
            public ByteValue startMinuteLocalTime = 0;
            public ByteValue startSecondLocalTime = 0;
            public ByteValue durationHourTime = 0;
            public ByteValue durationMinuteTime = 0;
            public ByteValue durationSecondTime = 0;
            public ByteValue eventPriority = 0;
            public ByteValue loadShedding = 0;
            public ByteValue startAssociationGroup = 0;
            public ByteValue stopAssociationGroup = 0;
            public ByteValue randomizationInterval = 0;
            public static implicit operator DCP_LIST_SET(byte[] data)
            {
                DCP_LIST_SET ret = new DCP_LIST_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.year = (data.Length - index) >= yearBytesCount ? new byte[yearBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.year[0] = data[index++];
                    if (data.Length > index) ret.year[1] = data[index++];
                    ret.month = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.day = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.hourLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.minuteLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.secondLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.dcpRateId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.vg1 = new List<TVG1>();
                    for (int j = 0; j < ret.properties1.numberOfDc; j++)
                    {
                        TVG1 tmp = new TVG1();
                        tmp.genericDeviceClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.specificDeviceClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        ret.vg1.Add(tmp);
                    }
                    ret.startYear = (data.Length - index) >= startYearBytesCount ? new byte[startYearBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.startYear[0] = data[index++];
                    if (data.Length > index) ret.startYear[1] = data[index++];
                    ret.startMonth = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startDay = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startHourLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startMinuteLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startSecondLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.durationHourTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.durationMinuteTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.durationSecondTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.eventPriority = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.loadShedding = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startAssociationGroup = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopAssociationGroup = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.randomizationInterval = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](DCP_LIST_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_DCP_CONFIG.ID);
                ret.Add(ID);
                if (command.year != null)
                {
                    foreach (var tmp in command.year)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.month.HasValue) ret.Add(command.month);
                if (command.day.HasValue) ret.Add(command.day);
                if (command.hourLocalTime.HasValue) ret.Add(command.hourLocalTime);
                if (command.minuteLocalTime.HasValue) ret.Add(command.minuteLocalTime);
                if (command.secondLocalTime.HasValue) ret.Add(command.secondLocalTime);
                if (command.dcpRateId.HasValue) ret.Add(command.dcpRateId);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.vg1 != null)
                {
                    foreach (var item in command.vg1)
                    {
                        if (item.genericDeviceClass.HasValue) ret.Add(item.genericDeviceClass);
                        if (item.specificDeviceClass.HasValue) ret.Add(item.specificDeviceClass);
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
                if (command.startHourLocalTime.HasValue) ret.Add(command.startHourLocalTime);
                if (command.startMinuteLocalTime.HasValue) ret.Add(command.startMinuteLocalTime);
                if (command.startSecondLocalTime.HasValue) ret.Add(command.startSecondLocalTime);
                if (command.durationHourTime.HasValue) ret.Add(command.durationHourTime);
                if (command.durationMinuteTime.HasValue) ret.Add(command.durationMinuteTime);
                if (command.durationSecondTime.HasValue) ret.Add(command.durationSecondTime);
                if (command.eventPriority.HasValue) ret.Add(command.eventPriority);
                if (command.loadShedding.HasValue) ret.Add(command.loadShedding);
                if (command.startAssociationGroup.HasValue) ret.Add(command.startAssociationGroup);
                if (command.stopAssociationGroup.HasValue) ret.Add(command.stopAssociationGroup);
                if (command.randomizationInterval.HasValue) ret.Add(command.randomizationInterval);
                return ret.ToArray();
            }
        }
        public partial class DCP_LIST_SUPPORTED_GET
        {
            public const byte ID = 0x01;
            public static implicit operator DCP_LIST_SUPPORTED_GET(byte[] data)
            {
                DCP_LIST_SUPPORTED_GET ret = new DCP_LIST_SUPPORTED_GET();
                return ret;
            }
            public static implicit operator byte[](DCP_LIST_SUPPORTED_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_DCP_CONFIG.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class DCP_LIST_SUPPORTED_REPORT
        {
            public const byte ID = 0x02;
            public ByteValue dcpListSize = 0;
            public ByteValue freeDcpListEntries = 0;
            public static implicit operator DCP_LIST_SUPPORTED_REPORT(byte[] data)
            {
                DCP_LIST_SUPPORTED_REPORT ret = new DCP_LIST_SUPPORTED_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.dcpListSize = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.freeDcpListEntries = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](DCP_LIST_SUPPORTED_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_DCP_CONFIG.ID);
                ret.Add(ID);
                if (command.dcpListSize.HasValue) ret.Add(command.dcpListSize);
                if (command.freeDcpListEntries.HasValue) ret.Add(command.freeDcpListEntries);
                return ret.ToArray();
            }
        }
    }
}

