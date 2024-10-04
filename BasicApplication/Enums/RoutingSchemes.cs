/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZWave.BasicApplication.Enums
{
    public enum RoutingSchemes
    {
        Idle = 0x00,
        Direct = 1,
        CachedRouteSr = 2,
        CachedRoute = 3,
        CachedRouteNLwr = 4,
        Route = 5,
        ResortDirect = 6,
        ResortExplore = 7
    }
}
