/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_INCLUSION_CONTROLLER
    {
        public const byte ID = 0x74;
        public const byte VERSION = 1;
        public partial class INITIATE
        {
            public const byte ID = 0x01;
            public ByteValue nodeId = 0;
            public ByteValue stepId = 0;
            public static implicit operator INITIATE(byte[] data)
            {
                INITIATE ret = new INITIATE();
                if (data != null)
                {
                    int index = 2;
                    ret.nodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.stepId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](INITIATE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_INCLUSION_CONTROLLER.ID);
                ret.Add(ID);
                if (command.nodeId.HasValue) ret.Add(command.nodeId);
                if (command.stepId.HasValue) ret.Add(command.stepId);
                return ret.ToArray();
            }
        }
        public partial class COMPLETE
        {
            public const byte ID = 0x02;
            public ByteValue stepId = 0;
            public ByteValue status = 0;
            public static implicit operator COMPLETE(byte[] data)
            {
                COMPLETE ret = new COMPLETE();
                if (data != null)
                {
                    int index = 2;
                    ret.stepId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.status = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](COMPLETE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_INCLUSION_CONTROLLER.ID);
                ret.Add(ID);
                if (command.stepId.HasValue) ret.Add(command.stepId);
                if (command.status.HasValue) ret.Add(command.status);
                return ret.ToArray();
            }
        }
    }
}

