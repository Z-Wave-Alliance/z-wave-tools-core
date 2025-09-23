/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_SENSOR_BINARY_V2
    {
        public const byte ID = 0x30;
        public const byte VERSION = 2;
        public partial class SENSOR_BINARY_GET
        {
            public const byte ID = 0x02;
            public ByteValue sensorType = 0;
            public static implicit operator SENSOR_BINARY_GET(byte[] data)
            {
                SENSOR_BINARY_GET ret = new SENSOR_BINARY_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.sensorType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SENSOR_BINARY_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SENSOR_BINARY_V2.ID);
                ret.Add(ID);
                if (command.sensorType.HasValue) ret.Add(command.sensorType);
                return ret.ToArray();
            }
        }
        public partial class SENSOR_BINARY_REPORT
        {
            public const byte ID = 0x03;
            public ByteValue sensorValue = 0;
            public ByteValue sensorType = 0;
            public static implicit operator SENSOR_BINARY_REPORT(byte[] data)
            {
                SENSOR_BINARY_REPORT ret = new SENSOR_BINARY_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.sensorValue = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.sensorType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SENSOR_BINARY_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SENSOR_BINARY_V2.ID);
                ret.Add(ID);
                if (command.sensorValue.HasValue) ret.Add(command.sensorValue);
                if (command.sensorType.HasValue) ret.Add(command.sensorType);
                return ret.ToArray();
            }
        }
        public partial class SENSOR_BINARY_SUPPORTED_GET_SENSOR
        {
            public const byte ID = 0x01;
            public static implicit operator SENSOR_BINARY_SUPPORTED_GET_SENSOR(byte[] data)
            {
                SENSOR_BINARY_SUPPORTED_GET_SENSOR ret = new SENSOR_BINARY_SUPPORTED_GET_SENSOR();
                return ret;
            }
            public static implicit operator byte[](SENSOR_BINARY_SUPPORTED_GET_SENSOR command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SENSOR_BINARY_V2.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class SENSOR_BINARY_SUPPORTED_SENSOR_REPORT
        {
            public const byte ID = 0x04;
            public IList<byte> bitMask = new List<byte>();
            public static implicit operator SENSOR_BINARY_SUPPORTED_SENSOR_REPORT(byte[] data)
            {
                SENSOR_BINARY_SUPPORTED_SENSOR_REPORT ret = new SENSOR_BINARY_SUPPORTED_SENSOR_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.bitMask = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.bitMask.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](SENSOR_BINARY_SUPPORTED_SENSOR_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SENSOR_BINARY_V2.ID);
                ret.Add(ID);
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

