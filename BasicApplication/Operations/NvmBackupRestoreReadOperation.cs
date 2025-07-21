/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using Utils;
using ZWave.BasicApplication.Enums;
using ZWave.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class NvmBackupRestoreReadOperation : ApiOperation
    {
        readonly byte Length;
        readonly ushort Offset;
        public NvmBackupRestoreReadOperation(byte length, ushort offset)
            : base(true, CommandTypes.CmdZWaveNVMBackupRestore, true)
        {
            Length = length;
            Offset = offset;
        }

        protected ApiMessage message;
        private ApiHandler handler;

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 2000, message));
            ActionUnits.Add(new DataReceivedUnit(handler, SetStateCompleted));
        }

        protected override void CreateInstance()
        {
            byte offsetMSB = (byte)(Offset >> 8);
            byte offsetLSB = (byte)Offset;
            message = new ApiMessage(SerialApiCommands[0], new byte[] { 0x01/*Read*/, Length, offsetMSB, offsetLSB });
            message.SetSequenceNumber(SequenceNumber);
            handler = new ApiHandler(FrameTypes.Response, SerialApiCommands[0]);
            handler.AddConditions(ByteIndex.AnyValue, ByteIndex.AnyValue, new ByteIndex(offsetMSB), new ByteIndex(offsetLSB));
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            byte[] res = ((DataReceivedUnit)ou).DataFrame.Payload;
            if (res != null && res.Length > 4)
            {
                SpecificResult.Status = (NvmBackupRestoreStatuses)res[0];
                byte dataLength = res[1];
                SpecificResult.Length = dataLength;
                SpecificResult.Offset = (ushort)((res[2] << 8) + res[3]);
                SpecificResult.Data = new byte[dataLength];
                Array.Copy(res, 4, SpecificResult.Data, 0, dataLength);
            }
            base.SetStateCompleted(ou);
        }

        public NvmBackupRestoreReadResult SpecificResult
        {
            get { return (NvmBackupRestoreReadResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new NvmBackupRestoreReadResult();
        }
    }

    public class NvmBackupRestoreReadResult : ActionResult
    {
        public NvmBackupRestoreStatuses Status { get; set; }
        public byte Length { get; set; }
        public ushort Offset { get; set; }
        public byte[] Data { get; set; }
    }
}
