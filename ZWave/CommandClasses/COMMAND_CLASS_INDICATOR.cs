using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_INDICATOR
    {
        public const byte ID = 0x87;
        public const byte VERSION = 1;
        public partial class INDICATOR_GET
        {
            public const byte ID = 0x02;
            public static implicit operator INDICATOR_GET(byte[] data)
            {
                INDICATOR_GET ret = new INDICATOR_GET();
                return ret;
            }
            public static implicit operator byte[](INDICATOR_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_INDICATOR.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class INDICATOR_REPORT
        {
            public const byte ID = 0x03;
            public ByteValue value = 0;
            public static implicit operator INDICATOR_REPORT(byte[] data)
            {
                INDICATOR_REPORT ret = new INDICATOR_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.value = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](INDICATOR_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_INDICATOR.ID);
                ret.Add(ID);
                if (command.value.HasValue) ret.Add(command.value);
                return ret.ToArray();
            }
        }
        public partial class INDICATOR_SET
        {
            public const byte ID = 0x01;
            public ByteValue value = 0;
            public static implicit operator INDICATOR_SET(byte[] data)
            {
                INDICATOR_SET ret = new INDICATOR_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.value = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](INDICATOR_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_INDICATOR.ID);
                ret.Add(ID);
                if (command.value.HasValue) ret.Add(command.value);
                return ret.ToArray();
            }
        }
    }
}

