/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_DEVICE_RESET_LOCALLY
    {
        public const byte ID = 0x5A;
        public const byte VERSION = 1;
        public partial class DEVICE_RESET_LOCALLY_NOTIFICATION
        {
            public const byte ID = 0x01;
            public static implicit operator DEVICE_RESET_LOCALLY_NOTIFICATION(byte[] data)
            {
                DEVICE_RESET_LOCALLY_NOTIFICATION ret = new DEVICE_RESET_LOCALLY_NOTIFICATION();
                return ret;
            }
            public static implicit operator byte[](DEVICE_RESET_LOCALLY_NOTIFICATION command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_DEVICE_RESET_LOCALLY.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
    }
}

