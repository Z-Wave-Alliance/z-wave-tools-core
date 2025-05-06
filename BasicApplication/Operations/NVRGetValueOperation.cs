/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    ///HOST->ZW: REQ | 0x28 | offset | length
    ///ZW->HOST: RES | 0x28 | length | NVRValue
    /// </summary>
    public class NVRGetValueOperation : RequestApiOperation
    {
        private byte Offset { get; set; }
        private byte Lenght { get; set; }

        public NVRGetValueOperation(byte offset, byte length)
            : base(CommandTypes.CmdZWaveNVRGetValue, true)
        {
            Offset = offset;
            Lenght = length;
            TimeoutMs = 2000;
        }

        protected override byte[] CreateInputParameters()
        {
            return new byte[] { Offset, Lenght };
        }

        public NVRGetValueResult SpecificResult
        {
            get { return (NVRGetValueResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new NVRGetValueResult();
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            byte[] res = ((DataReceivedUnit)ou).DataFrame.Payload;
            SpecificResult.Lenght = Lenght;
            SpecificResult.NVRValue = res;
            base.SetStateCompleted(ou);
        }
    }

    public class NVRGetValueResult : ActionResult
    {
        public int Lenght { get; set; }
        public byte[] NVRValue { get; set; }
    }
}
