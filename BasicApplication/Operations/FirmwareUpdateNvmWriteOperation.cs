/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Collections.Generic;
using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class FirmwareUpdateNvmWriteOperation : RequestApiOperation
    {
        private int Offset { get; set; }
        private int Length { get; set; }
        private byte[] Buffer { get; set; }
        public FirmwareUpdateNvmWriteOperation(int offset, ushort length, byte[] buffer)
            : base(CommandTypes.CmdZWaveFirmwareUpdateNvm, false)
        {
            Offset = offset;
            Length = length;
            Buffer = buffer;
        }

        protected override byte[] CreateInputParameters()
        {
            byte offset1 = (byte)((Offset >> 16) & 0xFF);
            byte offset2 = (byte)((Offset >> 8) & 0xFF);
            byte offset3 = (byte)(Offset & 0xFF);

            byte length1 = (byte)((Length >> 8) & 0xFF);
            byte length2 = (byte)(Length & 0xFF);

            List<byte> request = new List<byte>();

            request.Add((byte)FirmwareUpdateNvmFunctionality.FIRMWARE_UPDATE_NVM_WRITE);
            request.Add(offset1);
            request.Add(offset2);
            request.Add(offset3);
            request.Add(length1);
            request.Add(length2);
            request.AddRange(Buffer);

            return request.ToArray();
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            byte[] res = ((DataReceivedUnit)ou).DataFrame.Payload;
            if (res != null && res.Length > 1)
            {
                SpecificResult.IsOk = res[1] > 0;
            }
            base.SetStateCompleted(ou);
        }

        public FirmwareUpdateNvmWriteResult SpecificResult
        {
            get { return (FirmwareUpdateNvmWriteResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new FirmwareUpdateNvmWriteResult();
        }
    }
    public class FirmwareUpdateNvmWriteResult : ActionResult
    {
        public bool IsOk { get; set; }
    }
}
