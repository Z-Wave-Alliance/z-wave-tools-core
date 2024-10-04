/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using Utils;
using ZWave.Enums;

namespace ZWave.Devices
{
    public struct NodeInfo
    {
        public byte Capability;
        public byte Security;
        public byte Properties1;
        public byte Basic;
        public byte Generic;
        public byte Specific;
        public byte LongRange;
        public DeviceOptions DeviceOptions
        {
            get
            {
                var ret = DeviceOptions.NoneListening;
                if ((Capability & 0x80) > 0)
                {
                    ret |= DeviceOptions.Listening;
                }
                if ((Security & 0x80) > 0)
                {
                    ret |= DeviceOptions.OptionalFunctionality;
                }
                if ((Security & 0x40) > 0)
                {
                    ret |= DeviceOptions.FreqListeningMode1000ms;
                }
                if ((Security & 0x20) > 0)
                {
                    ret |= DeviceOptions.FreqListeningMode250ms;
                }
                return ret;
            }
        }
        public bool IsEmpty
        {
            get { return Capability == 0 && Security == 0 && Properties1 == 0; }
        }

        public static implicit operator NodeInfo(byte[] data)
        {
            NodeInfo ret = new NodeInfo();
            if (data != null)
            {
                int index = 0;
                ret.Capability = data.Length > index ? data[index++] : (byte)0x00;
                ret.Security = data.Length > index ? data[index++] : (byte)0x00;
                ret.Properties1 = data.Length > index ? data[index++] : (byte)0x00;
                ret.Basic = data.Length > index ? data[index++] : (byte)0x00;
                ret.Generic = data.Length > index ? data[index++] : (byte)0x00;
                ret.Specific = data.Length > index ? data[index++] : (byte)0x00;
                ret.LongRange = data.Length > index ? data[index++] : (byte)0x00;
            }
            return ret;
        }
        public static implicit operator byte[] (NodeInfo nodeInfo)
        {
            byte[] ret;
            if (nodeInfo.LongRange == 0)
            {
                ret = new byte[]
                    {
                        nodeInfo.Capability,
                        nodeInfo.Security,
                        nodeInfo.Properties1,
                        nodeInfo.Basic,
                        nodeInfo.Generic,
                        nodeInfo.Specific
                    };
            }
            else
            {
                ret = new byte[]
                    {
                        nodeInfo.Capability,
                        nodeInfo.Security,
                        nodeInfo.Properties1,
                        nodeInfo.Basic,
                        nodeInfo.Generic,
                        nodeInfo.Specific,
                        nodeInfo.LongRange
                    };
            }
            return ret;
        }

        public static NodeInfo UpdateTo(NodeInfo nodeInfo, DeviceOptions deviceOptions, byte generic, byte specific)
        {
            var ret = nodeInfo;
            var capabuility = nodeInfo.Capability;
            var security = nodeInfo.Security;

            if (deviceOptions.HasFlag(DeviceOptions.Listening))
            {
                capabuility |= 0x80;
            }
            else
            {
                capabuility &= 0x7F;
            }

            if (deviceOptions.HasFlag(DeviceOptions.OptionalFunctionality))
            {
                security |= 0x80;
            }
            else
            {
                security &= 0x7F;
            }

            if (deviceOptions.HasFlag(DeviceOptions.FreqListeningMode1000ms))
            {
                security |= 0x40;
            }
            else
            {
                security &= 0xBF;
            }

            if (deviceOptions.HasFlag(DeviceOptions.FreqListeningMode250ms))
            {
                security |= 0x20;
            }
            else
            {
                security &= 0xDF;
            }

            ret.Capability = capabuility;
            ret.Security = security;
            ret.Generic = generic;
            ret.Specific = specific;
            return ret;
        }

        public static NodeInfo UpdateTo(NodeInfo nodeInfo, byte basic, byte generic, byte specific)
        {
            var ret = nodeInfo;
            ret.Basic = basic;
            ret.Generic = generic;
            ret.Specific = specific;
            return ret;
        }

        public static NodeInfo UpdateTo(NodeInfo nodeInfo, byte generic, byte specific)
        {
            var ret = nodeInfo;
            ret.Generic = generic;
            ret.Specific = specific;
            return ret;
        }

        public override string ToString()
        {
            return ((byte[])this).GetHex();
        }
    }
}
