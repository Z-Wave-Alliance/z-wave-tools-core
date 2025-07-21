/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.BasicApplication.Enums;
using Utils;
using ZWave.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class RFPowerLevelSetOperation : ControlApiOperation
    {
        public byte PowerLevel { get; set; }
        public RFPowerLevelSetOperation(byte powerLevel)
            : base(CommandTypes.CmdZWaveRFPowerLevelSet, false)
        {
            PowerLevel = powerLevel;
        }

        protected override byte[] CreateInputParameters()
        {
            return new byte[] { PowerLevel };
        }
        private ApiHandler handlerRet;

        protected override void CreateWorkflow()
        {
            base.CreateWorkflow();
            ActionUnits.Add(new DataReceivedUnit(handlerRet, SetStateCompleted));
        }

        protected override void CreateInstance()
        {
            base.CreateInstance();
            handlerRet = new ApiHandler(FrameTypes.Response, SerialApiCommands[0]);
            handlerRet.AddConditions(new ByteIndex(PowerLevel));
        }
    }
}
