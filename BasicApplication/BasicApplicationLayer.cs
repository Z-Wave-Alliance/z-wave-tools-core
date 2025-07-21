/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.Layers;
using ZWave.BasicApplication.Devices;
using ZWave.Layers.Application;
using ZWave.Enums;
using Utils;
using System;
using ZWave.Exceptions;

namespace ZWave.BasicApplication
{
    public class BasicApplicationLayer : ApplicationLayer
    {
        public BasicApplicationLayer(ISessionLayer sessionLayer, IFrameLayer frameLayer, ITransportLayer transportLayer)
            : base(ApiTypes.Basic, sessionLayer, frameLayer, transportLayer)
        { }

        public Controller CreateController(IApplicationClient client)
        {
            Controller ret = null;
            try
            {
                ret = new Controller(client.SessionId, client.SessionClient, client.FrameClient, client.TransportClient);
            }
            catch (System.NullReferenceException)
            {
            }
            return ret;
        }

        public Controller CreateController(bool preInitNodes = true)
        {
            var sessionId = SessionLayer.NextSessionId();
            Controller ret = new Controller(sessionId, SessionLayer.CreateClient(sessionId), FrameLayer.CreateClient(sessionId), TransportLayer.CreateClient(sessionId), preInitNodes);
            return ret;
        }

        public EndDevice CreateEndDevice(IApplicationClient client, bool preInitNodes = true)
        {
            EndDevice ret = null;
            try
            {
                ret = new EndDevice(client.SessionId, client.SessionClient, client.FrameClient, client.TransportClient, preInitNodes);
            }
            catch (System.NullReferenceException)
            {
            }
            return ret;
        }

        public EndDevice CreateEndDevice(bool preInitNodes = true)
        {
            var sessionId = SessionLayer.NextSessionId();
            EndDevice ret = new EndDevice(sessionId, SessionLayer.CreateClient(sessionId), FrameLayer.CreateClient(sessionId), TransportLayer.CreateClient(sessionId), preInitNodes);
            return ret;
        }

        public BridgeController CreateBridgeController(IApplicationClient client, bool preInitNodes = true)
        {
            BridgeController ret = new BridgeController(client.SessionId, client.SessionClient, client.FrameClient, client.TransportClient, preInitNodes);
            return ret;
        }

        public BridgeController CreateBridgeController()
        {
            var sessionId = SessionLayer.NextSessionId();
            BridgeController ret = new BridgeController(sessionId, SessionLayer.CreateClient(sessionId), FrameLayer.CreateClient(sessionId), TransportLayer.CreateClient(sessionId));
            return ret;
        }

        public InstallerController CreateInstallerController(IApplicationClient client)
        {
            InstallerController ret = new InstallerController(client.SessionId, client.SessionClient, client.FrameClient, client.TransportClient);
            return ret;
        }

        public InstallerController CreateInstallerController()
        {
            var sessionId = SessionLayer.NextSessionId();
            InstallerController ret = new InstallerController(sessionId, SessionLayer.CreateClient(sessionId), FrameLayer.CreateClient(sessionId), TransportLayer.CreateClient(sessionId));
            return ret;
        }

        public TestInterfaceDevice CreateTestInterfaceDevice(IApplicationClient client)
        {
            TestInterfaceDevice ret = null;
            try
            {
                ret = new TestInterfaceDevice(client.SessionId, client.SessionClient, client.FrameClient, client.TransportClient);
                ret.ChipType = client.ChipType;
            }
            catch (System.NullReferenceException)
            {
            }
            return ret;
        }

        public TestInterfaceDevice CreateTestInterfaceDevice(string chipTypeString)
        {
            if (Enum.TryParse(chipTypeString, out ChipTypes chipType))
            {
                return CreateTestInterfaceDevice(chipType);
            }
            else
            {
                throw new OperationException();
            }
        }

        public TestInterfaceDevice CreateTestInterfaceDevice(ChipTypes chipType)
        {
            var sessionId = SessionLayer.NextSessionId();
            TestInterfaceDevice ret = new TestInterfaceDevice(sessionId, SessionLayer.CreateClient(sessionId), FrameLayer.CreateClient(sessionId), TransportLayer.CreateClient(sessionId));
            ret.ChipType = chipType;
            return ret;
        }
    }
}
