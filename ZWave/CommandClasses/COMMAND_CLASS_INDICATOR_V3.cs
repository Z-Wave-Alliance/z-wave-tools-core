/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_INDICATOR_V3
    {
        public const byte ID = 0x87;
        public const byte VERSION = 3;
        public partial class INDICATOR_GET
        {
            public const byte ID = 0x02;
            public ByteValue indicatorId = 0;
            public static implicit operator INDICATOR_GET(byte[] data)
            {
                INDICATOR_GET ret = new INDICATOR_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.indicatorId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](INDICATOR_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_INDICATOR_V3.ID);
                ret.Add(ID);
                if (command.indicatorId.HasValue) ret.Add(command.indicatorId);
                return ret.ToArray();
            }
        }
        public partial class INDICATOR_REPORT
        {
            public const byte ID = 0x03;
            public ByteValue indicator0Value = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte indicatorObjectCount
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
            public class TVG1
            {
                public ByteValue indicatorId = 0;
                public ByteValue propertyId = 0;
                public ByteValue value = 0;
            }
            public List<TVG1> vg1 = new List<TVG1>();
            public static implicit operator INDICATOR_REPORT(byte[] data)
            {
                INDICATOR_REPORT ret = new INDICATOR_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.indicator0Value = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.vg1 = new List<TVG1>();
                    for (int j = 0; j < ret.properties1.indicatorObjectCount; j++)
                    {
                        TVG1 tmp = new TVG1();
                        tmp.indicatorId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.propertyId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.value = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        ret.vg1.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](INDICATOR_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_INDICATOR_V3.ID);
                ret.Add(ID);
                if (command.indicator0Value.HasValue) ret.Add(command.indicator0Value);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.vg1 != null)
                {
                    foreach (var item in command.vg1)
                    {
                        if (item.indicatorId.HasValue) ret.Add(item.indicatorId);
                        if (item.propertyId.HasValue) ret.Add(item.propertyId);
                        if (item.value.HasValue) ret.Add(item.value);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class INDICATOR_SET
        {
            public const byte ID = 0x01;
            public ByteValue indicator0Value = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte indicatorObjectCount
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
            public class TVG1
            {
                public ByteValue indicatorId = 0;
                public ByteValue propertyId = 0;
                public ByteValue value = 0;
            }
            public List<TVG1> vg1 = new List<TVG1>();
            public static implicit operator INDICATOR_SET(byte[] data)
            {
                INDICATOR_SET ret = new INDICATOR_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.indicator0Value = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.vg1 = new List<TVG1>();
                    for (int j = 0; j < ret.properties1.indicatorObjectCount; j++)
                    {
                        TVG1 tmp = new TVG1();
                        tmp.indicatorId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.propertyId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.value = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        ret.vg1.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](INDICATOR_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_INDICATOR_V3.ID);
                ret.Add(ID);
                if (command.indicator0Value.HasValue) ret.Add(command.indicator0Value);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.vg1 != null)
                {
                    foreach (var item in command.vg1)
                    {
                        if (item.indicatorId.HasValue) ret.Add(item.indicatorId);
                        if (item.propertyId.HasValue) ret.Add(item.propertyId);
                        if (item.value.HasValue) ret.Add(item.value);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class INDICATOR_SUPPORTED_GET
        {
            public const byte ID = 0x04;
            public ByteValue indicatorId = 0;
            public static implicit operator INDICATOR_SUPPORTED_GET(byte[] data)
            {
                INDICATOR_SUPPORTED_GET ret = new INDICATOR_SUPPORTED_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.indicatorId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](INDICATOR_SUPPORTED_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_INDICATOR_V3.ID);
                ret.Add(ID);
                if (command.indicatorId.HasValue) ret.Add(command.indicatorId);
                return ret.ToArray();
            }
        }
        public partial class INDICATOR_SUPPORTED_REPORT
        {
            public const byte ID = 0x05;
            public ByteValue indicatorId = 0;
            public ByteValue nextIndicatorId = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte propertySupportedBitMaskLength
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
            public IList<byte> propertySupportedBitMask = new List<byte>();
            public static implicit operator INDICATOR_SUPPORTED_REPORT(byte[] data)
            {
                INDICATOR_SUPPORTED_REPORT ret = new INDICATOR_SUPPORTED_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.indicatorId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nextIndicatorId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.propertySupportedBitMask = new List<byte>();
                    for (int i = 0; i < ret.properties1.propertySupportedBitMaskLength; i++)
                    {
                        if (data.Length > index) ret.propertySupportedBitMask.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](INDICATOR_SUPPORTED_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_INDICATOR_V3.ID);
                ret.Add(ID);
                if (command.indicatorId.HasValue) ret.Add(command.indicatorId);
                if (command.nextIndicatorId.HasValue) ret.Add(command.nextIndicatorId);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.propertySupportedBitMask != null)
                {
                    foreach (var tmp in command.propertySupportedBitMask)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

