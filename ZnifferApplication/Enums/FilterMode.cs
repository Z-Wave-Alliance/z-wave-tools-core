/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ZWave.ZnifferApplication.Enums
{
    [Serializable]
    public enum FilterMode
    {
        Equal,
        StartsWith,
        EndsWith,
        Contains
    }
}
