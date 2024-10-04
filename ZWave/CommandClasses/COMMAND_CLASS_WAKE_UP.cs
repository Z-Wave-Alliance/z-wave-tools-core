/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_WAKE_UP
    {
        public const byte ID = 0x84;
        public const byte VERSION = 1;
        public partial class WAKE_UP_INTERVAL_GET
        {
            public const byte ID = 0x05;
            public static implicit operator WAKE_UP_INTERVAL_GET(byte[] data)
            {
                WAKE_UP_INTERVAL_GET ret = new WAKE_UP_INTERVAL_GET();
                return ret;
            }
            public static implicit operator byte[](WAKE_UP_INTERVAL_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_WAKE_UP.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class WAKE_UP_INTERVAL_REPORT
        {
            public const byte ID = 0x06;
            public const byte secondsBytesCount = 3;
            public byte[] seconds = new byte[secondsBytesCount];
            public ByteValue nodeid = 0;
            public static implicit operator WAKE_UP_INTERVAL_REPORT(byte[] data)
            {
                WAKE_UP_INTERVAL_REPORT ret = new WAKE_UP_INTERVAL_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.seconds = (data.Length - index) >= secondsBytesCount ? new byte[secondsBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.seconds[0] = data[index++];
                    if (data.Length > index) ret.seconds[1] = data[index++];
                    if (data.Length > index) ret.seconds[2] = data[index++];
                    ret.nodeid = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](WAKE_UP_INTERVAL_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_WAKE_UP.ID);
                ret.Add(ID);
                if (command.seconds != null)
                {
                    foreach (var tmp in command.seconds)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.nodeid.HasValue) ret.Add(command.nodeid);
                return ret.ToArray();
            }
        }
        public partial class WAKE_UP_INTERVAL_SET
        {
            public const byte ID = 0x04;
            public const byte secondsBytesCount = 3;
            public byte[] seconds = new byte[secondsBytesCount];
            public ByteValue nodeid = 0;
            public static implicit operator WAKE_UP_INTERVAL_SET(byte[] data)
            {
                WAKE_UP_INTERVAL_SET ret = new WAKE_UP_INTERVAL_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.seconds = (data.Length - index) >= secondsBytesCount ? new byte[secondsBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.seconds[0] = data[index++];
                    if (data.Length > index) ret.seconds[1] = data[index++];
                    if (data.Length > index) ret.seconds[2] = data[index++];
                    ret.nodeid = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](WAKE_UP_INTERVAL_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_WAKE_UP.ID);
                ret.Add(ID);
                if (command.seconds != null)
                {
                    foreach (var tmp in command.seconds)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.nodeid.HasValue) ret.Add(command.nodeid);
                return ret.ToArray();
            }
        }
        public partial class WAKE_UP_NO_MORE_INFORMATION
        {
            public const byte ID = 0x08;
            public static implicit operator WAKE_UP_NO_MORE_INFORMATION(byte[] data)
            {
                WAKE_UP_NO_MORE_INFORMATION ret = new WAKE_UP_NO_MORE_INFORMATION();
                return ret;
            }
            public static implicit operator byte[](WAKE_UP_NO_MORE_INFORMATION command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_WAKE_UP.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class WAKE_UP_NOTIFICATION
        {
            public const byte ID = 0x07;
            public static implicit operator WAKE_UP_NOTIFICATION(byte[] data)
            {
                WAKE_UP_NOTIFICATION ret = new WAKE_UP_NOTIFICATION();
                return ret;
            }
            public static implicit operator byte[](WAKE_UP_NOTIFICATION command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_WAKE_UP.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
    }
}

