/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class NUnitListOperation: ControlNApiOperation
    {
        public NUnitListOperation()
            : base(CommandTypes.CmdZWaveNUnitList, true)
        {
        }

        protected override byte[] CreateInputParameters()
        {
            return null;
        }
    }
}
