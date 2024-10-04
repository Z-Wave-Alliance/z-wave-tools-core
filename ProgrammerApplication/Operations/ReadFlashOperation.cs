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
    public class ReadFlashOperation : ActionBase
    {
        public int InitialProgressValue { get; set; }
        private readonly int SectorSize = 0x800;
        private readonly byte MinSector;
        private readonly byte MaxSector;
        private readonly byte CurrentSector;
        private readonly int MinAddress;
        private readonly int MaxAddress;
        private int CurrentAddress;
        private int CurrentCommandIndex;
        private readonly int TotalCommandsCount;
        internal int CommandsCount { get; set; }
        internal Action<int, int> ProgressCallback { get; set; }

        public ReadFlashOperation(int address, int length, int commandsCount, Action<int, int> progressCallback)
            : base(true)
        {
            ProgressCallback = progressCallback;
            MinAddress = address;
            MinSector = (byte)(MinAddress / SectorSize);

            MaxAddress = address + length - 1;
            MaxSector = (byte)(MaxAddress / SectorSize);

            CurrentSector = MinSector;
            CurrentAddress = MinSector * SectorSize;

            TotalCommandsCount = (MaxSector - MinSector + 1) * (1 + (SectorSize - 1) / 3 + ((SectorSize - 1) % 3 > 0 ? 1 : 0));
            CurrentCommandIndex = 0;

            CommandsCount = commandsCount;
            if (CommandsCount < 1)
                CommandsCount = 1;
            else if (CommandsCount > TotalCommandsCount)
                CommandsCount = TotalCommandsCount;

            SpecificResult.Value = new byte[MaxAddress - MinAddress + 1];
        }

        public void AnnounceProgress(int currentAddress)
        {
            if (ProgressCallback != null && currentAddress - MinAddress >= 0)
            {
                ProgressCallback(currentAddress - MinAddress + InitialProgressValue, MaxAddress - MinAddress + 1 + InitialProgressValue);
            }
        }

        public override string AboutMe()
        {
            return Tools.FormatStr("MinAddress={0:X2}, MinSector={1}, MaxAddress={2:X2}, MaxSector={3}", MinAddress, MinSector, MaxAddress, MaxSector);
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
            payload[0] = (byte)CommandTypes.ReadFlash;
            payload[1] = CurrentSector;
            payload[2] = 0xFF;
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
            handler.AddConditions(new ByteIndex((byte)CommandTypes.ReadFlash), new ByteIndex(CurrentSector), new ByteIndex(0xFF), ByteIndex.AnyValue);

            handlerSpi = new CommandHandler();
            handlerSpi.AddConditions(new ByteIndex((byte)CommandTypes.ReadFlash), new ByteIndex(0x10), new ByteIndex(CurrentSector), ByteIndex.AnyValue);
        }

        int lastAnnouncedAddress = 0;
        private void OnHandled(DataReceivedUnit ou)
        {
            CurrentCommandIndex++;
            if (ou.DataFrame.Payload[0] == (byte)CommandTypes.ReadFlash)
            {
                if (CurrentAddress >= MinAddress)
                {
                    SpecificResult.Value[CurrentAddress - MinAddress] = ou.DataFrame.Payload[3];
                }
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
                    if (CurrentAddress >= MinAddress)
                    {
                        SpecificResult.Value[CurrentAddress - MinAddress] = ou.DataFrame.Payload[1 + i];
                    }
                    CurrentAddress++;
                }
            }

            if (CurrentAddress > MaxAddress)
            {
                AnnounceProgress(CurrentAddress);
                SpecificResult.ReadCount = CurrentAddress - MinAddress;
                SetStateCompleted(ou);
            }
            else
            {
                if (CurrentAddress - lastAnnouncedAddress > MaxAddress / 100)
                {
                    lastAnnouncedAddress = CurrentAddress;
                    AnnounceProgress(CurrentAddress);
                }

                if ((CurrentCommandIndex) % CommandsCount != 0)
                {
                    ou.SetNextActionItems();
                }
                else
                {
                    if (CurrentAddress >= MinAddress)
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

        public ReadFlashResult SpecificResult
        {
            get { return (ReadFlashResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ReadFlashResult();
        }
    }

    public class ReadFlashResult : ActionResult
    {
        public byte[] Value { get; set; }
        public int ReadCount { get; set; }
    }
}
