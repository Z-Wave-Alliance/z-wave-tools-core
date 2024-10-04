/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class RFPowerlevelRediscoverySetOperation : ControlNApiOperation
    {
        private byte PowerLevel { get; set; }
        public RFPowerlevelRediscoverySetOperation(byte powerLevel)
            : base(CommandTypes.CmdZWaveRFPowerlevelRediscoverySet)
        {
            PowerLevel = powerLevel;
        }

        protected override byte[] CreateInputParameters()
        {
            return new byte[] { PowerLevel };
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ActionResult();
        }
    }
}
