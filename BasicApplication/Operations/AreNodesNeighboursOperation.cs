/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.BasicApplication.Enums;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    public class AreNodesNeighboursOperation : ControlApiOperation
    {
        private NodeTag Node1 { get; set; }
        private NodeTag Node2 { get; set; }
        public AreNodesNeighboursOperation(NodeTag node1, NodeTag node2)
            : base(CommandTypes.CmdAreNodesNeighbours, false)
        {
            Node1 = node1;
            Node2 = node2;
        }

        protected override byte[] CreateInputParameters()
        {
            return new byte[] { (byte)Node1.Id, (byte)Node2.Id };
        }
    }
}
