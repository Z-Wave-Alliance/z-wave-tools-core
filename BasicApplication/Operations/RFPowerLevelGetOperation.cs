/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class RFPowerLevelGetOperation : RequestApiOperation
    {
        public RFPowerLevelGetOperation()
            : base(CommandTypes.CmdZWaveRFPowerLevelGet, false)
        {
        }

        protected override byte[] CreateInputParameters()
        {
            return null;
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            SpecificResult.PowerLevel = ((DataReceivedUnit)ou).DataFrame.Payload[0];
            base.SetStateCompleted(ou);
        }

        public RFPowerLevelGetResult SpecificResult
        {
            get { return (RFPowerLevelGetResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new RFPowerLevelGetResult();
        }
    }

    public class RFPowerLevelGetResult : ActionResult
    {
        public byte PowerLevel { get; set; }
    }
}
