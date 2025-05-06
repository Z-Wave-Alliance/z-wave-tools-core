using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_BATTERY_V3
    {
        public const byte ID = 0x80;
        public const byte VERSION = 3;
        public partial class BATTERY_GET
        {
            public const byte ID = 0x02;
            public static implicit operator BATTERY_GET(byte[] data)
            {
                BATTERY_GET ret = new BATTERY_GET();
                return ret;
            }
            public static implicit operator byte[](BATTERY_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_BATTERY_V3.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class BATTERY_REPORT
        {
            public const byte ID = 0x03;
            public ByteValue batteryLevel = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte replaceRecharge
                {
                    get { return (byte)(_value >> 0 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x03; _value += (byte)(value << 0 & 0x03); }
                }
                public byte lowFluid
                {
                    get { return (byte)(_value >> 2 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x04; _value += (byte)(value << 2 & 0x04); }
                }
                public byte overheating
                {
                    get { return (byte)(_value >> 3 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x08; _value += (byte)(value << 3 & 0x08); }
                }
                public byte backupBattery
                {
                    get { return (byte)(_value >> 4 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x10; _value += (byte)(value << 4 & 0x10); }
                }
                public byte rechargeable
                {
                    get { return (byte)(_value >> 5 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x20; _value += (byte)(value << 5 & 0x20); }
                }
                public byte chargingStatus
                {
                    get { return (byte)(_value >> 6 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0xC0; _value += (byte)(value << 6 & 0xC0); }
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
                public byte disconnected
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte lowTemperatureStatus
                {
                    get { return (byte)(_value >> 1 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x02; _value += (byte)(value << 1 & 0x02); }
                }
                public byte reserved1
                {
                    get { return (byte)(_value >> 2 & 0x3F); }
                    set { HasValue = true; _value &= 0xFF - 0xFC; _value += (byte)(value << 2 & 0xFC); }
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
            public static implicit operator BATTERY_REPORT(byte[] data)
            {
                BATTERY_REPORT ret = new BATTERY_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.batteryLevel = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](BATTERY_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_BATTERY_V3.ID);
                ret.Add(ID);
                if (command.batteryLevel.HasValue) ret.Add(command.batteryLevel);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                return ret.ToArray();
            }
        }
        public partial class BATTERY_HEALTH_GET
        {
            public const byte ID = 0x04;
            public static implicit operator BATTERY_HEALTH_GET(byte[] data)
            {
                BATTERY_HEALTH_GET ret = new BATTERY_HEALTH_GET();
                return ret;
            }
            public static implicit operator byte[](BATTERY_HEALTH_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_BATTERY_V3.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class BATTERY_HEALTH_REPORT
        {
            public const byte ID = 0x05;
            public ByteValue maximumCapacity = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte size
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte scale
                {
                    get { return (byte)(_value >> 3 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x18; _value += (byte)(value << 3 & 0x18); }
                }
                public byte precision
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
            public IList<byte> batteryTemperature = new List<byte>();
            public static implicit operator BATTERY_HEALTH_REPORT(byte[] data)
            {
                BATTERY_HEALTH_REPORT ret = new BATTERY_HEALTH_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.maximumCapacity = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.batteryTemperature = new List<byte>();
                    for (int i = 0; i < ret.properties1.size; i++)
                    {
                        if (data.Length > index) ret.batteryTemperature.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](BATTERY_HEALTH_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_BATTERY_V3.ID);
                ret.Add(ID);
                if (command.maximumCapacity.HasValue) ret.Add(command.maximumCapacity);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.batteryTemperature != null)
                {
                    foreach (var tmp in command.batteryTemperature)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

