/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZWave.Layers.Application;
using ZWave.Layers;
using ZWave.ProgrammerApplication.Devices;
using ZWave.Enums;

namespace ZWave.ProgrammerApplication
{
    public class ProgrammerApplicationLayer : ApplicationLayer
    {
        public ProgrammerApplicationLayer(ISessionLayer sessionLayer, IFrameLayer frameLayer, ITransportLayer transportLayer)
            : base(ApiTypes.Programmer, sessionLayer, frameLayer, transportLayer)
        {

        }

        public Programmer CreateProgrammer(ApplicationClient client)
        {
            Programmer ret = new Programmer(client.SessionId, client.SessionClient, client.FrameClient, client.TransportClient);
            return ret;
        }

        public Programmer CreateProgrammer()
        {
            var sessionId = SessionLayer.NextSessionId();
            Programmer ret = new Programmer(sessionId, SessionLayer.CreateClient(sessionId), FrameLayer.CreateClient(sessionId), TransportLayer.CreateClient(sessionId));
            return ret;
        }
    }
}
