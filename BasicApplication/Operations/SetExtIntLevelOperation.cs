/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class SetExtIntLevelOperation : ControlNApiOperation
    {
        private byte IntSrc { get; set; }
        private bool TriggerLevel { get; set; }
        public SetExtIntLevelOperation(byte intSrc, bool triggerLevel)
            : base(CommandTypes.CmdZWaveSetExtIntLevel)
        {
            IntSrc = intSrc;
            TriggerLevel = triggerLevel;
        }

        protected override byte[] CreateInputParameters()
        {
            return new byte[] { IntSrc, (byte)(TriggerLevel ? 0x01 : 0x00) };
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ActionResult();
        }
    }
}
