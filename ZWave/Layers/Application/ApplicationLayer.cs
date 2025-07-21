/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections;
using ZWave.Enums;

namespace ZWave.Layers.Application
{
    public abstract class ApplicationLayer : IApplicationLayer
    {
        public bool SuppressDebugOutput { get; set; }
        public ISessionLayer SessionLayer { get; set; }
        public IFrameLayer FrameLayer { get; set; }
        public ITransportLayer TransportLayer { get; set; }
        public ApiTypes ApiType { get; set; }
        private ApplicationLayer()
        {
        }

        public ApplicationLayer(ApiTypes apiType, ISessionLayer sessionLayer, IFrameLayer frameLayer, ITransportLayer transportLayer)
        {
            ApiType = apiType;
            SessionLayer = sessionLayer;
            FrameLayer = frameLayer;
            TransportLayer = transportLayer;
        }

        public ApplicationClient CreateClient()
        {
            var sessionId = SessionLayer.NextSessionId();
            ApplicationClient ret = new ApplicationClient(ApiType, sessionId, SessionLayer.CreateClient(sessionId), FrameLayer.CreateClient(sessionId), TransportLayer.CreateClient(sessionId));
            return ret;
        }
    }
}
