/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_CRC_16_ENCAP
    {
        public const byte ID = 0x56;
        public const byte VERSION = 1;
        public partial class CRC_16_ENCAP
        {
            public const byte ID = 0x01;
            public ByteValue commandClass = 0;
            public ByteValue command = 0;
            public IList<byte> data = new List<byte>();
            public const byte checksumBytesCount = 2;
            public byte[] checksum = new byte[checksumBytesCount];
            public static implicit operator CRC_16_ENCAP(byte[] data)
            {
                CRC_16_ENCAP ret = new CRC_16_ENCAP();
                if (data != null)
                {
                    int index = 2;
                    ret.commandClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.command = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.data = new List<byte>();
                    while (data.Length - 2 > index)
                    {
                        if (data.Length > index) ret.data.Add(data[index++]);
                    }
                    ret.checksum = (data.Length - index) >= checksumBytesCount ? new byte[checksumBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.checksum[0] = data[index++];
                    if (data.Length > index) ret.checksum[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](CRC_16_ENCAP command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_CRC_16_ENCAP.ID);
                ret.Add(ID);
                if (command.commandClass.HasValue) ret.Add(command.commandClass);
                if (command.command.HasValue) ret.Add(command.command);
                if (command.data != null)
                {
                    foreach (var tmp in command.data)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.checksum != null)
                {
                    foreach (var tmp in command.checksum)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

