/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace ZWave.ProgrammerApplication
{
    public abstract class ProgrammerApiOperation : ActionBase
    {
        public ProgrammerApiOperation()
            : base(true)
        {
        }

        protected static Queue<Pair<byte, byte>> PrepareDataTrim(byte[] data, byte minAddress, byte maxAddress, out byte lastAddress)
        {
            Queue<Pair<byte, byte>> ret = new Queue<Pair<byte, byte>>();
            int ffCount = 0;
            lastAddress = 0x00;
            for (int i = minAddress; i < data.Length && i <= maxAddress; i++)
            {
                if (data[i] != 0xFF)
                {
                    for (int j = 0; j < ffCount; j++)
                    {
                        ret.Enqueue(new Pair<byte, byte>((byte)(i - ffCount + j), 0xFF));
                    }
                    lastAddress = (byte)i;
                    ret.Enqueue(new Pair<byte, byte>(lastAddress, data[i]));
                    ffCount = 0;
                }
                else
                {
                    if (ret.Count > 0)
                    {
                        ffCount++;
                    }
                }
            }
            return ret;
        }

        protected static Queue<Pair<byte, byte>> PrepareDataNotEmpty(byte[] data, byte minAddress, byte maxAddress, out byte lastAddress)
        {
            Queue<Pair<byte, byte>> ret = new Queue<Pair<byte, byte>>();
            lastAddress = 0x00;
            for (int i = minAddress; i < data.Length && i <= maxAddress; i++)
            {
                if (data[i] != 0xFF)
                {
                    lastAddress = (byte)(i);
                    ret.Enqueue(new Pair<byte, byte>(lastAddress, data[i]));
                }
            }
            return ret;
        }
    }
}
