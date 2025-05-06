/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0xDE 
    /// ZW->HOST: RES | 0xDE | Mode
    /// </summary>
    /// 
    public class GetDcdcModeOperation : RequestApiOperation
    {
        public GetDcdcModeOperation() :
            base(CommandTypes.CmdGetDcdcMode, false)
        {
            /// FUNC_ID_SET_DCDC_CONFIG
        }

        protected override byte[] CreateInputParameters()
        {
            return null;
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            var payload = ((DataReceivedUnit)ou).DataFrame.Payload;
            if (payload != null && payload.Length > 0)
            {
                SpecificResult.DcdcMode = (DcdcModes)payload[0];
            }
            base.SetStateCompleted(ou);
        }

        public DcdcModeGetResult SpecificResult
        {
            get { return (DcdcModeGetResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new DcdcModeGetResult();
        }
    }

    public class DcdcModeGetResult : ActionResult
    {
        public DcdcModes DcdcMode { get; set; }
    }
}
