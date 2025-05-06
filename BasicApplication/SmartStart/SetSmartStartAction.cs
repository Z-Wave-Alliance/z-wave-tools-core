/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;
using ZWave.Enums;

namespace ZWave.BasicApplication
{
    public class SetSmartStartAction : ApiOperation
    {
        private readonly bool _isStart;
        private int _delay;
        public SetSmartStartAction(bool isStart, int delayBeforeStart)
            : base(false, null, true)
        {
            _delay = delayBeforeStart;
            _isStart = isStart;
        }

        private ApiMessage _messageStart;
        private ApiMessage _messageStop;
        private ITimeoutItem _timeoutItem;
        protected override void CreateWorkflow()
        {
            if (_isStart)
            {
                if (_delay > 0)
                {
                    ActionUnits.Add(new StartActionUnit(null, 0, _timeoutItem));
                    ActionUnits.Add(new TimeElapsedUnit(_timeoutItem, OnPrepare, 500, _messageStart));
                }
                else
                {
                    ActionUnits.Add(new StartActionUnit(OnPrepare, 500, _messageStart));
                }
            }
            else
            {
                ActionUnits.Add(new StartActionUnit(OnPrepare, 500, _messageStop));
            }
        }

        private void OnPrepare(IActionUnit ou)
        {
            SetStateCompleting(ou);
        }

        protected override void CreateInstance()
        {
            _messageStart = new ApiMessage(CommandTypes.CmdZWaveAddNodeToNetwork, (byte)(Modes.NodeOptionNetworkWide | Modes.NodeSmartStart));
            _messageStart.SetSequenceNumber(SequenceNumber);

            _messageStop = new ApiMessage(CommandTypes.CmdZWaveAddNodeToNetwork, new byte[] { (byte)Modes.NodeStop });
            _messageStop.SetSequenceNumber(0); //NULL funcID = 0

            _timeoutItem = new TimeInterval(GetNextCounter(), Id, _delay);
        }

        public override string AboutMe()
        {
            return _isStart ? "ON" : "OFF";
        }
    }
}
