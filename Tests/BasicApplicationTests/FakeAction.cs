/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using ZWave;

namespace BasicApplicationTests
{
    class FakeAction : ActionBase
    {
        readonly int _timeoutMs;
        readonly IActionItem[] _actionItems;
        readonly Action<StartActionUnit> _onStart;
        public FakeAction()
           : base(true)
        {
        }

        public FakeAction(bool isExclusive, Action<StartActionUnit> onStart, int timeoutMs, params IActionItem[] actionItems)
            : base(isExclusive)
        {
            _timeoutMs = timeoutMs;
            _actionItems = actionItems;
            _onStart = onStart;
        }

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(_onStart, _timeoutMs, _actionItems));
        }

        protected override void CreateInstance()
        {
        }
    }

    class AnotherFakeAction : ActionBase
    {
        readonly int _timeoutMs;
        readonly IActionItem[] _actionItems;
        readonly Action<StartActionUnit> _onStart;
        public AnotherFakeAction()
           : base(true)
        {
        }

        public AnotherFakeAction(bool isExclusive, Action<StartActionUnit> onStart, int timeoutMs, params IActionItem[] actionItems)
            : base(isExclusive)
        {
            _timeoutMs = timeoutMs;
            _actionItems = actionItems;
            _onStart = onStart;
        }

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(_onStart, _timeoutMs, _actionItems));
        }

        protected override void CreateInstance()
        {
        }
    }
}
