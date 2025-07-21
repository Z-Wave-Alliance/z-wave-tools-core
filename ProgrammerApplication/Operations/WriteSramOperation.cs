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
    public class WriteSramOperation : ActionBase
    {
        private readonly int MinAddress;
        private readonly int MaxAddress;
        private int CurrentAddress;
        private int CurrentCommandIndex;
        private readonly int TotalCommandsCount;
        internal int CommandsCount { get; set; }

        private int CurrentIndex;
        private readonly int SourceIndex;
        internal byte[] Data { get; set; }
        public WriteSramOperation(byte[] data, int sourceIndex, int address, int length, int commandsCount)
            : base(true)
        {
            SourceIndex = sourceIndex;
            CurrentIndex = sourceIndex;
            Data = data;

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
            payload[0] = (byte)CommandTypes.WriteSRAM;
            payload[1] = (byte)(CurrentAddress >> 8);
            payload[2] = (byte)CurrentAddress;
            payload[3] = GetDataByIndex(CurrentIndex);

            if (CommandsCount > 1)
            {
                for (int i = 1; i < CommandsCount; i++)
                {
                    payload[4 * i + 0] = (byte)CommandTypes.ContinueWriteSRAM;
                    payload[4 * i + 1] = GetDataByIndex(3 * (i - 1) + CurrentIndex + 1);
                    payload[4 * i + 2] = GetDataByIndex(3 * (i - 1) + CurrentIndex + 2);
                    payload[4 * i + 3] = GetDataByIndex(3 * (i - 1) + CurrentIndex + 3);
                }
            }
            message = new CommandMessage();
            message.Data = payload;

            handler = new CommandHandler();
            handler.AddConditions(
                new ByteIndex((byte)CommandTypes.WriteSRAM),
                new ByteIndex((byte)(CurrentAddress >> 8)),
                new ByteIndex((byte)CurrentAddress),
                GetMaskDataByIndex(SourceIndex));

            handlerSpi = new CommandHandler();
            handlerSpi.AddConditions(
                new ByteIndex((byte)CommandTypes.WriteSRAM),
                new ByteIndex(0x04),
                new ByteIndex((byte)(CurrentAddress >> 8)),
                new ByteIndex((byte)CurrentAddress));
        }

        private void OnHandledSpi(DataReceivedUnit ou)
        {
            OnHandled(ou);
            handlerSpi.Mask[0] = handler.Mask[0];
            handlerSpi.Mask[1] = handler.Mask[0];
            handlerSpi.Mask[2] = handler.Mask[1];
            handlerSpi.Mask[3] = handler.Mask[2];
        }

        private void OnHandled(DataReceivedUnit ou)
        {
            CurrentCommandIndex++;
            if (ou.DataFrame.Payload[0] == (byte)CommandTypes.WriteSRAM)
            {
                CurrentAddress++;
                CurrentIndex++;
                handler.Mask[0] = new ByteIndex((byte)CommandTypes.ContinueWriteSRAM);
            }
            else
            {
                for (int i = 0; i < 3 && CurrentAddress <= MaxAddress; i++)
                {
                    CurrentAddress++;
                    CurrentIndex++;
                }
            }

            if (CurrentAddress > MaxAddress)
            {
                SetStateCompleted(ou);
                SpecificResult.WriteCount = CurrentAddress - MinAddress;
            }
            else
            {
                handler.Mask[1] = GetMaskDataByIndex(CurrentIndex + 0);
                handler.Mask[2] = GetMaskDataByIndex(CurrentIndex + 1);
                handler.Mask[3] = GetMaskDataByIndex(CurrentIndex + 2);

                if ((CurrentCommandIndex) % CommandsCount != 0)
                {
                    ou.SetNextActionItems();
                }
                else
                {
                    SpecificResult.WriteCount = CurrentAddress - MinAddress;
                    int cmdCount = CommandsCount;
                    if ((cmdCount > TotalCommandsCount - CurrentCommandIndex))
                        cmdCount = TotalCommandsCount - CurrentCommandIndex;

                    byte[] payload = new byte[cmdCount * 4];
                    for (int i = 0; i < cmdCount; i++)
                    {
                        payload[i * 4 + 0] = (byte)CommandTypes.ContinueWriteSRAM;
                        payload[i * 4 + 1] = GetDataByIndex(3 * i + CurrentIndex + 0);
                        payload[i * 4 + 2] = GetDataByIndex(3 * i + CurrentIndex + 1);
                        payload[i * 4 + 3] = GetDataByIndex(3 * i + CurrentIndex + 2);
                    }
                    ou.SetNextActionItems(new CommandMessage() { Data = payload });
                }
            }
        }

        private byte GetDataByIndex(int index)
        {
            if (Data.Length > index)
                return Data[index];
            else
                return 0;
        }

        private ByteIndex GetMaskDataByIndex(int index)
        {
            if (Data.Length > index)
                return new ByteIndex(Data[index]);
            else
                return ByteIndex.AnyValue;
        }

        public WriteSramResult SpecificResult
        {
            get { return (WriteSramResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new WriteSramResult();
        }
    }

    public class WriteSramResult : ActionResult
    {
        public int WriteCount { get; set; }
    }
}
