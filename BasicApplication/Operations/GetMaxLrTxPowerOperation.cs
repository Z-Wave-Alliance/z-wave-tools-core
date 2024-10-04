/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZWave.BasicApplication.Enums;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    public class GetMaxLrTxPowerOperation : SerialApiSetupOperation
    {
        public const byte SERIAL_API_SETUP_CMD_MAX_LR_TX_PWR_GET = 5;

        public GetMaxLrTxPowerOperation(NetworkViewPoint network)
            : base(SERIAL_API_SETUP_CMD_MAX_LR_TX_PWR_GET)
        {
            _network = network;
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            base.SetStateCompleted(ou);
            var res = base.SpecificResult.ByteArray;
            SpecificResult.Value = (short)MaxLrTxPowerModes.Undefined;
            if (res != null && res.Length > 2)
            {
                if (res[0] == SERIAL_API_SETUP_CMD_MAX_LR_TX_PWR_GET)
                {
                    SpecificResult.Value = (short)((res[1] << 8) + res[2]);
                }
            }
        }
        public GetMaxLrTxPowerResult SpecificResult
        {
            get { return (GetMaxLrTxPowerResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new GetMaxLrTxPowerResult();
        }
    }

    public class GetMaxLrTxPowerResult : ReturnValueResult
    {
        public short Value { get; set; }
    }
}
