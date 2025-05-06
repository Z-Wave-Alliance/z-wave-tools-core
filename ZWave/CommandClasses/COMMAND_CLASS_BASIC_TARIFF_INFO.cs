using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_BASIC_TARIFF_INFO
    {
        public const byte ID = 0x36;
        public const byte VERSION = 1;
        public partial class BASIC_TARIFF_INFO_GET
        {
            public const byte ID = 0x01;
            public static implicit operator BASIC_TARIFF_INFO_GET(byte[] data)
            {
                BASIC_TARIFF_INFO_GET ret = new BASIC_TARIFF_INFO_GET();
                return ret;
            }
            public static implicit operator byte[](BASIC_TARIFF_INFO_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_BASIC_TARIFF_INFO.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class BASIC_TARIFF_INFO_REPORT
        {
            public const byte ID = 0x02;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte totalNoImportRates
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte reserved1
                {
                    get { return (byte)(_value >> 4 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x70; _value += (byte)(value << 4 & 0x70); }
                }
                public byte dual
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
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte e1CurrentRateInUse
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte reserved2
                {
                    get { return (byte)(_value >> 4 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0xF0; _value += (byte)(value << 4 & 0xF0); }
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
            public const byte e1RateConsumptionRegisterBytesCount = 4;
            public byte[] e1RateConsumptionRegister = new byte[e1RateConsumptionRegisterBytesCount];
            public ByteValue e1TimeForNextRateHours = 0;
            public ByteValue e1TimeForNextRateMinutes = 0;
            public ByteValue e1TimeForNextRateSeconds = 0;
            public struct Tproperties3
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties3 Empty { get { return new Tproperties3() { _value = 0, HasValue = false }; } }
                public byte e2CurrentRateInUse
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte reserved3
                {
                    get { return (byte)(_value >> 4 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0xF0; _value += (byte)(value << 4 & 0xF0); }
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
            public const byte e2RateConsumptionRegisterBytesCount = 4;
            public byte[] e2RateConsumptionRegister = new byte[e2RateConsumptionRegisterBytesCount];
            public static implicit operator BASIC_TARIFF_INFO_REPORT(byte[] data)
            {
                BASIC_TARIFF_INFO_REPORT ret = new BASIC_TARIFF_INFO_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.e1RateConsumptionRegister = (data.Length - index) >= e1RateConsumptionRegisterBytesCount ? new byte[e1RateConsumptionRegisterBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.e1RateConsumptionRegister[0] = data[index++];
                    if (data.Length > index) ret.e1RateConsumptionRegister[1] = data[index++];
                    if (data.Length > index) ret.e1RateConsumptionRegister[2] = data[index++];
                    if (data.Length > index) ret.e1RateConsumptionRegister[3] = data[index++];
                    ret.e1TimeForNextRateHours = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.e1TimeForNextRateMinutes = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.e1TimeForNextRateSeconds = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties3 = data.Length > index ? (Tproperties3)data[index++] : Tproperties3.Empty;
                    ret.e2RateConsumptionRegister = (data.Length - index) >= e2RateConsumptionRegisterBytesCount ? new byte[e2RateConsumptionRegisterBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.e2RateConsumptionRegister[0] = data[index++];
                    if (data.Length > index) ret.e2RateConsumptionRegister[1] = data[index++];
                    if (data.Length > index) ret.e2RateConsumptionRegister[2] = data[index++];
                    if (data.Length > index) ret.e2RateConsumptionRegister[3] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](BASIC_TARIFF_INFO_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_BASIC_TARIFF_INFO.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.e1RateConsumptionRegister != null)
                {
                    foreach (var tmp in command.e1RateConsumptionRegister)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.e1TimeForNextRateHours.HasValue) ret.Add(command.e1TimeForNextRateHours);
                if (command.e1TimeForNextRateMinutes.HasValue) ret.Add(command.e1TimeForNextRateMinutes);
                if (command.e1TimeForNextRateSeconds.HasValue) ret.Add(command.e1TimeForNextRateSeconds);
                if (command.properties3.HasValue) ret.Add(command.properties3);
                if (command.e2RateConsumptionRegister != null)
                {
                    foreach (var tmp in command.e2RateConsumptionRegister)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

