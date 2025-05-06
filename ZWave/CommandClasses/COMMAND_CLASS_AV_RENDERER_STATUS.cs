using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_AV_RENDERER_STATUS
    {
        public const byte ID = 0x96;
        public const byte VERSION = 1;
        public partial class AV_RENDERER_STATUS_GET
        {
            public const byte ID = 0x01;
            public static implicit operator AV_RENDERER_STATUS_GET(byte[] data)
            {
                AV_RENDERER_STATUS_GET ret = new AV_RENDERER_STATUS_GET();
                return ret;
            }
            public static implicit operator byte[](AV_RENDERER_STATUS_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_AV_RENDERER_STATUS.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class AV_RENDERER_STATUS_REPORT
        {
            public const byte ID = 0x02;
            public static implicit operator AV_RENDERER_STATUS_REPORT(byte[] data)
            {
                AV_RENDERER_STATUS_REPORT ret = new AV_RENDERER_STATUS_REPORT();
                return ret;
            }
            public static implicit operator byte[](AV_RENDERER_STATUS_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_AV_RENDERER_STATUS.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
    }
}

