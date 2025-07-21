/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using Utils;
using ZWave.Enums;
using System.Threading;
using Utils.UI;
using System;
using ZWave.Operations;
using System.Collections.Concurrent;

namespace ZWave.Layers.Application
{
    public class ApplicationClient : EntityBase, IApplicationClient
    {
        public ushort SessionId { get; set; }
        public string Version { get; set; }
        public Libraries Library { get; set; }
        public ChipTypes ChipType { get; set; }
        public byte ChipRevision { get; set; }
        public ApiTypes ApiType { get; set; }
        public ITransportClient TransportClient { get; set; }
        public IFrameClient FrameClient { get; set; }
        public ISessionClient SessionClient { get; set; }
        public IDataSource DataSource
        {
            get { return TransportClient.DataSource; }
            set { TransportClient.DataSource = value; }
        }

        protected ConcurrentDictionary<int, ActionToken> activeTokens = new ConcurrentDictionary<int, ActionToken>();

        public ApplicationClient(ApiTypes apiType, ushort sessionId, ISessionClient sc, IFrameClient fc, ITransportClient tc)
        {
            ApiType = apiType;
            SessionId = sessionId;
            SessionClient = sc;
            FrameClient = fc;
            TransportClient = tc;
            BindLayers();
        }

        private void BindLayers()
        {
            SessionClient.SessionId = SessionId;
            FrameClient.SessionId = SessionId;
            TransportClient.SessionId = SessionId;
            TransportClient.ApiType = ApiType;

            SessionClient.SendFramesCallback = FrameClient.SendFrames;
            FrameClient.ReceiveFrameCallback = SessionClient.HandleActionCase;
            FrameClient.SendDataCallback = TransportClient.WriteData;
            TransportClient.ReceiveDataCallback = FrameClient.HandleData;
        }

        private void UnBindLayers()
        {
            SessionClient.SendFramesCallback = null;
            FrameClient.ReceiveFrameCallback = null;
            FrameClient.SendDataCallback = null;
            TransportClient.ReceiveDataCallback = null;
        }

        protected ActionResult WaitCompletedSignal(ActionToken token)
        {
            ActionResult res = null;
            if (!isDisposing)
            {
                activeTokens.TryAdd(token.ActionId, token);
                res = token.WaitCompletedSignal();
                activeTokens.TryRemove(token.ActionId, out ActionToken t);
            }
            return res;
        }

        #region IApplicationClient Members

        public virtual CommunicationStatuses Connect()
        {
            return TransportClient.Connect();
        }

        public virtual CommunicationStatuses Connect(IDataSource ds)
        {
            return TransportClient.Connect(ds);
        }

        public virtual void Disconnect()
        {
            TransportClient.Disconnect();
        }

        public void Cancel(ActionToken token)
        {
            SessionClient.Cancel(token);
        }

        public ActionToken Listen(ByteIndex[] mask, Action<byte[]> data)
        {
            return SessionClient.ExecuteAsync(new ListenOperation(mask, data));
        }

        public ExpectResult Expect(ByteIndex[] mask, int timeoutMs)
        {
            var token = SessionClient.ExecuteAsync(new ExpectOperation(mask, timeoutMs));
            return (ExpectResult)token.WaitCompletedSignal();
        }

        public ActionToken Expect(ByteIndex[] mask, int timeoutMs, Action<IActionItem> completedCallback)
        {
            return SessionClient.ExecuteAsync(new ExpectOperation(mask, timeoutMs) { CompletedCallback = completedCallback });
        }

        public SendResult Send(byte[] data)
        {
            var token = SessionClient.ExecuteAsync(new SendOperation(data));
            return (SendResult)token.WaitCompletedSignal();
        }

        public ActionToken Send(byte[] data, Action<IActionItem> completedCallback)
        {
            return SessionClient.ExecuteAsync(new SendOperation(data) { CompletedCallback = completedCallback });
        }

        public RequestResult Request(byte[] data, ByteIndex[] mask, int timeoutMs)
        {
            var action = new RequestOperation(data, mask, timeoutMs);
            action.Token.LogEntryPointSource = DataSource == null ? "" : DataSource.SourceName;
            var token = SessionClient.ExecuteAsync(action);
            return (RequestResult)token.WaitCompletedSignal();
        }

        public ActionToken Request(byte[] data, ByteIndex[] mask, int timeoutMs, Action<IActionItem> completedCallback)
        {
            return SessionClient.ExecuteAsync(new RequestOperation(data, mask, timeoutMs) { CompletedCallback = completedCallback });
        }

        #endregion

        #region IDisposable Support
        private bool disposedValue = false;
        protected volatile bool isDisposing;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    isDisposing = true;
                    foreach (var activeToken in activeTokens)
                    {
                        activeToken.Value.SetCompletedSignal();
                    }
                    SessionClient.Dispose();
                    FrameClient.Dispose();
                    TransportClient.Dispose();
                    UnBindLayers();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
