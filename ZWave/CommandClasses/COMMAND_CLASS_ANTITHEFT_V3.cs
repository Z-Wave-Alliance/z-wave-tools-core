/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_ANTITHEFT_V3
    {
        public const byte ID = 0x5D;
        public const byte VERSION = 3;
        public partial class ANTITHEFT_SET
        {
            public const byte ID = 0x01;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte numberOfMagicCodeBytes
                {
                    get { return (byte)(_value >> 0 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0x7F; _value += (byte)(value << 0 & 0x7F); }
                }
                public byte enable
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
            public IList<byte> magicCode = new List<byte>();
            public const byte manufacturerIdBytesCount = 2;
            public byte[] manufacturerId = new byte[manufacturerIdBytesCount];
            public ByteValue antiTheftHintNumberBytes = 0;
            public IList<byte> antiTheftHintByte = new List<byte>();
            public const byte zWaveAllianceLockingEntityIdBytesCount = 2;
            public byte[] zWaveAllianceLockingEntityId = new byte[zWaveAllianceLockingEntityIdBytesCount];
            public static implicit operator ANTITHEFT_SET(byte[] data)
            {
                ANTITHEFT_SET ret = new ANTITHEFT_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.magicCode = new List<byte>();
                    for (int i = 0; i < ret.properties1.numberOfMagicCodeBytes; i++)
                    {
                        if (data.Length > index) ret.magicCode.Add(data[index++]);
                    }
                    ret.manufacturerId = (data.Length - index) >= manufacturerIdBytesCount ? new byte[manufacturerIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.manufacturerId[0] = data[index++];
                    if (data.Length > index) ret.manufacturerId[1] = data[index++];
                    ret.antiTheftHintNumberBytes = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.antiTheftHintByte = new List<byte>();
                    for (int i = 0; i < ret.antiTheftHintNumberBytes; i++)
                    {
                        if (data.Length > index) ret.antiTheftHintByte.Add(data[index++]);
                    }
                    ret.zWaveAllianceLockingEntityId = (data.Length - index) >= zWaveAllianceLockingEntityIdBytesCount ? new byte[zWaveAllianceLockingEntityIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.zWaveAllianceLockingEntityId[0] = data[index++];
                    if (data.Length > index) ret.zWaveAllianceLockingEntityId[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](ANTITHEFT_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ANTITHEFT_V3.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.magicCode != null)
                {
                    foreach (var tmp in command.magicCode)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.manufacturerId != null)
                {
                    foreach (var tmp in command.manufacturerId)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.antiTheftHintNumberBytes.HasValue) ret.Add(command.antiTheftHintNumberBytes);
                if (command.antiTheftHintByte != null)
                {
                    foreach (var tmp in command.antiTheftHintByte)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.zWaveAllianceLockingEntityId != null)
                {
                    foreach (var tmp in command.zWaveAllianceLockingEntityId)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class ANTITHEFT_GET
        {
            public const byte ID = 0x02;
            public static implicit operator ANTITHEFT_GET(byte[] data)
            {
                ANTITHEFT_GET ret = new ANTITHEFT_GET();
                return ret;
            }
            public static implicit operator byte[](ANTITHEFT_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ANTITHEFT_V3.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class ANTITHEFT_REPORT
        {
            public const byte ID = 0x03;
            public ByteValue antiTheftProtectionStatus = 0;
            public const byte manufacturerIdBytesCount = 2;
            public byte[] manufacturerId = new byte[manufacturerIdBytesCount];
            public ByteValue antiTheftHintNumberBytes = 0;
            public IList<byte> antiTheftHintByte = new List<byte>();
            public const byte zWaveAllianceLockingEntityIdBytesCount = 2;
            public byte[] zWaveAllianceLockingEntityId = new byte[zWaveAllianceLockingEntityIdBytesCount];
            public static implicit operator ANTITHEFT_REPORT(byte[] data)
            {
                ANTITHEFT_REPORT ret = new ANTITHEFT_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.antiTheftProtectionStatus = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.manufacturerId = (data.Length - index) >= manufacturerIdBytesCount ? new byte[manufacturerIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.manufacturerId[0] = data[index++];
                    if (data.Length > index) ret.manufacturerId[1] = data[index++];
                    ret.antiTheftHintNumberBytes = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.antiTheftHintByte = new List<byte>();
                    for (int i = 0; i < ret.antiTheftHintNumberBytes; i++)
                    {
                        if (data.Length > index) ret.antiTheftHintByte.Add(data[index++]);
                    }
                    ret.zWaveAllianceLockingEntityId = (data.Length - index) >= zWaveAllianceLockingEntityIdBytesCount ? new byte[zWaveAllianceLockingEntityIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.zWaveAllianceLockingEntityId[0] = data[index++];
                    if (data.Length > index) ret.zWaveAllianceLockingEntityId[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](ANTITHEFT_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ANTITHEFT_V3.ID);
                ret.Add(ID);
                if (command.antiTheftProtectionStatus.HasValue) ret.Add(command.antiTheftProtectionStatus);
                if (command.manufacturerId != null)
                {
                    foreach (var tmp in command.manufacturerId)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.antiTheftHintNumberBytes.HasValue) ret.Add(command.antiTheftHintNumberBytes);
                if (command.antiTheftHintByte != null)
                {
                    foreach (var tmp in command.antiTheftHintByte)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.zWaveAllianceLockingEntityId != null)
                {
                    foreach (var tmp in command.zWaveAllianceLockingEntityId)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

