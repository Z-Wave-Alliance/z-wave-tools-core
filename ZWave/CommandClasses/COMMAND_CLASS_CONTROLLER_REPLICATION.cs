/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_CONTROLLER_REPLICATION
    {
        public const byte ID = 0x21;
        public const byte VERSION = 1;
        public partial class CTRL_REPLICATION_TRANSFER_GROUP
        {
            public const byte ID = 0x31;
            public ByteValue sequenceNumber = 0;
            public ByteValue groupId = 0;
            public ByteValue nodeId = 0;
            public static implicit operator CTRL_REPLICATION_TRANSFER_GROUP(byte[] data)
            {
                CTRL_REPLICATION_TRANSFER_GROUP ret = new CTRL_REPLICATION_TRANSFER_GROUP();
                if (data != null)
                {
                    int index = 2;
                    ret.sequenceNumber = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.groupId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](CTRL_REPLICATION_TRANSFER_GROUP command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_CONTROLLER_REPLICATION.ID);
                ret.Add(ID);
                if (command.sequenceNumber.HasValue) ret.Add(command.sequenceNumber);
                if (command.groupId.HasValue) ret.Add(command.groupId);
                if (command.nodeId.HasValue) ret.Add(command.nodeId);
                return ret.ToArray();
            }
        }
        public partial class CTRL_REPLICATION_TRANSFER_GROUP_NAME
        {
            public const byte ID = 0x32;
            public ByteValue sequenceNumber = 0;
            public ByteValue groupId = 0;
            public IList<byte> groupName = new List<byte>();
            public static implicit operator CTRL_REPLICATION_TRANSFER_GROUP_NAME(byte[] data)
            {
                CTRL_REPLICATION_TRANSFER_GROUP_NAME ret = new CTRL_REPLICATION_TRANSFER_GROUP_NAME();
                if (data != null)
                {
                    int index = 2;
                    ret.sequenceNumber = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.groupId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.groupName = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.groupName.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](CTRL_REPLICATION_TRANSFER_GROUP_NAME command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_CONTROLLER_REPLICATION.ID);
                ret.Add(ID);
                if (command.sequenceNumber.HasValue) ret.Add(command.sequenceNumber);
                if (command.groupId.HasValue) ret.Add(command.groupId);
                if (command.groupName != null)
                {
                    foreach (var tmp in command.groupName)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class CTRL_REPLICATION_TRANSFER_SCENE
        {
            public const byte ID = 0x33;
            public ByteValue sequenceNumber = 0;
            public ByteValue sceneId = 0;
            public ByteValue nodeId = 0;
            public ByteValue level = 0;
            public static implicit operator CTRL_REPLICATION_TRANSFER_SCENE(byte[] data)
            {
                CTRL_REPLICATION_TRANSFER_SCENE ret = new CTRL_REPLICATION_TRANSFER_SCENE();
                if (data != null)
                {
                    int index = 2;
                    ret.sequenceNumber = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.sceneId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.nodeId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.level = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](CTRL_REPLICATION_TRANSFER_SCENE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_CONTROLLER_REPLICATION.ID);
                ret.Add(ID);
                if (command.sequenceNumber.HasValue) ret.Add(command.sequenceNumber);
                if (command.sceneId.HasValue) ret.Add(command.sceneId);
                if (command.nodeId.HasValue) ret.Add(command.nodeId);
                if (command.level.HasValue) ret.Add(command.level);
                return ret.ToArray();
            }
        }
        public partial class CTRL_REPLICATION_TRANSFER_SCENE_NAME
        {
            public const byte ID = 0x34;
            public ByteValue sequenceNumber = 0;
            public ByteValue sceneId = 0;
            public IList<byte> sceneName = new List<byte>();
            public static implicit operator CTRL_REPLICATION_TRANSFER_SCENE_NAME(byte[] data)
            {
                CTRL_REPLICATION_TRANSFER_SCENE_NAME ret = new CTRL_REPLICATION_TRANSFER_SCENE_NAME();
                if (data != null)
                {
                    int index = 2;
                    ret.sequenceNumber = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.sceneId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.sceneName = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        if (data.Length > index) ret.sceneName.Add(data[index++]);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](CTRL_REPLICATION_TRANSFER_SCENE_NAME command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_CONTROLLER_REPLICATION.ID);
                ret.Add(ID);
                if (command.sequenceNumber.HasValue) ret.Add(command.sequenceNumber);
                if (command.sceneId.HasValue) ret.Add(command.sceneId);
                if (command.sceneName != null)
                {
                    foreach (var tmp in command.sceneName)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

