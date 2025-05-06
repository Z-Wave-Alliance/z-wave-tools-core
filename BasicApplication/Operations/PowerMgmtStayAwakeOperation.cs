/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// To send a FUNC_ID_PM_STAY_AWAKE cmd send 0xD7 <powerLockType> <timeout value>
    /// powerLockType 0 for radio powerLock and 1 for IOPowerLock
    /// timeout is 32 bit value in ms first byte is MSB and last is LSB
    /// </summary>
    public class PowerMgmtStayAwakeOperation : ApiOperation
    {
        private readonly byte _powerLockType;
        private readonly int _awakeTimeoutMs;
        private readonly int _wakeupTimeoutMs;
        private readonly int timeoutMs = 2000;
        public PowerMgmtStayAwakeOperation(byte powerLockType, int awakeTimeoutMs, int wakeupTimeoutMs)
            : base(true, CommandTypes.CmdPowerMgmtStayAwake, false)
        {
            _powerLockType = powerLockType;
            _awakeTimeoutMs = awakeTimeoutMs;
            _wakeupTimeoutMs = wakeupTimeoutMs;
        }

        ApiMessage message;

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(SetStateCompleting, timeoutMs, message));
        }

        protected override void CreateInstance()
        {
            message = new ApiMessage(CommandTypes.CmdPowerMgmtStayAwake,
                _powerLockType,
                (byte)(_awakeTimeoutMs >> 24),
                (byte)(_awakeTimeoutMs >> 16),
                (byte)(_awakeTimeoutMs >> 8),
                (byte)_awakeTimeoutMs,
                (byte)(_wakeupTimeoutMs >> 24),
                (byte)(_wakeupTimeoutMs >> 16),
                (byte)(_wakeupTimeoutMs >> 8),
                (byte)_wakeupTimeoutMs);
        }
    }
}
