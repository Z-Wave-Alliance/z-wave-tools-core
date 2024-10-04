/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class NUnitRunOperation : ControlNApiOperation
    {
        private byte ScenarioId { get; set; }
        public NUnitRunOperation(byte scenarioId)
            : base(CommandTypes.CmdZWaveNUnitRun, true)
        {
            ScenarioId = scenarioId;
        }
        
        protected override byte[] CreateInputParameters()
        {
            return new byte[] { ScenarioId };
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            SpecificResult.RetValue = 0x01;
                //((OperationUnit)ou).CommandHandler.DataFrame.Payload[0] == 1;
            base.SetStateCompleted(ou);
        }

        public NUnitRunResult SpecificResult
        {
            get { return (NUnitRunResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new NUnitRunResult();
        }
    }
    public class NUnitRunResult : ActionResult
    {
        public byte RetValue { get; set; }
    }
}
