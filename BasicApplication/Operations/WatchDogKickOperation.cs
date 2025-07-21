/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{

    public class WatchDogKickOperation : ControlNApiOperation
    {
        public WatchDogKickOperation()
            : base(CommandTypes.CmdZWaveWatchDogKick)
        { }

        protected override byte[] CreateInputParameters()
        {
            return null;
        }

      
    }
}
