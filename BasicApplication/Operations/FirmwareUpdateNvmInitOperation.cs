/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class FirmwareUpdateNvmInitOperation : RequestApiOperation
    {
        public FirmwareUpdateNvmInitOperation()
            : base(CommandTypes.CmdZWaveFirmwareUpdateNvm, false)
        {
            TimeoutMs = 2000;
        }

        protected override byte[] CreateInputParameters()
        {
            return new byte[] { (byte)FirmwareUpdateNvmFunctionality.FIRMWARE_UPDATE_NVM_INIT };
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            byte[] res = ((DataReceivedUnit)ou).DataFrame.Payload;
            if (res != null && res.Length > 1)
            {
                SpecificResult.IsSupported = res[1] > 0;
            }
            base.SetStateCompleted(ou);
        }

        public FirmwareUpdateNvmInitResult SpecificResult
        {
            get { return (FirmwareUpdateNvmInitResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new FirmwareUpdateNvmInitResult();
        }
    }
    public class FirmwareUpdateNvmInitResult : ActionResult
    {
        public bool IsSupported { get; set; }
    }
}
