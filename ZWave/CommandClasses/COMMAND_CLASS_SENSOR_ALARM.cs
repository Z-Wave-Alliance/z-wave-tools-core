/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_SENSOR_ALARM
    {
        public const byte ID = 0x9C;
        public const byte VERSION = 1;
        public partial class SENSOR_ALARM_GET
        {
            public const byte ID = 0x01;
            public ByteValue sensorType = 0;
            public static implicit operator SENSOR_ALARM_GET(byte[] data)
            {
                SENSOR_ALARM_GET ret = new SENSOR_ALARM_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.sensorType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SENSOR_ALARM_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SENSOR_ALARM.ID);
                ret.Add(ID);
                if (command.sensorType.HasValue) ret.Add(command.sensorType);
                return ret.ToArray();
            }
        }
        public partial class SENSOR_ALARM_REPORT
        {
            public const byte ID = 0x02;
            public ByteValue sourceNodeId = 0;
            public ByteValue sensorType = 0;
            public ByteValue sensorState = 0;
            public const byte secondsBytesCount = 2;
            public byte[] seconds = new byte[secondsBytesCount];
            public static implicit operator SENSOR_ALARM_REPORT(byte[] data)
            {
                SENSOR_ALARM_REPORT ret = new SENSOR_ALARM_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.sourceNodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.sensorType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.sensorState = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.seconds = (data.Length - index) >= secondsBytesCount ? new byte[secondsBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.seconds[0] = data[index++];
                    if (data.Length > index) ret.seconds[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](SENSOR_ALARM_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SENSOR_ALARM.ID);
                ret.Add(ID);
                if (command.sourceNodeId.HasValue) ret.Add(command.sourceNodeId);
                if (command.sensorType.HasValue) ret.Add(command.sensorType);
                if (command.sensorState.HasValue) ret.Add(command.sensorState);
                if (command.seconds != null)
                {
                    foreach (var tmp in command.seconds)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class SENSOR_ALARM_SUPPORTED_GET
        {
            public const byte ID = 0x03;
            public static implicit operator SENSOR_ALARM_SUPPORTED_GET(byte[] data)
            {
                SENSOR_ALARM_SUPPORTED_GET ret = new SENSOR_ALARM_SUPPORTED_GET();
                return ret;
            }
            public static implicit operator byte[](SENSOR_ALARM_SUPPORTED_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SENSOR_ALARM.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class SENSOR_ALARM_SUPPORTED_REPORT
        {
            public const byte ID = 0x04;
            public ByteValue numberOfBitMasks = 0;
            public IList<byte> bitMask = new List<byte>();
            public static implicit operator SENSOR_ALARM_SUPPORTED_REPORT(byte[] data)
            {
                SENSOR_ALARM_SUPPORTED_REPORT ret = new SENSOR_ALARM_SUPPORTED_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.numberOfBitMasks = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.bitMask = new List<byte>();
                    for (int i = 0; i < ret.numberOfBitMasks; i++)
                    {
                        if (data.Length > index) ret.bitMask.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](SENSOR_ALARM_SUPPORTED_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SENSOR_ALARM.ID);
                ret.Add(ID);
                if (command.numberOfBitMasks.HasValue) ret.Add(command.numberOfBitMasks);
                if (command.bitMask != null)
                {
                    foreach (var tmp in command.bitMask)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

