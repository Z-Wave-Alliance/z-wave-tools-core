/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_BATTERY
    {
        public const byte ID = 0x80;
        public const byte VERSION = 1;
        public partial class BATTERY_GET
        {
            public const byte ID = 0x02;
            public static implicit operator BATTERY_GET(byte[] data)
            {
                BATTERY_GET ret = new BATTERY_GET();
                return ret;
            }
            public static implicit operator byte[](BATTERY_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_BATTERY.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class BATTERY_REPORT
        {
            public const byte ID = 0x03;
            public ByteValue batteryLevel = 0;
            public static implicit operator BATTERY_REPORT(byte[] data)
            {
                BATTERY_REPORT ret = new BATTERY_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.batteryLevel = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](BATTERY_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_BATTERY.ID);
                ret.Add(ID);
                if (command.batteryLevel.HasValue) ret.Add(command.batteryLevel);
                return ret.ToArray();
            }
        }
    }
}

