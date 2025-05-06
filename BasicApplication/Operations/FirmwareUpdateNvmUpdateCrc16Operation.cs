/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Collections.Generic;
using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class FirmwareUpdateNvmUpdateCrc16Operation: RequestApiOperation
    {
        private int Offset { get; set; }
        private ushort Length { get; set; }
        private ushort Seed { get; set; }
        public FirmwareUpdateNvmUpdateCrc16Operation(int offset, ushort length, ushort seed)
            : base(CommandTypes.CmdZWaveFirmwareUpdateNvm, false)
        {
            Offset = offset;
            Length = length;
            Seed = seed;
        }

        protected override byte[] CreateInputParameters()
        {
            byte offset1 = (byte)((Offset >> 16) & 0xFF);
            byte offset2 = (byte)((Offset >> 8) & 0xFF);
            byte offset3 = (byte)(Offset & 0xFF);

            byte length1 = (byte)((Length >> 8) & 0xFF);
            byte length2 = (byte)(Length & 0xFF);

            byte seed1 = (byte)((Seed >> 8) & 0xFF);
            byte seed2 = (byte)(Seed & 0xFF);

            List<byte> request = new List<byte>();

            request.Add((byte)FirmwareUpdateNvmFunctionality.FIRMWARE_UPDATE_NVM_UPDATE_CRC16);
            request.Add(offset1);
            request.Add(offset2);
            request.Add(offset3);
            request.Add(length1);
            request.Add(length2);
            request.Add(seed1);
            request.Add(seed2);

            return request.ToArray();
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            byte[] res = ((DataReceivedUnit)ou).DataFrame.Payload;
            if (res != null && res.Length > 2 )
            {
                SpecificResult.Crc16 = (ushort)((res[1] << 8) + res[2]);
            }
            base.SetStateCompleted(ou);
        }

        public FirmwareUpdateNvmUpdateCrc16Result SpecificResult
        {
            get { return (FirmwareUpdateNvmUpdateCrc16Result)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new FirmwareUpdateNvmUpdateCrc16Result();
        }
    }
    public class FirmwareUpdateNvmUpdateCrc16Result : ActionResult
    {
        public ushort Crc16 { get; set; }
    }
}
