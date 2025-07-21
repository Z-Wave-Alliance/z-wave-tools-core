/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using ZWave.ZnifferApplication.Enums;

namespace ZWave.ZnifferApplication.Operations
{
    /// <summary>
    /// This class represents GetLRRegionsOperation which is used to get the LR regions from the Serial Zniffer namely zwave_ncp_zniffer.
    /// This command might be not supported by the older versions of the Serial Zniffer.
    /// </summary>
    public class GetLRRegionsOperation : ActionBase
    {
        private const int TIMEOUT_MS = 300;
        public byte CurrentLRRegion { get; set; }
        public byte[] LRRegions { get; set; }

        public GetLRRegionsOperation() : base(true) { }
        public ZnifferApiMessage GetLRRegionsMessage { get; set; }
        public ZnifferApiHandler GetLRRegionsHandler { get; set; }

        /// <summary>
        /// CreateInstance method is used to create the GetLRRegionsMessage and GetLRRegionsHandler.
        /// The get command must be "0x08 0x00", where the second byte is the length of the payload.
        /// The answer must be started with the same command type, in this case with 0x08.
        /// </summary>
        protected override void CreateInstance()
        {
            GetLRRegionsMessage = new ZnifferApiMessage(CommandTypes.GetLRRegions, new byte[] { 0x00 });
            GetLRRegionsHandler = new ZnifferApiHandler(CommandTypes.GetLRRegions);
        }

        /// <summary>
        ///  CreateWorkflow method is used to create the workflow for the GetLRRegionsOperation. This will send get LR regions command to the Serial Zniffer.
        ///  For the answer, which will begins with the GetLRChConfigsHandler, the SetStateCompleted method from this class will be called.
        /// </summary>
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, TIMEOUT_MS, GetLRRegionsMessage));
            ActionUnits.Add(new DataReceivedUnit(GetLRRegionsHandler, SetStateCompleted));
        }

        /// <summary>
        /// SetStateCompleted method is used to set the state of the GetLRRegionsOperation to completed.
        /// This method will handle the incomming frame where the first byte is the current LR Region. The rest of the payload is the supported LR regions.
        /// Important: This command won't report current region in the firmware and it is differs from other command behaviours
        /// </summary>
        /// <param name="ou"></param>
        protected override void SetStateCompleted(IActionUnit ou)
        {
            byte[] payload = ((DataReceivedUnit)ou).DataFrame.Payload;
            if (payload != null && payload.Length > 0)
            {
                LRRegions = new byte[payload.Length];
                Array.Copy(payload, 0, LRRegions, 0, payload.Length);
            }
            base.SetStateCompleted(ou);
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ActionResult();
        }
    }
}
