/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZWave.ZipApplication.Operations;
using ZWave.CommandClasses;

namespace ZWave.ZipApplication.CommandClasses
{
    public class FirmwareUpdateMDSupport : ResponseDataOperation
    {
        public FirmwareUpdateMDSupport()
            : base(COMMAND_CLASS_FIRMWARE_UPDATE_MD.ID)
        {
        }
        public class SendCommand : ZipApiOperation
        {
            private readonly byte[] _command;
            private readonly bool _isNoAck = false;
            public SendCommand(byte[] command)
                : base(true)
            {
                _command = command;
                if (command != null && command.Length > 1)
                {
                    if (command[0] == COMMAND_CLASS_FIRMWARE_UPDATE_MD.ID)
                    {
                        if (command[1] != COMMAND_CLASS_FIRMWARE_UPDATE_MD_V5.FIRMWARE_UPDATE_MD_PREPARE_GET.ID &&
                            command[1] != COMMAND_CLASS_FIRMWARE_UPDATE_MD_V5.FIRMWARE_UPDATE_MD_GET.ID)
                        {
                            throw new InvalidOperationException("Command must be COMMAND_CLASS_FIRMWARE_UPDATE_MD");
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("Command Class must be COMMAND_CLASS_FIRMWARE_UPDATE_MD");
                    }
                }
                else
                {
                    throw new InvalidOperationException("Command parameter is missing");
                }
            }

            protected ZipApiMessage message;

            protected override void CreateWorkflow()
            {
                if (_command[1] == COMMAND_CLASS_FIRMWARE_UPDATE_MD_V5.FIRMWARE_UPDATE_MD_PREPARE_GET.ID)
                {
                    var cmd = (COMMAND_CLASS_FIRMWARE_UPDATE_MD_V5.FIRMWARE_UPDATE_MD_PREPARE_GET)_command;

                }
                else if (_command[1] == COMMAND_CLASS_FIRMWARE_UPDATE_MD_V5.FIRMWARE_UPDATE_MD_GET.ID)
                {
                    var cmd = (COMMAND_CLASS_FIRMWARE_UPDATE_MD_V5.FIRMWARE_UPDATE_MD_GET)_command;
                }
            }

            protected override void CreateInstance()
            {
                message = new ZipApiMessage(null, _command);
                message.SetSequenceNumber(SequenceNumber);
                message.IsNoAck = _isNoAck;
            }
        }
    }
}
