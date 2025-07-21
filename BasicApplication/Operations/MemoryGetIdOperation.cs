/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.BasicApplication.Enums;
using Utils;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    public class MemoryGetIdOperation : RequestApiOperation
    {
        public MemoryGetIdOperation(NetworkViewPoint network)
            : base(CommandTypes.CmdMemoryGetId, false)
        {
            _network = network;
        }

        protected override byte[] CreateInputParameters()
        {
            return null;
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            byte[] res = ((DataReceivedUnit)ou).DataFrame.Payload;
            SpecificResult.HomeId = new byte[] { res[0], res[1], res[2], res[3] };
            SpecificResult.Node = new NodeTag(res[4]);
            if (_network.IsNodeIdBaseTypeLR && res.Length > 5)
            {
                SpecificResult.Node = new NodeTag((ushort)((res[4] << 8) + res[5]));
            }
            _network.HomeId = SpecificResult.HomeId;
            _network.NodeTag = SpecificResult.Node;
            base.SetStateCompleted(ou);
        }

        public override string AboutMe()
        {
            return string.Format("Id={0}, HomeId={1}", SpecificResult.Node.Id, SpecificResult.HomeId == null ? "" : Tools.GetHexShort(SpecificResult.HomeId));
        }

        public MemoryGetIdResult SpecificResult
        {
            get { return (MemoryGetIdResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new MemoryGetIdResult();
        }
    }

    public class MemoryGetIdResult : ActionResult
    {
        public NodeTag Node { get; set; }
        public byte[] HomeId { get; set; }
    }
}
