/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;

namespace ZWave
{
    public class CommandMessage : IActionItem
    {
        public bool IsNoAck { get; set; }
        public byte SequenceNumber { get; private set; }
        public bool IsSequenceNumberRequired { get; set; }
        public int SequenceNumberCustomIndex { get; set; }
        public Action<IActionItem> CompletedCallback { get; set; }
        public int DataDelay { get; set; }
        private ActionBase _parentAction;
        public ActionBase ParentAction
        {
            get { return _parentAction; }
            set { _parentAction = value; }
        }

        protected byte[] _data;
        public byte[] Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public void AddData(params byte[] data)
        {
            if (data != null)
            {
                if (_data != null)
                {
                    byte[] originPayload = _data;
                    _data = new byte[originPayload.Length + data.Length];
                    Array.Copy(originPayload, 0, _data, 0, originPayload.Length);
                    Array.Copy(data, 0, _data, originPayload.Length, data.Length);
                }
                else
                {
                    _data = new byte[data.Length];
                    Array.Copy(data, 0, _data, 0, data.Length);
                }
            }
        }
        public void SetSequenceNumber(byte funcId)
        {
            IsSequenceNumberRequired = true;
            SequenceNumber = funcId;
        }

    }
}
