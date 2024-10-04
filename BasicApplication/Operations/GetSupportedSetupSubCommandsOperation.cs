/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ZWave.BasicApplication.Enums;
using ZWave.Devices;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0x0B | 0x01
    /// ZW->HOST: RES | 0x0B | 0x01 | Z-Wave API Setup Supported Sub Commands flags | Extended Z-Wave API Setup Supported Sub Commands bitmask[1] | .. | Extended Z-Wave API Setup Supported Sub Commands bitmask [N]
    /// </summary>
    public class GetSupportedSetupSubCommandsOperation : SerialApiSetupOperation
    {
        private const byte SERIAL_API_SETUP_CMD_GET_SUPPORTED_COMMANDS = 0x01;

        public GetSupportedSetupSubCommandsOperation(NetworkViewPoint network)
            : base(SERIAL_API_SETUP_CMD_GET_SUPPORTED_COMMANDS)
        {
            _network = network;
        }

        protected override void SetStateCompleted(IActionUnit ou)
        {
            byte[] res = ((DataReceivedUnit)ou).DataFrame.Payload;
            SpecificResult.SetupSupportedSubCommandsFlag = res[1];
            byte funcIdx = 0;
            List<byte> extendedSubCommandIds = new List<byte>();
            var tmp = new BitArray(res.Skip(1).ToArray());
            _network.ExtendedSetupSupportedSubCommands = new BitArray(256);
            //BitArray ExtendedSetupSupportedSubCommands = new BitArray(256);
            for (int j = 2; j < res.Length; j++)
            {
                byte availabilityMask = res[j];
                for (byte bit = 0; bit < 8; bit++)
                {
                    funcIdx++;
                    if ((availabilityMask & (1 << bit)) > 0)
                    {
                        extendedSubCommandIds.Add(funcIdx);
                        _network.ExtendedSetupSupportedSubCommands[funcIdx] = true;
                        //ExtendedSetupSupportedSubCommands[funcIdx] = true;
                    }
                }
            }
            SpecificResult.ExtendedSetupSupportedSubCommands = extendedSubCommandIds.ToArray();
            base.SetStateCompleted(ou);
        }

        public GetSupportedSetupSubCommandsResult SpecificResult
        {
            get { return (GetSupportedSetupSubCommandsResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new GetSupportedSetupSubCommandsResult();
        }
    }

    public class GetSupportedSetupSubCommandsResult : ReturnValueResult
    {
        /// <summary>
        /// ? Z-Wave API Setup Supported Sub Commands ?
        /// </summary>
        public byte SetupSupportedSubCommandsFlag { get; set; }
        /// <summary>
        /// Extended Z-Wave API Setup Supported Sub Commands
        /// </summary>
        public byte[] ExtendedSetupSupportedSubCommands { get; set; }

    }
}
