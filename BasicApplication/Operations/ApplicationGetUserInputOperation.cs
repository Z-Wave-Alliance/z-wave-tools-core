/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class ApplicationGetUserInputOperation : ControlNApiOperation
    {
        public ApplicationGetUserInputOperation()
            : base(CommandTypes.CmdSerialApiSetup)
        {
            
        }

        protected override byte[] CreateInputParameters()
        {
            byte[] ret = new byte[2];
            ret[0] = 0x10; //SERIAL_API_SETUP_CMD_NETWORK_MANAGEMENT
            ret[1] = 0x02; //CMD_NETWORK_MANAGEMENT_CMD_INIF_GET_USER_INPUT 
            return ret;
        }
    }
}
