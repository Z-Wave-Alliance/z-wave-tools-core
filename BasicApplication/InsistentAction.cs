/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿namespace ZWave.BasicApplication
{
    public class InsistentAction : ApiOperation
    {
        private ActionBase _action;
        private int _maxAttemptsCount;
        private readonly int _interval;
        public InsistentAction(ActionBase action, int maxAttemptsCount, int interval)
            : base(false, null, false)
        {
            _action = action;
            _maxAttemptsCount = maxAttemptsCount;
            _interval = interval;
        }
        private ITimeoutItem timeoutItem;
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 3000, _action));
            ActionUnits.Add(new ActionCompletedUnit(_action, OnActionCompleted, timeoutItem));
            ActionUnits.Add(new TimeElapsedUnit(timeoutItem, null, 0, _action));
        }

        protected override void CreateInstance()
        {
            timeoutItem = new TimeInterval(GetNextCounter(), Id, _interval);
        }

        private void OnActionCompleted(ActionCompletedUnit au)
        {
            if (_action.Result || _maxAttemptsCount == 0)
            {
                SetStateCompleted(au);
            }
            else
            {
                _maxAttemptsCount--;
                _action.NewToken();
            }
        }
    }
}
