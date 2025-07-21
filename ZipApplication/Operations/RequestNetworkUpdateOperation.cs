/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.CommandClasses;

namespace ZWave.ZipApplication.Operations
{
    public class RequestNetworkUpdateOperation : RequestDataOperation
    {
        public RequestNetworkUpdateOperation(int timeoutMs)
            : base(null, null,
            COMMAND_CLASS_NETWORK_MANAGEMENT_BASIC_V2.ID,
            COMMAND_CLASS_NETWORK_MANAGEMENT_BASIC_V2.NETWORK_UPDATE_REQUEST_STATUS.ID,
            timeoutMs)
        {
            Data = new COMMAND_CLASS_NETWORK_MANAGEMENT_BASIC_V2.NETWORK_UPDATE_REQUEST()
            {
                seqNo = SequenceNumber
            };
        }

        protected override void OnReceived(DataReceivedUnit ou)
        {
            COMMAND_CLASS_NETWORK_MANAGEMENT_BASIC_V2.NETWORK_UPDATE_REQUEST_STATUS packet = (COMMAND_CLASS_NETWORK_MANAGEMENT_BASIC_V2.NETWORK_UPDATE_REQUEST_STATUS)ou.DataFrame.Payload;
            SpecificResult.Status = packet.status;
            
            SetStateCompleted(ou);
        }

        public override string AboutMe()
        {
            return string.Format("Status={0}", SpecificResult.Status);
        }

        public new RequestNetworkUpdateResult SpecificResult
        {
            get { return (RequestNetworkUpdateResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new RequestNetworkUpdateResult();
        }
    }

    public class RequestNetworkUpdateResult : RequestDataResult
    {
        public byte Status { get; set; }
    }
}
