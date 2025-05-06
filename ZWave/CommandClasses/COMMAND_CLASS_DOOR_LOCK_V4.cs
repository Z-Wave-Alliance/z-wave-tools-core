using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_DOOR_LOCK_V4
    {
        public const byte ID = 0x62;
        public const byte VERSION = 4;
        public partial class DOOR_LOCK_CONFIGURATION_GET
        {
            public const byte ID = 0x05;
            public static implicit operator DOOR_LOCK_CONFIGURATION_GET(byte[] data)
            {
                DOOR_LOCK_CONFIGURATION_GET ret = new DOOR_LOCK_CONFIGURATION_GET();
                return ret;
            }
            public static implicit operator byte[](DOOR_LOCK_CONFIGURATION_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_DOOR_LOCK_V4.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class DOOR_LOCK_CONFIGURATION_REPORT
        {
            public const byte ID = 0x06;
            public ByteValue operationType = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte insideDoorHandlesEnabled
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte outsideDoorHandlesEnabled
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
            public ByteValue lockTimeoutMinutes = 0;
            public ByteValue lockTimeoutSeconds = 0;
            public const byte autoRelockTimeBytesCount = 2;
            public byte[] autoRelockTime = new byte[autoRelockTimeBytesCount];
            public const byte holdAndReleaseTimeBytesCount = 2;
            public byte[] holdAndReleaseTime = new byte[holdAndReleaseTimeBytesCount];
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte ta
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte btb
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
            public static implicit operator DOOR_LOCK_CONFIGURATION_REPORT(byte[] data)
            {
                DOOR_LOCK_CONFIGURATION_REPORT ret = new DOOR_LOCK_CONFIGURATION_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.operationType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.lockTimeoutMinutes = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.lockTimeoutSeconds = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.autoRelockTime = (data.Length - index) >= autoRelockTimeBytesCount ? new byte[autoRelockTimeBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.autoRelockTime[0] = data[index++];
                    if (data.Length > index) ret.autoRelockTime[1] = data[index++];
                    ret.holdAndReleaseTime = (data.Length - index) >= holdAndReleaseTimeBytesCount ? new byte[holdAndReleaseTimeBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.holdAndReleaseTime[0] = data[index++];
                    if (data.Length > index) ret.holdAndReleaseTime[1] = data[index++];
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](DOOR_LOCK_CONFIGURATION_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_DOOR_LOCK_V4.ID);
                ret.Add(ID);
                if (command.operationType.HasValue) ret.Add(command.operationType);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.lockTimeoutMinutes.HasValue) ret.Add(command.lockTimeoutMinutes);
                if (command.lockTimeoutSeconds.HasValue) ret.Add(command.lockTimeoutSeconds);
                if (command.autoRelockTime != null)
                {
                    foreach (var tmp in command.autoRelockTime)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.holdAndReleaseTime != null)
                {
                    foreach (var tmp in command.holdAndReleaseTime)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties2.HasValue) ret.Add(command.properties2);
                return ret.ToArray();
            }
        }
        public partial class DOOR_LOCK_CONFIGURATION_SET
        {
            public const byte ID = 0x04;
            public ByteValue operationType = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte insideDoorHandlesEnabled
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte outsideDoorHandlesEnabled
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
            public ByteValue lockTimeoutMinutes = 0;
            public ByteValue lockTimeoutSeconds = 0;
            public const byte autoRelockTimeBytesCount = 2;
            public byte[] autoRelockTime = new byte[autoRelockTimeBytesCount];
            public const byte holdAndReleaseTimeBytesCount = 2;
            public byte[] holdAndReleaseTime = new byte[holdAndReleaseTimeBytesCount];
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte ta
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte btb
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
            public static implicit operator DOOR_LOCK_CONFIGURATION_SET(byte[] data)
            {
                DOOR_LOCK_CONFIGURATION_SET ret = new DOOR_LOCK_CONFIGURATION_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.operationType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.lockTimeoutMinutes = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.lockTimeoutSeconds = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.autoRelockTime = (data.Length - index) >= autoRelockTimeBytesCount ? new byte[autoRelockTimeBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.autoRelockTime[0] = data[index++];
                    if (data.Length > index) ret.autoRelockTime[1] = data[index++];
                    ret.holdAndReleaseTime = (data.Length - index) >= holdAndReleaseTimeBytesCount ? new byte[holdAndReleaseTimeBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.holdAndReleaseTime[0] = data[index++];
                    if (data.Length > index) ret.holdAndReleaseTime[1] = data[index++];
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](DOOR_LOCK_CONFIGURATION_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_DOOR_LOCK_V4.ID);
                ret.Add(ID);
                if (command.operationType.HasValue) ret.Add(command.operationType);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.lockTimeoutMinutes.HasValue) ret.Add(command.lockTimeoutMinutes);
                if (command.lockTimeoutSeconds.HasValue) ret.Add(command.lockTimeoutSeconds);
                if (command.autoRelockTime != null)
                {
                    foreach (var tmp in command.autoRelockTime)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.holdAndReleaseTime != null)
                {
                    foreach (var tmp in command.holdAndReleaseTime)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties2.HasValue) ret.Add(command.properties2);
                return ret.ToArray();
            }
        }
        public partial class DOOR_LOCK_OPERATION_GET
        {
            public const byte ID = 0x02;
            public static implicit operator DOOR_LOCK_OPERATION_GET(byte[] data)
            {
                DOOR_LOCK_OPERATION_GET ret = new DOOR_LOCK_OPERATION_GET();
                return ret;
            }
            public static implicit operator byte[](DOOR_LOCK_OPERATION_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_DOOR_LOCK_V4.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class DOOR_LOCK_OPERATION_REPORT
        {
            public const byte ID = 0x03;
            public ByteValue currentDoorLockMode = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte insideDoorHandlesMode
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte outsideDoorHandlesMode
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
            public ByteValue doorCondition = 0;
            public ByteValue remainingLockTimeMinutes = 0;
            public ByteValue remainingLockTimeSeconds = 0;
            public ByteValue targetDoorLockMode = 0;
            public ByteValue duration = 0;
            public static implicit operator DOOR_LOCK_OPERATION_REPORT(byte[] data)
            {
                DOOR_LOCK_OPERATION_REPORT ret = new DOOR_LOCK_OPERATION_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.currentDoorLockMode = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.doorCondition = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.remainingLockTimeMinutes = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.remainingLockTimeSeconds = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.targetDoorLockMode = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.duration = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](DOOR_LOCK_OPERATION_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_DOOR_LOCK_V4.ID);
                ret.Add(ID);
                if (command.currentDoorLockMode.HasValue) ret.Add(command.currentDoorLockMode);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.doorCondition.HasValue) ret.Add(command.doorCondition);
                if (command.remainingLockTimeMinutes.HasValue) ret.Add(command.remainingLockTimeMinutes);
                if (command.remainingLockTimeSeconds.HasValue) ret.Add(command.remainingLockTimeSeconds);
                if (command.targetDoorLockMode.HasValue) ret.Add(command.targetDoorLockMode);
                if (command.duration.HasValue) ret.Add(command.duration);
                return ret.ToArray();
            }
        }
        public partial class DOOR_LOCK_OPERATION_SET
        {
            public const byte ID = 0x01;
            public ByteValue doorLockMode = 0;
            public static implicit operator DOOR_LOCK_OPERATION_SET(byte[] data)
            {
                DOOR_LOCK_OPERATION_SET ret = new DOOR_LOCK_OPERATION_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.doorLockMode = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](DOOR_LOCK_OPERATION_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_DOOR_LOCK_V4.ID);
                ret.Add(ID);
                if (command.doorLockMode.HasValue) ret.Add(command.doorLockMode);
                return ret.ToArray();
            }
        }
        public partial class DOOR_LOCK_CAPABILITIES_GET
        {
            public const byte ID = 0x07;
            public static implicit operator DOOR_LOCK_CAPABILITIES_GET(byte[] data)
            {
                DOOR_LOCK_CAPABILITIES_GET ret = new DOOR_LOCK_CAPABILITIES_GET();
                return ret;
            }
            public static implicit operator byte[](DOOR_LOCK_CAPABILITIES_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_DOOR_LOCK_V4.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class DOOR_LOCK_CAPABILITIES_REPORT
        {
            public const byte ID = 0x08;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte supportedOperationTypeBitMaskLength
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
            public IList<byte> supportedOperationTypeBitMask = new List<byte>();
            public ByteValue supportedDoorLockModeListLength = 0;
            public IList<byte> supportedDoorLockMode = new List<byte>();
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte supportedInsideHandleModesBitmask
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte supportedOutsideHandleModesBitmask
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
            public ByteValue supportedDoorComponents = 0;
            public struct Tproperties3
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties3 Empty { get { return new Tproperties3() { _value = 0, HasValue = false }; } }
                public byte btbs
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte tas
                {
                    get { return (byte)(_value >> 1 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x02; _value += (byte)(value << 1 & 0x02); }
                }
                public byte hrs
                {
                    get { return (byte)(_value >> 2 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x04; _value += (byte)(value << 2 & 0x04); }
                }
                public byte ars
                {
                    get { return (byte)(_value >> 3 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x08; _value += (byte)(value << 3 & 0x08); }
                }
                public byte reserved
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
            public static implicit operator DOOR_LOCK_CAPABILITIES_REPORT(byte[] data)
            {
                DOOR_LOCK_CAPABILITIES_REPORT ret = new DOOR_LOCK_CAPABILITIES_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.supportedOperationTypeBitMask = new List<byte>();
                    for (int i = 0; i < ret.properties1.supportedOperationTypeBitMaskLength; i++)
                    {
                        if (data.Length > index) ret.supportedOperationTypeBitMask.Add(data[index++]);
                    }
                    ret.supportedDoorLockModeListLength = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.supportedDoorLockMode = new List<byte>();
                    for (int i = 0; i < ret.supportedDoorLockModeListLength; i++)
                    {
                        if (data.Length > index) ret.supportedDoorLockMode.Add(data[index++]);
                    }
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.supportedDoorComponents = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties3 = data.Length > index ? (Tproperties3)data[index++] : Tproperties3.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](DOOR_LOCK_CAPABILITIES_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_DOOR_LOCK_V4.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.supportedOperationTypeBitMask != null)
                {
                    foreach (var tmp in command.supportedOperationTypeBitMask)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.supportedDoorLockModeListLength.HasValue) ret.Add(command.supportedDoorLockModeListLength);
                if (command.supportedDoorLockMode != null)
                {
                    foreach (var tmp in command.supportedDoorLockMode)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.supportedDoorComponents.HasValue) ret.Add(command.supportedDoorComponents);
                if (command.properties3.HasValue) ret.Add(command.properties3);
                return ret.ToArray();
            }
        }
    }
}

