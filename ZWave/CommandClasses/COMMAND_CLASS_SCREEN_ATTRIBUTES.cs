/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_SCREEN_ATTRIBUTES
    {
        public const byte ID = 0x93;
        public const byte VERSION = 1;
        public partial class SCREEN_ATTRIBUTES_GET
        {
            public const byte ID = 0x01;
            public static implicit operator SCREEN_ATTRIBUTES_GET(byte[] data)
            {
                SCREEN_ATTRIBUTES_GET ret = new SCREEN_ATTRIBUTES_GET();
                return ret;
            }
            public static implicit operator byte[](SCREEN_ATTRIBUTES_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SCREEN_ATTRIBUTES.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class SCREEN_ATTRIBUTES_REPORT
        {
            public const byte ID = 0x02;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte numberOfLines
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
            public ByteValue numberOfCharactersPerLine = 0;
            public ByteValue sizeOfLineBuffer = 0;
            public ByteValue numericalPresentationOfACharacter = 0;
            public static implicit operator SCREEN_ATTRIBUTES_REPORT(byte[] data)
            {
                SCREEN_ATTRIBUTES_REPORT ret = new SCREEN_ATTRIBUTES_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.numberOfCharactersPerLine = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.sizeOfLineBuffer = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.numericalPresentationOfACharacter = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SCREEN_ATTRIBUTES_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SCREEN_ATTRIBUTES.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.numberOfCharactersPerLine.HasValue) ret.Add(command.numberOfCharactersPerLine);
                if (command.sizeOfLineBuffer.HasValue) ret.Add(command.sizeOfLineBuffer);
                if (command.numericalPresentationOfACharacter.HasValue) ret.Add(command.numericalPresentationOfACharacter);
                return ret.ToArray();
            }
        }
    }
}

