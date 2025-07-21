/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ZWave.Enums;

namespace ZWave.BasicApplication.EmulatedLink
{
    public class BasicLinkModuleMemory
    {
        private static int SharedCounter = 1;
        public ControllerCapabilities ControllerCapability { get; set; }
        public byte NodeInfoCapability { get; set; }
        public byte NodeId { get; set; }
        public byte[] HomeId { get; set; }
        public byte[] CmdClasses { get; set; }
        public byte AddOrReplaceNodeId { get; set; }
        public bool IsAddingNode { get; set; }
        public bool IsShiftNode { get; set; }
        public bool IsReplacingNode { get; set; }
        public bool IsRemovingNode { get; set; }
        public byte FuncId { get; set; }
        public bool IsRFReceiveMode { get; set; }
        public ushort SucNodeId { get; set; }
        public byte Basic { get; set; }
        public byte Generic { get; set; }
        public byte Specific { get; set; }
        public List<byte> NodesList { get; set; } = new List<byte>();
        public bool IsNifResponseEnabled { get; set; } = true;

        //private byte _nextNodeId;
        private readonly byte _defaultNodeId;

        public BasicLinkModuleMemory(byte defaultNodeId)
        {
            _defaultNodeId = defaultNodeId;
            Basic = 1;
            Generic = 2;
            Specific = 1;
            Reset();
        }

        public static void ResetSharedIdCounter()
        {
            SharedCounter = 1;
        }

        public void Reset()
        {
            NodeId = _defaultNodeId;
            var home = Interlocked.Increment(ref SharedCounter);
            HomeId = BitConverter.GetBytes(home);
            IsRFReceiveMode = true;
            SucNodeId = 0;
            IsAddingNode = false;
            IsRemovingNode = false;
            IsReplacingNode = false;
            FuncId = 0;
            NodesList = new List<byte> { NodeId };
            ControllerCapability = ControllerCapabilities.IS_REAL_PRIMARY;
        }

        internal byte SeedNextNodeId()
        {
            var nextNodeId = (byte)(NodesList.Max() + 1);
            var index = 0;
            while (NodesList.Any(i => i == nextNodeId) && index++ < 255)
            {
                nextNodeId = (byte)(nextNodeId + 1);
            }
            return nextNodeId;
        }
    }
}
