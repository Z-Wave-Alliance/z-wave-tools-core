/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_IRRIGATION
    {
        public const byte ID = 0x6B;
        public const byte VERSION = 1;
        public partial class IRRIGATION_SYSTEM_INFO_GET
        {
            public const byte ID = 0x01;
            public static implicit operator IRRIGATION_SYSTEM_INFO_GET(byte[] data)
            {
                IRRIGATION_SYSTEM_INFO_GET ret = new IRRIGATION_SYSTEM_INFO_GET();
                return ret;
            }
            public static implicit operator byte[](IRRIGATION_SYSTEM_INFO_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IRRIGATION.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class IRRIGATION_SYSTEM_INFO_REPORT
        {
            public const byte ID = 0x02;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte mainValve
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte reserved1
                {
                    get { return (byte)(_value >> 1 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x06; _value += (byte)(value << 1 & 0x06); }
                }
                public byte reserved2
                {
                    get { return (byte)(_value >> 3 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x18; _value += (byte)(value << 3 & 0x18); }
                }
                public byte reserved3
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
            public ByteValue totalNumberOfValves = 0;
            public ByteValue totalNumberOfValveTables = 0;
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte valveTableMaxSize
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte reserved
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
            public static implicit operator IRRIGATION_SYSTEM_INFO_REPORT(byte[] data)
            {
                IRRIGATION_SYSTEM_INFO_REPORT ret = new IRRIGATION_SYSTEM_INFO_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.totalNumberOfValves = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.totalNumberOfValveTables = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](IRRIGATION_SYSTEM_INFO_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IRRIGATION.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.totalNumberOfValves.HasValue) ret.Add(command.totalNumberOfValves);
                if (command.totalNumberOfValveTables.HasValue) ret.Add(command.totalNumberOfValveTables);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                return ret.ToArray();
            }
        }
        public partial class IRRIGATION_SYSTEM_STATUS_GET
        {
            public const byte ID = 0x03;
            public static implicit operator IRRIGATION_SYSTEM_STATUS_GET(byte[] data)
            {
                IRRIGATION_SYSTEM_STATUS_GET ret = new IRRIGATION_SYSTEM_STATUS_GET();
                return ret;
            }
            public static implicit operator byte[](IRRIGATION_SYSTEM_STATUS_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IRRIGATION.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class IRRIGATION_SYSTEM_STATUS_REPORT
        {
            public const byte ID = 0x04;
            public ByteValue systemVoltage = 0;
            public ByteValue sensorStatus = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte flowSize
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte flowScale
                {
                    get { return (byte)(_value >> 3 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x18; _value += (byte)(value << 3 & 0x18); }
                }
                public byte flowPrecision
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
            public IList<byte> flowValue = new List<byte>();
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte pressureSize
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte pressureScale
                {
                    get { return (byte)(_value >> 3 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x18; _value += (byte)(value << 3 & 0x18); }
                }
                public byte pressurePrecision
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
            public IList<byte> pressureValue = new List<byte>();
            public ByteValue shutoffDuration = 0;
            public ByteValue systemErrorStatus = 0;
            public struct Tproperties3
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties3 Empty { get { return new Tproperties3() { _value = 0, HasValue = false }; } }
                public byte mainValve
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 1 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0xFE; _value += (byte)(value << 1 & 0xFE); }
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
            public ByteValue valveId = 0;
            public static implicit operator IRRIGATION_SYSTEM_STATUS_REPORT(byte[] data)
            {
                IRRIGATION_SYSTEM_STATUS_REPORT ret = new IRRIGATION_SYSTEM_STATUS_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.systemVoltage = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.sensorStatus = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.flowValue = new List<byte>();
                    for (int i = 0; i < ret.properties1.flowSize; i++)
                    {
                        if (data.Length > index) ret.flowValue.Add(data[index++]);
                    }
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.pressureValue = new List<byte>();
                    for (int i = 0; i < ret.properties2.pressureSize; i++)
                    {
                        if (data.Length > index) ret.pressureValue.Add(data[index++]);
                    }
                    ret.shutoffDuration = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.systemErrorStatus = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties3 = data.Length > index ? (Tproperties3)data[index++] : Tproperties3.Empty;
                    ret.valveId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](IRRIGATION_SYSTEM_STATUS_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IRRIGATION.ID);
                ret.Add(ID);
                if (command.systemVoltage.HasValue) ret.Add(command.systemVoltage);
                if (command.sensorStatus.HasValue) ret.Add(command.sensorStatus);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.flowValue != null)
                {
                    foreach (var tmp in command.flowValue)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.pressureValue != null)
                {
                    foreach (var tmp in command.pressureValue)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.shutoffDuration.HasValue) ret.Add(command.shutoffDuration);
                if (command.systemErrorStatus.HasValue) ret.Add(command.systemErrorStatus);
                if (command.properties3.HasValue) ret.Add(command.properties3);
                if (command.valveId.HasValue) ret.Add(command.valveId);
                return ret.ToArray();
            }
        }
        public partial class IRRIGATION_SYSTEM_CONFIG_SET
        {
            public const byte ID = 0x05;
            public ByteValue mainValveDelay = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte highPressureThresholdSize
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte highPressureThresholdScale
                {
                    get { return (byte)(_value >> 3 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x18; _value += (byte)(value << 3 & 0x18); }
                }
                public byte highPressureThresholdPrecision
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
            public IList<byte> highPressureThresholdValue = new List<byte>();
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte lowPressureThresholdSize
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte lowPressureThresholdScale
                {
                    get { return (byte)(_value >> 3 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x18; _value += (byte)(value << 3 & 0x18); }
                }
                public byte lowPressureThresholdPrecision
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
            public IList<byte> lowPressureThresholdValue = new List<byte>();
            public ByteValue sensorPolarity = 0;
            public static implicit operator IRRIGATION_SYSTEM_CONFIG_SET(byte[] data)
            {
                IRRIGATION_SYSTEM_CONFIG_SET ret = new IRRIGATION_SYSTEM_CONFIG_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.mainValveDelay = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.highPressureThresholdValue = new List<byte>();
                    for (int i = 0; i < ret.properties1.highPressureThresholdSize; i++)
                    {
                        if (data.Length > index) ret.highPressureThresholdValue.Add(data[index++]);
                    }
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.lowPressureThresholdValue = new List<byte>();
                    for (int i = 0; i < ret.properties2.lowPressureThresholdSize; i++)
                    {
                        if (data.Length > index) ret.lowPressureThresholdValue.Add(data[index++]);
                    }
                    ret.sensorPolarity = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](IRRIGATION_SYSTEM_CONFIG_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IRRIGATION.ID);
                ret.Add(ID);
                if (command.mainValveDelay.HasValue) ret.Add(command.mainValveDelay);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.highPressureThresholdValue != null)
                {
                    foreach (var tmp in command.highPressureThresholdValue)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.lowPressureThresholdValue != null)
                {
                    foreach (var tmp in command.lowPressureThresholdValue)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.sensorPolarity.HasValue) ret.Add(command.sensorPolarity);
                return ret.ToArray();
            }
        }
        public partial class IRRIGATION_SYSTEM_CONFIG_GET
        {
            public const byte ID = 0x06;
            public static implicit operator IRRIGATION_SYSTEM_CONFIG_GET(byte[] data)
            {
                IRRIGATION_SYSTEM_CONFIG_GET ret = new IRRIGATION_SYSTEM_CONFIG_GET();
                return ret;
            }
            public static implicit operator byte[](IRRIGATION_SYSTEM_CONFIG_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IRRIGATION.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class IRRIGATION_SYSTEM_CONFIG_REPORT
        {
            public const byte ID = 0x07;
            public ByteValue mainValveDelay = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte highPressureThresholdSize
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte highPressureThresholdScale
                {
                    get { return (byte)(_value >> 3 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x18; _value += (byte)(value << 3 & 0x18); }
                }
                public byte highPressureThresholdPrecision
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
            public IList<byte> highPressureThresholdValue = new List<byte>();
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte lowPressureThresholdSize
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte lowPressureThresholdScale
                {
                    get { return (byte)(_value >> 3 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x18; _value += (byte)(value << 3 & 0x18); }
                }
                public byte lowPressureThresholdPrecision
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
            public IList<byte> lowPressureThresholdValue = new List<byte>();
            public ByteValue sensorPolarity = 0;
            public static implicit operator IRRIGATION_SYSTEM_CONFIG_REPORT(byte[] data)
            {
                IRRIGATION_SYSTEM_CONFIG_REPORT ret = new IRRIGATION_SYSTEM_CONFIG_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.mainValveDelay = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.highPressureThresholdValue = new List<byte>();
                    for (int i = 0; i < ret.properties1.highPressureThresholdSize; i++)
                    {
                        if (data.Length > index) ret.highPressureThresholdValue.Add(data[index++]);
                    }
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.lowPressureThresholdValue = new List<byte>();
                    for (int i = 0; i < ret.properties2.lowPressureThresholdSize; i++)
                    {
                        if (data.Length > index) ret.lowPressureThresholdValue.Add(data[index++]);
                    }
                    ret.sensorPolarity = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](IRRIGATION_SYSTEM_CONFIG_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IRRIGATION.ID);
                ret.Add(ID);
                if (command.mainValveDelay.HasValue) ret.Add(command.mainValveDelay);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.highPressureThresholdValue != null)
                {
                    foreach (var tmp in command.highPressureThresholdValue)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.lowPressureThresholdValue != null)
                {
                    foreach (var tmp in command.lowPressureThresholdValue)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.sensorPolarity.HasValue) ret.Add(command.sensorPolarity);
                return ret.ToArray();
            }
        }
        public partial class IRRIGATION_VALVE_INFO_GET
        {
            public const byte ID = 0x08;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte mainValve
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 1 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0xFE; _value += (byte)(value << 1 & 0xFE); }
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
            public ByteValue valveId = 0;
            public static implicit operator IRRIGATION_VALVE_INFO_GET(byte[] data)
            {
                IRRIGATION_VALVE_INFO_GET ret = new IRRIGATION_VALVE_INFO_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.valveId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](IRRIGATION_VALVE_INFO_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IRRIGATION.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.valveId.HasValue) ret.Add(command.valveId);
                return ret.ToArray();
            }
        }
        public partial class IRRIGATION_VALVE_INFO_REPORT
        {
            public const byte ID = 0x09;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte main
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte connected
                {
                    get { return (byte)(_value >> 1 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x02; _value += (byte)(value << 1 & 0x02); }
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
            public ByteValue valveId = 0;
            public ByteValue nominalCurrent = 0;
            public ByteValue valveErrorStatus = 0;
            public static implicit operator IRRIGATION_VALVE_INFO_REPORT(byte[] data)
            {
                IRRIGATION_VALVE_INFO_REPORT ret = new IRRIGATION_VALVE_INFO_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.valveId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nominalCurrent = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.valveErrorStatus = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](IRRIGATION_VALVE_INFO_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IRRIGATION.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.valveId.HasValue) ret.Add(command.valveId);
                if (command.nominalCurrent.HasValue) ret.Add(command.nominalCurrent);
                if (command.valveErrorStatus.HasValue) ret.Add(command.valveErrorStatus);
                return ret.ToArray();
            }
        }
        public partial class IRRIGATION_VALVE_CONFIG_SET
        {
            public const byte ID = 0x0A;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte mainValve
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 1 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0xFE; _value += (byte)(value << 1 & 0xFE); }
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
            public ByteValue valveId = 0;
            public ByteValue nominalCurrentHighThreshold = 0;
            public ByteValue nominalCurrentLowThreshold = 0;
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte maximumFlowSize
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte maximumFlowScale
                {
                    get { return (byte)(_value >> 3 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x18; _value += (byte)(value << 3 & 0x18); }
                }
                public byte maximumFlowPrecision
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
            public IList<byte> maximumFlowValue = new List<byte>();
            public struct Tproperties3
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties3 Empty { get { return new Tproperties3() { _value = 0, HasValue = false }; } }
                public byte flowHighThresholdSize
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte flowHighThresholdScale
                {
                    get { return (byte)(_value >> 3 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x18; _value += (byte)(value << 3 & 0x18); }
                }
                public byte flowHighThresholdPrecision
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
            public IList<byte> flowHighThresholdValue = new List<byte>();
            public struct Tproperties4
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties4 Empty { get { return new Tproperties4() { _value = 0, HasValue = false }; } }
                public byte flowLowThresholdSize
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte flowLowThresholdScale
                {
                    get { return (byte)(_value >> 3 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x18; _value += (byte)(value << 3 & 0x18); }
                }
                public byte flowLowThresholdPrecision
                {
                    get { return (byte)(_value >> 5 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0xE0; _value += (byte)(value << 5 & 0xE0); }
                }
                public static implicit operator Tproperties4(byte data)
                {
                    Tproperties4 ret = new Tproperties4();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties4 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties4 properties4 = 0;
            public IList<byte> flowLowThresholdValue = new List<byte>();
            public ByteValue sensorUsage = 0;
            public static implicit operator IRRIGATION_VALVE_CONFIG_SET(byte[] data)
            {
                IRRIGATION_VALVE_CONFIG_SET ret = new IRRIGATION_VALVE_CONFIG_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.valveId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nominalCurrentHighThreshold = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nominalCurrentLowThreshold = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.maximumFlowValue = new List<byte>();
                    for (int i = 0; i < ret.properties2.maximumFlowSize; i++)
                    {
                        if (data.Length > index) ret.maximumFlowValue.Add(data[index++]);
                    }
                    ret.properties3 = data.Length > index ? (Tproperties3)data[index++] : Tproperties3.Empty;
                    ret.flowHighThresholdValue = new List<byte>();
                    for (int i = 0; i < ret.properties3.flowHighThresholdSize; i++)
                    {
                        if (data.Length > index) ret.flowHighThresholdValue.Add(data[index++]);
                    }
                    ret.properties4 = data.Length > index ? (Tproperties4)data[index++] : Tproperties4.Empty;
                    ret.flowLowThresholdValue = new List<byte>();
                    for (int i = 0; i < ret.properties4.flowLowThresholdSize; i++)
                    {
                        if (data.Length > index) ret.flowLowThresholdValue.Add(data[index++]);
                    }
                    ret.sensorUsage = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](IRRIGATION_VALVE_CONFIG_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IRRIGATION.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.valveId.HasValue) ret.Add(command.valveId);
                if (command.nominalCurrentHighThreshold.HasValue) ret.Add(command.nominalCurrentHighThreshold);
                if (command.nominalCurrentLowThreshold.HasValue) ret.Add(command.nominalCurrentLowThreshold);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.maximumFlowValue != null)
                {
                    foreach (var tmp in command.maximumFlowValue)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties3.HasValue) ret.Add(command.properties3);
                if (command.flowHighThresholdValue != null)
                {
                    foreach (var tmp in command.flowHighThresholdValue)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties4.HasValue) ret.Add(command.properties4);
                if (command.flowLowThresholdValue != null)
                {
                    foreach (var tmp in command.flowLowThresholdValue)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.sensorUsage.HasValue) ret.Add(command.sensorUsage);
                return ret.ToArray();
            }
        }
        public partial class IRRIGATION_VALVE_CONFIG_GET
        {
            public const byte ID = 0x0B;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte mainValve
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 1 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0xFE; _value += (byte)(value << 1 & 0xFE); }
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
            public ByteValue valveId = 0;
            public static implicit operator IRRIGATION_VALVE_CONFIG_GET(byte[] data)
            {
                IRRIGATION_VALVE_CONFIG_GET ret = new IRRIGATION_VALVE_CONFIG_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.valveId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](IRRIGATION_VALVE_CONFIG_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IRRIGATION.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.valveId.HasValue) ret.Add(command.valveId);
                return ret.ToArray();
            }
        }
        public partial class IRRIGATION_VALVE_CONFIG_REPORT
        {
            public const byte ID = 0x0C;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte mainValve
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 1 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0xFE; _value += (byte)(value << 1 & 0xFE); }
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
            public ByteValue valveId = 0;
            public ByteValue nominalCurrentHighThreshold = 0;
            public ByteValue nominalCurrentLowThreshold = 0;
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte maximumFlowSize
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte maximumFlowScale
                {
                    get { return (byte)(_value >> 3 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x18; _value += (byte)(value << 3 & 0x18); }
                }
                public byte maximumFlowPrecision
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
            public IList<byte> maximumFlowValue = new List<byte>();
            public struct Tproperties3
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties3 Empty { get { return new Tproperties3() { _value = 0, HasValue = false }; } }
                public byte flowHighThresholdSize
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte flowHighThresholdScale
                {
                    get { return (byte)(_value >> 3 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x18; _value += (byte)(value << 3 & 0x18); }
                }
                public byte flowHighThresholdPrecision
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
            public IList<byte> flowHighThresholdValue = new List<byte>();
            public struct Tproperties4
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties4 Empty { get { return new Tproperties4() { _value = 0, HasValue = false }; } }
                public byte flowLowThresholdSize
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte flowLowThresholdScale
                {
                    get { return (byte)(_value >> 3 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x18; _value += (byte)(value << 3 & 0x18); }
                }
                public byte flowLowThresholdPrecision
                {
                    get { return (byte)(_value >> 5 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0xE0; _value += (byte)(value << 5 & 0xE0); }
                }
                public static implicit operator Tproperties4(byte data)
                {
                    Tproperties4 ret = new Tproperties4();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties4 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties4 properties4 = 0;
            public IList<byte> flowLowThresholdValue = new List<byte>();
            public ByteValue sensorUsage = 0;
            public static implicit operator IRRIGATION_VALVE_CONFIG_REPORT(byte[] data)
            {
                IRRIGATION_VALVE_CONFIG_REPORT ret = new IRRIGATION_VALVE_CONFIG_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.valveId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nominalCurrentHighThreshold = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nominalCurrentLowThreshold = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.maximumFlowValue = new List<byte>();
                    for (int i = 0; i < ret.properties2.maximumFlowSize; i++)
                    {
                        if (data.Length > index) ret.maximumFlowValue.Add(data[index++]);
                    }
                    ret.properties3 = data.Length > index ? (Tproperties3)data[index++] : Tproperties3.Empty;
                    ret.flowHighThresholdValue = new List<byte>();
                    for (int i = 0; i < ret.properties3.flowHighThresholdSize; i++)
                    {
                        if (data.Length > index) ret.flowHighThresholdValue.Add(data[index++]);
                    }
                    ret.properties4 = data.Length > index ? (Tproperties4)data[index++] : Tproperties4.Empty;
                    ret.flowLowThresholdValue = new List<byte>();
                    for (int i = 0; i < ret.properties4.flowLowThresholdSize; i++)
                    {
                        if (data.Length > index) ret.flowLowThresholdValue.Add(data[index++]);
                    }
                    ret.sensorUsage = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](IRRIGATION_VALVE_CONFIG_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IRRIGATION.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.valveId.HasValue) ret.Add(command.valveId);
                if (command.nominalCurrentHighThreshold.HasValue) ret.Add(command.nominalCurrentHighThreshold);
                if (command.nominalCurrentLowThreshold.HasValue) ret.Add(command.nominalCurrentLowThreshold);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.maximumFlowValue != null)
                {
                    foreach (var tmp in command.maximumFlowValue)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties3.HasValue) ret.Add(command.properties3);
                if (command.flowHighThresholdValue != null)
                {
                    foreach (var tmp in command.flowHighThresholdValue)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties4.HasValue) ret.Add(command.properties4);
                if (command.flowLowThresholdValue != null)
                {
                    foreach (var tmp in command.flowLowThresholdValue)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.sensorUsage.HasValue) ret.Add(command.sensorUsage);
                return ret.ToArray();
            }
        }
        public partial class IRRIGATION_VALVE_RUN
        {
            public const byte ID = 0x0D;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte mainValve
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 1 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0xFE; _value += (byte)(value << 1 & 0xFE); }
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
            public ByteValue valveId = 0;
            public const byte durationBytesCount = 2;
            public byte[] duration = new byte[durationBytesCount];
            public static implicit operator IRRIGATION_VALVE_RUN(byte[] data)
            {
                IRRIGATION_VALVE_RUN ret = new IRRIGATION_VALVE_RUN();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.valveId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.duration = (data.Length - index) >= durationBytesCount ? new byte[durationBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.duration[0] = data[index++];
                    if (data.Length > index) ret.duration[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](IRRIGATION_VALVE_RUN command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IRRIGATION.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.valveId.HasValue) ret.Add(command.valveId);
                if (command.duration != null)
                {
                    foreach (var tmp in command.duration)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class IRRIGATION_VALVE_TABLE_SET
        {
            public const byte ID = 0x0E;
            public ByteValue valveTableId = 0;
            public class TVG1
            {
                public ByteValue valveId = 0;
                public const byte durationBytesCount = 2;
                public byte[] duration = new byte[durationBytesCount];
            }
            public List<TVG1> vg1 = new List<TVG1>();
            public static implicit operator IRRIGATION_VALVE_TABLE_SET(byte[] data)
            {
                IRRIGATION_VALVE_TABLE_SET ret = new IRRIGATION_VALVE_TABLE_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.valveTableId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.vg1 = new List<TVG1>();
                    while (data.Length - 0 > index)
                    {
                        TVG1 tmp = new TVG1();
                        tmp.valveId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.duration = (data.Length - index) >= TVG1.durationBytesCount ? new byte[TVG1.durationBytesCount] : new byte[data.Length - index];
                        if (data.Length > index) tmp.duration[0] = data[index++];
                        if (data.Length > index) tmp.duration[1] = data[index++];
                        ret.vg1.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](IRRIGATION_VALVE_TABLE_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IRRIGATION.ID);
                ret.Add(ID);
                if (command.valveTableId.HasValue) ret.Add(command.valveTableId);
                if (command.vg1 != null)
                {
                    foreach (var item in command.vg1)
                    {
                        if (item.valveId.HasValue) ret.Add(item.valveId);
                        if (item.duration != null)
                        {
                            foreach (var tmp in item.duration)
                            {
                                ret.Add(tmp);
                            }
                        }
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class IRRIGATION_VALVE_TABLE_GET
        {
            public const byte ID = 0x0F;
            public ByteValue valveTableId = 0;
            public static implicit operator IRRIGATION_VALVE_TABLE_GET(byte[] data)
            {
                IRRIGATION_VALVE_TABLE_GET ret = new IRRIGATION_VALVE_TABLE_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.valveTableId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](IRRIGATION_VALVE_TABLE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IRRIGATION.ID);
                ret.Add(ID);
                if (command.valveTableId.HasValue) ret.Add(command.valveTableId);
                return ret.ToArray();
            }
        }
        public partial class IRRIGATION_VALVE_TABLE_REPORT
        {
            public const byte ID = 0x10;
            public ByteValue valveTableId = 0;
            public class TVG1
            {
                public ByteValue valveId = 0;
                public const byte durationBytesCount = 2;
                public byte[] duration = new byte[durationBytesCount];
            }
            public List<TVG1> vg1 = new List<TVG1>();
            public static implicit operator IRRIGATION_VALVE_TABLE_REPORT(byte[] data)
            {
                IRRIGATION_VALVE_TABLE_REPORT ret = new IRRIGATION_VALVE_TABLE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.valveTableId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.vg1 = new List<TVG1>();
                    while (data.Length - 0 > index)
                    {
                        TVG1 tmp = new TVG1();
                        tmp.valveId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.duration = (data.Length - index) >= TVG1.durationBytesCount ? new byte[TVG1.durationBytesCount] : new byte[data.Length - index];
                        if (data.Length > index) tmp.duration[0] = data[index++];
                        if (data.Length > index) tmp.duration[1] = data[index++];
                        ret.vg1.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](IRRIGATION_VALVE_TABLE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IRRIGATION.ID);
                ret.Add(ID);
                if (command.valveTableId.HasValue) ret.Add(command.valveTableId);
                if (command.vg1 != null)
                {
                    foreach (var item in command.vg1)
                    {
                        if (item.valveId.HasValue) ret.Add(item.valveId);
                        if (item.duration != null)
                        {
                            foreach (var tmp in item.duration)
                            {
                                ret.Add(tmp);
                            }
                        }
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class IRRIGATION_VALVE_TABLE_RUN
        {
            public const byte ID = 0x11;
            public IList<byte> valveTableId = new List<byte>();
            public static implicit operator IRRIGATION_VALVE_TABLE_RUN(byte[] data)
            {
                IRRIGATION_VALVE_TABLE_RUN ret = new IRRIGATION_VALVE_TABLE_RUN();
                if (data != null)
                {
                    int index = 2;
                    ret.valveTableId = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.valveTableId.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](IRRIGATION_VALVE_TABLE_RUN command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IRRIGATION.ID);
                ret.Add(ID);
                if (command.valveTableId != null)
                {
                    foreach (var tmp in command.valveTableId)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class IRRIGATION_SYSTEM_SHUTOFF
        {
            public const byte ID = 0x12;
            public ByteValue duration = 0;
            public static implicit operator IRRIGATION_SYSTEM_SHUTOFF(byte[] data)
            {
                IRRIGATION_SYSTEM_SHUTOFF ret = new IRRIGATION_SYSTEM_SHUTOFF();
                if (data != null)
                {
                    int index = 2;
                    ret.duration = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](IRRIGATION_SYSTEM_SHUTOFF command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IRRIGATION.ID);
                ret.Add(ID);
                if (command.duration.HasValue) ret.Add(command.duration);
                return ret.ToArray();
            }
        }
    }
}

