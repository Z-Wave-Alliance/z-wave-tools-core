/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZWave.ZnifferApplication.Enums;
using ZWave.Enums;
using System.Threading;
using Utils;
using System.Collections;

namespace ZWave.ZnifferApplication
{
    public abstract class ZnifferApiTaskBase : ActionBase
    {
        public ZnifferApiTaskBase(bool isExclusive)
            : base(isExclusive)
        {
        }
    }
}
