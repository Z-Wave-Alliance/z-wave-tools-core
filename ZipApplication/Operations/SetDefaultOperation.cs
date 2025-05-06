/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.CommandClasses;

namespace ZWave.ZipApplication.Operations
{
    public class SetDefaultOperation : RequestDataOperation
    {
        public SetDefaultOperation(int timeoutMs)
            : base(null, null,
            COMMAND_CLASS_NETWORK_MANAGEMENT_BASIC_V2.ID,
            COMMAND_CLASS_NETWORK_MANAGEMENT_BASIC_V2.DEFAULT_SET_COMPLETE.ID,
            timeoutMs)
        {
            Data = new COMMAND_CLASS_NETWORK_MANAGEMENT_BASIC_V2.DEFAULT_SET() { seqNo = SequenceNumber };
        }

        protected override void OnReceived(DataReceivedUnit ou)
        {
            COMMAND_CLASS_NETWORK_MANAGEMENT_BASIC_V2.DEFAULT_SET_COMPLETE packet = (COMMAND_CLASS_NETWORK_MANAGEMENT_BASIC_V2.DEFAULT_SET_COMPLETE)ou.DataFrame.Payload;
            SpecificResult.Status = packet.status;

            SetStateCompleted(ou);
        }

        public override string AboutMe()
        {
            return string.Format("Status={0}", SpecificResult.Status);
        }

        public new SetDefaultResult SpecificResult
        {
            get { return (SetDefaultResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new SetDefaultResult();
        }
    }

    public class SetDefaultResult : RequestDataResult
    {
        public byte Status { get; set; }
    }
}
