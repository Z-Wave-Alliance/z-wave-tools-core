/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.Enums;
using ZWave.BasicApplication.Enums;
using Utils;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0x0F | dataLength | pData[ ] | txOptions | securityKey | groupID | funcID  
    /// ZW->HOST: RES | 0x0F | RetVal  
    /// If either (funcID == 0) OR (RetVal == FALSE) -> no callback 
    /// If (funcID != 0) AND (RetVal == TRUE) then callback returns with: 
    /// ZW->HOST: REQ | 0x0F | funcID | txStatus 
    /// </summary>
    public class SendDataMultiExOperation : CallbackApiOperation
    {
        public byte[] Data { get; private set; }
        public TransmitOptions TxOptions { get; private set; }
        public SecuritySchemes SecurityScheme { get; private set; }
        public byte GroupId { get; private set; }
        public SendDataMultiExOperation(byte[] data, TransmitOptions txOptions, SecuritySchemes scheme, byte groupId)
            : base(CommandTypes.CmdSendDataMultiEx)
        {
            Data = data;
            TxOptions = txOptions;
            SecurityScheme = scheme;
            GroupId = groupId;
        }

        protected override byte[] CreateInputParameters()
        {
            byte[] ret = new byte[4 + Data.Length];
            ret[0] = (byte)Data.Length;
            for (int i = 0; i < Data.Length; i++)
            {
                ret[i + 1] = Data[i];
            }
            ret[1 + Data.Length] = (byte)TxOptions;
            ret[2 + Data.Length] = (byte)SecurityScheme;
            ret[3 + Data.Length] = GroupId;
            return ret;
        }

        protected override void OnCallbackInternal(DataReceivedUnit ou)
        {
            if (ou.DataFrame.Payload != null && ou.DataFrame.Payload.Length > 1)
            {
                SpecificResult.FuncId = ou.DataFrame.Payload[0];
                SpecificResult.TransmitStatus = (TransmitStatuses)ou.DataFrame.Payload[1];
            }
        }

        public override string AboutMe()
        {
            return $"Data={Data.GetHex()}; {SpecificResult.GetReport()}";
        }

        public SendDataResult SpecificResult
        {
            get { return (SendDataResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new SendDataResult();
        }
    }
}
