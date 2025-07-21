/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using Utils;

namespace ZWave.Operations
{
    public class ExpectOperation : ActionBase
    {
        private ByteIndex[] _mask;
        private readonly int _timeoutMs;
        public ExpectOperation(ByteIndex[] mask, int timeoutMs)
            : base(false)
        {
            _timeoutMs = timeoutMs;
            _mask = mask;
        }

        private CommandHandler _dataReceived;

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, _timeoutMs));
            ActionUnits.Add(new DataReceivedUnit(_dataReceived, OnHandled));
        }

        protected override void CreateInstance()
        {
            _dataReceived = new CommandHandler
            {
                Mask = _mask
            };
        }

        private void OnHandled(DataReceivedUnit ou)
        {
            SpecificResult.Data = ou.DataFrame.Data;
            SpecificResult.Payload = ou.DataFrame.Payload;
            SetStateCompleted(ou);
        }

        public override string AboutMe()
        {
            return string.Format($"Expect={_mask?.GetHex()}, Data={SpecificResult.Data?.GetHex()}");
        }

        public ExpectResult SpecificResult
        {
            get { return (ExpectResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ExpectResult();
        }
    }

    public class ExpectResult : ActionResult
    {
        public byte[] Data { get; set; }
        public byte[] Payload { get; set; }
    }
}
