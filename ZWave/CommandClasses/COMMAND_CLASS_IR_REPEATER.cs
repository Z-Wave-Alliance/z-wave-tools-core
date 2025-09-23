/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_IR_REPEATER
    {
        public const byte ID = 0xA0;
        public const byte VERSION = 1;
        public partial class IR_REPEATER_CAPABILITIES_GET
        {
            public const byte ID = 0x01;
            public static implicit operator IR_REPEATER_CAPABILITIES_GET(byte[] data)
            {
                IR_REPEATER_CAPABILITIES_GET ret = new IR_REPEATER_CAPABILITIES_GET();
                return ret;
            }
            public static implicit operator byte[](IR_REPEATER_CAPABILITIES_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IR_REPEATER.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class IR_REPEATER_CAPABILITIES_REPORT
        {
            public const byte ID = 0x02;
            public ByteValue numberOfIrCodeIdentifiersForLearning = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte dutyCycleBitmask
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte bes
                {
                    get { return (byte)(_value >> 3 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x08; _value += (byte)(value << 3 & 0x08); }
                }
                public byte pes
                {
                    get { return (byte)(_value >> 4 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x10; _value += (byte)(value << 4 & 0x10); }
                }
                public byte rlc
                {
                    get { return (byte)(_value >> 5 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x20; _value += (byte)(value << 5 & 0x20); }
                }
                public byte lcr
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte irr
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
            public ByteValue carrier = 0;
            public ByteValue minimumSubCarrier = 0;
            public ByteValue maxiumSubCarrier = 0;
            public ByteValue minimumPulseTimeUnitMsb = 0;
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte maximumPulseTimeUnitMsb
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte minimumPulseTimeUnitLsb
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
            public ByteValue maximumPulseTimeUnitLsb = 0;
            public static implicit operator IR_REPEATER_CAPABILITIES_REPORT(byte[] data)
            {
                IR_REPEATER_CAPABILITIES_REPORT ret = new IR_REPEATER_CAPABILITIES_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.numberOfIrCodeIdentifiersForLearning = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.carrier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.minimumSubCarrier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.maxiumSubCarrier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.minimumPulseTimeUnitMsb = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.maximumPulseTimeUnitLsb = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](IR_REPEATER_CAPABILITIES_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IR_REPEATER.ID);
                ret.Add(ID);
                if (command.numberOfIrCodeIdentifiersForLearning.HasValue) ret.Add(command.numberOfIrCodeIdentifiersForLearning);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.carrier.HasValue) ret.Add(command.carrier);
                if (command.minimumSubCarrier.HasValue) ret.Add(command.minimumSubCarrier);
                if (command.maxiumSubCarrier.HasValue) ret.Add(command.maxiumSubCarrier);
                if (command.minimumPulseTimeUnitMsb.HasValue) ret.Add(command.minimumPulseTimeUnitMsb);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.maximumPulseTimeUnitLsb.HasValue) ret.Add(command.maximumPulseTimeUnitLsb);
                return ret.ToArray();
            }
        }
        public partial class IR_REPEATER_IR_CODE_LEARNING_START
        {
            public const byte ID = 0x03;
            public ByteValue irCodeIdentifier = 0;
            public ByteValue timeout = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte pulseTimeUnitMsb
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte reserved1
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
            public ByteValue pulseTimeUnitLsb = 0;
            public static implicit operator IR_REPEATER_IR_CODE_LEARNING_START(byte[] data)
            {
                IR_REPEATER_IR_CODE_LEARNING_START ret = new IR_REPEATER_IR_CODE_LEARNING_START();
                if (data != null)
                {
                    int index = 2;
                    ret.irCodeIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.timeout = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.pulseTimeUnitLsb = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](IR_REPEATER_IR_CODE_LEARNING_START command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IR_REPEATER.ID);
                ret.Add(ID);
                if (command.irCodeIdentifier.HasValue) ret.Add(command.irCodeIdentifier);
                if (command.timeout.HasValue) ret.Add(command.timeout);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.pulseTimeUnitLsb.HasValue) ret.Add(command.pulseTimeUnitLsb);
                return ret.ToArray();
            }
        }
        public partial class IR_REPEATER_IR_CODE_LEARNING_STOP
        {
            public const byte ID = 0x04;
            public static implicit operator IR_REPEATER_IR_CODE_LEARNING_STOP(byte[] data)
            {
                IR_REPEATER_IR_CODE_LEARNING_STOP ret = new IR_REPEATER_IR_CODE_LEARNING_STOP();
                return ret;
            }
            public static implicit operator byte[](IR_REPEATER_IR_CODE_LEARNING_STOP command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IR_REPEATER.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class IR_REPEATER_IR_CODE_LEARNING_STATUS
        {
            public const byte ID = 0x05;
            public ByteValue irCodeIdentifier = 0;
            public ByteValue status = 0;
            public static implicit operator IR_REPEATER_IR_CODE_LEARNING_STATUS(byte[] data)
            {
                IR_REPEATER_IR_CODE_LEARNING_STATUS ret = new IR_REPEATER_IR_CODE_LEARNING_STATUS();
                if (data != null)
                {
                    int index = 2;
                    ret.irCodeIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.status = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](IR_REPEATER_IR_CODE_LEARNING_STATUS command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IR_REPEATER.ID);
                ret.Add(ID);
                if (command.irCodeIdentifier.HasValue) ret.Add(command.irCodeIdentifier);
                if (command.status.HasValue) ret.Add(command.status);
                return ret.ToArray();
            }
        }
        public partial class IR_REPEATER_LEARNT_IR_CODE_REMOVE
        {
            public const byte ID = 0x06;
            public ByteValue irCodeIdentifier = 0;
            public static implicit operator IR_REPEATER_LEARNT_IR_CODE_REMOVE(byte[] data)
            {
                IR_REPEATER_LEARNT_IR_CODE_REMOVE ret = new IR_REPEATER_LEARNT_IR_CODE_REMOVE();
                if (data != null)
                {
                    int index = 2;
                    ret.irCodeIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](IR_REPEATER_LEARNT_IR_CODE_REMOVE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IR_REPEATER.ID);
                ret.Add(ID);
                if (command.irCodeIdentifier.HasValue) ret.Add(command.irCodeIdentifier);
                return ret.ToArray();
            }
        }
        public partial class IR_REPEATER_LEARNT_IR_CODE_GET
        {
            public const byte ID = 0x07;
            public static implicit operator IR_REPEATER_LEARNT_IR_CODE_GET(byte[] data)
            {
                IR_REPEATER_LEARNT_IR_CODE_GET ret = new IR_REPEATER_LEARNT_IR_CODE_GET();
                return ret;
            }
            public static implicit operator byte[](IR_REPEATER_LEARNT_IR_CODE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IR_REPEATER.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class IR_REPEATER_LEARNT_IR_CODE_REPORT
        {
            public const byte ID = 0x08;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte irCodeIdenfierStatusLength
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
            public IList<byte> irCodeIdentifierStatusBitmask = new List<byte>();
            public static implicit operator IR_REPEATER_LEARNT_IR_CODE_REPORT(byte[] data)
            {
                IR_REPEATER_LEARNT_IR_CODE_REPORT ret = new IR_REPEATER_LEARNT_IR_CODE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.irCodeIdentifierStatusBitmask = new List<byte>();
                    for (int i = 0; i < ret.properties1.irCodeIdenfierStatusLength; i++)
                    {
                        if (data.Length > index) ret.irCodeIdentifierStatusBitmask.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](IR_REPEATER_LEARNT_IR_CODE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IR_REPEATER.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.irCodeIdentifierStatusBitmask != null)
                {
                    foreach (var tmp in command.irCodeIdentifierStatusBitmask)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class IR_REPEATER_LEARNT_IR_CODE_READBACK_GET
        {
            public const byte ID = 0x09;
            public ByteValue irCodeIdentifier = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte reportNumberMsb
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte reserved1
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
            public ByteValue reportNumberLsb = 0;
            public static implicit operator IR_REPEATER_LEARNT_IR_CODE_READBACK_GET(byte[] data)
            {
                IR_REPEATER_LEARNT_IR_CODE_READBACK_GET ret = new IR_REPEATER_LEARNT_IR_CODE_READBACK_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.irCodeIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.reportNumberLsb = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](IR_REPEATER_LEARNT_IR_CODE_READBACK_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IR_REPEATER.ID);
                ret.Add(ID);
                if (command.irCodeIdentifier.HasValue) ret.Add(command.irCodeIdentifier);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.reportNumberLsb.HasValue) ret.Add(command.reportNumberLsb);
                return ret.ToArray();
            }
        }
        public partial class IR_REPEATER_LEARNT_IR_CODE_READBACK_REPORT
        {
            public const byte ID = 0x0A;
            public ByteValue irCodeIdentifier = 0;
            public ByteValue subCarrier = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte pulseTimeUnitMsb
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte dutyCycle
                {
                    get { return (byte)(_value >> 4 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x30; _value += (byte)(value << 4 & 0x30); }
                }
                public byte reserved1
                {
                    get { return (byte)(_value >> 6 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0xC0; _value += (byte)(value << 6 & 0xC0); }
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
            public ByteValue pulseTimeUnitLsb = 0;
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte reportNumberMsb
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte last
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
            public ByteValue reportNumberLsb = 0;
            public class TVG1
            {
                public struct Tproperties1
                {
                    private byte _value;
                    public bool HasValue { get; private set; }
                    public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                    public byte dataBlockLength
                    {
                        get { return (byte)(_value >> 0 & 0x3F); }
                        set { HasValue = true; _value &= 0xFF - 0x3F; _value += (byte)(value << 0 & 0x3F); }
                    }
                    public byte dataBlockType
                    {
                        get { return (byte)(_value >> 6 & 0x03); }
                        set { HasValue = true; _value &= 0xFF - 0xC0; _value += (byte)(value << 6 & 0xC0); }
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
                public IList<byte> dataBlockValue = new List<byte>();
            }
            public List<TVG1> vg1 = new List<TVG1>();
            public static implicit operator IR_REPEATER_LEARNT_IR_CODE_READBACK_REPORT(byte[] data)
            {
                IR_REPEATER_LEARNT_IR_CODE_READBACK_REPORT ret = new IR_REPEATER_LEARNT_IR_CODE_READBACK_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.irCodeIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.subCarrier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.pulseTimeUnitLsb = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.reportNumberLsb = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.vg1 = new List<TVG1>();
                    while (data.Length - 0 > index)
                    {
                        TVG1 tmp = new TVG1();
                        tmp.properties1 = data.Length > index ? (TVG1.Tproperties1)data[index++] : TVG1.Tproperties1.Empty;
                        tmp.dataBlockValue = new List<byte>();
                        for (int i = 0; i < tmp.properties1.dataBlockLength; i++)
                        {
                            if (data.Length > index) tmp.dataBlockValue.Add(data[index++]);
                        }
                        ret.vg1.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](IR_REPEATER_LEARNT_IR_CODE_READBACK_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IR_REPEATER.ID);
                ret.Add(ID);
                if (command.irCodeIdentifier.HasValue) ret.Add(command.irCodeIdentifier);
                if (command.subCarrier.HasValue) ret.Add(command.subCarrier);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.pulseTimeUnitLsb.HasValue) ret.Add(command.pulseTimeUnitLsb);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.reportNumberLsb.HasValue) ret.Add(command.reportNumberLsb);
                if (command.vg1 != null)
                {
                    foreach (var item in command.vg1)
                    {
                        if (item.properties1.HasValue) ret.Add(item.properties1);
                        if (item.dataBlockValue != null)
                        {
                            foreach (var tmp in item.dataBlockValue)
                            {
                                ret.Add(tmp);
                            }
                        }
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class IR_REPEATER_CONFIGURATION_SET
        {
            public const byte ID = 0x0B;
            public ByteValue periodMsb = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte periodLsb
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte reserved1
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
            public class TVG1
            {
                public const byte dataBitEncodingLengthBytesCount = 2;
                public byte[] dataBitEncodingLength = new byte[dataBitEncodingLengthBytesCount];
                public IList<byte> dataBitEncoding = new List<byte>();
            }
            public List<TVG1> vg1 = new List<TVG1>();
            public static implicit operator IR_REPEATER_CONFIGURATION_SET(byte[] data)
            {
                IR_REPEATER_CONFIGURATION_SET ret = new IR_REPEATER_CONFIGURATION_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.periodMsb = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.vg1 = new List<TVG1>();
                    while (data.Length - 0 > index)
                    {
                        TVG1 tmp = new TVG1();
                        tmp.dataBitEncodingLength = (data.Length - index) >= TVG1.dataBitEncodingLengthBytesCount ? new byte[TVG1.dataBitEncodingLengthBytesCount] : new byte[data.Length - index];
                        if (data.Length > index) tmp.dataBitEncodingLength[0] = data[index++];
                        if (data.Length > index) tmp.dataBitEncodingLength[1] = data[index++];
                        tmp.dataBitEncoding = new List<byte>();
                        for (int i = 0; i < (tmp.dataBitEncodingLength[0] << 8) + tmp.dataBitEncodingLength[1]; i++)
                        {
                            if (data.Length > index) tmp.dataBitEncoding.Add(data[index++]);
                        }
                        ret.vg1.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](IR_REPEATER_CONFIGURATION_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IR_REPEATER.ID);
                ret.Add(ID);
                if (command.periodMsb.HasValue) ret.Add(command.periodMsb);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.vg1 != null)
                {
                    foreach (var item in command.vg1)
                    {
                        if (item.dataBitEncodingLength != null)
                        {
                            foreach (var tmp in item.dataBitEncodingLength)
                            {
                                ret.Add(tmp);
                            }
                        }
                        if (item.dataBitEncoding != null)
                        {
                            foreach (var tmp in item.dataBitEncoding)
                            {
                                ret.Add(tmp);
                            }
                        }
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class IR_REPEATER_CONFIGURATION_GET
        {
            public const byte ID = 0x0C;
            public static implicit operator IR_REPEATER_CONFIGURATION_GET(byte[] data)
            {
                IR_REPEATER_CONFIGURATION_GET ret = new IR_REPEATER_CONFIGURATION_GET();
                return ret;
            }
            public static implicit operator byte[](IR_REPEATER_CONFIGURATION_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IR_REPEATER.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class IR_REPEATER_CONFIGURATION_REPORT
        {
            public const byte ID = 0x0D;
            public ByteValue periodMsb = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte periodLsb
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte reserved1
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
            public class TVG1
            {
                public const byte dataBitEncodingLengthBytesCount = 2;
                public byte[] dataBitEncodingLength = new byte[dataBitEncodingLengthBytesCount];
                public IList<byte> dataBitEncoding = new List<byte>();
            }
            public List<TVG1> vg1 = new List<TVG1>();
            public static implicit operator IR_REPEATER_CONFIGURATION_REPORT(byte[] data)
            {
                IR_REPEATER_CONFIGURATION_REPORT ret = new IR_REPEATER_CONFIGURATION_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.periodMsb = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.vg1 = new List<TVG1>();
                    while (data.Length - 0 > index)
                    {
                        TVG1 tmp = new TVG1();
                        tmp.dataBitEncodingLength = (data.Length - index) >= TVG1.dataBitEncodingLengthBytesCount ? new byte[TVG1.dataBitEncodingLengthBytesCount] : new byte[data.Length - index];
                        if (data.Length > index) tmp.dataBitEncodingLength[0] = data[index++];
                        if (data.Length > index) tmp.dataBitEncodingLength[1] = data[index++];
                        tmp.dataBitEncoding = new List<byte>();
                        for (int i = 0; i < (tmp.dataBitEncodingLength[0] << 8) + tmp.dataBitEncodingLength[1]; i++)
                        {
                            if (data.Length > index) tmp.dataBitEncoding.Add(data[index++]);
                        }
                        ret.vg1.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](IR_REPEATER_CONFIGURATION_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IR_REPEATER.ID);
                ret.Add(ID);
                if (command.periodMsb.HasValue) ret.Add(command.periodMsb);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.vg1 != null)
                {
                    foreach (var item in command.vg1)
                    {
                        if (item.dataBitEncodingLength != null)
                        {
                            foreach (var tmp in item.dataBitEncodingLength)
                            {
                                ret.Add(tmp);
                            }
                        }
                        if (item.dataBitEncoding != null)
                        {
                            foreach (var tmp in item.dataBitEncoding)
                            {
                                ret.Add(tmp);
                            }
                        }
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class IR_REPEATER_REPEAT_LEARNT_CODE
        {
            public const byte ID = 0x0E;
            public ByteValue irCodeIdentifier = 0;
            public static implicit operator IR_REPEATER_REPEAT_LEARNT_CODE(byte[] data)
            {
                IR_REPEATER_REPEAT_LEARNT_CODE ret = new IR_REPEATER_REPEAT_LEARNT_CODE();
                if (data != null)
                {
                    int index = 2;
                    ret.irCodeIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](IR_REPEATER_REPEAT_LEARNT_CODE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IR_REPEATER.ID);
                ret.Add(ID);
                if (command.irCodeIdentifier.HasValue) ret.Add(command.irCodeIdentifier);
                return ret.ToArray();
            }
        }
        public partial class IR_REPEATER_REPEAT
        {
            public const byte ID = 0x0F;
            public ByteValue sequenceNumber = 0;
            public ByteValue subCarrier = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte pulseTimeUnitMsb
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte dutyCycle
                {
                    get { return (byte)(_value >> 4 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x30; _value += (byte)(value << 4 & 0x30); }
                }
                public byte reserved1
                {
                    get { return (byte)(_value >> 6 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0xC0; _value += (byte)(value << 6 & 0xC0); }
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
            public ByteValue pulseTimeUnitLsb = 0;
            public struct Tproperties2
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties2 Empty { get { return new Tproperties2() { _value = 0, HasValue = false }; } }
                public byte reportNumberMsb
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte last
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
            public ByteValue reportNumberLsb = 0;
            public class TVG1
            {
                public struct Tproperties1
                {
                    private byte _value;
                    public bool HasValue { get; private set; }
                    public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                    public byte dataBlockLength
                    {
                        get { return (byte)(_value >> 0 & 0x3F); }
                        set { HasValue = true; _value &= 0xFF - 0x3F; _value += (byte)(value << 0 & 0x3F); }
                    }
                    public byte dataBlockType
                    {
                        get { return (byte)(_value >> 6 & 0x03); }
                        set { HasValue = true; _value &= 0xFF - 0xC0; _value += (byte)(value << 6 & 0xC0); }
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
                public IList<byte> dataBlockValue = new List<byte>();
            }
            public List<TVG1> vg1 = new List<TVG1>();
            public static implicit operator IR_REPEATER_REPEAT(byte[] data)
            {
                IR_REPEATER_REPEAT ret = new IR_REPEATER_REPEAT();
                if (data != null)
                {
                    int index = 2;
                    ret.sequenceNumber = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.subCarrier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.pulseTimeUnitLsb = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties2 = data.Length > index ? (Tproperties2)data[index++] : Tproperties2.Empty;
                    ret.reportNumberLsb = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.vg1 = new List<TVG1>();
                    while (data.Length - 0 > index)
                    {
                        TVG1 tmp = new TVG1();
                        tmp.properties1 = data.Length > index ? (TVG1.Tproperties1)data[index++] : TVG1.Tproperties1.Empty;
                        tmp.dataBlockValue = new List<byte>();
                        for (int i = 0; i < tmp.properties1.dataBlockLength; i++)
                        {
                            if (data.Length > index) tmp.dataBlockValue.Add(data[index++]);
                        }
                        ret.vg1.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](IR_REPEATER_REPEAT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_IR_REPEATER.ID);
                ret.Add(ID);
                if (command.sequenceNumber.HasValue) ret.Add(command.sequenceNumber);
                if (command.subCarrier.HasValue) ret.Add(command.subCarrier);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.pulseTimeUnitLsb.HasValue) ret.Add(command.pulseTimeUnitLsb);
                if (command.properties2.HasValue) ret.Add(command.properties2);
                if (command.reportNumberLsb.HasValue) ret.Add(command.reportNumberLsb);
                if (command.vg1 != null)
                {
                    foreach (var item in command.vg1)
                    {
                        if (item.properties1.HasValue) ret.Add(item.properties1);
                        if (item.dataBlockValue != null)
                        {
                            foreach (var tmp in item.dataBlockValue)
                            {
                                ret.Add(tmp);
                            }
                        }
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

