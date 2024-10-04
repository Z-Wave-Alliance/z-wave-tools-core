/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_TIME_PARAMETERS
    {
        public const byte ID = 0x8B;
        public const byte VERSION = 1;
        public partial class TIME_PARAMETERS_GET
        {
            public const byte ID = 0x02;
            public static implicit operator TIME_PARAMETERS_GET(byte[] data)
            {
                TIME_PARAMETERS_GET ret = new TIME_PARAMETERS_GET();
                return ret;
            }
            public static implicit operator byte[](TIME_PARAMETERS_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_TIME_PARAMETERS.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class TIME_PARAMETERS_REPORT
        {
            public const byte ID = 0x03;
            public const byte yearBytesCount = 2;
            public byte[] year = new byte[yearBytesCount];
            public ByteValue month = 0;
            public ByteValue day = 0;
            public ByteValue hourUtc = 0;
            public ByteValue minuteUtc = 0;
            public ByteValue secondUtc = 0;
            public static implicit operator TIME_PARAMETERS_REPORT(byte[] data)
            {
                TIME_PARAMETERS_REPORT ret = new TIME_PARAMETERS_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.year = (data.Length - index) >= yearBytesCount ? new byte[yearBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.year[0] = data[index++];
                    if (data.Length > index) ret.year[1] = data[index++];
                    ret.month = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.day = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.hourUtc = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.minuteUtc = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.secondUtc = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](TIME_PARAMETERS_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_TIME_PARAMETERS.ID);
                ret.Add(ID);
                if (command.year != null)
                {
                    foreach (var tmp in command.year)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.month.HasValue) ret.Add(command.month);
                if (command.day.HasValue) ret.Add(command.day);
                if (command.hourUtc.HasValue) ret.Add(command.hourUtc);
                if (command.minuteUtc.HasValue) ret.Add(command.minuteUtc);
                if (command.secondUtc.HasValue) ret.Add(command.secondUtc);
                return ret.ToArray();
            }
        }
        public partial class TIME_PARAMETERS_SET
        {
            public const byte ID = 0x01;
            public const byte yearBytesCount = 2;
            public byte[] year = new byte[yearBytesCount];
            public ByteValue month = 0;
            public ByteValue day = 0;
            public ByteValue hourUtc = 0;
            public ByteValue minuteUtc = 0;
            public ByteValue secondUtc = 0;
            public static implicit operator TIME_PARAMETERS_SET(byte[] data)
            {
                TIME_PARAMETERS_SET ret = new TIME_PARAMETERS_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.year = (data.Length - index) >= yearBytesCount ? new byte[yearBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.year[0] = data[index++];
                    if (data.Length > index) ret.year[1] = data[index++];
                    ret.month = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.day = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.hourUtc = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.minuteUtc = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.secondUtc = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](TIME_PARAMETERS_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_TIME_PARAMETERS.ID);
                ret.Add(ID);
                if (command.year != null)
                {
                    foreach (var tmp in command.year)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.month.HasValue) ret.Add(command.month);
                if (command.day.HasValue) ret.Add(command.day);
                if (command.hourUtc.HasValue) ret.Add(command.hourUtc);
                if (command.minuteUtc.HasValue) ret.Add(command.minuteUtc);
                if (command.secondUtc.HasValue) ret.Add(command.secondUtc);
                return ret.ToArray();
            }
        }
    }
}

