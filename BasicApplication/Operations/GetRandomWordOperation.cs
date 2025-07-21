/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class GetRandomWordOperation : RequestApiOperation
    {
        private byte Count { get; set; }
        public GetRandomWordOperation(byte count)
            : base(CommandTypes.CmdZWaveGetRandom, false)
        {
            Count = count;
        }

        protected override byte[] CreateInputParameters()
        {
            return new byte[] { Count };
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            SpecificResult.RetValue = ((DataReceivedUnit)ou).DataFrame.Payload;
            base.SetStateCompleted(ou);
        }

        public GetRandomWordResult SpecificResult
        {
            get { return (GetRandomWordResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new GetRandomWordResult();
        }
    }

    public class GetRandomWordResult : ActionResult
    {
        public byte[] RetValue { get; set; }
    }
}
