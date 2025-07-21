/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.ZnifferApplication.Enums;

namespace ZWave.ZnifferApplication.Operations
{
    /// <summary>
    /// This class represents SetLRChConfigOperation which is used to set the LR channel config from the Serial Zniffer namely zwave_ncp_zniffer.
    /// This command might be not supported by the older versions of the Serial Zniffer.
    /// </summary>
    public class SetLRChConfigOperation : ActionBase
    {
        private const int TIMEOUT_MS = 300;

        public byte LRChannel { get; set; }

        public SetLRChConfigOperation(byte lrChannel) : base(true)
        {
            LRChannel = lrChannel;
        }

        public ZnifferApiMessage SetLRChConfigMessage { get; set; }

        public ZnifferApiHandler SetLRChConfigHandler { get; set; }

        /// <summary>
        /// CreateInstance method is used to create the SetLRChConfigMessage and SetLRChConfigHandler.
        /// The get command must be "0x06 0x01 0x0x", where the second byte is the length of the payload. The payload is the code to set for the lr channel.
        /// The answer must be started with the same command type, in this case with 0x06.
        /// </summary>
        protected override void CreateInstance()
        {
            SetLRChConfigMessage = new ZnifferApiMessage(CommandTypes.SetLRChConfig, new byte[] { 0x01, LRChannel });
            SetLRChConfigHandler = new ZnifferApiHandler(CommandTypes.SetLRChConfig);
        }

        /// <summary>
        ///  CreateWorkflow method is used to create the workflow for the SetLRChConfigOperation. This will send set LR channel config command to the Serial Zniffer.
        ///  For the answer, which will begins with the GetLRChConfigStrHandler, the SetStateCompleted method from this class will be called.
        /// </summary>
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, TIMEOUT_MS, SetLRChConfigMessage));
            ActionUnits.Add(new DataReceivedUnit(SetLRChConfigHandler, SetStateCompleted));
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ActionResult();
        }
    }
}
