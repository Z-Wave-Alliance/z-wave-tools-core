/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0x9C | 2
    /// ZW->HOST: RES | 0x9C | 2 | publicDSKLen(16) | publicDSK[16]
    /// </summary>

    public class GetSecurityS2PublicDSKOperation : RequestApiOperation
    {
        public GetSecurityS2PublicDSKOperation() : base(CommandTypes.CmdZWaveSecuritySetup, false)
        {
        }

        protected override byte[] CreateInputParameters()
        {
            return new byte[] { 0x02 };
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            byte[] res = ((DataReceivedUnit)ou).DataFrame.Payload;
            if (res != null && res.Length == 18 && res[1] == 16)
            {
                SpecificResult.DSK = res.Skip(2).ToArray();
            }
            base.SetStateCompleted(ou);
        }

        public override string AboutMe()
        {
            if (SpecificResult.DSK != null)
            {
                return $"DSK: {Tools.GetHex(SpecificResult.DSK)}";
            }
            return string.Empty;
        }

        public GetSecurityS2PublicDSKResult SpecificResult
        {
            get { return (GetSecurityS2PublicDSKResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new GetSecurityS2PublicDSKResult();
        }
    }

    public class GetSecurityS2PublicDSKResult : ActionResult
    {
        public byte[] DSK { get; set; }
    }
}
