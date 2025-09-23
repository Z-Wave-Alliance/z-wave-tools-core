/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_SWITCH_BINARY_V2
    {
        public const byte ID = 0x25;
        public const byte VERSION = 2;
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
                ret.Add(COMMAND_CLASS_SWITCH_BINARY_V2.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class SWITCH_BINARY_REPORT
        {
            public const byte ID = 0x03;
            public ByteValue currentValue = 0;
            public ByteValue targetValue = 0;
            public ByteValue duration = 0;
            public static implicit operator SWITCH_BINARY_REPORT(byte[] data)
            {
                SWITCH_BINARY_REPORT ret = new SWITCH_BINARY_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.currentValue = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.targetValue = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.duration = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SWITCH_BINARY_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SWITCH_BINARY_V2.ID);
                ret.Add(ID);
                if (command.currentValue.HasValue) ret.Add(command.currentValue);
                if (command.targetValue.HasValue) ret.Add(command.targetValue);
                if (command.duration.HasValue) ret.Add(command.duration);
                return ret.ToArray();
            }
        }
        public partial class SWITCH_BINARY_SET
        {
            public const byte ID = 0x01;
            public ByteValue targetValue = 0;
            public ByteValue duration = 0;
            public static implicit operator SWITCH_BINARY_SET(byte[] data)
            {
                SWITCH_BINARY_SET ret = new SWITCH_BINARY_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.targetValue = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.duration = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SWITCH_BINARY_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SWITCH_BINARY_V2.ID);
                ret.Add(ID);
                if (command.targetValue.HasValue) ret.Add(command.targetValue);
                if (command.duration.HasValue) ret.Add(command.duration);
                return ret.ToArray();
            }
        }
    }
}

