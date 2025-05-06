/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Devices;
using ZWave.Enums;
using ZWave.Layers;
using ZWave.Layers.Application;

namespace ZWave.BasicApplication
{
    public class XModemApplicationLayer : ApplicationLayer
    {
        public XModemApplicationLayer(ISessionLayer sessionLayer, IFrameLayer frameLayer, ITransportLayer transportLayer) :
            base(ApiTypes.XModem, sessionLayer, frameLayer, transportLayer)
        {
        }

        public XModemDevice CreateXModem()
        {
            var sessionId = SessionLayer.NextSessionId();
            return new XModemDevice(SessionLayer.CreateClient(sessionId), FrameLayer.CreateClient(sessionId), TransportLayer.CreateClient(sessionId));
        }
    }
}
