/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_METER_PULSE
    {
        public const byte ID = 0x35;
        public const byte VERSION = 1;
        public partial class METER_PULSE_GET
        {
            public const byte ID = 0x04;
            public static implicit operator METER_PULSE_GET(byte[] data)
            {
                METER_PULSE_GET ret = new METER_PULSE_GET();
                return ret;
            }
            public static implicit operator byte[](METER_PULSE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_METER_PULSE.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class METER_PULSE_REPORT
        {
            public const byte ID = 0x05;
            public const byte pulseCountBytesCount = 4;
            public byte[] pulseCount = new byte[pulseCountBytesCount];
            public static implicit operator METER_PULSE_REPORT(byte[] data)
            {
                METER_PULSE_REPORT ret = new METER_PULSE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.pulseCount = (data.Length - index) >= pulseCountBytesCount ? new byte[pulseCountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.pulseCount[0] = data[index++];
                    if (data.Length > index) ret.pulseCount[1] = data[index++];
                    if (data.Length > index) ret.pulseCount[2] = data[index++];
                    if (data.Length > index) ret.pulseCount[3] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](METER_PULSE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_METER_PULSE.ID);
                ret.Add(ID);
                if (command.pulseCount != null)
                {
                    foreach (var tmp in command.pulseCount)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

