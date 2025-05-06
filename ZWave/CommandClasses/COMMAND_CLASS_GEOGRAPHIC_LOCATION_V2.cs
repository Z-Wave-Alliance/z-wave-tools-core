using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_GEOGRAPHIC_LOCATION_V2
    {
        public const byte ID = 0x8C;
        public const byte VERSION = 2;
        public partial class GEOGRAPHIC_LOCATION_GET
        {
            public const byte ID = 0x02;
            public static implicit operator GEOGRAPHIC_LOCATION_GET(byte[] data)
            {
                GEOGRAPHIC_LOCATION_GET ret = new GEOGRAPHIC_LOCATION_GET();
                return ret;
            }
            public static implicit operator byte[](GEOGRAPHIC_LOCATION_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_GEOGRAPHIC_LOCATION_V2.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class GEOGRAPHIC_LOCATION_REPORT
        {
            public const byte ID = 0x03;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte longitudeInteger71
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte longitudeSign
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
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte longitudeFraction2216
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte longitudeInteger0
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
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
            public ByteValue longitudeFraction158 = 0;
            public ByteValue longitudeFraction70 = 0;
            public struct Tproperties3
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties3 Empty { get { return new Tproperties3() { _value = 0, HasValue = false }; } }
                public byte latitudeInteger71
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte latitudeSign
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
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
            public struct Tproperties4
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties4 Empty { get { return new Tproperties4() { _value = 0, HasValue = false }; } }
                public byte latitudeFraction2216
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte latitudeInteger0
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
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
            public ByteValue latitudeFraction158 = 0;
            public ByteValue latitudeFraction70 = 0;
            public ByteValue altitude2316 = 0;
            public ByteValue altitude158 = 0;
            public ByteValue altitude70 = 0;
            public struct Tproperties5
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties5 Empty { get { return new Tproperties5() { _value = 0, HasValue = false }; } }
                public byte reserved
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte readOnly
                {
                    get { return (byte)(_value >> 3 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x08; _value += (byte)(value << 3 & 0x08); }
                }
                public byte quality
                {
                    get { return (byte)(_value >> 4 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0xF0; _value += (byte)(value << 4 & 0xF0); }
                }
                public static implicit operator Tproperties5(byte data)
                {
                    Tproperties5 ret = new Tproperties5();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties5 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties5 properties5 = 0;
            public static implicit operator GEOGRAPHIC_LOCATION_REPORT(byte[] data)
            {
                GEOGRAPHIC_LOCATION_REPORT ret = new GEOGRAPHIC_LOCATION_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.longitudeFraction158 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.longitudeFraction70 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties3 = data.Length > index ? (Tproperties3)data[index++] : Tproperties3.Empty;
                    ret.properties4 = data.Length > index ? (Tproperties4)data[index++] : Tproperties4.Empty;
                    ret.latitudeFraction158 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.latitudeFraction70 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.altitude2316 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.altitude158 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.altitude70 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties5 = data.Length > index ? (Tproperties5)data[index++] : Tproperties5.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](GEOGRAPHIC_LOCATION_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_GEOGRAPHIC_LOCATION_V2.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.longitudeFraction158.HasValue) ret.Add(command.longitudeFraction158);
                if (command.longitudeFraction70.HasValue) ret.Add(command.longitudeFraction70);
                if (command.properties3.HasValue) ret.Add(command.properties3);
                if (command.properties4.HasValue) ret.Add(command.properties4);
                if (command.latitudeFraction158.HasValue) ret.Add(command.latitudeFraction158);
                if (command.latitudeFraction70.HasValue) ret.Add(command.latitudeFraction70);
                if (command.altitude2316.HasValue) ret.Add(command.altitude2316);
                if (command.altitude158.HasValue) ret.Add(command.altitude158);
                if (command.altitude70.HasValue) ret.Add(command.altitude70);
                if (command.properties5.HasValue) ret.Add(command.properties5);
                return ret.ToArray();
            }
        }
        public partial class GEOGRAPHIC_LOCATION_SET
        {
            public const byte ID = 0x01;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte longitudeInteger71
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte longitudeSign
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
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte longitudeFraction2216
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte longitudeInteger0
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
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
            public ByteValue longitudeFraction158 = 0;
            public ByteValue longitudeFraction70 = 0;
            public struct Tproperties3
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties3 Empty { get { return new Tproperties3() { _value = 0, HasValue = false }; } }
                public byte latitudeInteger71
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte latitudeSign
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
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
            public struct Tproperties4
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties4 Empty { get { return new Tproperties4() { _value = 0, HasValue = false }; } }
                public byte latitudeFraction2216
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte latitudeInteger0
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
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
            public ByteValue latitudeFraction158 = 0;
            public ByteValue latitudeFraction70 = 0;
            public ByteValue altitude2316 = 0;
            public ByteValue altitude158 = 0;
            public ByteValue altitude70 = 0;
            public static implicit operator GEOGRAPHIC_LOCATION_SET(byte[] data)
            {
                GEOGRAPHIC_LOCATION_SET ret = new GEOGRAPHIC_LOCATION_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.longitudeFraction158 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.longitudeFraction70 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties3 = data.Length > index ? (Tproperties3)data[index++] : Tproperties3.Empty;
                    ret.properties4 = data.Length > index ? (Tproperties4)data[index++] : Tproperties4.Empty;
                    ret.latitudeFraction158 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.latitudeFraction70 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.altitude2316 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.altitude158 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.altitude70 = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](GEOGRAPHIC_LOCATION_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_GEOGRAPHIC_LOCATION_V2.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.longitudeFraction158.HasValue) ret.Add(command.longitudeFraction158);
                if (command.longitudeFraction70.HasValue) ret.Add(command.longitudeFraction70);
                if (command.properties3.HasValue) ret.Add(command.properties3);
                if (command.properties4.HasValue) ret.Add(command.properties4);
                if (command.latitudeFraction158.HasValue) ret.Add(command.latitudeFraction158);
                if (command.latitudeFraction70.HasValue) ret.Add(command.latitudeFraction70);
                if (command.altitude2316.HasValue) ret.Add(command.altitude2316);
                if (command.altitude158.HasValue) ret.Add(command.altitude158);
                if (command.altitude70.HasValue) ret.Add(command.altitude70);
                return ret.ToArray();
            }
        }
    }
}

