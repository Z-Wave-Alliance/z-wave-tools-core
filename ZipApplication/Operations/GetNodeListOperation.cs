/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;
using ZWave.CommandClasses;
using ZWave.Devices;

namespace ZWave.ZipApplication.Operations
{
    public class GetNodeListOperation : RequestDataOperation
    {
        public GetNodeListOperation(int timeoutMs)
            : base(null, null,
            COMMAND_CLASS_NETWORK_MANAGEMENT_PROXY_V4.ID,
            COMMAND_CLASS_NETWORK_MANAGEMENT_PROXY_V4.NODE_LIST_REPORT.ID,
            timeoutMs)
        {
            Data = new COMMAND_CLASS_NETWORK_MANAGEMENT_PROXY_V4.NODE_LIST_GET()
            {
                seqNo = SequenceNumber
            };
        }

        protected override void OnReceived(DataReceivedUnit ou)
        {
            var packet = (COMMAND_CLASS_NETWORK_MANAGEMENT_PROXY_V4.NODE_LIST_REPORT)ou.DataFrame.Payload;
            SpecificResult.ControllerId = packet.nodeListControllerId;
            var nodes = GetNodeListResult.ParseNodesList(packet.nodeListData);
            if (packet.extendedNodeListLength != null && packet.extendedNodeListLength.Length == 2)
            {
                var length = (packet.extendedNodeListLength[0] << 8) + packet.extendedNodeListLength[1];
                if (length > 0)
                    nodes.AddRange(GetNodeListResult.ParseNodesList(packet.extendedNodeList, true));
            }
            SpecificResult.Nodes = nodes.ToArray();
            SpecificResult.Status = packet.status;

            SetStateCompleted(ou);
        }

        public new GetNodeListResult SpecificResult
        {
            get { return (GetNodeListResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new GetNodeListResult();
        }
    }

    public class GetNodeListResult : RequestDataResult
    {
        public byte ControllerId { get; set; }
        public NodeTag[] Nodes { get; set; }
        public byte Status { get; set; }

        public static List<NodeTag> ParseNodesList(IList<byte> nodeListData, bool isExtended = false)
        {
            var nodes = new List<NodeTag>();
            if (nodeListData != null)
            {
                for (int i = 0; i < nodeListData.Count; i++)
                {
                    byte maskByte = nodeListData[i];
                    if (maskByte == 0)
                    {
                        continue;
                    }
                    byte bitMask = 0x01;
                    byte bitOffset = 0x01; // Nodes starting from 1 in mask bytes array.
                    for (int j = 0; j < 8; j++)
                    {
                        if ((bitMask & maskByte) != 0)
                        {
                            byte nodeID = (byte)(((i * 8) + j) + bitOffset);
                            if (isExtended)
                                nodes.Add(new NodeTag((ushort)(255 + nodeID)));
                            else
                                nodes.Add(new NodeTag(nodeID));
                        }
                        bitMask <<= 1;
                    }
                }
            }
            return nodes;
        }
    }
}
