/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_SCENE_ACTUATOR_CONF
    {
        public const byte ID = 0x2C;
        public const byte VERSION = 1;
        public partial class SCENE_ACTUATOR_CONF_GET
        {
            public const byte ID = 0x02;
            public ByteValue sceneId = 0;
            public static implicit operator SCENE_ACTUATOR_CONF_GET(byte[] data)
            {
                SCENE_ACTUATOR_CONF_GET ret = new SCENE_ACTUATOR_CONF_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.sceneId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SCENE_ACTUATOR_CONF_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SCENE_ACTUATOR_CONF.ID);
                ret.Add(ID);
                if (command.sceneId.HasValue) ret.Add(command.sceneId);
                return ret.ToArray();
            }
        }
        public partial class SCENE_ACTUATOR_CONF_REPORT
        {
            public const byte ID = 0x03;
            public ByteValue sceneId = 0;
            public ByteValue level = 0;
            public ByteValue dimmingDuration = 0;
            public static implicit operator SCENE_ACTUATOR_CONF_REPORT(byte[] data)
            {
                SCENE_ACTUATOR_CONF_REPORT ret = new SCENE_ACTUATOR_CONF_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.sceneId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.level = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.dimmingDuration = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SCENE_ACTUATOR_CONF_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SCENE_ACTUATOR_CONF.ID);
                ret.Add(ID);
                if (command.sceneId.HasValue) ret.Add(command.sceneId);
                if (command.level.HasValue) ret.Add(command.level);
                if (command.dimmingDuration.HasValue) ret.Add(command.dimmingDuration);
                return ret.ToArray();
            }
        }
        public partial class SCENE_ACTUATOR_CONF_SET
        {
            public const byte ID = 0x01;
            public ByteValue sceneId = 0;
            public ByteValue dimmingDuration = 0;
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
                public byte moverride
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
            public ByteValue level = 0;
            public static implicit operator SCENE_ACTUATOR_CONF_SET(byte[] data)
            {
                SCENE_ACTUATOR_CONF_SET ret = new SCENE_ACTUATOR_CONF_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.sceneId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.dimmingDuration = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                    ret.level = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SCENE_ACTUATOR_CONF_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SCENE_ACTUATOR_CONF.ID);
                ret.Add(ID);
                if (command.sceneId.HasValue) ret.Add(command.sceneId);
                if (command.dimmingDuration.HasValue) ret.Add(command.dimmingDuration);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.level.HasValue) ret.Add(command.level);
                return ret.ToArray();
            }
        }
    }
}

