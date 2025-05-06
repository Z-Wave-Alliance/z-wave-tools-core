using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_WAKE_UP_V3
    {
        public const byte ID = 0x84;
        public const byte VERSION = 3;
        public partial class WAKE_UP_INTERVAL_CAPABILITIES_GET
        {
            public const byte ID = 0x09;
            public static implicit operator WAKE_UP_INTERVAL_CAPABILITIES_GET(byte[] data)
            {
                WAKE_UP_INTERVAL_CAPABILITIES_GET ret = new WAKE_UP_INTERVAL_CAPABILITIES_GET();
                return ret;
            }
            public static implicit operator byte[](WAKE_UP_INTERVAL_CAPABILITIES_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_WAKE_UP_V3.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class WAKE_UP_INTERVAL_CAPABILITIES_REPORT
        {
            public const byte ID = 0x0A;
            public const byte minimumWakeUpIntervalSecondsBytesCount = 3;
            public byte[] minimumWakeUpIntervalSeconds = new byte[minimumWakeUpIntervalSecondsBytesCount];
            public const byte maximumWakeUpIntervalSecondsBytesCount = 3;
            public byte[] maximumWakeUpIntervalSeconds = new byte[maximumWakeUpIntervalSecondsBytesCount];
            public const byte defaultWakeUpIntervalSecondsBytesCount = 3;
            public byte[] defaultWakeUpIntervalSeconds = new byte[defaultWakeUpIntervalSecondsBytesCount];
            public const byte wakeUpIntervalStepSecondsBytesCount = 3;
            public byte[] wakeUpIntervalStepSeconds = new byte[wakeUpIntervalStepSecondsBytesCount];
            public struct Tproperties1
            {
                private byte _value;
                public bool HasValue { get; private set; }
                public static Tproperties1 Empty { get { return new Tproperties1() { _value = 0, HasValue = false }; } }
                public byte wakeUpOnDemand
                {
                    get { return (byte)(_value >> 0 & 0x01); }
                    set { HasValue = true; _value &= 0xFF - 0x01; _value += (byte)(value << 0 & 0x01); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 1 & 0x7F); }
                    set { HasValue = true; _value &= 0xFF - 0xFE; _value += (byte)(value << 1 & 0xFE); }
                }
                public static implicit operator Tproperties1(byte data)
                {
                    Tproperties1 ret = new Tproperties1();
                    ret._value = data;
                    ret.HasValue = true;
                    return ret;
                }
                public static implicit operator byte(Tproperties1 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties1 properties1 = 0;
            public static implicit operator WAKE_UP_INTERVAL_CAPABILITIES_REPORT(byte[] data)
            {
                WAKE_UP_INTERVAL_CAPABILITIES_REPORT ret = new WAKE_UP_INTERVAL_CAPABILITIES_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.minimumWakeUpIntervalSeconds = (data.Length - index) >= minimumWakeUpIntervalSecondsBytesCount ? new byte[minimumWakeUpIntervalSecondsBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.minimumWakeUpIntervalSeconds[0] = data[index++];
                    if (data.Length > index) ret.minimumWakeUpIntervalSeconds[1] = data[index++];
                    if (data.Length > index) ret.minimumWakeUpIntervalSeconds[2] = data[index++];
                    ret.maximumWakeUpIntervalSeconds = (data.Length - index) >= maximumWakeUpIntervalSecondsBytesCount ? new byte[maximumWakeUpIntervalSecondsBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.maximumWakeUpIntervalSeconds[0] = data[index++];
                    if (data.Length > index) ret.maximumWakeUpIntervalSeconds[1] = data[index++];
                    if (data.Length > index) ret.maximumWakeUpIntervalSeconds[2] = data[index++];
                    ret.defaultWakeUpIntervalSeconds = (data.Length - index) >= defaultWakeUpIntervalSecondsBytesCount ? new byte[defaultWakeUpIntervalSecondsBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.defaultWakeUpIntervalSeconds[0] = data[index++];
                    if (data.Length > index) ret.defaultWakeUpIntervalSeconds[1] = data[index++];
                    if (data.Length > index) ret.defaultWakeUpIntervalSeconds[2] = data[index++];
                    ret.wakeUpIntervalStepSeconds = (data.Length - index) >= wakeUpIntervalStepSecondsBytesCount ? new byte[wakeUpIntervalStepSecondsBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.wakeUpIntervalStepSeconds[0] = data[index++];
                    if (data.Length > index) ret.wakeUpIntervalStepSeconds[1] = data[index++];
                    if (data.Length > index) ret.wakeUpIntervalStepSeconds[2] = data[index++];
                    ret.properties1 = data.Length > index ? (Tproperties1)data[index++] : Tproperties1.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](WAKE_UP_INTERVAL_CAPABILITIES_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_WAKE_UP_V3.ID);
                ret.Add(ID);
                if (command.minimumWakeUpIntervalSeconds != null)
                {
                    foreach (var tmp in command.minimumWakeUpIntervalSeconds)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.maximumWakeUpIntervalSeconds != null)
                {
                    foreach (var tmp in command.maximumWakeUpIntervalSeconds)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.defaultWakeUpIntervalSeconds != null)
                {
                    foreach (var tmp in command.defaultWakeUpIntervalSeconds)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.wakeUpIntervalStepSeconds != null)
                {
                    foreach (var tmp in command.wakeUpIntervalStepSeconds)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.properties1.HasValue) ret.Add(command.properties1);
                return ret.ToArray();
            }
        }
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
                ret.Add(COMMAND_CLASS_WAKE_UP_V3.ID);
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
                ret.Add(COMMAND_CLASS_WAKE_UP_V3.ID);
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
                ret.Add(COMMAND_CLASS_WAKE_UP_V3.ID);
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
                ret.Add(COMMAND_CLASS_WAKE_UP_V3.ID);
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
                ret.Add(COMMAND_CLASS_WAKE_UP_V3.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
    }
}

