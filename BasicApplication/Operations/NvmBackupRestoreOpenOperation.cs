/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class NvmBackupRestoreOpenOperation : RequestApiOperation
    {
        public NvmBackupRestoreOpenOperation()
            : base(CommandTypes.CmdZWaveNVMBackupRestore, false)
        {
        }

        protected override byte[] CreateInputParameters()
        {
            // HOST->ZW: REQ | 0x2E | Operation | Lenght
            return new byte[] { 0x00, 0x00 };
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            byte[] res = ((DataReceivedUnit)ou).DataFrame.Payload;
            if (res != null && res.Length > 2)
            {
                SpecificResult.Status = (NvmBackupRestoreStatuses)res[0];
                if (res[0] == 0x00)
                {
                    SpecificResult.NvmSize = (short)((res[2] << 8) + res[3]);
                }
            }
            base.SetStateCompleted(ou);
        }

        public NvmBackupRestoreOpenResult SpecificResult
        {
            get { return (NvmBackupRestoreOpenResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new NvmBackupRestoreOpenResult();
        }
    }

    public class NvmBackupRestoreOpenResult : ActionResult
    {
        public NvmBackupRestoreStatuses Status { get; set; }
        public short NvmSize { get; set; }
    }
}
