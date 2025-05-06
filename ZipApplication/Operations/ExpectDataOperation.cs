/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Linq;
using ZWave.CommandClasses;
using System.Collections.Generic;

namespace ZWave.ZipApplication.Operations
{
    public class ExpectDataOperation : ZipApiOperation
    {
        private byte CmdClass { get; set; }
        private byte Cmd { get; set; }
        private int TimeoutMs { get; set; }
        private int HitCount { get; set; }

        public ExpectDataOperation(byte cmdClass, byte cmd, int timeoutMs, int hitCount)
            : this(cmdClass, cmd, timeoutMs)
        {
            HitCount = hitCount;
        }

        public ExpectDataOperation(byte cmdClass, byte cmd, int timeoutMs)
            : base(false)
        {
            CmdClass = cmdClass;
            Cmd = cmd;
            TimeoutMs = timeoutMs;
        }

        private ZipApiHandler expectReceived;
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, TimeoutMs));
            ActionUnits.Add(new DataReceivedUnit(expectReceived, OnReceived));
        }

        protected override void CreateInstance()
        {
            expectReceived = new ZipApiHandler(CmdClass, Cmd);
        }

        private void OnReceived(DataReceivedUnit ou)
        {
            SpecificResult.ReceivedData = ou.DataFrame.Payload;
            SpecificResult.ReceivedPacket = ou.DataFrame.Data;
            var data = ou.DataFrame.Data;
            if (data != null && data.Length > 2 && data[0] == COMMAND_CLASS_ZIP_V3.ID && data[1] == COMMAND_CLASS_ZIP_V3.COMMAND_ZIP_PACKET.ID)
            {
                COMMAND_CLASS_ZIP_V3.COMMAND_ZIP_PACKET packet = data;
            }
            if (HitCount > 0)
            {
                HitCount--;
                if (SpecificResult.ReceivedDataList == null)
                {
                    SpecificResult.ReceivedDataList = new List<byte[]>();
                }
                SpecificResult.ReceivedDataList.Add(ou.DataFrame.Data);
            }

            if (HitCount == 0)
            {
                base.SetStateCompleted(ou);
            }
        }

        public ExpectDataResult SpecificResult
        {
            get { return (ExpectDataResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ExpectDataResult();
        }
    }

    public class ExpectDataResult : ActionResult
    {
        public List<byte[]> ReceivedDataList { get; set; }
        public byte[] ReceivedData { get; set; }
        public COMMAND_CLASS_ZIP_V3.COMMAND_ZIP_PACKET ReceivedPacket { get; set; }
    }
}
