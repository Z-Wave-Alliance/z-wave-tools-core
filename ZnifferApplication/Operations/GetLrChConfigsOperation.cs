/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Linq;
using ZWave.ZnifferApplication.Enums;

namespace ZWave.ZnifferApplication.Operations
{
    /// <summary>
    /// This class represents GetLRChConfigsOperation which is used to get the LR channel configurations from the Serial Zniffer namely zwave_ncp_zniffer.
    /// This command might be not supported by the older versions of the Serial Zniffer.
    /// </summary>
    public class GetLRChConfigsOperation : ActionBase
    {
        private const int TIMEOUT_MS = 300;

        public byte CurrentLRConfig { get; set; }

        public byte[] LRConfigs { get; set; }

        public GetLRChConfigsOperation() : base(true) { }

        public ZnifferApiMessage GetLRChConfigsMessage { get; set; }

        public ZnifferApiHandler GetLRChConfigsHandler { get; set; }

        /// <summary>
        /// CreateInstance method is used to create the GetLRChConfigsMessage and GetLRChConfigsHandler.
        /// The get command must be "0x07 0x00", where the second byte is the length of the payload.
        /// The answer must be started with the same command type, in this case with 0x07.
        /// </summary>
        protected override void CreateInstance()
        {
            GetLRChConfigsMessage = new ZnifferApiMessage(CommandTypes.GetLRChConfigs);
            GetLRChConfigsHandler = new ZnifferApiHandler(CommandTypes.GetLRChConfigs);
        }

        /// <summary>
        ///  CreateWorkflow method is used to create the workflow for the GetLRChConfigsOperation. This will send get LR channel configurations command to the Serial Zniffer.
        ///  For the answer, which will begins with the GetLRChConfigsHandler, the SetStateCompleted method from this class will be called.
        /// </summary>
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, TIMEOUT_MS, GetLRChConfigsMessage));
            ActionUnits.Add(new DataReceivedUnit(GetLRChConfigsHandler, SetStateCompleted));
        }

        /// <summary>
        /// SetStateCompleted method is used to set the state of the GetLRChConfigsOperation to completed.
        /// Current overwrites the CurrentLRConfig and LRConfigs properties with the values from the payload of the answer.
        /// The payload should look like CurrentLRConfig + LRConfigCH1 ... LR.
        /// </summary>
        /// <param name="ou"></param>
        protected override void SetStateCompleted(IActionUnit ou)
        {
            byte[] payload = ((DataReceivedUnit)ou).DataFrame.Payload;
            if (payload != null && payload.Length > 0)
            {
                CurrentLRConfig = payload.First();
                // Skip first element because it is the current LR config
                int lrConfigLength = payload.Length - 1;
                LRConfigs = new byte[lrConfigLength];
                // Skip first element
                Array.Copy(payload, 1, LRConfigs, 0, lrConfigLength);
            }
            base.SetStateCompleted(ou);
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ActionResult();
        }
    }
}
