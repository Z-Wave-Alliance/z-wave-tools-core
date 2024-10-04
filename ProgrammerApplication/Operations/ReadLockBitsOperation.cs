/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZWave.ProgrammerApplication.Enums;
using Utils;

namespace ZWave.ProgrammerApplication.Operations
{
    public class ReadLockBitsOperation : ActionBase
    {
        private readonly byte MinAddress = 0x00;
        private readonly byte MaxAddress = 0x08;
        private byte CurrentAddress;

        private int CurrentCommandIndex;
        private readonly int TotalCommandsCount;

        internal int CommandsCount { get; set; }
        public ReadLockBitsOperation(int commandsCount)
            : base(true)
        {
            CurrentAddress = MinAddress;
            TotalCommandsCount = MaxAddress - CurrentAddress + 1;
            CurrentCommandIndex = 0;

            CommandsCount = commandsCount;
            if (CommandsCount < 1)
                CommandsCount = 1;
            else if (CommandsCount > TotalCommandsCount)
                CommandsCount = TotalCommandsCount;

            SpecificResult.Value = new byte[MaxAddress + 1];
        }

        CommandHandler handler;
        CommandHandler handlerSpi;
        CommandMessage message;
        protected override void CreateWorkflow()
        {
            if (TotalCommandsCount > 0)
            {
                ActionUnits.Add(new StartActionUnit(null, 2000, message));
                ActionUnits.Add(new DataReceivedUnit(handler, OnHandled, 2000, message));
                ActionUnits.Add(new DataReceivedUnit(handlerSpi, OnHandledSpi, 2000, message));
            }
            else
                ActionUnits.Add(new StartActionUnit(SetStateCompleted, 0));
        }

        protected override void CreateInstance()
        {
            byte[] payload = new byte[CommandsCount * 4];
            for (int i = 0; i < CommandsCount; i++)
            {
                payload[i * 4 + 0] = (byte)CommandTypes.ReadLockBits;
                payload[i * 4 + 1] = (byte)(CurrentAddress + i);
                payload[i * 4 + 2] = 0xFF;
                payload[i * 4 + 3] = 0xFF;
            }
            message = new CommandMessage();
            message.Data = payload;

            handler = new CommandHandler();
            handler.AddConditions(new ByteIndex((byte)CommandTypes.ReadLockBits), new ByteIndex(CurrentAddress), new ByteIndex(0xFF), ByteIndex.AnyValue);

            handlerSpi = new CommandHandler();
            handlerSpi.AddConditions(new ByteIndex((byte)CommandTypes.ReadLockBits), new ByteIndex(0xF1), new ByteIndex(CurrentAddress), ByteIndex.AnyValue);
        }

        private void OnHandledSpi(DataReceivedUnit ou)
        {
            OnHandled(ou);
            handlerSpi.Mask[2] = handler.Mask[1];

        }
        private void OnHandled(DataReceivedUnit ou)
        {
            CurrentCommandIndex++;
            SpecificResult.Value[CurrentAddress] = ou.DataFrame.Payload[3];
            if (CurrentAddress == MaxAddress)
                SetStateCompleted(ou);
            else
            {
                CurrentAddress++;
                if ((CurrentCommandIndex) % CommandsCount != 0)
                {
                    ou.SetNextActionItems();
                }
                else
                {
                    int cmdCount = CommandsCount;
                    if ((cmdCount > TotalCommandsCount - CurrentCommandIndex))
                        cmdCount = TotalCommandsCount - CurrentCommandIndex;

                    byte[] payload = new byte[cmdCount * 4];
                    for (int i = 0; i < cmdCount; i++)
                    {
                        payload[i * 4 + 0] = (byte)CommandTypes.ReadLockBits;
                        payload[i * 4 + 1] = (byte)(CurrentAddress + i);
                        payload[i * 4 + 2] = 0xFF;
                        payload[i * 4 + 3] = 0xFF;
                    }
                    ou.SetNextActionItems(new CommandMessage() { Data = payload });
                }

                handler.Mask[1] = new ByteIndex(CurrentAddress);
            }
        }

        public ReadLockBitsResult SpecificResult
        {
            get { return (ReadLockBitsResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ReadLockBitsResult();
        }
    }

    public class ReadLockBitsResult : ActionResult
    {
        public byte[] Value { get; set; }
    }
}
