/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class FirmwareUpdateNvmGetNewImageOperation : RequestApiOperation
    {
        public FirmwareUpdateNvmGetNewImageOperation()
            : base(CommandTypes.CmdZWaveFirmwareUpdateNvm, false)
        {
        }

        protected override byte[] CreateInputParameters()
        {
            return new byte[] { (byte)FirmwareUpdateNvmFunctionality.FIRMWARE_UPDATE_NVM_GET_NEW_IMAGE };
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            byte[] res = ((DataReceivedUnit)ou).DataFrame.Payload;
            if (res != null && res.Length > 1)
            {
                SpecificResult.IsNewImage = res[1] > 0;
            }
            base.SetStateCompleted(ou);
        }

        public FirmwareUpdateNvmGetNewImageResult SpecificResult
        {
            get { return (FirmwareUpdateNvmGetNewImageResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new FirmwareUpdateNvmGetNewImageResult();
        }
    }
    public class FirmwareUpdateNvmGetNewImageResult : ActionResult
    {
        public bool IsNewImage { get; set; }
    }
}
