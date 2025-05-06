using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_CENTRAL_SCENE
    {
        public const byte ID = 0x5B;
        public const byte VERSION = 1;
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
                ret.Add(COMMAND_CLASS_CENTRAL_SCENE.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class CENTRAL_SCENE_SUPPORTED_REPORT
        {
            public const byte ID = 0x02;
            public ByteValue supportedScenes = 0;
            public static implicit operator CENTRAL_SCENE_SUPPORTED_REPORT(byte[] data)
            {
                CENTRAL_SCENE_SUPPORTED_REPORT ret = new CENTRAL_SCENE_SUPPORTED_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.supportedScenes = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](CENTRAL_SCENE_SUPPORTED_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_CENTRAL_SCENE.ID);
                ret.Add(ID);
                if (command.supportedScenes.HasValue) ret.Add(command.supportedScenes);
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
                    get { return (byte)(_value >> 3 & 0x1F); }
                    set { HasValue = true; _value &= 0xFF - 0xF8; _value += (byte)(value << 3 & 0xF8); }
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
                ret.Add(COMMAND_CLASS_CENTRAL_SCENE.ID);
                ret.Add(ID);
                if (command.sequenceNumber.HasValue) ret.Add(command.sequenceNumber);
                if (command.properties1.HasValue) ret.Add(command.properties1);
                if (command.sceneNumber.HasValue) ret.Add(command.sceneNumber);
                return ret.ToArray();
            }
        }
    }
}

