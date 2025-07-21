/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.Layers;
using ZWave.Layers.Transport;
using UicApplication.Devices;
using ZWave.Layers.Application;
using ZWave.Enums;

namespace UicApplication
{
    public class UicDeviceApplicationLayer : ApplicationLayer
    {
        public UicDeviceApplicationLayer(ISessionLayer sessionLayer, IFrameLayer frameLayer, ITransportLayer transportLayer)
            : base(ApiTypes.Uic, sessionLayer, frameLayer, transportLayer)
        {
        }

        public UicDevice CreateUicDevice()
        {
            ushort sessionId = SessionLayer.NextSessionId();
            return new UicDevice(sessionId, SessionLayer.CreateClient(sessionId),
                FrameLayer.CreateClient(sessionId), TransportLayer.CreateClient(sessionId));
        }

        public UicDevice RegisterUicDevice(UicTransportClient transportClient)
        {
            ushort sessionId = SessionLayer.NextSessionId();
            return new UicDevice(sessionId, SessionLayer.CreateClient(sessionId),
                FrameLayer.CreateClient(sessionId), transportClient);
        }
    }
}
