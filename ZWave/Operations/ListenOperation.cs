/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using Utils;

namespace ZWave.Operations
{
    public class ListenOperation : ActionBase
    {
        private readonly Action<byte[]> _listenCallback;
        private ByteIndex[] _mask;
        public ListenOperation(ByteIndex[] mask, Action<byte[]> listenCallback)
            : base(false)
        {
            _mask = mask;
            _listenCallback = listenCallback;
        }

        private CommandHandler _dataReceived;

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 0));
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
            if (_listenCallback != null && ou.DataFrame != null && ou.DataFrame.Payload != null)
            {
                _listenCallback(ou.DataFrame.Payload);
            }
        }
    }
}
