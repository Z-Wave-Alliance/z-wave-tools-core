/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.Layers;
using ZWave.Layers.Transport;
using ZWave.ZipApplication.Devices;
using ZWave.Layers.Application;
using ZWave.Enums;

namespace ZWave.ZipApplication
{
    public class ZipApplicationLayer : ApplicationLayer
    {
        public ZipApplicationLayer(ISessionLayer sessionLayer, IFrameLayer frameLayer, ITransportLayer transportLayer)
            : base(ApiTypes.Zip, sessionLayer, frameLayer, transportLayer)
        {
        }

        public ZipDevice CreateZipDevice()
        {
            var sessionId = SessionLayer.NextSessionId();
            ZipDevice ret = new ZipDevice
                (sessionId, SessionLayer.CreateClient(sessionId), FrameLayer.CreateClient(sessionId), TransportLayer.CreateClient(sessionId));
            return ret;
        }

        public ZipController CreateZipController()
        {
            var sessionId = SessionLayer.NextSessionId();
            ZipController ret = new ZipController(sessionId, SessionLayer.CreateClient(sessionId), FrameLayer.CreateClient(sessionId), TransportLayer.CreateClient(sessionId));            
            return ret;
        }

        public ISocketListener CreateZipUpdListener()
        {
            var ret = new UdpClientTransportListener();
            return ret;
        }
    }
}
