/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.Layers;
using ZWave.Layers.Transport;
using UicApplication.Devices;
using ZWave.Layers.Application;
using ZWave.Enums;

namespace UicApplication
{
    public class UicApplicationLayer : ApplicationLayer
    {
        public UicApplicationLayer(ISessionLayer sessionLayer, IFrameLayer frameLayer, ITransportLayer transportLayer)
            : base(ApiTypes.Uic, sessionLayer, frameLayer, transportLayer)
        {
        }

        public UicClient CreateUicClient()
        {
            var sessionId = SessionLayer.NextSessionId();
            return new UicClient(sessionId, SessionLayer.CreateClient(sessionId),
                FrameLayer.CreateClient(sessionId), TransportLayer.CreateClient(sessionId));
        }

        public UicClient RegisterUicClient(UicTransportClient transportClient)
        {
            var sessionId = SessionLayer.NextSessionId();
            return new UicClient(sessionId, SessionLayer.CreateClient(sessionId),
                FrameLayer.CreateClient(sessionId), transportClient);
        }
    }
}
