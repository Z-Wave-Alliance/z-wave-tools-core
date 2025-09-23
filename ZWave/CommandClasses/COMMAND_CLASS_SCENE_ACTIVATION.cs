/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_SCENE_ACTIVATION
    {
        public const byte ID = 0x2B;
        public const byte VERSION = 1;
        public partial class SCENE_ACTIVATION_SET
        {
            public const byte ID = 0x01;
            public ByteValue sceneId = 0;
            public ByteValue dimmingDuration = 0;
            public static implicit operator SCENE_ACTIVATION_SET(byte[] data)
            {
                SCENE_ACTIVATION_SET ret = new SCENE_ACTIVATION_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.sceneId = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                    ret.dimmingDuration = data.Length > index ? (ByteValue)data[index++] : ByteValue.Empty;
                }
                return ret;
            }
            public static implicit operator byte[](SCENE_ACTIVATION_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SCENE_ACTIVATION.ID);
                ret.Add(ID);
                if (command.sceneId.HasValue) ret.Add(command.sceneId);
                if (command.dimmingDuration.HasValue) ret.Add(command.dimmingDuration);
                return ret.ToArray();
            }
        }
    }
}

