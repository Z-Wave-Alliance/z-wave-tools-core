/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using Utils.Threading;

namespace ZWave.ZnifferApplication.Operations
{
    public class ExpectDataOperation : CollectDataOperation
    {
        public ExpectDataOperation(byte[] homeId, byte? srcNodeId, Func<ActionToken, DataItem, bool> dataItemPredicate, int timeoutMs) :
            base(homeId, srcNodeId, dataItemPredicate, timeoutMs)
        {
        }

        public new ExpectDataResult SpecificResult
        {
            get { return (ExpectDataResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ExpectDataResult(10000);
        }

        protected override void OnReceived(DataReceivedUnit ou)
        {
            if (Token.IsStateActive && SpecificResult.DataItems.Count < SpecificResult.MaxCapacity)
            {
                var dataItem = ((DataFrame)ou.DataFrame).DataItem;
                var FilterObjects = Token.CustomObject as ConcurrentList<FilterObject>;
                if (FilterObjects!=null && DataItemPredicate != null)
                {
                    if (DataItemPredicate(Token, dataItem))
                    {
                        SpecificResult.DataItems.Add(dataItem);

                        bool isExpectCompleted = true;
                        for (int i = 0; i < FilterObjects.Count; i++)
                        {
                            isExpectCompleted &= FilterObjects[i].FramesCount >= FilterObjects[i].ExpectedFramesCount;
                        }
                        if (isExpectCompleted)
                        {
                            SetStateCompleted(ou);
                        }
                    }
                }
            }
        }

    }

    public class ExpectDataResult : CollectDataResult
    {
        public ExpectDataResult(int maxCapacity)
            : base(maxCapacity)
        { }
    }
}
