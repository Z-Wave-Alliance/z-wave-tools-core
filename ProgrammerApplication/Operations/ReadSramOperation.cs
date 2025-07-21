/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZWave.ProgrammerApplication.Enums;
using Utils;

namespace ZWave.ProgrammerApplication.Operations
{
    public class ReadSramOperation : ActionBase
    {
        private readonly int MinAddress;
        private readonly int MaxAddress;
        private int CurrentAddress;
        private int CurrentCommandIndex;
        private readonly int TotalCommandsCount;
        internal int CommandsCount { get; set; }

        public ReadSramOperation(int address, int length, int commandsCount)
            : base(true)
        {
            MinAddress = address;
            MaxAddress = address + length - 1;
            CurrentAddress = MinAddress;

            TotalCommandsCount = 1 + (MaxAddress - CurrentAddress) / 3 + ((MaxAddress - CurrentAddress) % 3 > 0 ? 1 : 0);
            CurrentCommandIndex = 0;

            CommandsCount = commandsCount;
            if (CommandsCount < 1)
                CommandsCount = 1;
            else if (CommandsCount > TotalCommandsCount)
                CommandsCount = TotalCommandsCount;

            SpecificResult.Value = new byte[MaxAddress - MinAddress + 1];
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
                ActionUnits.Add(new DataReceivedUnit(handlerSpi, OnHandled, 2000, message));
            }
            else
                ActionUnits.Add(new StartActionUnit(SetStateCompleted, 0));
        }

        protected override void CreateInstance()
        {
            byte[] payload = new byte[CommandsCount * 4];
            payload[0] = (byte)CommandTypes.ReadSRAM;
            payload[1] = (byte)(CurrentAddress >> 8);
            payload[2] = (byte)CurrentAddress;
            payload[3] = 0xFF;

            if (CommandsCount > 1)
            {
                for (int i = 1; i < CommandsCount; i++)
                {
                    payload[i * 4 + 0] = (byte)CommandTypes.ContinueRead;
                    payload[i * 4 + 1] = 0x00;
                    payload[i * 4 + 2] = 0x00;
                    payload[i * 4 + 3] = 0x00;
                }
            }
            message = new CommandMessage();
            message.Data = payload;

            handler = new CommandHandler();
            handler.AddConditions(new ByteIndex((byte)CommandTypes.ReadSRAM), new ByteIndex((byte)(CurrentAddress >> 8)), new ByteIndex((byte)CurrentAddress), ByteIndex.AnyValue);

            handlerSpi = new CommandHandler();
            handlerSpi.AddConditions(new ByteIndex((byte)CommandTypes.ReadSRAM), new ByteIndex(0x06), new ByteIndex((byte)(CurrentAddress >> 8)), ByteIndex.AnyValue);
        }

        private void OnHandled(DataReceivedUnit ou)
        {
            CurrentCommandIndex++;
            if (ou.DataFrame.Payload[0] == (byte)CommandTypes.ReadSRAM)
            {
                SpecificResult.Value[CurrentAddress - MinAddress] = ou.DataFrame.Payload[3];
                CurrentAddress++;
                handler.Mask[0] = new ByteIndex((byte)CommandTypes.ContinueRead);
                handler.Mask[1] = ByteIndex.AnyValue;
                handler.Mask[2] = ByteIndex.AnyValue;
                handler.Mask[3] = ByteIndex.AnyValue;
            }
            else
            {
                for (int i = 0; i < 3 && CurrentAddress <= MaxAddress; i++)
                {
                    SpecificResult.Value[CurrentAddress - MinAddress] = ou.DataFrame.Payload[1 + i];
                    CurrentAddress++;
                }
            }

            if (CurrentAddress > MaxAddress)
            {
                SetStateCompleted(ou);
                SpecificResult.ReadCount = CurrentAddress - MinAddress;
            }
            else
            {
                if ((CurrentCommandIndex) % CommandsCount != 0)
                {
                    ou.SetNextActionItems();
                }
                else
                {
                    SpecificResult.ReadCount = CurrentAddress - MinAddress;
                    int cmdCount = CommandsCount;
                    if ((cmdCount > TotalCommandsCount - CurrentCommandIndex))
                        cmdCount = TotalCommandsCount - CurrentCommandIndex;

                    byte[] payload = new byte[cmdCount * 4];
                    for (int i = 0; i < cmdCount; i++)
                    {
                        payload[i * 4 + 0] = (byte)CommandTypes.ContinueRead;
                        payload[i * 4 + 1] = 0x00;
                        payload[i * 4 + 2] = 0x00;
                        payload[i * 4 + 3] = 0x00;
                    }
                    ou.SetNextActionItems(new CommandMessage() { Data = payload });
                }
            }
        }

        public ReadSramResult SpecificResult
        {
            get { return (ReadSramResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ReadSramResult();
        }
    }

    public class ReadSramResult : ActionResult
    {
        public byte[] Value { get; set; }
        public int ReadCount { get; set; }
    }
}
