/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using ZWave.BasicApplication.Enums;
using ZWave.Devices;

namespace ZWave.BasicApplication
{
    public abstract class ApiOperation : ActionBase
    {
        protected NetworkViewPoint _network;
        public CommandTypes[] SerialApiCommands { get; private set; }
        public SubstituteSettings SubstituteSettings { get; set; }

        public ApiOperation(bool isExclusive, CommandTypes[] serialApiCommands, bool isSequenceNumberRequired)
            : base(isExclusive)
        {
            SerialApiCommands = serialApiCommands;
            IsSequenceNumberRequired = isSequenceNumberRequired;
            SubstituteSettings = new SubstituteSettings();
        }

        public ApiOperation(bool isExclusive, CommandTypes serialApiCommand, bool isSequenceNumberRequired)
            : base(isExclusive)
        {
            SerialApiCommands = new CommandTypes[] { serialApiCommand };
            IsSequenceNumberRequired = isSequenceNumberRequired;
            SubstituteSettings = new SubstituteSettings();
        }
    }

    public class MessageOperation : ActionBase
    {
        private readonly ApiMessage _message;
        public MessageOperation(ApiMessage message)
            : base(false)
        {
            _message = message;
        }

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(SetStateCompleting, 0, _message));
        }

        protected override void CreateInstance()
        {
        }
    }
}
