/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_THERMOSTAT_FAN_STATE
    {
        public const byte ID = 0x45;
        public const byte VERSION = 1;
        public partial class THERMOSTAT_FAN_STATE_GET
        {
            public const byte ID = 0x02;
            public static implicit operator THERMOSTAT_FAN_STATE_GET(byte[] data)
            {
                THERMOSTAT_FAN_STATE_GET ret = new THERMOSTAT_FAN_STATE_GET();
                return ret;
            }
            public static implicit operator byte[](THERMOSTAT_FAN_STATE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_THERMOSTAT_FAN_STATE.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class THERMOSTAT_FAN_STATE_REPORT
        {
            public const byte ID = 0x03;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte fanOperatingState
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte reserved
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
            public static implicit operator THERMOSTAT_FAN_STATE_REPORT(byte[] data)
            {
                THERMOSTAT_FAN_STATE_REPORT ret = new THERMOSTAT_FAN_STATE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](THERMOSTAT_FAN_STATE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_THERMOSTAT_FAN_STATE.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                return ret.ToArray();
            }
        }
    }
}

