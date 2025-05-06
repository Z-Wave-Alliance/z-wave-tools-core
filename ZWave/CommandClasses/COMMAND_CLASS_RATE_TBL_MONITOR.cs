using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_RATE_TBL_MONITOR
    {
        public const byte ID = 0x49;
        public const byte VERSION = 1;
        public partial class RATE_TBL_ACTIVE_RATE_GET
        {
            public const byte ID = 0x05;
            public static implicit operator RATE_TBL_ACTIVE_RATE_GET(byte[] data)
            {
                RATE_TBL_ACTIVE_RATE_GET ret = new RATE_TBL_ACTIVE_RATE_GET();
                return ret;
            }
            public static implicit operator byte[](RATE_TBL_ACTIVE_RATE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_RATE_TBL_MONITOR.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class RATE_TBL_ACTIVE_RATE_REPORT
        {
            public const byte ID = 0x06;
            public ByteValue rateParameterSetId = 0;
            public static implicit operator RATE_TBL_ACTIVE_RATE_REPORT(byte[] data)
            {
                RATE_TBL_ACTIVE_RATE_REPORT ret = new RATE_TBL_ACTIVE_RATE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.rateParameterSetId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](RATE_TBL_ACTIVE_RATE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_RATE_TBL_MONITOR.ID);
                ret.Add(ID);
                if (command.rateParameterSetId.HasValue) ret.Add(command.rateParameterSetId);
                return ret.ToArray();
            }
        }
        public partial class RATE_TBL_CURRENT_DATA_GET
        {
            public const byte ID = 0x07;
            public ByteValue rateParameterSetId = 0;
            public const byte datasetRequestedBytesCount = 3;
            public byte[] datasetRequested = new byte[datasetRequestedBytesCount];
            public static implicit operator RATE_TBL_CURRENT_DATA_GET(byte[] data)
            {
                RATE_TBL_CURRENT_DATA_GET ret = new RATE_TBL_CURRENT_DATA_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.rateParameterSetId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.datasetRequested = (data.Length - index) >= datasetRequestedBytesCount ? new byte[datasetRequestedBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.datasetRequested[0] = data[index++];
                    if (data.Length > index) ret.datasetRequested[1] = data[index++];
                    if (data.Length > index) ret.datasetRequested[2] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](RATE_TBL_CURRENT_DATA_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_RATE_TBL_MONITOR.ID);
                ret.Add(ID);
                if (command.rateParameterSetId.HasValue) ret.Add(command.rateParameterSetId);
                if (command.datasetRequested != null)
                {
                    foreach (var tmp in command.datasetRequested)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class RATE_TBL_CURRENT_DATA_REPORT
        {
            public const byte ID = 0x08;
            public ByteValue reportsToFollow = 0;
            public ByteValue rateParameterSetId = 0;
            public const byte datasetBytesCount = 3;
            public byte[] dataset = new byte[datasetBytesCount];
            public const byte yearBytesCount = 2;
            public byte[] year = new byte[yearBytesCount];
            public ByteValue month = 0;
            public ByteValue day = 0;
            public ByteValue hourLocalTime = 0;
            public ByteValue minuteLocalTime = 0;
            public ByteValue secondLocalTime = 0;
            public class TVG
            {
                public struct Tproperties1
                {
                    private byte _value;
                    public bool HasValue { get; private set; }
                    public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                    public byte currentScale
                    {
                        get { return (byte)(_value >> 0 & 0x1F); }
                        set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                    }
                    public byte currentPrecision
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
                public const byte currentValueBytesCount = 4;
                public byte[] currentValue = new byte[currentValueBytesCount];
            }
            public List<TVG> vg = new List<TVG>();
            public static implicit operator RATE_TBL_CURRENT_DATA_REPORT(byte[] data)
            {
                RATE_TBL_CURRENT_DATA_REPORT ret = new RATE_TBL_CURRENT_DATA_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.reportsToFollow = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.rateParameterSetId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.dataset = (data.Length - index) >= datasetBytesCount ? new byte[datasetBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.dataset[0] = data[index++];
                    if (data.Length > index) ret.dataset[1] = data[index++];
                    if (data.Length > index) ret.dataset[2] = data[index++];
                    ret.year = (data.Length - index) >= yearBytesCount ? new byte[yearBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.year[0] = data[index++];
                    if (data.Length > index) ret.year[1] = data[index++];
                    ret.month = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.day = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.hourLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.minuteLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.secondLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.vg = new List<TVG>();
                    for (int j = 0; j < ret.reportsToFollow; j++)
                    {
                        TVG tmp = new TVG();
                        tmp.properties1 = data.Length > index ? (TVG.Tproperties1)data[index++] : TVG.Tproperties1.Empty;
                        tmp.currentValue = (data.Length - index) >= TVG.currentValueBytesCount ? new byte[TVG.currentValueBytesCount] : new byte[data.Length - index];
                        if (data.Length > index) tmp.currentValue[0] = data[index++];
                        if (data.Length > index) tmp.currentValue[1] = data[index++];
                        if (data.Length > index) tmp.currentValue[2] = data[index++];
                        if (data.Length > index) tmp.currentValue[3] = data[index++];
                        ret.vg.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](RATE_TBL_CURRENT_DATA_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_RATE_TBL_MONITOR.ID);
                ret.Add(ID);
                if (command.reportsToFollow.HasValue) ret.Add(command.reportsToFollow);
                if (command.rateParameterSetId.HasValue) ret.Add(command.rateParameterSetId);
                if (command.dataset != null)
                {
                    foreach (var tmp in command.dataset)
                    {
                        ret.Add(tmp);
                    }
                }
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
                if (command.vg != null)
                {
                    foreach (var item in command.vg)
                    {
                        if (item.properties1.HasValue) ret.Add(item.properties1);
                        if (item.currentValue != null)
                        {
                            foreach (var tmp in item.currentValue)
                            {
                                ret.Add(tmp);
                            }
                        }
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class RATE_TBL_GET
        {
            public const byte ID = 0x03;
            public ByteValue rateParameterSetId = 0;
            public static implicit operator RATE_TBL_GET(byte[] data)
            {
                RATE_TBL_GET ret = new RATE_TBL_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.rateParameterSetId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](RATE_TBL_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_RATE_TBL_MONITOR.ID);
                ret.Add(ID);
                if (command.rateParameterSetId.HasValue) ret.Add(command.rateParameterSetId);
                return ret.ToArray();
            }
        }
        public partial class RATE_TBL_HISTORICAL_DATA_GET
        {
            public const byte ID = 0x09;
            public ByteValue maximumReports = 0;
            public ByteValue rateParameterSetId = 0;
            public const byte datasetRequestedBytesCount = 3;
            public byte[] datasetRequested = new byte[datasetRequestedBytesCount];
            public const byte startYearBytesCount = 2;
            public byte[] startYear = new byte[startYearBytesCount];
            public ByteValue startMonth = 0;
            public ByteValue startDay = 0;
            public ByteValue startHourLocalTime = 0;
            public ByteValue startMinuteLocalTime = 0;
            public ByteValue startSecondLocalTime = 0;
            public const byte stopYearBytesCount = 2;
            public byte[] stopYear = new byte[stopYearBytesCount];
            public ByteValue stopMonth = 0;
            public ByteValue stopDay = 0;
            public ByteValue stopHourLocalTime = 0;
            public ByteValue stopMinuteLocalTime = 0;
            public ByteValue stopSecondLocalTime = 0;
            public static implicit operator RATE_TBL_HISTORICAL_DATA_GET(byte[] data)
            {
                RATE_TBL_HISTORICAL_DATA_GET ret = new RATE_TBL_HISTORICAL_DATA_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.maximumReports = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.rateParameterSetId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.datasetRequested = (data.Length - index) >= datasetRequestedBytesCount ? new byte[datasetRequestedBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.datasetRequested[0] = data[index++];
                    if (data.Length > index) ret.datasetRequested[1] = data[index++];
                    if (data.Length > index) ret.datasetRequested[2] = data[index++];
                    ret.startYear = (data.Length - index) >= startYearBytesCount ? new byte[startYearBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.startYear[0] = data[index++];
                    if (data.Length > index) ret.startYear[1] = data[index++];
                    ret.startMonth = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startDay = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startHourLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startMinuteLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startSecondLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopYear = (data.Length - index) >= stopYearBytesCount ? new byte[stopYearBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.stopYear[0] = data[index++];
                    if (data.Length > index) ret.stopYear[1] = data[index++];
                    ret.stopMonth = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopDay = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopHourLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopMinuteLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopSecondLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](RATE_TBL_HISTORICAL_DATA_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_RATE_TBL_MONITOR.ID);
                ret.Add(ID);
                if (command.maximumReports.HasValue) ret.Add(command.maximumReports);
                if (command.rateParameterSetId.HasValue) ret.Add(command.rateParameterSetId);
                if (command.datasetRequested != null)
                {
                    foreach (var tmp in command.datasetRequested)
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
                if (command.startHourLocalTime.HasValue) ret.Add(command.startHourLocalTime);
                if (command.startMinuteLocalTime.HasValue) ret.Add(command.startMinuteLocalTime);
                if (command.startSecondLocalTime.HasValue) ret.Add(command.startSecondLocalTime);
                if (command.stopYear != null)
                {
                    foreach (var tmp in command.stopYear)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.stopMonth.HasValue) ret.Add(command.stopMonth);
                if (command.stopDay.HasValue) ret.Add(command.stopDay);
                if (command.stopHourLocalTime.HasValue) ret.Add(command.stopHourLocalTime);
                if (command.stopMinuteLocalTime.HasValue) ret.Add(command.stopMinuteLocalTime);
                if (command.stopSecondLocalTime.HasValue) ret.Add(command.stopSecondLocalTime);
                return ret.ToArray();
            }
        }
        public partial class RATE_TBL_HISTORICAL_DATA_REPORT
        {
            public const byte ID = 0x0A;
            public ByteValue reportsToFollow = 0;
            public ByteValue rateParameterSetId = 0;
            public const byte datasetBytesCount = 3;
            public byte[] dataset = new byte[datasetBytesCount];
            public const byte yearBytesCount = 2;
            public byte[] year = new byte[yearBytesCount];
            public ByteValue month = 0;
            public ByteValue day = 0;
            public ByteValue hourLocalTime = 0;
            public ByteValue minuteLocalTime = 0;
            public ByteValue secondLocalTime = 0;
            public class TVG
            {
                public struct Tproperties1
                {
                    private byte _value;
                    public bool HasValue { get; private set; }
                    public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                    public byte historicalScale
                    {
                        get { return (byte)(_value >> 0 & 0x1F); }
                        set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                    }
                    public byte historicalPrecision
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
                public const byte historicalValueBytesCount = 4;
                public byte[] historicalValue = new byte[historicalValueBytesCount];
            }
            public List<TVG> vg = new List<TVG>();
            public static implicit operator RATE_TBL_HISTORICAL_DATA_REPORT(byte[] data)
            {
                RATE_TBL_HISTORICAL_DATA_REPORT ret = new RATE_TBL_HISTORICAL_DATA_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.reportsToFollow = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.rateParameterSetId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.dataset = (data.Length - index) >= datasetBytesCount ? new byte[datasetBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.dataset[0] = data[index++];
                    if (data.Length > index) ret.dataset[1] = data[index++];
                    if (data.Length > index) ret.dataset[2] = data[index++];
                    ret.year = (data.Length - index) >= yearBytesCount ? new byte[yearBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.year[0] = data[index++];
                    if (data.Length > index) ret.year[1] = data[index++];
                    ret.month = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.day = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.hourLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.minuteLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.secondLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.vg = new List<TVG>();
                    for (int j = 0; j < ret.reportsToFollow; j++)
                    {
                        TVG tmp = new TVG();
                        tmp.properties1 = data.Length > index ? (TVG.Tproperties1)data[index++] : TVG.Tproperties1.Empty;
                        tmp.historicalValue = (data.Length - index) >= TVG.historicalValueBytesCount ? new byte[TVG.historicalValueBytesCount] : new byte[data.Length - index];
                        if (data.Length > index) tmp.historicalValue[0] = data[index++];
                        if (data.Length > index) tmp.historicalValue[1] = data[index++];
                        if (data.Length > index) tmp.historicalValue[2] = data[index++];
                        if (data.Length > index) tmp.historicalValue[3] = data[index++];
                        ret.vg.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](RATE_TBL_HISTORICAL_DATA_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_RATE_TBL_MONITOR.ID);
                ret.Add(ID);
                if (command.reportsToFollow.HasValue) ret.Add(command.reportsToFollow);
                if (command.rateParameterSetId.HasValue) ret.Add(command.rateParameterSetId);
                if (command.dataset != null)
                {
                    foreach (var tmp in command.dataset)
                    {
                        ret.Add(tmp);
                    }
                }
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
                if (command.vg != null)
                {
                    foreach (var item in command.vg)
                    {
                        if (item.properties1.HasValue) ret.Add(item.properties1);
                        if (item.historicalValue != null)
                        {
                            foreach (var tmp in item.historicalValue)
                            {
                                ret.Add(tmp);
                            }
                        }
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class RATE_TBL_REPORT
        {
            public const byte ID = 0x04;
            public ByteValue rateParameterSetId = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte numberOfRateChar
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte rateType
                {
                    get { return (byte)(_value >> 5 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x60; _value += (byte)(value << 5 & 0x60); }
                }
                public byte reserved
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
            public IList<byte> rateCharacter = new List<byte>();
            public ByteValue startHourLocalTime = 0;
            public ByteValue startMinuteLocalTime = 0;
            public const byte durationMinuteBytesCount = 2;
            public byte[] durationMinute = new byte[durationMinuteBytesCount];
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte consumptionScale
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte consumptionPrecision
                {
                    get { return (byte)(_value >> 5 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0xE0; _value += (byte)(value << 5 & 0xE0); }
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
            public const byte minConsumptionValueBytesCount = 4;
            public byte[] minConsumptionValue = new byte[minConsumptionValueBytesCount];
            public const byte maxConsumptionValueBytesCount = 4;
            public byte[] maxConsumptionValue = new byte[maxConsumptionValueBytesCount];
            public struct Tproperties3
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties3 Empty { get { return new Tproperties3() { _value = 0, HasValue = false }; } }
                public byte maxDemandScale
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte maxDemandPrecision
                {
                    get { return (byte)(_value >> 5 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0xE0; _value += (byte)(value << 5 & 0xE0); }
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
            public const byte maxDemandValueBytesCount = 4;
            public byte[] maxDemandValue = new byte[maxDemandValueBytesCount];
            public ByteValue dcpRateId = 0;
            public static implicit operator RATE_TBL_REPORT(byte[] data)
            {
                RATE_TBL_REPORT ret = new RATE_TBL_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.rateParameterSetId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.rateCharacter = new List<byte>();
                    for (int i = 0; i < ret.properties1.numberOfRateChar; i++)
                    {
                        if (data.Length > index) ret.rateCharacter.Add(data[index++]);
                    }
                    ret.startHourLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startMinuteLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.durationMinute = (data.Length - index) >= durationMinuteBytesCount ? new byte[durationMinuteBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.durationMinute[0] = data[index++];
                    if (data.Length > index) ret.durationMinute[1] = data[index++];
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.minConsumptionValue = (data.Length - index) >= minConsumptionValueBytesCount ? new byte[minConsumptionValueBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.minConsumptionValue[0] = data[index++];
                    if (data.Length > index) ret.minConsumptionValue[1] = data[index++];
                    if (data.Length > index) ret.minConsumptionValue[2] = data[index++];
                    if (data.Length > index) ret.minConsumptionValue[3] = data[index++];
                    ret.maxConsumptionValue = (data.Length - index) >= maxConsumptionValueBytesCount ? new byte[maxConsumptionValueBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.maxConsumptionValue[0] = data[index++];
                    if (data.Length > index) ret.maxConsumptionValue[1] = data[index++];
                    if (data.Length > index) ret.maxConsumptionValue[2] = data[index++];
                    if (data.Length > index) ret.maxConsumptionValue[3] = data[index++];
                    ret.properties3 = data.Length > index ? (Tproperties3)data[index++] : Tproperties3.Empty;
                    ret.maxDemandValue = (data.Length - index) >= maxDemandValueBytesCount ? new byte[maxDemandValueBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.maxDemandValue[0] = data[index++];
                    if (data.Length > index) ret.maxDemandValue[1] = data[index++];
                    if (data.Length > index) ret.maxDemandValue[2] = data[index++];
                    if (data.Length > index) ret.maxDemandValue[3] = data[index++];
                    ret.dcpRateId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](RATE_TBL_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_RATE_TBL_MONITOR.ID);
                ret.Add(ID);
                if (command.rateParameterSetId.HasValue) ret.Add(command.rateParameterSetId);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.rateCharacter != null)
                {
                    foreach (var tmp in command.rateCharacter)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.startHourLocalTime.HasValue) ret.Add(command.startHourLocalTime);
                if (command.startMinuteLocalTime.HasValue) ret.Add(command.startMinuteLocalTime);
                if (command.durationMinute != null)
                {
                    foreach (var tmp in command.durationMinute)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.minConsumptionValue != null)
                {
                    foreach (var tmp in command.minConsumptionValue)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.maxConsumptionValue != null)
                {
                    foreach (var tmp in command.maxConsumptionValue)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties3.HasValue) ret.Add(command.properties3);
                if (command.maxDemandValue != null)
                {
                    foreach (var tmp in command.maxDemandValue)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.dcpRateId.HasValue) ret.Add(command.dcpRateId);
                return ret.ToArray();
            }
        }
        public partial class RATE_TBL_SUPPORTED_GET
        {
            public const byte ID = 0x01;
            public static implicit operator RATE_TBL_SUPPORTED_GET(byte[] data)
            {
                RATE_TBL_SUPPORTED_GET ret = new RATE_TBL_SUPPORTED_GET();
                return ret;
            }
            public static implicit operator byte[](RATE_TBL_SUPPORTED_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_RATE_TBL_MONITOR.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class RATE_TBL_SUPPORTED_REPORT
        {
            public const byte ID = 0x02;
            public ByteValue ratesSupported = 0;
            public const byte parameterSetSupportedBitMaskBytesCount = 2;
            public byte[] parameterSetSupportedBitMask = new byte[parameterSetSupportedBitMaskBytesCount];
            public static implicit operator RATE_TBL_SUPPORTED_REPORT(byte[] data)
            {
                RATE_TBL_SUPPORTED_REPORT ret = new RATE_TBL_SUPPORTED_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.ratesSupported = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.parameterSetSupportedBitMask = (data.Length - index) >= parameterSetSupportedBitMaskBytesCount ? new byte[parameterSetSupportedBitMaskBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.parameterSetSupportedBitMask[0] = data[index++];
                    if (data.Length > index) ret.parameterSetSupportedBitMask[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](RATE_TBL_SUPPORTED_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_RATE_TBL_MONITOR.ID);
                ret.Add(ID);
                if (command.ratesSupported.HasValue) ret.Add(command.ratesSupported);
                if (command.parameterSetSupportedBitMask != null)
                {
                    foreach (var tmp in command.parameterSetSupportedBitMask)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

