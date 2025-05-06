/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Collections.Generic;
using Utils;

namespace ZWave.BasicApplication.Operations
{
    public class ExpectZWOperation : ActionBase
    {
        public int TimeoutMs { get; set; }
        public IEnumerable<ByteIndex[]> Masks { get; set; }
        public ExpectZWOperation(IEnumerable<ByteIndex[]> masks, int timeoutMs)
            : base(false)
        {
            Masks = masks;
            TimeoutMs = timeoutMs;
        }

        List<CommandHandler> handlers;
        protected override void CreateInstance()
        {
            handlers = new List<CommandHandler>();
            foreach (var mask in Masks)
            {
                handlers.Add(new CommandHandler { Mask = mask });
            }
        }

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, TimeoutMs));
            foreach (var handler in handlers)
            {
                ActionUnits.Add(new DataReceivedUnit(handler, OnReceived));
            }
        }

        private void OnReceived(DataReceivedUnit ou)
        {
            SpecificResult.Data = ou.DataFrame.Data;
            SetStateCompleted(ou);
        }

        public override string AboutMe()
        {
            return string.Format("Data={0}", SpecificResult.Data != null ? SpecificResult.Data.GetHex() : "");
        }

        public ExpectZWResult SpecificResult
        {
            get { return (ExpectZWResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ExpectZWResult();
        }
    }

    public class ExpectZWResult : ActionResult
    {
        public byte[] Data { get; set; }
    }
}
