/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0x0B | 0x08
    /// ZW->HOST: RES | 0x0B | 0x08 | NormalTxPower | Measured0dBmPower
    /// </summary>
    /// 
    public class GetDefaultTxPowerLevelOperation : SerialApiSetupOperation
    {
        private const byte SERIAL_API_SETUP_CMD_TX_POWERLEVEL_GET = 1 << 3;

        public GetDefaultTxPowerLevelOperation()
            : base(SERIAL_API_SETUP_CMD_TX_POWERLEVEL_GET)
        {
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            var normalTxPower = ((DataReceivedUnit)ou).DataFrame.Payload[1];
            var measured0dBmPower = ((DataReceivedUnit)ou).DataFrame.Payload[2];
            SpecificResult.NormalTxPower = (sbyte)normalTxPower;
            SpecificResult.Measured0dBmPower = (sbyte)measured0dBmPower;
            base.SetStateCompleted(ou);
        }

        public DefaultTxPowerLevelGetResult SpecificResult
        {
            get { return (DefaultTxPowerLevelGetResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new DefaultTxPowerLevelGetResult();
        }
    }

    public class DefaultTxPowerLevelGetResult : ReturnValueResult
    {
        public short NormalTxPower { get; set; }
        public short Measured0dBmPower { get; set; }
    }
}
