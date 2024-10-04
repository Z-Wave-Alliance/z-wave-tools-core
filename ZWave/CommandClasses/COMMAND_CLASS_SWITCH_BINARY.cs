/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_SWITCH_BINARY
    {
        public const byte ID = 0x25;
        public const byte VERSION = 1;
        public partial class SWITCH_BINARY_GET
        {
            public const byte ID = 0x02;
            public static implicit operator SWITCH_BINARY_GET(byte[] data)
            {
                SWITCH_BINARY_GET ret = new SWITCH_BINARY_GET();
                return ret;
            }
            public static implicit operator byte[](SWITCH_BINARY_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SWITCH_BINARY.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class SWITCH_BINARY_REPORT
        {
            public const byte ID = 0x03;
            public ByteValue value = 0;
            public static implicit operator SWITCH_BINARY_REPORT(byte[] data)
            {
                SWITCH_BINARY_REPORT ret = new SWITCH_BINARY_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.value = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SWITCH_BINARY_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SWITCH_BINARY.ID);
                ret.Add(ID);
                if (command.value.HasValue) ret.Add(command.value);
                return ret.ToArray();
            }
        }
        public partial class SWITCH_BINARY_SET
        {
            public const byte ID = 0x01;
            public ByteValue switchValue = 0;
            public static implicit operator SWITCH_BINARY_SET(byte[] data)
            {
                SWITCH_BINARY_SET ret = new SWITCH_BINARY_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.switchValue = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SWITCH_BINARY_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SWITCH_BINARY.ID);
                ret.Add(ID);
                if (command.switchValue.HasValue) ret.Add(command.switchValue);
                return ret.ToArray();
            }
        }
    }
}

