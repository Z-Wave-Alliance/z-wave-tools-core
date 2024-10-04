/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_CENTRAL_SCENE_V3
    {
        public const byte ID = 0x5B;
        public const byte VERSION = 3;
        public partial class CENTRAL_SCENE_SUPPORTED_GET
        {
            public const byte ID = 0x01;
            public static implicit operator CENTRAL_SCENE_SUPPORTED_GET(byte[] data)
            {
                CENTRAL_SCENE_SUPPORTED_GET ret = new CENTRAL_SCENE_SUPPORTED_GET();
                return ret;
            }
            public static implicit operator byte[](CENTRAL_SCENE_SUPPORTED_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_CENTRAL_SCENE_V3.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class CENTRAL_SCENE_SUPPORTED_REPORT
        {
            public const byte ID = 0x02;
            public ByteValue supportedScenes = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte identical
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte numberOfBitMaskBytes
                {
                    get { return (byte)(_value >> 1 & 0x03); }
                    set { HasValue = true; _value &= 0xFF - 0x06; _value += (byte)(value << 1 & 0x06); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 3 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x78; _value += (byte)(value << 3 & 0x78); }
                }
                public byte slowRefreshSupport
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
            public class TVG1
            {
                public IList<byte> supportedKeyAttributesForScene = new List<byte>();
            }
            public List<TVG1> vg1 = new List<TVG1>();
            public static implicit operator CENTRAL_SCENE_SUPPORTED_REPORT(byte[] data)
            {
                CENTRAL_SCENE_SUPPORTED_REPORT ret = new CENTRAL_SCENE_SUPPORTED_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.supportedScenes = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.vg1 = new List<TVG1>();
                    for (int j = 0; j < ret.supportedScenes; j++)
                    {
                        TVG1 tmp = new TVG1();
                        tmp.supportedKeyAttributesForScene = new List<byte>();
                        for (int i = 0; i < ret.properties1.numberOfBitMaskBytes; i++)
                        {
                            if (data.Length > index) tmp.supportedKeyAttributesForScene.Add(data[index++]);
                        }
                        ret.vg1.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](CENTRAL_SCENE_SUPPORTED_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_CENTRAL_SCENE_V3.ID);
                ret.Add(ID);
                if (command.supportedScenes.HasValue) ret.Add(command.supportedScenes);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.vg1 != null)
                {
                    foreach (var item in command.vg1)
                    {
                        if (item.supportedKeyAttributesForScene != null)
                        {
                            foreach (var tmp in item.supportedKeyAttributesForScene)
                            {
                                ret.Add(tmp);
                            }
                        }
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class CENTRAL_SCENE_NOTIFICATION
        {
            public const byte ID = 0x03;
            public ByteValue sequenceNumber = 0;
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte keyAttributes
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { HasValue = true; _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 3 & 0x0F); }
                    set { HasValue = true; _value &= 0xFF - 0x78; _value += (byte)(value << 3 & 0x78); }
                }
                public byte slowRefresh
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
            public ByteValue sceneNumber = 0;
            public static implicit operator CENTRAL_SCENE_NOTIFICATION(byte[] data)
            {
                CENTRAL_SCENE_NOTIFICATION ret = new CENTRAL_SCENE_NOTIFICATION();
                if (data != null)
                {
                    int index = 2;
                    ret.sequenceNumber = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.sceneNumber = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](CENTRAL_SCENE_NOTIFICATION command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_CENTRAL_SCENE_V3.ID);
                ret.Add(ID);
                if (command.sequenceNumber.HasValue) ret.Add(command.sequenceNumber);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.sceneNumber.HasValue) ret.Add(command.sceneNumber);
                return ret.ToArray();
            }
        }
        public partial class CENTRAL_SCENE_CONFIGURATION_SET
        {
            public const byte ID = 0x04;
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
                public byte slowRefresh
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
            public static implicit operator CENTRAL_SCENE_CONFIGURATION_SET(byte[] data)
            {
                CENTRAL_SCENE_CONFIGURATION_SET ret = new CENTRAL_SCENE_CONFIGURATION_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](CENTRAL_SCENE_CONFIGURATION_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_CENTRAL_SCENE_V3.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                return ret.ToArray();
            }
        }
        public partial class CENTRAL_SCENE_CONFIGURATION_GET
        {
            public const byte ID = 0x05;
            public static implicit operator CENTRAL_SCENE_CONFIGURATION_GET(byte[] data)
            {
                CENTRAL_SCENE_CONFIGURATION_GET ret = new CENTRAL_SCENE_CONFIGURATION_GET();
                return ret;
            }
            public static implicit operator byte[](CENTRAL_SCENE_CONFIGURATION_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_CENTRAL_SCENE_V3.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class CENTRAL_SCENE_CONFIGURATION_REPORT
        {
            public const byte ID = 0x06;
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
                public byte slowRefresh
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
            public static implicit operator CENTRAL_SCENE_CONFIGURATION_REPORT(byte[] data)
            {
                CENTRAL_SCENE_CONFIGURATION_REPORT ret = new CENTRAL_SCENE_CONFIGURATION_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](CENTRAL_SCENE_CONFIGURATION_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_CENTRAL_SCENE_V3.ID);
                ret.Add(ID);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                return ret.ToArray();
            }
        }
    }
}

