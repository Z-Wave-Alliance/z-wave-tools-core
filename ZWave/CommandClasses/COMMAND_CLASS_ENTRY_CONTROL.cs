using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_ENTRY_CONTROL
    {
        public const byte ID = 0x6F;
        public const byte VERSION = 1;
        public partial class ENTRY_CONTROL_NOTIFICATION
        {
            public const byte ID = 0x01;
            public ByteValue sequenceNumber = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte dataType
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
            public ByteValue eventType = 0;
            public ByteValue eventDataLength = 0;
            public IList<byte> eventData = new List<byte>();
            public static implicit operator ENTRY_CONTROL_NOTIFICATION(byte[] data)
            {
                ENTRY_CONTROL_NOTIFICATION ret = new ENTRY_CONTROL_NOTIFICATION();
                if (data != null)
                {
                    int index = 2;
                    ret.sequenceNumber = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.eventType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.eventDataLength = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.eventData = new List<byte>();
                    for (int i = 0; i < ret.eventDataLength; i++)
                    {
                        if (data.Length > index) ret.eventData.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](ENTRY_CONTROL_NOTIFICATION command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ENTRY_CONTROL.ID);
                ret.Add(ID);
                if (command.sequenceNumber.HasValue) ret.Add(command.sequenceNumber);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.eventType.HasValue) ret.Add(command.eventType);
                if (command.eventDataLength.HasValue) ret.Add(command.eventDataLength);
                if (command.eventData != null)
                {
                    foreach (var tmp in command.eventData)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class ENTRY_CONTROL_KEY_SUPPORTED_GET
        {
            public const byte ID = 0x02;
            public static implicit operator ENTRY_CONTROL_KEY_SUPPORTED_GET(byte[] data)
            {
                ENTRY_CONTROL_KEY_SUPPORTED_GET ret = new ENTRY_CONTROL_KEY_SUPPORTED_GET();
                return ret;
            }
            public static implicit operator byte[](ENTRY_CONTROL_KEY_SUPPORTED_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ENTRY_CONTROL.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class ENTRY_CONTROL_KEY_SUPPORTED_REPORT
        {
            public const byte ID = 0x03;
            public ByteValue keySupportedBitMaskLength = 0;
            public IList<byte> keySupportedBitMask = new List<byte>();
            public static implicit operator ENTRY_CONTROL_KEY_SUPPORTED_REPORT(byte[] data)
            {
                ENTRY_CONTROL_KEY_SUPPORTED_REPORT ret = new ENTRY_CONTROL_KEY_SUPPORTED_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.keySupportedBitMaskLength = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.keySupportedBitMask = new List<byte>();
                    for (int i = 0; i < ret.keySupportedBitMaskLength; i++)
                    {
                        if (data.Length > index) ret.keySupportedBitMask.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](ENTRY_CONTROL_KEY_SUPPORTED_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ENTRY_CONTROL.ID);
                ret.Add(ID);
                if (command.keySupportedBitMaskLength.HasValue) ret.Add(command.keySupportedBitMaskLength);
                if (command.keySupportedBitMask != null)
                {
                    foreach (var tmp in command.keySupportedBitMask)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class ENTRY_CONTROL_EVENT_SUPPORTED_GET
        {
            public const byte ID = 0x04;
            public static implicit operator ENTRY_CONTROL_EVENT_SUPPORTED_GET(byte[] data)
            {
                ENTRY_CONTROL_EVENT_SUPPORTED_GET ret = new ENTRY_CONTROL_EVENT_SUPPORTED_GET();
                return ret;
            }
            public static implicit operator byte[](ENTRY_CONTROL_EVENT_SUPPORTED_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ENTRY_CONTROL.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class ENTRY_CONTROL_EVENT_SUPPORTED_REPORT
        {
            public const byte ID = 0x05;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte dataTypeSupportedBitMaskLength
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
            public IList<byte> dataTypeSupportedBitMask = new List<byte>();
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte eventSupportedBitMaskLength
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte reserved2
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
            public IList<byte> eventTypeSupportedBitMask = new List<byte>();
            public ByteValue keyCachedSizeSupportedMinimum = 0;
            public ByteValue keyCachedSizeSupportedMaximum = 0;
            public ByteValue keyCachedTimeoutSupportedMinimum = 0;
            public ByteValue keyCachedTimeoutSupportedMaximum = 0;
            public static implicit operator ENTRY_CONTROL_EVENT_SUPPORTED_REPORT(byte[] data)
            {
                ENTRY_CONTROL_EVENT_SUPPORTED_REPORT ret = new ENTRY_CONTROL_EVENT_SUPPORTED_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.dataTypeSupportedBitMask = new List<byte>();
                    for (int i = 0; i < ret.properties1.dataTypeSupportedBitMaskLength; i++)
                    {
                        if (data.Length > index) ret.dataTypeSupportedBitMask.Add(data[index++]);
                    }
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.eventTypeSupportedBitMask = new List<byte>();
                    for (int i = 0; i < ret.properties2.eventSupportedBitMaskLength; i++)
                    {
                        if (data.Length > index) ret.eventTypeSupportedBitMask.Add(data[index++]);
                    }
                    ret.keyCachedSizeSupportedMinimum = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.keyCachedSizeSupportedMaximum = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.keyCachedTimeoutSupportedMinimum = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.keyCachedTimeoutSupportedMaximum = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](ENTRY_CONTROL_EVENT_SUPPORTED_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ENTRY_CONTROL.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.dataTypeSupportedBitMask != null)
                {
                    foreach (var tmp in command.dataTypeSupportedBitMask)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.eventTypeSupportedBitMask != null)
                {
                    foreach (var tmp in command.eventTypeSupportedBitMask)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.keyCachedSizeSupportedMinimum.HasValue) ret.Add(command.keyCachedSizeSupportedMinimum);
                if (command.keyCachedSizeSupportedMaximum.HasValue) ret.Add(command.keyCachedSizeSupportedMaximum);
                if (command.keyCachedTimeoutSupportedMinimum.HasValue) ret.Add(command.keyCachedTimeoutSupportedMinimum);
                if (command.keyCachedTimeoutSupportedMaximum.HasValue) ret.Add(command.keyCachedTimeoutSupportedMaximum);
                return ret.ToArray();
            }
        }
        public partial class ENTRY_CONTROL_CONFIGURATION_SET
        {
            public const byte ID = 0x06;
            public ByteValue keyCacheSize = 0;
            public ByteValue keyCacheTimeout = 0;
            public static implicit operator ENTRY_CONTROL_CONFIGURATION_SET(byte[] data)
            {
                ENTRY_CONTROL_CONFIGURATION_SET ret = new ENTRY_CONTROL_CONFIGURATION_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.keyCacheSize = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.keyCacheTimeout = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](ENTRY_CONTROL_CONFIGURATION_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ENTRY_CONTROL.ID);
                ret.Add(ID);
                if (command.keyCacheSize.HasValue) ret.Add(command.keyCacheSize);
                if (command.keyCacheTimeout.HasValue) ret.Add(command.keyCacheTimeout);
                return ret.ToArray();
            }
        }
        public partial class ENTRY_CONTROL_CONFIGURATION_GET
        {
            public const byte ID = 0x07;
            public static implicit operator ENTRY_CONTROL_CONFIGURATION_GET(byte[] data)
            {
                ENTRY_CONTROL_CONFIGURATION_GET ret = new ENTRY_CONTROL_CONFIGURATION_GET();
                return ret;
            }
            public static implicit operator byte[](ENTRY_CONTROL_CONFIGURATION_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ENTRY_CONTROL.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class ENTRY_CONTROL_CONFIGURATION_REPORT
        {
            public const byte ID = 0x08;
            public ByteValue keyCacheSize = 0;
            public ByteValue keyCacheTimeout = 0;
            public static implicit operator ENTRY_CONTROL_CONFIGURATION_REPORT(byte[] data)
            {
                ENTRY_CONTROL_CONFIGURATION_REPORT ret = new ENTRY_CONTROL_CONFIGURATION_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.keyCacheSize = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.keyCacheTimeout = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](ENTRY_CONTROL_CONFIGURATION_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ENTRY_CONTROL.ID);
                ret.Add(ID);
                if (command.keyCacheSize.HasValue) ret.Add(command.keyCacheSize);
                if (command.keyCacheTimeout.HasValue) ret.Add(command.keyCacheTimeout);
                return ret.ToArray();
            }
        }
    }
}

