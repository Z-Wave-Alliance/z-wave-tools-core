/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_DOOR_LOCK_V3
    {
        public const byte ID = 0x62;
        public const byte VERSION = 3;
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
                ret.Add(COMMAND_CLASS_DOOR_LOCK_V3.ID);
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
                }
                return ret;
            }
            public static implicit operator byte[](DOOR_LOCK_CONFIGURATION_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_DOOR_LOCK_V3.ID);
                ret.Add(ID);
                if (command.operationType.HasValue) ret.Add(command.operationType);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.lockTimeoutMinutes.HasValue) ret.Add(command.lockTimeoutMinutes);
                if (command.lockTimeoutSeconds.HasValue) ret.Add(command.lockTimeoutSeconds);
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
                }
                return ret;
            }
            public static implicit operator byte[](DOOR_LOCK_CONFIGURATION_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_DOOR_LOCK_V3.ID);
                ret.Add(ID);
                if (command.operationType.HasValue) ret.Add(command.operationType);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.lockTimeoutMinutes.HasValue) ret.Add(command.lockTimeoutMinutes);
                if (command.lockTimeoutSeconds.HasValue) ret.Add(command.lockTimeoutSeconds);
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
                ret.Add(COMMAND_CLASS_DOOR_LOCK_V3.ID);
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
                ret.Add(COMMAND_CLASS_DOOR_LOCK_V3.ID);
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
                ret.Add(COMMAND_CLASS_DOOR_LOCK_V3.ID);
                ret.Add(ID);
                if (command.doorLockMode.HasValue) ret.Add(command.doorLockMode);
                return ret.ToArray();
            }
        }
    }
}

