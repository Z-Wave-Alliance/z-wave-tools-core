/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class FirmwareUpdateNvmSetNewImageOperation : RequestApiOperation
    {
        private bool IsNewImage { get; set; }
        public FirmwareUpdateNvmSetNewImageOperation(bool isNewImage)
            : base(CommandTypes.CmdZWaveFirmwareUpdateNvm, false)
        {
            IsNewImage = isNewImage;
        }

        protected override byte[] CreateInputParameters()
        {
            return new byte[] { 
                (byte)FirmwareUpdateNvmFunctionality.FIRMWARE_UPDATE_NVM_SET_NEW_IMAGE,
                (IsNewImage==true) ? (byte)0x01 : (byte)0x00};
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            byte[] res = ((DataReceivedUnit)ou).DataFrame.Payload;
            if (res != null && res.Length > 1)
            {
                SpecificResult.IsSet = res[1] > 0;
            }
            base.SetStateCompleted(ou);
        }

        public FirmwareUpdateNvmSetNewImageResult SpecificResult
        {
            get { return (FirmwareUpdateNvmSetNewImageResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new FirmwareUpdateNvmSetNewImageResult();
        }
    }
    public class FirmwareUpdateNvmSetNewImageResult : ActionResult
    {
        public bool IsSet { get; set; }
    }
}
