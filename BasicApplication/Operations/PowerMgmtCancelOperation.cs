/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// To send FUNC_ID_PM_CANCEL cmd send 0xD8 <powerLockType>
    /// powerLockType 0 for radio powerLock and 1 for IOPowerLock
    /// </summary>
    public class PowerMgmtCancelOperation : ApiOperation
    {
        private readonly byte _powerLockType;
        private readonly int timeoutMs = 2000;
        public PowerMgmtCancelOperation(byte powerLockType) : base(true, CommandTypes.CmdPowerMgmtCancel, false)
        {
            _powerLockType = powerLockType;
        }

        ApiMessage message;

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(SetStateCompleting, timeoutMs, message));
        }

        protected override void CreateInstance()
        {
            message = new ApiMessage(CommandTypes.CmdPowerMgmtCancel, _powerLockType);
        }
    }
}
