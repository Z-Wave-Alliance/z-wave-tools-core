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
    public class ClearTxTimersOperation: ApiOperation
    {
        public ClearTxTimersOperation() :base(true, CommandTypes.CmdClearTxTimer, false)
        { }
        private ApiMessage _message;
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(SetStateCompleting, 2000, _message));
        }
        protected override void CreateInstance()
        {
            _message = new ApiMessage(SerialApiCommands[0]);
        }

    }
}
