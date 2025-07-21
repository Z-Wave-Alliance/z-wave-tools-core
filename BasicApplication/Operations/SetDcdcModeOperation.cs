/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0xDF | Mode
    /// ZW->HOST: RES | 0xDF | RetVal
    /// RetVal: 1 if succeeded, 0 - otherwise
    /// </summary>
    public class SetDcdcModeOperation : ControlNApiOperation
    {
        private DcdcModes _dcdcMode;
        public SetDcdcModeOperation(DcdcModes dcdcMode) : base(CommandTypes.CmdSetDcdcMode)
        {
            /// FUNC_ID_SET_DCDC_CONFIG
            _dcdcMode = dcdcMode;
        }

        protected override byte[] CreateInputParameters()
        {
            return new byte[] { (byte)_dcdcMode };
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            var payload = ((DataReceivedUnit)ou).DataFrame.Payload;
            if (payload != null && payload.Length >= 1)
            {
                var retVal = ((DataReceivedUnit)ou).DataFrame.Payload[0];
                if (retVal == 1)
                    base.SetStateCompleted(ou);
                else
                    base.SetStateFailed(ou);
            }
            else
            {
                base.SetStateFailed(ou);
            }
        }
    }
}
