/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_ZWAVEPLUS_INFO_V2
    {
        public const byte ID = 0x5E;
        public const byte VERSION = 2;
        public partial class ZWAVEPLUS_INFO_GET
        {
            public const byte ID = 0x01;
            public static implicit operator ZWAVEPLUS_INFO_GET(byte[] data)
            {
                ZWAVEPLUS_INFO_GET ret = new ZWAVEPLUS_INFO_GET();
                return ret;
            }
            public static implicit operator byte[](ZWAVEPLUS_INFO_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZWAVEPLUS_INFO_V2.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class ZWAVEPLUS_INFO_REPORT
        {
            public const byte ID = 0x02;
            public ByteValue zWaveVersion = 0;
            public ByteValue roleType = 0;
            public ByteValue nodeType = 0;
            public const byte installerIconTypeBytesCount = 2;
            public byte[] installerIconType = new byte[installerIconTypeBytesCount];
            public const byte userIconTypeBytesCount = 2;
            public byte[] userIconType = new byte[userIconTypeBytesCount];
            public static implicit operator ZWAVEPLUS_INFO_REPORT(byte[] data)
            {
                ZWAVEPLUS_INFO_REPORT ret = new ZWAVEPLUS_INFO_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.zWaveVersion = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.roleType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nodeType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.installerIconType = (data.Length - index) >= installerIconTypeBytesCount ? new byte[installerIconTypeBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.installerIconType[0] = data[index++];
                    if (data.Length > index) ret.installerIconType[1] = data[index++];
                    ret.userIconType = (data.Length - index) >= userIconTypeBytesCount ? new byte[userIconTypeBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.userIconType[0] = data[index++];
                    if (data.Length > index) ret.userIconType[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](ZWAVEPLUS_INFO_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_ZWAVEPLUS_INFO_V2.ID);
                ret.Add(ID);
                if (command.zWaveVersion.HasValue) ret.Add(command.zWaveVersion);
                if (command.roleType.HasValue) ret.Add(command.roleType);
                if (command.nodeType.HasValue) ret.Add(command.nodeType);
                if (command.installerIconType != null)
                {
                    foreach (var tmp in command.installerIconType)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.userIconType != null)
                {
                    foreach (var tmp in command.userIconType)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

