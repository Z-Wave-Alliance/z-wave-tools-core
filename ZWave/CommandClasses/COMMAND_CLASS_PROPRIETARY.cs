using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_PROPRIETARY
    {
        public const byte ID = 0x88;
        public const byte VERSION = 1;
        public partial class PROPRIETARY_GET
        {
            public const byte ID = 0x02;
            public IList<byte> data = new List<byte>();
            public static implicit operator PROPRIETARY_GET(byte[] data)
            {
                PROPRIETARY_GET ret = new PROPRIETARY_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.data = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.data.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](PROPRIETARY_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_PROPRIETARY.ID);
                ret.Add(ID);
                if (command.data != null)
                {
                    foreach (var tmp in command.data)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class PROPRIETARY_REPORT
        {
            public const byte ID = 0x03;
            public IList<byte> data = new List<byte>();
            public static implicit operator PROPRIETARY_REPORT(byte[] data)
            {
                PROPRIETARY_REPORT ret = new PROPRIETARY_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.data = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.data.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](PROPRIETARY_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_PROPRIETARY.ID);
                ret.Add(ID);
                if (command.data != null)
                {
                    foreach (var tmp in command.data)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class PROPRIETARY_SET
        {
            public const byte ID = 0x01;
            public IList<byte> data = new List<byte>();
            public static implicit operator PROPRIETARY_SET(byte[] data)
            {
                PROPRIETARY_SET ret = new PROPRIETARY_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.data = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.data.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](PROPRIETARY_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_PROPRIETARY.ID);
                ret.Add(ID);
                if (command.data != null)
                {
                    foreach (var tmp in command.data)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

