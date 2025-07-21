/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class SerialApiSetupOperation : ApiOperation
    {
        private readonly byte[] _args;

        public static int RET_TIMEOUT = 200;

        public SerialApiSetupOperation(params byte[] args)
            : base(false, CommandTypes.CmdSerialApiSetup, false)
        {
            _args = args;
        }

        ApiMessage _message;
        ApiHandler _handler;
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, RET_TIMEOUT, _message));
            ActionUnits.Add(new DataReceivedUnit(_handler, SetStateCompleted));
        }

        protected override void CreateInstance()
        {
            _message = new ApiMessage(SerialApiCommands[0], _args);
            _handler = new ApiHandler(SerialApiCommands[0]);
            if (_args != null && _args.Length > 0)
            {
                _handler.AddConditions(new Utils.ByteIndex(_args[0]));
            }
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            if (ou.ActionHandler != null)
            {
                SpecificResult.ByteArray = ((ApiHandler)ou.ActionHandler).DataFrame.Payload;
            }
            base.SetStateCompleted(ou);
        }

        public ReturnValueResult SpecificResult
        {
            get { return (ReturnValueResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ReturnValueResult();
        }
    }
}
