using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_TARIFF_TBL_MONITOR
    {
        public const byte ID = 0x4B;
        public const byte VERSION = 1;
        public partial class TARIFF_TBL_COST_GET
        {
            public const byte ID = 0x05;
            public ByteValue rateParameterSetId = 0;
            public const byte startYearBytesCount = 2;
            public byte[] startYear = new byte[startYearBytesCount];
            public ByteValue startMonth = 0;
            public ByteValue startDay = 0;
            public ByteValue startHourLocalTime = 0;
            public ByteValue startMinuteLocalTime = 0;
            public const byte stopYearBytesCount = 2;
            public byte[] stopYear = new byte[stopYearBytesCount];
            public ByteValue stopMonth = 0;
            public ByteValue stopDay = 0;
            public ByteValue stopHourLocalTime = 0;
            public ByteValue stopMinuteLocalTime = 0;
            public static implicit operator TARIFF_TBL_COST_GET(byte[] data)
            {
                TARIFF_TBL_COST_GET ret = new TARIFF_TBL_COST_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.rateParameterSetId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startYear = (data.Length - index) >= startYearBytesCount ? new byte[startYearBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.startYear[0] = data[index++];
                    if (data.Length > index) ret.startYear[1] = data[index++];
                    ret.startMonth = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startDay = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startHourLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startMinuteLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopYear = (data.Length - index) >= stopYearBytesCount ? new byte[stopYearBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.stopYear[0] = data[index++];
                    if (data.Length > index) ret.stopYear[1] = data[index++];
                    ret.stopMonth = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopDay = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopHourLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopMinuteLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](TARIFF_TBL_COST_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_TARIFF_TBL_MONITOR.ID);
                ret.Add(ID);
                if (command.rateParameterSetId.HasValue) ret.Add(command.rateParameterSetId);
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
                return ret.ToArray();
            }
        }
        public partial class TARIFF_TBL_COST_REPORT
        {
            public const byte ID = 0x06;
            public ByteValue rateParameterSetId = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte rateType
                {
                    get { return (byte)(_value >> 0 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x03; _value += (byte)(value << 0 & 0x03); }
                }
                public byte reserved1
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
            public const byte startYearBytesCount = 2;
            public byte[] startYear = new byte[startYearBytesCount];
            public ByteValue startMonth = 0;
            public ByteValue startDay = 0;
            public ByteValue startHourLocalTime = 0;
            public ByteValue startMinuteLocalTime = 0;
            public const byte stopYearBytesCount = 2;
            public byte[] stopYear = new byte[stopYearBytesCount];
            public ByteValue stopMonth = 0;
            public ByteValue stopDay = 0;
            public ByteValue stopHourLocalTime = 0;
            public ByteValue stopMinuteLocalTime = 0;
            public const byte currencyBytesCount = 3;
            public byte[] currency = new byte[currencyBytesCount];
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte reserved2
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte costPrecision
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
            public const byte costValueBytesCount = 4;
            public byte[] costValue = new byte[costValueBytesCount];
            public static implicit operator TARIFF_TBL_COST_REPORT(byte[] data)
            {
                TARIFF_TBL_COST_REPORT ret = new TARIFF_TBL_COST_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.rateParameterSetId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.startYear = (data.Length - index) >= startYearBytesCount ? new byte[startYearBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.startYear[0] = data[index++];
                    if (data.Length > index) ret.startYear[1] = data[index++];
                    ret.startMonth = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startDay = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startHourLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.startMinuteLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopYear = (data.Length - index) >= stopYearBytesCount ? new byte[stopYearBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.stopYear[0] = data[index++];
                    if (data.Length > index) ret.stopYear[1] = data[index++];
                    ret.stopMonth = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopDay = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopHourLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stopMinuteLocalTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.currency = (data.Length - index) >= currencyBytesCount ? new byte[currencyBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.currency[0] = data[index++];
                    if (data.Length > index) ret.currency[1] = data[index++];
                    if (data.Length > index) ret.currency[2] = data[index++];
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.costValue = (data.Length - index) >= costValueBytesCount ? new byte[costValueBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.costValue[0] = data[index++];
                    if (data.Length > index) ret.costValue[1] = data[index++];
                    if (data.Length > index) ret.costValue[2] = data[index++];
                    if (data.Length > index) ret.costValue[3] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](TARIFF_TBL_COST_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_TARIFF_TBL_MONITOR.ID);
                ret.Add(ID);
                if (command.rateParameterSetId.HasValue) ret.Add(command.rateParameterSetId);
                if (command.properties1.HasValue) ret.Add(command.properties1);
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
                if (command.currency != null)
                {
                    foreach (var tmp in command.currency)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.costValue != null)
                {
                    foreach (var tmp in command.costValue)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class TARIFF_TBL_GET
        {
            public const byte ID = 0x03;
            public ByteValue rateParameterSetId = 0;
            public static implicit operator TARIFF_TBL_GET(byte[] data)
            {
                TARIFF_TBL_GET ret = new TARIFF_TBL_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.rateParameterSetId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](TARIFF_TBL_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_TARIFF_TBL_MONITOR.ID);
                ret.Add(ID);
                if (command.rateParameterSetId.HasValue) ret.Add(command.rateParameterSetId);
                return ret.ToArray();
            }
        }
        public partial class TARIFF_TBL_REPORT
        {
            public const byte ID = 0x04;
            public ByteValue rateParameterSetId = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte reserved
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte tariffPrecision
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
            public const byte tariffValueBytesCount = 4;
            public byte[] tariffValue = new byte[tariffValueBytesCount];
            public static implicit operator TARIFF_TBL_REPORT(byte[] data)
            {
                TARIFF_TBL_REPORT ret = new TARIFF_TBL_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.rateParameterSetId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.tariffValue = (data.Length - index) >= tariffValueBytesCount ? new byte[tariffValueBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.tariffValue[0] = data[index++];
                    if (data.Length > index) ret.tariffValue[1] = data[index++];
                    if (data.Length > index) ret.tariffValue[2] = data[index++];
                    if (data.Length > index) ret.tariffValue[3] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](TARIFF_TBL_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_TARIFF_TBL_MONITOR.ID);
                ret.Add(ID);
                if (command.rateParameterSetId.HasValue) ret.Add(command.rateParameterSetId);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.tariffValue != null)
                {
                    foreach (var tmp in command.tariffValue)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class TARIFF_TBL_SUPPLIER_GET
        {
            public const byte ID = 0x01;
            public static implicit operator TARIFF_TBL_SUPPLIER_GET(byte[] data)
            {
                TARIFF_TBL_SUPPLIER_GET ret = new TARIFF_TBL_SUPPLIER_GET();
                return ret;
            }
            public static implicit operator byte[](TARIFF_TBL_SUPPLIER_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_TARIFF_TBL_MONITOR.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class TARIFF_TBL_SUPPLIER_REPORT
        {
            public const byte ID = 0x02;
            public const byte yearBytesCount = 2;
            public byte[] year = new byte[yearBytesCount];
            public ByteValue month = 0;
            public ByteValue day = 0;
            public ByteValue hourLocalTime = 0;
            public ByteValue minuteLocalTime = 0;
            public ByteValue secondLocalTime = 0;
            public const byte currencyBytesCount = 3;
            public byte[] currency = new byte[currencyBytesCount];
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte standingChargePeriod
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte standingChargePrecision
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
            public const byte standingChargeValueBytesCount = 4;
            public byte[] standingChargeValue = new byte[standingChargeValueBytesCount];
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte numberOfSupplierCharacters
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte reserved
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
            public IList<byte> supplierCharacter = new List<byte>();
            public static implicit operator TARIFF_TBL_SUPPLIER_REPORT(byte[] data)
            {
                TARIFF_TBL_SUPPLIER_REPORT ret = new TARIFF_TBL_SUPPLIER_REPORT();
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
                    ret.currency = (data.Length - index) >= currencyBytesCount ? new byte[currencyBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.currency[0] = data[index++];
                    if (data.Length > index) ret.currency[1] = data[index++];
                    if (data.Length > index) ret.currency[2] = data[index++];
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.standingChargeValue = (data.Length - index) >= standingChargeValueBytesCount ? new byte[standingChargeValueBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.standingChargeValue[0] = data[index++];
                    if (data.Length > index) ret.standingChargeValue[1] = data[index++];
                    if (data.Length > index) ret.standingChargeValue[2] = data[index++];
                    if (data.Length > index) ret.standingChargeValue[3] = data[index++];
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.supplierCharacter = new List<byte>();
                    for (int i = 0; i < ret.properties2.numberOfSupplierCharacters; i++)
                    {
                        if (data.Length > index) ret.supplierCharacter.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](TARIFF_TBL_SUPPLIER_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_TARIFF_TBL_MONITOR.ID);
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
                if (command.currency != null)
                {
                    foreach (var tmp in command.currency)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.standingChargeValue != null)
                {
                    foreach (var tmp in command.standingChargeValue)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.supplierCharacter != null)
                {
                    foreach (var tmp in command.supplierCharacter)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

