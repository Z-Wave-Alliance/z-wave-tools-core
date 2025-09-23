/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_APPLICATION_CAPABILITY
    {
        public const byte ID = 0x57;
        public const byte VERSION = 1;
        public partial class COMMAND_COMMAND_CLASS_NOT_SUPPORTED
        {
            public const byte ID = 0x01;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte reserved
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte dynamic
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
            public ByteValue offendingCommandClass = 0;
            public ByteValue offendingCommand = 0;
            public static implicit operator COMMAND_COMMAND_CLASS_NOT_SUPPORTED(byte[] data)
            {
                COMMAND_COMMAND_CLASS_NOT_SUPPORTED ret = new COMMAND_COMMAND_CLASS_NOT_SUPPORTED();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.offendingCommandClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.offendingCommand = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](COMMAND_COMMAND_CLASS_NOT_SUPPORTED command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_APPLICATION_CAPABILITY.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.offendingCommandClass.HasValue) ret.Add(command.offendingCommandClass);
                if (command.offendingCommand.HasValue) ret.Add(command.offendingCommand);
                return ret.ToArray();
            }
        }
    }
}

