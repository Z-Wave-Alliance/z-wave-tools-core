/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.CommandClasses;
using ZWave.Devices;
using ZWave.Enums;

namespace ZWave.ZipApplication.Operations
{
    public class RequestNodeNeighborUpdateOperation : RequestDataOperation
    {
        internal NodeTag Node { get; set; }
        public RequestNodeNeighborUpdateOperation(NodeTag node, int timeoutMs)
            : base(null, null,
            COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V3.ID,
            COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V3.NODE_NEIGHBOR_UPDATE_STATUS.ID,
            timeoutMs)
        {
            Node = node;
            Data = new COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V3.NODE_NEIGHBOR_UPDATE_REQUEST()
            {
                nodeId = (byte)Node.Id,
                seqNo = SequenceNumber
            };
        }

        protected override void OnReceived(DataReceivedUnit ou)
        {
            COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V3.NODE_NEIGHBOR_UPDATE_STATUS packet = (COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION_V3.NODE_NEIGHBOR_UPDATE_STATUS)ou.DataFrame.Payload;
            SpecificResult.Status = (RequestNeighborUpdateStatuses)packet.status.Value;

            if (SpecificResult.Status == RequestNeighborUpdateStatuses.Failed)
                SetStateFailed(ou);
            else
                SetStateCompleted(ou);
        }

        public override string AboutMe()
        {
            return string.Format("Status={0}", SpecificResult.Status);
        }

        public new RequestNodeNeighborUpdateResult SpecificResult
        {
            get { return (RequestNodeNeighborUpdateResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new RequestNodeNeighborUpdateResult();
        }
    }

    public class RequestNodeNeighborUpdateResult : RequestDataResult
    {
        public RequestNeighborUpdateStatuses Status { get; set; }
    }
}
