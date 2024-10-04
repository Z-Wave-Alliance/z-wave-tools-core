/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_AV_TAGGING_MD
    {
        public const byte ID = 0x99;
        public const byte VERSION = 1;
        public partial class AV_TAGGING_MD_GET
        {
            public const byte ID = 0x01;
            public static implicit operator AV_TAGGING_MD_GET(byte[] data)
            {
                AV_TAGGING_MD_GET ret = new AV_TAGGING_MD_GET();
                return ret;
            }
            public static implicit operator byte[](AV_TAGGING_MD_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_AV_TAGGING_MD.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class AV_TAGGING_MD_REPORT
        {
            public const byte ID = 0x02;
            public static implicit operator AV_TAGGING_MD_REPORT(byte[] data)
            {
                AV_TAGGING_MD_REPORT ret = new AV_TAGGING_MD_REPORT();
                return ret;
            }
            public static implicit operator byte[](AV_TAGGING_MD_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_AV_TAGGING_MD.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
    }
}

