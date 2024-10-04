/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class MemoryPutByteOperation : ControlApiOperation
    {
        public ushort Offset { get; set; }
        public byte Data { get; set; }
        public MemoryPutByteOperation(ushort offset, byte data)
            : base(CommandTypes.CmdMemoryPutByte, false)
        {
            Offset = offset;
            Data = data;
        }

        protected override byte[] CreateInputParameters()
        {
            return new byte[] { (byte)(Offset >> 8), (byte)(Offset & 0xFF), Data };
        }
    }
}
