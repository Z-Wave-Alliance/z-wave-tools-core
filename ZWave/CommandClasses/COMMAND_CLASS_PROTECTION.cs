/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_PROTECTION
    {
        public const byte ID = 0x75;
        public const byte VERSION = 1;
        public partial class PROTECTION_GET
        {
            public const byte ID = 0x02;
            public static implicit operator PROTECTION_GET(byte[] data)
            {
                PROTECTION_GET ret = new PROTECTION_GET();
                return ret;
            }
            public static implicit operator byte[](PROTECTION_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_PROTECTION.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class PROTECTION_REPORT
        {
            public const byte ID = 0x03;
            public ByteValue protectionState = 0;
            public static implicit operator PROTECTION_REPORT(byte[] data)
            {
                PROTECTION_REPORT ret = new PROTECTION_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.protectionState = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](PROTECTION_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_PROTECTION.ID);
                ret.Add(ID);
                if (command.protectionState.HasValue) ret.Add(command.protectionState);
                return ret.ToArray();
            }
        }
        public partial class PROTECTION_SET
        {
            public const byte ID = 0x01;
            public ByteValue protectionState = 0;
            public static implicit operator PROTECTION_SET(byte[] data)
            {
                PROTECTION_SET ret = new PROTECTION_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.protectionState = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](PROTECTION_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_PROTECTION.ID);
                ret.Add(ID);
                if (command.protectionState.HasValue) ret.Add(command.protectionState);
                return ret.ToArray();
            }
        }
    }
}

