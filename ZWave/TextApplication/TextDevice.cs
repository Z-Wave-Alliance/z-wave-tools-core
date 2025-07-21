/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Text;
using ZWave.Layers;
using ZWave.TextApplication.Operations;
using ZWave.Layers.Application;
using ZWave.Enums;

namespace ZWave.TextApplication
{
    public class TextDevice : ApplicationClient
    {
        public string[] ResetOutBufferRequests { get; set; }
        public string VersionRequest { get; set; }
        public int RequestTimeoutMs { get; set; }
        public string RequestEndOfLine { get; set; }
        public string ResponseEndOfLine { get; set; }
        public bool IsAllowEmptyResponse { get; set; }
        public Encoding TextEncoding { get; set; }

        public TextDevice(ApiTypes apiType, ushort sessionId, ISessionClient sc, IFrameClient fc, ITransportClient tc)
           : base(apiType, sessionId, sc, fc, tc)
        {
            TextEncoding = Encoding.ASCII;
            RequestTimeoutMs = 1100;
            RequestEndOfLine = Environment.NewLine;
            ResponseEndOfLine = Environment.NewLine;
        }

        public void Stop(Type taskType)
        {
            SessionClient.Cancel(taskType);
            //SessionLayer.WaitForCompletedSignal(SessionId);
        }

        public ActionToken ExecuteAsync(ActionBase action, Action<IActionItem> completedCallback)
        {
            action.CompletedCallback = completedCallback;
            action.Token.LogEntryPointCategory = "Text";
            action.Token.LogEntryPointSource = DataSource == null ? "" : DataSource.SourceName;
            return SessionClient.ExecuteAsync(action);
        }

        public ActionResult Execute(ActionBase action)
        {
            action.Token.LogEntryPointCategory = "Text";
            action.Token.LogEntryPointSource = DataSource == null ? "" : DataSource.SourceName;
            var token = SessionClient.ExecuteAsync(action);
            token.WaitCompletedSignal();
            return token.Result;
        }

        private bool ExpectNewLine(string str, string expectStr)
        {
            var ret = false;
            if (IsAllowEmptyResponse)
            {
                ret = str.Contains(ResponseEndOfLine);
            }
            else
            {
                ret = !string.IsNullOrWhiteSpace(str) && (str.EndsWith(ResponseEndOfLine));
            }
            if (!string.IsNullOrEmpty(expectStr))
            {
                ret &= str.Contains(expectStr);
            }
            return ret;
        }

        public ActionResult Send(string text)
        {
            return Execute(new SendOperation(text + RequestEndOfLine, TextEncoding));
        }

        public void Initialize()
        {
            if (ResetOutBufferRequests != null && ResetOutBufferRequests.Length > 0)
            {
                foreach (var item in ResetOutBufferRequests)
                {
                    if (!string.IsNullOrWhiteSpace(item))
                    {
                        RequestLine(item);
                    }
                }
            }
        }

        public string GetVersion()
        {
            string ret = null;
            if (!string.IsNullOrWhiteSpace(VersionRequest))
            {
                var res = RequestLine(VersionRequest);
                ret = res.ReceivedText;
                Version = ret;
            }
            return ret;
        }

        public RequestResult Request(string text)
        {
            return Request(text, RequestTimeoutMs);
        }

        public RequestResult Request(string text, int timeoutMs)
        {
            var res = (RequestResult)Execute(new RequestOperation(text + RequestEndOfLine, TextEncoding, timeoutMs));
            return res;
        }

        public RequestResult Request(string text, Func<string, bool> expectedCallback, int timeoutMs)
        {
            var res = (RequestResult)Execute(new RequestOperation(text + RequestEndOfLine, TextEncoding, expectedCallback, timeoutMs));
            return res;
        }

        public RequestResult RequestLine(string text)
        {
            return RequestLine(text, null, RequestTimeoutMs);
        }

        public RequestResult RequestLine(string text, int timeoutMs)
        {
            return RequestLine(text, null, timeoutMs);
        }

        public RequestResult RequestLine(string text, string expectText, int timeoutMs)
        {
            var res = (RequestResult)Execute(new RequestOperation(text + RequestEndOfLine, TextEncoding, (x) => ExpectNewLine(x, expectText), timeoutMs));
            if (res && !string.IsNullOrEmpty(res.ReceivedText))
            {
                res.ReceivedText = res.ReceivedText.Trim();
            }
            return res;
        }

        public RequestResult RequestWithDelay(string text, int timeoutMs)
        {
            var res = (RequestResult)Execute(new RequestWithDelayOperation(text + RequestEndOfLine, TextEncoding, timeoutMs));
            return res;
        }

        public ReceiveResult ReceiveData(int timeoutMs)
        {
            var res = (ReceiveResult)Execute(new ReceiveOperation(TextEncoding, timeoutMs));
            return res;
        }

        public ActionToken ReceiveData(int timeoutMs, Action<IActionItem> callBack)
        {
            var res = ExecuteAsync(new ReceiveOperation(TextEncoding, timeoutMs), callBack);
            return res;
        }

        public ActionToken Listen(Action<string> listenCallback, Action<IActionItem> callBack)
        {
            var res = ExecuteAsync(new ListenOperation(TextEncoding, listenCallback), callBack);
            return res;
        }
    }
}
