/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_SCREEN_MD
    {
        public const byte ID = 0x92;
        public const byte VERSION = 1;
        public partial class SCREEN_MD_GET
        {
            public const byte ID = 0x01;
            public ByteValue numberOfReports = 0;
            public ByteValue nodeId = 0;
            public static implicit operator SCREEN_MD_GET(byte[] data)
            {
                SCREEN_MD_GET ret = new SCREEN_MD_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.numberOfReports = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SCREEN_MD_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SCREEN_MD.ID);
                ret.Add(ID);
                if (command.numberOfReports.HasValue) ret.Add(command.numberOfReports);
                if (command.nodeId.HasValue) ret.Add(command.nodeId);
                return ret.ToArray();
            }
        }
        public partial class SCREEN_MD_REPORT
        {
            public const byte ID = 0x02;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte charPresentation
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte screenSettings
                {
                    get { return (byte)(_value >> 3 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x38; _value += (byte)(value << 3 & 0x38); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte moreData
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
            public class TVG
            {
                public struct Tproperties1
                {
                    private byte _value;
                    public bool HasValue { get; private set; }
                    public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                    public byte lineNumber
                    {
                        get { return (byte)(_value >> 0 & 0x0F); }
                        set { HasValue = true; _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                    }
                    public byte clear
                    {
                        get { return (byte)(_value >> 4 & 0x01); }
                        set { HasValue = true; _value &= 0xFF - 0x10; _value += (byte)(value << 4 & 0x10); }
                    }
                    public byte lineSettings
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
                public ByteValue characterPosition = 0;
                public ByteValue numberOfCharacters = 0;
                public IList<byte> character = new List<byte>();
            }
            public List<TVG> vg = new List<TVG>();
            public static implicit operator SCREEN_MD_REPORT(byte[] data)
            {
                SCREEN_MD_REPORT ret = new SCREEN_MD_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.vg = new List<TVG>();
                    while (data.Length - 0 > index)
                    {
                        TVG tmp = new TVG();
                        tmp.properties1 = data.Length > index ? (TVG.Tproperties1)data[index++] : TVG.Tproperties1.Empty;
                        tmp.characterPosition = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.numberOfCharacters = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.character = new List<byte>();
                        for (int i = 0; i < tmp.numberOfCharacters; i++)
                        {
                            if (data.Length > index) tmp.character.Add(data[index++]);
                        }
                        ret.vg.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](SCREEN_MD_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SCREEN_MD.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.vg != null)
                {
                    foreach (var item in command.vg)
                    {
                        if (item.properties1.HasValue) ret.Add(item.properties1);
                        if (item.characterPosition.HasValue) ret.Add(item.characterPosition);
                        if (item.numberOfCharacters.HasValue) ret.Add(item.numberOfCharacters);
                        if (item.character != null)
                        {
                            foreach (var tmp in item.character)
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

