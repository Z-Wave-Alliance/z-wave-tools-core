/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_ZWAVEPLUS_INFO
    {
        public const byte ID = 0x5E;
        public const byte VERSION = 1;
        public partial class ZWAVEPLUS_INFO_GET
        {
            public const byte ID = 0x01;
            public static implicit operator ZWAVEPLUS_INFO_GET(byte[] data)
            {
                ZWAVEPLUS_INFO_GET ret = new ZWAVEPLUS_INFO_GET();
                return ret;
            }
            public static implicit operator byte[](ZWAVEPLUS_INFO_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZWAVEPLUS_INFO.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class ZWAVEPLUS_INFO_REPORT
        {
            public const byte ID = 0x02;
            public ByteValue zWaveVersion = 0;
            public ByteValue roleType = 0;
            public ByteValue nodeType = 0;
            public static implicit operator ZWAVEPLUS_INFO_REPORT(byte[] data)
            {
                ZWAVEPLUS_INFO_REPORT ret = new ZWAVEPLUS_INFO_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.zWaveVersion = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.roleType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nodeType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](ZWAVEPLUS_INFO_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZWAVEPLUS_INFO.ID);
                ret.Add(ID);
                if (command.zWaveVersion.HasValue) ret.Add(command.zWaveVersion);
                if (command.roleType.HasValue) ret.Add(command.roleType);
                if (command.nodeType.HasValue) ret.Add(command.nodeType);
                return ret.ToArray();
            }
        }
    }
}

