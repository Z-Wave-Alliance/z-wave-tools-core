/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class ApplicationUserInputOperation : ControlNApiOperation
    {
        private byte bUserInputIdent1 { get; set; }
        private byte bUserInputIdent2 { get; set; }
        private byte bUserInputLocalRes1 { get; set; }
        private byte bUserInputLocalRes2 { get; set; }

        public ApplicationUserInputOperation(byte[] UserInputId, byte[] UserInputLocal)
            : base(CommandTypes.CmdSerialApiSetup)
        {
            bUserInputIdent1 = UserInputId[0];
            bUserInputIdent2 = UserInputId[1];
            bUserInputLocalRes1 = UserInputLocal[0];
            bUserInputLocalRes2 = UserInputLocal[1];
        }

        protected override byte[] CreateInputParameters()
        {
            byte[] ret = new byte[6];
            ret[0] = 0x10; //SERIAL_API_SETUP_CMD_NETWORK_MANAGEMENT
            ret[1] = 0x01; //CMD_NETWORK_MANAGEMENT_CMD_INIF_SET_USER_INPUT 
            ret[2] = bUserInputIdent1;
            ret[3] = bUserInputIdent2;
            ret[4] = bUserInputLocalRes1;
            ret[5] = bUserInputLocalRes2;
            return ret;
        }
    }
}
