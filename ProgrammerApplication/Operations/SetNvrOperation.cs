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
    public class SetNvrOperation : ProgrammerApiOperation
    {
        internal byte MinAddress = 0x09;
        internal byte MaxAddress = 0xFF;

        internal Queue<byte> ExpectedAddresses;
        internal int CommandsCount { get; set; }
        internal Queue<Pair<byte, byte>> Data { get; set; }
        public SetNvrOperation(byte[] data, int commandsCount)
        {
            Data = PrepareDataNotEmpty(data, MinAddress, MaxAddress, out MaxAddress);
            CommandsCount = commandsCount;
            if (CommandsCount < 1)
                CommandsCount = 1;
            else if (CommandsCount > Data.Count)
                CommandsCount = Data.Count;
        }

        CommandHandler handler;
        CommandHandler handlerSpi;
        CommandMessage message;
        protected override void CreateWorkflow()
        {
            if (CommandsCount > 0)
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
            if (CommandsCount > 0)
            {
                ExpectedAddresses = new Queue<byte>();
                byte[] payload = new byte[CommandsCount * 4];
                for (int i = 0; i < CommandsCount; i++)
                {
                    Pair<byte, byte> b = Data.Dequeue();
                    payload[i * 4 + 0] = (byte)CommandTypes.SetNvrByte;
                    payload[i * 4 + 1] = 0x00;
                    payload[i * 4 + 2] = b.First;
                    payload[i * 4 + 3] = b.Second;
                    ExpectedAddresses.Enqueue(b.First);
                }
                message = new CommandMessage();
                message.Data = payload;

                byte expectedAddress = ExpectedAddresses.Dequeue();
                handler = new CommandHandler();
                handler.AddConditions(new ByteIndex((byte)CommandTypes.SetNvrByte), new ByteIndex(0x00), new ByteIndex(expectedAddress), ByteIndex.AnyValue);

                handlerSpi = new CommandHandler();
                handlerSpi.AddConditions(new ByteIndex((byte)CommandTypes.SetNvrByte), new ByteIndex(0xFE), new ByteIndex(0x00), new ByteIndex(expectedAddress));
            }
        }

        private void OnHandledSpi(DataReceivedUnit ou)
        {
            OnHandled(ou);
            handlerSpi.Mask[3] = handler.Mask[2];
        }

        private void OnHandled(DataReceivedUnit ou)
        {
            if (Data.Count == 0)
                SetStateCompleted(ou);
            else
            {
                if (ExpectedAddresses.Count > 0)
                {
                    ou.SetNextActionItems();
                }
                else
                {
                    if ((CommandsCount > Data.Count))
                        CommandsCount = Data.Count;

                    ExpectedAddresses = new Queue<byte>();
                    byte[] payload = new byte[CommandsCount * 4];
                    for (int i = 0; i < CommandsCount; i++)
                    {
                        Pair<byte, byte> b = Data.Dequeue();
                        payload[i * 4 + 0] = (byte)CommandTypes.SetNvrByte;
                        payload[i * 4 + 1] = 0x00;
                        payload[i * 4 + 2] = b.First;
                        payload[i * 4 + 3] = b.Second;
                        ExpectedAddresses.Enqueue(b.First);
                    }
                    ou.SetNextActionItems(new CommandMessage() { Data = payload });
                }
                handler.Mask[2] = new ByteIndex(ExpectedAddresses.Dequeue());
            }
        }
    }
}
