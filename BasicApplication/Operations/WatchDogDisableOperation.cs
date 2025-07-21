/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class WatchDogDisableOperation : ControlNApiOperation
    {
        public WatchDogDisableOperation()
            : base(CommandTypes.CmdZWaveWatchDogDisable)
        { }

        protected override byte[] CreateInputParameters()
        {
            return null;
        }
    }
}
