/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using ZWave.ZnifferApplication.Enums;

namespace ZWave.ZnifferApplication.Operations
{
    /// <summary>
    /// This class represents GetLRChConfigStrOperation which is used to get the LR channel config name from the Serial Zniffer namely zwave_ncp_zniffer.
    /// This command might be not supported by the older versions of the Serial Zniffer.
    /// </summary>
    public class GetLRChConfigStrOperation : ActionBase
    {
        private const int TIMEOUT_MS = 300;

        private byte currentLrChConfig {  get; set; }

        private byte lrChCode { get; set; }

        public GetLRChConfigStrOperation(byte lrChannelCode) : base(true)
        {
            lrChCode = lrChannelCode;
        }

        public ZnifferApiMessage GetLRChConfigStrMessage { get; set; }

        public ZnifferApiHandler GetLRChConfigStrHandler { get; set; }

        /// <summary>
        /// CreateInstance method is used to create the GetLRChConfigStrMessage and GetLRChConfigStrHandler.
        /// The get command must be "0x14 0x01 0x0x", where the second byte is the length of the payload. The payload is the supported lr channel code
        /// The answer must be started with the same command type, in this case with 0x14.
        /// </summary>
        protected override void CreateInstance()
        {
            GetLRChConfigStrMessage = new ZnifferApiMessage(CommandTypes.GetLRChConfigStr, new byte[] { 0x01, lrChCode });
            GetLRChConfigStrHandler = new ZnifferApiHandler(CommandTypes.GetLRChConfigStr);
        }

        /// <summary>
        ///  CreateWorkflow method is used to create the workflow for the GetLRChConfigStrOperation. This will send get LR channel name/string command to the Serial Zniffer.
        ///  For the answer, which will begins with the GetLRChConfigStrHandler, the SetStateCompleted method from this class will be called.
        /// </summary>
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, TIMEOUT_MS, GetLRChConfigStrMessage));
            ActionUnits.Add(new DataReceivedUnit(GetLRChConfigStrHandler, SetStateCompleted));
        }

        /// <summary>
        /// SetStateCompleted method is used to set the state of the GetLRChConfigStrOperation to completed.
        /// This method will handle the incomming frame where the first byte is the current LR channel.
        /// The second byte stores the CH config code and the rest of the payload is the LR channel config name.
        /// </summary>
        /// <param name="ou"></param>
        protected override void SetStateCompleted(IActionUnit ou)
        {
            byte[] payload = ((DataReceivedUnit)ou).DataFrame.Payload;
            if (payload != null && payload.Length > 2)
            {
                byte currentLrChConfig = payload[0];
                SpecificResult.lrChConfig = payload[1];

                System.Text.UTF7Encoding utf = new System.Text.UTF7Encoding();
                byte[] res = new byte[payload.Length - 2];
                Array.Copy(payload, 2, res, 0, payload.Length - 2);
                SpecificResult.lrChConfigName = utf.GetString(res, 0, res.Length);
            }
            base.SetStateCompleted(ou);
        }

        public GetLrChConfigStrResult SpecificResult
        {
            get { return (GetLrChConfigStrResult)Result; }
        }
        
        protected override ActionResult CreateOperationResult()
        {
            return new GetLrChConfigStrResult();
        }
    }

    /// <summary>
    /// This class represents GetLrChConfigStrResult which is used to store the result of the GetLRChConfigStrOperation.
    /// This class stores the Config code and the Config name.
    /// </summary>
    public class GetLrChConfigStrResult : ActionResult
    {
        public byte lrChConfig { get; set; }
        public string lrChConfigName { get; set; }
    }
}
