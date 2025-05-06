using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_ANTITHEFT_UNLOCK
    {
        public const byte ID = 0x7E;
        public const byte VERSION = 1;
        public partial class ANTITHEFT_UNLOCK_STATE_GET
        {
            public const byte ID = 0x01;
            public static implicit operator ANTITHEFT_UNLOCK_STATE_GET(byte[] data)
            {
                ANTITHEFT_UNLOCK_STATE_GET ret = new ANTITHEFT_UNLOCK_STATE_GET();
                return ret;
            }
            public static implicit operator byte[](ANTITHEFT_UNLOCK_STATE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ANTITHEFT_UNLOCK.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class ANTITHEFT_UNLOCK_STATE_REPORT
        {
            public const byte ID = 0x02;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte state
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte restricted
                {
                    get { return (byte)(_value >> 1 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x02; _value += (byte)(value << 1 & 0x02); }
                }
                public byte antiTheftHintLength
                {
                    get { return (byte)(_value >> 2 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x3C; _value += (byte)(value << 2 & 0x3C); }
                }
                public byte reserved
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
            public IList<byte> antiTheftHint = new List<byte>();
            public const byte manufacturerIdBytesCount = 2;
            public byte[] manufacturerId = new byte[manufacturerIdBytesCount];
            public const byte zWaveAllianceLockingEntityIdBytesCount = 2;
            public byte[] zWaveAllianceLockingEntityId = new byte[zWaveAllianceLockingEntityIdBytesCount];
            public static implicit operator ANTITHEFT_UNLOCK_STATE_REPORT(byte[] data)
            {
                ANTITHEFT_UNLOCK_STATE_REPORT ret = new ANTITHEFT_UNLOCK_STATE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.antiTheftHint = new List<byte>();
                    for (int i = 0; i < ret.properties1.antiTheftHintLength; i++)
                    {
                        if (data.Length > index) ret.antiTheftHint.Add(data[index++]);
                    }
                    ret.manufacturerId = (data.Length - index) >= manufacturerIdBytesCount ? new byte[manufacturerIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.manufacturerId[0] = data[index++];
                    if (data.Length > index) ret.manufacturerId[1] = data[index++];
                    ret.zWaveAllianceLockingEntityId = (data.Length - index) >= zWaveAllianceLockingEntityIdBytesCount ? new byte[zWaveAllianceLockingEntityIdBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.zWaveAllianceLockingEntityId[0] = data[index++];
                    if (data.Length > index) ret.zWaveAllianceLockingEntityId[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](ANTITHEFT_UNLOCK_STATE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ANTITHEFT_UNLOCK.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.antiTheftHint != null)
                {
                    foreach (var tmp in command.antiTheftHint)
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
        public partial class ANTITHEFT_UNLOCK_SET
        {
            public const byte ID = 0x03;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte magicCodeLength
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
            public IList<byte> magicCode = new List<byte>();
            public static implicit operator ANTITHEFT_UNLOCK_SET(byte[] data)
            {
                ANTITHEFT_UNLOCK_SET ret = new ANTITHEFT_UNLOCK_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.magicCode = new List<byte>();
                    for (int i = 0; i < ret.properties1.magicCodeLength; i++)
                    {
                        if (data.Length > index) ret.magicCode.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](ANTITHEFT_UNLOCK_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ANTITHEFT_UNLOCK.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.magicCode != null)
                {
                    foreach (var tmp in command.magicCode)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

