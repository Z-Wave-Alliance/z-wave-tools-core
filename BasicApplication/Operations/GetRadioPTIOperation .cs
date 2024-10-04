/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// FUNC_ID_GET_RADIO_PTI(0xE8)
    /// HOST->ZW: REQ | 0xE8 
    /// ZW->HOST: RES | 0xE8 | retVal
    /// </summary>
    public class GetRadioPTIOperation : RequestApiOperation
    {
        public GetRadioPTIOperation() : base(CommandTypes.CmdGetRadioPTI, false) { }

        protected override byte[] CreateInputParameters()
        {
            return null;
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            var payload = ((DataReceivedUnit)ou).DataFrame.Payload;
            if (payload != null && payload.Length > 0)
            {
                SpecificResult.IsEnabled = payload[0] == 1;
            }
            base.SetStateCompleted(ou);
        }

        public GetRadioPTIResult SpecificResult
        {
            get { return (GetRadioPTIResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new GetRadioPTIResult();
        }
    }

    public class GetRadioPTIResult : ActionResult
    {
        public bool IsEnabled { get; set; }
    }
}