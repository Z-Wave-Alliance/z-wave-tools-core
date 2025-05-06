using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_ALARM
    {
        public const byte ID = 0x71;
        public const byte VERSION = 1;
        public partial class ALARM_GET
        {
            public const byte ID = 0x04;
            public ByteValue alarmType = 0;
            public static implicit operator ALARM_GET(byte[] data)
            {
                ALARM_GET ret = new ALARM_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.alarmType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](ALARM_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ALARM.ID);
                ret.Add(ID);
                if (command.alarmType.HasValue) ret.Add(command.alarmType);
                return ret.ToArray();
            }
        }
        public partial class ALARM_REPORT
        {
            public const byte ID = 0x05;
            public ByteValue alarmType = 0;
            public ByteValue alarmLevel = 0;
            public static implicit operator ALARM_REPORT(byte[] data)
            {
                ALARM_REPORT ret = new ALARM_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.alarmType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.alarmLevel = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](ALARM_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ALARM.ID);
                ret.Add(ID);
                if (command.alarmType.HasValue) ret.Add(command.alarmType);
                if (command.alarmLevel.HasValue) ret.Add(command.alarmLevel);
                return ret.ToArray();
            }
        }
    }
}

