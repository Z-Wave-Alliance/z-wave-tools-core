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
    class ClearNetworkStatsOperation: ApiOperation
    {
        public ClearNetworkStatsOperation():base (true, CommandTypes.CmdClearNetworkStats, false)
        { }

        private ApiMessage _message;
        private ApiHandler _handler;

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 2000, _message));
            ActionUnits.Add(new DataReceivedUnit(_handler, OnReceived));
        }
        protected override void CreateInstance()
        {
            _message = new ApiMessage(SerialApiCommands[0]);
            _handler = new ApiHandler(SerialApiCommands[0]);
        }
        private void OnReceived(DataReceivedUnit ou)
        {
            var payload = ou.DataFrame.Payload;
            SpecificResult.RetValue = payload[0];
            SetStateCompleted(ou);
        }

        public ClearNetworkStatsResult SpecificResult
        {
            get { return (ClearNetworkStatsResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ClearNetworkStatsResult();
        }

    }

    public class ClearNetworkStatsResult: ActionResult
    {
        public byte RetValue { get; set; }
    }
}
