/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.Layers.Application;
using ZWave.Layers;
using ZWave.Enums;

namespace ZWave.TextApplication
{
    public class TextApplicationLayer : ApplicationLayer
    {
        public TextApplicationLayer(ISessionLayer sessionLayer, IFrameLayer frameLayer, ITransportLayer transportLayer)
            : base(ApiTypes.Text, sessionLayer, frameLayer, transportLayer)
        {
        }

        public TextApplicationLayer(ApiTypes apiType, ISessionLayer sessionLayer, IFrameLayer frameLayer, ITransportLayer transportLayer)
            : base(apiType, sessionLayer, frameLayer, transportLayer)
        {
        }

        public TextDevice CreateTextDevice(IApplicationClient client)
        {
            IFrameClient frameClient;
            if (client.FrameClient is TextFrameClient)
            {
                frameClient = client.FrameClient;
            }
            else
            {
                client.FrameClient.Dispose();
                frameClient = FrameLayer.CreateClient(client.SessionId);
            }
            TextDevice ret = new TextDevice(ApiType, client.SessionId, client.SessionClient, frameClient, client.TransportClient);
            return ret;
        }

        public TextDevice CreateTextDevice()
        {
            var sessionId = SessionLayer.NextSessionId();
            TextDevice ret = new TextDevice(ApiType, sessionId, SessionLayer.CreateClient(sessionId), FrameLayer.CreateClient(sessionId), TransportLayer.CreateClient(sessionId));
            return ret;
        }
    }
}
