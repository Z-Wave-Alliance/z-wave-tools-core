/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_THERMOSTAT_MODE_V3
    {
        public const byte ID = 0x40;
        public const byte VERSION = 3;
        public partial class THERMOSTAT_MODE_GET
        {
            public const byte ID = 0x02;
            public static implicit operator THERMOSTAT_MODE_GET(byte[] data)
            {
                THERMOSTAT_MODE_GET ret = new THERMOSTAT_MODE_GET();
                return ret;
            }
            public static implicit operator byte[](THERMOSTAT_MODE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_THERMOSTAT_MODE_V3.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class THERMOSTAT_MODE_REPORT
        {
            public const byte ID = 0x03;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte mode
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte noOfManufacturerDataFields
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
            public IList<byte> manufacturerData = new List<byte>();
            public static implicit operator THERMOSTAT_MODE_REPORT(byte[] data)
            {
                THERMOSTAT_MODE_REPORT ret = new THERMOSTAT_MODE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.manufacturerData = new List<byte>();
                    for (int i = 0; i < ret.properties1.noOfManufacturerDataFields; i++)
                    {
                        if (data.Length > index) ret.manufacturerData.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](THERMOSTAT_MODE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_THERMOSTAT_MODE_V3.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.manufacturerData != null)
                {
                    foreach (var tmp in command.manufacturerData)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class THERMOSTAT_MODE_SET
        {
            public const byte ID = 0x01;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte mode
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte noOfManufacturerDataFields
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
            public IList<byte> manufacturerData = new List<byte>();
            public static implicit operator THERMOSTAT_MODE_SET(byte[] data)
            {
                THERMOSTAT_MODE_SET ret = new THERMOSTAT_MODE_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.manufacturerData = new List<byte>();
                    for (int i = 0; i < ret.properties1.noOfManufacturerDataFields; i++)
                    {
                        if (data.Length > index) ret.manufacturerData.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](THERMOSTAT_MODE_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_THERMOSTAT_MODE_V3.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.manufacturerData != null)
                {
                    foreach (var tmp in command.manufacturerData)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class THERMOSTAT_MODE_SUPPORTED_GET
        {
            public const byte ID = 0x04;
            public static implicit operator THERMOSTAT_MODE_SUPPORTED_GET(byte[] data)
            {
                THERMOSTAT_MODE_SUPPORTED_GET ret = new THERMOSTAT_MODE_SUPPORTED_GET();
                return ret;
            }
            public static implicit operator byte[](THERMOSTAT_MODE_SUPPORTED_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_THERMOSTAT_MODE_V3.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class THERMOSTAT_MODE_SUPPORTED_REPORT
        {
            public const byte ID = 0x05;
            public IList<byte> bitMask = new List<byte>();
            public static implicit operator THERMOSTAT_MODE_SUPPORTED_REPORT(byte[] data)
            {
                THERMOSTAT_MODE_SUPPORTED_REPORT ret = new THERMOSTAT_MODE_SUPPORTED_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.bitMask = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.bitMask.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](THERMOSTAT_MODE_SUPPORTED_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_THERMOSTAT_MODE_V3.ID);
                ret.Add(ID);
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

