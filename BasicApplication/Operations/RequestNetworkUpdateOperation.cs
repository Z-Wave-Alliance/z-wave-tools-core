/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.BasicApplication.Enums;
using ZWave.Enums;
using Utils;

namespace ZWave.BasicApplication.Operations
{
    public class RequestNetworkUpdateOperation : ApiOperation
    {
        public RequestNetworkUpdateOperation()
            : base(true, CommandTypes.CmdZWaveRequestNetworkUpdate, true)
        {
        }

        private ApiMessage messageStart;
        private ApiHandler handler;
        private ApiHandler handlerRetValFalse;

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 60000, messageStart));
            ActionUnits.Add(new DataReceivedUnit(handlerRetValFalse, SetStateFailed));
            ActionUnits.Add(new DataReceivedUnit(handler, OnReceived));
        }

        protected override void CreateInstance()
        {
            messageStart = new ApiMessage(SerialApiCommands[0]);
            messageStart.SetSequenceNumber(SequenceNumber);

            handler = new ApiHandler(FrameTypes.Request, CommandTypes.CmdZWaveRequestNetworkUpdate);
            handler.AddConditions(ByteIndex.AnyValue, ByteIndex.AnyValue);

            handlerRetValFalse = new ApiHandler(CommandTypes.CmdZWaveRequestNetworkUpdate);
            handlerRetValFalse.AddConditions(new ByteIndex(0x00));
        }

        private void OnReceived(DataReceivedUnit ou)
        {
            byte[] ret = ou.DataFrame.Payload;
            SpecificResult.Status = (SucUpdateStatuses)ret[1];
            SetStateCompleted(ou);
        }

        public override string AboutMe()
        {
            return $"Status={SpecificResult.Status}";
        }

        public RequestNetworkUpdateResult SpecificResult
        {
            get { return (RequestNetworkUpdateResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new RequestNetworkUpdateResult();
        }
    }

    public class RequestNetworkUpdateResult : ActionResult
    {
        public SucUpdateStatuses Status { get; set; }
    }
}
