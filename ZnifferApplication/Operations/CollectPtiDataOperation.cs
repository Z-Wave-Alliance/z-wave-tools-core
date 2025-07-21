/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using ZWave.ZnifferApplication.Enums;
using Utils;
using Utils.Threading;

namespace ZWave.ZnifferApplication.Operations
{
    public class CollectPtiDataOperation : ActionBase
    {
        private byte[] HomeId { get; set; }
        private byte? SrcNodeId { get; set; }
        protected Func<ActionToken, DataItem, bool> DataItemPredicate { get; set; }
        private int TimeoutMs { get; set; }
        public CollectPtiDataOperation(byte[] homeId, byte? srcNodeId, Func<ActionToken, DataItem, bool> dataItemPredicate, int timeoutMs) :
            base(false)
        {
            HomeId = homeId;
            SrcNodeId = srcNodeId;
            DataItemPredicate = dataItemPredicate;
            TimeoutMs = timeoutMs;
        }

        private SnifferPtiApiHandler Handler { get; set; }

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, TimeoutMs));
            ActionUnits.Add(new DataReceivedUnit(Handler, OnReceived));
        }

        protected override void CreateInstance()
        {
            Handler = new SnifferPtiApiHandler();
            if (HomeId != null && HomeId.Length >= 4)
            {
                if (SrcNodeId != null)
                {
                    Handler.AddConditions(
                        new ByteIndex(HomeId[0]),
                        new ByteIndex(HomeId[1]),
                        new ByteIndex(HomeId[2]),
                        new ByteIndex(HomeId[3]),
                        new ByteIndex((byte)SrcNodeId));
                }
                else
                {
                    Handler.AddConditions(
                        new ByteIndex(HomeId[0]),
                        new ByteIndex(HomeId[1]),
                        new ByteIndex(HomeId[2]),
                        new ByteIndex(HomeId[3]));
                }
            }
            else
            {
                if (SrcNodeId != null)
                {
                    Handler.AddConditions(
                        ByteIndex.AnyValue,
                        ByteIndex.AnyValue,
                        ByteIndex.AnyValue,
                        ByteIndex.AnyValue,
                        new ByteIndex((byte)SrcNodeId));
                }
            }
        }

        protected virtual void OnReceived(DataReceivedUnit ou)
        {
            if (Token.IsStateActive && SpecificResult.DataItems.Count < SpecificResult.MaxCapacity)
            {
                DataItem dataItem = ((DataFrame)ou.DataFrame).DataItem;
                if (DataItemPredicate != null)
                {
                    if (DataItemPredicate(Token, dataItem))
                    {
                        SpecificResult.DataItems.Add(dataItem);
                    }
                }
            }
        }

        public CollectDataResult SpecificResult
        {
            get { return (CollectDataResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new CollectDataResult(10000);
        }
    }
}
