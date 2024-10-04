/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class GetProtocolStatusOperation : RequestApiOperation
    {
        public GetProtocolStatusOperation()
            : base(CommandTypes.CmdZWaveGetProtocolStatus, false)
        {
        }

        protected override byte[] CreateInputParameters()
        {
            return null;
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            SpecificResult.ProtocolStatus = (ProtocolStatuses)((DataReceivedUnit)ou).DataFrame.Payload[0];
            base.SetStateCompleted(ou);
        }

        public GetProtocolStatusResult SpecificResult
        {
            get { return (GetProtocolStatusResult)Result; }
        }
        
        protected override ActionResult CreateOperationResult()
        {
            return new GetProtocolStatusResult();
        }
    }

    public class GetProtocolStatusResult : ActionResult
    {
        public ProtocolStatuses ProtocolStatus { get; set; }
    }
}
