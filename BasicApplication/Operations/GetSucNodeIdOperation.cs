/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.BasicApplication.Enums;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    public class GetSucNodeIdOperation : RequestApiOperation
    {
        public GetSucNodeIdOperation(NetworkViewPoint network)
            : base(CommandTypes.CmdZWaveGetSucNodeId, false)
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
            SpecificResult.SucNode = new NodeTag(res[0]);
            if (_network.IsNodeIdBaseTypeLR && res.Length > 1)
            {
                SpecificResult.SucNode = new NodeTag((ushort)((res[0] << 8) + res[1]));
            }
            base.SetStateCompleted(ou);
        }

        public GetSucNodeIdResult SpecificResult
        {
            get { return (GetSucNodeIdResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new GetSucNodeIdResult();
        }
    }

    public class GetSucNodeIdResult : ActionResult
    {
        public NodeTag SucNode { get; set; }
    }
}
