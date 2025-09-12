/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
/// SPDX-FileCopyrightText: 2024 Z-Wave Alliance
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZWave.Enums;
using ZWave.Layers;
using ZWave.Layers.Application;
using ZWave.Xml.FrameHeader;
using ZWave.ZnifferApplication.Enums;
using ZWave.ZnifferApplication.Operations;

namespace ZWave.ZnifferApplication.Devices
{
    public class SnifferPti : ApplicationClient, ISnifferDevice
    {
        private ActionToken _listenToken;

        internal SnifferPti(ushort sessionId, ISessionClient sc, IFrameClient fc, ITransportClient tc)
           : base(ApiTypes.Pti, sessionId, sc, fc, tc)
        {
        }

        public byte[] Frequencies { get; set; }
        public byte Frequency { get; set; }

        public ActionToken RestartListening(Func<ActionToken, DataItem, bool> onListenDataItemReceived)
        {
            if (_listenToken != null)
            {
                Cancel(_listenToken);
            }
            _listenToken = ExpectData(true, null, null, onListenDataItemReceived, 0, null);
            return _listenToken;
        }

        public GetVersionResult GetVersion()
        {
            throw new NotImplementedException();
        }

        public ActionToken ExecuteAsync(IActionItem actionItem, Action<IActionItem> completedCallback)
        {
            actionItem.CompletedCallback = completedCallback;
            var action = actionItem as ActionBase;
            if (action != null)
            {
                action.Token.LogEntryPointCategory = "Pti";
                action.Token.LogEntryPointSource = DataSource == null ? "" : DataSource.SourceName;
            }
            return SessionClient.ExecuteAsync(actionItem);
        }

        public ActionResult Execute(IActionItem actionItem)
        {
            var action = actionItem as ActionBase;
            if (action != null)
            {
                action.Token.LogEntryPointCategory = "Pti";
                action.Token.LogEntryPointSource = DataSource == null ? "" : DataSource.SourceName;
            }
            var token = SessionClient.ExecuteAsync(actionItem);
            token.WaitCompletedSignal();
            return token.Result;
        }

        public ActionToken ExpectData(bool isTop, byte[] homeId, byte? nodeId, Func<ActionToken, DataItem, bool> dataItemPredicate, int timeoutMs, Action<IActionItem> completedCallback)
        {
            ExpectPtiDataOperation op = new ExpectPtiDataOperation(homeId, nodeId, dataItemPredicate, timeoutMs) { IsFirstPriority = isTop };
            return ExecuteAsync(op, completedCallback);
        }

        public ActionToken ExpectData(byte[] homeId, byte? nodeId, Func<ActionToken, DataItem, bool> dataItemPredicate, int timeoutMs, Action<IActionItem> completedCallback)
        {
            ExpectPtiDataOperation op = new ExpectPtiDataOperation(homeId, nodeId, dataItemPredicate, timeoutMs);
            return ExecuteAsync(op, completedCallback);
        }

        public ActionToken CollectData(byte[] homeId, byte? nodeId, Func<ActionToken, DataItem, bool> dataItemPredicate, int timeoutMs, Action<IActionItem> completedCallback)
        {
            CollectPtiDataOperation op = new CollectPtiDataOperation(homeId, nodeId, dataItemPredicate, timeoutMs);
            return ExecuteAsync(op, completedCallback);
        }

        public ActionToken ListenData(ListenDataOperation.DataItemCallbackDelegate dataItemCallback, Action<IActionItem> completedCallback)
        {
            ListenDataOperation op = new ListenDataOperation(dataItemCallback);
            return ExecuteAsync(op, completedCallback);
        }

        public void SetLRChannelConfig(byte channelConfigCode)
        {
            throw new NotSupportedException();
        }

        public bool Start()
        {
            throw new NotImplementedException();
        }
    }
}
