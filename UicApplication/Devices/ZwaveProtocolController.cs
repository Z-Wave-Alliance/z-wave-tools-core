/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Net;
using Utils;
using ZWave.CommandClasses;
using ZWave.Devices;
using ZWave.Enums;
using ZWave.Layers;
using ZWave.Layers.Application;
using ZWave.Operations;

namespace UicApplication.Devices
{
    public class ZwaveProtocolController : UicClient
    {
        public string UnId { get; set; }

        internal ZwaveProtocolController(byte sessionId, ISessionClient sc, IFrameClient fc, ITransportClient tc)
            : base( sessionId, sc, fc, tc)
        {
            
        }
    }        
}
