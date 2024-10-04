/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZWave.ZnifferApplication.Enums;
using Utils;
using Utils.Threading;

namespace ZWave.ZnifferApplication.Operations
{
    public class ListenDataOperation : ActionBase
    {
        public delegate void DataItemCallbackDelegate(DataItem dataIem);
        private DataItemCallbackDelegate DataItemCallback { get; set; }
        public ListenDataOperation(DataItemCallbackDelegate dataItemCallback) :
            base(false)
        {
            DataItemCallback = dataItemCallback;
        }

        private ZnifferApiHandler Handler { get; set; }

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 0));
            ActionUnits.Add(new DataReceivedUnit(Handler, OnReceived));
        }

        protected override void CreateInstance()
        {
            Handler = new ZnifferApiHandler(CommandTypes.DataHandler);
        }

        protected void OnReceived(DataReceivedUnit ou)
        {
            if (Token.IsStateActive)
            {
                DataItem dataItem = ((DataFrame)ou.DataFrame).DataItem;
                DataItemCallback?.Invoke(dataItem);
            }
        }
    }
}
