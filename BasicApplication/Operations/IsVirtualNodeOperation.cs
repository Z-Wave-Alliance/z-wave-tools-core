/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.BasicApplication.Enums;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// 
    /// only use 8 bit for nodeID (regardless of basetype)
    /// </summary>
    public class IsVirtualNodeOperation : RequestApiOperation
    {
        private NodeTag Node { get; set; }
        public IsVirtualNodeOperation(NetworkViewPoint network, NodeTag node)
            : base(CommandTypes.CmdZWaveIsVirtualDeviceNode, false)
        {
            _network = network;
            Node = node;
        }

        protected override byte[] CreateInputParameters()
        {
            return new byte[] { (byte)Node.Id };
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            SpecificResult.RetValue = ((DataReceivedUnit)ou).DataFrame.Payload[0] == 1;
            base.SetStateCompleted(ou);
        }

        public IsVirtualNodeResult SpecificResult
        {
            get { return (IsVirtualNodeResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new IsVirtualNodeResult();
        }
    }

    public class IsVirtualNodeResult : ActionResult
    {
        public bool RetValue { get; set; }
    }
}
