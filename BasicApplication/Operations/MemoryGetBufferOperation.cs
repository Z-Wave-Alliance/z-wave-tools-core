/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class MemoryGetBufferOperation : RequestApiOperation
    {
        private ushort Offset { get; set; }
        private byte Length { get; set; }
        public MemoryGetBufferOperation(ushort offset, byte length)
            : base(CommandTypes.CmdMemoryGetBuffer, false)
        {
            Offset = offset;
            Length = length;
        }

        protected override byte[] CreateInputParameters()
        {
            return new byte[] { (byte)(Offset >> 8), (byte)(Offset & 0xFF), Length };
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            SpecificResult.RetValue = ((DataReceivedUnit)ou).DataFrame.Payload;
            base.SetStateCompleted(ou);
        }

        public MemoryGetBufferResult SpecificResult
        {
            get { return (MemoryGetBufferResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new MemoryGetBufferResult();
        }
    }

    public class MemoryGetBufferResult : ActionResult
    {
        public byte[] RetValue { get; set; }
    }
}
