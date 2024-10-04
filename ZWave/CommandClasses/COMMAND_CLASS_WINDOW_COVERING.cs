/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_WINDOW_COVERING
    {
        public const byte ID = 0x6A;
        public const byte VERSION = 1;
        public partial class WINDOW_COVERING_SUPPORTED_GET
        {
            public const byte ID = 0x01;
            public static implicit operator WINDOW_COVERING_SUPPORTED_GET(byte[] data)
            {
                WINDOW_COVERING_SUPPORTED_GET ret = new WINDOW_COVERING_SUPPORTED_GET();
                return ret;
            }
            public static implicit operator byte[](WINDOW_COVERING_SUPPORTED_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_WINDOW_COVERING.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class WINDOW_COVERING_SUPPORTED_REPORT
        {
            public const byte ID = 0x02;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte numberOfParameterMaskBytes
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
            public IList<byte> parameterMask = new List<byte>();
            public static implicit operator WINDOW_COVERING_SUPPORTED_REPORT(byte[] data)
            {
                WINDOW_COVERING_SUPPORTED_REPORT ret = new WINDOW_COVERING_SUPPORTED_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.parameterMask = new List<byte>();
                    for (int i = 0; i < ret.properties1.numberOfParameterMaskBytes; i++)
                    {
                        if (data.Length > index) ret.parameterMask.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](WINDOW_COVERING_SUPPORTED_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_WINDOW_COVERING.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.parameterMask != null)
                {
                    foreach (var tmp in command.parameterMask)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class WINDOW_COVERING_GET
        {
            public const byte ID = 0x03;
            public ByteValue parameterId = 0;
            public static implicit operator WINDOW_COVERING_GET(byte[] data)
            {
                WINDOW_COVERING_GET ret = new WINDOW_COVERING_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.parameterId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](WINDOW_COVERING_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_WINDOW_COVERING.ID);
                ret.Add(ID);
                if (command.parameterId.HasValue) ret.Add(command.parameterId);
                return ret.ToArray();
            }
        }
        public partial class WINDOW_COVERING_REPORT
        {
            public const byte ID = 0x04;
            public ByteValue parameterId = 0;
            public ByteValue currentValue = 0;
            public ByteValue targetValue = 0;
            public ByteValue duration = 0;
            public static implicit operator WINDOW_COVERING_REPORT(byte[] data)
            {
                WINDOW_COVERING_REPORT ret = new WINDOW_COVERING_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.parameterId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.currentValue = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.targetValue = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.duration = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](WINDOW_COVERING_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_WINDOW_COVERING.ID);
                ret.Add(ID);
                if (command.parameterId.HasValue) ret.Add(command.parameterId);
                if (command.currentValue.HasValue) ret.Add(command.currentValue);
                if (command.targetValue.HasValue) ret.Add(command.targetValue);
                if (command.duration.HasValue) ret.Add(command.duration);
                return ret.ToArray();
            }
        }
        public partial class WINDOW_COVERING_SET
        {
            public const byte ID = 0x05;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte parameterCount
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
                public ByteValue parameterId = 0;
                public ByteValue value = 0;
            }
            public List<TVG1> vg1 = new List<TVG1>();
            public ByteValue duration = 0;
            public static implicit operator WINDOW_COVERING_SET(byte[] data)
            {
                WINDOW_COVERING_SET ret = new WINDOW_COVERING_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.vg1 = new List<TVG1>();
                    for (int j = 0; j < ret.properties1.parameterCount; j++)
                    {
                        TVG1 tmp = new TVG1();
                        tmp.parameterId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.value = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        ret.vg1.Add(tmp);
                    }
                    ret.duration = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](WINDOW_COVERING_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_WINDOW_COVERING.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.vg1 != null)
                {
                    foreach (var item in command.vg1)
                    {
                        if (item.parameterId.HasValue) ret.Add(item.parameterId);
                        if (item.value.HasValue) ret.Add(item.value);
                    }
                }
                if (command.duration.HasValue) ret.Add(command.duration);
                return ret.ToArray();
            }
        }
        public partial class WINDOW_COVERING_START_LEVEL_CHANGE
        {
            public const byte ID = 0x06;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte res1
                {
                    get { return (byte)(_value >> 0 & 0x3F); }
                    set { HasValue = true; _value &= 0xFF - 0x3F; _value += (byte)(value << 0 & 0x3F); }
                }
                public byte upDown
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte res2
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
            public ByteValue parameterId = 0;
            public ByteValue duration = 0;
            public static implicit operator WINDOW_COVERING_START_LEVEL_CHANGE(byte[] data)
            {
                WINDOW_COVERING_START_LEVEL_CHANGE ret = new WINDOW_COVERING_START_LEVEL_CHANGE();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.parameterId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.duration = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](WINDOW_COVERING_START_LEVEL_CHANGE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_WINDOW_COVERING.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.parameterId.HasValue) ret.Add(command.parameterId);
                if (command.duration.HasValue) ret.Add(command.duration);
                return ret.ToArray();
            }
        }
        public partial class WINDOW_COVERING_STOP_LEVEL_CHANGE
        {
            public const byte ID = 0x07;
            public ByteValue parameterId = 0;
            public static implicit operator WINDOW_COVERING_STOP_LEVEL_CHANGE(byte[] data)
            {
                WINDOW_COVERING_STOP_LEVEL_CHANGE ret = new WINDOW_COVERING_STOP_LEVEL_CHANGE();
                if (data != null)
                {
                    int index = 2;
                    ret.parameterId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](WINDOW_COVERING_STOP_LEVEL_CHANGE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_WINDOW_COVERING.ID);
                ret.Add(ID);
                if (command.parameterId.HasValue) ret.Add(command.parameterId);
                return ret.ToArray();
            }
        }
    }
}

