/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class FirmwareUpdateNvmIsValidCrc16Operation : RequestApiOperation
    {
        public FirmwareUpdateNvmIsValidCrc16Operation()
            : base(CommandTypes.CmdZWaveFirmwareUpdateNvm, false)
        {
        }

        protected override byte[] CreateInputParameters()
        {
            return new byte[] { (byte)FirmwareUpdateNvmFunctionality.FIRMWARE_UPDATE_NVM_IS_VALID_CRC16 };
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            byte[] res = ((DataReceivedUnit)ou).DataFrame.Payload;
            if (res != null && res.Length > 1)
            {
                SpecificResult.IsValid = res[1] > 0;
            }
            if (res != null && res.Length > 3)
            {
                SpecificResult.Crc16 = (ushort)((res[2] << 8) + res[3]);
            }
            base.SetStateCompleted(ou);
        }

        public FirmwareUpdateNvmIsValidCrc16Result SpecificResult
        {
            get { return (FirmwareUpdateNvmIsValidCrc16Result)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new FirmwareUpdateNvmIsValidCrc16Result();
        }
    }
    public class FirmwareUpdateNvmIsValidCrc16Result : ActionResult
    {
        public bool IsValid { get; set; }
        public ushort Crc16 { get; set; }
    }
}
