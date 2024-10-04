/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_PREPAYMENT_ENCAPSULATION
    {
        public const byte ID = 0x41;
        public const byte VERSION = 1;
        public partial class CMD_ENCAPSULATION
        {
            public const byte ID = 0x01;
            public IList<byte> data = new List<byte>();
            public static implicit operator CMD_ENCAPSULATION(byte[] data)
            {
                CMD_ENCAPSULATION ret = new CMD_ENCAPSULATION();
                if (data != null)
                {
                    int index = 2;
                    ret.data = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.data.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](CMD_ENCAPSULATION command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_PREPAYMENT_ENCAPSULATION.ID);
                ret.Add(ID);
                if (command.data != null)
                {
                    foreach (var tmp in command.data)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

