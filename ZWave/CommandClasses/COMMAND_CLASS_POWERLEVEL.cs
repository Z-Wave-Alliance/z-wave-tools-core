/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_POWERLEVEL
    {
        public const byte ID = 0x73;
        public const byte VERSION = 1;
        public partial class POWERLEVEL_GET
        {
            public const byte ID = 0x02;
            public static implicit operator POWERLEVEL_GET(byte[] data)
            {
                POWERLEVEL_GET ret = new POWERLEVEL_GET();
                return ret;
            }
            public static implicit operator byte[](POWERLEVEL_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_POWERLEVEL.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class POWERLEVEL_REPORT
        {
            public const byte ID = 0x03;
            public ByteValue powerLevel = 0;
            public ByteValue timeout = 0;
            public static implicit operator POWERLEVEL_REPORT(byte[] data)
            {
                POWERLEVEL_REPORT ret = new POWERLEVEL_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.powerLevel = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.timeout = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](POWERLEVEL_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_POWERLEVEL.ID);
                ret.Add(ID);
                if (command.powerLevel.HasValue) ret.Add(command.powerLevel);
                if (command.timeout.HasValue) ret.Add(command.timeout);
                return ret.ToArray();
            }
        }
        public partial class POWERLEVEL_SET
        {
            public const byte ID = 0x01;
            public ByteValue powerLevel = 0;
            public ByteValue timeout = 0;
            public static implicit operator POWERLEVEL_SET(byte[] data)
            {
                POWERLEVEL_SET ret = new POWERLEVEL_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.powerLevel = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.timeout = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](POWERLEVEL_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_POWERLEVEL.ID);
                ret.Add(ID);
                if (command.powerLevel.HasValue) ret.Add(command.powerLevel);
                if (command.timeout.HasValue) ret.Add(command.timeout);
                return ret.ToArray();
            }
        }
        public partial class POWERLEVEL_TEST_NODE_GET
        {
            public const byte ID = 0x05;
            public static implicit operator POWERLEVEL_TEST_NODE_GET(byte[] data)
            {
                POWERLEVEL_TEST_NODE_GET ret = new POWERLEVEL_TEST_NODE_GET();
                return ret;
            }
            public static implicit operator byte[](POWERLEVEL_TEST_NODE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_POWERLEVEL.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class POWERLEVEL_TEST_NODE_REPORT
        {
            public const byte ID = 0x06;
            public ByteValue testNodeid = 0;
            public ByteValue statusOfOperation = 0;
            public const byte testFrameCountBytesCount = 2;
            public byte[] testFrameCount = new byte[testFrameCountBytesCount];
            public static implicit operator POWERLEVEL_TEST_NODE_REPORT(byte[] data)
            {
                POWERLEVEL_TEST_NODE_REPORT ret = new POWERLEVEL_TEST_NODE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.testNodeid = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.statusOfOperation = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.testFrameCount = (data.Length - index) >= testFrameCountBytesCount ? new byte[testFrameCountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.testFrameCount[0] = data[index++];
                    if (data.Length > index) ret.testFrameCount[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](POWERLEVEL_TEST_NODE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_POWERLEVEL.ID);
                ret.Add(ID);
                if (command.testNodeid.HasValue) ret.Add(command.testNodeid);
                if (command.statusOfOperation.HasValue) ret.Add(command.statusOfOperation);
                if (command.testFrameCount != null)
                {
                    foreach (var tmp in command.testFrameCount)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class POWERLEVEL_TEST_NODE_SET
        {
            public const byte ID = 0x04;
            public ByteValue testNodeid = 0;
            public ByteValue powerLevel = 0;
            public const byte testFrameCountBytesCount = 2;
            public byte[] testFrameCount = new byte[testFrameCountBytesCount];
            public static implicit operator POWERLEVEL_TEST_NODE_SET(byte[] data)
            {
                POWERLEVEL_TEST_NODE_SET ret = new POWERLEVEL_TEST_NODE_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.testNodeid = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.powerLevel = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.testFrameCount = (data.Length - index) >= testFrameCountBytesCount ? new byte[testFrameCountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.testFrameCount[0] = data[index++];
                    if (data.Length > index) ret.testFrameCount[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](POWERLEVEL_TEST_NODE_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_POWERLEVEL.ID);
                ret.Add(ID);
                if (command.testNodeid.HasValue) ret.Add(command.testNodeid);
                if (command.powerLevel.HasValue) ret.Add(command.powerLevel);
                if (command.testFrameCount != null)
                {
                    foreach (var tmp in command.testFrameCount)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

