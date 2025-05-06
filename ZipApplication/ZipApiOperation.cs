/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿namespace ZWave.ZipApplication
{
    public abstract class ZipApiOperation : ActionBase
    {
        protected int AckTimeout = 66000;
        protected byte[] _headerExtension;
        public ZipApiOperation(bool isExclusive)
            : base(isExclusive)
        {
            IsSequenceNumberRequired = true;
        }
    }
}
