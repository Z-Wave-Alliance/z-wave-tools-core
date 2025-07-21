/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class MemoryPutBufferOperation : CallbackApiOperation
    {
        private ushort Offset { get; set; }
        private ushort Length { get; set; }
        private byte[] Data { get; set; }
        public MemoryPutBufferOperation(ushort offset, ushort length, byte[] data)
            : base(CommandTypes.CmdMemoryPutBuffer)
        {
            Offset = offset;
            Length = length;
            Data = data;
        }

        protected override byte[] CreateInputParameters()
        {
            byte[] ret = new byte[4 + Data.Length];
            ret[0] = (byte)(Offset >> 8);
            ret[1] = (byte)(Offset & 0xFF);
            ret[2] = (byte)(Length >> 8);
            ret[3] = (byte)(Length & 0xFF);
            for (int i = 0; i < Data.Length; i++)
            {
                ret[i + 4] = Data[i];
            }
            return ret;
        }

        protected override void OnCallbackInternal(DataReceivedUnit ou)
        {
            if (ou.DataFrame.Payload != null && ou.DataFrame.Payload.Length > 1)
            {
                SpecificResult.RetStatus = ou.DataFrame.Payload[1];
            }
        }

        public MemoryPutBufferResult SpecificResult
        {
            get { return (MemoryPutBufferResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new MemoryPutBufferResult();
        }
    }

    public class MemoryPutBufferResult : ActionResult
    {
        public byte RetStatus { get; set; }
    }
}
