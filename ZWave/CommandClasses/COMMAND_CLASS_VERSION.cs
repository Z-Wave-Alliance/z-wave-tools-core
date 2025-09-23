/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_VERSION
    {
        public const byte ID = 0x86;
        public const byte VERSION = 1;
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
                ret.Add(COMMAND_CLASS_VERSION.ID);
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
                ret.Add(COMMAND_CLASS_VERSION.ID);
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
                ret.Add(COMMAND_CLASS_VERSION.ID);
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
            public ByteValue applicationVersion = 0;
            public ByteValue applicationSubVersion = 0;
            public static implicit operator VERSION_REPORT(byte[] data)
            {
                VERSION_REPORT ret = new VERSION_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.zWaveLibraryType = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.zWaveProtocolVersion = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.zWaveProtocolSubVersion = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.applicationVersion = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.applicationSubVersion = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](VERSION_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_VERSION.ID);
                ret.Add(ID);
                if (command.zWaveLibraryType.HasValue) ret.Add(command.zWaveLibraryType);
                if (command.zWaveProtocolVersion.HasValue) ret.Add(command.zWaveProtocolVersion);
                if (command.zWaveProtocolSubVersion.HasValue) ret.Add(command.zWaveProtocolSubVersion);
                if (command.applicationVersion.HasValue) ret.Add(command.applicationVersion);
                if (command.applicationSubVersion.HasValue) ret.Add(command.applicationSubVersion);
                return ret.ToArray();
            }
        }
    }
}

