/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;
using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class NvmBackupRestoreWriteOperation : RequestApiOperation
    {
        readonly byte Length;
        readonly int Offset;
        readonly byte[] Data;
        public NvmBackupRestoreWriteOperation(byte length, int offset, byte[] data)
            : base(CommandTypes.CmdZWaveNVMBackupRestore, true)
        {
            Length = length;
            Offset = offset;
            Data = data;
        }

        protected override byte[] CreateInputParameters()
        {
            byte offsetMSB = (byte)(Offset >> 8);
            byte offsetLSB = (byte)Offset;
            List<byte> request = new List<byte>();
            request.Add(0x02/*Write*/);
            request.Add(Length);
            request.Add(offsetMSB);
            request.Add(offsetLSB);
            request.AddRange(Data);
            return request.ToArray();
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            byte[] res = ((DataReceivedUnit)ou).DataFrame.Payload;
            if (res != null && res.Length > 3)
            {
                SpecificResult.Status = (NvmBackupRestoreStatuses)res[0];
                SpecificResult.Length = res[1]; 
                SpecificResult.Offset = (ushort)((res[2] << 8) + res[3]);
            }
            base.SetStateCompleted(ou);
        }

        public NvmBackupRestoreWriteResult SpecificResult
        {
            get { return (NvmBackupRestoreWriteResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new NvmBackupRestoreWriteResult();
        }
    }

    public class NvmBackupRestoreWriteResult : ActionResult
    {
        public NvmBackupRestoreStatuses Status { get; set; }
        public byte Length { get; set; }
        public ushort Offset { get; set; }
    }
}
