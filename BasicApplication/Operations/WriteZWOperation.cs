/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace ZWave.BasicApplication.Operations
{
    public class WriteZWOperation : ActionBase
    {
        public byte[] Data { get; set; }
        public WriteZWOperation(byte[] data)
            : base(true)
        {
            Data = data;
        }

        CommandMessage message;
        protected override void CreateInstance()
        {
            message = new CommandMessage { Data = Data };
        }

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(SetStateCompleting, 0, message));
        }

        public override string AboutMe()
        {
            return string.Format("Data={0}", Data != null ? Data.GetHex() : "");
        }
    }
}
