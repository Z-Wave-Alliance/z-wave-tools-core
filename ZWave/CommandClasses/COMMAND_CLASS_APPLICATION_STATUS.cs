using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_APPLICATION_STATUS
    {
        public const byte ID = 0x22;
        public const byte VERSION = 1;
        public partial class APPLICATION_BUSY
        {
            public const byte ID = 0x01;
            public ByteValue status = 0;
            public ByteValue waitTime = 0;
            public static implicit operator APPLICATION_BUSY(byte[] data)
            {
                APPLICATION_BUSY ret = new APPLICATION_BUSY();
                if (data != null)
                {
                    int index = 2;
                    ret.status = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.waitTime = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](APPLICATION_BUSY command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_APPLICATION_STATUS.ID);
                ret.Add(ID);
                if (command.status.HasValue) ret.Add(command.status);
                if (command.waitTime.HasValue) ret.Add(command.waitTime);
                return ret.ToArray();
            }
        }
        public partial class APPLICATION_REJECTED_REQUEST
        {
            public const byte ID = 0x02;
            public ByteValue status = 0;
            public static implicit operator APPLICATION_REJECTED_REQUEST(byte[] data)
            {
                APPLICATION_REJECTED_REQUEST ret = new APPLICATION_REJECTED_REQUEST();
                if (data != null)
                {
                    int index = 2;
                    ret.status = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](APPLICATION_REJECTED_REQUEST command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_APPLICATION_STATUS.ID);
                ret.Add(ID);
                if (command.status.HasValue) ret.Add(command.status);
                return ret.ToArray();
            }
        }
    }
}

