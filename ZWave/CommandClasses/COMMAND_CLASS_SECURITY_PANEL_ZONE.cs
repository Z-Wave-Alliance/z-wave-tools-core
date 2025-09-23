/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_SECURITY_PANEL_ZONE
    {
        public const byte ID = 0x2E;
        public const byte VERSION = 1;
        public partial class SECURITY_PANEL_ZONE_NUMBER_SUPPORTED_GET
        {
            public const byte ID = 0x01;
            public static implicit operator SECURITY_PANEL_ZONE_NUMBER_SUPPORTED_GET(byte[] data)
            {
                SECURITY_PANEL_ZONE_NUMBER_SUPPORTED_GET ret = new SECURITY_PANEL_ZONE_NUMBER_SUPPORTED_GET();
                return ret;
            }
            public static implicit operator byte[](SECURITY_PANEL_ZONE_NUMBER_SUPPORTED_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SECURITY_PANEL_ZONE.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class SECURITY_PANEL_ZONE_STATE_GET
        {
            public const byte ID = 0x05;
            public ByteValue zoneNumber = 0;
            public static implicit operator SECURITY_PANEL_ZONE_STATE_GET(byte[] data)
            {
                SECURITY_PANEL_ZONE_STATE_GET ret = new SECURITY_PANEL_ZONE_STATE_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.zoneNumber = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SECURITY_PANEL_ZONE_STATE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SECURITY_PANEL_ZONE.ID);
                ret.Add(ID);
                if (command.zoneNumber.HasValue) ret.Add(command.zoneNumber);
                return ret.ToArray();
            }
        }
        public partial class SECURITY_PANEL_ZONE_STATE_REPORT
        {
            public const byte ID = 0x06;
            public ByteValue zoneNumber = 0;
            public ByteValue zoneState = 0;
            public static implicit operator SECURITY_PANEL_ZONE_STATE_REPORT(byte[] data)
            {
                SECURITY_PANEL_ZONE_STATE_REPORT ret = new SECURITY_PANEL_ZONE_STATE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.zoneNumber = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.zoneState = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SECURITY_PANEL_ZONE_STATE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SECURITY_PANEL_ZONE.ID);
                ret.Add(ID);
                if (command.zoneNumber.HasValue) ret.Add(command.zoneNumber);
                if (command.zoneState.HasValue) ret.Add(command.zoneState);
                return ret.ToArray();
            }
        }
        public partial class SECURITY_PANEL_ZONE_SUPPORTED_REPORT
        {
            public const byte ID = 0x02;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte zonesSupported
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte zm
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
            public static implicit operator SECURITY_PANEL_ZONE_SUPPORTED_REPORT(byte[] data)
            {
                SECURITY_PANEL_ZONE_SUPPORTED_REPORT ret = new SECURITY_PANEL_ZONE_SUPPORTED_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SECURITY_PANEL_ZONE_SUPPORTED_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SECURITY_PANEL_ZONE.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                return ret.ToArray();
            }
        }
        public partial class SECURITY_PANEL_ZONE_TYPE_GET
        {
            public const byte ID = 0x03;
            public ByteValue zoneNumber = 0;
            public static implicit operator SECURITY_PANEL_ZONE_TYPE_GET(byte[] data)
            {
                SECURITY_PANEL_ZONE_TYPE_GET ret = new SECURITY_PANEL_ZONE_TYPE_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.zoneNumber = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SECURITY_PANEL_ZONE_TYPE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SECURITY_PANEL_ZONE.ID);
                ret.Add(ID);
                if (command.zoneNumber.HasValue) ret.Add(command.zoneNumber);
                return ret.ToArray();
            }
        }
        public partial class SECURITY_PANEL_ZONE_TYPE_REPORT
        {
            public const byte ID = 0x04;
            public ByteValue zoneNumber = 0;
            public ByteValue zoneType = 0;
            public static implicit operator SECURITY_PANEL_ZONE_TYPE_REPORT(byte[] data)
            {
                SECURITY_PANEL_ZONE_TYPE_REPORT ret = new SECURITY_PANEL_ZONE_TYPE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.zoneNumber = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.zoneType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SECURITY_PANEL_ZONE_TYPE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SECURITY_PANEL_ZONE.ID);
                ret.Add(ID);
                if (command.zoneNumber.HasValue) ret.Add(command.zoneNumber);
                if (command.zoneType.HasValue) ret.Add(command.zoneType);
                return ret.ToArray();
            }
        }
    }
}

