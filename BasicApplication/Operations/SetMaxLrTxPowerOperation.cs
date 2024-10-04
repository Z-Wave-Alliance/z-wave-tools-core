/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    public class SetMaxLrTxPowerOperation : SerialApiSetupOperation
    {
        public const byte SERIAL_API_SETUP_CMD_MAX_LR_TX_PWR_SET = 3;

        public SetMaxLrTxPowerOperation(NetworkViewPoint network, short value)
            : base(SERIAL_API_SETUP_CMD_MAX_LR_TX_PWR_SET, (byte)(value >> 8), (byte)value)
        {
            _network = network;
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            base.SetStateCompleted(ou);
            var res = base.SpecificResult.ByteArray;
            if (res != null && res.Length > 0)
            {
                if (res[0] == SERIAL_API_SETUP_CMD_MAX_LR_TX_PWR_SET)
                {
                }
            }
        }
    }
}
