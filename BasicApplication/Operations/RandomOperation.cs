/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{

    public class RandomOperation : RequestApiOperation
    {
        public RandomOperation()
            : base(CommandTypes.CmdZWaveRandom, false)
        {
        }

        protected override byte[] CreateInputParameters()
        {
            return null;
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            SpecificResult.RetValue = ((DataReceivedUnit)ou).DataFrame.Payload[0];
            base.SetStateCompleted(ou);
        }

        public override string AboutMe()
        {
            return string.Format("Value=0x{0:X2}", SpecificResult.RetValue);
        }

        public RandomResult SpecificResult
        {
            get { return (RandomResult)Result; }
        }
        
        protected override ActionResult CreateOperationResult()
        {
            return new RandomResult();
        }
    }

    public class RandomResult : ActionResult
    {
        public byte RetValue { get; set; }
    }
}
