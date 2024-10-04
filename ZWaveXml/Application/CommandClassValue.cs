/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;

namespace ZWave.Xml.Application
{
    [Serializable]
    public class CommandClassValue
    {
        public CommandClass CommandClassDefinition { get; set; }
        public CommandValue CommandValue { get; set; }
        /// <summary>
        /// Data excluding ClassId and CmdId
        /// </summary>
        public byte[] Payload { get; set; }
        /// <summary>
        /// Data with ClassId and CmdId
        /// </summary>
        public byte[] Data { get; set; }
    }
}
