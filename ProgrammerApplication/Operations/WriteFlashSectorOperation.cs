/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;
using ZWave.ProgrammerApplication.Enums;

namespace ZWave.ProgrammerApplication.Operations
{
    public class WriteFlashSectorOperation : ActionBase
    {
        internal byte SectorNumber { get; set; }
        public WriteFlashSectorOperation(byte sectorNumber)
            : base(true)
        {
            SectorNumber = sectorNumber;
        }

        CommandHandler handler;
        CommandHandler handlerSpi;
        CommandMessage message;
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 2000, message));
            ActionUnits.Add(new DataReceivedUnit(handler, SetStateCompleted));
            ActionUnits.Add(new DataReceivedUnit(handlerSpi, SetStateCompleted));
        }

        protected override void CreateInstance()
        {
            message = new CommandMessage();
            message.AddData((byte)CommandTypes.WriteFlashSector, SectorNumber, 0xFF, 0xFF);

            handler = new CommandHandler();
            handler.AddConditions(new ByteIndex((byte)CommandTypes.WriteFlashSector), new ByteIndex(SectorNumber), new ByteIndex(0xFF), new ByteIndex(0xFF));

            handlerSpi = new CommandHandler();
            handlerSpi.AddConditions(new ByteIndex((byte)CommandTypes.WriteFlashSector), new ByteIndex(0x20), new ByteIndex(SectorNumber), new ByteIndex(0xFF));
        }
    }

    public class WriteFlashSectorOperationUni : ActionBase
    {
        internal byte SectorNumber { get; set; }
        public WriteFlashSectorOperationUni(byte sectorNumber)
            : base(true)
        {
            SectorNumber = sectorNumber;
        }

        CommandHandler handler;
        CommandMessage message0;
        CommandMessage message1;
        CommandMessage message2;
        CommandMessage message3;
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 2000, message0));
            ActionUnits.Add(new DataReceivedUnit(handler, OnHandler, 2000));
        }

        protected override void CreateInstance()
        {
            message0 = new CommandMessage();
            message0.AddData((byte)CommandTypes.WriteFlashSector);
            message1 = new CommandMessage();
            message1.AddData(SectorNumber);
            message2 = new CommandMessage();
            message2.AddData(0xFF);
            message3 = new CommandMessage();
            message3.AddData(0xFF);

            handler = new CommandHandler();
            handler.AddConditions(ByteIndex.AnyValue);
        }

        int counter = 0;
        private void OnHandler(DataReceivedUnit ou)
        {
            switch (counter)
            {
                case 0:
                    ou.SetNextActionItems(message1);
                    break;
                case 1:
                    ou.SetNextActionItems(message2);
                    break;
                case 2:
                    ou.SetNextActionItems(message3);
                    break;
                case 3:
                    SetStateCompleted(ou);
                    break;
                default:
                    SetStateCompleted(ou);
                    break;
            }
            counter++;
        }
    }
}
