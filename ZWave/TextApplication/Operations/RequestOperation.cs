/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Linq;
using System.Text;
using Utils;

namespace ZWave.TextApplication.Operations
{
    public class RequestOperation : ActionBase
    {
        private int TimeoutMs { get; set; }
        private Encoding TextEncoding { get; set; }
        private byte[] TextData { get; set; }
        private string Text { get; set; }
        private Func<string, bool> _expectedCallback;
        public RequestOperation(byte[] textData, int timeoutMs)
            : this(null, null, null, timeoutMs)
        {
            TextData = textData;
        }

        public RequestOperation(string text, Encoding textEncoding, int timeoutMs)
            : this(text, textEncoding, null, timeoutMs)
        {
        }

        public RequestOperation(string text, Encoding textEncoding, Func<string, bool> expectedCallback, int timeoutMs)
            : base(true)
        {
            _expectedCallback = expectedCallback;
            TextEncoding = textEncoding;
            TimeoutMs = timeoutMs;
            Text = text;
            TextData = textEncoding.GetBytes(text);
        }

        private CommandMessage message;
        private CommandHandler handler;
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, TimeoutMs, message));
            ActionUnits.Add(new DataReceivedUnit(handler, OnReceived));
        }

        protected override void CreateInstance()
        {
            message = new TextApiMessage(TextData);
            message.IsNoAck = true;
            handler = new CommandHandler();
            handler.AddConditions(ByteIndex.AnyValue);
        }

        private void OnReceived(DataReceivedUnit ou)
        {
            if (SpecificResult.ReceivedData == null)
            {
                SpecificResult.ReceivedData = ou.DataFrame.Payload;
            }
            else
            {
                SpecificResult.ReceivedData = SpecificResult.ReceivedData.Concat(ou.DataFrame.Payload).ToArray();
            }
            if (TextEncoding != null)
            {
                SpecificResult.ReceivedText = TextEncoding.GetString(SpecificResult.ReceivedData);
                if (_expectedCallback == null || _expectedCallback(SpecificResult.ReceivedText))
                {
                    SetStateCompleted(ou);
                }
            }
            else
            {
                SetStateCompleted(ou);
            }

        }

        public override string AboutMe()
        {
            if (Text != null && SpecificResult.ReceivedText != null)
            {
                string tt = Text.Replace("\r", "\\r");
                string rr = SpecificResult.ReceivedText.Replace("\r", "\\r");
                return string.Format("Tx:\"{0}\", Rx:\"{1}\"", tt, rr);
            }
            return Tools.GetHex(TextData);
        }

        public RequestResult SpecificResult
        {
            get { return (RequestResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new RequestResult();
        }
    }

    public class RequestWithDelayOperation : ActionBase
    {
        private int TimeoutMs { get; set; }
        private Encoding TextEncoding { get; set; }
        private byte[] TextData { get; set; }
        private string Text { get; set; }
        public RequestWithDelayOperation(byte[] textData, int timeoutMs)
            : base(true)
        {
            TimeoutMs = timeoutMs;
            TextData = textData;
        }

        public RequestWithDelayOperation(string text, Encoding textEncoding, int timeoutMs)
            : base(true)
        {
            TextEncoding = textEncoding;
            TimeoutMs = timeoutMs;
            Text = text;
            TextData = textEncoding.GetBytes(text);
        }

        private CommandMessage message;
        private CommandHandler handler;
        private ITimeoutItem timeoutItem;
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 0, message, timeoutItem));
            ActionUnits.Add(new DataReceivedUnit(handler, OnReceived));
            ActionUnits.Add(new TimeElapsedUnit(timeoutItem, SetStateCompleted, 0));
        }

        protected override void CreateInstance()
        {
            message = new TextApiMessage(TextData);
            message.IsNoAck = true;
            handler = new CommandHandler();
            handler.AddConditions(ByteIndex.AnyValue);
            timeoutItem = new TimeInterval(GetNextCounter(), Id, TimeoutMs);
        }

        private void OnReceived(DataReceivedUnit ou)
        {
            SpecificResult.ReceivedData = ou.DataFrame.Payload;
            if (TextEncoding != null)
                SpecificResult.ReceivedText += TextEncoding.GetString(SpecificResult.ReceivedData);
        }

        public override string AboutMe()
        {
            return $"{Environment.NewLine}{Text}{Environment.NewLine}{SpecificResult.ReceivedText}";
        }

        public RequestResult SpecificResult
        {
            get { return (RequestResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new RequestResult();
        }
    }

    public class RequestResult : ActionResult
    {
        public byte[] ReceivedData { get; set; }
        public string ReceivedText { get; set; }
        public float ReceivedValue { get; set; }
    }
}
