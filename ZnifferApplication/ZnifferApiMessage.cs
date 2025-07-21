/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZWave.ZnifferApplication.Enums;

namespace ZWave.ZnifferApplication
{
    public class ZnifferApiMessage : CommandMessage
    {
        /// <summary>
        /// This API message create will create a frame for the Zniffer application where the frame will be { 0x23, command, 0x00 },
        /// where the last byte is the length of the payload. In this case 0.
        /// </summary>
        /// <param name="command">Zniffer NCP command</param>
        public ZnifferApiMessage(CommandTypes command) 
        {
            AddData((byte)command);
            AddData(new byte[] { 0x00 });
        }

        /// <summary>
        /// This API message create will create a frame for the Zniffer application where the frame will be { 0x23, command, parameters },
        /// where the first byte of the inputParameters is the length of the payload.
        /// </summary>
        /// <param name="command">Zniffer NCP command</param>
        /// <param name="inputParameters">Zniffer NCP message payload</param>
        public ZnifferApiMessage(CommandTypes command, params byte[] inputParameters)
        {
            AddData((byte)command);
            AddData(inputParameters);
        }
    }
}
