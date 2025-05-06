using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_BASIC
    {
        public const byte ID = 0x20;
        public const byte VERSION = 1;
        public partial class BASIC_GET
        {
            public const byte ID = 0x02;
            public static implicit operator BASIC_GET(byte[] data)
            {
                BASIC_GET ret = new BASIC_GET();
                return ret;
            }
            public static implicit operator byte[](BASIC_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_BASIC.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class BASIC_REPORT
        {
            public const byte ID = 0x03;
            public ByteValue value = 0;
            public static implicit operator BASIC_REPORT(byte[] data)
            {
                BASIC_REPORT ret = new BASIC_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.value = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](BASIC_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_BASIC.ID);
                ret.Add(ID);
                if (command.value.HasValue) ret.Add(command.value);
                return ret.ToArray();
            }
        }
        public partial class BASIC_SET
        {
            public const byte ID = 0x01;
            public ByteValue value = 0;
            public static implicit operator BASIC_SET(byte[] data)
            {
                BASIC_SET ret = new BASIC_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.value = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](BASIC_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_BASIC.ID);
                ret.Add(ID);
                if (command.value.HasValue) ret.Add(command.value);
                return ret.ToArray();
            }
        }
    }
}

