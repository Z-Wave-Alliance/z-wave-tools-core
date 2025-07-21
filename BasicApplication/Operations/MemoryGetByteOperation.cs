/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class MemoryGetByteOperation : RequestApiOperation
    {
        public ushort Offset { get; set; }
        public MemoryGetByteOperation(ushort offset)
            : base(CommandTypes.CmdMemoryGetByte, false)
        {
            Offset = offset;
        }

        protected override byte[] CreateInputParameters()
        {
            return new byte[] { (byte)(Offset >> 8), (byte)(Offset & 0xFF) };
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            SpecificResult.RetValue = ((DataReceivedUnit)ou).DataFrame.Payload[0];
            base.SetStateCompleted(ou);
        }

        public MemoryGetByteResult SpecificResult
        {
            get { return (MemoryGetByteResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new MemoryGetByteResult();
        }
    }

    public class MemoryGetByteResult : ActionResult
    {
        public byte RetValue { get; set; }
    }
}
