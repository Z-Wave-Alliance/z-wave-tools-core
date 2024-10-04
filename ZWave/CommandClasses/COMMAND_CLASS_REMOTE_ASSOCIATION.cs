/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_REMOTE_ASSOCIATION
    {
        public const byte ID = 0x7D;
        public const byte VERSION = 1;
        public partial class REMOTE_ASSOCIATION_CONFIGURATION_GET
        {
            public const byte ID = 0x02;
            public ByteValue localGroupingIdentifier = 0;
            public static implicit operator REMOTE_ASSOCIATION_CONFIGURATION_GET(byte[] data)
            {
                REMOTE_ASSOCIATION_CONFIGURATION_GET ret = new REMOTE_ASSOCIATION_CONFIGURATION_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.localGroupingIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](REMOTE_ASSOCIATION_CONFIGURATION_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_REMOTE_ASSOCIATION.ID);
                ret.Add(ID);
                if (command.localGroupingIdentifier.HasValue) ret.Add(command.localGroupingIdentifier);
                return ret.ToArray();
            }
        }
        public partial class REMOTE_ASSOCIATION_CONFIGURATION_REPORT
        {
            public const byte ID = 0x03;
            public ByteValue localGroupingIdentifier = 0;
            public ByteValue remoteNodeid = 0;
            public ByteValue remoteGroupingIdentifier = 0;
            public static implicit operator REMOTE_ASSOCIATION_CONFIGURATION_REPORT(byte[] data)
            {
                REMOTE_ASSOCIATION_CONFIGURATION_REPORT ret = new REMOTE_ASSOCIATION_CONFIGURATION_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.localGroupingIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.remoteNodeid = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.remoteGroupingIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](REMOTE_ASSOCIATION_CONFIGURATION_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_REMOTE_ASSOCIATION.ID);
                ret.Add(ID);
                if (command.localGroupingIdentifier.HasValue) ret.Add(command.localGroupingIdentifier);
                if (command.remoteNodeid.HasValue) ret.Add(command.remoteNodeid);
                if (command.remoteGroupingIdentifier.HasValue) ret.Add(command.remoteGroupingIdentifier);
                return ret.ToArray();
            }
        }
        public partial class REMOTE_ASSOCIATION_CONFIGURATION_SET
        {
            public const byte ID = 0x01;
            public ByteValue localGroupingIdentifier = 0;
            public ByteValue remoteNodeid = 0;
            public ByteValue remoteGroupingIdentifier = 0;
            public static implicit operator REMOTE_ASSOCIATION_CONFIGURATION_SET(byte[] data)
            {
                REMOTE_ASSOCIATION_CONFIGURATION_SET ret = new REMOTE_ASSOCIATION_CONFIGURATION_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.localGroupingIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.remoteNodeid = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.remoteGroupingIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](REMOTE_ASSOCIATION_CONFIGURATION_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_REMOTE_ASSOCIATION.ID);
                ret.Add(ID);
                if (command.localGroupingIdentifier.HasValue) ret.Add(command.localGroupingIdentifier);
                if (command.remoteNodeid.HasValue) ret.Add(command.remoteNodeid);
                if (command.remoteGroupingIdentifier.HasValue) ret.Add(command.remoteGroupingIdentifier);
                return ret.ToArray();
            }
        }
    }
}

