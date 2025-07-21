/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Text;
using Utils;

namespace ZWave.TextApplication.Operations
{
    public class ReceiveOperation : ActionBase
    {
        private int TimeoutMs { get; set; }
        private Encoding TextEncoding { get; set; }
        public ReceiveOperation(Encoding textEncoding, int timeoutMs)
            : base(true)
        {
            TextEncoding = textEncoding;
            TimeoutMs = timeoutMs;
        }

        private CommandHandler handler;
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, TimeoutMs));
            ActionUnits.Add(new DataReceivedUnit(handler, OnReceived));
        }

        protected override void CreateInstance()
        {
            handler = new CommandHandler();
            handler.AddConditions(ByteIndex.AnyValue);
        }

        private void OnReceived(DataReceivedUnit ou)
        {
            SpecificResult.ReceivedData = ou.DataFrame.Payload;
            if (TextEncoding != null)
                SpecificResult.ReceivedText += TextEncoding.GetString(SpecificResult.ReceivedData);
            if(SpecificResult.ReceivedText.Contains("\n") || SpecificResult.ReceivedText.Contains("\r") ||
                SpecificResult.ReceivedText.Contains("\\n") || SpecificResult.ReceivedText.Contains("\\r"))
                SetStateCompleted(ou);
        }

        public override string AboutMe()
        {
            string ret = string.Empty;
            if (SpecificResult.ReceivedText != null)
            {
                string rr = SpecificResult.ReceivedText.Replace("\r", "\\r");
                ret = string.Format("Rx:\"{0}\"", rr);
            }
            return ret;
        }

        public ReceiveResult SpecificResult
        {
            get { return (ReceiveResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ReceiveResult();
        }
    }

    public class ReceiveResult : ActionResult
    {
        public byte[] ReceivedData { get; set; }
        public string ReceivedText { get; set; }
    }
}
