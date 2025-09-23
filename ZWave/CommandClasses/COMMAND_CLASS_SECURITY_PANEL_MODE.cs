/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_SECURITY_PANEL_MODE
    {
        public const byte ID = 0x24;
        public const byte VERSION = 1;
        public partial class SECURITY_PANEL_MODE_GET
        {
            public const byte ID = 0x03;
            public static implicit operator SECURITY_PANEL_MODE_GET(byte[] data)
            {
                SECURITY_PANEL_MODE_GET ret = new SECURITY_PANEL_MODE_GET();
                return ret;
            }
            public static implicit operator byte[](SECURITY_PANEL_MODE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SECURITY_PANEL_MODE.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class SECURITY_PANEL_MODE_REPORT
        {
            public const byte ID = 0x04;
            public ByteValue mode = 0;
            public static implicit operator SECURITY_PANEL_MODE_REPORT(byte[] data)
            {
                SECURITY_PANEL_MODE_REPORT ret = new SECURITY_PANEL_MODE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.mode = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SECURITY_PANEL_MODE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SECURITY_PANEL_MODE.ID);
                ret.Add(ID);
                if (command.mode.HasValue) ret.Add(command.mode);
                return ret.ToArray();
            }
        }
        public partial class SECURITY_PANEL_MODE_SET
        {
            public const byte ID = 0x05;
            public ByteValue mode = 0;
            public static implicit operator SECURITY_PANEL_MODE_SET(byte[] data)
            {
                SECURITY_PANEL_MODE_SET ret = new SECURITY_PANEL_MODE_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.mode = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SECURITY_PANEL_MODE_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SECURITY_PANEL_MODE.ID);
                ret.Add(ID);
                if (command.mode.HasValue) ret.Add(command.mode);
                return ret.ToArray();
            }
        }
        public partial class SECURITY_PANEL_MODE_SUPPORTED_GET
        {
            public const byte ID = 0x01;
            public static implicit operator SECURITY_PANEL_MODE_SUPPORTED_GET(byte[] data)
            {
                SECURITY_PANEL_MODE_SUPPORTED_GET ret = new SECURITY_PANEL_MODE_SUPPORTED_GET();
                return ret;
            }
            public static implicit operator byte[](SECURITY_PANEL_MODE_SUPPORTED_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SECURITY_PANEL_MODE.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class SECURITY_PANEL_MODE_SUPPORTED_REPORT
        {
            public const byte ID = 0x02;
            public const byte supportedModeBitMaskBytesCount = 2;
            public byte[] supportedModeBitMask = new byte[supportedModeBitMaskBytesCount];
            public static implicit operator SECURITY_PANEL_MODE_SUPPORTED_REPORT(byte[] data)
            {
                SECURITY_PANEL_MODE_SUPPORTED_REPORT ret = new SECURITY_PANEL_MODE_SUPPORTED_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.supportedModeBitMask = (data.Length - index) >= supportedModeBitMaskBytesCount ? new byte[supportedModeBitMaskBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.supportedModeBitMask[0] = data[index++];
                    if (data.Length > index) ret.supportedModeBitMask[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](SECURITY_PANEL_MODE_SUPPORTED_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SECURITY_PANEL_MODE.ID);
                ret.Add(ID);
                if (command.supportedModeBitMask != null)
                {
                    foreach (var tmp in command.supportedModeBitMask)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

