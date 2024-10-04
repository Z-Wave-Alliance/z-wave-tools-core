/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_USER_CODE
    {
        public const byte ID = 0x63;
        public const byte VERSION = 1;
        public partial class USER_CODE_GET
        {
            public const byte ID = 0x02;
            public ByteValue userIdentifier = 0;
            public static implicit operator USER_CODE_GET(byte[] data)
            {
                USER_CODE_GET ret = new USER_CODE_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.userIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](USER_CODE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CODE.ID);
                ret.Add(ID);
                if (command.userIdentifier.HasValue) ret.Add(command.userIdentifier);
                return ret.ToArray();
            }
        }
        public partial class USER_CODE_REPORT
        {
            public const byte ID = 0x03;
            public ByteValue userIdentifier = 0;
            public ByteValue userIdStatus = 0;
            public IList<byte> userCode = new List<byte>();
            public static implicit operator USER_CODE_REPORT(byte[] data)
            {
                USER_CODE_REPORT ret = new USER_CODE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.userIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.userIdStatus = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.userCode = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.userCode.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](USER_CODE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CODE.ID);
                ret.Add(ID);
                if (command.userIdentifier.HasValue) ret.Add(command.userIdentifier);
                if (command.userIdStatus.HasValue) ret.Add(command.userIdStatus);
                if (command.userCode != null)
                {
                    foreach (var tmp in command.userCode)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class USER_CODE_SET
        {
            public const byte ID = 0x01;
            public ByteValue userIdentifier = 0;
            public ByteValue userIdStatus = 0;
            public IList<byte> userCode = new List<byte>();
            public static implicit operator USER_CODE_SET(byte[] data)
            {
                USER_CODE_SET ret = new USER_CODE_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.userIdentifier = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.userIdStatus = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.userCode = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.userCode.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](USER_CODE_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CODE.ID);
                ret.Add(ID);
                if (command.userIdentifier.HasValue) ret.Add(command.userIdentifier);
                if (command.userIdStatus.HasValue) ret.Add(command.userIdStatus);
                if (command.userCode != null)
                {
                    foreach (var tmp in command.userCode)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class USERS_NUMBER_GET
        {
            public const byte ID = 0x04;
            public static implicit operator USERS_NUMBER_GET(byte[] data)
            {
                USERS_NUMBER_GET ret = new USERS_NUMBER_GET();
                return ret;
            }
            public static implicit operator byte[](USERS_NUMBER_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CODE.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class USERS_NUMBER_REPORT
        {
            public const byte ID = 0x05;
            public ByteValue supportedUsers = 0;
            public static implicit operator USERS_NUMBER_REPORT(byte[] data)
            {
                USERS_NUMBER_REPORT ret = new USERS_NUMBER_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.supportedUsers = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](USERS_NUMBER_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_USER_CODE.ID);
                ret.Add(ID);
                if (command.supportedUsers.HasValue) ret.Add(command.supportedUsers);
                return ret.ToArray();
            }
        }
    }
}

