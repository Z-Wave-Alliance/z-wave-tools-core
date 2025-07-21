/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.Layers;
using ZWave.BasicApplication.Operations;

namespace ZWave.BasicApplication.Devices
{
    public class InstallerController : Controller
    {
        internal InstallerController(ushort sessionId, ISessionClient sc, IFrameClient fc, ITransportClient tc)
            : base(sessionId, sc, fc, tc)
        { }

        public GetTransmitCountResult GetTransmitCount()
        {
            return (GetTransmitCountResult)Execute(new GetTransmitCountOperation());
        }

        public ActionResult ResetTransmitCount()
        {
            return Execute(new ResetTransmitCountOperation());
        }

        public ActionResult StoreHomeId(byte[] homeId, byte nodeId)
        {
            return Execute(new StoreHomeIdOperation(homeId, nodeId));
        }

        public ActionResult StoreNodeInfo(byte nodeId, byte[] nodeInfo)
        {
            return Execute(new StoreNodeInfoOperation(nodeId, nodeInfo));
        }
    }
}
