/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using Utils;

namespace ZWave.BasicApplication.Operations
{
    public class RequestZWOperation : ActionBase
    {
        public byte[] Data { get; set; }
        public ByteIndex[] Mask { get; set; }
        public int TimeoutMs { get; set; }

        public RequestZWOperation(byte[] data, ByteIndex[] mask, int timeoutMs)
            : base(true)
        {
            Data = data;
            Mask = mask;
            TimeoutMs = timeoutMs;
        }

        CommandMessage message;
        CommandHandler handler;
        protected override void CreateInstance()
        {
            message = new CommandMessage { Data = Data };
            handler = new CommandHandler { Mask = Mask };
        }

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, TimeoutMs, message));
            ActionUnits.Add(new DataReceivedUnit(handler, OnReceived));
        }

        private void OnReceived(DataReceivedUnit ou)
        {
            SpecificResult.Data = ou.DataFrame.Data;
            SetStateCompleted(ou);
        }

        public override string AboutMe()
        {
            return string.Format("TxData={0}, RxData={1}", Data != null ? Data.GetHex() : "", SpecificResult.Data != null ? SpecificResult.Data.GetHex() : "");
        }

        public ExpectZWResult SpecificResult
        {
            get { return (ExpectZWResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ExpectZWResult();
        }
    }
}
