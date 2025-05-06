/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZWave.Devices;
using ZWave.Enums;

namespace ZWave.BasicApplication
{
    public struct NodeProvision
    {
        public NodeTag Node { get; set; }
        public NetworkKeyS2Flags GrantedSchemes { get; set; }
        public Modes NodeOptions { get; set; }
        private int dsk0;
        private int dsk1;
        private int dsk2;
        private int dsk3;
        public byte[] DSK
        {
            get
            {
                return new byte[]
                {
                    (byte)(dsk0 >> 24),
                    (byte)(dsk0 >> 16),
                    (byte)(dsk0 >> 8),
                    (byte)(dsk0),
                    (byte)(dsk1 >> 24),
                    (byte)(dsk1 >> 16),
                    (byte)(dsk1 >> 8),
                    (byte)(dsk1),
                    (byte)(dsk2 >> 24),
                    (byte)(dsk2 >> 16),
                    (byte)(dsk2 >> 8),
                    (byte)(dsk2),
                    (byte)(dsk3 >> 24),
                    (byte)(dsk3 >> 16),
                    (byte)(dsk3 >> 8),
                    (byte)(dsk3),
                };
            }
        }

        public NodeProvision(NodeTag node, byte[] dsk, NetworkKeyS2Flags grantedSchemes)
            : this(node, Modes.NodeOptionNetworkWide, dsk, grantedSchemes)
        {

        }

        public NodeProvision(NodeTag node, Modes nodeOptions, byte[] dsk, NetworkKeyS2Flags grantedSchemes)
        {
            Node = node;
            NodeOptions = nodeOptions;
            dsk0 = 0;
            dsk1 = 0;
            dsk2 = 0;
            dsk3 = 0;
            if (dsk != null)
            {
                int index = 0;
                if (dsk.Length > index) dsk0 += dsk[index++] << 24;
                if (dsk.Length > index) dsk0 += dsk[index++] << 16;
                if (dsk.Length > index) dsk0 += dsk[index++] << 8;
                if (dsk.Length > index) dsk0 += dsk[index++];
                if (dsk.Length > index) dsk1 += dsk[index++] << 24;
                if (dsk.Length > index) dsk1 += dsk[index++] << 16;
                if (dsk.Length > index) dsk1 += dsk[index++] << 8;
                if (dsk.Length > index) dsk1 += dsk[index++];
                if (dsk.Length > index) dsk2 += dsk[index++] << 24;
                if (dsk.Length > index) dsk2 += dsk[index++] << 16;
                if (dsk.Length > index) dsk2 += dsk[index++] << 8;
                if (dsk.Length > index) dsk2 += dsk[index++];
                if (dsk.Length > index) dsk3 += dsk[index++] << 24;
                if (dsk.Length > index) dsk3 += dsk[index++] << 16;
                if (dsk.Length > index) dsk3 += dsk[index++] << 8;
                if (dsk.Length > index) dsk3 += dsk[index++];
            }
            GrantedSchemes = grantedSchemes;
        }

        private static NodeProvision _empty = new NodeProvision();
        public static NodeProvision Empty
        {
            get { return _empty; }
        }
    }
}
