/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_MULTI_CMD
    {
        public const byte ID = 0x8F;
        public const byte VERSION = 1;
        public partial class MULTI_CMD_ENCAP
        {
            public const byte ID = 0x01;
            public ByteValue numberOfCommands = 0;
            public class TENCAPSULATEDCOMMAND
            {
                public ByteValue commandLength = 0;
                public ByteValue commandClass = 0;
                public ByteValue command = 0;
                public IList<byte> data = new List<byte>();
            }
            public List<TENCAPSULATEDCOMMAND> encapsulatedCommand = new List<TENCAPSULATEDCOMMAND>();
            public static implicit operator MULTI_CMD_ENCAP(byte[] data)
            {
                MULTI_CMD_ENCAP ret = new MULTI_CMD_ENCAP();
                if (data != null)
                {
                    int index = 2;
                    ret.numberOfCommands = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.encapsulatedCommand = new List<TENCAPSULATEDCOMMAND>();
                    for (int j = 0; j < ret.numberOfCommands; j++)
                    {
                        TENCAPSULATEDCOMMAND tmp = new TENCAPSULATEDCOMMAND();
                        tmp.commandLength = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.commandClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.command = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.data = new List<byte>();
                        for (int i = 0; i < tmp.commandLength - 2; i++)
                        {
                            if (data.Length > index) tmp.data.Add(data[index++]);
                        }
                        ret.encapsulatedCommand.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](MULTI_CMD_ENCAP command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_MULTI_CMD.ID);
                ret.Add(ID);
                if (command.numberOfCommands.HasValue) ret.Add(command.numberOfCommands);
                if (command.encapsulatedCommand != null)
                {
                    foreach (var item in command.encapsulatedCommand)
                    {
                        if (item.commandLength.HasValue) ret.Add(item.commandLength);
                        if (item.commandClass.HasValue) ret.Add(item.commandClass);
                        if (item.command.HasValue) ret.Add(item.command);
                        if (item.data != null)
                        {
                            foreach (var tmp in item.data)
                            {
                                ret.Add(tmp);
                            }
                        }
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

