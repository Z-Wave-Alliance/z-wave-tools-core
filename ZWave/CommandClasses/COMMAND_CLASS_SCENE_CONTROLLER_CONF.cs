/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_SCENE_CONTROLLER_CONF
    {
        public const byte ID = 0x2D;
        public const byte VERSION = 1;
        public partial class SCENE_CONTROLLER_CONF_GET
        {
            public const byte ID = 0x02;
            public ByteValue groupId = 0;
            public static implicit operator SCENE_CONTROLLER_CONF_GET(byte[] data)
            {
                SCENE_CONTROLLER_CONF_GET ret = new SCENE_CONTROLLER_CONF_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.groupId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SCENE_CONTROLLER_CONF_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SCENE_CONTROLLER_CONF.ID);
                ret.Add(ID);
                if (command.groupId.HasValue) ret.Add(command.groupId);
                return ret.ToArray();
            }
        }
        public partial class SCENE_CONTROLLER_CONF_REPORT
        {
            public const byte ID = 0x03;
            public ByteValue groupId = 0;
            public ByteValue sceneId = 0;
            public ByteValue dimmingDuration = 0;
            public static implicit operator SCENE_CONTROLLER_CONF_REPORT(byte[] data)
            {
                SCENE_CONTROLLER_CONF_REPORT ret = new SCENE_CONTROLLER_CONF_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.groupId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.sceneId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.dimmingDuration = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SCENE_CONTROLLER_CONF_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SCENE_CONTROLLER_CONF.ID);
                ret.Add(ID);
                if (command.groupId.HasValue) ret.Add(command.groupId);
                if (command.sceneId.HasValue) ret.Add(command.sceneId);
                if (command.dimmingDuration.HasValue) ret.Add(command.dimmingDuration);
                return ret.ToArray();
            }
        }
        public partial class SCENE_CONTROLLER_CONF_SET
        {
            public const byte ID = 0x01;
            public ByteValue groupId = 0;
            public ByteValue sceneId = 0;
            public ByteValue dimmingDuration = 0;
            public static implicit operator SCENE_CONTROLLER_CONF_SET(byte[] data)
            {
                SCENE_CONTROLLER_CONF_SET ret = new SCENE_CONTROLLER_CONF_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.groupId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.sceneId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.dimmingDuration = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SCENE_CONTROLLER_CONF_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SCENE_CONTROLLER_CONF.ID);
                ret.Add(ID);
                if (command.groupId.HasValue) ret.Add(command.groupId);
                if (command.sceneId.HasValue) ret.Add(command.sceneId);
                if (command.dimmingDuration.HasValue) ret.Add(command.dimmingDuration);
                return ret.ToArray();
            }
        }
    }
}

