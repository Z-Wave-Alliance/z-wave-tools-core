using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_THERMOSTAT_HEATING
    {
        public const byte ID = 0x38;
        public const byte VERSION = 1;
        public partial class THERMOSTAT_HEATING_STATUS_REPORT
        {
            public const byte ID = 0x0D;
            public ByteValue status = 0;
            public static implicit operator THERMOSTAT_HEATING_STATUS_REPORT(byte[] data)
            {
                THERMOSTAT_HEATING_STATUS_REPORT ret = new THERMOSTAT_HEATING_STATUS_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.status = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](THERMOSTAT_HEATING_STATUS_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_THERMOSTAT_HEATING.ID);
                ret.Add(ID);
                if (command.status.HasValue) ret.Add(command.status);
                return ret.ToArray();
            }
        }
        public partial class THERMOSTAT_HEATING_MODE_GET
        {
            public const byte ID = 0x02;
            public static implicit operator THERMOSTAT_HEATING_MODE_GET(byte[] data)
            {
                THERMOSTAT_HEATING_MODE_GET ret = new THERMOSTAT_HEATING_MODE_GET();
                return ret;
            }
            public static implicit operator byte[](THERMOSTAT_HEATING_MODE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_THERMOSTAT_HEATING.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class THERMOSTAT_HEATING_MODE_REPORT
        {
            public const byte ID = 0x03;
            public ByteValue mode = 0;
            public static implicit operator THERMOSTAT_HEATING_MODE_REPORT(byte[] data)
            {
                THERMOSTAT_HEATING_MODE_REPORT ret = new THERMOSTAT_HEATING_MODE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.mode = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](THERMOSTAT_HEATING_MODE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_THERMOSTAT_HEATING.ID);
                ret.Add(ID);
                if (command.mode.HasValue) ret.Add(command.mode);
                return ret.ToArray();
            }
        }
        public partial class THERMOSTAT_HEATING_MODE_SET
        {
            public const byte ID = 0x01;
            public ByteValue mode = 0;
            public static implicit operator THERMOSTAT_HEATING_MODE_SET(byte[] data)
            {
                THERMOSTAT_HEATING_MODE_SET ret = new THERMOSTAT_HEATING_MODE_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.mode = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](THERMOSTAT_HEATING_MODE_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_THERMOSTAT_HEATING.ID);
                ret.Add(ID);
                if (command.mode.HasValue) ret.Add(command.mode);
                return ret.ToArray();
            }
        }
        public partial class THERMOSTAT_HEATING_RELAY_STATUS_GET
        {
            public const byte ID = 0x09;
            public static implicit operator THERMOSTAT_HEATING_RELAY_STATUS_GET(byte[] data)
            {
                THERMOSTAT_HEATING_RELAY_STATUS_GET ret = new THERMOSTAT_HEATING_RELAY_STATUS_GET();
                return ret;
            }
            public static implicit operator byte[](THERMOSTAT_HEATING_RELAY_STATUS_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_THERMOSTAT_HEATING.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class THERMOSTAT_HEATING_RELAY_STATUS_REPORT
        {
            public const byte ID = 0x0A;
            public ByteValue relayStatus = 0;
            public static implicit operator THERMOSTAT_HEATING_RELAY_STATUS_REPORT(byte[] data)
            {
                THERMOSTAT_HEATING_RELAY_STATUS_REPORT ret = new THERMOSTAT_HEATING_RELAY_STATUS_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.relayStatus = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](THERMOSTAT_HEATING_RELAY_STATUS_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_THERMOSTAT_HEATING.ID);
                ret.Add(ID);
                if (command.relayStatus.HasValue) ret.Add(command.relayStatus);
                return ret.ToArray();
            }
        }
        public partial class THERMOSTAT_HEATING_SETPOINT_GET
        {
            public const byte ID = 0x05;
            public ByteValue setpointNr = 0;
            public static implicit operator THERMOSTAT_HEATING_SETPOINT_GET(byte[] data)
            {
                THERMOSTAT_HEATING_SETPOINT_GET ret = new THERMOSTAT_HEATING_SETPOINT_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.setpointNr = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](THERMOSTAT_HEATING_SETPOINT_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_THERMOSTAT_HEATING.ID);
                ret.Add(ID);
                if (command.setpointNr.HasValue) ret.Add(command.setpointNr);
                return ret.ToArray();
            }
        }
        public partial class THERMOSTAT_HEATING_SETPOINT_REPORT
        {
            public const byte ID = 0x06;
            public ByteValue setpointNr = 0;
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
            public IList<byte> value = new List<byte>();
            public static implicit operator THERMOSTAT_HEATING_SETPOINT_REPORT(byte[] data)
            {
                THERMOSTAT_HEATING_SETPOINT_REPORT ret = new THERMOSTAT_HEATING_SETPOINT_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.setpointNr = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.value = new List<byte>();
                    for (int i = 0; i < ret.properties1.size; i++)
                    {
                        if (data.Length > index) ret.value.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](THERMOSTAT_HEATING_SETPOINT_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_THERMOSTAT_HEATING.ID);
                ret.Add(ID);
                if (command.setpointNr.HasValue) ret.Add(command.setpointNr);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.value != null)
                {
                    foreach (var tmp in command.value)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class THERMOSTAT_HEATING_SETPOINT_SET
        {
            public const byte ID = 0x04;
            public ByteValue setpointNr = 0;
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
            public IList<byte> value = new List<byte>();
            public static implicit operator THERMOSTAT_HEATING_SETPOINT_SET(byte[] data)
            {
                THERMOSTAT_HEATING_SETPOINT_SET ret = new THERMOSTAT_HEATING_SETPOINT_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.setpointNr = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.value = new List<byte>();
                    for (int i = 0; i < ret.properties1.size; i++)
                    {
                        if (data.Length > index) ret.value.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](THERMOSTAT_HEATING_SETPOINT_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_THERMOSTAT_HEATING.ID);
                ret.Add(ID);
                if (command.setpointNr.HasValue) ret.Add(command.setpointNr);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.value != null)
                {
                    foreach (var tmp in command.value)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class THERMOSTAT_HEATING_STATUS_GET
        {
            public const byte ID = 0x0C;
            public static implicit operator THERMOSTAT_HEATING_STATUS_GET(byte[] data)
            {
                THERMOSTAT_HEATING_STATUS_GET ret = new THERMOSTAT_HEATING_STATUS_GET();
                return ret;
            }
            public static implicit operator byte[](THERMOSTAT_HEATING_STATUS_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_THERMOSTAT_HEATING.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class THERMOSTAT_HEATING_STATUS_SET
        {
            public const byte ID = 0x0B;
            public ByteValue status = 0;
            public static implicit operator THERMOSTAT_HEATING_STATUS_SET(byte[] data)
            {
                THERMOSTAT_HEATING_STATUS_SET ret = new THERMOSTAT_HEATING_STATUS_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.status = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](THERMOSTAT_HEATING_STATUS_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_THERMOSTAT_HEATING.ID);
                ret.Add(ID);
                if (command.status.HasValue) ret.Add(command.status);
                return ret.ToArray();
            }
        }
        public partial class THERMOSTAT_HEATING_TIMED_OFF_SET
        {
            public const byte ID = 0x11;
            public ByteValue minutes = 0;
            public ByteValue hours = 0;
            public static implicit operator THERMOSTAT_HEATING_TIMED_OFF_SET(byte[] data)
            {
                THERMOSTAT_HEATING_TIMED_OFF_SET ret = new THERMOSTAT_HEATING_TIMED_OFF_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.minutes = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.hours = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](THERMOSTAT_HEATING_TIMED_OFF_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_THERMOSTAT_HEATING.ID);
                ret.Add(ID);
                if (command.minutes.HasValue) ret.Add(command.minutes);
                if (command.hours.HasValue) ret.Add(command.hours);
                return ret.ToArray();
            }
        }
    }
}

