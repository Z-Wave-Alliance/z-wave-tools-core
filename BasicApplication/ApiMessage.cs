/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;
using ZWave.Enums;

namespace ZWave.BasicApplication
{
    public class ApiMessage : CommandMessage
    {
        public ApiMessage(CommandTypes command, params byte[] inputParameters)
        {
            AddData((byte)FrameTypes.Request, (byte)command);
            AddData(inputParameters);
        }
    }

    public class ApiProgMessage : CommandMessage
    {
        public ApiProgMessage(params byte[] inputParameters)
        {
            AddData((byte)CommandTypes.CmdTestInterface, 0x00);
            AddData(inputParameters);
        }
    }
}
