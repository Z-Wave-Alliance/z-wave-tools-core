using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_MULTI_INSTANCE
    {
        public const byte ID = 0x60;
        public const byte VERSION = 1;
        public partial class MULTI_INSTANCE_CMD_ENCAP
        {
            public const byte ID = 0x06;
            public ByteValue instance = 0;
            public ByteValue commandClass = 0;
            public ByteValue command = 0;
            public IList<byte> parameter = new List<byte>();
            public static implicit operator MULTI_INSTANCE_CMD_ENCAP(byte[] data)
            {
                MULTI_INSTANCE_CMD_ENCAP ret = new MULTI_INSTANCE_CMD_ENCAP();
                if (data != null)
                {
                    int index = 2;
                    ret.instance = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.commandClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.command = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.parameter = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.parameter.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](MULTI_INSTANCE_CMD_ENCAP command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_MULTI_INSTANCE.ID);
                ret.Add(ID);
                if (command.instance.HasValue) ret.Add(command.instance);
                if (command.commandClass.HasValue) ret.Add(command.commandClass);
                if (command.command.HasValue) ret.Add(command.command);
                if (command.parameter != null)
                {
                    foreach (var tmp in command.parameter)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class MULTI_INSTANCE_GET
        {
            public const byte ID = 0x04;
            public ByteValue commandClass = 0;
            public static implicit operator MULTI_INSTANCE_GET(byte[] data)
            {
                MULTI_INSTANCE_GET ret = new MULTI_INSTANCE_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.commandClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](MULTI_INSTANCE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_MULTI_INSTANCE.ID);
                ret.Add(ID);
                if (command.commandClass.HasValue) ret.Add(command.commandClass);
                return ret.ToArray();
            }
        }
        public partial class MULTI_INSTANCE_REPORT
        {
            public const byte ID = 0x05;
            public ByteValue commandClass = 0;
            public ByteValue instances = 0;
            public static implicit operator MULTI_INSTANCE_REPORT(byte[] data)
            {
                MULTI_INSTANCE_REPORT ret = new MULTI_INSTANCE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.commandClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.instances = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](MULTI_INSTANCE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_MULTI_INSTANCE.ID);
                ret.Add(ID);
                if (command.commandClass.HasValue) ret.Add(command.commandClass);
                if (command.instances.HasValue) ret.Add(command.instances);
                return ret.ToArray();
            }
        }
    }
}

