/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_AV_CONTENT_DIRECTORY_MD
    {
        public const byte ID = 0x95;
        public const byte VERSION = 1;
        public partial class AV_CONTENT_BROWSE_MD_BY_LETTER_GET
        {
            public const byte ID = 0x03;
            public static implicit operator AV_CONTENT_BROWSE_MD_BY_LETTER_GET(byte[] data)
            {
                AV_CONTENT_BROWSE_MD_BY_LETTER_GET ret = new AV_CONTENT_BROWSE_MD_BY_LETTER_GET();
                return ret;
            }
            public static implicit operator byte[](AV_CONTENT_BROWSE_MD_BY_LETTER_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_AV_CONTENT_DIRECTORY_MD.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class AV_CONTENT_BROWSE_MD_BY_LETTER_REPORT
        {
            public const byte ID = 0x04;
            public static implicit operator AV_CONTENT_BROWSE_MD_BY_LETTER_REPORT(byte[] data)
            {
                AV_CONTENT_BROWSE_MD_BY_LETTER_REPORT ret = new AV_CONTENT_BROWSE_MD_BY_LETTER_REPORT();
                return ret;
            }
            public static implicit operator byte[](AV_CONTENT_BROWSE_MD_BY_LETTER_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_AV_CONTENT_DIRECTORY_MD.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class AV_CONTENT_BROWSE_MD_CHILD_COUNT_GET
        {
            public const byte ID = 0x05;
            public static implicit operator AV_CONTENT_BROWSE_MD_CHILD_COUNT_GET(byte[] data)
            {
                AV_CONTENT_BROWSE_MD_CHILD_COUNT_GET ret = new AV_CONTENT_BROWSE_MD_CHILD_COUNT_GET();
                return ret;
            }
            public static implicit operator byte[](AV_CONTENT_BROWSE_MD_CHILD_COUNT_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_AV_CONTENT_DIRECTORY_MD.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class AV_CONTENT_BROWSE_MD_CHILD_COUNT_REPORT
        {
            public const byte ID = 0x06;
            public static implicit operator AV_CONTENT_BROWSE_MD_CHILD_COUNT_REPORT(byte[] data)
            {
                AV_CONTENT_BROWSE_MD_CHILD_COUNT_REPORT ret = new AV_CONTENT_BROWSE_MD_CHILD_COUNT_REPORT();
                return ret;
            }
            public static implicit operator byte[](AV_CONTENT_BROWSE_MD_CHILD_COUNT_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_AV_CONTENT_DIRECTORY_MD.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class AV_CONTENT_BROWSE_MD_GET
        {
            public const byte ID = 0x01;
            public static implicit operator AV_CONTENT_BROWSE_MD_GET(byte[] data)
            {
                AV_CONTENT_BROWSE_MD_GET ret = new AV_CONTENT_BROWSE_MD_GET();
                return ret;
            }
            public static implicit operator byte[](AV_CONTENT_BROWSE_MD_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_AV_CONTENT_DIRECTORY_MD.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class AV_CONTENT_BROWSE_MD_REPORT
        {
            public const byte ID = 0x02;
            public static implicit operator AV_CONTENT_BROWSE_MD_REPORT(byte[] data)
            {
                AV_CONTENT_BROWSE_MD_REPORT ret = new AV_CONTENT_BROWSE_MD_REPORT();
                return ret;
            }
            public static implicit operator byte[](AV_CONTENT_BROWSE_MD_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_AV_CONTENT_DIRECTORY_MD.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class AV_MATCH_ITEM_TO_RENDERER_MD_GET
        {
            public const byte ID = 0x07;
            public static implicit operator AV_MATCH_ITEM_TO_RENDERER_MD_GET(byte[] data)
            {
                AV_MATCH_ITEM_TO_RENDERER_MD_GET ret = new AV_MATCH_ITEM_TO_RENDERER_MD_GET();
                return ret;
            }
            public static implicit operator byte[](AV_MATCH_ITEM_TO_RENDERER_MD_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_AV_CONTENT_DIRECTORY_MD.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class AV_MATCH_ITEM_TO_RENDERER_MD_REPORT
        {
            public const byte ID = 0x08;
            public static implicit operator AV_MATCH_ITEM_TO_RENDERER_MD_REPORT(byte[] data)
            {
                AV_MATCH_ITEM_TO_RENDERER_MD_REPORT ret = new AV_MATCH_ITEM_TO_RENDERER_MD_REPORT();
                return ret;
            }
            public static implicit operator byte[](AV_MATCH_ITEM_TO_RENDERER_MD_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_AV_CONTENT_DIRECTORY_MD.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
    }
}

