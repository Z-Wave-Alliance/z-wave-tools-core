/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_SILENCE_ALARM
    {
        public const byte ID = 0x9D;
        public const byte VERSION = 1;
        public partial class SENSOR_ALARM_SET
        {
            public const byte ID = 0x01;
            public ByteValue mode = 0;
            public const byte secondsBytesCount = 2;
            public byte[] seconds = new byte[secondsBytesCount];
            public ByteValue numberOfBitMasks = 0;
            public IList<byte> bitMask = new List<byte>();
            public static implicit operator SENSOR_ALARM_SET(byte[] data)
            {
                SENSOR_ALARM_SET ret = new SENSOR_ALARM_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.mode = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.seconds = (data.Length - index) >= secondsBytesCount ? new byte[secondsBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.seconds[0] = data[index++];
                    if (data.Length > index) ret.seconds[1] = data[index++];
                    ret.numberOfBitMasks = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.bitMask = new List<byte>();
                    for (int i = 0; i < ret.numberOfBitMasks; i++)
                    {
                        if (data.Length > index) ret.bitMask.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](SENSOR_ALARM_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SILENCE_ALARM.ID);
                ret.Add(ID);
                if (command.mode.HasValue) ret.Add(command.mode);
                if (command.seconds != null)
                {
                    foreach (var tmp in command.seconds)
                    {
                        ret.Add(tmp);
                    }
                }
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

