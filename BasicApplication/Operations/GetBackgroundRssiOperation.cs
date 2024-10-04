/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class GetBackgroundRssiOperation : RequestApiOperation
    {
        public GetBackgroundRssiOperation()
            : base(CommandTypes.CmdGetBackgroundRSSI, false)
        {
        }

        protected override byte[] CreateInputParameters()
        {
            return null;
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            byte[] res = ((DataReceivedUnit)ou).DataFrame.Payload;
            SpecificResult.BackgroundRSSILevels = res;
            base.SetStateCompleted(ou);
        }

        public GetBackgroundRssiResult SpecificResult
        {
            get { return (GetBackgroundRssiResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new GetBackgroundRssiResult();
        }
    }

    public class GetBackgroundRssiResult : ActionResult
    {
        public byte[] BackgroundRSSILevels { get; set; }
    }
}
