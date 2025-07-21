/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0xDB 
    /// ZW->HOST: RES | 0xDB | Channel
    /// </summary>
    public class GetLRChannelOperation : RequestApiOperation
    {
        public GetLRChannelOperation() :
            base(CommandTypes.CmdGetLRChannel, false)
        {
            /// FUNC_ID_GET_LR_CHANNEL
        }

        protected override byte[] CreateInputParameters()
        {
            return null;
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            var payload = ((DataReceivedUnit)ou).DataFrame.Payload;
            if (payload != null && payload.Length > 0)
            {
                SpecificResult.Channel = (LongRangeChannels)payload[0];
                //Second byte: bit 4 1 if Automatic channel selection supported or 0 if not supported
                //Second byte: bit 5 1 if Automatic channel selection enabled, 0 if not enabled
                if (payload.Length > 1 && ((payload[1] & 0x30) == 0x30))
                {
                    SpecificResult.Channel = LongRangeChannels.ChannelAuto;
                }
            }
            base.SetStateCompleted(ou);
        }

        public GetLRChannelResult SpecificResult
        {
            get { return (GetLRChannelResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new GetLRChannelResult();
        }
    }

    public class GetLRChannelResult : ActionResult
    {
        public LongRangeChannels Channel { get; set; }
    }
}
