/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.Layers;
using ZWave.Layers.Transport;
using ZWave.Layers.Application;
using ZWave.ZnifferApplication.Devices;
using ZWave.Enums;

namespace ZWave.ZnifferApplication
{
    public class ZnifferApplicationLayer : ApplicationLayer
    {
        public ZnifferApplicationLayer(ISessionLayer sessionLayer, IFrameLayer frameLayer, ITransportLayer transportLayer)
            : base(ApiTypes.Zniffer, sessionLayer, frameLayer, transportLayer)
        {
        }

        public Zniffer CreateZniffer()
        {
            var sessionId = SessionLayer.NextSessionId();
            Zniffer ret = new Zniffer(sessionId, SessionLayer.CreateClient(sessionId), FrameLayer.CreateClient(sessionId), TransportLayer.CreateClient(sessionId));
            return ret;
        }

        public Zniffer CreateZniffer(IApplicationClient client)
        {
            Zniffer ret = new Zniffer(client.SessionId, client.SessionClient, client.FrameClient, client.TransportClient);
            return ret;
        }

        public SnifferPti CreateSnifferPti()
        {
            var sessionId = SessionLayer.NextSessionId();
            var sessionCLient = SessionLayer.CreateClient(sessionId);
            sessionCLient.SuppressDebugOutput = true;
            SnifferPti ret = new SnifferPti(sessionId, sessionCLient, FrameLayer.CreateClient(sessionId), TransportLayer.CreateClient(sessionId));
            return ret;
        }

        public SnifferPti CreateSnifferPti(IApplicationClient client)
        {
            SnifferPti ret = new SnifferPti(client.SessionId, client.SessionClient, client.FrameClient, client.TransportClient);
            return ret;
        }
    }
}
