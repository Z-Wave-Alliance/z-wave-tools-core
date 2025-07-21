/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
namespace ZWave.BasicApplication.Operations
{
    public class DelayOperation : ActionBase
    {
        private readonly int _timeoutMs;

        public DelayOperation(int timeoutMs)
            : base(false)
        {
            _timeoutMs = timeoutMs;
        }

        ITimeoutItem timeoutItem;
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 0, timeoutItem));
            ActionUnits.Add(new TimeElapsedUnit(timeoutItem, SetStateCompleted, 0));
        }

        protected override void CreateInstance()
        {
            timeoutItem = new TimeInterval(GetNextCounter(), Id, _timeoutMs);
        }
    }
}
