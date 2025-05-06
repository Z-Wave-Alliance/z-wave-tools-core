using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_NOTIFICATION_V5
    {
        public const byte ID = 0x71;
        public const byte VERSION = 5;
        public partial class NOTIFICATION_GET
        {
            public const byte ID = 0x04;
            public ByteValue v1AlarmType = 0;
            public ByteValue notificationType = 0;
            public ByteValue mevent = 0;
            public static implicit operator NOTIFICATION_GET(byte[] data)
            {
                NOTIFICATION_GET ret = new NOTIFICATION_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.v1AlarmType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.notificationType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.mevent = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](NOTIFICATION_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NOTIFICATION_V5.ID);
                ret.Add(ID);
                if (command.v1AlarmType.HasValue) ret.Add(command.v1AlarmType);
                if (command.notificationType.HasValue) ret.Add(command.notificationType);
                if (command.mevent.HasValue) ret.Add(command.mevent);
                return ret.ToArray();
            }
        }
        public partial class NOTIFICATION_REPORT
        {
            public const byte ID = 0x05;
            public ByteValue v1AlarmType = 0;
            public ByteValue v1AlarmLevel = 0;
            public ByteValue reserved = 0;
            public ByteValue notificationStatus = 0;
            public ByteValue notificationType = 0;
            public ByteValue mevent = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte eventParametersLength
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte reserved2
                {
                    get { return (byte)(_value >> 5 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x60; _value += (byte)(value << 5 & 0x60); }
                }
                public byte sequence
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
            public IList<byte> eventParameter = new List<byte>();
            public ByteValue sequenceNumber = 0;
            public static implicit operator NOTIFICATION_REPORT(byte[] data)
            {
                NOTIFICATION_REPORT ret = new NOTIFICATION_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.v1AlarmType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.v1AlarmLevel = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.reserved = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.notificationStatus = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.notificationType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.mevent = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.eventParameter = new List<byte>();
                    for (int i = 0; i < ret.properties1.eventParametersLength; i++)
                    {
                        if (data.Length > index) ret.eventParameter.Add(data[index++]);
                    }
                    ret.sequenceNumber = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](NOTIFICATION_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NOTIFICATION_V5.ID);
                ret.Add(ID);
                if (command.v1AlarmType.HasValue) ret.Add(command.v1AlarmType);
                if (command.v1AlarmLevel.HasValue) ret.Add(command.v1AlarmLevel);
                if (command.reserved.HasValue) ret.Add(command.reserved);
                if (command.notificationStatus.HasValue) ret.Add(command.notificationStatus);
                if (command.notificationType.HasValue) ret.Add(command.notificationType);
                if (command.mevent.HasValue) ret.Add(command.mevent);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.eventParameter != null)
                {
                    foreach (var tmp in command.eventParameter)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.sequenceNumber.HasValue) ret.Add(command.sequenceNumber);
                return ret.ToArray();
            }
        }
        public partial class NOTIFICATION_SET
        {
            public const byte ID = 0x06;
            public ByteValue notificationType = 0;
            public ByteValue notificationStatus = 0;
            public static implicit operator NOTIFICATION_SET(byte[] data)
            {
                NOTIFICATION_SET ret = new NOTIFICATION_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.notificationType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.notificationStatus = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](NOTIFICATION_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NOTIFICATION_V5.ID);
                ret.Add(ID);
                if (command.notificationType.HasValue) ret.Add(command.notificationType);
                if (command.notificationStatus.HasValue) ret.Add(command.notificationStatus);
                return ret.ToArray();
            }
        }
        public partial class NOTIFICATION_SUPPORTED_GET
        {
            public const byte ID = 0x07;
            public static implicit operator NOTIFICATION_SUPPORTED_GET(byte[] data)
            {
                NOTIFICATION_SUPPORTED_GET ret = new NOTIFICATION_SUPPORTED_GET();
                return ret;
            }
            public static implicit operator byte[](NOTIFICATION_SUPPORTED_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NOTIFICATION_V5.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class NOTIFICATION_SUPPORTED_REPORT
        {
            public const byte ID = 0x08;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte numberOfBitMasks
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 5 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x60; _value += (byte)(value << 5 & 0x60); }
                }
                public byte v1Alarm
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
            public IList<byte> bitMask = new List<byte>();
            public static implicit operator NOTIFICATION_SUPPORTED_REPORT(byte[] data)
            {
                NOTIFICATION_SUPPORTED_REPORT ret = new NOTIFICATION_SUPPORTED_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.bitMask = new List<byte>();
                    for (int i = 0; i < ret.properties1.numberOfBitMasks; i++)
                    {
                        if (data.Length > index) ret.bitMask.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](NOTIFICATION_SUPPORTED_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NOTIFICATION_V5.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.bitMask != null)
                {
                    foreach (var tmp in command.bitMask)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class EVENT_SUPPORTED_GET
        {
            public const byte ID = 0x01;
            public ByteValue notificationType = 0;
            public static implicit operator EVENT_SUPPORTED_GET(byte[] data)
            {
                EVENT_SUPPORTED_GET ret = new EVENT_SUPPORTED_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.notificationType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](EVENT_SUPPORTED_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NOTIFICATION_V5.ID);
                ret.Add(ID);
                if (command.notificationType.HasValue) ret.Add(command.notificationType);
                return ret.ToArray();
            }
        }
        public partial class EVENT_SUPPORTED_REPORT
        {
            public const byte ID = 0x02;
            public ByteValue notificationType = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte numberOfBitMasks
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
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
            public IList<byte> bitMask = new List<byte>();
            public static implicit operator EVENT_SUPPORTED_REPORT(byte[] data)
            {
                EVENT_SUPPORTED_REPORT ret = new EVENT_SUPPORTED_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.notificationType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.bitMask = new List<byte>();
                    for (int i = 0; i < ret.properties1.numberOfBitMasks; i++)
                    {
                        if (data.Length > index) ret.bitMask.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](EVENT_SUPPORTED_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_NOTIFICATION_V5.ID);
                ret.Add(ID);
                if (command.notificationType.HasValue) ret.Add(command.notificationType);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.bitMask != null)
                {
                    foreach (var tmp in command.bitMask)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

