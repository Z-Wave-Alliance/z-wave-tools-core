/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave;
using System;
using System.Text;

namespace UicApplication
{
    public abstract class UicApiOperation : ActionBase
    {
        protected int AckTimeout = 1000;
        public UicApiOperation(bool isExclusive)
            : base(isExclusive)
        {
            IsSequenceNumberRequired = true;
        }
    }
}
