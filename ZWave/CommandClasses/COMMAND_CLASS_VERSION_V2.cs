/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_VERSION_V2
    {
        public const byte ID = 0x86;
        public const byte VERSION = 2;
        public partial class VERSION_COMMAND_CLASS_GET
        {
            public const byte ID = 0x13;
            public ByteValue requestedCommandClass = 0;
            public static implicit operator VERSION_COMMAND_CLASS_GET(byte[] data)
            {
                VERSION_COMMAND_CLASS_GET ret = new VERSION_COMMAND_CLASS_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.requestedCommandClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](VERSION_COMMAND_CLASS_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_VERSION_V2.ID);
                ret.Add(ID);
                if (command.requestedCommandClass.HasValue) ret.Add(command.requestedCommandClass);
                return ret.ToArray();
            }
        }
        public partial class VERSION_COMMAND_CLASS_REPORT
        {
            public const byte ID = 0x14;
            public ByteValue requestedCommandClass = 0;
            public ByteValue commandClassVersion = 0;
            public static implicit operator VERSION_COMMAND_CLASS_REPORT(byte[] data)
            {
                VERSION_COMMAND_CLASS_REPORT ret = new VERSION_COMMAND_CLASS_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.requestedCommandClass = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.commandClassVersion = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](VERSION_COMMAND_CLASS_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_VERSION_V2.ID);
                ret.Add(ID);
                if (command.requestedCommandClass.HasValue) ret.Add(command.requestedCommandClass);
                if (command.commandClassVersion.HasValue) ret.Add(command.commandClassVersion);
                return ret.ToArray();
            }
        }
        public partial class VERSION_GET
        {
            public const byte ID = 0x11;
            public static implicit operator VERSION_GET(byte[] data)
            {
                VERSION_GET ret = new VERSION_GET();
                return ret;
            }
            public static implicit operator byte[](VERSION_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_VERSION_V2.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class VERSION_REPORT
        {
            public const byte ID = 0x12;
            public ByteValue zWaveLibraryType = 0;
            public ByteValue zWaveProtocolVersion = 0;
            public ByteValue zWaveProtocolSubVersion = 0;
            public ByteValue firmware0Version = 0;
            public ByteValue firmware0SubVersion = 0;
            public ByteValue hardwareVersion = 0;
            public ByteValue numberOfFirmwareTargets = 0;
            public class TVG
            {
                public ByteValue firmwareVersion = 0;
                public ByteValue firmwareSubVersion = 0;
            }
            public List<TVG> vg = new List<TVG>();
            public static implicit operator VERSION_REPORT(byte[] data)
            {
                VERSION_REPORT ret = new VERSION_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.zWaveLibraryType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.zWaveProtocolVersion = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.zWaveProtocolSubVersion = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.firmware0Version = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.firmware0SubVersion = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.hardwareVersion = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.numberOfFirmwareTargets = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.vg = new List<TVG>();
                    for (int j = 0; j < ret.numberOfFirmwareTargets; j++)
                    {
                        TVG tmp = new TVG();
                        tmp.firmwareVersion = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        tmp.firmwareSubVersion = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                        ret.vg.Add(tmp);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](VERSION_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_VERSION_V2.ID);
                ret.Add(ID);
                if (command.zWaveLibraryType.HasValue) ret.Add(command.zWaveLibraryType);
                if (command.zWaveProtocolVersion.HasValue) ret.Add(command.zWaveProtocolVersion);
                if (command.zWaveProtocolSubVersion.HasValue) ret.Add(command.zWaveProtocolSubVersion);
                if (command.firmware0Version.HasValue) ret.Add(command.firmware0Version);
                if (command.firmware0SubVersion.HasValue) ret.Add(command.firmware0SubVersion);
                if (command.hardwareVersion.HasValue) ret.Add(command.hardwareVersion);
                if (command.numberOfFirmwareTargets.HasValue) ret.Add(command.numberOfFirmwareTargets);
                if (command.vg != null)
                {
                    foreach (var item in command.vg)
                    {
                        if (item.firmwareVersion.HasValue) ret.Add(item.firmwareVersion);
                        if (item.firmwareSubVersion.HasValue) ret.Add(item.firmwareSubVersion);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

