/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0x5C | destList[5] | funcID
    /// ZW->HOST: RES | 0x5C | retVal
    /// ZW->HOST: REQ | 0x5C | funcID | bStatus
    /// </summary>
    public class RequestNewRouteDestinationsOperation: CallbackApiOperation
    {
        public byte[] DestList { get; set; }
        public RequestNewRouteDestinationsOperation(byte[] destList)
            : base(CommandTypes.CmdZWaveRequestNewRouteDestinations)
        {
            DestList = destList;            
        }

        protected override byte[] CreateInputParameters()
        {
            return DestList;
        }

        protected override void OnCallbackInternal(DataReceivedUnit ou)
        {
            if (ou.DataFrame.Payload != null && ou.DataFrame.Payload.Length > 1)
            {
                SpecificResult.RetStatus = ou.DataFrame.Payload[1];
            }
        }

        public RequestNewRouteDestinationsResult SpecificResult
        {
            get { return (RequestNewRouteDestinationsResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new RequestNewRouteDestinationsResult();
        }
    }

    public class RequestNewRouteDestinationsResult : ActionResult
    {
        public byte RetStatus { get; set; }
    }
}
